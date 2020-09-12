using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Ana.ToDo.FunctionApp;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Ana.ToDo.LoadTest
{
    public class LoadTestAPI
    {
        private static HttpClient client;

        static LoadTestAPI()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("https://apim-todoapp-test.azure-api.net/")
            };
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("Ocp-Apim-Subscription-Key"));
            client.DefaultRequestHeaders.Add("Ocp-Apim-Trace", "true");
        }
        
        [FunctionName("PostMultipleItems"), NoAutomaticTrigger]
        public static async Task<IActionResult> PostMultipleItems(string numberOfRequests, ILogger log)
        {
            log.LogInformation("C# Manual trigger function processed a PostMultipleItems request.");
            var toDoItem = new ToDoItem()
            {
                Name = "new errand",
                IsComplete = false
            };

            var toDo = JsonConvert.SerializeObject(toDoItem);
            int number;

            if(!int.TryParse(numberOfRequests, out number)) 
            {
                log.LogInformation("Number format is invalid");
                return new BadRequestObjectResult("Number format is invalid");
            }
            
            for (int i=0; i<=number; i++)
            {
                await client.PostAsync("toDoItems", new StringContent(toDo, Encoding.UTF8, "application/json"));
            }

            log.LogInformation("ToDo Items were created");
            return new CreatedResult("ToDo Items were created", new {});
        }
    }
}