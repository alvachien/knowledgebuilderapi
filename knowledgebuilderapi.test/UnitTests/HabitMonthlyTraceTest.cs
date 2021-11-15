using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using knowledgebuilderapi.Models;
using System.Collections;
using Xunit.Abstractions;
using System.Text.Json;

namespace knowledgebuilderapi.test.UnitTests
{
    public class HabitMonthlyDateTestData : IXunitSerializable
    {
        public int DateInMonth { get; set; }
        public DateTime RecordDate { get; set; }
        public DateTime ExpectedDate { get; set; }

        public HabitMonthlyDateTestData()
        {
        }
        public HabitMonthlyDateTestData(int dim, DateTime recordDate, DateTime expectDate) : this()
        {
            DateInMonth = dim;
            RecordDate = recordDate;
            ExpectedDate = expectDate;
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            String val = info.GetValue<String>("Value");
            HabitMonthlyDateTestData other = JsonSerializer.Deserialize<HabitMonthlyDateTestData>(val);

            // CaseID = other.CaseID;
            DateInMonth = other.DateInMonth;
            RecordDate = other.RecordDate;
            ExpectedDate = other.ExpectedDate;
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            String val = JsonSerializer.Serialize(this);
            info.AddValue("Value", val, typeof(String));
        }
    }

    public class HabitMonthlyRecordTestData : IXunitSerializable
    {
        public DateTime BeginDate { get; set; }
        public List<UserHabitRecord> RecordList { get; set; }
        public int ExpectedFirstMonthRecordCount { get; set; }
        public int ExpectedSecondMonthRecordCount { get; set; }

        public HabitMonthlyRecordTestData()
        {
            RecordList = new List<UserHabitRecord>();
        }

        public HabitMonthlyRecordTestData(DateTime dtBegin, List<UserHabitRecord> habitRecords, int firstMonthRecordCount, int secondMonthRecordCount) : this()
        {
            BeginDate = dtBegin;
            if (habitRecords.Count > 0)
                RecordList.AddRange(habitRecords);
            ExpectedFirstMonthRecordCount = firstMonthRecordCount;
            ExpectedSecondMonthRecordCount = secondMonthRecordCount;
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            String val = info.GetValue<String>("Value");
            HabitMonthlyRecordTestData other = JsonSerializer.Deserialize<HabitMonthlyRecordTestData>(val);

            // CaseID = other.CaseID;
            BeginDate = other.BeginDate;
            if (other.RecordList.Count > 0)
                RecordList.AddRange(other.RecordList);
            ExpectedFirstMonthRecordCount = other.ExpectedFirstMonthRecordCount;
            ExpectedSecondMonthRecordCount = other.ExpectedSecondMonthRecordCount;
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            String val = JsonSerializer.Serialize(this);
            info.AddValue("Value", val, typeof(String));
        }
    }

