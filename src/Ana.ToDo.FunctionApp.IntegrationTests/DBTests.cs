using System.Data.SqlClient;
using System;
using Ana.ToDo.FunctionApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Ana.Todo.FunctionApp.IntegrationTests
{
    [TestClass]
    public class DBTests
    {
        IConfigurationRoot config;
        string connectionDetails; 
        public DBTests()
        {
            config = new ConfigurationBuilder().AddJsonFile("settings.json").Build();
            connectionDetails = config["connectionString"];
        }

        [TestMethod]
        public void postToDoItemToDB_success() 
        {
            //arange
            var newToDo = new ToDoItem {
                Name = "wash your car",
                IsComplete = true
            };

            //act
            var newItemId = new Repository(connectionDetails).SaveToDoItemToDB(newToDo).Result.Id;
            var returnedToDoItem = new Repository(connectionDetails).GetItemByIdFromDB(newItemId).Result;

            //assert
            Assert.IsTrue(newToDo.Name == returnedToDoItem.Name);
            Assert.IsTrue(newToDo.IsComplete == returnedToDoItem.IsComplete);
            
        }

        [TestMethod]
        public void getAllToDoItemsFromDB_success() 
        {
            //act
            var listOfToDoItems = new Repository(connectionDetails).GetAllToToItemsFromDB().Result;

            //assert
            Assert.IsTrue(listOfToDoItems.Any());
        }

    }
}