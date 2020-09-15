using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ana.ToDo.FunctionApp;
using System.Collections.Generic;

namespace Ana.Todo.FunctionApp.IntegrationTests.APITests
{
    [TestClass]
    public class GETAllToDoItemsTests
    {
        private HttpClient client;
        IConfigurationRoot config;
        public GETAllToDoItemsTests()
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
        [Description("Positive; GET All ToDo Items")]
        public async Task getAllToDoItems_OK() 
        {
            //execute Http request and extract data from the response
            var response = await client.GetAsync("toDoItems");

            //assert that response contains success status code, otherwise print status code
            Assert.IsTrue(response.IsSuccessStatusCode, $"Status: {response.StatusCode}");

            //read the response content as a string
            var toDoItems = await response.Content.ReadAsStringAsync();
            //convert it into a list of ToDoItems
            var result = JsonConvert.DeserializeObject<List<ToDoItem>>(toDoItems);
            //assert that there is no matching Ids
            for (int i=0; i<result.Count; i++)
            {
                for (int j=i+1; j<result.Count; j++)
                {
                    if (result[i].Id == result[j].Id)
                    {
                        Assert.Fail("Ids must differ");
                    }
                }
            }

            //write each item to Console, run in Debug mode to see output
            result.ForEach(i => System.Diagnostics.Debug.WriteLine($"itemId: {i.Id}, itemName: {i.Name}, completionStatus: {i.IsComplete}"));
        }
    }
}