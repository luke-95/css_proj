using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseKeeper
{
    class Database
    {
        private List<Table> tables;
        public List<string> GetTableNames()
        {
            return new List<string>();
        }

        private Database()
        {

        }

        public static Database FromFile(string path)
        {
            return new Database();
        }

        public void CreateTable(string name)
        {

        }

        public void SaveTable(Table table)
        {

        }


    }
}
