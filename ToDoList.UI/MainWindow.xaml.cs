using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ToDoList.Data.Models;

namespace ToDoList.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<Border, int> borderToId = new Dictionary<Border, int>();
        Dictionary<CheckBox, int> checkBoxToId = new Dictionary<CheckBox, int>();
        string connectionString =
           "Server=(localdb)\\mssqllocaldb;Database=ToDoList;Trusted_Connection=True;MultipleActiveResultSets=true;";


        public MainWindow()
        {
            InitializeComponent();
            ReadData();
        }

        public void ReadData()
        {
            ToDoListBox.Items.Clear();
            string queryString = "SELECT * FROM ToDoItems ORDER BY Priority DESC";
            using (SqlConnection connection =
            new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var item = new ToDoItem
                        {
                            Name = reader["Name"].ToString(),
                            Completed = (bool)reader["Completed"],
                            Description = reader["Description"].ToString(),
                            Priority = (Priority)reader["priority"],
                            Id = (int)reader["Id"],
                            Created = (DateTime)reader["Created"]
                        };

                        NewToDoItem(item);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void WriteData()
        {
            string queryString = $"INSERT INTO ToDoItems (Name, Description, Completed, Priority) VALUES ('{NewItem.Name}', '{NewItem.Description}', 0, {(int)NewItem.Priority});";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void RemoveToDoItem(int id)
        {
            string queryString = $"DELETE FROM ToDoItems WHERE Id = {id};";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void ChangeItemCompletetion(int id, bool completed)
        {
            string queryString = $"UPDATE ToDoItems set Completed = {(completed? 1:0)} WHERE Id = {id};";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        public ToDoItem NewItem { get; set; }

        public void NewToDoItem(ToDoItem item)
        {
            Border border = Helper.GetNewToDoItemBorder(item, ToDoListBox.Width);
            ToDoListBox.Items.Add(border);
        }

        private void NewBtn_Click(object sender, RoutedEventArgs e)
        {
            NewItemWindow newItemWindow = new NewItemWindow();
            newItemWindow.Show();
            newItemWindow.Closing += (s, ev) =>
            {
                if (newItemWindow.ToDoItem != null)
                {
                    NewItem = newItemWindow.ToDoItem;

                    WriteData();
                    ReadData();
                }
            };
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            int id = borderToId[(Border)ToDoListBox.SelectedItem];
            RemoveToDoItem(id);
            ReadData();
        }
    }
}