    [Collection("KBAPI_UnitTests#1")]
    public class HabitMonthlyTraceTest
    {
        public static TheoryData<HabitMonthlyDateTestData> InputtedDates =>
            new TheoryData<HabitMonthlyDateTestData>
            {
                new HabitMonthlyDateTestData( 1, new DateTime(2021, 11, 1), new DateTime(2021, 10, 1) ),
                new HabitMonthlyDateTestData( 2, new DateTime(2021, 11, 1), new DateTime(2021, 9, 2) ),
                new HabitMonthlyDateTestData( 3, new DateTime(2021, 11, 1), new DateTime(2021, 9, 3) ),
                new HabitMonthlyDateTestData( 4, new DateTime(2021, 11, 1), new DateTime(2021, 9, 4) ),
                new HabitMonthlyDateTestData( 5, new DateTime(2021, 11, 1), new DateTime(2021, 9, 5) ),
                new HabitMonthlyDateTestData( 6, new DateTime(2021, 11, 1), new DateTime(2021, 9, 6) ),
                new HabitMonthlyDateTestData( 7, new DateTime(2021, 11, 1), new DateTime(2021, 9, 7) ),
                new HabitMonthlyDateTestData( 8, new DateTime(2021, 11, 1), new DateTime(2021, 9, 8) ),
                new HabitMonthlyDateTestData( 9, new DateTime(2021, 11, 1), new DateTime(2021, 9, 9) ),
                new HabitMonthlyDateTestData( 10, new DateTime(2021, 11, 1), new DateTime(2021, 9, 10) ),
                new HabitMonthlyDateTestData( 11, new DateTime(2021, 11, 1), new DateTime(2021, 9, 11) ),
                new HabitMonthlyDateTestData( 12, new DateTime(2021, 11, 1), new DateTime(2021, 9, 12) ),
                new HabitMonthlyDateTestData( 13, new DateTime(2021, 11, 1), new DateTime(2021, 9, 13) ),
                new HabitMonthlyDateTestData( 14, new DateTime(2021, 11, 1), new DateTime(2021, 9, 14) ),
                new HabitMonthlyDateTestData( 15, new DateTime(2021, 11, 1), new DateTime(2021, 9, 15) ),
                new HabitMonthlyDateTestData( 16, new DateTime(2021, 11, 1), new DateTime(2021, 9, 16) ),
                new HabitMonthlyDateTestData( 17, new DateTime(2021, 11, 1), new DateTime(2021, 9, 17) ),
                new HabitMonthlyDateTestData( 18, new DateTime(2021, 11, 1), new DateTime(2021, 9, 18) ),
                new HabitMonthlyDateTestData( 19, new DateTime(2021, 11, 1), new DateTime(2021, 9, 19) ),
                new HabitMonthlyDateTestData( 20, new DateTime(2021, 11, 1), new DateTime(2021, 9, 20) ),
                new HabitMonthlyDateTestData( 21, new DateTime(2021, 11, 1), new DateTime(2021, 9, 21) ),
                new HabitMonthlyDateTestData( 22, new DateTime(2021, 11, 1), new DateTime(2021, 9, 22) ),
                new HabitMonthlyDateTestData( 23, new DateTime(2021, 11, 1), new DateTime(2021, 9, 23) ),
                new HabitMonthlyDateTestData( 24, new DateTime(2021, 11, 1), new DateTime(2021, 9, 24) ),
                new HabitMonthlyDateTestData( 25, new DateTime(2021, 11, 1), new DateTime(2021, 9, 25) ),
                new HabitMonthlyDateTestData( 26, new DateTime(2021, 11, 1), new DateTime(2021, 9, 26) ),
                new HabitMonthlyDateTestData( 27, new DateTime(2021, 11, 1), new DateTime(2021, 9, 27) ),
                new HabitMonthlyDateTestData( 28, new DateTime(2021, 11, 1), new DateTime(2021, 9, 28) ),
                new HabitMonthlyDateTestData( 29, new DateTime(2021, 11, 1), new DateTime(2021, 9, 29) ),
            };

        public static TheoryData<HabitMonthlyRecordTestData> InputtedRecords =>
            new TheoryData<HabitMonthlyRecordTestData>
            {
                new HabitMonthlyRecordTestData(new DateTime(2021, 11, 3), new List<UserHabitRecord> { }, 0, 0 ),
                new HabitMonthlyRecordTestData(new DateTime(2021, 11, 3), new List<UserHabitRecord> { 
                    new UserHabitRecord { RecordDate = new DateTime(2021, 11, 7), HabitID = 1, SubID = 1, }
                }, 1, 0),
                new HabitMonthlyRecordTestData(new DateTime(2021, 11, 3), new List<UserHabitRecord> {
                    new UserHabitRecord { RecordDate = new DateTime(2021, 12, 2), HabitID = 1, SubID = 1, }
                }, 1, 0),
                new HabitMonthlyRecordTestData(new DateTime(2021, 11, 3), new List<UserHabitRecord> {
                    new UserHabitRecord { RecordDate = new DateTime(2021, 12, 2), HabitID = 1, SubID = 1 },
                    new UserHabitRecord { RecordDate = new DateTime(2021, 12, 3), HabitID = 1, SubID = 1 }
                }, 1, 1),
            };

        [Theory]
        [MemberData(nameof(InputtedDates))]
        public void getDBSelectionDate(HabitMonthlyDateTestData testData)
        {
            DateTime actDate = HabitMonthlyTrace.getDBSelectionDate(testData.DateInMonth, testData.RecordDate);
            Assert.Equal(testData.ExpectedDate, actDate);
        }

