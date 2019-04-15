using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDatabase.Models
{
    public class Table
    {
        private readonly Dictionary<string, Type> _columns;

        public Dictionary<string, Type> Columns => _columns;

        public Table()
        {
            _columns = new Dictionary<string, Type>();
        }

        public void AddColumn(String name, Type type) 
        {
            _columns.Add(name, type);
        }

        public void RemoveColumn(String name)
        {
            _columns.Remove(name);
        }
    }
}
