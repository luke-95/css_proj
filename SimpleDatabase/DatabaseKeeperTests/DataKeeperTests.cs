using System;
using System.Collections.Generic;
using DatabaseKeeper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Moq;

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
        private TBDatabaseKeeper keeper;
        private Moq.Mock<TBDatabaseKeeper> mockedDBKeeper;

        [SetUp]
        public void Init()
        {
            mockedDBKeeper = new Mock<TBDatabaseKeeper>();
            table =new List<string>();
            table.AddRange(new[] { "!Col1", "0-a1", "1-a2", "2-a3","!Col2", "0-b1", "1-b2", "2-b3" });

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
    }
}
