using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace knowledgebuilderapi.Models
{
    public class HabitWeeklyTrace
    {
        private Dictionary<DayOfWeek, UserHabitRecord> records = new Dictionary<DayOfWeek, UserHabitRecord>();
        
        public DayOfWeek StartWeekDay 
        { 
            get 
            {
                return this.StartDate.DayOfWeek;
            }
        }
        
        // The start date. Such as '2021-11-03';
        public DateTime StartDate { get; set; }

        public HabitWeeklyTrace()
        {
            foreach (DayOfWeek dow in Enum.GetValues(typeof(DayOfWeek)))
                this.records.Add(dow, null);
        }

        public UserHabitRecord getRecord(DayOfWeek dow)
        {
            return this.records[dow];
        }

        public void setRecord(DayOfWeek dow, UserHabitRecord record)
        {
            this.records[dow] = record;
        }

        public void resetRecords()
        {
            foreach (DayOfWeek dow in records.Keys)
                records[dow] = null;
        }

        public int getRecordCount()
        {
            int nCount = 0;
            foreach (UserHabitRecord record in records.Values)
                if (record != null) nCount++;
            return nCount;
        }

        public static DateTime getDBSelectionDate(DayOfWeek dow, DateTime recordDate)
        {
            int nPrvDays = (int)recordDate.DayOfWeek - (int)dow;
            DateTime dtbgn;
            if (nPrvDays >= 0)
                dtbgn = recordDate - TimeSpan.FromDays(7 + nPrvDays);
            else
                dtbgn = recordDate - TimeSpan.FromDays(14 + nPrvDays);

            return dtbgn;
        }

        public static void analyzeUserRecord(List<UserHabitRecord> listRecords, DateTime dtBegin, HabitWeeklyTrace firstWeek, HabitWeeklyTrace secondWeek)
        {
            firstWeek.resetRecords();            
            firstWeek.StartDate = dtBegin;
            secondWeek.resetRecords();
            secondWeek.StartDate = dtBegin.AddDays(7);

            if (listRecords.Count > 0)
            {
                for(int i = 0; i < 14; i++)
                {
                    DateTime dtCur = dtBegin.AddDays(i).Date;

                    var idx = listRecords.FindIndex(rcd => rcd.RecordDate.Date == dtCur);
                    if (i >= 7)
                    {
                        if (idx != -1)
                            secondWeek.setRecord(dtCur.DayOfWeek, listRecords[idx]);
                    }
                    else
                    {
                        if (idx != -1)
                            firstWeek.setRecord(dtCur.DayOfWeek, listRecords[idx]);                        
                    }
                }
            }
        }
    }

    public class HabitMonthlyTrace
    {
        private Dictionary<Int32, UserHabitRecord> records = new Dictionary<int, UserHabitRecord>();

        // The start date. Such as '2021-11-03';
        public DateTime StartDate { get; set; }

        public HabitMonthlyTrace()
        {
            for (int i = 1; i < 32; i++)
                this.records.Add(i, null);
        }

        public UserHabitRecord getRecord(Int32 date)
        {
            return this.records[date];
        }

        public void setRecord(Int32 date, UserHabitRecord record)
        {
            this.records[date] = record;
        }

        public void resetRecords()
        {
            foreach (int nkey in records.Keys)
                records[nkey] = null;
        }

        public int getRecordCount()
        {
            int nCount = 0;
            foreach (UserHabitRecord record in records.Values)
                if (record != null) nCount++;
            return nCount;
        }


        public static DateTime getDBSelectionDate(int dateInMonth, DateTime recordDate)
        {
            int nPrvDays = recordDate.Day - dateInMonth;
            DateTime dtbgn;
            if (nPrvDays >= 0)
                dtbgn = recordDate.AddMonths(-1) - TimeSpan.FromDays(nPrvDays);
            else
                dtbgn = recordDate.AddMonths(-2) - TimeSpan.FromDays(nPrvDays);

            return dtbgn;
        }

        public static void analyzeUserRecord(List<UserHabitRecord> listRecords, DateTime dtBegin, HabitMonthlyTrace firstMonth, HabitMonthlyTrace secondMonth)
        {
            firstMonth.resetRecords();
            firstMonth.StartDate = dtBegin.Date;
            secondMonth.resetRecords();
            secondMonth.StartDate = dtBegin.AddDays(7).Date;

            DateTime dtSecondMonth = dtBegin.AddMonths(1).Date;
            DateTime dtEnd = dtBegin.AddMonths(2).Date;
            if (listRecords.Count > 0)
            {
                for (DateTime dtCur = dtBegin.Date; dtCur < dtEnd; )
                {
                    var idx = listRecords.FindIndex(rcd => rcd.RecordDate.Date == dtCur);
                    if (dtCur >= dtSecondMonth)
                    {
                        if (idx != -1)
                            secondMonth.setRecord(dtCur.Day, listRecords[idx]);
                    }
                    else
                    {
                        if (idx != -1)
                            firstMonth.setRecord(dtCur.Day, listRecords[idx]);
                    }
                }
            }
        }
    }
}
