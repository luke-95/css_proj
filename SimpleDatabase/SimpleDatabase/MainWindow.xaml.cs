using DatabaseKeeper;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using Importer;
using SimpleDatabase.Controllers;
using SimpleDatabase.Models;

namespace SimpleDatabase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatabaseController databaseController;

        private TableModel SelectedTable => databaseController.ImportedDatabaseModel.GetTable(SelectedTableName);
        private string SelectedTableName => TableNamesComboBox.SelectedValue.ToString();

        private SimpleDatabaseModel SelectedDatabase => databaseController.ImportedDatabaseModel;
        private string SelectedDatabaseName => DatabaseNamesComboBox.SelectedValue.ToString();

        private bool databaseImportInProgress = false;
        private bool databaseCreationInProgress = false;

        public MainWindow()
        {
            InitializeComponent();

            TBDatabaseKeeper tbKeeper = new TBDatabaseKeeper();
            DataKeeper dataKeeper = new DataKeeper(tbKeeper);
            InitializeController(tbKeeper, dataKeeper);
        }

        public void InitializeController(IDatabaseKeeper tbKeeper, DataKeeper dataKeeper)
        {
            databaseController = new DatabaseController(tbKeeper, dataKeeper);
        }

        public void DisplayTable(TableModel tableModel)
        {
            // --- Prepare Data to be displayed in a DataGrid
            // Create DataTable
            DataTable dataTable = new DataTable();
            // Create DataColumns
            foreach (string columnName in tableModel.ColumnNames)
            {
                DataColumn dataColumn = new DataColumn(columnName, typeof(string));
                dataTable.Columns.Add(dataColumn);
            }
            // Create DataRows
            for (int rowIndex = 0; rowIndex < tableModel.RowCount; ++rowIndex)
            {
                DataRow row = dataTable.NewRow();
                for (int columnIndex = 0; columnIndex < tableModel.ColumnNames.Count; ++columnIndex)
                {
                    string currentColumn = tableModel.ColumnNames[columnIndex];
                    if (rowIndex < tableModel.Columns[currentColumn].Count)
                    {
                        row[columnIndex] = tableModel.Columns[currentColumn][rowIndex];
                    }
                }
                dataTable.Rows.Add(row);
            }
            // --- Display in DataGrid
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void Button_ImportDB_Click(object sender, RoutedEventArgs e)
        {
            databaseImportInProgress = true;
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                InitialDirectory = DatabaseController.DEFAULT_DATABASE_PATH,
                IsFolderPicker = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string selectionPath = Path.GetDirectoryName(dialog.FileName);
                string selectedDatabase = Path.GetFileName(dialog.FileName);

                databaseController.LoadDatabase(selectedDatabase, selectionPath);

                ReloadDatabaseNames();
                ReloadTableNames();
                
                //Console.WriteLine($"SelectedDB: {selectedDatabase}");
                // Display first table in the database, if one exists
                if (databaseController.GetTableNames(selectedDatabase).Count > 0)
                {
                    DisplayTable(databaseController.ImportedDatabaseModel.Tables[0]);
                }
            }
            databaseImportInProgress = false;
        }

        private void Button_ImportCSV_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "ImageCSV (*.csv;)";

            if (openFileDialog.ShowDialog() == true)
            {
                CsvImporter csvIMporter = new CsvImporter();
                Dictionary<string, List<string>> import = csvIMporter.ReadCsv(openFileDialog.FileName);
                string tableName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);

                var columns = new List<string>();
                foreach (string column in import.Keys)
                {
                    columns.Add(column);
                }

                databaseController.CreateEmptyTable(tableName, columns);
                ReloadTableNames();
                TableNamesComboBox.SelectedIndex = TableNamesComboBox.Items.IndexOf(tableName);
                databaseController.AddEntries(SelectedTableName, import);
                DisplayTable(databaseController.ImportedDatabaseModel.GetTable(SelectedTableName));
            }
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            DataView dataView = (DataView)dataGrid.ItemsSource;
            DataTable dataTable = dataView.Table;
            List<string> tableOutputData = new List<string>();

            foreach (DataColumn column in dataTable.Columns)
            {
                // Write Column Name
                tableOutputData.Add($"!{column.ColumnName}");
                // Write Column Data
                int rowIndex = 0;
                foreach (DataRow row in dataTable.Rows)
                {
                    int currentColumnIndex = SelectedTable.ColumnNames.IndexOf(column.ColumnName);
                    string rowValue = row[currentColumnIndex].ToString();
                    tableOutputData.Add($"{rowIndex}-{rowValue}");
                    rowIndex += 1;
                }
            }
            // Save to disk
            databaseController.SaveTable(SelectedTableName, tableOutputData);
        }

        private void Button_CreateDatabase_Click(object sender, RoutedEventArgs e)
        {
            databaseCreationInProgress = true;

            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                InitialDirectory = DatabaseController.DEFAULT_DATABASE_PATH,
                IsFolderPicker = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                databaseController.CreateDatabase(DatabaseNameTextBox.Text, dialog.FileName);
                ReloadDatabaseNames();
            }
            databaseCreationInProgress = false;

        }

        private void Button_CreateTable_Click(object sender, RoutedEventArgs e)
        {
            databaseController.CreateEmptyTable(TableNameTextBox.Text, new List<string>());
            ReloadTableNames();
        }

        private void Button_AddColumn_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.ItemsSource != null)
            {
                DataTable table = ((DataView)dataGrid.ItemsSource).Table;
                string newColumnName = ColumnNameTextBox.Text;
                if (!table.Columns.Contains(newColumnName))
                {
                    table.Columns.Add(new DataColumn(newColumnName, typeof(string)));
                    databaseController.ImportedDatabaseModel.GetTable(SelectedTableName).AddColumn(newColumnName, new List<string>());

                    dataGrid.ItemsSource = null;
                    dataGrid.Items.Refresh();
                    dataGrid.ItemsSource = table.DefaultView;
                    dataGrid.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Column name already exists.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        public void SetComboBoxItems(ComboBox comboBox, List<string> ItemsSource)
        {
            if (ItemsSource.Count > 0)
            {
                // Force ItemSource reload.
                comboBox.ItemsSource = null;
                // Set ItemsSource
                comboBox.ItemsSource = ItemsSource;

                //If no item is selected, select first item.
                if (comboBox.SelectedValue == null)
                {
                    comboBox.SelectedIndex = 0;
                }
            }
        }

        public void ReloadTableNames()
        {
            SetComboBoxItems(TableNamesComboBox, databaseController.GetTableNames(SelectedDatabaseName));
        }

        public void ReloadDatabaseNames()
        {
            SetComboBoxItems(DatabaseNamesComboBox, databaseController.GetDatabaseNames());
        }


        private void DatabaseNamesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DatabaseNamesComboBox.SelectedValue != null)
            {
                if (!databaseImportInProgress && !databaseCreationInProgress)
                {
                    databaseController.LoadDatabase(SelectedDatabaseName);
                }

                if (SelectedDatabase.TableNames.Count > 0)
                {
                    DisplayTable(SelectedDatabase.Tables[0]);
                }
            }
        }

        private void TableNamesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TableNamesComboBox.SelectedValue != null)
            {
                DisplayTable(SelectedTable);
            }
        }

        private void Button_DeleteTable_Click(object sender, RoutedEventArgs e)
        {
            databaseController.DeleteTable(SelectedTableName);

            ReloadTableNames();
            if (SelectedDatabase.TableNames.Count > 0)
            {
                TableNamesComboBox.SelectedIndex = 0;
            }
        }
    }
}
