using DatabaseKeeper;
using System;
using System.Collections.Generic;
using System.Windows;

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
            var column1Entries = dk.ReadColumn(TABLE_NAME, "Col1");
            var column2Entries = dk.ReadColumn(TABLE_NAME, "Col2");
            

            dataGrid.ItemsSource = (List<string>)table;
            Console.WriteLine("ReadTable:");
            foreach (string column in (List<string>)table) {
                Console.WriteLine($"{column}:");
                //foreach (string item in (List<string>)column1Entries)
                //{
                //    Console.WriteLine(item);
                //}
            }


            //Importer.Importer importer= new Importer.Importer();
            //var parsedcsv = importer.ReadCsv(@"C:\scrap\AJsonDB\exampleCSV.csv");
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
    }
}
