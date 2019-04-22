using System;
using System.Windows;
using ToDoList.Data.Enums;
using ToDoList.Data.Models;

namespace ToDoList.UI
{
    /// <summary>
    /// Interaction logic for NewItemWindow.xaml
    /// </summary>
    public partial class NewItemWindow : Window
    {
        public ToDoItem ToDoItem { get; private set; }
        public NewItemWindow()
        {
            InitializeComponent();
            ZeroPriorityRadioButton.IsChecked = true;
            NewTodoNameTextBox.Focus();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckForValidItem())
            {
                ToDoItem = new ToDoItem
                {
                    Completed = false,
                    Created = DateTime.Now,
                    Description = NewTodoDescriptionTextBox.Text,
                    Name = NewTodoNameTextBox.Text,
                    Priority = GetToDoPriority()
                };
                DialogResult = true;
                Close();
            }
            else
            {
                invalidNameLbl.Visibility = Visibility.Visible;
            }

        }

        private Priority GetToDoPriority()
        {
            if (ZeroPriorityRadioButton.IsChecked.Value)
            {
                return Priority.Zero;
            }

            if (LowPriorityRadioButton.IsChecked.Value)
            {
                return Priority.Low;
            }

            if (MediumPriorityRadioButton.IsChecked.Value)
            {
                return Priority.Medium;
            }

            if (ImportantPriorityRadioButton.IsChecked.Value)
            {
                return Priority.Important;
            }

            return Priority.Critical;
        }

        private bool CheckForValidItem()
        {
            if (string.IsNullOrEmpty(NewTodoNameTextBox.Text) || string.IsNullOrWhiteSpace(NewTodoNameTextBox.Text))
            {
                return false;
            }
            return true;
        }

        private void NewTodoNameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (invalidNameLbl.Visibility == Visibility.Visible)
            {
                invalidNameLbl.Visibility = Visibility.Collapsed;
            }
        }
    }
}
