using System;
using System.Collections.Generic;
using System.Globalization;

namespace TaxSchedule
{
    public class DateTimeRange{
        private DateTime start;
        private DateTime end;

        public DateTimeRange(DateTime start, DateTime end){
            this.start=start;
            this.end=end;
        }

        public DateTime GetStart(){
            return this.start;
        }

        public DateTime GetEnd(){
            return this.end;
        }

        public Boolean IsDuring(DateTime currentDate){
            return (this.start <= currentDate) && (currentDate <= this.end);
        }
    }

    public class Municipality{
        private List<(DateTimeRange, float)> daily {get;}
        private List<(DateTimeRange, float)> weekly {get;}
        private List<(DateTimeRange, float)> monthly {get;}
        private List<(DateTimeRange, float)> yearly {get;}

        public Municipality(){
            this.daily=new List<(DateTimeRange, float)>();
            this.weekly=new List<(DateTimeRange, float)>();
            this.monthly=new List<(DateTimeRange, float)>();
            this.yearly=new List<(DateTimeRange, float)>();
        }

        public Municipality(List<(DateTimeRange, float)> daily,
                            List<(DateTimeRange, float)> weekly,
                            List<(DateTimeRange, float)> monthly,
                            List<(DateTimeRange, float)> yearly)
        {
            this.daily=daily;
            this.weekly=weekly;
            this.monthly=monthly;
            this.yearly=yearly;
        }

        //Generate SQL from the lists of data. Needs its own name as input
        public string GenerateSQL(string name){
            string sql = "";
            string type = "daily";
            foreach((DateTimeRange time, float tax) in daily){
                sql += string.Format("('{0}', '{1}', '{2}', '{3}', {4}), ",
                                     name,
                                     type,
                                     time.GetStart().ToString("yyyy.MM.dd",CultureInfo.InvariantCulture),
                                     time.GetEnd().ToString("yyyy.MM.dd",CultureInfo.InvariantCulture),
                                     tax.ToString(CultureInfo.InvariantCulture)
                );
            }
            type = "weekly";
            foreach((DateTimeRange time, float tax) in weekly){
                sql += string.Format("('{0}', '{1}', '{2}', '{3}', {4}), ",
                                     name,
                                     type,
                                     time.GetStart().ToString("yyyy.MM.dd",CultureInfo.InvariantCulture),
                                     time.GetEnd().ToString("yyyy.MM.dd",CultureInfo.InvariantCulture),
                                     tax.ToString(CultureInfo.InvariantCulture)
                );
            }
            type = "monthly";
            foreach((DateTimeRange time, float tax) in monthly){
                sql += string.Format("('{0}', '{1}', '{2}', '{3}', {4}), ",
                                     name,
                                     type,
                                     time.GetStart().ToString("yyyy.MM.dd",CultureInfo.InvariantCulture),
                                     time.GetEnd().ToString("yyyy.MM.dd",CultureInfo.InvariantCulture),
                                     tax.ToString(CultureInfo.InvariantCulture)
                );
            }
            type = "yearly";
            foreach((DateTimeRange time, float tax) in yearly){
                sql += string.Format("('{0}', '{1}', '{2}', '{3}', {4}), ",
                                     name,
                                     type,
                                     time.GetStart().ToString("yyyy.MM.dd",CultureInfo.InvariantCulture),
                                     time.GetEnd().ToString("yyyy.MM.dd",CultureInfo.InvariantCulture),
                                     tax.ToString(CultureInfo.InvariantCulture)
                );
            }
            return sql;
        }


        public float GetTax(DateTime currentDate){
            //Check daily
            foreach((DateTimeRange duration, float tax) in this.daily){
                if(duration.IsDuring(currentDate)){
                    return tax;
                }
            }
            //Check weekly
            foreach((DateTimeRange duration, float tax) in this.weekly){
                if(duration.IsDuring(currentDate)){
                    return tax;
                }
            }
            //Check monthly
            foreach((DateTimeRange duration, float tax) in this.monthly){
                if(duration.IsDuring(currentDate)){
                    return tax;
                }
            }
            //Check yearly
            foreach((DateTimeRange duration, float tax) in this.yearly){
                if(duration.IsDuring(currentDate)){
                    return tax;
                }
            }
            throw new ArgumentException("Tax not found");
        }

        public bool AddDaily(DateTimeRange duration, float tax){
            daily.Add((duration,tax));
            return true;
        }

        public bool AddWeekly(DateTimeRange duration, float tax){
            weekly.Add((duration,tax));
            return true;
        }

        public bool AddMonthly(DateTimeRange duration, float tax){
            monthly.Add((duration,tax));
            return true;
        }

        public bool AddYearly(DateTimeRange duration, float tax){
            yearly.Add((duration,tax));
            return true;
        }
    }

    public class TaxSchedule{
        //All municipality names need to be uppercase, this is to make the names case-insensitive
        private Dictionary<string, Municipality> municipalities{get;}

        public TaxSchedule(){
            this.municipalities = new Dictionary<string, Municipality>();
        }

        public TaxSchedule(Dictionary<string, Municipality> municipalities){
            this.municipalities = municipalities;
        }

        public string GenerateSQL(){
            string sql = "";
            //Iterate over municipalities
            foreach(KeyValuePair<string, Municipality> municipality in municipalities){
                //Iterate over each tax-type
                //The municipality does not know it's own name so it needs that info
                string name = municipality.Key;
                sql += municipality.Value.GenerateSQL(name);
            }
            //Remove the last ", "
            sql = sql.Substring(0,sql.Length-2);
            return sql;
        }

        //Add a municipality, returns true if it succeeded, false otherwise.
        public bool AddMunicipality(string name, Municipality municipality){
            return this.municipalities.TryAdd(name.ToUpperInvariant(),municipality);
        }

        public bool AddMunicipality(string name){
            return this.municipalities.TryAdd(name.ToUpperInvariant(),new Municipality());
        }

        public bool AddDaily(string name, DateTimeRange duration, float tax){
            try{
                return this.municipalities[name.ToUpperInvariant()]
                    .AddDaily(duration,tax);
            }
            catch (KeyNotFoundException) {
                return false;
            }
        }

        public bool AddWeekly(string name, DateTimeRange duration, float tax){
            try{
                return this.municipalities[name.ToUpperInvariant()]
                    .AddWeekly(duration,tax);
            }
            catch (KeyNotFoundException) {
                return false;
            }
        }

        public bool AddMonthly(string name, DateTimeRange duration, float tax){
            try{
                return this.municipalities[name.ToUpperInvariant()]
                    .AddMonthly(duration,tax);
            }
            catch (KeyNotFoundException) {
                return false;
            }
        }

        public bool AddYearly(string name, DateTimeRange duration, float tax){
            try{
                return this.municipalities[name.ToUpperInvariant()]
                    .AddYearly(duration,tax);
            }
            catch (KeyNotFoundException) {
                return false;
            }
        }

        public float GetTax(string municipality, DateTime date){
            try{
                return this.municipalities[municipality.ToUpperInvariant()]
                    .GetTax(date);
            }
            catch (KeyNotFoundException) {
                throw new ArgumentException("Municipality not found", municipality);
            }
        }
    }
}
