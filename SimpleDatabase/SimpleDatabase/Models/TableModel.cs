using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDatabase.Models
{
    public class TableModel
    {
        private readonly Dictionary<string, List<string>> _columns;
        private string _name;
        private int _rowCount = 0;

        public Dictionary<string, List<string>> Columns => _columns;
        public string Name { get => _name; set => _name = value; }

        public List<string> ColumnNames {
            get
            {
                return Columns.Keys.ToList();
            }
        }

        public int RowCount
        {
            get => _rowCount;
            set => _rowCount = value;
        }

        public TableModel(string name, Dictionary<string, List<string>> columns)
        {
            _name = name;
            _columns = columns;

            UpdateRowCount();
        }

        /**
         * Go through all columns and count their values. Highest number is the row count.
         */
        private void UpdateRowCount()
        {
            foreach (List<string> columnValues in Columns.Values)
            {
                if (columnValues.Count > RowCount)
                {
                    RowCount = columnValues.Count;
                }
            }
        }

        /**
         * Add empty values, to ensure that every column has the same number of values.
         */
        private void FillTableGaps()
        {
            foreach (List<string> columnValues in Columns.Values)
            {
                while (columnValues.Count < RowCount)
                {
                    columnValues.Add("");
                }
            }
        }


        public void AddColumn(String columnName, List<string> values) 
        {
            _columns.Add(columnName, values);

            if (values.Count > RowCount)
            {
                RowCount = values.Count;
            }
            FillTableGaps();
        }

        public void AddRow(String value, String columnName)
        {
            if (ColumnNames.Contains(columnName))
            {
                Columns[columnName].Add(value);

                RowCount += 1;
                FillTableGaps();
            }
        }

        public void RemoveColumn(String name)
        {
            _columns.Remove(name);
        }
    }
}
