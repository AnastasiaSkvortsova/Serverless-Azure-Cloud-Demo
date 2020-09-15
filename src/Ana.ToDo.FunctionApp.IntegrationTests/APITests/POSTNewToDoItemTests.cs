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
    public class POSTNewToDoItemTests
    {
        private HttpClient client;
        IConfigurationRoot config;
        public POSTNewToDoItemTests()
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
            var response = await client.PostAsync("toDoItems", new StringContent(jsonItem, Encoding.UTF8, "application/json"));

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
        [Description("Positive; POST an ToDo json with Id (Id will be ignored)")]
        public async Task postInvalidToDoJson_WithId_BadRequest() 
        {
            var toDo = new ToDoItem()
            {
                Id = 2,
                Name = "feed the cat",
                IsComplete = true
            };
            //convert toDo object into json to pass it as a parameter of POST method
            var jsonItem = JsonConvert.SerializeObject(toDo);

            //execute request and extract data from the response
            var response = await client.PostAsync("toDoItems", new StringContent(jsonItem, Encoding.UTF8, "application/json"));

            //assert that Response contains "Created" status code
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        [Description("Negative; POST an Empty ToDo json")]
        public async Task postEmptyToDoJson_BadRequest() 
        {
            var str = "{}";

            //execute request and extract data from the response
            var response = await client.PostAsync("toDoItems", new StringContent(str, Encoding.UTF8, "application/json"));

            //assert that Response contains "Created" status code
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        //VALIDATION API TESTS

        [TestMethod]
        [Description("Negative; POST an Invalid ToDo json (with Missing Required Parameter 'Name')")]
        public async Task postInvalidToDoJson_MissingRequiredParameterName_BadRequest() 
        {
            var toDo = new ToDoItem()
            {
                IsComplete = true
            };
            //convert toDo object into json to pass it as a parameter of POST method
            var jsonItem = JsonConvert.SerializeObject(toDo);

            //execute request and extract data from the response
            var response = await client.PostAsync("toDoItems", new StringContent(jsonItem, Encoding.UTF8, "application/json"));

            //assert that Response contains "Created" status code
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [Description("Negative; POST an Invalid ToDo json (with Short'Name')")]
        public async Task postInvalidToDoJson_ShortName_BadRequest() 
        {
            var toDo = new ToDoItem()
            {
                Name = "n",
                IsComplete = true
            };
            //convert toDo object into json to pass it as a parameter of POST method
            var jsonItem = JsonConvert.SerializeObject(toDo);

            //execute request and extract data from the response
            var response = await client.PostAsync("toDoItems", new StringContent(jsonItem, Encoding.UTF8, "application/json"));

            //assert that Response contains "Created" status code
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [Description("Negative; POST an Invalid ToDo json (with Long'Name')")]
        public async Task postInvalidToDoJson_LongName_BadRequest() 
        {
            var toDo = new ToDoItem()
            {
                Name = "M7NUfbHo7cD6w4J9sG0GzoLTS3HoV3VhJ02kjng6xz5dWADySEaGRAufHVAUf",
                IsComplete = true
            };
            //convert toDo object into json to pass it as a parameter of POST method
            var jsonItem = JsonConvert.SerializeObject(toDo);

            //execute request and extract data from the response
            var response = await client.PostAsync("toDoItems", new StringContent(jsonItem, Encoding.UTF8, "application/json"));

            //assert that Response contains "Created" status code
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}