using System.Data.SqlClient;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
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
    public class ToDoItemTests
    {
        private HttpClient client;
        IConfigurationRoot config;
        public ToDoItemTests()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("https://todofunctionappana.azurewebsites.net/api/")
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
            var response = await client.GetAsync("getToDoItem");

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

        [TestMethod]
        [Description("Positive; POST New ToDo Item")]
        public async Task postToDoItems_Created() 
        {
            var toDo = new ToDoItem()
            {
                Name = "feed the cat",
                IsComplete = true
            };
            //convert toDo object into json to pass it as a parameter of POST method
            var jsonItem = JsonConvert.SerializeObject(toDo);

            //execute request and extract data from the response
            var response = await client.PostAsync("postToDoItem", new StringContent(jsonItem, Encoding.UTF8, "application/json"));

            //assert that Response contains "Created" status code
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            //read the response as a string
            var toDoItem = await response.Content.ReadAsStringAsync();
            //convert it into ToDoItem model
            var result = JsonConvert.DeserializeObject<ToDoItem>(toDoItem);
            //assert that responce contain a created item Id and correct item name
            Assert.IsTrue(result.Id != 0);
            Assert.AreEqual(toDo.Name, result.Name);
        }

        [TestMethod]
        [Description("Negative; POST an Empty ToDo json")]
        public async Task postEmptyToDoJson_BadRequest() 
        {
            var str = "";
            //convert toDo object into json to pass it as a parameter of POST method
            var jsonItem = JsonConvert.SerializeObject(str);

            //execute request and extract data from the response
            var response = await client.PostAsync("postToDoItem", new StringContent(jsonItem, Encoding.UTF8, "application/json"));

            //assert that Response contains "Created" status code
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

    }
}