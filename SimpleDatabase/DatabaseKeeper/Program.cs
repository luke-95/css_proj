using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseKeeper
{
    class Program
    {
        static void Main(string[] args)
        {
            DataKeeper dk=new DataKeeper();
            var columns = new List<string>();
            columns.AddRange(new []{"Col1","Col2","Col3"});

            //dk.CreateDatabase("AJsonDB", @"C:\scrap");
            dk.LoadDatabase("AJsonDB", @"C:\scrap");
            dk.SelectDatabase("AJsonDB");

            dk.CreateTable("MyFirstTable",columns);
            //dk.DeleteTable("MyFirstTable");
            var table = dk.ReadTable("MyFirstTable");
        }
    }
}
