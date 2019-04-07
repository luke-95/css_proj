using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseManagement
{
    public interface IDatabaseManager
    {
        Task CreateTable(string tableName, List<string> columnsList);
        Task CreateOrUpdateColumn(string tableName, string columnName, List<string> entries);
        Task AppendToColumn(string tableName, string columnName, List<string> entries);
        Task UpdateEntry(string tableName, string columnName, string index, string value);
        Task UpdateEntries(string tableName, string columnName, List<string> oldEntriesValues, List<string> newEntriesValues);
        Task<IDictionary<string, List<string>>> ReadTable(string tableName);
        Task DeleteTable(string tableName);
        Task DeleteColumn(string tableName, string columnName);
        Task DeleteEntry(string tableName, string columnName, string index);
    }
}
