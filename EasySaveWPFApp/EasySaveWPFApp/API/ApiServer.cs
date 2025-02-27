using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using EasySaveWPFApp.Model;
using EasySaveWPFApp.ViewModel;
using EasySaveWPFApp.Utilities;
using Microsoft.AspNetCore.WebSockets;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using System.Xml;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System.Net.WebSockets;
using System.Reflection.Metadata;
using System.Collections.Concurrent;
using EasySaveWPFApp.ViewModel;
using System.Text;
using System.Text.Json.Serialization;
using System.Diagnostics;


namespace EasySaveWPFApp.Api
{

    public class WebSocketMessage
    {
        [JsonPropertyName("Action")] // Correspondance exacte avec la clé JSON
        public string Action { get; set; }

        [JsonPropertyName("Names")]
        public List<string> Names { get; set; }
    }
    public class ApiServer
    {
        private IWebHost _host;
        private ConcurrentBag<WebSocket> _webSockets = new ConcurrentBag<WebSocket>();

        internal SaveTaskManager saveTaskManager;
        internal SaveTaskViewModel saveTaskViewModel;   
        internal ApiServer(SaveTaskViewModel saveTaskViewModel, SaveTaskManager saveTaskManager)
        {
            this.saveTaskManager = saveTaskManager;
            this.saveTaskViewModel = saveTaskViewModel;
        }

        internal void bindSaveTaskCallBack(string saveTaskName, WebSocket webSocket)
        {
            saveTaskViewModel.BindSaveTasks.FirstOrDefault((task) => task.name == saveTaskName).SaveTaskProgressPercentageChangedCallback = async (newValue) =>
            {
                Trace.WriteLine($"Nouvelle valeur détectée dans un autre thread {newValue}");
                // Création du message JSON à envoyer
                var progressMessage = new
                {
                    Action = "ProgressUpdate",  
                    TaskName = saveTaskName,
                    Progress = newValue,
                };
                sendActionMessage(webSocket, progressMessage);
            };
            saveTaskViewModel.BindSaveTasks.FirstOrDefault((task) => task.name == saveTaskName).SaveTaskProgressStateChangedCallback = async (newValue) =>
            {
                Trace.WriteLine($"Nouvelle valeur détectée dans un autre thread {newValue}");
                // Création du message JSON à envoyer
                var progressMessage = new
                {
                    Action = "StateUpdate",
                    TaskName = saveTaskName,
                    State = newValue,
                };
                sendActionMessage(webSocket, progressMessage);
            };
        }

