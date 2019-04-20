using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ToDoList.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

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
                        var item = new ToDoItem
                        {
                            Name = reader["Name"].ToString(),
                            Completed = (bool)reader["Completed"],
                            Description = reader["Description"].ToString(),
                            Priority = Priority.Zero
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

        public void NewToDoItem(ToDoItem item)
        {
            Border border = new Border
            {
                BorderThickness = new Thickness(4),
                BorderBrush = Brushes.Black
            };
            var stackPanel = new StackPanel
            {
                Width = ToDoListBox.Width*.85,
                Height = 120
            };
            Label labelName = new Label
            {
                Content = item.Name,
                FontSize = 23
            };

            Label labelContent = new Label
            {
                 Content = item.Description,
                 FontSize = 16
            };
            CheckBox checkBox = new CheckBox
            {
                IsChecked = false
            };
            Label labelPriority = new Label
            {
                Content = $"Priority: {item.Priority}"
            };

            border.Child = stackPanel;
            stackPanel.Children.Add(labelName);
            if(!string.IsNullOrEmpty (item.Description) && !string.IsNullOrWhiteSpace(item.Description))
                stackPanel.Children.Add(labelContent);
            stackPanel.Children.Add(labelPriority);
            stackPanel.Children.Add(checkBox);

            ToDoListBox.Items.Add(border);
        }

        private void NewBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
