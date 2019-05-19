using System.Collections.Generic;
using System.Diagnostics;
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

        public virtual void CreateDatabase(string databaseName, string path)
        {
            Debug.Assert(databaseName != null && databaseName.Length > 0, "Empty database name!");
            Debug.Assert(path != null && path.Length > 0, "Empty database path!");
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

        public virtual void LoadDatabase(string databaseName, string path)
        {
            if (!DatabasesList.ContainsKey(databaseName))
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
            Debug.Assert(DatabasesList.ContainsKey(databaseName), "Failed to load database!");
        }

        public virtual void SelectDatabase(string databaseName)
        {
            this.databaseName = databaseName;
            keeper.SetDatabase(DatabaseTables, DatabasesList, databaseName);
        }

        public object ReadTable(string tableName)
        {
            Debug.Assert(tableName != null && tableName.Length > 0, "Empty table name!");
            return keeper.ReadTable(tableName);
        }

        public virtual void CreateTable(string tableName, List<string> columns)
        {
            Debug.Assert(tableName != null && tableName.Length > 0, "Empty table name!");
            keeper.CreateTable(tableName, columns);
        }

        public virtual void UpdateTable(string tableName, object table)
        {
            Debug.Assert(tableName != null && tableName.Length > 0, "Empty table name!");
            Debug.Assert(table != null, "Null table object!");
            keeper.UpdateTable(tableName, table);
        }

        public virtual void DeleteTable(string tableName)
        {
            Debug.Assert(tableName != null && tableName.Length > 0, "Empty table name!");
            keeper.DeleteTable(tableName);
            this.DatabaseTables[databaseName].Remove(tableName + ".TB");
        }

        public void AddColumns(string tableName, List<string> columnNames)
        {
            Debug.Assert(tableName != null && tableName.Length > 0, "Empty table name!");
            keeper.AddColumns(tableName, columnNames);
        }

        public virtual void AddEntries(string tableName, string columnName, List<string> entriesList)
        {
            Debug.Assert(tableName != null && tableName.Length > 0, "Empty table name!");
            keeper.AddEntries(tableName, columnName, entriesList);
        }
        public List<string> ReadColumn(string tableName, string columnName)
        {
            Debug.Assert(tableName != null && tableName.Length > 0, "Empty table name!");
            return keeper.ReadColumn(tableName, columnName);
        }

        public void UpdateEntry(string tableName, string columnName, int index, string newValue)
        {
            Debug.Assert(tableName != null && tableName.Length > 0, "Empty table name!");
            Debug.Assert(columnName != null && columnName.Length > 0, "Empty column name!");

            Debug.Assert(index > 0, "Invalid Index!");
            keeper.UpdateEntry(tableName, columnName, index, newValue);
        }

        public void InsertEntries(string tableName, string columnName, int index, List<string> newEntries)
        {
            Debug.Assert(tableName != null && tableName.Length > 0, "Empty table name!");
            Debug.Assert(columnName != null && columnName.Length > 0, "Empty column name!");

            Debug.Assert(index > 0, "Invalid Index!");
            keeper.InsertEntries(tableName, columnName, index, newEntries);
        }

        public virtual void DeleteEntries(string tableName, string columnName, int startIndex, int stopIndex)
        {
            Debug.Assert(tableName != null && tableName.Length > 0, "Empty table name!");
            Debug.Assert(columnName != null && columnName.Length > 0, "Empty column name!");

            Debug.Assert(startIndex > 0, "Invalid start Index!");
            Debug.Assert(stopIndex > 0, "Invalid stop Index!");

            keeper.DeleteEntries(tableName, columnName, startIndex, stopIndex);
        }

        public void DeleteColumn(string tableName, string columnName)
        {
            Debug.Assert(tableName != null && tableName.Length > 0, "Empty table name!");
            Debug.Assert(columnName != null && columnName.Length > 0, "Empty column name!");

            keeper.DeleteColumn(tableName, columnName);
        }

        public virtual Dictionary<string, List<string>> SelectData(string tableName, string op, string valueCmp)
        {
            Debug.Assert(tableName != null && tableName.Length > 0, "Empty table name!");

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

        public virtual List<string> GetTableNames()
        {
            return keeper.GetTableNames();
        }
    }
}