        internal async Task sendMessageStartSaveTask(WebSocket webSocket, List<string> saveTaskNames)
        {
            var startSaveTasks = new
            {
                Action = "StartSaveTasks",
                SaveTasks = JsonManager.SerializeSaveTasksAPI(saveTaskViewModel.GetSaveTasksByNames(saveTaskNames))
            };

            string jsonMessage = JsonSerializer.Serialize(startSaveTasks);
            var bytesMessage = Encoding.UTF8.GetBytes(jsonMessage);
            var bufferMessage = new ArraySegment<byte>(bytesMessage);

            // Envoi du message WebSocket
            if (webSocket.State == WebSocketState.Open)
            {
                await webSocket.SendAsync(bufferMessage, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        internal async Task sendActionMessage(WebSocket webSocket, object Action)
        {
            string jsonMessage = JsonSerializer.Serialize(Action);
            var bytesMessage = Encoding.UTF8.GetBytes(jsonMessage);
            var bufferMessage = new ArraySegment<byte>(bytesMessage);

            // Envoi du message WebSocket
            if (webSocket.State == WebSocketState.Open)
            {
                await webSocket.SendAsync(bufferMessage, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
        private async Task HandleWebSocketConnection(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            try
            {
                WebSocketReceiveResult result;
                while(webSocket.State == WebSocketState.Open)
                {
                        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    // Vérifier si le client demande la fermeture
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                        break;
                    }

                    // Traitement du message reçu
                    var receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    try
                    {
                        WebSocketMessage? webSocketMessage = JsonSerializer.Deserialize<WebSocketMessage>(receivedMessage);
                        if (webSocketMessage.Action == "Start")
                        {
                            await sendMessageStartSaveTask(webSocket, webSocketMessage.Names);//start de la saveTask
                            foreach (string saveTaskName in webSocketMessage.Names)
                            {
                                bindSaveTaskCallBack(saveTaskName, webSocket);
                                saveTaskViewModel.ExecuteSaveTaskAsync(saveTaskName);
                            }
                        }
                        else if(webSocketMessage.Action == "Pause")
                        {
                            foreach (string saveTaskName in webSocketMessage.Names)
                            {
                                saveTaskViewModel.PauseSaveTaskByName(saveTaskName);
                            }
                        }
                        else if (webSocketMessage.Action == "Play")
                        {
                            foreach (string saveTaskName in webSocketMessage.Names)
                            {
                                saveTaskViewModel.PlaySaveTaskByName(saveTaskName);
                            }
                        }
                        else if (webSocketMessage.Action == "Stop")
                        {
                            foreach (string saveTaskName in webSocketMessage.Names)
                            {
                                saveTaskViewModel.StopSaveTaskByName(saveTaskName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    Console.WriteLine($"Message reçu : {receivedMessage}");

                    // Exemple : Réponse vers le client
                    var responseMessage = $"Serveur a reçu : {receivedMessage}";
                    var responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
                }
            }
            catch (WebSocketException wsEx)
            {
                Console.WriteLine($"Erreur WebSocket : {wsEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur générale : {ex.Message}");
            }
            finally
            {
                if (webSocket != null)
                    webSocket.Dispose();
            }
        }

        public void Start()
        {
            _host = new WebHostBuilder()
                .UseKestrel(options =>
                {
                    options.ListenLocalhost(5000); // L'API tourne sur http://localhost:5000
                })
                .Configure(app =>
                {
                    app.UseWebSockets();
                    app.Use(async (context, next) =>
                    {
                        //  Ajouter les en-têtes CORS à toutes les réponses
                        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                        context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                        context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");

                        // Répondre immédiatement aux requêtes OPTIONS pour éviter le blocage CORS
                        if (context.Request.Method == "OPTIONS")
                        {
                            context.Response.StatusCode = 200; // Répondre avec un statut OK
                            await context.Response.WriteAsync(""); // Répondre avec un corps vide
                            return; 
                        }

                        await next(); // Continuer vers les autres middlewares
                    });
                    app.Use(async (context, next) => 
                    {
                        if (context.Request.Path == "/api/ws")
                        {
                            if (context.WebSockets.IsWebSocketRequest)
                            {
                                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                                _webSockets.Add(webSocket); // Sauvegarde du websocket

                                await HandleWebSocketConnection(webSocket);
                            }
                            else
                            {
                                context.Response.StatusCode = 400;
                            }
                        }
                        else
                        {
                            await next();
                        }
                    });

                    app.Run(async context =>
                    {

                        if (context.Request.Path == "/api/getsavetasks" && context.Request.Method == "GET")
                        {
                            await context.Response.WriteAsync(GetSaveTasksToString());
                        }
                        else if (context.Request.Path == "/api/sendmodification" && context.Request.Method == "PUT")
                        {
                            try
                            {
                                await SendResponse(context, new { status = ModifySaveTaskAPI(await ReadJsonRequestBody(context)) });
                            }
                            catch (Exception ex)
                            {
                                context.Response.StatusCode = 400;
                                await context.Response.WriteAsync($"Erreur: {ex.Message}");
                            }
                        }
                        else if (context.Request.Path == "/api/deletesavetasks" && context.Request.Method == "DELETE")
                        {
                            try
                            {
                                await SendResponse(context, DeleteSaveTasksAPI(JsonSerializer.Deserialize<List<string>>(await ReadJsonRequestBody(context))));
                            }
                            catch (Exception ex)
                            {
                                context.Response.StatusCode = 400;
                                await context.Response.WriteAsync($"Erreur: {ex.Message}");
                            }
                        }
                        else if (context.Request.Path == "/api/addsavetask" && context.Request.Method == "POST")
                        {
                            try
                            {
                                await SendResponse(context, new { status = AddSaveTaskAPI(await ReadJsonRequestBody(context)) });
                            }
                            catch (Exception ex)
                            {
                                context.Response.StatusCode = 400;
                                await context.Response.WriteAsync($"Erreur: {ex.Message}");
                            }
                        }
                        else
                        {
                            context.Response.StatusCode = 404;
                        }
                    });
                })
                .Build();

            Task.Run(() => _host.Run());
        }

        public async Task StopAsync()
        {
            if (_host != null)
            {
                await _host.StopAsync();
            }
        }

        public async Task<dynamic> ReadJsonRequestBody(HttpContext context)
        {
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            return body;
        }

        public async Task SendResponse<T>(HttpContext context, T content)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            await context.Response.WriteAsync(JsonSerializer.Serialize(content, new JsonSerializerOptions { WriteIndented = true }));
        }

        public string GetSaveTasksToString()
        {
            List<SaveTask> tasks = saveTaskManager.GetSaveTasksClone();
            string saveTasks = "";
            saveTasks = JsonManager.SerializeSaveTasksAPI(tasks);
            return saveTasks;
        }



        public bool ModifySaveTaskAPI(string body)
        {
            // Désérialisation en objet dynamique
            var data = JsonSerializer.Deserialize<JsonElement>(body);

            // Accéder à la propriété "field"
            if (data.TryGetProperty("field", out JsonElement fieldElement))
            {
                string field = fieldElement.GetString();

                switch (field)
                {
                    case null:
                        return false;
                    case "name":
                        return saveTaskViewModel.ModifySaveTaskName(data.GetProperty("name").GetString(), data.GetProperty("newvalue").GetString());
                    case "targetPath":
                        return saveTaskViewModel.ModifySaveTaskTargetPath(data.GetProperty("name").GetString(), data.GetProperty("newvalue").GetString());
                    case "sourcePath":
                        return saveTaskViewModel.ModifySaveTaskSourcePath(data.GetProperty("name").GetString(), data.GetProperty("newvalue").GetString());
                    case "type":
                        return saveTaskViewModel.ModifySaveTaskType(data.GetProperty("name").GetString(), ESaveTaskTypesExtension.ToESaveTaskTypes(data.GetProperty("newvalue").GetString()));
                }
            }

            return false;
        }

        public string DeleteSaveTasksAPI(List<string> names)
        {
            var result = new Dictionary<string, bool>();

            foreach (var name in names)
            {
                bool isSuccessful = saveTaskManager.RemoveSaveTask(name);
                result[name] = isSuccessful; // Ajout de l'état de la tâche
            }

            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }

        public bool AddSaveTaskAPI(string saveTask)
        {
            var data = JsonSerializer.Deserialize<JsonElement>(saveTask);
            bool isSucessful = saveTaskViewModel.CreateSaveTask(data.GetProperty("name").GetString(), data.GetProperty("sourcePath").GetString(), data.GetProperty("targetPath").GetString(), ESaveTaskTypesExtension.ToESaveTaskTypes(data.GetProperty("type").GetString()));
            return isSucessful;
        }
}
}