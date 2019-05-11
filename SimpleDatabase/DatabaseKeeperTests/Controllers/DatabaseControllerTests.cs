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

namespace SimpleDatabase.Controllers.Tests
{

    [TestFixture]
    public class DatabaseControllerTests
    {
        private readonly string path = @"F:\FII\M1\2\CSS\Proiect\css_proj\Databases";

        [TestMethod()]
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

        [TestMethod()]
        public void CreateEmptyTableTest()
        {
        }

        [TestMethod()]
        public void SaveTableTest()
        {
        }

        [TestMethod()]
        public void DeleteTableTest()
        {
        }

        [TestMethod()]
        public void AddEntriesTest()
        {
        }

        [TestMethod()]
        public void GetTableNamesTest()
        {
        }

        [TestMethod()]
        public void GetColumnNamesTest()
        {
        }

        [TestMethod()]
        public void LoadDatabaseTest()
        {
        }

        [TestMethod()]
        public void GetDatabaseNamesTest()
        {
        }
    }
}