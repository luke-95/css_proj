using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using Newtonsoft.Json;

namespace DatabaseApp
{
    public class DatabaseManager
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
        
        public async Task<IDictionary<string, List<string>>> ReadTable(string tableName)
        {
            var response = await client.GetAsync(tableName);
            return response.ResultAs<IDictionary<string, List<string>>>();
        }

        /*
        public async Task DeleteTable(string tableName)
        {
            await client.Child(tableName).DeleteAsync();
        }

        public async Task DeleteColumn(string tableName, string columnName)
        {
            await client.Child(tableName).Child(columnName).DeleteAsync();
        }
        */
    }
}
