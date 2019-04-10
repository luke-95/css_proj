using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDatabase.Models
{
    public class Column<T>
    {
        private List<T> _values;
        private string _name;

        public string Name { get => _name; set => _name = value; }
        public List<T> Values { get => _values; set => _values = value; }
    }
}
