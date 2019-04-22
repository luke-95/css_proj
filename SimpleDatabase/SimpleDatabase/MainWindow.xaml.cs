using DatabaseKeeper;
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
        string DATABASE_PATH = @"D:\Facultate\Master_1\CSSoft\databases";
        string DATABASE_NAME = "NewJsonDB";
        string TABLE_NAME = "SomeTable";

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Button_Import_Click(object sender, RoutedEventArgs e)
        {
            //JsonDatabaseKeeper keeper= new JsonDatabaseKeeper();
            TBDatabaseKeeper keeper = new TBDatabaseKeeper();
            DataKeeper dk = new DataKeeper(keeper);
            var columns = new List<string>();
            var new_columns = new List<string>();
            var values = new List<string>();
            columns.AddRange(new[] { "Col1", "Col2", "Col3" });
            //new_columns.AddRange(new[] { "Col4", "Col5" });
            values.AddRange(new[] { "b1", "b2", "b3" });

            //dk.CreateDatabase(DATABASE_NAME, DATABASE_PATH);
            dk.LoadDatabase(DATABASE_NAME, DATABASE_PATH);
            dk.SelectDatabase(DATABASE_NAME);



            //dk.CreateTable(TABLE_NAME, columns);
            //dk.DeleteTable(TABLE_NAME);
            var table = dk.ReadTable(TABLE_NAME);
            //dk.AddEntries(TABLE_NAME, "Col3", values);
            //dk.AddColumns(TABLE_NAME, ncolumns);
            List<string> columnNames = keeper.GetColumnNames(TABLE_NAME);
            var column1Entries = dk.ReadColumn(TABLE_NAME, "Col1");
            var column2Entries = dk.ReadColumn(TABLE_NAME, "Col2");

            Dictionary<string, List<string>> columnData = new Dictionary<string, List<string>>();

            // DataGrid - Init DataTable
            DataTable dataTable = new DataTable();

            // DataGrid - Init Columns
            Console.WriteLine("Columns:");
            foreach (string columnName in columnNames)
            {
                DataColumn dataColumn = new DataColumn(columnName, typeof(string));
                dataTable.Columns.Add(dataColumn);

                List<string> columnValues = dk.ReadColumn(TABLE_NAME, columnName);
                columnData[columnName] = columnValues;
            }


            /// DataGrid - Init Rows
            // Get number of row
            int rowCount = MaxCount(columnData);
            // Populate rows
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
            
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void Button_Export_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_AddItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_DeleteItem_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
