using System;
using System.Windows;
using ToDoList.Data.Models;

namespace ToDoList.UI
{
    /// <summary>
    /// Interaction logic for ModifyItemWindow.xaml
    /// </summary>
    public partial class ModifyItemWindow : Window
    {
        public ToDoItem ToDoItem { get; private set; }
        public ModifyItemWindow(ToDoItem item)
        {
            InitializeComponent();
            ToDoItem = item;
            InitControls();
        }

        private void InitControls()
        {
            ModifyTodoNameTextBox.Text = ToDoItem.Name;
            ModifyTodoDescriptionTextBox.Text = ToDoItem.Description;
            SetPirorityButton();
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
               var newItem = new ToDoItem
                {
                    Id = ToDoItem.Id,
                    Completed = ToDoItem.Completed,
                    Created = ToDoItem.Created,
                    Description = ModifyTodoDescriptionTextBox.Text,
                    Name = ModifyTodoNameTextBox.Text,
                    Priority = GetToDoPriority()
                };
                if (CheckIfItemChanged(newItem))
                {
                    ToDoItem = newItem;
                    DialogResult = true;
                }
                else
                {
                    DialogResult = false;
                }
                Close();
            }
            else
            {
                invalidNameLbl.Visibility = Visibility.Visible;
            }

        }

        private bool CheckIfItemChanged(ToDoItem newItem)
        {
            return newItem.Name != ToDoItem.Name
                || newItem.Description != ToDoItem.Description
                || newItem.Priority != ToDoItem.Priority;
                
        }

        private void SetPirorityButton()
        {
            switch (ToDoItem.Priority)
            {
                case Priority.Zero:
                    ZeroPriorityRadioButton.IsChecked = true;
                    return;
                case Priority.Low:
                    LowPriorityRadioButton.IsChecked = true;
                    return;
                case Priority.Medium:
                    MediumPriorityRadioButton.IsChecked = true;
                    return;
                case Priority.Important:
                    ImportantPriorityRadioButton.IsChecked = true;
                    return;
                case Priority.Critical:
                    CriticalPriorityRadioButton.IsChecked = true;
                    return;
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
            if (string.IsNullOrEmpty(ModifyTodoNameTextBox.Text) || string.IsNullOrWhiteSpace(ModifyTodoNameTextBox.Text))
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
