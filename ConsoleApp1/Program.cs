using System;
using System.Collections.Generic;

namespace DatabaseApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseManager dbManager = new DatabaseManager("D3hbc2J4iw4OrRPzwV9BFxs669KSIdp6jvbj7knf",
                "https://cssproj-fbcd0.firebaseio.com/");
            var columns=new List<string>();
            columns.Add("Insects");
            columns.Add("Birds");
            columns.Add("Fishes");
            columns.Add("Mammals");
            columns.Add("Crustaceans");

            var crustaceans = new List<string>();
            crustaceans.AddRange(new []{"Shrimp","Crab","Squid"});
            //dbManager.CreateTable("Animals", columns).GetAwaiter().GetResult();
            //dbManager.CreateOrUpdateColumn("Animals", "Crustaceans", crustaceans).GetAwaiter().GetResult();
            dbManager.AppendToColumn("Animals", "Crustaceans",crustaceans).GetAwaiter().GetResult();
            var table= dbManager.ReadTable("Animals").GetAwaiter().GetResult();
            Console.WriteLine("Done");
            //var table = dbManager.ReadTable("Animals").GetAwaiter().GetResult();
            Console.ReadLine();
        }
    }
}
