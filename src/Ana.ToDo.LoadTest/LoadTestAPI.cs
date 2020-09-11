using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Ana.ToDo.FunctionApp;
using System.Net.Http;
using System.Text;

namespace Ana.ToDo.LoadTest
{
    public class LoadTestAPI
    {
        private static HttpClient client;

        public LoadTestAPI()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("https://apim-todoapp-test.azure-api.net/")
            };
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("Ocp-Apim-Subscription-Key"));
            client.DefaultRequestHeaders.Add("Ocp-Apim-Trace", "true");
        }
        
        [FunctionName("PostMultipleItems"), NoAutomaticTrigger]
        public static async Task PostMultipleItems(int number)
        {
            var toDoItem = new ToDoItem()
            {
                Name = "new errand",
                IsComplete = false
            };

            var toDo = JsonConvert.SerializeObject(toDoItem);

            for (int i=0; i<=number; i++)
            {
                await client.PostAsync("toDoItems", new StringContent(toDo, Encoding.UTF8, "application/json"));
            }
        }
    }
}