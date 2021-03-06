﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Data.Models;

namespace ToDoList.Data
{
    public interface IToDoList
    {
        IList<ToDoItem> GetAll(string sortQuery);
        void PostItem(ToDoItem item);
        void ModifyItem(ToDoItem item);
        void RemoveItem(int id);
        void RemoveCompletedItems();
        void RemoveAllItems();
        void ChangeItemCompletetion(int id, bool completed);
    }
}
