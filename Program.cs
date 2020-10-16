using System;
using System.IO;
using System.Collections.Generic;

namespace TaxSchedule
{
    public class Client{
        TaxSchedule tax;

        public Client(TaxSchedule tax){
            this.tax = tax;
        }

        public Client(){
            this.tax=new TaxSchedule();
        }

        //True to continue, false to stop that part of the program
        private receiveInput(string* input){
            *input = Console.ReadLine();
            if(input.ToUpper() == "Q"){
                return false;
            }
            return true;
        }

        public void tutorial(){
            Console.WriteLine("Enter T to see this tutorial again");


            
            Console.WriteLine("Enter H to see the simple help");
            Console.WriteLine("Enter R to see the readme for this project which contains aditional information not covered here");
            Console.WriteLine("Enter Q to exit the program, shutting this interactive client off");
            Console.WriteLine("Enter Q during an action to stop the current action");
        }

        public void getRecord(){
            Console.WriteLine("Please enther the municipality that you want to get a record from");
            string name = "";
            if(!reeiveInput(&name)){
                break;
            }
            Console.WriteLine("Please enter the date you want in the format of YYYY.MM.DD");
            string date = "";
            if(!reeiveInput(&date)){
                break;
            }
            //LOOK HERE!!!!
            
                                                                       
            
            
            
            DateTime time = new ParseFile(".").stringToDate(date);
            float res = tax.getTax(name,time);
            Console.WriteLine("The tax in {0}, on {1} is: {2}", name, time, res);
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
                    Console.WriteLine("Please enter the name of the municipality you want to add");
                    string name = "";
                    if(!reeiveInput(&name)){
                        break;
                    }
                    //Add the municipality
                    try{
                        tax.addMunicipality(name.ToUpper());
                        Console.WriteLine("Municipality {0} was inserted into the tax schedule", name);
                    }
                    catch{
                        Console.WriteLine("Something went wrong when inserting Municipality {0}, please make sure that it is not there already, this is NOT case sensitive.", name);
                    }
                    break;
                case 'D':
                    //Get municipality name and then dates
                    Console.WriteLine("Please enter the name of the Municipality you want to add a daily tax to");
                    string name = "";
                    if(!reeiveInput(&name)){
                        break;
                    }
                    Console.WriteLine("Please enter the start date of the tax");
                    string start = "";
                    if(!reeiveInput(&start)){
                        break;
                    }
                    Console.WriteLine("Please enter the end date of the tax");
                    string end = "";
                    if(!reeiveInput(&end)){
                        break;
                    }
                    Console.WriteLine("Please enter the level of the tax");
                    string level = "";
                    if(!reeiveInput(&level)){
                        break;
                    }
                    
                    break;
            }
        }

        public void checkTax(){


            
        }

        public void import(){
            Console.WriteLine("What file would like to import from?");
            Console.WriteLine("Please include full or complete relative path to the file");
            string path = Console.ReadLine();
            ParseFile parser = new ParseFile(path);
            parser.parse(this.tax);
        }

        public void help(){
            Console.WriteLine("Enter T for a tutorial");
            Console.WriteLine("Enter A to add a record");
            Console.WriteLine("Enter C to check a tax");
            Console.WriteLine("Enter I to import data from a file");
            Console.WriteLine("Enter H to see this help again");
            Console.WriteLine("Enter R to see the readme for this project");
            Console.WriteLine("Enter Q to the program");
            Console.WriteLine("Enter Q during an action to stop the current action");
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
                    case 'Q':
                        Console.WriteLine("Goodbye");
                        return;
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
            var tax = new TaxSchedule();
            tax = parser.parse(tax);
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
