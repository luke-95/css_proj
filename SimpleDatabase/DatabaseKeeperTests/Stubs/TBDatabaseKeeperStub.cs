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
        public void InsertEntriesInColumn(string columnName, List<string> entriesList, List<string> TBtable)
        {
            base.InsertEntriesInColumn(columnName,entriesList,TBtable);
        }

        public void DeleteEntriesInColumn(string columnName, int startIndex, int stopIndex, List<string> table)
        {
            base.DeleteEntriesInColumn(columnName,startIndex,stopIndex,table);
        }
    }
}
