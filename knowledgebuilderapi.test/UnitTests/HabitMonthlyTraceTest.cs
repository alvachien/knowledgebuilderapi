using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using knowledgebuilderapi.Models;
using System.Collections;

namespace knowledgebuilderapi.test.UnitTests
{
    [Collection("KBAPI_UnitTests#1")]
    public class HabitMonthlyTraceTest
    {
        //public static IEnumerable<object[]> MonthlyHabitRecords
        //{
        //    get
        //    {
        //        return new[]
        //        {
        //            new object[] {
        //                new List<UserHabitRecord>
        //                {
        //                    new UserHabitRecord
        //                    {
        //                        RecordDate = new DateTime(2021, 11, 7),
        //                        HabitID = 1,
        //                        SubID = 1,
        //                    }
        //                },
        //                new DateTime(2021, 11, 3), 1, 0
        //            }
        //        };
        //    }
        //}

        [Theory]
        //[MemberData(nameof(MonthlyDates))]
        [ClassData(typeof(HabitMonthlyTraceTestDates))]
        public void getDBSelectionDate(int dateInMonth, DateTime recordDates, DateTime expDate)
        {
            DateTime actDate = HabitMonthlyTrace.getDBSelectionDate(dateInMonth, recordDates);
            Assert.Equal(expDate, actDate);
        }

        [Theory]
        //[MemberData(nameof(MonthlyHabitRecords))]
        [ClassData(typeof(HabitMonthlyTraceTestMonthRecords))]
        public void analyzeUserRecord(DateTime dtBegin, List<UserHabitRecord> habitRecords, int firstMonthRecordCount, int secondMonthRecordCount)
        {
            HabitMonthlyTrace firstMonth = new HabitMonthlyTrace();
            HabitMonthlyTrace secondMonth = new HabitMonthlyTrace();

            HabitMonthlyTrace.analyzeUserRecord(habitRecords, dtBegin, firstMonth, secondMonth);
            Assert.Equal(firstMonthRecordCount, firstMonth.getRecordCount());
            Assert.Equal(secondMonthRecordCount, secondMonth.getRecordCount());
        }
    }

    public class HabitMonthlyTraceTestDates : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 1, new DateTime(2021, 11, 1), new DateTime(2021, 10, 1) };
            yield return new object[] { 2, new DateTime(2021, 11, 1), new DateTime(2021, 9, 2) };
            yield return new object[] { 3, new DateTime(2021, 11, 1), new DateTime(2021, 9, 3) };
            yield return new object[] { 4, new DateTime(2021, 11, 1), new DateTime(2021, 9, 4) };
            yield return new object[] { 5, new DateTime(2021, 11, 1), new DateTime(2021, 9, 5) };
            yield return new object[] { 6, new DateTime(2021, 11, 1), new DateTime(2021, 9, 6) };
            yield return new object[] { 7, new DateTime(2021, 11, 1), new DateTime(2021, 9, 7) };
            yield return new object[] { 8, new DateTime(2021, 11, 1), new DateTime(2021, 9, 8) };
            yield return new object[] { 9, new DateTime(2021, 11, 1), new DateTime(2021, 9, 9) };
            yield return new object[] { 10, new DateTime(2021, 11, 1), new DateTime(2021, 9, 10) };
            yield return new object[] { 11, new DateTime(2021, 11, 1), new DateTime(2021, 9, 11) };
            yield return new object[] { 12, new DateTime(2021, 11, 1), new DateTime(2021, 9, 12) };
            yield return new object[] { 13, new DateTime(2021, 11, 1), new DateTime(2021, 9, 13) };
            yield return new object[] { 14, new DateTime(2021, 11, 1), new DateTime(2021, 9, 14) };
            yield return new object[] { 15, new DateTime(2021, 11, 1), new DateTime(2021, 9, 15) };
            yield return new object[] { 16, new DateTime(2021, 11, 1), new DateTime(2021, 9, 16) };
            yield return new object[] { 17, new DateTime(2021, 11, 1), new DateTime(2021, 9, 17) };
            yield return new object[] { 18, new DateTime(2021, 11, 1), new DateTime(2021, 9, 18) };
            yield return new object[] { 19, new DateTime(2021, 11, 1), new DateTime(2021, 9, 19) };
            yield return new object[] { 20, new DateTime(2021, 11, 1), new DateTime(2021, 9, 20) };
            yield return new object[] { 21, new DateTime(2021, 11, 1), new DateTime(2021, 9, 21) };
            yield return new object[] { 22, new DateTime(2021, 11, 1), new DateTime(2021, 9, 22) };
            yield return new object[] { 23, new DateTime(2021, 11, 1), new DateTime(2021, 9, 23) };
            yield return new object[] { 24, new DateTime(2021, 11, 1), new DateTime(2021, 9, 24) };
            yield return new object[] { 25, new DateTime(2021, 11, 1), new DateTime(2021, 9, 25) };
            yield return new object[] { 26, new DateTime(2021, 11, 1), new DateTime(2021, 9, 26) };
            yield return new object[] { 27, new DateTime(2021, 11, 1), new DateTime(2021, 9, 27) };
            yield return new object[] { 28, new DateTime(2021, 11, 1), new DateTime(2021, 9, 28) };
            yield return new object[] { 29, new DateTime(2021, 11, 1), new DateTime(2021, 9, 29) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class HabitMonthlyTraceTestMonthRecords : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new DateTime(2021, 11, 3),
                new List<UserHabitRecord> { },
                 0, 0 
            };

            yield return new object[] {
                new DateTime(2021, 11, 3),
                new List<UserHabitRecord>
                {
                    new UserHabitRecord
                    {
                        RecordDate = new DateTime(2021, 11, 7),
                        HabitID = 1,
                        SubID = 1,
                    }
                },
                1, 0
            };

            yield return new object[]
            {
                new DateTime(2021, 11, 3),
                new List<UserHabitRecord>
                {
                    new UserHabitRecord
                    {
                        RecordDate = new DateTime(2021, 12, 2),
                        HabitID = 1,
                        SubID = 1,
                    }
                },                
                1,
                0
            };
            yield return new object[]
            {
                new DateTime(2021, 11, 3),
                new List<UserHabitRecord>
                {
                    new UserHabitRecord
                    {
                        RecordDate = new DateTime(2021, 12, 2),
                        HabitID = 1,
                        SubID = 1
                    },
                    new UserHabitRecord
                    {
                        RecordDate = new DateTime(2021, 12, 3),
                        HabitID = 1,
                        SubID = 1
                    }
                },                
                1,
                1
            };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
