using System;
using System.Collections.Generic;

namespace TaxSchedule
{
    public static class Client{
        
    }

        class MainClass
    {
        public static void Main(string[] args)
        {
            var parser = new ParseFile("municipalities.txt");
            var tax = parser.parse();
            float res = tax.getTax("Copenhagen",new DateTime(2016,01,01));
            Console.WriteLine(res);
            res = tax.getTax("Copenhagen",new DateTime(2016,12,25));
            Console.WriteLine(res);
            res = tax.getTax("Copenhagen",new DateTime(2016,05,02));
            Console.WriteLine(res);
            res = tax.getTax("Copenhagen",new DateTime(2016,07,10));
            Console.WriteLine(res);
            res = tax.getTax("Copenhagen",new DateTime(2016,03,16));
            Console.WriteLine(res);
        }
    }
}
