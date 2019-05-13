using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DatabaseKeeper
{
    public class JsonDatabaseKeeper 
    {
        
        public Dictionary<string, List<string>> DatabaseTables;
        public Dictionary<string, string> DatabasesList;
        public string databaseName;
        /*

        public void CreateTable(string tableName, List<string> columns)
        {
            if (!DatabasesList.ContainsKey(databaseName))
                throw new Exception("Database Not Loaded!");

            JObject table = new JObject(
                from column in columns select new JProperty(column, new JArray())
                );

            var path = DatabasesList[databaseName] + tableName + ".json";

            var file = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var writer = new StreamWriter(file);
            writer.Write(table.ToString());
            writer.Flush();
            file.Close();

            DatabaseTables[databaseName].Add(tableName);
        }

        public void UpdateTable(string tableName, object table)
        {
            table = (JObject) table;
            tableName += ".json";
            if (!DatabasesList.ContainsKey(databaseName))
                throw new Exception("Database Not Loaded!");
            if (!DatabaseTables[databaseName].Contains(tableName))
                throw new Exception("Table dose not exist!");

            var path = DatabasesList[databaseName] + tableName;

            var file = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var writer = new StreamWriter(file);
            writer.Write(table.ToString());
            writer.Flush();
            file.Close();
        }

        public void DeleteTable(string tableName)
        {
            if (!DatabasesList.ContainsKey(databaseName))
                throw new Exception("Database Not Loaded!");
            if (!DatabaseTables[databaseName].Contains(tableName))
                throw new Exception("Table dose not exist!");

            var path = DatabasesList[databaseName] + tableName + ".json";

            File.Delete(path);
        }

        public object ReadTable(string tableName)
        {
            try
            {
                JObject table;

                var path = DatabasesList[databaseName] + tableName + ".json";

                using (StreamReader file = File.OpenText(path))
                using (JsonTextReader jreader = new JsonTextReader(file))
                {
                    table = (JObject)JToken.ReadFrom(jreader);
                }

                return table;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public void AddColumns(string tableName, List<string> columnNames)
        {
            var table = (JObject)ReadTable(tableName);

            foreach (var column in columnNames)
            {
                table.Add(new JProperty(column, new JArray()));
                UpdateTable(tableName, table);
            }

        }

        public void DeleteColumn(string tableName, string columnName)
        {
            throw new NotImplementedException();
        }

        public void AddEntries(string tableName, string columnName, List<string> entriesList)
        {
            var table = (JObject)ReadTable(tableName);
            var oldEntries = table.GetValue(columnName).Values<string>().ToList();
            var newEntries = new JArray();

            foreach (var nEntry in entriesList)
            {
                oldEntries.Add(nEntry);
            }

            if (table.Remove(columnName))
            {
                table.Add(columnName, JToken.FromObject(oldEntries));
            }
            UpdateTable(tableName, table);
        }

        public void SetDatabase(Dictionary<string, List<string>> databaseTables, Dictionary<string, string> databasesList, string databaseName)
        {
            this.DatabaseTables = databaseTables;
            this.DatabasesList = databasesList;
            this.databaseName = databaseName;
        }

        public void RenameTable(string oldTableName, string newTableName)
        {
            throw new NotImplementedException();
        }

        public List<string> ReadColumn(string tableName, string columnName)
        {
            throw new NotImplementedException();
        }

        public void UpdateEntry(string tableName, string columnName, int index, string newValue)
        {
            throw new NotImplementedException();
        }

        public void InsertEntries(string tableName, string columnName, int index, List<string> newEntries)
        {
            throw new NotImplementedException();
        }

        public void DeleteEntries(string tableName, string columnName, int startIndex, int stopIndex)
        {
            throw new NotImplementedException();
        }

        public List<string> GetColumnNames(string tableName)
        {
            throw new NotImplementedException();
        }
        public List<string> GetTableNames()
        {
            return DatabaseTables.Keys.ToList<string>();
        }
        */
    }
}
