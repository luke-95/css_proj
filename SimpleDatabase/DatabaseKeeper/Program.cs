using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Importer;

namespace DatabaseKeeper
{
    class Program
    {
        static void Main(string[] args)
        {
            CLIParser.Main2(args);
            //JsonDatabaseKeeper keeper= new JsonDatabaseKeeper();
            TBDatabaseKeeper keeper = new TBDatabaseKeeper();
            DataKeeper dk=new DataKeeper(keeper);
            var columns = new List<string>();
            var ncolumns = new List<string>();
            var values = new List<string>();
            var nvalues = new List<string>();
            columns.AddRange(new []{"Col1","Col2","Col3"});
            ncolumns.AddRange(new []{"Col4","Col5"});
            values.AddRange(new []{"a1","a2","a3"});
            nvalues.AddRange(new []{"b1","b2","b3"});

            //dk.CreateDatabase("AJsonDB", @"C:\scrap");
            dk.LoadDatabase("AJsonDB", @"C:\scrap");
            dk.SelectDatabase("AJsonDB");

            //dk.CreateTable("MyFirstTable",columns);
            //dk.DeleteTable("MyFirstTable");
            //var table = dk.ReadTable("MyFirstTable");
            //dk.AddEntries("MyFirstTable", "Col1", values);
            //dk.AddColumns("MyFirstTable", ncolumns);
            //dk.UpdateEntry("MyFirstTable","Col1",1,"c2");
            //dk.InsertEntries("MyFirstTable", "Col1",1,nvalues);
            //var columnEntries = dk.ReadColumn("MyFirstTable", "Col1");
            //dk.DeleteColumn("MyFirstTable","Col1");
            dk.DeleteEntries("MyFirstTable","Col1",2,3);

            //Importer.Importer importer= new Importer.Importer();
            //var parsedcsv = importer.ReadCsv(@"C:\scrap\AJsonDB\exampleCSV.csv");
        }
    }
}
