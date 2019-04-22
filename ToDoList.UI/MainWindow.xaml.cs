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
                 //if (ToDoListBox.SelectedItem != null)
                 //{
                 RemoveBtn.IsEnabled = true;
                 ModifyBtn.IsEnabled = true;
                 //}
             };

            ToDoListBox.LostFocus += (sender, e) =>
             {
                 if (ToDoListBox.SelectedItem == null)
                 {
                     if (!RemoveBtn.IsMouseOver)
                     {
                         RemoveBtn.IsEnabled = false;
                     }
                     if (!ModifyBtn.IsMouseOver)
                     {
                         ModifyBtn.IsEnabled = false;
                     }
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
            if (ToDoListBox.SelectedItem == null)
            {
                MessageBox.Show("Please select item to remove it");
                ModifyBtn.IsEnabled = false;
                RemoveBtn.IsEnabled = false;
                return;
            }
            Helper.RemoveItem((Border)ToDoListBox.SelectedItem);
            RefreshList();
        }

        private void RefreshList()
        {
            ToDoListBox.Items.Clear();
            foreach (var item in Helper.ToDoList)
            {
                ToDoListBox.Items.Add(Helper.GetNewToDoItemBorder(item, ToDoListBox.Width));
                if (item.Completed)
                {
                    RemoveCompletedBtn.IsEnabled = true;
                }
            }
        }

        private void RemoveCompletedBtn_Click(object sender, RoutedEventArgs e)
        {
            Helper.RemoveCompletedItems();
            RefreshList();
            ((Button)sender).IsEnabled = false;
        }

        private void ModifyBtn_Click(object sender, RoutedEventArgs e)
        {

            if (ToDoListBox.SelectedItem == null)
            {
                MessageBox.Show("Please select item to modify it");
                ModifyBtn.IsEnabled = false;
                RemoveBtn.IsEnabled = false;
                return;
            }
            var item = Helper.GetItemByBorder((Border)ToDoListBox.SelectedItem);
            if (item != null)
            {
                ModifyItemWindow modifyItemWindow = new ModifyItemWindow(item);
                modifyItemWindow.Owner = Application.Current.MainWindow;
                if (modifyItemWindow.ShowDialog().Value)
                {
                    if (modifyItemWindow.ToDoItem != null)
                    {
                        item = modifyItemWindow.ToDoItem;

                        Helper.ModifyItem(item);
                        RefreshList();
                    }
                }
            }
        }
    }
}
