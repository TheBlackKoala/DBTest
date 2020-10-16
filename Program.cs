using System;
using System.IO;
using System.Collections.Generic;

namespace TaxSchedule
{
    public class Client{
        public void tutorial(){
            Console.WriteLine("Enter T to see this tutorial again");

            Console.WriteLine("Enter H to see the simple help");
            Console.WriteLine("Enter R to see the readme for this project which contains aditional information not covered here");
        }

        public void getRecord(){
            
        }

        public void addRecord(){
            Console.WriteLine("What do you want to add to the records?");
            Console.WriteLine("Enter M to add a Municipality");
            Console.WriteLine("Enter D to add a Daily tax to a municipality");
            Console.WriteLine("Enter W to add a Weekly tax to a municipality");
            Console.WriteLine("Enter T to add a Monthly tax to a municipality");
            Console.WriteLine("Enter Y to add a Yearly tax to a municipality");
            string command = Console.ReadLine();
            switch (command.ToUpper()[0])
            {
                case 'M':
                    Console.WriteLine("Please enter the name of the municipality you want to add, write Q to exit the process");
                    command = Console.ReadLine();
                    if(command.ToUpper() == "Q"){
                        break;
                    }
                    //Add the municipality


                    break;
                case 'D':
                    //Get municipality name and then dates
                    break;
            }
        }

        public void checkTax(){

        }

        public void import(){
            Console.WriteLine("What file would like to import from?");
            Console.WriteLine("Please include the full path and make sure that ");
            string path = Console.ReadLine();
            
        }

        public void help(){
            Console.WriteLine("Enter T for a tutorial");
            Console.WriteLine("Enter A to add a record");
            Console.WriteLine("Enter C to check a tax");
            Console.WriteLine("Enter I to import data from a file");
            Console.WriteLine("Enter H to see this help again");
            Console.WriteLine("Enter R to see the readme for this project");
        }

        public void readme(){
            Console.WriteLine("This is the contents of the readme file for this project:");
            using (StreamReader file = File.OpenText("./readme")){
                Console.Write(file.ReadToEnd());
            }
        }

        public void StartActivity(){
            Console.WriteLine("Hey there.");
            Console.WriteLine("Welcome to this municipality tax program.");
            Console.WriteLine("Please let me know how i can help.");
            help();
            for(;;){
                string command = Console.ReadLine();
                switch(command.ToUpper()[0])
                {
                    case 'T':
                        tutorial();
                        break;
                    case 'A':
                        addRecord();
                        break;
                    case 'C':
                        checkTax();
                        break;
                    case 'I':
                        import();
                        break;
                    case 'H':
                        help();
                        break;
                    case 'R':
                        readme();
                        break;
                    default:
                        Console.WriteLine("Unknown command, see description below for commands");
                        help();
                        break;
                }
            }
        }
    }

    public static class API{
        
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
