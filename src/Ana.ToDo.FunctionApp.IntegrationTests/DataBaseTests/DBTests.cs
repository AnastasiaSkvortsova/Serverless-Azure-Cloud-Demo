using System.Data.SqlClient;
using System;
using Cosmos.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Ana.Todo.FunctionApp.IntegrationTests.DataBaseTests
{
    [TestClass]
    public class DBTests
    {
        
        SqlConnection sqlConnection;
        public DBTests()
        {
            string connectionDetails = Environment.GetEnvironmentVariable("MyConnectionString");
            sqlConnection = new SqlConnection(connectionDetails);
        }

        [TestMethod]
        public void getAllToDoItems_success() 
        {
            //act
            var listOfToDoItems = new Repository().GetAllToToItemsFromDB().Result;

            //assert
            Assert.IsTrue(listOfToDoItems.Any());
        }

        [TestMethod]
        public void postToDoItem_success() 
        {
            //arange
            var newToDo = new ToDoItem {
                Name = "wash your car",
                IsComplete = true
            };

            //act
            var newItemId = new Repository().SaveToDoItemToDB(newToDo).Result.Id;
            var returnedToDoItem = new Repository().GetItemByIdFromDB(newItemId).Result;

            //assert
            Assert.IsTrue(newToDo.Name == returnedToDoItem.Name);
            Assert.IsTrue(newToDo.IsComplete == returnedToDoItem.IsComplete);
            
        }

    }
}