        [Theory]
        [MemberData(nameof(InputtedRecords))]
        public void analyzeUserRecord(HabitMonthlyRecordTestData testData)
        {
            HabitMonthlyTrace firstMonth = new HabitMonthlyTrace();
            HabitMonthlyTrace secondMonth = new HabitMonthlyTrace();

            HabitMonthlyTrace.analyzeUserRecord(testData.RecordList, testData.BeginDate, firstMonth, secondMonth);
            Assert.Equal(testData.ExpectedFirstMonthRecordCount, firstMonth.getRecordCount());
            Assert.Equal(testData.ExpectedSecondMonthRecordCount, secondMonth.getRecordCount());
        }
    }

    //public class HabitMonthlyTraceTestDates : IEnumerable<object[]>
    //{
    //    public IEnumerator<object[]> GetEnumerator()
    //    {
    //        yield return new object[] { 1, new DateTime(2021, 11, 1), new DateTime(2021, 10, 1) };
    //        yield return new object[] { 2, new DateTime(2021, 11, 1), new DateTime(2021, 9, 2) };
    //        yield return new object[] { 3, new DateTime(2021, 11, 1), new DateTime(2021, 9, 3) };
    //        yield return new object[] { 4, new DateTime(2021, 11, 1), new DateTime(2021, 9, 4) };
    //        yield return new object[] { 5, new DateTime(2021, 11, 1), new DateTime(2021, 9, 5) };
    //        yield return new object[] { 6, new DateTime(2021, 11, 1), new DateTime(2021, 9, 6) };
    //        yield return new object[] { 7, new DateTime(2021, 11, 1), new DateTime(2021, 9, 7) };
    //        yield return new object[] { 8, new DateTime(2021, 11, 1), new DateTime(2021, 9, 8) };
    //        yield return new object[] { 9, new DateTime(2021, 11, 1), new DateTime(2021, 9, 9) };
    //        yield return new object[] { 10, new DateTime(2021, 11, 1), new DateTime(2021, 9, 10) };
    //        yield return new object[] { 11, new DateTime(2021, 11, 1), new DateTime(2021, 9, 11) };
    //        yield return new object[] { 12, new DateTime(2021, 11, 1), new DateTime(2021, 9, 12) };
    //        yield return new object[] { 13, new DateTime(2021, 11, 1), new DateTime(2021, 9, 13) };
    //        yield return new object[] { 14, new DateTime(2021, 11, 1), new DateTime(2021, 9, 14) };
    //        yield return new object[] { 15, new DateTime(2021, 11, 1), new DateTime(2021, 9, 15) };
    //        yield return new object[] { 16, new DateTime(2021, 11, 1), new DateTime(2021, 9, 16) };
    //        yield return new object[] { 17, new DateTime(2021, 11, 1), new DateTime(2021, 9, 17) };
    //        yield return new object[] { 18, new DateTime(2021, 11, 1), new DateTime(2021, 9, 18) };
    //        yield return new object[] { 19, new DateTime(2021, 11, 1), new DateTime(2021, 9, 19) };
    //        yield return new object[] { 20, new DateTime(2021, 11, 1), new DateTime(2021, 9, 20) };
    //        yield return new object[] { 21, new DateTime(2021, 11, 1), new DateTime(2021, 9, 21) };
    //        yield return new object[] { 22, new DateTime(2021, 11, 1), new DateTime(2021, 9, 22) };
    //        yield return new object[] { 23, new DateTime(2021, 11, 1), new DateTime(2021, 9, 23) };
    //        yield return new object[] { 24, new DateTime(2021, 11, 1), new DateTime(2021, 9, 24) };
    //        yield return new object[] { 25, new DateTime(2021, 11, 1), new DateTime(2021, 9, 25) };
    //        yield return new object[] { 26, new DateTime(2021, 11, 1), new DateTime(2021, 9, 26) };
    //        yield return new object[] { 27, new DateTime(2021, 11, 1), new DateTime(2021, 9, 27) };
    //        yield return new object[] { 28, new DateTime(2021, 11, 1), new DateTime(2021, 9, 28) };
    //        yield return new object[] { 29, new DateTime(2021, 11, 1), new DateTime(2021, 9, 29) };
    //    }

    //    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    //}

    //public class HabitMonthlyTraceTestMonthRecords : IEnumerable<object[]>
    //{
    //    public IEnumerator<object[]> GetEnumerator()
    //    {
    //        yield return new object[] {
                
    //        };

    //        yield return new object[] {
                
    //        };

    //        yield return new object[]
    //        {
                
    //        };
    //        yield return new object[]
    //        {
                
    //        };
    //    }
    //    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    //}
}
