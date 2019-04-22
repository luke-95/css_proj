using DatabaseKeeper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseKeeper
{
    class CLIParser
    {
        private static List<string> operators = new List<string> { "==", "=", "<", ">", "<=", ">=", "!=", "<>", "less", "lesseq", "greater", "greatereq"};
        private static List<string> actions = new List<string> { SELECT, DROP, CREATE, DELETE, IMPORT, EXPORT, HELP};

        //Actions
        private const string SELECT = "select";
        private const string DELETE = "delete";

        private const string DROP = "drop";
        private const string CREATE = "create";

        private const string IMPORT = "import";
        private const string EXPORT = "export";

        private const string HELP = "help";

        //Adjacent  
        private const string FROM = "from";
        private const string WHERE = "where";
        private const string TABLE = "table";
        private const string DATABASE = "database";


        private static readonly string DATABASE_PATH = @"F:\FII\M1\2\CSS\Proiect\css_proj\Databases";
        static string selectedDatabase = "NewDatabase";
        static string selectedTable = "MyTable";

        public static void Main2(string[] args)
        {
            String action = args[0].ToLower();
            if (!actions.Contains(action))
            {
                printActionNotSupportedError(action);
            }
            else
            {
                switch (action)
                {
                    case SELECT:
                        if (args.Length != 7)
                        {
                            printIncorrectNumberOfParametersError();
                            break;
                        } 
                        else if (!args[1].Equals(FROM) || !args[3].Equals(WHERE) || !operators.Contains(args[5]))
                        {
                            printUnexpectedParametersError();
                            break;
                        }
                        else
                        {
                            selectEntries(args[2], args[4], args[5], args[6]);
                        }
                        break;
                    case DELETE:
                        if (args.Length != 5)
                        {
                            printIncorrectNumberOfParametersError();
                            break;
                        }
                        else if(!args[1].Equals(FROM) || !args[3].Equals(WHERE) || !operators.Contains(args[5]))
                        {
                            printUnexpectedParametersError();
                            break;
                        }
                        else
                        {
                            deleteEntries(args[2], args[4], args[5], args[6]);
                        }
                        break;
                    case DROP:
                        if (args.Length != 3)
                        {
                            printIncorrectNumberOfParametersError();
                            break;
                        }
                        else if (!args[1].Equals(TABLE))
                        {
                            printUnexpectedParametersError();
                            break;
                        }
                        else
                        {
                            dropTable(args[2]);
                        }
                        break;
                    case CREATE:
                        if (args.Length < 3)
                        {
                            printIncorrectNumberOfParametersError();
                            break;
                        }
                        else if(!args[1].Equals(TABLE) && !args[1].Equals(DATABASE))
                        {
                            printUnexpectedParametersError();
                            break;
                        }
                        else
                        {
                            if (args[1].Equals(TABLE))
                            {
                                List<String> columns = new List<string>();
                                for (int i = 3; i < args.Length; i++)
                                {
                                    columns.Add(args[i]);
                                }
                                createTable(args[2], columns);
                            }
                            if (args[1].Equals(DATABASE))
                            {
                                createDatabase(args[2]);
                            }
                        }
                        break;
                    case IMPORT:
                        if (args.Length != 3)
                        {
                            printIncorrectNumberOfParametersError();
                            break;
                        }
                        else if(!args[2].EndsWith(".csv")) 
                        {
                            printImportExportFileTypeNotSupportedError();
                            break;
                        }
                        else
                        {
                            importTable(args[1], args[2]);
                        }
                        break;
                    case EXPORT:
                        if (args.Length != 3)
                        {
                            printIncorrectNumberOfParametersError();
                            break;
                        }
                        else if(!args[2].EndsWith(".csv"))
                        {
                            printImportExportFileTypeNotSupportedError();
                            break;
                        }
                        else
                        {
                            exportTable(args[1], args[2]);
                        }
                        break;
                    case HELP:
                        if(args.Length > 1)
                        {
                            printIncorrectNumberOfParametersError();
                            break;
                        }
                        readHelpMessage();
                        break;
                    default:
                        Console.WriteLine("Unsupported command!");
                        break;
                }
            }
            Console.ReadLine();
        }

        private static void selectEntries(string tableName, string columnName, string op, string value)
        {
            Console.WriteLine("Selecting stuff: " + tableName + " " + columnName + " " + op + " " + value);
            TBDatabaseKeeper keeper = new TBDatabaseKeeper();
            DataKeeper dk = new DataKeeper(keeper);
            dk.LoadDatabase(selectedDatabase, DATABASE_PATH);
            dk.SelectDatabase(selectedDatabase);

            Dictionary<string, List<string>> result = dk.SelectData(tableName, op, value);
            Console.WriteLine("Select result:\n");
            foreach (KeyValuePair<string, List<string>> kvp in result)
            {
                Console.Write("\nKey = {0}, Values: ", kvp.Key);
                foreach (string val in kvp.Value)
                {
                    Console.Write(val + " ");
                }
            }
        }

        private static void deleteEntries(string tableName, string columnName, string op, string value)
        {
            Console.WriteLine("Deleting stuff" + tableName + " " + columnName + " " + op + " " + value);
        }
       
        private static void dropTable(string table)
        {
            Console.WriteLine("Deleting table " + table);
            TBDatabaseKeeper keeper = new TBDatabaseKeeper();
            DataKeeper dk = new DataKeeper(keeper);

            dk.DeleteTable(table);
        }

        private static void createTable(string table, List<String> columns)
        {
            Console.WriteLine("Creating table " + table);
            TBDatabaseKeeper keeper = new TBDatabaseKeeper();
            DataKeeper dk = new DataKeeper(keeper);

            dk.LoadDatabase(selectedDatabase, DATABASE_PATH);
            dk.SelectDatabase(selectedDatabase);

            dk.CreateTable(table, columns);
        }

        private static void importTable(string table, string csv)
        {

        }

        private static void exportTable(string table, string csv)
        {

        }

        private static void createDatabase(string databaseName)
        {
            TBDatabaseKeeper keeper = new TBDatabaseKeeper();
            DataKeeper dk = new DataKeeper(keeper);
            dk.CreateDatabase(databaseName, DATABASE_PATH);
        }
        private static void listTable()
        {
            Console.WriteLine("Listing table");
        }

        //Error messages
        private static void printIncorrectNumberOfParametersError()
        {
            Console.WriteLine("Incorrent number of parameters!\nType \"help\" for support.");
            Console.ReadLine();
        }

        private static void printImportExportFileTypeNotSupportedError()
        {
            Console.WriteLine("Import/Export file type not supported! Only .csv accepted");
            Console.ReadLine();
        }

        private static void printUnexpectedParametersError()
        {
            Console.WriteLine("Unexpected parametes found!\nType \"help\" for support.");
            Console.ReadLine();
        }

        private static void printActionNotSupportedError(string action)
        {
            Console.WriteLine("Action " + action + " not supported!\nType \"help\" for support.");
            Console.ReadLine();
        }

        //Read file utils
        private static void readHelpMessage()
        {
            {
                string text = System.IO.File.ReadAllText("HelpFile.txt");

                Console.Write(text);
                System.Console.ReadKey();
            }
        }
    }
}
