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
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using System.Xml;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System.Reflection.Metadata;


namespace EasySaveWPFApp.Api
{
    public class ApiServer
    {
        private IWebHost _host;

        internal SaveTaskManager saveTaskManager;
        internal SaveTaskViewModel saveTaskViewModel;
        internal ApiServer(SaveTaskViewModel saveTaskViewModel, SaveTaskManager saveTaskManager)
        {
            this.saveTaskManager = saveTaskManager;
            this.saveTaskViewModel = saveTaskViewModel;
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
                    app.Run(async context =>
                    {
                        if (context.Request.Path == "/api/getsavetasks" && context.Request.Method == "PUT")
                        {
                            await context.Response.WriteAsync(GetSaveTasksToString());
                        }
                        else if (context.Request.Path == "/api/sendmodification" && context.Request.Method == "POST")
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
                        else if (context.Request.Path == "/api/deletesavetasks" && context.Request.Method == "POST")
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
                    case "Name":
                        return saveTaskViewModel.ModifySaveTaskName(data.GetProperty("name").GetString(), data.GetProperty("newvalue").GetString());
                    case "TargetPath":
                        return saveTaskViewModel.ModifySaveTaskTargetPath(data.GetProperty("name").GetString(), data.GetProperty("newvalue").GetString());
                    case "SourcePath":
                        return saveTaskViewModel.ModifySaveTaskSourcePath(data.GetProperty("name").GetString(), data.GetProperty("newvalue").GetString());
                    case "Type":
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