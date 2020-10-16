using System;
using System.Collections.Generic;

namespace TaxSchedule
{
    public class DateTimeRange{
        private DateTime start;
        private DateTime end;

        public DateTimeRange(DateTime start, DateTime end){
            this.start=start;
            this.end=end;
        }

        public Boolean isDuring(DateTime currentDate){
            return (this.start <= currentDate) && (currentDate <= this.end);
        }
    }

    public class Municipality{
        private Dictionary<DateTimeRange, float> daily;
        private Dictionary<DateTimeRange, float> weekly;
        private Dictionary<DateTimeRange, float> monthly;
        private Dictionary<DateTimeRange, float> yearly;

        public Municipality(Dictionary<DateTimeRange, float> daily,
                            Dictionary<DateTimeRange, float> weekly,
                            Dictionary<DateTimeRange, float> monthly,
                            Dictionary<DateTimeRange, float> yearly)
        {
            this.daily=daily;
            this.weekly=weekly;
            this.monthly=monthly;
            this.yearly=yearly;
        }

        public float getTax(DateTime currentDate){
            //Check daily
            foreach((DateTimeRange duration, float tax) in this.daily){
                if(duration.isDuring(currentDate)){
                    return tax;
                }
            }
            //Check weekly
            foreach((DateTimeRange duration, float tax) in this.weekly){
                if(duration.isDuring(currentDate)){
                    return tax;
                }
            }
            //Check monthly
            foreach((DateTimeRange duration, float tax) in this.monthly){
                if(duration.isDuring(currentDate)){
                    return tax;
                }
            }
            //Check yearly
            foreach((DateTimeRange duration, float tax) in this.yearly){
                if(duration.isDuring(currentDate)){
                    return tax;
                }
            }
            throw new System.ArgumentException("Tax not found");
        }

        public Boolean addDaily(DateTimeRange duration, float tax){
            try{
                daily.Add(duration,tax);
            }
            catch{
                return false;
            }
            return true;
        }

        public Boolean addWeekly(DateTimeRange duration, float tax){
            try{
                weekly.Add(duration,tax);
            }
            catch{
                return false;
            }
            return true;
        }

        public Boolean addMonthly(DateTimeRange duration, float tax){
            try{
                monthly.Add(duration,tax);
            }
            catch{
                return false;
            }
            return true;
        }

        public Boolean addYearly(DateTimeRange duration, float tax){
            try{
                yearly.Add(duration,tax);
            }
            catch{
                return false;
            }
            return true;
        }
    }

    public class TaxSchedule{
        //All municipality names need to be uppercase, this is to make the names case-insensitive
        private Dictionary<string, Municipality> municipalities;

        public TaxSchedule(){
            this.municipalities = new Dictionary<string, Municipality>();
        }

        public TaxSchedule(Dictionary<string, Municipality> municipalities){
            this.municipalities = municipalities;
        }

        //Add a municipality, returns true if it succeeded, false otherwise.
        public Boolean addMunicipality(string name, Municipality municipality){
            try{
                this.municipalities.Add(name.ToUpper(),municipality);
            }
            catch(ArgumentException){
                return false;
            }
            return true;
        }

        public Boolean addDaily(string name, DateTimeRange duration, float tax){
            try{
                return this.municipalities[name.ToUpper()].addDaily(duration,tax);
            }
            catch (KeyNotFoundException) {
                return false;
            }
        }

        public Boolean addWeekly(string name, DateTimeRange duration, float tax){
            try{
                return this.municipalities[name.ToUpper()].addWeekly(duration,tax);
            }
            catch (KeyNotFoundException) {
                return false;
            }
        }

        public Boolean addMonthly(string name, DateTimeRange duration, float tax){
            try{
                return this.municipalities[name.ToUpper()].addMonthly(duration,tax);
            }
            catch (KeyNotFoundException) {
                return false;
            }
        }

        public Boolean addYearly(string name, DateTimeRange duration, float tax){
            try{
                return this.municipalities[name.ToUpper()].addYearly(duration,tax);
            }
            catch (KeyNotFoundException) {
                return false;
            }
        }

        public float getTax(string municipality, DateTime date){
            try{
                return this.municipalities[municipality.ToUpper()].getTax(date);
            }
            catch (KeyNotFoundException) {
                throw new System.ArgumentException("Municipality not found", municipality);
            }
        }
    }
}
