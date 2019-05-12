using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using DatabaseKeeper;
using DatabaseKeeperTests.Stubs;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Moq;
using NUnit.Framework.Constraints;

namespace DatabaseKeeperTests
{
    [TestFixture]
    public class DataKeeperTests
    {
        private readonly string tableName ="testTable";
        private readonly string dbName = "testDB";
        private readonly string path = "C:\\someRandomPath";
        private List<string> table;
        private Dictionary<string, List<string>> databaseTables;
        private Dictionary<string, string> databasesList;
        private TBDatabaseKeeperStub keeper;
        private Moq.Mock<TBDatabaseKeeperStub> mockedDBKeeper;

        [SetUp]
        public void Init()
        {
            mockedDBKeeper = new Mock<TBDatabaseKeeperStub>();
            table =new List<string>();
            table.AddRange(new[] { "!Col1", "0-a1", "1-a2", "2-a3","!Col2", "0-b1", "1-b2", "2-b3", "!Col3", "!Col4", "0-v1" });

            databaseTables= new Dictionary<string, List<string>>();
            var tables= new List<string>();
            tables.Add(tableName+".TB");
            databaseTables.Add(dbName, tables);

            databasesList= new Dictionary<string, string>();
            databasesList[dbName] = path;


            mockedDBKeeper.Setup(mq => mq.ReadTable(tableName)).Returns(table);

            mockedDBKeeper.Object.SetDatabase(databaseTables,databasesList,dbName);

        }
        [Test]
        public void WhenTableExistsThenVerificationShouldReturnTableNameWithExtension()
        {
            var tableNameWithExtension = mockedDBKeeper.Object.VerifyTableExistance(tableName);

            tableNameWithExtension.Should().Be(tableName + ".TB");
        }

        [Test]

        public void WhenTableColumnExistsThenReadColumnShouldReturnItsEntries()
        {
            //Arrange
            var expectedEntries = new List<string>();
            expectedEntries.AddRange( new []{ "b1", "b2", "b3" });
            var requestedColumn = "Col2";

            //Act
            var readColumn = mockedDBKeeper.Object.ReadColumn(tableName, requestedColumn);

            //Assert
            readColumn.Should().BeEquivalentTo(expectedEntries);
        }

        [Test]
        public void WhenTableColumnDoseNotExistThenReadColumnShouldReturnEmpty()
        {
            //Arrange
            var requestedColumn = "Col20";

            //Act
            var readColumn = mockedDBKeeper.Object.ReadColumn(tableName, requestedColumn);

            //Assert
            readColumn.Should().BeEmpty();
        }

        [Test]
        public void WhenTableColumnExistsButDoseNotHaveEntriesThenReadColumnReturnsEmpty()
        {
            //Arrange
            var requestedColumn = "Col3";

            //Act
            var readColumn = mockedDBKeeper.Object.ReadColumn(tableName, requestedColumn);

            //Assert
            readColumn.Should().BeEmpty();
        }

        [Test]
        public void GetColumnNamesShouldReturnAllColumnsInTableWithoutEntries()
        {
            //Arrange
            var expectedColumns = new List<string>();
            expectedColumns.AddRange(new[] { "Col1", "Col2", "Col3", "Col4" });

            //Act
            var columns = mockedDBKeeper.Object.GetColumnNames(tableName);

            //Assert
            columns.Should().BeEquivalentTo(expectedColumns);

        }

        [Test]
        public void WhenColumnNameExistsEntriesThenEntriesAreInsertedCorrectly()
        {
            //Arrange
            var columnOfInsertion = "Col3";
            var entriesToInsert = new List<string>();
            entriesToInsert.AddRange(new []{ "c1","c2"});

            var expectedTable = new List<string>();
            expectedTable.AddRange(new[] { "!Col1", "0-a1", "1-a2", "2-a3", "!Col2", "0-b1", "1-b2", "2-b3",
                "!Col3", "0-c1", "1-c2", "!Col4", "0-v1" });

            //Act
            var TBtable = (List<string>)mockedDBKeeper.Object.ReadTable(tableName);
            mockedDBKeeper.Object.AddEntriesInColumn(columnOfInsertion, entriesToInsert,TBtable);

            //Assert
            TBtable.Should().BeEquivalentTo(expectedTable);
        }

        [Test]
        public void WhenColumnNameAndEntriesBetweenSpecifiedEntriesExistDeleteEntriesShouldRemoveThem()
        {
            //Arrange
            var columnOfDeletion = "Col2";
            int startDeleteIndex = 0;
            int stopDeleteIndex = 1;

            var expectedTable = new List<string>();
            expectedTable.AddRange(new[] { "!Col1", "0-a1", "1-a2", "2-a3", "!Col2", "0-b3", "!Col3", "!Col4", "0-v1" });

            //Act
            var TBtable = (List<string>)mockedDBKeeper.Object.ReadTable(tableName);
            mockedDBKeeper.Object.DeleteEntriesInColumn(columnOfDeletion,startDeleteIndex,stopDeleteIndex,TBtable);

            //Assert
            TBtable.Should().BeEquivalentTo(expectedTable);
        }

        [Test]
        public void WhenSpecifiedIndexAndColumnExistsThenUpdateEntryShouldReplaceItsValueInTable()
        {
            //Arrange
            var columnName = "Col1";
            var indexOfValueToUpdate = 0;
            var newValue = "UpdatedValue";

            var expectedTable = new List<string>();
            expectedTable.AddRange(new[] { "!Col1", "0-UpdatedValue", "1-a2", "2-a3", "!Col2", "0-b1", "1-b2", "2-b3", "!Col3", "!Col4", "0-v1" });

            //Act
            var TBtable = (List<string>)mockedDBKeeper.Object.ReadTable(tableName);
            mockedDBKeeper.Object.UpdateEntryInColumn(TBtable,columnName,indexOfValueToUpdate,newValue);

            //Assert
            TBtable.Should().BeEquivalentTo(expectedTable);
        }

        [Test]
        public void WhenIndexAndColumnExistsThenEntriesShouldBeInsertedAndColumnReindexed()
        {
            //Arrange
            var columnName = "Col1";
            var indexOfInsertion = 1;

            var entriesToInsert = new List<string>();
            entriesToInsert.AddRange(new string[] {"x","y","z"});

            var expectedTable = new List<string>();
            expectedTable.AddRange(new[] { "!Col1", "0-a1", "1-x", "2-y","3-z", "4-a2", "5-a3", "!Col2", "0-b1", "1-b2", "2-b3", "!Col3", "!Col4", "0-v1" });

            //Act
            var TBtable = (List<string>)mockedDBKeeper.Object.ReadTable(tableName);
            mockedDBKeeper.Object.InsertEntriesInColumnAt(TBtable,columnName,indexOfInsertion,entriesToInsert);

            //Assert
            TBtable.Should().BeEquivalentTo(expectedTable);
        }
    }
}
