using System.Data.SqlClient;
using System;
using Ana.ToDo.FunctionApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

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

        [TestMethod]
        public void deleteToDoItemByIdFromDB_success() 
        {
            //arange
            var newToDo = new ToDoItem {
                Name = "wash your car",
                IsComplete = true
            };

            //act
            var newItemId = new Repository(connectionDetails).SaveToDoItemToDB(newToDo).Result.Id;
            var rowsAffected = new Repository(connectionDetails).DeleteItemByIdFromDB(newItemId).Result;

            //assert
            Assert.AreEqual(1, rowsAffected);
        }

        [TestMethod]
        public async Task deleteAllToDoItemsFromDB_success() 
        {
            //arange
            var newToDo1 = new ToDoItem {
                Name = "wash your car",
                IsComplete = true
            };

            var newToDo2 = new ToDoItem {
                Name = "wash your teeth",
                IsComplete = true
            };

            //act
            await new Repository(connectionDetails).SaveToDoItemToDB(newToDo1);
            await new Repository(connectionDetails).SaveToDoItemToDB(newToDo2);
            var rowsAffected = new Repository(connectionDetails).DeleteAllToDoItemsFromDB().Result;

            //assert
            Assert.IsTrue(rowsAffected >= 2);
        }
    }
}