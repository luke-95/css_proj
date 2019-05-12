using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseKeeper;

namespace DatabaseKeeperTests.Stubs
{
    public class TBDatabaseKeeperStub : TBDatabaseKeeper
    {
        public void AddEntriesInColumn(string columnName, List<string> entriesList, List<string> TBtable)
        {
            base.AddEntriesInColumn(columnName,entriesList,TBtable);
        }

        public void DeleteEntriesInColumn(string columnName, int startIndex, int stopIndex, List<string> table)
        {
            base.DeleteEntriesInColumn(columnName,startIndex,stopIndex,table);
        }

        public void UpdateEntryInColumn(List<string> table, string columnName, int index, string newValue)
        {
            base.UpdateEntryInColumn(table,columnName,index,newValue);
        }

        public void InsertEntriesInColumnAt(List<string> table, string columnName, int index,
            List<string> newEntries)
        {
            base.InsertEntriesInColumnAt(table,columnName,index,newEntries);
        }
    }
}
