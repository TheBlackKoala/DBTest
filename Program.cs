using System;
using System.IO;
using System.Collections.Generic;

namespace TaxSchedule
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var parser = new ParseFile("municipalities.txt");
            var schedule = new TaxSchedule();
            schedule = parser.Parse(schedule);
            float res = schedule.GetTax("Copenhagen",new DateTime(2016,01,01));
            Console.WriteLine(res);
            res = schedule.GetTax("Copenhagen",new DateTime(2016,12,25));
            Console.WriteLine(res);
            res = schedule.GetTax("Copenhagen",new DateTime(2016,05,02));
            Console.WriteLine(res);
            res = schedule.GetTax("Copenhagen",new DateTime(2016,07,10));
            Console.WriteLine(res);
            res = schedule.GetTax("Copenhagen",new DateTime(2016,03,16));
            Console.WriteLine(res);
            var client = new Client(schedule);
            client.StartActivity();
        }
    }
}
