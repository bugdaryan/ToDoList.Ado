using System;
using System.Windows;

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
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
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
                Close();
            }
            else
            {
                //TriggerInvalidInput();
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
    }
}
