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
    public class UtilityMethods
    {
        private HttpClient client;
        IConfigurationRoot config;
        public UtilityMethods()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("https://apim-todoapp-test.azure-api.net/")
            };
            config = new ConfigurationBuilder().AddJsonFile("settings.json").Build();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", config["Ocp-Apim-Subscription-Key"]);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Trace", "true");
        }

         public async Task<ToDoItem> postNewToDoItem(string name, bool status) 
        {
            var toDo = new ToDoItem()
            {
                Name = name,
                IsComplete = status
            };
            //convert toDo object into json to pass it as a parameter of POST method
            var jsonItem = JsonConvert.SerializeObject(toDo);

            //execute request and extract data from the response
            var response = await client.PostAsync("toDoItems", new StringContent(jsonItem, Encoding.UTF8, "application/json"));
            System.Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            //assert that Response contains "Created" status code
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            //read the response as a string
            var toDoItem = await response.Content.ReadAsStringAsync();
            
            //convert it into ToDoItem model
            var result = JsonConvert.DeserializeObject<ToDoItem>(toDoItem);
            //assert that responce contain a created item Id and correct item name
            return result;
        }

        public async Task<ToDoItem> getToDoItemById(int id) 
        {
            //execute Http request and extract data from the response
            var response = await client.GetAsync("toDoItem/"+id);

            //assert that response contains success status code, otherwise print status code
            Assert.IsTrue(response.IsSuccessStatusCode, $"Status: {response.StatusCode}");

            //read the response content as a string
            var toDoItem = await response.Content.ReadAsStringAsync();
            //convert it into ToDoItem format
            var result = JsonConvert.DeserializeObject<ToDoItem>(toDoItem);
            //write item parameters to Console, run in Debug mode to see output
            return result;
        }

    }
}