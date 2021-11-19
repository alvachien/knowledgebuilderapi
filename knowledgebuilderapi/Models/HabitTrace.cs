using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace knowledgebuilderapi.Models
{
    public class HabitWeeklyTrace
    {
        private readonly Dictionary<DayOfWeek, List<UserHabitRecord>> dictRecords = new Dictionary<DayOfWeek, List<UserHabitRecord>>();
        
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
                dictRecords.Add(dow, new List<UserHabitRecord>());
        }

        public List<UserHabitRecord> getRecord(DayOfWeek dow)
        {
            return dictRecords[dow];
        }

        public void addRecord(DayOfWeek dow, UserHabitRecord record)
        {
            dictRecords[dow].Add(record);
        }

        public void setRecord(DayOfWeek dow, List<UserHabitRecord> listRecords)
        {
            dictRecords[dow].Clear();
            foreach (UserHabitRecord record in listRecords)
                dictRecords[dow].Add(record);
        }

        public void resetRecords()
        {
            foreach (DayOfWeek dow in dictRecords.Keys)
                dictRecords[dow].Clear();
        }

        public int getRecordCount()
        {
            int nCount = 0;
            foreach (List<UserHabitRecord> record in dictRecords.Values)
                nCount += record.Count;
            return nCount;
        }

        public int? getRuleID()
        {
            int? ruleID = null;

            foreach(KeyValuePair<DayOfWeek, List<UserHabitRecord>> kvp in dictRecords)
            {
                foreach(var record in kvp.Value)
                {
                    if (record.RuleID.HasValue)
                        ruleID = record.RuleID;
                }
            }

            return ruleID;
        }

        public UserHabitRecord getRecordWithRule()
        {
            UserHabitRecord rtnrecord = null;

            foreach (KeyValuePair<DayOfWeek, List<UserHabitRecord>> kvp in dictRecords)
            {
                foreach (var record in kvp.Value)
                {
                    if (record.RuleID.HasValue)
                        rtnrecord = record;
                }
            }
            return rtnrecord;
        }

        public int? getRuleContinuousCount()
        {
            int? cdays = null;
            foreach (KeyValuePair<DayOfWeek, List<UserHabitRecord>> kvp in dictRecords)
            {
                foreach (var record in kvp.Value)
                {
                    if (record.RuleID.HasValue)
                        cdays = record.ContinuousCount;
                }
            }

            return cdays;
        }

        public int getNumberOfTimes()
        {
            int count = 0;
            foreach (KeyValuePair<DayOfWeek, List<UserHabitRecord>> kvp in dictRecords)
            {
                count += kvp.Value.Count;
            }
            return count;
        }

        public int? getNumberOfCount()
        {
            int? cfact = null;
            foreach (KeyValuePair<DayOfWeek, List<UserHabitRecord>> kvp in dictRecords)
            {
                foreach (var record in kvp.Value)
                {
                    if (cfact.HasValue)
                        cfact = cfact.Value + record.CompleteFact.Value;
                    else
                        cfact = record.CompleteFact.Value;
                }
            }
            return cfact;
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

                    foreach(UserHabitRecord record in listRecords)
                    {
                        if (record.RecordDate.Date == dtCur)
                        {
                            if (i >= 7) // Second week
                                secondWeek.addRecord(dtCur.DayOfWeek, record);
                            else // First week
                                firstWeek.addRecord(dtCur.DayOfWeek, record);
                        }
                    }
                }
            }
        }
    }

    public class HabitMonthlyTrace
    {
        private readonly Dictionary<Int32, List<UserHabitRecord>> dictRecords = new Dictionary<int, List<UserHabitRecord>>();

        // The start date. Such as '2021-11-03';
        public DateTime StartDate { get; set; }

        public HabitMonthlyTrace()
        {
            for (int i = 1; i < 32; i++)
                dictRecords.Add(i, new List<UserHabitRecord>());
        }

        public List<UserHabitRecord> getRecord(Int32 date)
        {
            return dictRecords[date];
        }

        public void addRecord(Int32 date, UserHabitRecord record)
        {
            dictRecords[date].Add(record);
        }

        public void resetRecords()
        {
            foreach (int nkey in dictRecords.Keys)
                dictRecords[nkey].Clear();
        }

        public int getRecordCount()
        {
            int nCount = 0;
            foreach (List<UserHabitRecord> record in dictRecords.Values)
                nCount += record.Count;
            return nCount;
        }

        public int? getRuleID()
        {
            int? ruleID = null;

            foreach (KeyValuePair<Int32, List<UserHabitRecord>> kvp in dictRecords)
            {
                foreach (var record in kvp.Value)
                {
                    if (record.RuleID.HasValue)
                        ruleID = record.RuleID;
                }
            }

            return ruleID;
        }

        public UserHabitRecord getRecordWithRule()
        {
            UserHabitRecord rtnrecord = null;

            foreach (KeyValuePair<Int32, List<UserHabitRecord>> kvp in dictRecords)
            {
                foreach (var record in kvp.Value)
                {
                    if (record.RuleID.HasValue)
                        rtnrecord = record;
                }
            }
            return rtnrecord;
        }

        public int? getRuleContinuousCount()
        {
            int? cdays = null;
            foreach (KeyValuePair<Int32, List<UserHabitRecord>> kvp in dictRecords)
            {
                foreach (var record in kvp.Value)
                {
                    if (record.RuleID.HasValue)
                        cdays = record.ContinuousCount;
                }
            }

            return cdays;
        }

        public int getNumberOfTimes()
        {
            int count = 0;
            foreach (KeyValuePair<Int32, List<UserHabitRecord>> kvp in dictRecords)
            {
                count += kvp.Value.Count;
            }
            return count;
        }

        public int? getNumberOfCount()
        {
            int? cfact = null;
            foreach (KeyValuePair<Int32, List<UserHabitRecord>> kvp in dictRecords)
            {
                foreach (var record in kvp.Value)
                {
                    if (cfact.HasValue)
                        cfact = cfact.Value + record.CompleteFact.Value;
                    else
                        cfact = record.CompleteFact.Value;
                }
            }
            return cfact;
        }

        public static DateTime getDBSelectionDate(int dateInMonth, DateTime recordDate)
        {
            if (dateInMonth == 1)
            {
                // Beginning of the month
                return new DateTime(recordDate.Year, recordDate.Month, 1).AddMonths(-1);
            }

            int nPrvDays = recordDate.Day - dateInMonth;
            
            DateTime dtbgn;
            if (nPrvDays >= 0)
                dtbgn = recordDate.AddMonths(-1) - TimeSpan.FromDays(nPrvDays);
            else
                dtbgn = recordDate.AddMonths(-2) - TimeSpan.FromDays(nPrvDays);

            return dtbgn;
        }

        public static void analyzeUserRecord(List<UserHabitRecord> listRecords, DateTime dtBegin, 
            HabitMonthlyTrace firstMonth, HabitMonthlyTrace secondMonth)
        {
            firstMonth.resetRecords();
            firstMonth.StartDate = dtBegin.Date;
            secondMonth.resetRecords();
            DateTime dtSecondMonth = dtBegin.AddMonths(1).Date;
            secondMonth.StartDate = dtSecondMonth;

            DateTime dtEnd = dtBegin.AddMonths(2).Date;

            if (listRecords.Count > 0)
            {                
                for (DateTime dtCur = dtBegin.Date; dtCur < dtEnd; )
                {
                    foreach (UserHabitRecord record in listRecords)
                    {
                        if (record.RecordDate.Date == dtCur)
                        {
                            if (dtCur >= dtSecondMonth) // Second month
                                secondMonth.addRecord(dtCur.Day, record);
                            else // First month
                                firstMonth.addRecord(dtCur.Day, record);
                        }
                    }

                    dtCur = dtCur.AddDays(1).Date;
                }
            }
        }
    }

    public class HabitDailyTrace
    {
        private readonly List<UserHabitRecord> RecordList = new List<UserHabitRecord>();

        public DateTime StartDate { get; set; }
        
        public HabitDailyTrace()
        { }

        public void addRecord(UserHabitRecord record)
        {
            RecordList.Add(record);
        }

        public void setRecord(List<UserHabitRecord> listRecords)
        {
            RecordList.Clear();
            foreach (UserHabitRecord record in listRecords)
                RecordList.Add(record);
        }

        public void resetRecords()
        {
            RecordList.Clear();
        }

        public int getRecordCount()
        {
            return RecordList.Count;
        }

        public int? getRuleID()
        {
            int? ruleID = null;

            foreach (var record in RecordList)
            {
                if (record.RuleID.HasValue)
                    ruleID = record.RuleID;
            }

            return ruleID;
        }

        public UserHabitRecord getRecordWithRule()
        {
            UserHabitRecord rtnrecord = null;

            foreach (var record in RecordList)
            {
                if (record.RuleID.HasValue)
                    rtnrecord = record;
            }

            return rtnrecord;
        }

        public int? getRuleContinuousCount()
        {
            int? cdays = null;

            foreach (var record in RecordList)
            {
                if (record.RuleID.HasValue)
                    cdays = record.ContinuousCount;
            }

            return cdays;
        }

        public int getNumberOfTimes()
        {
            return getRecordCount();
        }

        public int? getNumberOfCount()
        {
            int? cfact = null;

            foreach (var record in RecordList)
            {
                if (cfact.HasValue)
                    cfact = cfact.Value + record.CompleteFact.Value;
                else
                    cfact = record.CompleteFact.Value;
            }

            return cfact;
        }

        public static void analyzeUserRecord(List<UserHabitRecord> listRecords, DateTime dtBegin, HabitDailyTrace yesterday, HabitDailyTrace today)
        {
            yesterday.resetRecords();
            yesterday.StartDate = dtBegin;
            today.resetRecords();
            today.StartDate = dtBegin.AddDays(1);

            if (listRecords.Count > 0)
            {
                listRecords.ForEach(record =>
                {
                    if (record.RecordDate == yesterday.StartDate)
                        yesterday.addRecord(record);
                    else if(record.RecordDate == today.StartDate)
                        today.addRecord(record);
                });
            }
        }
    }
}
