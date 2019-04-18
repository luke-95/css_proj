using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseKeeper
{
    public class TBDatabaseKeeper : IDatabaseKeeper
    {
        public Dictionary<string, List<string>> DatabaseTables;
        public Dictionary<string, string> DatabasesList;
        public string databaseName;

        public void SetDatabase(Dictionary<string, List<string>> databaseTables, Dictionary<string, string> databasesList, string databaseName)
        {
            this.DatabaseTables = databaseTables;
            this.DatabasesList = databasesList;
            this.databaseName = databaseName;
        }

        public void CreateTable(string tableName, List<string> columns)
        {
            if (!DatabasesList.ContainsKey(databaseName))
                throw new Exception("Database Not Loaded!");

            var path = DatabasesList[databaseName] + tableName + ".TB";

            var file = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var writer = new StreamWriter(file);
            foreach (var column in columns)
            {
                writer.WriteLine("!"+column);
            }
            
            writer.Flush();
            file.Close();

            DatabaseTables[databaseName].Add(tableName);
        }

        public void UpdateTable(string tableName, object table)
        {
            var TBtable = (List<string>) table;
            tableName += ".TB";

            if (!DatabasesList.ContainsKey(databaseName))
                throw new Exception("Database Not Loaded!");
            if (!DatabaseTables[databaseName].Contains(tableName))
                throw new Exception("Table dose not exist!");

            var path = DatabasesList[databaseName] + tableName;

            var file = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var writer = new StreamWriter(file);
            foreach (var value in TBtable)
            {
                writer.WriteLine(value);
            }
            writer.Flush();
        }

        public void DeleteTable(string tableName)
        {
            if (!DatabasesList.ContainsKey(databaseName))
                throw new Exception("Database Not Loaded!");
            if (!DatabaseTables[databaseName].Contains(tableName))
                throw new Exception("Table dose not exist!");

            var path = DatabasesList[databaseName] + tableName + ".TB";

            File.Delete(path);
        }

        public object ReadTable(string tableName)
        {
            var path = DatabasesList[databaseName] + tableName + ".TB";
            var TBtable= new List<string>();
            using (StreamReader file = File.OpenText(path))
                while (!file.EndOfStream)
                {
                    TBtable.Add(file.ReadLine());
                }

            return TBtable;
        }

        public void AddColumns(string tableName, List<string> columnNames)
        {
            if (!DatabasesList.ContainsKey(databaseName))
                throw new Exception("Database Not Loaded!");
            if (!DatabaseTables[databaseName].Contains(tableName))
                throw new Exception("Table dose not exist!");

            var path = DatabasesList[databaseName] + tableName;
            var file = File.Open(path, FileMode.Append, FileAccess.ReadWrite);
            var writer = new StreamWriter(file);
            foreach (var columnName in columnNames)
            {
                writer.WriteLine("!"+columnName);
            }
        }

        public void AddEntries(string tableName, string columnName, List<string> entriesList)
        {
            if (!DatabasesList.ContainsKey(databaseName))
                throw new Exception("Database Not Loaded!");
            if (!DatabaseTables[databaseName].Contains(tableName))
                throw new Exception("Table dose not exist!");

            var path = DatabasesList[databaseName] + tableName + ".TB";
            var file = File.Open(path, FileMode.Open, FileAccess.ReadWrite);
            var writer = new StreamWriter(file);
            var reader = new StreamReader(file);
            var TBtable = new List<string>();

            while (!reader.EndOfStream)
            {
                TBtable.Add(reader.ReadLine());
            }

            file.Close();

            var index = TBtable.IndexOf("!" + columnName);
            int i = 0;
            while (index<TBtable.Count && !TBtable[index].StartsWith("!"))
            {
                index++;
                i++;
            }

            foreach (var entry in entriesList)
            {
                TBtable.Insert(index+1, $"{i}-{entry}");
                i++;
                index++;
            }
            UpdateTable(tableName,TBtable);
        }
    }
}
