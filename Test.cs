using System;

namespace TaxSchedule
{
    public class Test{
        public static bool TestError(){
            //Bolean for storing the results
            bool test = true;
            float res = 0;
            TaxSchedule schedule = new TaxSchedule();
            string name = "Copenhagen";
            //Test time
            DateTime time = new DateTime(2016,01,01);
            //Fail municipality
            try{
                res = schedule.GetTax(name,time);
            }
            catch(ArgumentException e){
                test = test && (e.Message=="Municipality not found (Parameter 'Copenhagen')");
            }
            test = test && schedule.AddMunicipality(name);
            //Fail getting a tax
            try{
                res = schedule.GetTax(name,time);
            }
            catch(ArgumentException e){
                test = test && (e.Message=="Tax not found");
            }
            return test;
        }

        public static bool TestDaily(){
            bool test = true;
            TaxSchedule schedule = new TaxSchedule();
            string name = "Copenhagen";
            //Test times
            DateTime dtime = new DateTime(2016,01,01);
            DateTime wtime = new DateTime(2016,02,01);
            DateTime mtime = new DateTime(2016,03,01);
            DateTime ytime = new DateTime(2016,04,01);
            DateTime start = new DateTime(2016,01,01);
            DateTime end = new DateTime(2016,04,01);
            //Test tax
            float dtax = 0.1f;
            float wtax = 0.2f;
            float mtax = 0.3f;
            float ytax = 0.4f;
            test = test && schedule.AddMunicipality(name);
            //The tax we are currently testing for
            test = test && schedule.AddDaily(name,new DateTimeRange(start,end),dtax);
            //Add the other taxes
            test = test && schedule.AddDaily(name,new DateTimeRange(dtime,dtime),dtax);
            test = test && schedule.AddWeekly(name,new DateTimeRange(wtime,wtime),wtax);
            test = test && schedule.AddMonthly(name,new DateTimeRange(mtime,mtime),mtax);
            test = test && schedule.AddYearly(name,new DateTimeRange(ytime,ytime),ytax);
            //Test the taxes
            try{
                //Daily overshadows all taxes
                test = test && (schedule.GetTax(name,dtime)==dtax);
                test = test && (schedule.GetTax(name,wtime)==dtax);
                test = test && (schedule.GetTax(name,mtime)==dtax);
                test = test && (schedule.GetTax(name,ytime)==dtax);
            }
            catch{
                return false;
            }

            return test;
        }

        public static bool TestWeekly(){
            bool test = true;
            TaxSchedule schedule = new TaxSchedule();
            string name = "Copenhagen";
            //Test times
            DateTime dtime = new DateTime(2016,01,01);
            DateTime wtime = new DateTime(2016,02,01);
            DateTime mtime = new DateTime(2016,03,01);
            DateTime ytime = new DateTime(2016,04,01);
            DateTime start = new DateTime(2016,01,01);
            DateTime end = new DateTime(2016,04,01);
            //Test tax
            float dtax = 0.1f;
            float wtax = 0.2f;
            float mtax = 0.3f;
            float ytax = 0.4f;
            test = test && schedule.AddMunicipality(name);
            //The tax we are currently testing for
            test = test && schedule.AddWeekly(name,new DateTimeRange(start,end),wtax);
            //Add the other taxes
            test = test && schedule.AddDaily(name,new DateTimeRange(dtime,dtime),dtax);
            test = test && schedule.AddWeekly(name,new DateTimeRange(wtime,wtime),wtax);
            test = test && schedule.AddMonthly(name,new DateTimeRange(mtime,mtime),mtax);
            test = test && schedule.AddYearly(name,new DateTimeRange(ytime,ytime),ytax);
            //Test the taxes
            try{
                //Weekly overshadows all but the daily tax
                test = test && (schedule.GetTax(name,dtime)==dtax);
                test = test && (schedule.GetTax(name,wtime)==wtax);
                test = test && (schedule.GetTax(name,mtime)==wtax);
                test = test && (schedule.GetTax(name,ytime)==wtax);
            }
            catch{
                return false;
            }

            return test;
        }

