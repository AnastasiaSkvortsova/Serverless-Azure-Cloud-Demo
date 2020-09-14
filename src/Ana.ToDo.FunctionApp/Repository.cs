using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Configuration;
using System;

namespace Ana.ToDo.FunctionApp
{
    public class Repository
    {
        
        SqlConnection sqlConnection;
        public Repository(string connectionString)
        {
            sqlConnection = new SqlConnection(connectionString);
        }
        public async Task<ToDoItem> SaveToDoItemToDB (ToDoItem toDoItem)
        {
            await sqlConnection.OpenAsync();
            string addToDB = @"INSERT INTO toDoItems (name, isComplete) OUTPUT inserted.id VALUES (@name, @isComplete)";
       
            SqlCommand saveToDBCommand = new SqlCommand(addToDB, sqlConnection);
            saveToDBCommand.Parameters.AddWithValue("@name", toDoItem.Name);
            saveToDBCommand.Parameters.AddWithValue("@isComplete", toDoItem.IsComplete);

            toDoItem.Id = (int)saveToDBCommand.ExecuteScalar();
            
            await sqlConnection.CloseAsync();
            return toDoItem;
        }

        public async Task<List<ToDoItem>> GetAllToToItemsFromDB ()
        {
            await sqlConnection.OpenAsync();
            string getAllToDos = "SELECT * FROM toDoItems";
            
            SqlCommand getAllToDosCommand = new SqlCommand(getAllToDos, sqlConnection);

            List<ToDoItem> listOfToDos = new List<ToDoItem>();

            using (SqlDataReader reader = getAllToDosCommand.ExecuteReader()) 
            {
                while (reader.Read())
                {            
                    ToDoItem item = new ToDoItem();       
                    item.Id = (int)reader.GetValue("id");
                    item.Name = (string)reader.GetValue("name");
                    item.IsComplete = (bool)reader.GetValue("isComplete");
                    listOfToDos.Add(item);
                }
            }
            
            await sqlConnection.CloseAsync();
            return listOfToDos;
        }

        public async Task<ToDoItem> GetItemByIdFromDB (int Id)
        {
            await sqlConnection.OpenAsync();
            string getItemById = @"SELECT * FROM toDoItems WHERE id=@Id";

            SqlCommand getItemByIdCommand = new SqlCommand(getItemById, sqlConnection);
            getItemByIdCommand.Parameters.AddWithValue("@Id", Id);

            ToDoItem toDoItem = new ToDoItem();
            using (SqlDataReader reader = getItemByIdCommand.ExecuteReader())
            {
                if(reader.Read())
                {
                    toDoItem.Id = (int)reader.GetValue("id");
                    toDoItem.Name = (string)reader.GetValue("name");
                    toDoItem.IsComplete = (bool)reader.GetValue("isComplete");
                }
            }

            await sqlConnection.CloseAsync();
            return toDoItem;
        }

        public async Task<int> DeleteAllToDoItemsFromDB ()
        {
            await sqlConnection.OpenAsync();
            string deleteAllToDos = "DELETE FROM toDoItems";
            
            SqlCommand deleteAllToDosCommand = new SqlCommand(deleteAllToDos, sqlConnection);
            var numberOfRowsAffected = deleteAllToDosCommand.ExecuteNonQuery();
            
            await sqlConnection.CloseAsync();
            return numberOfRowsAffected;
        }

        public async Task<int> DeleteItemByIdFromDB (int id)
        {
            await sqlConnection.OpenAsync();
            string deleteToDoById = @"DELETE FROM toDoItems WHERE id=@Id";
            
            SqlCommand deleteToDoByIdCommand = new SqlCommand(deleteToDoById, sqlConnection);
            deleteToDoByIdCommand.Parameters.AddWithValue("@Id", id);
            var numberOfRowsAffected = deleteToDoByIdCommand.ExecuteNonQuery();
            
            await sqlConnection.CloseAsync();
            return numberOfRowsAffected;
        }
    }
}