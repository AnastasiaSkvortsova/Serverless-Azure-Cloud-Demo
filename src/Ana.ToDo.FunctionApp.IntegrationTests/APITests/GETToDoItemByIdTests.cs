using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ana.ToDo.FunctionApp;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Ana.Todo.FunctionApp.IntegrationTests.APITests
{
    [TestClass]
    public class GETToDoItemByIdTests
    {
        private HttpClient client;
        IConfigurationRoot config;
        public GETToDoItemByIdTests()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("https://apim-todoapp-test.azure-api.net/")
            };
            config = new ConfigurationBuilder().AddJsonFile("settings.json").Build();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", config["Ocp-Apim-Subscription-Key"]);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Trace", "true");
        }

        [TestMethod]
        [Description("Positive; GET ToDo Item By provided Id (number)")]
        public async Task getToDoItemById_OK() 
        {
            //POST new toDo Item
            UtilityMethods u = new UtilityMethods();
            var newToDo = await u.postNewToDoItem("feed the dog", false);
            
            //execute Http request and extract data from the response
            var response = await client.GetAsync("toDoItem/"+newToDo.Id);

            //assert that response contains success status code, otherwise print status code
            Assert.IsTrue(response.IsSuccessStatusCode, $"Status: {response.StatusCode}");

            //read the response content as a string
            var toDoItem = await response.Content.ReadAsStringAsync();
            //convert it into ToDoItem format
            var result = JsonConvert.DeserializeObject<ToDoItem>(toDoItem);
            //write item parameters to Console, run in Debug mode to see output
            System.Diagnostics.Debug.WriteLine($"itemId: {result.Id}, itemName: {result.Name}, completionStatus: {result.IsComplete}");
        }

    }
}