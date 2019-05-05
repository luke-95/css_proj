using System.Collections.Generic;
using System.IO;

namespace DatabaseKeeper
{
    public class DataKeeper
    {
        public Dictionary<string, string> DatabasesList;
        public Dictionary<string, List<string>> DatabaseTables;
        public string databaseName;
        public IDatabaseKeeper keeper;

        public DataKeeper(IDatabaseKeeper keeper)
        {
            DatabasesList = new Dictionary<string, string>();
            DatabaseTables = new Dictionary<string, List<string>>();
            this.keeper = keeper;
        }

        public void CreateDatabase(string databaseName, string path)
        {
            this.databaseName = databaseName;
            FileInfo file = new FileInfo(path + $"\\{databaseName}\\{databaseName}.txt");
            file.Directory.Create();
            DatabasesList.Add(databaseName, path + $"\\{databaseName}\\");
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
            DatabasesList.Add(databaseName, path + $"\\{databaseName}\\");

            DirectoryInfo directory = new DirectoryInfo(Path.Combine(path, databaseName));
            FileInfo[] files = directory.GetFiles();

            DatabaseTables.Add(databaseName, new List<string>());
            foreach (var table in files)
            {
                DatabaseTables[databaseName].Add(table.Name);
            }

        }

        public void SelectDatabase(string databaseName)
        {
            this.databaseName = databaseName;
            keeper.SetDatabase(DatabaseTables, DatabasesList, databaseName);
        }

        public object ReadTable(string tableName)
        {
            return keeper.ReadTable(tableName);
        }

        public void CreateTable(string tableName, List<string> columns)
        {
            keeper.CreateTable(tableName, columns);
        }

        public void UpdateTable(string tableName, object table)
        {
            keeper.UpdateTable(tableName, table);
        }

        public void DeleteTable(string tableName)
        {
            keeper.DeleteTable(tableName);
            this.DatabaseTables[databaseName].Remove(tableName + ".TB");
        }

        public void AddColumns(string tableName, List<string> columnNames)
        {
            keeper.AddColumns(tableName, columnNames);
        }

        public void AddEntries(string tableName, string columnName, List<string> entriesList)
        {
            keeper.AddEntries(tableName, columnName, entriesList);
        }
        public List<string> ReadColumn(string tableName, string columnName)
        {
            return keeper.ReadColumn(tableName, columnName);
        }

        public void UpdateEntry(string tableName, string columnName, int index, string newValue)
        {
            keeper.UpdateEntry(tableName, columnName, index, newValue);
        }

        public void InsertEntries(string tableName, string columnName, int index, List<string> newEntries)
        {
            keeper.InsertEntries(tableName, columnName, index, newEntries);
        }

        public void DeleteEntries(string tableName, string columnName, int startIndex, int stopIndex)
        {
            keeper.DeleteEntries(tableName, columnName, startIndex, stopIndex);
        }

        public void DeleteColumn(string tableName, string columnName)
        {
            keeper.DeleteColumn(tableName, columnName);
        }

        public Dictionary<string, List<string>> SelectData(string tableName, string op, string valueCmp)
        {
            List<string> columnNames = keeper.GetColumnNames(tableName);
            Dictionary<string, List<string>> columnData = new Dictionary<string, List<string>>();

            foreach (string column in columnNames)
            {
                List<string> columnValues = keeper.ReadColumn(tableName, column);
                foreach (string value in columnValues)
                {
                    switch (op)
                    {
                        case "==":
                            if (value.Equals(valueCmp))
                            {
                                columnData[column] = columnValues;
                            }
                            break;
                        case "!=":
                            if (!value.Equals(valueCmp))
                            {
                                columnData[column] = columnValues;
                            }
                            break;
                        case "<":
                        case "less":
                            if (value.CompareTo(valueCmp) < 0)
                            {
                                columnData[column] = columnValues;
                            }
                            break;
                        case ">":
                        case "greater":
                            if (value.CompareTo(valueCmp) > 0)
                            {
                                columnData[column] = columnValues;
                            }
                            break;
                        case "<=":
                        case "lesseq":
                            if (value.CompareTo(valueCmp) <= 0)
                            {
                                columnData[column] = columnValues;
                            }
                            break;
                        case ">=":
                        case "greatereq":
                            if (value.CompareTo(valueCmp) >= 0)
                            {
                                columnData[column] = columnValues;
                            }
                            break;
                    }
                }
            }
            return columnData;
        }

        public List<string> GetTableNames()
        {
            return keeper.GetTableNames();
        }
    }
}