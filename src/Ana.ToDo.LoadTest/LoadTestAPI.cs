using System;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Azure.WebJobs;
using Ana.ToDo.FunctionApp;

namespace Ana.ToDo.LoadTest
{
    public class LoadTestAPI
    {
        private HttpClient client;
        public LoadTestAPI()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("https://apim-todoapp-test.azure-api.net/")
            };
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("Ocp-Apim-Subscription-Key"));
            client.DefaultRequestHeaders.Add("Ocp-Apim-Trace", "true");
        }

        [FunctionName("postMutipleToDoItems")]
        public async Task postMultipleItems(int number)
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