using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;
using System.Globalization;

namespace TaxSchedule
{
    public class DatabaseClient{
        private TaxSchedule schedule{get;}

        public DatabaseClient(TaxSchedule schedule){
            this.schedule = schedule;
        }

        public DatabaseClient(){
            this.schedule=new TaxSchedule();
        }

        public bool ReadFromDatabase(){
            return ReadFromDatabase(this.schedule);
        }

        public static bool ReadFromDatabase(TaxSchedule schedule){
            if (!File.Exists("TaxSchedule.sqlite")){
                return false;
            }

            //Open a connection to the database
            string cs = "Data Source=TaxSchedule.sqlite";
            SQLiteConnection con = new SQLiteConnection(cs);
            con.Open();

            string sql = "select distinct * from schedule";
            SQLiteCommand cmd = new SQLiteCommand(sql, con);
            var ret = cmd.ExecuteReader();
            while (ret.Read())
            {
                //Adds the municipality if it is not already there
                schedule.AddMunicipality(ret.GetString(0));
                //Switches on which list to add it to
                float tax;
                DateTimeRange duration;
                try{
                    //Parse the date values
                    duration = ParseFile.StringsToDuration(ret.GetString(2),ret.GetString(3));
                }
                catch (FormatException){
                    return false;
                }
                tax = ret.GetFloat(4);
                switch(ret.GetString(1).ToUpperInvariant())
                {
                    //The only difference between each case is which list it is added to
                    case "DAILY":
                        schedule.AddDaily(ret.GetString(0),duration,tax);
                        break;
                    case "WEEKLY":
                        schedule.AddWeekly(ret.GetString(0),duration,tax);
                        break;
                    case "MONTHLY":
                        schedule.AddMonthly(ret.GetString(0),duration,tax);
                        break;
                    case "YEARLY":
                        schedule.AddYearly(ret.GetString(0),duration,tax);
                        break;
                    default:
                        return false;
                }
            }
            return true;
        }

        public bool WriteToDatabase(){
            return WriteToDatabase(this.schedule);
        }

        public static bool WriteToDatabase(TaxSchedule schedule){
            //Create/overwrite the database. This is to avoid duplicates
            bool create = !File.Exists("TaxSchedule.sqlite");
            if(create){
                SQLiteConnection.CreateFile("TaxSchedule.sqlite");
            }
            //Open a connection to the database
            string cs = "Data Source=TaxSchedule.sqlite";
            SQLiteConnection con = new SQLiteConnection(cs);
            con.Open();
            string sql;
            SQLiteCommand cmd;

            //Create the table for the values if you created the database
            if(create){
                sql = "create table schedule (municipality varchar(20), type varchar(7), start varchar(10), end varchar(10),tax float)";
                cmd = new SQLiteCommand(sql, con);
                cmd.ExecuteNonQuery();
            }

            //The start of the insert command
            sql = "insert into schedule (municipality, type, start, end, tax)  values ";
            //Generate the variable values for the SQL-insert
            sql += schedule.GenerateSQL();

            //Generate the command and perform it
            cmd = new SQLiteCommand(sql, con);
            cmd.ExecuteNonQuery();

            //Close the connection
            con.Close();
            return true;
        }
    }
}
