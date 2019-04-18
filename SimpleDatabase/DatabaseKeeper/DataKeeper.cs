

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DatabaseKeeper
{
    public class DataKeeper
    {
        private Dictionary<string, string> DatabasesList;
        private Dictionary<string, List<string>> DatabaseTables;
        private string databaseName;

        public DataKeeper()
        {
            DatabasesList=new Dictionary<string, string>();
            DatabaseTables = new Dictionary<string, List<string>>();
        }

        public void CreateDatabase(string path)
        {
            FileInfo file = new FileInfo(path+$"\\{databaseName}\\{databaseName}.txt");
            file.Directory.Create();
            DatabasesList.Add(databaseName, path+$"\\{databaseName}\\");
        }

        // Dangerous Method !UNTESTED!
        /*
        public void DeleteDatabase(string path)
        {
            FileInfo file = new FileInfo(path + $"\\{databaseName}\\{databaseName}.txt");
            file.Directory.Delete();
            DatabasesList.Remove(databaseName);
            databaseName = "";
        }*/

        public void LoadDatabase(string databaseName, string path)
        {
            DatabasesList.Add(databaseName,path+$"\\{databaseName}\\");

            DirectoryInfo directory = new DirectoryInfo(path + $"\\{databaseName}");
            FileInfo[] files = directory.GetFiles();

            DatabaseTables.Add(databaseName,new List<string>());
            foreach (var table in files)
            {
                DatabaseTables[databaseName].Add(table.Name);
            }
            
        }

        public void SelectDatabase(string databaseName)
        {
            this.databaseName = databaseName;
        }

        public void CreateTable(string tableName, List<string> columns)
        {
            if(!DatabasesList.ContainsKey(databaseName))
                throw new Exception("Database Not Loaded!");
            
            JObject table = new JObject(
                from column in columns select new JProperty(column,new JArray())
                );

            var path = DatabasesList[databaseName]+tableName+".json";

            var file = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var writer = new StreamWriter(file);
            writer.Write(table.ToString());
            writer.Flush();
            file.Close();

            DatabaseTables[databaseName].Add(tableName);
        }

        public void UpdateTable(string tableName, JObject table)
        {
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

        public JObject ReadTable(string tableName)
        {
            try
            {
                JObject table;

                var path = DatabasesList[databaseName] + tableName + ".json";

                using (StreamReader file = File.OpenText(path))
                using (JsonTextReader jreader = new JsonTextReader(file))
                {
                    table = (JObject) JToken.ReadFrom(jreader);
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
            var table = ReadTable(tableName);

            foreach (var column in columnNames)
            {
                table.Add(new JProperty(column, new JArray()));
                UpdateTable(tableName, table);
            }

        }

        public void AddEntries(string tableName, string columnName, List<string> entriesList)
        {
            var table = ReadTable(tableName);
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
            UpdateTable(tableName,table);
        }

        
    }
}
