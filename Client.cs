using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

namespace TaxSchedule
{
    public class Client{
        TaxSchedule schedule;

        public Client(TaxSchedule schedule){
            this.schedule = schedule;
        }

        public Client(){
            this.schedule=new TaxSchedule();
        }

        //True to continue, false to stop that part of the program
        private static bool ReceiveInput(out string input){
            input = Console.ReadLine();
            if(input.ToUpperInvariant() == "Q"){
                return false;
            }
            return true;
        }

        public void Tutorial(){
            Console.WriteLine("Enter T to see this tutorial again");
            //Record adding tutorial
            Console.WriteLine("Enter A to add a record");
            Console.WriteLine("You can add a municipality or a daily, weekly, monthly or yearly tax.");
            Console.WriteLine("The input formats are as follows:");
            Console.WriteLine("The name of a municipality is any string: fx Copenhagen or Høje Taastrup");
            Console.WriteLine("The names are not case sensitive and need to be unique");
            Console.WriteLine("Dates are in the format YYYY.MM.DD");
            Console.WriteLine("For example the 1st of april 2016 will be 2016.04.01");
            Console.WriteLine("The tax format is N.N.");
            Console.WriteLine("For example a tax level could be 0.2");
            //Finding tax tutorial
            Console.WriteLine("Enter C to check a tax");
            Console.WriteLine("You need to enter a municipality and date and the tax in the given municipality on the given day will be shown");
            //Importing tutorial
            Console.WriteLine("Enter I to import data from a file");
            Console.WriteLine("You need to enter the complete path to the file you want to import, both relative and complete works");
            Console.WriteLine("The format is as follows:");
            Console.WriteLine("The first line is the Municipality name");
            Console.WriteLine("Next is a couple of options:");
            Console.WriteLine("The line \"Daily\" will mark the start of Daily records");
            Console.WriteLine("Likewise \"Weekly\", \"Monthly\" or \"Yearly\" will start the appropriate records");
            Console.WriteLine("After this comes the date and tax info: start-date end-date tax");
            Console.WriteLine("The date formats are YYYY.MM.DD and the tax format is N.N");
            Console.WriteLine("A line could look like this:");
            Console.WriteLine("2016.01.01 2016.01.01 0,2");
            Console.WriteLine("When the records for the given municipality are done the last line has to be \"End\" to mark the end of the current Municipality");
            Console.WriteLine("After ending the municipality the next line will be a new municipality name and can continue on like described");
            //Regular info
            Console.WriteLine("Enter H to see the simple help");
            Console.WriteLine("Enter R to see the readme for this project which contains aditional information not covered here");
            Console.WriteLine("Enter Q to exit the program, shutting this interactive client off");
            Console.WriteLine("Enter Q during an action to stop the current action");
        }

        public void GetRecord(){
            //Get the municipality
            Console.WriteLine("Please enther the municipality that you want to get a record from");
            string name = "";
            if(!ReceiveInput(out name)){
                return;
            }

            //Receive the date
            Console.WriteLine("Please enter the date you want in the format of YYYY.MM.DD");
            string date = "";
            if(!ReceiveInput(out date)){
                return;
            }

            //Parse the date and find the tax
            try{
                DateTime time = DateTime.ParseExact(date, "yyyy.MM.dd", CultureInfo.InvariantCulture);
                float res = schedule.GetTax(name,time);
                Console.WriteLine("The tax in {0}, on {1} is: {2}", name, time.ToString("yyyy.MM.dd"), res);
            }
            catch (FormatException e){
                Console.WriteLine("Failed to convert string into a date with the following message: " + e.Message);
            }
            catch (ArgumentException e){
                Console.WriteLine("Failed to find a tax with the following error: " + e.Message);
            }
            return;
        }

        //Helper function for receiving info for a new record, works for all four different tax-types.
        public static bool GetRecordInfo(out string name, out DateTimeRange duration, out float tax){
            //We need to write to the references before returning
            duration = null;
            name = "";
            tax = 0;
            //Get municipality name, dates and tax
            Console.WriteLine("Please enter the name of the Municipality you want to add a daily tax to");
            if(!ReceiveInput(out name)){
                return false;
            }
            Console.WriteLine("Please enter the start date of the tax in the format of YYYY.MM.DD");
            string start = "";
            if(!ReceiveInput(out start)){
                return false;
            }
            Console.WriteLine("Please enter the end date of the tax in the format of YYYY.MM.DD");
            string end = "";
            if(!ReceiveInput(out end)){
                return false;
            }
            Console.WriteLine("Please enter the level of the tax in the format of N.N");
            string level = "";
            if(!ReceiveInput(out level)){
                return false;
            }
            try{
                //Parse the tax value into a float
                tax = float.Parse(level,CultureInfo.InvariantCulture);
            }
            catch{
                Console.WriteLine("Could not parse the tax given");
                return false;
            }
            try{
                duration = ParseFile.StringsToDuration(start,end);
            }
            catch (FormatException e){
                Console.WriteLine("Failed to convert strings into date with the following error message: " + e.Message);
            }
            return true;
        }

