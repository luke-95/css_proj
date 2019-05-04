using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseKeeper
{
    class Table
    {
        List<string> RowList;
        Dictionary<string, List<string>> ColumnDict;


        private Table()
        {

        }

        public static Table FromFile(string path)
        {
            return new Table();
        }

        public void Export(string path)
        {

        }
    }
}
