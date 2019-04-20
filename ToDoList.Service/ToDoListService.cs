using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ToDoList.Data;
using ToDoList.Data.Models;

namespace ToDoList.Service
{
    public class ToDoListService : IToDoList
    {
        SqlConnectionStringBuilder builder;

        public ToDoListService(string connection)
        {
            builder = new SqlConnectionStringBuilder(connection);
        }

        public void ChangeItemCompletetion(int id, bool completed)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ToDoItem> GetAll()
        {
            string query = $"SELECT * FROM {builder.DataSource}";
            var items = new List<ToDoItem>();
            using (var connection = new SqlConnection(query))
            {
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var item = new ToDoItem
                        {
                            Name = reader["Name"].ToString(),
                            Description = reader["Description"].ToString(),
                            Completed = (bool)reader["Completed"],
                            Created = DateTime.Now,
                            Id = (int)reader["Id"],
                            Priority = (Priority)reader["Priority"]
                        };
                        items.Add(item);
                    }
                    reader.Close();
                }
                catch (Exception)
                {

                    throw;
                }
                return items;
            }
        }

        public void PostItem()
        {
            throw new NotImplementedException();
        }

        public void RemoveItem(int id)
        {
            throw new NotImplementedException();
        }
    }
}
