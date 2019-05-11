using DatabaseKeeper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace DatabaseKeeper.Tests
{
    [TestClass]
    public class CLIParserTests
    {
        private Mock<TBDatabaseKeeper> mockedDBKeeper;
        private readonly string tableName = "testTable";
        private readonly string dbName = "testDB";
        private readonly string path = @"F:\FII\M1\2\CSS\Proiect\css_proj\Databases";
        private List<string> table;
        private Dictionary<string, List<string>> databaseTables;
        private Dictionary<string, string> databasesList;
        private TBDatabaseKeeper keeper;
        private DataKeeper dataKeeper;

        [SetUp]
        public void Init()
        {
            mockedDBKeeper = new Mock<TBDatabaseKeeper>();
            //var dataKeeperMq = new Mock<DataKeeper>();
            //dataKeeper = dataKeeperMq.Object;

            table = new List<string>();
            table.AddRange(new[] { "!Col1", "0-a1", "1-a2", "2-a3", "!Col2", "0-b1", "1-b2", "2-b3" });

            databaseTables = new Dictionary<string, List<string>>();
            var tables = new List<string>();
            tables.Add(tableName + ".TB");
            databaseTables.Add(dbName, tables);

            databasesList = new Dictionary<string, string>();
            databasesList[dbName] = path;


            mockedDBKeeper.Setup(mq => mq.ReadTable(tableName)).Returns(table);
            mockedDBKeeper.Object.SetDatabase(databaseTables, databasesList, dbName);
        }

        [TestMethod()]
        public void selectEntriesTest()
        {
            string tableName = "tableName";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            dkMock.Setup(mock => mock.SelectData(tableName, ">", "10"));

            string[] args = new string[] { "select", "from", tableName, "where", "column", ">", "10" };
            CLIParser cLIParser = new CLIParser(dkMock.Object);
            cLIParser.parseArguments(args);

            dkMock.Verify(mock => mock.SelectData(tableName, ">", "10"), Times.Once());
        }

        [TestMethod()]
        public void selectEntriesTestWithMoreArguments()
        {
            string tableName = "tableName";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            Mock<CLIParser> cLIParserMock = new Mock<CLIParser>(dkMock.Object);

            //more arguments
            string[] args = new string[] { "select", "from", tableName, "and", "where", "column", ">", "10" };
            cLIParserMock.Object.parseArguments(args);

            dkMock.Verify(mock => mock.SelectData(tableName, ">", "10"), Times.Never());
            cLIParserMock.Verify(mock => mock.printIncorrectNumberOfParametersError(), Times.Once);
        }

        [TestMethod()]
        public void selectEntriesTestWithLessArguments()
        {
            string tableName = "tableName";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            Mock<CLIParser> cLIParserMock = new Mock<CLIParser>(dkMock.Object);

            //less arguments
            string[] args = new string[] { "select", "from", "where", "column", ">", "10" };
            cLIParserMock.Object.parseArguments(args);

            dkMock.Verify(mock => mock.SelectData(tableName, ">", "10"), Times.Never());
            cLIParserMock.Verify(mock => mock.printIncorrectNumberOfParametersError(), Times.Once);
        }

        [TestMethod()]
        public void selectEntriesTestWithWrongArguments()
        {
            string tableName = "tableName";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            Mock<CLIParser> cLIParserMock = new Mock<CLIParser>(dkMock.Object);

            string[] args = new string[] { "select", "from", tableName, "whereas", "column", ">", "10" };
            cLIParserMock.Object.parseArguments(args);

            dkMock.Verify(mock => mock.SelectData(tableName, ">", "10"), Times.Never());
            cLIParserMock.Verify(mock => mock.printUnexpectedParametersError(), Times.Once);
        }

        [TestMethod()]
        public void deleteEntriesTest()
        {
            string tableName = "tableName";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            dkMock.Setup(mock => mock.DeleteEntries(tableName, "column", 1, 3));

            string[] args = new string[] { "delete", "from", tableName, "column", "1", "3" };
            CLIParser cLIParser = new CLIParser(dkMock.Object);
            cLIParser.parseArguments(args);

            dkMock.Verify(mock => mock.DeleteEntries(tableName, "column", 1, 3), Times.Once());
        }


        [TestMethod()]
        public void deleteEntriesTestWithMoreArguments()
        {
            string tableName = "tableName";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            Mock<CLIParser> cLIParserMock = new Mock<CLIParser>(dkMock.Object);

            //more arguments
            string[] args = new string[] { "delete", "from", "some", tableName, "column", "1", "3" };
            cLIParserMock.Object.parseArguments(args);

            dkMock.Verify(mock => mock.DeleteEntries(tableName, "column", 1, 3), Times.Never());
            cLIParserMock.Verify(mock => mock.printIncorrectNumberOfParametersError(), Times.Once);
        }

        [TestMethod()]
        public void deleteEntriesTestWithLessArguments()
        {
            string tableName = "tableName";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            Mock<CLIParser> cLIParserMock = new Mock<CLIParser>(dkMock.Object);

            //less arguments
            string[] args = new string[] { "delete", tableName, "column", "1", "3" };
            cLIParserMock.Object.parseArguments(args);

            dkMock.Verify(mock => mock.DeleteEntries(tableName, "column", 1, 3), Times.Never());
            cLIParserMock.Verify(mock => mock.printIncorrectNumberOfParametersError(), Times.Once);
        }

        [TestMethod()]
        public void deleteEntriesTestWithWrongArguments()
        {
            string tableName = "tableName";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            Mock<CLIParser> cLIParserMock = new Mock<CLIParser>(dkMock.Object);

            string[] args = new string[] { "delete", "from where", tableName, "column", "1", "3" };
            cLIParserMock.Object.parseArguments(args);

            dkMock.Verify(mock => mock.DeleteEntries(tableName, "column", 1, 3), Times.Never());
            cLIParserMock.Verify(mock => mock.printUnexpectedParametersError(), Times.Once);
        }

        [TestMethod()]
        public void dropTableTest()
        {
            string tableName = "tableName";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            dkMock.Setup(mock => mock.DeleteTable(tableName));

            string[] args = new string[] { "drop", "table", tableName };
            CLIParser cLIParser = new CLIParser(dkMock.Object);
            cLIParser.parseArguments(args);

            dkMock.Verify(mock => mock.DeleteTable(tableName), Times.Once());
        }

        [TestMethod()]
        public void dropTableTestWithMoreArguments()
        {
            string tableName = "tableName";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            Mock<CLIParser> cLIParserMock = new Mock<CLIParser>(dkMock.Object);

            //more arguments
            string[] args = new string[] { "drop", "my", "table", tableName };
            cLIParserMock.Object.parseArguments(args);

            dkMock.Verify(mock => mock.DeleteTable(tableName), Times.Never());
            cLIParserMock.Verify(mock => mock.printIncorrectNumberOfParametersError(), Times.Once);
        }

        [TestMethod()]
        public void dropTableTestWithLessArguments()
        {
            string tableName = "tableName";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            Mock<CLIParser> cLIParserMock = new Mock<CLIParser>(dkMock.Object);

            //less arguments
            string[] args = new string[] { "drop", tableName };
            cLIParserMock.Object.parseArguments(args);

            dkMock.Verify(mock => mock.DeleteTable(tableName), Times.Never());
            cLIParserMock.Verify(mock => mock.printIncorrectNumberOfParametersError(), Times.Once);
        }

        [TestMethod()]
        public void dropTableTestWithWrongArguments()
        {
            string tableName = "tableName";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            Mock<CLIParser> cLIParserMock = new Mock<CLIParser>(dkMock.Object);

            string[] args = new string[] { "drop", "tabler", tableName };
            cLIParserMock.Object.parseArguments(args);

            dkMock.Verify(mock => mock.DeleteTable(tableName), Times.Never());
            cLIParserMock.Verify(mock => mock.printUnexpectedParametersError(), Times.Once);
        }

        [TestMethod()]
        public void createTableTest()
        {
            string tableName = "tableName";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            dkMock.Setup(mock => mock.CreateTable(tableName, new List<string>()));

            string[] args = new string[] { "create", "table", tableName };
            CLIParser cLIParser = new CLIParser(dkMock.Object);
            cLIParser.parseArguments(args);

            dkMock.Verify(mock => mock.CreateTable(tableName, new List<string>()), Times.Once());
        }

        [TestMethod()]
        public void createTableTestWithMoreArguments()
        {
            string tableName = "tableName";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            Mock<CLIParser> cLIParserMock = new Mock<CLIParser>(dkMock.Object);

            //more arguments
            string[] args = new string[] { "create", "table", "and", "database", tableName };
            cLIParserMock.Object.parseArguments(args);

            dkMock.Verify(mock => mock.CreateTable(tableName, new List<string>()), Times.Never());
            cLIParserMock.Verify(mock => mock.printIncorrectNumberOfParametersError(), Times.Once);
        }

        [TestMethod()]
        public void createTableTestWithLessArguments()
        {
            string tableName = "tableName";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            Mock<CLIParser> cLIParserMock = new Mock<CLIParser>(dkMock.Object);

            //less arguments
            string[] args = new string[] { "create", tableName };
            cLIParserMock.Object.parseArguments(args);

            dkMock.Verify(mock => mock.CreateTable(tableName, new List<string>()), Times.Never());
            cLIParserMock.Verify(mock => mock.printIncorrectNumberOfParametersError(), Times.Once);
        }

        [TestMethod()]
        public void createTableTestWithWrongArguments()
        {
            string tableName = "tableName";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            Mock<CLIParser> cLIParserMock = new Mock<CLIParser>(dkMock.Object);

            string[] args = new string[] { "create", "some", tableName };
            cLIParserMock.Object.parseArguments(args);

            dkMock.Verify(mock => mock.CreateTable(tableName, new List<string>()), Times.Never());
            cLIParserMock.Verify(mock => mock.printUnexpectedParametersError(), Times.Once);
        }

        [TestMethod()]
        public void createDatabaseTest()
        {
            string databaseName = "databaseName";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            dkMock.Setup(mock => mock.CreateDatabase(databaseName, path));

            string[] args = new string[] { "create", "database", databaseName };
            CLIParser cLIParser = new CLIParser(dkMock.Object);
            cLIParser.parseArguments(args);

            dkMock.Verify(mock => mock.CreateDatabase(databaseName, path), Times.Once());
        }

        [TestMethod()]
        public void createDatabaseTestWithMoreArguments()
        {
            string databaseName = "databaseName";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            Mock<CLIParser> cLIParserMock = new Mock<CLIParser>(dkMock.Object);

            //more arguments
            string[] args = new string[] { "create", "a ", "database", databaseName };
            cLIParserMock.Object.parseArguments(args);

            dkMock.Verify(mock => mock.CreateDatabase(databaseName, path), Times.Never());
            cLIParserMock.Verify(mock => mock.printIncorrectNumberOfParametersError(), Times.Once);
        }

        [TestMethod()]
        public void createDatabaseTestWithLessArguments()
        {
            string databaseName = "databaseName";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            Mock<CLIParser> cLIParserMock = new Mock<CLIParser>(dkMock.Object);

            //less arguments
            string[] args = new string[] { "create", "database" };
            cLIParserMock.Object.parseArguments(args);

            dkMock.Verify(mock => mock.CreateDatabase(databaseName, path), Times.Never());
            cLIParserMock.Verify(mock => mock.printIncorrectNumberOfParametersError(), Times.Once);
        }

        [TestMethod()]
        public void createDatabaseTestWithWrongArguments()
        {
            string databaseName = "databaseName";
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            Mock<CLIParser> cLIParserMock = new Mock<CLIParser>(dkMock.Object);

            string[] args = new string[] { "create", "a database", databaseName };
            cLIParserMock.Object.parseArguments(args);

            dkMock.Verify(mock => mock.CreateDatabase(databaseName, path), Times.Never());
            cLIParserMock.Verify(mock => mock.printUnexpectedParametersError(), Times.Once);
        }

        [TestMethod()]
        public void listTableTest()
        {
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            dkMock.Setup(mock => mock.GetTableNames());

            string[] args = new string[] { "list", "tables" };
            CLIParser cLIParser = new CLIParser(dkMock.Object);
            cLIParser.parseArguments(args);

            dkMock.Verify(mock => mock.GetTableNames(), Times.Once());
        }

        [TestMethod()]
        public void listTableTestWithMoreArguments()
        {
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            Mock<CLIParser> cLIParserMock = new Mock<CLIParser>(dkMock.Object);

            //more arguments
            string[] args = new string[] { "list", "all", "tables" };
            cLIParserMock.Object.parseArguments(args);

            dkMock.Verify(mock => mock.GetTableNames(), Times.Never());
            cLIParserMock.Verify(mock => mock.printIncorrectNumberOfParametersError(), Times.Once);
        }

        [TestMethod()]
        public void listTableTestWithLessArguments()
        {
            TBDatabaseKeeper keeper = new Mock<TBDatabaseKeeper>().Object;
            Mock<DataKeeper> dkMock = new Mock<DataKeeper>(keeper);
            Mock<CLIParser> cLIParserMock = new Mock<CLIParser>(dkMock.Object);

            //less arguments
            string[] args = new string[] { "list" };
            cLIParserMock.Object.parseArguments(args);

            dkMock.Verify(mock => mock.GetTableNames(), Times.Never());
            cLIParserMock.Verify(mock => mock.printIncorrectNumberOfParametersError(), Times.Once);
        }
    }
}