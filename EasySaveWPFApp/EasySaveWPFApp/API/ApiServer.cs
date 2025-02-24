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
                                // Lire le body JSON
                                using var reader = new StreamReader(context.Request.Body);
                                var body = await reader.ReadToEndAsync();

                                // Désérialisation en objet anonyme
                                // Répond avec les données reçues
                                context.Response.ContentType = "application/json";
                                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                                {
                                    status = ModifySaveTaskAPI(body)
                                }));
                            }
                            catch (Exception ex)
                            {
                                context.Response.StatusCode = 400;
                                await context.Response.WriteAsync($"Erreur: {ex.Message}");
                            }
                        }
                        else if (context.Request.Path == "/api/deletesavetask" && context.Request.Method == "POST")
                        {
                            try
                            {
                                // Lire le body JSON
                                using var reader = new StreamReader(context.Request.Body);
                                var body = await reader.ReadToEndAsync();

                                List<string> content = JsonSerializer.Deserialize<List<string>>(body);

                                context.Response.ContentType = "application/json";
                                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                                {
                                    status = DeleteSaveTaskAPI(content)
                                }));
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

        public string DeleteSaveTaskAPI(List<string> names)
        {
            var result = new Dictionary<string, bool>();

            foreach (var name in names)
            {
                bool isSuccessful = saveTaskManager.RemoveSaveTask(name);
                result[name] = isSuccessful; // Ajout de l'état de la tâche
            }

            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }

    }
}