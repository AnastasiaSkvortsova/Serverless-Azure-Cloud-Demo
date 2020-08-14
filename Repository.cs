using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Cosmos.Test
{
    public class Repository
    {
        string connectionDetails = "Server=tcp:ana-sqlserver.database.windows.net,1433;Initial Catalog=database-ana;Persist Security Info=False;User ID=ana;Password=007Pus007;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        SqlConnection sqlConnection;
        public Repository()
        {
            sqlConnection = new SqlConnection(connectionDetails);
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
            string getAllToDos = @"SELECT * FROM toDoItems";
            
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
    }
}