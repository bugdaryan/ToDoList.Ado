using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Data;
using ToDoList.Data.Models;

namespace ToDoList.UI
{
    public class Service
    {
        private readonly IToDoList _toDoListService;

        public Service(IToDoList toDoListService)
        {
            _toDoListService = toDoListService;
        }

        public IEnumerable<ToDoItem> GetAll()
        {
            return _toDoListService.GetAll();
        }

        public void PostItem(ToDoItem item)
        {
            _toDoListService.PostItem(item);
        }

        public void RemoveItem(int id)
        {
            _toDoListService.RemoveItem(id);
        }

        public void ChangeItemCompletetion(int id, bool completed)
        {
            _toDoListService.ChangeItemCompletetion(id, completed);
        }
    }
}
