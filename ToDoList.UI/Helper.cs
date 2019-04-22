using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ToDoList.Data.Enums;
using ToDoList.Data.Models;
using ToDoList.Service;

namespace ToDoList.UI
{
    static public class Helper
    {
        static Dictionary<Border, int> _borderToId = new Dictionary<Border, int>();
        static Dictionary<CheckBox, int> _checkBoxToId = new Dictionary<CheckBox, int>();
        static Dictionary<CheckBox, StackPanel> _checkBoxToStackPanel = new Dictionary<CheckBox, StackPanel>();
        static SortBy _sortBy = SortBy.None;
        static SortOrder _sortOrder = SortOrder.ASC;
        public static Button RemoveCompletedBtn { get; set; }

        static readonly Service _service;

        public static IList<ToDoItem> ToDoList { get; private set; }

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
            ToDoList = _service.GetAll(_sortBy, _sortOrder);
        }

        public static void RemoveItem(Border border)
        {
            int id = _borderToId[border];
            _service.RemoveItem(id);
            RefreshList();
        }

        public static ToDoItem GetItemByBorder(Border border)
        {
            int id = _borderToId[border];
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

        public static void RemovaAllItems()
        {
            _service.RemoveAllItems();
            ToDoList.Clear();
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
                var id = _checkBoxToId[c];
                ToDoList.First(todo => todo.Id == id).Completed = true;
                _service.ChangeItemCompletetion(id, true);
                c.Content = "Completed";
                _checkBoxToStackPanel[c].Background = Brushes.ForestGreen;
                RemoveCompletedBtn.IsEnabled = true;
            };

            checkBox.Unchecked += (sender, e) =>
            {
                var c = (CheckBox)sender;
                var id = _checkBoxToId[c];
                ToDoList.First(todo => todo.Id == id).Completed = false;
                _service.ChangeItemCompletetion(id, false);
                c.Content = "Not completed";
                _checkBoxToStackPanel[c].Background = Brushes.CadetBlue;
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

            _borderToId.Add(border, item.Id);
            _checkBoxToId.Add(checkBox, item.Id);
            _checkBoxToStackPanel.Add(checkBox, stackPanel);

            return border;
        }

        public static void SetSortOrder(string sortQuery)
        {
            GetSortOrder(sortQuery);
            if (ToDoList.Count != 0)
            {
                bool orderByAsc = _sortOrder == SortOrder.ASC;
                switch (_sortBy)
                {
                    case SortBy.Created:
                        ToDoList = (orderByAsc ?
                            ToDoList.OrderBy(item => item.Created).ToList()
                            : ToDoList.OrderByDescending(item => item.Created).ToList());
                        break;
                    case SortBy.Name:
                        ToDoList = (orderByAsc ?
                            ToDoList.OrderBy(item => item.Name).ToList()
                            : ToDoList.OrderByDescending(item => item.Name).ToList());
                        break;
                    case SortBy.Priority:
                        ToDoList = (orderByAsc ?
                            ToDoList.OrderBy(item => item.Priority).ToList()
                            : ToDoList.OrderByDescending(item => item.Priority).ToList());
                        break;
                    default:
                        break;
                }
            }
        }

        public static void GetSortOrder(string sortQuery)
        {
            switch (sortQuery)
            {
                case "Created ascending":
                    _sortOrder = SortOrder.ASC;
                    _sortBy = SortBy.Created;
                    break;
                case "Created descending":
                    _sortOrder = SortOrder.DESC;
                    _sortBy = SortBy.Created;
                    break;
                case "Priority ascending":
                    _sortOrder = SortOrder.ASC;
                    _sortBy = SortBy.Priority;
                    break;
                case "Priority descending":
                    _sortOrder = SortOrder.DESC;
                    _sortBy = SortBy.Priority;
                    break;
                case "Name ascending":
                    _sortOrder = SortOrder.ASC;
                    _sortBy = SortBy.Name;
                    break;
                case "Name descending":
                    _sortOrder = SortOrder.DESC;
                    _sortBy = SortBy.Name;
                    break;
            }
        }
    }
}