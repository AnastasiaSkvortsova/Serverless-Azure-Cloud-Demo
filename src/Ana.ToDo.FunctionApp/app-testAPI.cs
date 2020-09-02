using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace Cosmos.Test
{
    public class app_testAPI
    {

        static string connectionDetails;
        static app_testAPI()
        {
            connectionDetails = Environment.GetEnvironmentVariable("MyConnectionString");
        }

        [FunctionName("postToDoItem")]
        public static async Task<IActionResult> postToDoItem(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a postToDoItem request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var toDoItem = JsonConvert.DeserializeObject<ToDoItem>(requestBody);

            toDoItem = await new Repository(connectionDetails).SaveToDoItemToDB(toDoItem);

            return new OkObjectResult(toDoItem);
        }

        [FunctionName("getToDoItem")]
        public static async Task<IActionResult> getToDoItem(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a getToDoItem request.");

            List<ToDoItem> toDoList= await new Repository(connectionDetails).GetAllToToItemsFromDB();

            return new OkObjectResult(toDoList);
        }

    }
}