        public void AddRecord(){
            Console.WriteLine("What do you want to add to the records?");
            Console.WriteLine("Enter M to add a Municipality");
            Console.WriteLine("Enter D to add a Daily tax to a municipality");
            Console.WriteLine("Enter W to add a Weekly tax to a municipality");
            Console.WriteLine("Enter T to add a Monthly tax to a municipality");
            Console.WriteLine("Enter Y to add a Yearly tax to a municipality");
            string command = Console.ReadLine();
            string name = "";
            //Used in all but the municipality case
            DateTimeRange duration;
            float tax;
            switch (command.ToUpperInvariant()[0])
            {
                case 'M':
                    Console.WriteLine("Please enter the name of the municipality you want to add");
                    if(!ReceiveInput(out name)){
                        break;
                    }
                    //Try to add the municipality and give the according response
                    if(schedule.AddMunicipality(name.ToUpperInvariant())){
                        Console.WriteLine("Municipality {0} was inserted into the tax schedule", name);
                    }
                    else{
                        Console.WriteLine("Something went wrong when inserting Municipality {0}, please make sure that it is not there already, this is NOT case sensitive.", name);
                    }
                    break;
                case 'D':
                    //Get the information for the record
                    if(!GetRecordInfo(out name, out duration, out tax)){
                        break;
                    }
                    if(!this.schedule.AddDaily(name,duration,tax)){
                        Console.WriteLine("Could not add the daily tax you entered");
                    }
                    break;
                case 'W':
                    //Get the information for the record
                    if(!GetRecordInfo(out name, out duration, out tax)){
                        break;
                    }
                    if(!this.schedule.AddWeekly(name,duration,tax)){
                        Console.WriteLine("Could not add the weekly tax you entered");
                    }
                    break;
                case 'T':
                    //Get the information for the record
                    if(!GetRecordInfo(out name, out duration, out tax)){
                        break;
                    }
                    if(!this.schedule.AddMonthly(name,duration,tax)){
                        Console.WriteLine("Could not add the monthly tax you entered");
                    }
                    break;
                case 'Y':
                    //Get the information for the record
                    if(!GetRecordInfo(out name, out duration, out tax)){
                        break;
                    }
                    if(!this.schedule.AddYearly(name,duration,tax)){
                        Console.WriteLine("Could not add the yearly tax you entered");
                    }
                    break;
            }
        }

        public void Import(){
            Console.WriteLine("What file would like to import from?");
            Console.WriteLine("Please include full or complete relative path to the file");
            string path = Console.ReadLine();
            ParseFile parser = new ParseFile(path);
            try{
                parser.Parse(this.schedule);
            }
            catch(ArgumentException e){
                Console.WriteLine("Importing the file failed with error message \"{0}\"", e.Message);
            }
        }

        public void Help(){
            Console.WriteLine("Enter T for a tutorial");
            Console.WriteLine("Enter A to add a record");
            Console.WriteLine("Enter G to get the tax for a given municipality and date");
            Console.WriteLine("Enter I to import data from a file");
            Console.WriteLine("Enter H to see this help again");
            Console.WriteLine("Enter R to see the readme for this project");
            Console.WriteLine("Enter Q to the program");
            Console.WriteLine("Enter Q during an action to stop the current action");
        }

        public void Readme(){
            Console.WriteLine("This is the contents of the readme file for this project:");
            using (StreamReader file = File.OpenText("./readme")){
                Console.Write(file.ReadToEnd());
            }
        }

        public void StartActivity(){
            Console.WriteLine("Hey there.");
            Console.WriteLine("Welcome to this municipality tax program.");
            Console.WriteLine("Please let me know how i can help.");
            Help();
            for(;;){
                Console.WriteLine("Please enter a command");
                string command = Console.ReadLine();
                switch(command.ToUpperInvariant()[0])
                {
                    case 'T':
                        Tutorial();
                        break;
                    case 'A':
                        AddRecord();
                        break;
                    case 'G':
                        GetRecord();
                        break;
                    case 'I':
                        Import();
                        break;
                    case 'H':
                        Help();
                        break;
                    case 'R':
                        Readme();
                        break;
                    case 'Q':
                        Console.WriteLine("Goodbye");
                        return;
                    default:
                        Console.WriteLine("Unknown command, see description below for commands");
                        Help();
                        break;
                }
            }
        }
    }

    public static class API{
        
    }
}
