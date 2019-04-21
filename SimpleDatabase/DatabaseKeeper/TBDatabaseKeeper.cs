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

            tableName = tableName + ".TB";
            var path = DatabasesList[databaseName] + tableName;

            var file = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var writer = new StreamWriter(file);
            foreach (var column in columns)
            {
                writer.WriteLine("!"+column);
            }
            
            writer.Flush();
            file.Close();

            try
            {
                DatabaseTables[databaseName].Add(tableName);
            }
            catch (KeyNotFoundException ex)
            {
                DatabaseTables = new Dictionary<string, List<string>>();
                DatabaseTables[databaseName] = new List<string>();
                DatabaseTables[databaseName].Add(tableName);
            }
            
        }

        public void UpdateTable(string tableName, object table)
        {
            var TBtable = (List<string>) table;

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
            tableName = tableName + ".TB";
            if (!DatabasesList.ContainsKey(databaseName))
                throw new Exception("Database Not Loaded!");
            if (!DatabaseTables[databaseName].Contains(tableName))
                throw new Exception("Table dose not exist!");

            var path = DatabasesList[databaseName] + tableName;

            File.Delete(path);
        }

        public object ReadTable(string tableName)
        {
            tableName = tableName + ".TB";
            var path = DatabasesList[databaseName] + tableName;
            var TBtable= new List<string>();
            using (StreamReader file = File.OpenText(path))
                while (!file.EndOfStream)
                {
                    TBtable.Add(file.ReadLine());
                }

            return TBtable;
        }

        public void RenameTable(string oldTableName, string newTableName)
        {
            oldTableName = oldTableName + ".TB";
            newTableName = newTableName + ".TB";

            var path = DatabasesList[databaseName] + oldTableName;
            var newpath = DatabasesList[databaseName] + newTableName;

            File.Move(path, newpath);

            var oldTableIndex = DatabaseTables[databaseName].IndexOf(oldTableName);
            DatabaseTables[databaseName][oldTableIndex] = newTableName;
        }

        public void AddColumns(string tableName, List<string> columnNames)
        {
            tableName = tableName + ".TB";

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

        public List<string> ReadColumn(string tableName, string columnName)
        {
            var table = (List<string>)ReadTable(tableName);

            var columnIndex = table.IndexOf("!" + columnName);
            var columnEntries = new List<string>();

            columnIndex++;
            while(columnIndex<table.Count && !table[columnIndex].StartsWith("!"))
            {
                columnEntries.Add(table[columnIndex].Split(new char[] { '-' }, 2)[1]);
                columnIndex++;
            }

            return columnEntries;
        }

        public void AddEntries(string tableName, string columnName, List<string> entriesList)
        {
            tableName = tableName + ".TB";

            if (!DatabasesList.ContainsKey(databaseName))
                throw new Exception("Database Not Loaded!");
            if (!DatabaseTables[databaseName].Contains(tableName))
                throw new Exception("Table dose not exist!");

            var path = DatabasesList[databaseName] + tableName;
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
