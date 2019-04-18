using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Importer
{
    public class Importer
    {
        public Dictionary<string,List<string>> ReadCsv(string path)
        {
            if(!path.EndsWith(".csv"))
                throw new Exception("File is not a csv file.");

            var parsedCsv = new Dictionary<string,List<string>>();

            using (StreamReader file = File.OpenText(path))
            {
                var header = file.ReadLine();
                var columnNames = header.Split(',');

                foreach (var columnName in columnNames)
                {
                    parsedCsv[columnName] = new List<string>();
                }

                while (!file.EndOfStream)
                {
                    var line = file.ReadLine();
                    var values = line.Split(',');
                    for (int index = 0; index < columnNames.Length; index++)
                    {
                        parsedCsv[columnNames[index]].Add(values[index]);
                    }
                }

                return parsedCsv;
            }
        }
    }
}
