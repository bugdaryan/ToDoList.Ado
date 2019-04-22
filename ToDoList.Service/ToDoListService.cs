using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ToDoList.Data;
using ToDoList.Data.Enums;
using ToDoList.Data.Models;

namespace ToDoList.Service
{
    public class ToDoListService : IToDoList
    {
        readonly SqlConnectionStringBuilder _builder;
        private readonly string _tableName;
        private readonly string _schemaName;

        public ToDoListService(string connection, string schemaName, string tableName)
        {
            _builder = new SqlConnectionStringBuilder(connection);
            _schemaName = schemaName;
            _tableName = tableName;
            EnsureDatabaseCreated();
        }

        public void ChangeItemCompletetion(int id, bool completed)
        {
            string query = $"UPDATE {_builder.InitialCatalog}.{_schemaName}.{_tableName} SET Completed={(completed ? 1 : 0)} WHERE Id = {id}";
            ExecuteNonQuery(query);
        }

        public IList<ToDoItem> GetAll()
        {
            string query = $"SELECT * FROM {_builder.InitialCatalog}.{_schemaName}.{_tableName} ORDER BY Priority DESC";
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
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
                return items;
            }
        }

        private void EnsureDatabaseCreated()
        {
            string query = $"SELECT database_id FROM sys.databases WHERE Name='{_builder.InitialCatalog}'";
            using (var connection = new SqlConnection(_builder.ConnectionString))
            {
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result == null)
                    {
                        query = $"CREATE DATABASE {_builder.InitialCatalog}";
                        command = new SqlCommand(query, connection);
                        command.ExecuteNonQuery();
                        query = $@"CREATE TABLE {_builder.InitialCatalog}.{_schemaName}.{_tableName}(
                            [Id] [int] IDENTITY NOT NULL,
                            [Name] [nvarchar](50) NOT NULL,
                            [Completed] [bit] NULL DEFAULT 0,
                            [Priority] [int] NULL DEFAULT 0,
                            [Created] [datetime2] NULL DEFAULT GETDATE(),
                            [Description] [nvarchar](200) NULL
                                )";
                        command = new SqlCommand(query, connection);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public void PostItem(ToDoItem item)
        {
            string query =
                $"INSERT INTO {_builder.InitialCatalog}.{_schemaName}.{_tableName} (Name, Description, Completed, Priority) VALUES ('{item.Name}', '{item.Description}', 0, {(int)item.Priority})";
            ExecuteNonQuery(query);
        }

        public void ModifyItem(ToDoItem item)
        {
            string query =
                $@"UPDATE {_builder.InitialCatalog}.{_schemaName}.{_tableName} 
                    SET [Name] = '{item.Name}', 
                        [Description] = '{item.Description}', 
                        [Priority] = {(int)item.Priority}
                    WHERE Id = {item.Id}";
            ExecuteNonQuery(query);
        }

        public void RemoveItem(int id)
        {
            string query =
             $"DELETE FROM {_builder.InitialCatalog}.{_schemaName}.{_tableName} WHERE Id = {id}";
            ExecuteNonQuery(query);
        }

        public void RemoveCompletedItems()
        {
            string query =
                $"DELETE FROM {_builder.InitialCatalog}.{_schemaName}.{_tableName} Where Completed = 1";
            ExecuteNonQuery(query);
        }

        public void RemoveAllItems()
        {
            string query =
                $"DELETE FROM {_builder.InitialCatalog}.{_schemaName}.{_tableName}";
            ExecuteNonQuery(query);
        }

        private void ExecuteNonQuery(string query)
        {
            using (var connection = new SqlConnection(_builder.ConnectionString))
            {
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }
    }
}
