using System;
using System.IO;
using System.Collections.Generic;

namespace TaxSchedule
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Test.Run();
            var schedule = new TaxSchedule();
            DatabaseClient database = new DatabaseClient("TaxSchedule.sqlite");
            if(!database.ReadFromDatabase(schedule)){
                Console.WriteLine("There was something wrong while reading from the database, please fix this");
                Console.WriteLine("We will try to use the file 'municipalities.txt' instead");
                schedule = new TaxSchedule();
                var parser = new ParseFile("municipalities.txt");
                schedule = parser.Parse(schedule);
            }
            var client = new Client(schedule);
            client.StartActivity();
            database.WriteToDatabase(schedule);
        }
    }
}
