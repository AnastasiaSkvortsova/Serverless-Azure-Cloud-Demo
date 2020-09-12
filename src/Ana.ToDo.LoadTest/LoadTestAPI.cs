using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Ana.ToDo.FunctionApp;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

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
        public static async Task PostMultipleItems(string input, ILogger log)
        {
            log.LogInformation("C# Manual trigger function processed a PostMultipleItems request.");
            var toDoItem = new ToDoItem()
            {
                Name = "new errand",
                IsComplete = false
            };
            var toDo = JsonConvert.SerializeObject(toDoItem);

            log.LogInformation(input);
            //var parsedNumber = JObject.Parse(numberOfRequests)["numberOfRequests"].ToObject<string>();
            int number;

            if(!int.TryParse(input, out number)) 
            {
                log.LogError($"Number format is invalid. Input: {input}");
                return;
            }
            
            var tasks = new Task[number];
            for (int i=0; i<number; i++)
            {
                tasks[i] = client.PostAsync("toDoItems", new StringContent(toDo, Encoding.UTF8, "application/json"));
            }
            await Task.WhenAll(tasks);

            log.LogInformation("ToDo Items were created");
        }
    }
}