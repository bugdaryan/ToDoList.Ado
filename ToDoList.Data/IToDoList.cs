using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Data
{
    public interface IToDoList
    {
        void GetAll();
        void PostItem();
        void RemoveItem(int id);
        void ChangeItemCompletetion(int id, bool completed);
    }
}
