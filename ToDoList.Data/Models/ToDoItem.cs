using System;
using ToDoList.Data.Enums;

namespace ToDoList.Data.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public DateTime Created { get; set; }
        public bool Completed { get; set; }
    }
}
