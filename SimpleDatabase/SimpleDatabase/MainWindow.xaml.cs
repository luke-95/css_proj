﻿using DatabaseKeeper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace SimpleDatabase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string DATABASE_PATH = @"D:\Facultate\Master_1\CSSoft\databases";
        string selectedDatabase = "NewJsonDB";
        string selectedTable = "SomeTable";

        TBDatabaseKeeper keeper;
        DataKeeper dataKeeper;

        List<string> columnNames;

        public MainWindow()
        {
            InitializeComponent();
            keeper = new TBDatabaseKeeper();
            dataKeeper = new DataKeeper(keeper);

            // --- Load Database Names
            dataKeeper.LoadDatabase(selectedDatabase, DATABASE_PATH);
            dataKeeper.SelectDatabase(selectedDatabase);

            ReloadDatabaseNames();
            selectedDatabase = DatabaseNamesComboBox.SelectedValue.ToString();
        }

        private void Button_Import_Click(object sender, RoutedEventArgs e)
        {
            DisplayDatabase(selectedDatabase);
        }

        private void Button_Export_Click(object sender, RoutedEventArgs e)
        {
            DataView dataView = (DataView)dataGrid.ItemsSource;
            DataTable table = dataView.Table;
            List<string> tableOutputData = new List<string>();

            foreach (DataColumn column in table.Columns)
            {
                // Write Column Name
                Console.WriteLine(column.ColumnName);
                tableOutputData.Add($"!{column.ColumnName}");
                
                // Write Column Data
                int rowIndex = 0;
                foreach (DataRow row in table.Rows)
                {
                    int currentColumnIndex = columnNames.IndexOf(column.ColumnName);
                    string rowValue = row[currentColumnIndex].ToString();
                    bool rowValueNotEmpty = true; // rowValue != null && rowValue.Length > 0;
                    if (rowValueNotEmpty)
                    {
                        tableOutputData.Add($"{rowIndex}-{rowValue}");
                        Console.WriteLine(rowValue);
                    }
                }
            }
            dataKeeper.UpdateTable($"{selectedTable}.TB", tableOutputData);
        }

        private void Button_CreateDatabase_Click(object sender, RoutedEventArgs e)
        {
            dataKeeper.CreateDatabase(DatabaseNameTextBox.Text, DATABASE_PATH);
            ReloadDatabaseNames();
        }

        private void Button_CreateTable_Click(object sender, RoutedEventArgs e)
        {
            dataKeeper.CreateTable(TableNameTextBox.Text, new List<string>());
            ReloadTableNames();
        }

        private void Button_AddColumn_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.ItemsSource != null)
            {
                DataTable table = ((DataView)dataGrid.ItemsSource).Table;
                string newColumnName = ColumnNameTextBox.Text;
                table.Columns.Add(new DataColumn(newColumnName, typeof(string)));
                dataGrid.ItemsSource = null;
                dataGrid.Items.Refresh();
                dataGrid.ItemsSource = table.DefaultView;
                dataGrid.Items.Refresh();
            }
        }

        // Returns the max item count out of each item list in the dictionary
        private int MaxCount<T>(Dictionary<T, List<T>> dict)
        {
            int max = 0;
            foreach (List<T> list in dict.Values)
            {
                if (list.Count > max)
                {
                    max = list.Count;
                }
            }
            return max;
        }

        private void ReloadDatabaseNames()
        {
            DatabaseNamesComboBox.ItemsSource = dataKeeper.DatabasesList.Keys;
            if (dataKeeper.DatabasesList.Keys.Count > 0)
            {
                DatabaseNamesComboBox.SelectedIndex = 0;
            }

            //Console.WriteLine("Database names:");
            //foreach (string item in keeper.DatabasesList.Keys)
            //{
            //    Console.WriteLine(item);
            //}
        }

        private void DisplayDatabase(string DatabaseName)
        {
            // --- Load table names
            ReloadTableNames();

            // --- Load column names
            columnNames = keeper.GetColumnNames(selectedTable);

            // --- Load data in a dict
            Dictionary<string, List<string>> columnData = new Dictionary<string, List<string>>();
            foreach (string columnName in columnNames)
            {
                List<string> columnValues = dataKeeper.ReadColumn(selectedTable, columnName);
                columnData[columnName] = columnValues;
            }

            // --- Prepare Data to be displayed in a DataGrid
            DataTable dataTable = new DataTable();
            /// Init Columns
            foreach (string columnName in columnNames)
            {
                DataColumn dataColumn = new DataColumn(columnName, typeof(string));
                dataTable.Columns.Add(dataColumn);
            }
            /// Populate Rows
            int rowCount = MaxCount(columnData);
            for (int rowIndex = 0; rowIndex < rowCount; ++rowIndex)
            {
                DataRow row = dataTable.NewRow();
                for (int columnIndex = 0; columnIndex < columnData.Keys.Count; ++columnIndex)
                {
                    string currentColumn = columnNames[columnIndex];
                    if (rowIndex < columnData[currentColumn].Count)
                    {
                        row[columnIndex] = columnData[currentColumn][rowIndex];
                    }
                }
                dataTable.Rows.Add(row);
            }
            // --- Display in DataGrid
            dataGrid.ItemsSource = dataTable.DefaultView;
        }


        private void ReloadTableNames()
        {
            // --- Load Table Names
            List<string> tableNames = keeper.GetTableNames();
            TableNamesComboBox.ItemsSource = tableNames;
            // --- Select first table
            if (tableNames.Count > 0 && TableNamesComboBox.SelectedIndex == -1)
            {
                TableNamesComboBox.SelectedIndex = 0;
            }
            selectedTable = tableNames[TableNamesComboBox.SelectedIndex];
            selectedTable = selectedTable.Substring(0, selectedTable.Length - 3);
        }

        private void DatabaseNamesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDatabase = DatabaseNamesComboBox.SelectedValue.ToString();
            DisplayDatabase(selectedDatabase);
        }

        private void TableNamesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTable = TableNamesComboBox.SelectedValue.ToString();
            DisplayDatabase(selectedDatabase);
        }
    }
}
