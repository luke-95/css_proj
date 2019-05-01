using DatabaseKeeper;
using System.Collections.Generic;

namespace SimpleDatabase.Controllers
{

    public class DatabaseController
    {
        private static DatabaseController instance;

        private IDatabaseKeeper keeper;
        private DataKeeper dataKeeper;

        public DatabaseController(IDatabaseKeeper keeper, DataKeeper dataKeeper)
        {
            this.keeper = keeper;
            this.dataKeeper = dataKeeper;
        }


        public List<string> GetTableNames(string DatabaseName)
        {
            List<string> tableNames = new List<string>();
            foreach (string name in dataKeeper.DatabaseTables[DatabaseName])
            {
                // Remove extension, if need be.
                string parsedName = name;
                if (name.Contains(".TB"))
                {
                    parsedName = name.Substring(0, name.Length - 3);
                }
                tableNames.Add(parsedName);
            }
            return tableNames;
        }

        public List<string> GetColumnNames(string TableName)
        {
            return keeper.GetColumnNames(TableName);
        }

        public void ImportDatabase(string DatabaseName, string DatabasePath)
        {
            dataKeeper.LoadDatabase(DatabaseName, DatabasePath);
            dataKeeper.SelectDatabase(DatabaseName);
        }

        private List<string> ReloadTableNames(string DatabaseName)
        {
            // --- Load Table Names
            List<string> tableNames = dataKeeper.DatabaseTables[DatabaseName];
            TableNamesComboBox.ItemsSource = null;
            TableNamesComboBox.ItemsSource = tableNames;

            // --- Select first table
            if (tableNames.Count > 0 && TableNamesComboBox.SelectedIndex == -1)
            {
                TableNamesComboBox.SelectedIndex = 0;
                selectedTable = tableNames[TableNamesComboBox.SelectedIndex];
                selectedTable = selectedTable.Substring(0, selectedTable.Length - 3);
            }
            else
            {
                selectedTable = null;
            }
        }

        public List<string> GetDatabaseNames()
        {
            List<string> DatabaseNames = new List<string>(dataKeeper.DatabasesList.Keys);
            return DatabaseNames;
        }
    }
}