        public static bool TestMonthly(){
            bool test = true;
            TaxSchedule schedule = new TaxSchedule();
            string name = "Copenhagen";
            //Test times
            DateTime dtime = new DateTime(2016,01,01);
            DateTime wtime = new DateTime(2016,02,01);
            DateTime mtime = new DateTime(2016,03,01);
            DateTime ytime = new DateTime(2016,04,01);
            DateTime start = new DateTime(2016,01,01);
            DateTime end = new DateTime(2016,04,01);
            //Test tax
            float dtax = 0.1f;
            float wtax = 0.2f;
            float mtax = 0.3f;
            float ytax = 0.4f;
            test = test && schedule.AddMunicipality(name);
            //The tax we are currently testing for
            test = test && schedule.AddMonthly(name,new DateTimeRange(start,end),mtax);
            //Add the other taxes
            test = test && schedule.AddDaily(name,new DateTimeRange(dtime,dtime),dtax);
            test = test && schedule.AddWeekly(name,new DateTimeRange(wtime,wtime),wtax);
            test = test && schedule.AddMonthly(name,new DateTimeRange(mtime,mtime),mtax);
            test = test && schedule.AddYearly(name,new DateTimeRange(ytime,ytime),ytax);
            //Test the taxes
            try{
                //Monthly overshadows yearly but is overshadowed by daily and weekly
                test = test && (schedule.GetTax(name,dtime)==dtax);
                test = test && (schedule.GetTax(name,wtime)==wtax);
                test = test && (schedule.GetTax(name,mtime)==mtax);
                test = test && (schedule.GetTax(name,ytime)==mtax);
            }
            catch{
                return false;
            }

            return test;
        }

        public static bool TestYearly(){
            bool test = true;
            TaxSchedule schedule = new TaxSchedule();
            string name = "Copenhagen";
            //Test times
            DateTime dtime = new DateTime(2016,01,01);
            DateTime wtime = new DateTime(2016,02,01);
            DateTime mtime = new DateTime(2016,03,01);
            DateTime ytime = new DateTime(2016,04,01);
            DateTime start = new DateTime(2016,01,01);
            DateTime end = new DateTime(2016,04,01);
            //Test tax
            float dtax = 0.1f;
            float wtax = 0.2f;
            float mtax = 0.3f;
            float ytax = 0.4f;
            test = test && schedule.AddMunicipality(name);
            //The tax we are currently testing for
            test = test && schedule.AddYearly(name,new DateTimeRange(start,end),ytax);
            //Add the other taxes
            test = test && schedule.AddDaily(name,new DateTimeRange(dtime,dtime),dtax);
            test = test && schedule.AddWeekly(name,new DateTimeRange(wtime,wtime),wtax);
            test = test && schedule.AddMonthly(name,new DateTimeRange(mtime,mtime),mtax);
            test = test && schedule.AddYearly(name,new DateTimeRange(ytime,ytime),ytax);
            //Test the taxes
            try{
                //Yearly overshadows no taxes
                test = test && (schedule.GetTax(name,dtime)==dtax);
                test = test && (schedule.GetTax(name,wtime)==wtax);
                test = test && (schedule.GetTax(name,mtime)==mtax);
                test = test && (schedule.GetTax(name,ytime)==ytax);
            }
            catch{
                return false;
            }

            return test;
        }

