using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using knowledgebuilderapi.Models;

namespace knowledgebuilderapi.test.UnitTests
{
    [Collection("KBAPI_UnitTests#1")]
    public class HabitWeeklyTraceTest
    {
        public static IEnumerable<object[]> WeeklyDates
        {
            get
            {
                // Or this could read from a file. :)
                return new[]
                {
                    new object[]
                    {
                        DayOfWeek.Monday,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 10, 25)
                    },
                    new object[] 
                    {
                        DayOfWeek.Tuesday,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 10, 19)
                    },
                    new object[] 
                    {
                        DayOfWeek.Wednesday,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 10, 20)
                    },
                    new object[] 
                    {
                        DayOfWeek.Thursday,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 10, 21)
                    },
                    new object[] 
                    {
                        DayOfWeek.Friday,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 10, 22)
                    },
                    new object[] 
                    {
                        DayOfWeek.Saturday,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 10, 23)
                    },
                    new object[] 
                    {
                        DayOfWeek.Sunday,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 10, 24)
                    }
                };
            }
        }

        public static IEnumerable<object[]> WeeklyHabitRecords
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new DateTime(2021, 11, 3),
                        new List<UserHabitRecord> { },
                        0,
                        0
                    },
                    new object[]
                    {
                        new DateTime(2021, 11, 3),
                        new List<UserHabitRecord>
                        {
                            new UserHabitRecord  { RecordDate = new DateTime(2021, 11, 7) }
                        },
                        1,
                        0
                    },
                    new object[]
                    {
                        new DateTime(2021, 11, 3),
                        new List<UserHabitRecord>
                        {
                            new UserHabitRecord { RecordDate = new DateTime(2021, 11, 9) }
                        },
                        1,
                        0
                    },
                    new object[]
                    {
                        new DateTime(2021, 11, 3),
                        new List<UserHabitRecord>
                        {
                            new UserHabitRecord { RecordDate = new DateTime(2021, 11, 9) },
                            new UserHabitRecord { RecordDate = new DateTime(2021, 11, 10) }
                        },
                        1,
                        1
                    }
                };
            }
        }


        [Theory]
        [MemberData(nameof(WeeklyDates))]
        public void getDBSelectionDate(DayOfWeek dow, DateTime recordDates, DateTime expDate)
        {
            DateTime actDate = HabitWeeklyTrace.getDBSelectionDate(dow, recordDates);
            Assert.Equal(expDate, actDate);
        }

        [Theory]
        [MemberData(nameof(WeeklyHabitRecords))]
        public void analyzeUserRecord(DateTime dtBegin, List<UserHabitRecord> habitRecords, int firstWeekCount, int secondWeekCount)
        {
            HabitWeeklyTrace firstWeek = new HabitWeeklyTrace();
            HabitWeeklyTrace secondWeek = new HabitWeeklyTrace();

            HabitWeeklyTrace.analyzeUserRecord(habitRecords, dtBegin, firstWeek, secondWeek);
            Assert.Equal(firstWeek.getRecordCount(), firstWeekCount);
            Assert.Equal(secondWeek.getRecordCount(), secondWeekCount);
        }
    }
}
