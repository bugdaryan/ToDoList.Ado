using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace ToDoList.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Library.ToDoList list = new Library.ToDoList();

        string connectionString =
           "Server=(localdb)\\mssqllocaldb;Database=ToDoList;Trusted_Connection=True;MultipleActiveResultSets=true;";


        public MainWindow()
        {
            InitializeComponent();
            ReadData();
        }

        public void ReadData()
        {
            string queryString = "SELECT * FROM ToDoItems";
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
                        ToDoListBox.Items.Add(new Label { Content = $"Id: {reader[0]}\tName: {reader[1]}\tCompleted: {((bool)reader[3])}" });
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void NewBtn_Click(object sender, RoutedEventArgs e)
        {
            list.Add(new Library.ToDoItem { Name = "Item" });
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
