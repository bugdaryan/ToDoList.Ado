using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ToDoList.Data.Models;

namespace ToDoList.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Helper.RefreshList();
            RefreshList();

            ToDoListBox.GotFocus += (sender, e) =>
             {
                 RemoveBtn.IsEnabled = true;
             };

            ToDoListBox.LostFocus += (sender, e) =>
             {
                 if (!RemoveBtn.IsMouseOver || ToDoListBox.SelectedItem == null)
                 {
                     RemoveBtn.IsEnabled = false;
                 }
             };
            Helper.RemoveCompletedBtn = this.RemoveCompletedBtn;
        }

        public void NewToDoItem(ToDoItem item)
        {
            Border border = Helper.GetNewToDoItemBorder(item, ToDoListBox.Width);
            ToDoListBox.Items.Add(border);
        }

        private void NewBtn_Click(object sender, RoutedEventArgs e)
        {
            NewItemWindow newItemWindow = new NewItemWindow();
            newItemWindow.Title = "Add new ToDo item";
            newItemWindow.Owner = Application.Current.MainWindow;
            if (newItemWindow.ShowDialog().Value)
            {
                if (newItemWindow.ToDoItem != null)
                {
                    var item = newItemWindow.ToDoItem;

                    Helper.AddItem(item);
                    RefreshList();
                }
            }
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            Helper.RemoveItem((Border)ToDoListBox.SelectedItem);
            RefreshList();
        }

        private void RefreshList()
        {
            ToDoListBox.Items.Clear();
            foreach (var item in Helper.ToDoList)
            {
                ToDoListBox.Items.Add(Helper.GetNewToDoItemBorder(item, ToDoListBox.Width));
            }
        }

        private void RemoveCompletedBtn_Click(object sender, RoutedEventArgs e)
        {
            Helper.RemoveCompletedItems();
            RefreshList();
            ((Button)sender).IsEnabled = false;
        }
    }
}
