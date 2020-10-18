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

        public static DateTime StringToDate(string date){
            return DateTime.ParseExact(date, "yyyy.MM.dd", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static DateTimeRange StringsToDuration(string start, string end){
            DateTime starts = StringToDate(start);
            //Make the end time the last second of the end date
            DateTime ends = StringToDate(end)
                .AddDays(1)
                .AddSeconds(-1);
            DateTimeRange duration = new DateTimeRange(starts,ends);
            return duration;
        }

        public TaxSchedule Parse(TaxSchedule schedule){
            if(!File.Exists(this.path)){
                throw new System.ArgumentException("File not found");
            }
            using (StreamReader file = File.OpenText(path)){
                string line;
                int count = 0;
                int state = 0;
                string name = "";
                while ((line = file.ReadLine()) != null){
                    count++;
                    switch (line.ToUpperInvariant())
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
                                    duration = StringsToDuration(ls[0],ls[1]);
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
                                    schedule.AddMunicipality(line.ToUpperInvariant());
                                    name=line;
                                    break;
                                case 1:
                                    //Add the values to the dictionary, catch errors
                                    if(!schedule.AddDaily(name,duration,tax)){
                                        throw new ArgumentException("Error inserting entry from line {count}");
                                    }
                                    break;
                                case 2:
                                    if(!schedule.AddWeekly(name,duration,tax)){
                                        throw new ArgumentException("Error inserting entry from line {count}");
                                    }
                                    break;
                                case 3:
                                    if(!schedule.AddMonthly(name,duration,tax)){
                                        throw new ArgumentException("Error inserting entry from line {count}");
                                    }
                                    break;
                                case 4:
                                    if(!schedule.AddYearly(name,duration,tax)){
                                        throw new ArgumentException("Error inserting entry from line {count}");
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
