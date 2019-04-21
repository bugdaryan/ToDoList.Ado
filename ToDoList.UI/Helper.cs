using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ToDoList.Data.Models;

namespace ToDoList.UI
{
    static public class Helper
    {
        static Dictionary<Border, int> borderToId = new Dictionary<Border, int>();
        static Dictionary<CheckBox, int> checkBoxToId = new Dictionary<CheckBox, int>();

        public static void OnNewButtonClick(object sender, RoutedEventArgs e)
        {
           
        }

        public static void OnRemoveButtonClick(object sender, RoutedEventArgs e)
        {
           
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
                var id = checkBoxToId[(CheckBox)sender];
                //ChangeItemCompletetion(id, true);
                //ReadData();
            };

            checkBox.Unchecked += (sender, e) =>
            {
                var id = checkBoxToId[(CheckBox)sender];
                //ChangeItemCompletetion(id, false);
                //ReadData();
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


            return border;
        }
    }
}
