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
            file.Close();
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

        public void UpdateEntry(string tableName, string columnName, int index, string newValue)
        {

            var table = (List<string>)ReadTable(tableName);

            var tableIndex = table.IndexOf("!" + columnName);
            tableIndex++;

            while (tableIndex<table.Count && !table[tableIndex].StartsWith("!"))
            {
                if (table[tableIndex].StartsWith(index.ToString()))
                {
                    table[tableIndex] = index.ToString() + "-" + newValue;
                    break;
                }

                tableIndex++;
            }

            tableName = tableName + ".TB";
            UpdateTable(tableName, table);
        }

        public void InsertEntries(string tableName, string columnName, int index, List<string> newEntries)
        {

            var table = (List<string>)ReadTable(tableName);

            var tableIndex = table.IndexOf("!" + columnName);
            tableIndex++;
            int i = 0;

            while (tableIndex < table.Count && !table[tableIndex].StartsWith("!"))
            {
                if (table[tableIndex].StartsWith(index.ToString()))
                {
                    break;
                }

                tableIndex++;
                i++;
            }

            foreach (var entry in newEntries)
            {
                table.Insert(tableIndex, $"{i}-{entry}");
                i++;
                tableIndex++;
            }

            while (tableIndex < table.Count && !table[tableIndex].StartsWith("!"))
            {
                table[tableIndex] = i + "-" + table[tableIndex].Split(new char[] {'-'}, 2)[1];
                i++;
                tableIndex++;
            }

            tableName = tableName + ".TB";
            UpdateTable(tableName, table);
        }

        public void DeleteColumn(string tableName, string columnName)
        {
            var table = (List<string>)ReadTable(tableName);
            var tableIndex = table.IndexOf("!" + columnName);
            table.RemoveAt(tableIndex);

            while (tableIndex < table.Count && !table[tableIndex].StartsWith("!"))
            {
                table.RemoveAt(tableIndex);
            }

            tableName = tableName + ".TB";
            UpdateTable(tableName, table);
        }

        public void DeleteEntries(string tableName, string columnName, int startIndex, int stopIndex)
        {
            if (stopIndex < startIndex)
            {
                return;
            }

            var table = (List<string>)ReadTable(tableName);
            var tableIndex = table.IndexOf("!" + columnName);
            tableIndex++;
            int i = 0;

            while (tableIndex < table.Count && !table[tableIndex].StartsWith("!") && startIndex<=stopIndex)
            {
                if (table[tableIndex].StartsWith(startIndex.ToString()))
                {
                    table.RemoveAt(tableIndex);
                    startIndex++;
                }
                else
                {
                    tableIndex++;
                    i++;
                }
            }

            while (tableIndex < table.Count && !table[tableIndex].StartsWith("!"))
            {
                table[tableIndex] = i + "-" + table[tableIndex].Split(new char[] { '-' }, 2)[1];
                i++;
                tableIndex++;
            }

            tableName = tableName + ".TB";
            UpdateTable(tableName, table);
        }

        public List<string> GetColumnNames(string tableName)
        {
            List<string> table = (List<string>)ReadTable(tableName);
            List<string> columns = new List<string>();
            foreach (string line in table)
            {
                if (line.StartsWith("!")) {
                    columns.Add(line.Substring(1));
                }
            }
            return columns;
        }

        public List<string> GetTableNames()
        {
            return DatabaseTables.Keys.ToList<string>();
        }
    }
}
