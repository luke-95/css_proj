using DatabaseKeeper;
using SimpleDatabase.Models;
using System.Collections.Generic;

namespace SimpleDatabase.Controllers
{

    public class DatabaseController
    {
        public const string DEFAULT_DATABASE_PATH = @"F:\Facultate\Master_1\CSS\databases";

        private readonly IDatabaseKeeper keeper;
        private readonly DataKeeper dataKeeper;

        private SimpleDatabaseModel _importedDatabaseModel;

        internal SimpleDatabaseModel ImportedDatabaseModel { get => _importedDatabaseModel; set => _importedDatabaseModel = value; }

        public DatabaseController(IDatabaseKeeper keeper, DataKeeper dataKeeper)
        {
            this.keeper = keeper;
            this.dataKeeper = dataKeeper;
        }

        public void CreateDatabase(string DatabaseName, string DatabasePath = DEFAULT_DATABASE_PATH)
        {
            dataKeeper.CreateDatabase(DatabaseName, DatabasePath);
            //dataKeeper.DatabasesList.Remove(DatabaseName);
            LoadDatabase(DatabaseName, DatabasePath);
        }

        public void CreateEmptyTable(string TableName, List<string> columnNames)
        {
            dataKeeper.CreateTable(TableName, columnNames);

            Dictionary<string, List<string>> columnData = new Dictionary<string, List<string>>();
            foreach(string columnName in columnNames)
            {
                columnData[columnName] = new List<string>();
            }
            ImportedDatabaseModel.Tables.Add(new TableModel(TableName, columnData));
        }

        public void SaveTable(string tableName, List<string> data)
        {
            dataKeeper.UpdateTable($"{tableName}.TB", data);
        }

        public void DeleteTable(string tableName)
        {
            dataKeeper.DeleteTable(tableName);
            ImportedDatabaseModel.DeleteTable(tableName);
        }


        public void AddEntries(string tableName, Dictionary<string, List<string>> columnsAndValues)
        {
            foreach (var column in columnsAndValues.Keys)
            {
                dataKeeper.AddEntries(tableName, column, columnsAndValues[column]);
            }
        }

        
        public List<string> GetTableNames(string DatabaseName)
        {
            List<string> tableNames = new List<string>();
            foreach (string name in dataKeeper.DatabaseTables[DatabaseName])
            {
                tableNames.Add(StripTableName(name));
            }
            return tableNames;
        }

        public List<string> GetColumnNames(string TableName)
        {
            return keeper.GetColumnNames(TableName);
        }

        public void LoadDatabase(string DatabaseName, string DatabasePath = DEFAULT_DATABASE_PATH)
        {
            dataKeeper.LoadDatabase(DatabaseName, DatabasePath);
            dataKeeper.SelectDatabase(DatabaseName);
            ImportedDatabaseModel = SelectedDatabaseToModel();
        }

        public List<string> GetDatabaseNames()
        {
            List<string> DatabaseNames = new List<string>(dataKeeper.DatabasesList.Keys);
            return DatabaseNames;
        }

        private SimpleDatabaseModel SelectedDatabaseToModel()
        {
            if (dataKeeper.DatabaseTables.Keys.Count > 0)
            {
                List<TableModel> tableModels = new List<TableModel>();
                foreach (string tableName in dataKeeper.GetTableNames())
                {
                    // Remove ".TB"
                    string curatedTableName = StripTableName(tableName);

                    // --- Load columns in a dict
                    List<string> columnNames = keeper.GetColumnNames(curatedTableName);
                    Dictionary<string, List<string>> columnData = new Dictionary<string, List<string>>();
                    foreach (string columnName in columnNames)
                    {
                        List<string> columnValues = dataKeeper.ReadColumn(curatedTableName, columnName);
                        columnData[columnName] = columnValues;
                    }
                    // --- Convert dict to TableModel
                    tableModels.Add(new TableModel(curatedTableName, columnData));
                }
                return SimpleDatabaseModel.WithTables(tableModels);
            }
            // Return empty model
            return SimpleDatabaseModel.WithNoData();
        }

        private string StripTableName(string tableName)
        {
            if (tableName.Contains(".TB"))
            {
                tableName = tableName.Substring(0, tableName.Length - 3);
            }
            return tableName;
        }
    }
}
