﻿using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ToDoList.Data.Models;
using ToDoList.Service;

namespace ToDoList.UI
{
    static public class Helper
    {
        static Dictionary<Border, int> borderToId = new Dictionary<Border, int>();
        static Dictionary<CheckBox, int> checkBoxToId = new Dictionary<CheckBox, int>();
        static Dictionary<CheckBox, StackPanel> checkBoxToStackPanel = new Dictionary<CheckBox, StackPanel>();

        public static Button RemoveCompletedBtn { get; set; }

        static readonly Service _service;

        public static IEnumerable<ToDoItem> ToDoList { get; private set; }

        static Helper()
        {
            var toDoService = new ToDoListService(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, "dbo", "ToDoItems");
            _service = new Service(toDoService);
        }

        public static void AddItem(ToDoItem item)
        {
            _service.PostItem(item);
            RefreshList();
        }

        public static void RefreshList()
        {
            ToDoList = _service.GetAll();
        }

        public static void RemoveItem(Border border)
        {
            int id = borderToId[border];
            _service.RemoveItem(id);
            RefreshList();
        }

        public static ToDoItem GetItemByBorder(Border border)
        {
            int id = borderToId[border];
            return ToDoList.FirstOrDefault(item => item.Id == id);
        }

        public static void ModifyItem(ToDoItem item)
        {
            _service.ModifyItem(item);
            RefreshList();
        }

        public static void RemoveCompletedItems()
        {
            _service.RemoveCompletedItems();
            RefreshList();
        }

        public static Border GetNewToDoItemBorder(ToDoItem item, double width)
        {
            var isDescNull = string.IsNullOrEmpty(item.Description) || string.IsNullOrWhiteSpace(item.Description);
            Border border = new Border
            {
                BorderThickness = new Thickness(4),
                BorderBrush = Brushes.Black
            };
            var stackPanel = new StackPanel
            {
                Width = width * .85,
                Height = isDescNull ? 70 : 90,
                Background = item.Completed ? Brushes.ForestGreen : Brushes.CadetBlue
            };
            Label labelName = new Label
            {
                Content = item.Name,
                FontSize = 20
            };

            Label labelContent = new Label
            {
                Content = item.Description,
                FontSize = 12
            };
            CheckBox checkBox = new CheckBox
            {
                IsChecked = item.Completed,
                Content = item.Completed ? "Completed" : "Not completed"
            };
            Label labelPriority = new Label
            {
                Content = $"Priority: {item.Priority}",
                HorizontalAlignment = HorizontalAlignment.Right
            };

            checkBox.Checked += (sender, e) =>
            {
                var c = (CheckBox)sender;
                var id = checkBoxToId[c];
                ToDoList.First(todo => todo.Id == id).Completed = true;
                _service.ChangeItemCompletetion(id, true);
                c.Content = "Completed";
                checkBoxToStackPanel[c].Background = Brushes.ForestGreen;
                RemoveCompletedBtn.IsEnabled = true;
            };

            checkBox.Unchecked += (sender, e) =>
            {
                var c = (CheckBox)sender;
                var id = checkBoxToId[c];
                ToDoList.First(todo => todo.Id == id).Completed = false;
                _service.ChangeItemCompletetion(id, false);
                c.Content = "Not completed";
                checkBoxToStackPanel[c].Background = Brushes.CadetBlue;
                if (!ToDoList.Any(todo => todo.Completed))
                {
                    RemoveCompletedBtn.IsEnabled = false;
                }
            };

            border.Child = stackPanel;
            stackPanel.Children.Add(labelName);
            if (!isDescNull)
            {
                stackPanel.Children.Add(labelContent);
            }

            var grid = new Grid();
            grid.Children.Add(labelPriority);
            grid.Children.Add(checkBox);

            stackPanel.Children.Add(grid);

            borderToId.Add(border, item.Id);
            checkBoxToId.Add(checkBox, item.Id);
            checkBoxToStackPanel.Add(checkBox, stackPanel);

            return border;
        }
    }
}
