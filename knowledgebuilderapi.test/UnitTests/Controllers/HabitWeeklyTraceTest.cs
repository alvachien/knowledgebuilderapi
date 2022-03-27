using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using knowledgebuilderapi.Models;
using Xunit.Abstractions;
using System.Text.Json;

namespace knowledgebuilderapi.test.unittest
{
    public class HabitWeeklyDateTestData : IXunitSerializable
    {
        public DayOfWeek Dow { get; set; }
        public DateTime RecordDate { get; set; }
        public DateTime ExpectedDate { get; set; }

        public HabitWeeklyDateTestData()
        {
        }
        public HabitWeeklyDateTestData(DayOfWeek dow, DateTime recordDate, DateTime expectDate): this()
        {
            Dow = dow;
            RecordDate = recordDate;
            ExpectedDate = expectDate;
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            String val = info.GetValue<String>("Value");
            HabitWeeklyDateTestData other = JsonSerializer.Deserialize<HabitWeeklyDateTestData>(val);

            // CaseID = other.CaseID;
            Dow = other.Dow;
            RecordDate = other.RecordDate;
            ExpectedDate = other.ExpectedDate;
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            String val = JsonSerializer.Serialize(this);
            info.AddValue("Value", val, typeof(String));
        }
    }

    public class HabitWeeklyRecordTestData : IXunitSerializable
    {
        public DateTime BeginDate { get; set; }
        public List<UserHabitRecord> RecordList { get; set; }
        public Int32 ExpectedFirstWeekCount { get; set; }
        public Int32 ExpectedSecondWeekCount { get; set; }

        public HabitWeeklyRecordTestData()
        {
            RecordList = new List<UserHabitRecord>();
        }
        public HabitWeeklyRecordTestData(DateTime bgndate, List<UserHabitRecord> listRecord, int firstWeek, int secondWeek): this()
        {
            BeginDate = bgndate;
            if (listRecord.Count > 0)
                RecordList.AddRange(listRecord);
            ExpectedFirstWeekCount = firstWeek;
            ExpectedSecondWeekCount = secondWeek;
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            String val = info.GetValue<String>("Value");
            HabitWeeklyRecordTestData other = JsonSerializer.Deserialize<HabitWeeklyRecordTestData>(val);

            // CaseID = other.CaseID;
            BeginDate = other.BeginDate;
            if (other.RecordList.Count > 0)
                RecordList.AddRange(other.RecordList);
            ExpectedFirstWeekCount = other.ExpectedFirstWeekCount;
            ExpectedSecondWeekCount = other.ExpectedSecondWeekCount;
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            String val = JsonSerializer.Serialize(this);
            info.AddValue("Value", val, typeof(String));
        }
    }

    [Collection("KBAPI_UnitTests#1")]
    public class HabitWeeklyTraceTest
    {
        public static TheoryData<HabitWeeklyDateTestData> InputtedDates =>
            new TheoryData<HabitWeeklyDateTestData>
            {
                new HabitWeeklyDateTestData(DayOfWeek.Monday, new DateTime(2021, 11, 1), new DateTime(2021, 10, 25)),
                new HabitWeeklyDateTestData(DayOfWeek.Tuesday, new DateTime(2021, 11, 1), new DateTime(2021, 10, 19)),
                new HabitWeeklyDateTestData(DayOfWeek.Wednesday, new DateTime(2021, 11, 1), new DateTime(2021, 10, 20)),
                new HabitWeeklyDateTestData(DayOfWeek.Thursday, new DateTime(2021, 11, 1), new DateTime(2021, 10, 21)),
                new HabitWeeklyDateTestData(DayOfWeek.Friday, new DateTime(2021, 11, 1), new DateTime(2021, 10, 22)),
                new HabitWeeklyDateTestData(DayOfWeek.Saturday, new DateTime(2021, 11, 1), new DateTime(2021, 10, 23)),
                new HabitWeeklyDateTestData(DayOfWeek.Sunday, new DateTime(2021, 11, 1), new DateTime(2021, 10, 24)),
            };

        public static TheoryData<HabitWeeklyRecordTestData> InputtedRecords =>
            new TheoryData<HabitWeeklyRecordTestData>
            {
                new HabitWeeklyRecordTestData(new DateTime(2021, 11, 3), new List<UserHabitRecord> { }, 0, 0),
                new HabitWeeklyRecordTestData(new DateTime(2021, 11, 3), new List<UserHabitRecord> { new UserHabitRecord  { RecordDate = new DateTime(2021, 11, 7) } },
                    1, 0),
                new HabitWeeklyRecordTestData(new DateTime(2021, 11, 3), new List<UserHabitRecord> { new UserHabitRecord { RecordDate = new DateTime(2021, 11, 9) } },
                    1, 0),
                new HabitWeeklyRecordTestData(new DateTime(2021, 11, 3), new List<UserHabitRecord> { 
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 9) }, 
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 10) }
                    }, 1, 1),
                new HabitWeeklyRecordTestData(new DateTime(2021, 11, 3), new List<UserHabitRecord> {
                            new UserHabitRecord { RecordDate = new DateTime(2021, 11, 9), SubID = 1 },
                            new UserHabitRecord { RecordDate = new DateTime(2021, 11, 9), SubID = 2 },
                            new UserHabitRecord { RecordDate = new DateTime(2021, 11, 10), SubID = 1 },
                            new UserHabitRecord { RecordDate = new DateTime(2021, 11, 10), SubID = 2 },
                            new UserHabitRecord { RecordDate = new DateTime(2021, 11, 10), SubID = 3 },
                    }, 2, 3),
            };

        [Theory]
        [MemberData(nameof(InputtedDates))]
        public void getDBSelectionDate(HabitWeeklyDateTestData testData)
        {
            DateTime actDate = HabitWeeklyTrace.getDBSelectionDate(testData.Dow, testData.RecordDate);
            Assert.Equal(testData.ExpectedDate, actDate);
        }

        [Theory]
        [MemberData(nameof(InputtedRecords))]
        public void analyzeUserRecord(HabitWeeklyRecordTestData testData)
        {
            HabitWeeklyTrace firstWeek = new HabitWeeklyTrace();
            HabitWeeklyTrace secondWeek = new HabitWeeklyTrace();

            HabitWeeklyTrace.analyzeUserRecord(testData.RecordList, testData.BeginDate, firstWeek, secondWeek);
            Assert.Equal(testData.ExpectedFirstWeekCount, firstWeek.getRecordCount());
            Assert.Equal(testData.ExpectedSecondWeekCount, secondWeek.getRecordCount());
        }
    }
}
