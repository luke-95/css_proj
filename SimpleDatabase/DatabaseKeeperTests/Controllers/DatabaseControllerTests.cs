using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleDatabase.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using System.Threading.Tasks;
using DatabaseKeeper;
using NUnit.Framework;
using SimpleDatabase.Models;

namespace SimpleDatabase.Controllers.Tests
{

    [TestFixture]
    public class DatabaseControllerTests
    {
        private readonly string path = @"F:\FII\M1\2\CSS\Proiect\css_proj\Databases";

        [Test]
        public void CreateDatabaseTest()
        {
            string databaseName = "databasename";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            dkMock.Setup(mock => mock.CreateDatabase(databaseName, path));
            dkMock.Setup(mock => mock.LoadDatabase(databaseName, path));

            DatabaseController databaseController = new Mock<DatabaseController>(keeper, dkMock.Object).Object;
            databaseController.CreateDatabase(databaseName, path);

            dkMock.Verify(mock => mock.CreateDatabase(databaseName, path), Times.Once());
            dkMock.Verify(mock => mock.LoadDatabase(databaseName, path), Times.Once());
        }

        [Test]
        public void CreateEmptyTableTest()
        {
            string tableName = "table";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            dkMock.Setup(mock => mock.CreateTable(tableName, new List<string>()));

            DatabaseController databaseController = new Mock<DatabaseController>(keeper, dkMock.Object).Object;
            databaseController.ImportedDatabaseModel = SimpleDatabaseModel.WithTables(new List<TableModel>());
            databaseController.CreateEmptyTable(tableName, new List<string>());

            dkMock.Verify(mock => mock.CreateTable(tableName, new List<string>()), Times.Once());
        }

        [Test]
        public void SaveTableTest()
        {
            string tableName = "table";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            dkMock.Setup(mock => mock.UpdateTable($"{tableName}.TB", new List<string>()));

            Mock<DatabaseController> databaseControllerMock = new Mock<DatabaseController>(keeper, dkMock.Object);
            DatabaseController databaseController = databaseControllerMock.Object;
            databaseController.SaveTable(tableName, new List<string>());

            dkMock.Verify(mock => mock.UpdateTable($"{tableName}.TB", new List<string>()), Times.Once());
        }

        [Test]
        public void DeleteTableTest()
        {
            string tableName = "table";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            dkMock.Setup(mock => mock.DeleteTable(tableName));

            Mock<DatabaseController> databaseControllerMock = new Mock<DatabaseController>(keeper, dkMock.Object);
            DatabaseController databaseController = databaseControllerMock.Object;
            databaseController.ImportedDatabaseModel = SimpleDatabaseModel.WithTables(new List<TableModel>());
            databaseController.DeleteTable(tableName);

            dkMock.Verify(mock => mock.DeleteTable(tableName), Times.Once());
        }

        [Test]
        public void AddEntriesTest()
        {
            string tableName = "table";
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            dict.Add("key1", new List<string>());
            dict.Add("key2", new List<string>());
            dict.Add("key3", new List<string>());
            dict.Add("key4", new List<string>());

            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            foreach (var column in dict.Keys)
            {
                dkMock.Setup(mock => mock.AddEntries(tableName, column, dict[column]));
            }

            Mock<DatabaseController> databaseControllerMock = new Mock<DatabaseController>(keeper, dkMock.Object);
            DatabaseController databaseController = databaseControllerMock.Object;
            databaseController.AddEntries(tableName, dict);

            foreach (var column in dict.Keys)
            {
                dkMock.Verify(mock => mock.AddEntries(tableName, column, dict[column]), Times.Once());
            }
        }

        [Test]
        public void GetTableNamesTest()
        {
            string databasename = "database";
            List<string> namesList = new List<string> { "table1", "table2", "table3" };

            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            Mock<DatabaseController> databaseControllerMock = new Mock<DatabaseController>(keeper, dkMock.Object);
            DatabaseController databaseController = databaseControllerMock.Object;

            databaseController.CreateDatabase(databasename);
            foreach (string name in namesList)
            {
                databaseController.CreateEmptyTable(name, new List<string>());
            }
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(namesList, databaseController.GetTableNames(databasename));
        }

        [Test]
        public void GetColumnNamesTest()
        {
            string tableName = "table";
            Mock<TBDatabaseKeeper> keeperMock = new Mock<TBDatabaseKeeper>();
            TBDatabaseKeeper keeper = keeperMock.Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);

            keeperMock.Setup(mock => mock.GetColumnNames(tableName));

            DatabaseController databaseController = new Mock<DatabaseController>(keeper, dkMock.Object).Object;
            databaseController.GetColumnNames(tableName);

            keeperMock.Verify(mock => mock.GetColumnNames(tableName), Times.Once());
        }

        [Test]
        public void LoadDatabaseTest()
        {
            string databaseName = "databasename";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            dkMock.Setup(mock => mock.LoadDatabase(databaseName, path));
            dkMock.Setup(mock => mock.SelectDatabase(databaseName));

            DatabaseController databaseController = new Mock<DatabaseController>(keeper, dkMock.Object).Object;
            databaseController.LoadDatabase(databaseName, path);

            dkMock.Verify(mock => mock.LoadDatabase(databaseName, path), Times.Once());
            dkMock.Verify(mock => mock.SelectDatabase(databaseName), Times.Once());
        }

        [Test]
        public void GetDatabaseNamesTest()
        {

        }

        [Test]
        public void StripTableNameTest()
        {
            string tableNameWithoutExtension = "tableName";
            string tableNameWithExtension = "tableName.TB";

            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            DatabaseController databaseController = new Mock<DatabaseController>(keeper, dkMock.Object).Object;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(databaseController.StripTableName(tableNameWithoutExtension), tableNameWithoutExtension);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(databaseController.StripTableName(tableNameWithExtension), tableNameWithoutExtension);
        }
    }
}