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
    public class HabitMonthlyTraceTest
    {
        public static IEnumerable<object[]> MonthlyDates
        {
            get
            {
                // Or this could read from a file. :)
                return new[]
                {
                    new object[] {
                        1,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 10, 1)
                    },
                    new object[] {
                        2,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 2)
                    },
                    new object[] {
                        3,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 3)
                    },
                    new object[] {
                        4,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 4)
                    },
                    new object[] {
                        5,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 5)
                    },
                    new object[] {
                        6,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 6)
                    },
                    new object[] {
                        7,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 7)
                    },
                    new object[] {
                        8,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 8)
                    },
                    new object[] {
                        9,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 9)
                    },
                    new object[] {
                        10,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 10)
                    },
                    new object[] {
                        11,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 11)
                    },
                    new object[] {
                        12,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 12)
                    },
                    new object[] {
                        13,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 13)
                    },
                    new object[] {
                        14,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 14)
                    },
                    new object[] {
                        15,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 15)
                    },
                    new object[] {
                        16,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 16)
                    },
                    new object[] {
                        17,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 17)
                    },
                    new object[] {
                        18,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 18)
                    },
                    new object[] {
                        19,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 19)
                    },
                    new object[] {
                        20,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 20)
                    },
                    new object[] {
                        21,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 21)
                    },
                    new object[] {
                        22,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 22)
                    },
                    new object[] {
                        23,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 23)
                    },
                    new object[] {
                        24,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 24)
                    },
                    new object[] {
                        25,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 25)
                    },
                    new object[] {
                        26,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 26)
                    },
                    new object[] {
                        27,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 27)
                    },
                    new object[] {
                        28,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 28)
                    },
                    new object[] {
                        29,
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 9, 29)
                    }
                };
            }
        }

        public static IEnumerable<object[]> MonthlyHabitRecords
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new List<UserHabitRecord>
                        {
                        },
                        new DateTime(2021, 11, 3),
                        0,
                        0
                    },
                    new object[]
                    {
                        new List<UserHabitRecord>
                        {
                            new UserHabitRecord
                            {
                                RecordDate = new DateTime(2021, 11, 7)
                            }
                        },
                        new DateTime(2021, 11, 3),
                        1,
                        0
                    },
                    new object[]
                    {
                        new List<UserHabitRecord>
                        {
                            new UserHabitRecord
                            {
                                RecordDate = new DateTime(2021, 12, 2)
                            }
                        },
                        new DateTime(2021, 11, 3),
                        1,
                        0
                    },
                    new object[]
                    {
                        new List<UserHabitRecord>
                        {
                            new UserHabitRecord
                            {
                                RecordDate = new DateTime(2021, 12, 2)
                            },
                            new UserHabitRecord
                            {
                                RecordDate = new DateTime(2021, 12, 3)
                            }
                        },
                        new DateTime(2021, 11, 3),
                        1,
                        1
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(MonthlyDates))]
        public void getDBSelectionDate(int dateInMonth, DateTime recordDates, DateTime expDate)
        {
            DateTime actDate = HabitMonthlyTrace.getDBSelectionDate(dateInMonth, recordDates);
            Assert.Equal(expDate, actDate);
        }

        [Theory]
        [MemberData(nameof(MonthlyHabitRecords))]
        public void analyzeUserRecord(List<UserHabitRecord> habitRecords, DateTime dtBegin, int firstMonthCount, int secondMonthCount)
        {
            HabitMonthlyTrace firstMonth = new HabitMonthlyTrace();
            HabitMonthlyTrace secondMonth = new HabitMonthlyTrace();

            HabitMonthlyTrace.analyzeUserRecord(habitRecords, dtBegin, firstMonth, secondMonth);
            Assert.Equal(firstMonth.getRecordCount(), firstMonthCount);
            Assert.Equal(secondMonth.getRecordCount(), secondMonthCount);
        }
    }
}
