using System;
using System.Collections.Generic;

namespace TaxSchedule
{
    public static class TaxInstance{
        public TaxInstance(DateTime start, DateTine end, float tax){
            this.start=start;
            this.end=end;
            this.tax=tax;
        }

        private DateTime start;
        private DateTime end;
        private float tax;

        public DateTime getStartDate(){
            return this.start;
        }

        public DateTime getEndDate(){
            return this.end;
        }

        public float getTax(){
            return this.tax;
        }

        public boolean isDuring(DateTime currentDate){
            return (this.start <= currentDate) && (currentDate <= this.end);
        }
    }

    public static class Municipality{
        private Dictionary<TaxInstance> yearly;
        private Dictionary<TaxInstance> monthly;
        private Dictionary<TaxInstance> weekly;
        private Dictionary<TaxInstance> daily;

        public Municipality(string name,
                            Dictionary<TaxInstance> daily,
                            Dictionary<TaxInstance> weekly,
                            Dictionary<TaxInstance> monthly,
                            Dictionary<TaxInstance> yearly)
        {
            
        }

        public float getTax(DateTime currentDate){
            //Check daily

            //Check weekly

            //Check monthly

            //Check yearly

            //Return -infinity if there is no tax scheduled.
            return float.NegativeInfinity;
        }
    }

    public static class TaxSchedule{
        private Dictionary<string, Municipality> municipalities;

        public TaxSchedule(){
            this.municipalities = new Dictionary<string, Municipality>();
        }

        public TaxSchedule(Dictionary<string, Municipality> municipalities){
            this.municipalities = municipalities;
        }

        //Add a municipality, returns true if it succeeded, false otherwise.
        public boolean addMunicipality(string name, Municipality municipality){
            try{
                this.municipalities.Add(name,municipality);
            }
            catch(ArgumentException){
                return false;
            }
            return true;
        }
    }
}
