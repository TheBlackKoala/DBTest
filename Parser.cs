using System;
using System.IO;
using System.Collections.Generic;

namespace TaxSchedule
{
    public class dateParser{

    }
    public class ParseFile{
        private string path;

        public ParseFile(string path){
            this.path=path;
        }

        public DateTime stringToDate(string date){
            string[] times = date.Split('.',6);
            DateTime time = new DateTime(Int32.Parse(times[0]),
                                         Int32.Parse(times[1]),
                                         Int32.Parse(times[2]),
                                         Int32.Parse(times[3]),
                                         Int32.Parse(times[4]),
                                         Int32.Parse(times[5]));
            return time;
        }

        public DateTimeRange stringsToDuration(string start, string end){
            DateTime starts = stringToDate(start+".0.0.0");
            DateTime ends = stringToDate(end+".23.59.59");
            DateTimeRange duration = new DateTimeRange(starts,ends);
            return duration;
        }

        public TaxSchedule parse(TaxSchedule schedule){
            if(!File.Exists(this.path)){
                throw new System.ArgumentException("File not found");
            }
            Dictionary<string, Municipality> municipalities = new Dictionary<string, Municipality>();
            using (StreamReader file = File.OpenText(path)){
                string line;
                int count = 0;
                int state = 0;
                string name = "";
                while ((line = file.ReadLine()) != null){
                    count++;
                    switch (line.ToUpper())
                    {
                        case "DAILY":
                            state = 1;
                            continue;
                        case "WEEKLY":
                            state = 2;
                            continue;
                        case "MONTHLY":
                            state = 3;
                            continue;
                        case "YEARLY":
                            state = 4;
                            continue;
                        case "END":
                            state=0;
                            continue;
                        default:
                            //Values in scope for later cases
                            string[] ls;
                            DateTimeRange duration = null;
                            float tax = 0;
                            //State 0 is finding the name - all others are finding duration values
                            if(state>0){
                                //Split the line into start-date, end-date and tax
                                ls = line.Split(" ",3);
                                try{
                                    //Create the duration
                                    duration = stringsToDuration(ls[0],ls[1]);
                                    //Find the tax value
                                    tax = float.Parse(ls[2]);
                                }
                                catch(FormatException){
                                    throw new System.ArgumentException("Error converting date or tax on line " + count.ToString());
                                }
                            }
                            switch (state)
                            {
                                case 0:
                                    schedule.addMunicipality(line.ToUpper(), new Municipality());
                                    name=line;
                                    break;
                                case 1:
                                    try{
                                        //Add the values to the dictionary
                                        schedule.addDaily(name,duration,tax);
                                    }
                                    catch (ArgumentException){
                                        throw new System.ArgumentException("Error inserting entry from line" + count.ToString());
                                    }
                                    break;
                                case 2:
                                    try{
                                        //Add the values to the dictionary
                                        schedule.addWeekly(name,duration,tax);
                                    }
                                    catch (ArgumentException){
                                        throw new System.ArgumentException("Error inserting entry from line" + count.ToString());
                                    }
                                    break;
                                case 3:
                                    try{
                                        //Add the values to the dictionary
                                        schedule.addMonthly(name,duration,tax);
                                    }
                                    catch (ArgumentException){
                                        throw new System.ArgumentException("Error inserting entry from line" + count.ToString());
                                    }
                                    break;
                                case 4:
                                    try{
                                        //Add the values to the dictionary
                                        schedule.addYearly(name,duration,tax);
                                    }
                                    catch (ArgumentException){
                                        throw new System.ArgumentException("Error inserting entry from line" + count.ToString());
                                    }
                                    break;
                            }
                            break;
                    }
                }
            }
            return schedule;
        }
    }
}