        public static bool TestParse(){
            //Variables
            bool test = true;
            string name = "Copenhagen";
            //Test times
            DateTime dtime = new DateTime(2016,01,01);
            DateTime wtime = new DateTime(2016,02,01);
            DateTime mtime = new DateTime(2016,03,01);
            DateTime ytime = new DateTime(2016,04,01);
            //Test tax
            float dtax = 0.1f;
            float wtax = 0.2f;
            float mtax = 0.3f;
            float ytax = 0.4f;
            //Write a new file to be parsed
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"./output.txt", false)){
                file.Write("Copenhagen\nDaily\n2016.01.01 2016.01.01 0.1\n"+
                           "Weekly\n2016.02.01 2016.02.01 0.2\n"+
                           "Monthly\n2016.03.01 2016.03.01 0.3\n"+
                           "Yearly\n2016.04.01 2016.04.01 0.4\n"
                );
            }
            //Parse and import the file
            TaxSchedule schedule = new TaxSchedule();
            var parser = new ParseFile("output.txt");
            parser.Parse(schedule);
            //Test the taxes
            try{
                test = test && (schedule.GetTax(name,dtime)==dtax);
                test = test && (schedule.GetTax(name,wtime)==wtax);
                test = test && (schedule.GetTax(name,mtime)==mtax);
                test = test && (schedule.GetTax(name,ytime)==ytax);
            }
            catch{
                return false;
            }
            //Cleanup
            System.IO.File.Delete("output.txt");

            return test;
        }

        public static bool TestDatabase(){
            //Variables
            string path = "output.sqlite";
            bool test = true;
            string name = "Copenhagen";
            //Test times
            DateTime dtime = new DateTime(2016,01,01);
            DateTime wtime = new DateTime(2016,02,01);
            DateTime mtime = new DateTime(2016,03,01);
            DateTime ytime = new DateTime(2016,04,01);
            //Test tax
            float dtax = 0.1f;
            float wtax = 0.2f;
            float mtax = 0.3f;
            float ytax = 0.4f;
            //The schedule we export
            TaxSchedule schedule = new TaxSchedule();
            //Add municipality
            test = test && schedule.AddMunicipality(name);
            //Add the other taxes
            test = test && schedule.AddDaily(name,new DateTimeRange(dtime,dtime),dtax);
            test = test && schedule.AddWeekly(name,new DateTimeRange(wtime,wtime),wtax);
            test = test && schedule.AddMonthly(name,new DateTimeRange(mtime,mtime),mtax);
            test = test && schedule.AddYearly(name,new DateTimeRange(ytime,ytime),ytax);
            //Create the database and write to it
            DatabaseClient database = new DatabaseClient(path);
            test = test && database.WriteToDatabase(schedule);
            //New schedule so that we can import onto a clean schedule
            schedule = new TaxSchedule();
            test = test && database.ReadFromDatabase(schedule);
            //Test the taxes
            try{
                Console.WriteLine(test);
                test = test && (schedule.GetTax(name,dtime)==dtax);
                Console.WriteLine(test);
                test = test && (schedule.GetTax(name,wtime)==wtax);
                Console.WriteLine(test);
                test = test && (schedule.GetTax(name,mtime)==mtax);
                Console.WriteLine(test);
                test = test && (schedule.GetTax(name,ytime)==ytax);
                Console.WriteLine(test);
                Console.WriteLine(schedule.GetTax(name,ytime));
            }
            catch{
                return false;
            }
            //Cleanup
            System.IO.File.Delete("output.sqlite");

            return test;
        }

        public static void Run(){
            bool tests = true;
            bool cur = TestError();
            tests = tests && cur;
            if(!cur){
                Console.WriteLine("Failed test of error");
            }

            cur = TestDaily();
            tests = tests && cur;
            if(!cur){
                Console.WriteLine("Failed test of daily tax");
            }

            cur = TestWeekly();
            tests = tests && cur;
            if(!cur){
                Console.WriteLine("Failed test of weekly tax");
            }

            cur = TestMonthly();
            tests = tests && cur;
            if(!cur){
                Console.WriteLine("Failed test of monthly tax");
            }

            cur = TestYearly();
            tests = tests && cur;
            if(!cur){
                Console.WriteLine("Failed test of yearly tax");
            }

            cur = TestParse();
            tests = tests && cur;
            if(!cur){
                Console.WriteLine("Failed test of parsing file and getting tax");
            }

            cur = TestDatabase();
            tests = tests && cur;
            if(!cur){
                Console.WriteLine("Failed test of exporting to and importing from database");
            }

            Console.WriteLine("Tests done");
            if(tests){
                Console.WriteLine("All tests completed succesfully");
            }
        }
    }
}
