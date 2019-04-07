using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseManagement;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;

namespace DatabaseApp
{
    public class DatabaseManager : IDatabaseManager
    {
        private FirebaseClient client;
        public DatabaseManager(string secret, string connString)
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = secret,
                BasePath = connString
            };

            client = new FirebaseClient(config);
        }

        public async Task CreateTable(string tableName, List<string> columnsList)
        {
            var emptyEntry = new List<string>();
            emptyEntry.Add("");
            foreach (var column in columnsList)
            {
                await client.SetAsync($"{tableName}/{column}",emptyEntry);
            }
            
        }

        
        public async Task CreateOrUpdateColumn(string tableName, string columnName, List<string> entries)
        {
            await client.SetAsync($"{tableName}/{columnName}", entries.ToArray());
        }


        
        public async Task AppendToColumn(string tableName, string columnName, List<string> entries)
        {
            var response = await client.GetAsync($"{tableName}/{columnName}");
            var existingEntries = response.ResultAs<List<string>>();
            existingEntries.AddRange(entries);
            await client.SetAsync($"{tableName}/{columnName}", existingEntries);
        }

        public async Task UpdateEntry(string tableName, string columnName,string index,string value)
        {
            await client.SetAsync($"{tableName}/{columnName}/{index}", value);
        }

        public async Task UpdateEntries(string tableName, string columnName, List<string> oldEntriesValues, List<string> newEntriesValues)
        {
            if(oldEntriesValues.Count!=newEntriesValues.Count)
            {
                throw new Exception("Every old entry needs a new entry");
            }
            var response = await client.GetAsync($"{tableName}/{columnName}");
            var existingEntries = response.ResultAs<List<string>>();

            for (int index = 0;index < existingEntries.Count;index++)
            {
                for(int i=0;i<oldEntriesValues.Count;i++)
                {
                    if(existingEntries[index]==oldEntriesValues[i])
                    {
                        existingEntries[index] = newEntriesValues[i];
                        break;
                    }
                }
            }
            await client.SetAsync($"{tableName}/{columnName}", existingEntries);
        }
        
        public async Task<IDictionary<string, List<string>>> ReadTable(string tableName)
        {
            var response = await client.GetAsync(tableName);
            return response.ResultAs<IDictionary<string, List<string>>>();
        }
        
        public async Task DeleteTable(string tableName)
        {
            await client.DeleteAsync(tableName);
        }

        public async Task DeleteColumn(string tableName, string columnName)
        {
            await client.DeleteAsync($"{tableName}/{columnName}");
        }

        public async Task DeleteEntry(string tableName, string columnName, string index)
        {
            await client.DeleteAsync($"{tableName}/{columnName}/{index}");
        }
        
    }
}
