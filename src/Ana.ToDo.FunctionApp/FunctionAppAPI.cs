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
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Ana.ToDo.FunctionApp
{
    public class FunctionAppAPI
    {
        static string connectionDetails;
        static FunctionAppAPI()
        {
            connectionDetails = Environment.GetEnvironmentVariable("MyConnectionString");
        }

        [FunctionName("postToDoItem")]
        public static async Task<IActionResult> postToDoItem(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a postToDoItem request.");
            ToDoValidator validator = new ToDoValidator();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var toDoItem = JsonConvert.DeserializeObject<ToDoItem>(requestBody);
            var result = validator.Validate(toDoItem);

            if (!result.IsValid)
            {
                return new BadRequestObjectResult("Pass a Valid ToDo Model");
            }

            toDoItem = await new Repository(connectionDetails).SaveToDoItemToDB(toDoItem);

            return new CreatedResult("toDoItems", toDoItem);
        }

        [FunctionName("getToDoItem")]
        public static async Task<IActionResult> getToDoItem(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getToDoItem/{id?}")] HttpRequest req, int? id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a getToDoItem request.");

            if (id == null) 
            {
                List<ToDoItem> toDoList= await new Repository(connectionDetails).GetAllToToItemsFromDB();

                return new OkObjectResult(toDoList);
            } else
            {
                ToDoItem toDo = await new Repository(connectionDetails).GetItemByIdFromDB(id.Value);

                return new OkObjectResult(toDo);
            }

        }

    }
}
