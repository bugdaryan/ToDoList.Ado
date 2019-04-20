using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.UI
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

    public enum Priority
    {
        Zero,
        Low,
        Medium,
        Important,
        Critical
    }
}
