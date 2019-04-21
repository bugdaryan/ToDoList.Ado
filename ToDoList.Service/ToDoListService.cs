﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ToDoList.Data;
using ToDoList.Data.Models;

namespace ToDoList.Service
{
    public class ToDoListService : IToDoList
    {
        readonly SqlConnectionStringBuilder _builder;


        public ToDoListService(string connection)
        {
            _builder = new SqlConnectionStringBuilder(connection);
        }

        public void ChangeItemCompletetion(int id, bool completed)
        {
            string query = $"UPDATE ToDoItems SET Completed={(completed ? 1 : 0)} WHERE Id = {id}";
            using (var connection = new SqlConnection(_builder.ConnectionString))
            {
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public IEnumerable<ToDoItem> GetAll()
        {
            string query = $"SELECT * FROM ToDoItems ORDER BY Priority DESC";
            var items = new List<ToDoItem>();
            using (var connection = new SqlConnection(_builder.ConnectionString))
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

        public void PostItem(ToDoItem item)
        {
            string query =
                $"INSERT INTO ToDoItems (Name, Description, Completed, Priority) VALUES ('{item.Name}', '{item.Description}', 0, {(int)item.Priority})";

            using (var connection = new SqlConnection(_builder.ConnectionString))
            {
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void RemoveItem(int id)
        {
            string query =
             $"DELETE FROM ToDoItems WHERE Id = {id}";

            using (var connection = new SqlConnection(_builder.ConnectionString))
            {
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
