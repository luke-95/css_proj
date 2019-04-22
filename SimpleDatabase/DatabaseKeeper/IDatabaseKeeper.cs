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
        void RenameTable(string oldTableName, string newTableName);
        object ReadTable(string tableName);
        void AddColumns(string tableName, List<string> columnNames);
        void AddEntries(string tableName, string columnName, List<string> entriesList);
        List<string> ReadColumn(string tableName, string columnName);
        void UpdateEntry(string tableName, string columnName, int index, string newValue);
        void InsertEntries(string tableName, string columnName, int index, List<string> newEntries);

        List<string> GetColumnNames(string tableName);
    }
}