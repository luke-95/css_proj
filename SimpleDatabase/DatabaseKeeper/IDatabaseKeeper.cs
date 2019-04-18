using System.Collections.Generic;

namespace DatabaseKeeper
{
    public interface IDatabaseKeeper
    {
        void SetDatabase(Dictionary<string, List<string>> databaseTables,
            Dictionary<string, string> databasesList,
            string databaseName);

        void CreateTable(string tableName, List<string> columns);
        void UpdateTable(string tableName, object table);
        void DeleteTable(string tableName);
        object ReadTable(string tableName);
        void AddColumns(string tableName, List<string> columnNames);
        void AddEntries(string tableName, string columnName, List<string> entriesList);
    }
}