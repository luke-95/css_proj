using System.Collections.Generic;

namespace SimpleDatabase.Models
{
    public class SimpleDatabaseModel
    {
        private List<TableModel> _tables;
        private string _name;
        public List<TableModel> Tables { get => _tables; set => _tables = value; }
        public string Name { get => _name; set => _name = value; }

        public List<string> TableNames {
            get
            {
                List<string> names = new List<string>();
                foreach (TableModel table in Tables)
                {
                    names.Add(table.Name);
                }
                return names;
            }
        }

        public TableModel GetTable(string TableName)
        {
            foreach( TableModel table in Tables)
            {
                if (table.Name == TableName)
                {
                    return table;
                }
            }
            return null;
        }

        private SimpleDatabaseModel()
        {
        }

        public static SimpleDatabaseModel WithTables(List<TableModel> tables)
        {
            SimpleDatabaseModel dbModel = new SimpleDatabaseModel();
            dbModel.Tables = tables;
            return dbModel;
        }

        public static SimpleDatabaseModel WithNoData()
        {
            SimpleDatabaseModel dbModel = new SimpleDatabaseModel();
            dbModel.Tables = new List<TableModel>();
            return dbModel;
        }

        public Dictionary<string, List<string>> GetColumns(TableModel table)
        {
            if (Tables.Contains(table))
            {
                return table.Columns;
            }
            return null;
        }

        public void DeleteTable(string TableName)
        {
            Tables.RemoveAll(tableModel => tableModel.Name == TableName);
        }
    }

}
