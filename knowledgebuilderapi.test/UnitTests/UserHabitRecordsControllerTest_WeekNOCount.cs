using System;
using Xunit;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using knowledgebuilderapi.Models;
using knowledgebuilderapi.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace knowledgebuilderapi.test.UnitTests
{
    public class UserHabitRecordsControllerTestData_WeekNoOfCount : IXunitSerializable
    {
        public DayOfWeek Dow { get; private set; }
        public List<UserHabitRecord> RecordList { get; private set; }
        public int CompleteCondition { get; private set; }
        public int RecordCount { get; private set; }
        public List<DateTime> TargetRuleDateList { get; private set; }
        public Guid InstanceID { get; private set; }

        public UserHabitRecordsControllerTestData_WeekNoOfCount()
        {
            this.RecordList = new List<UserHabitRecord>();
            this.TargetRuleDateList = new List<DateTime>();
            this.InstanceID = new Guid();
        }
        public UserHabitRecordsControllerTestData_WeekNoOfCount(DayOfWeek dow,
            List<UserHabitRecord> records, int completeCondition, int recordCount,
            List<DateTime> arTargetRuleDate) : this()
        {
            this.Dow = dow;
            this.RecordList.AddRange(records);
            this.CompleteCondition = completeCondition;
            this.RecordCount = recordCount;
            this.TargetRuleDateList.AddRange(arTargetRuleDate);
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            Dow = info.GetValue<DayOfWeek>("Dow");
            //RecordList = info.GetValue<List<UserHabitRecord>>("RecordList");
            CompleteCondition = info.GetValue<int>("CompleteCondition");
            RecordCount = info.GetValue<int>("RecordCount");
            TargetRuleDateList = info.GetValue<List<DateTime>>("TargetRuleDateList");
            InstanceID = Guid.Parse(info.GetValue<String>(nameof(InstanceID)));
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(Dow), Dow, typeof(DayOfWeek));
            //info.AddValue("RecordList", RecordList, typeof(List<UserHabitRecord>));
            info.AddValue("CompleteCondition", CompleteCondition, typeof(int));
            info.AddValue("RecordCount", RecordCount, typeof(int));
            info.AddValue("TargetRuleDateList", TargetRuleDateList, typeof(List<DateTime>));
            info.AddValue(nameof(InstanceID), InstanceID.ToString("N"), typeof(String));
        }

        public override string ToString()
        {
            return this.InstanceID.ToString("N");
        }
    }

    [Collection("KBAPI_UnitTests#1")]
    public class UserHabitRecordsControllerTest_WeekNOCount : UserHabitRecordsControllerTestAbtract
    {
        public UserHabitRecordsControllerTest_WeekNOCount(SqliteDatabaseFixture fixture)
            : base(fixture)
        {
        }

        public static TheoryData<UserHabitRecordsControllerTestData_WeekNoOfCount> InputtedData =>
            new TheoryData<UserHabitRecordsControllerTestData_WeekNoOfCount>
            {
                // Target: 100 exercises per week
                new UserHabitRecordsControllerTestData_WeekNoOfCount(
                    DayOfWeek.Monday,
                    new List<UserHabitRecord> { new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1 ), CompleteFact = 100, Comment = "Test1" } },
                    100, 1, new List<DateTime> { new DateTime(2021, 11, 1) }),
                new UserHabitRecordsControllerTestData_WeekNoOfCount(
                    DayOfWeek.Monday,
                    new List<UserHabitRecord> { new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1 ), CompleteFact = 90, Comment = "Test1" } },
                    100, 1, new List<DateTime> {} ),
                new UserHabitRecordsControllerTestData_WeekNoOfCount(
                    DayOfWeek.Monday,
                    new List<UserHabitRecord> { new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1 ), CompleteFact = 110, Comment = "Test1" } },
                    100, 1, new List<DateTime> { new DateTime(2021, 11, 1) } )
            };

        //    public static IEnumerable<object[]> GetInputtedData()
        //    {
        //        // Target: 100 exercises per week


        //        yield return new object[] { new UserHabitRecordsControllerTestData_WeekNoOfCount(
        //            DayOfWeek.Monday,
        //            new List<UserHabitRecord> { 
        //                new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, CompleteFact = 30, Comment = "Test1" },
        //                new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, CompleteFact = 30, Comment = "Test1" },
        //                new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), SubID = 1, CompleteFact = 40, Comment = "Test1" }, },
        //            100, 3, new List<DateTime> { new DateTime(2021, 11, 3) })
        //        };
        //        yield return new object[] { new UserHabitRecordsControllerTestData_WeekNoOfCount(
        //            DayOfWeek.Monday,
        //            new List<UserHabitRecord> {
        //                new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, CompleteFact = 30, Comment = "Test1" },
        //                new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, CompleteFact = 30, Comment = "Test1" },
        //                new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 2, CompleteFact = 40, Comment = "Test1" }, },
        //            100, 3, new List<DateTime> { new DateTime(2021, 11, 2) })
        //        };
        //        yield return new object[] { new UserHabitRecordsControllerTestData_WeekNoOfCount(
        //            DayOfWeek.Monday,
        //            new List<UserHabitRecord> {
        //                new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, CompleteFact = 30, Comment = "Test1" },
        //                new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, CompleteFact = 30, Comment = "Test2" },
        //                new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 2, CompleteFact = 40, Comment = "Test2" },
        //                new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), SubID = 1, CompleteFact = 20, Comment = "Test3" }, },
        //            100, 4, new List<DateTime> { new DateTime(2021, 11, 3) })
        //        };
        //        yield return new object[] { new UserHabitRecordsControllerTestData_WeekNoOfCount(
        //            DayOfWeek.Monday,
        //            new List<UserHabitRecord> {
        //                new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, CompleteFact = 30, Comment = "Test1" },
        //                new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, CompleteFact = 30, Comment = "Test2" },
        //                new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 2, CompleteFact = 40, Comment = "Test2" },
        //                new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), SubID = 1, CompleteFact = 20, Comment = "Test3" },

        //                new UserHabitRecord { RecordDate = new DateTime(2021, 11, 8), SubID = 1, CompleteFact = 30, Comment = "Test1" },
        //                new UserHabitRecord { RecordDate = new DateTime(2021, 11, 9), SubID = 1, CompleteFact = 30, Comment = "Test2" },
        //                new UserHabitRecord { RecordDate = new DateTime(2021, 11, 9), SubID = 2, CompleteFact = 40, Comment = "Test2" },
        //                new UserHabitRecord { RecordDate = new DateTime(2021, 11, 10), SubID = 1, CompleteFact = 20, Comment = "Test3" }, },
        //            100, 8, new List<DateTime> { new DateTime(2021, 11, 3), new DateTime(2021, 11, 10) })
        //        };
        //    }

        [Theory]
        [MemberData(nameof(InputtedData), MemberType = typeof(UserHabitRecordsControllerTest_WeekNOCount))]
        public async Task CalculatePoints_Weekly_NumberOfCounts(UserHabitRecordsControllerTestData_WeekNoOfCount testData)
        {
            var context = this.fixture.GetCurrentDataContext();

            UserHabitRecordsController control = new(context);

            // Add Habit, Habit Rules
            UserHabit habit = new UserHabit();
            habit.TargetUser = test_user1;
            habit.ValidFrom = new DateTime(2021, 1, 1);
            habit.ValidTo = new DateTime(2022, 12, 31);
            habit.Name = "Habit_1";
            habit.Category = HabitCategory.Positive;
            habit.Comment = habit.Name;
            habit.Frequency = HabitFrequency.Weekly;
            habit.CompleteCategory = HabitCompleteCategory.NumberOfCount;
            habit.CompleteCondition = testData.CompleteCondition;
            habit.StartDate = (int)testData.Dow;
            context.UserHabits.Add(habit);
            context.SaveChanges();
            Int32 nNewHabitID = habit.ID;

            UserHabitRule rule1 = new UserHabitRule();
            rule1.HabitID = habit.ID;
            rule1.RuleID = 1;
            rule1.ContinuousRecordFrom = 1;
            rule1.ContinuousRecordTo = 2;
            rule1.Point = 1;
            context.UserHabitRules.Add(rule1);

            UserHabitRule rule2 = new UserHabitRule();
            rule2.HabitID = habit.ID;
            rule2.RuleID = 2;
            rule2.ContinuousRecordFrom = 2;
            rule1.ContinuousRecordTo = 3;
            rule2.Point = 2;
            context.UserHabitRules.Add(rule2);

            UserHabitRule rule3 = new UserHabitRule();
            rule3.HabitID = habit.ID;
            rule3.RuleID = 3;
            rule3.ContinuousRecordFrom = 3;
            rule3.Point = 4;
            context.UserHabitRules.Add(rule3);
            context.SaveChanges();

            // Add user record.
            foreach (UserHabitRecord record in testData.RecordList)
            {
                record.HabitID = nNewHabitID;
                var rst = control.Post(record);
                Assert.NotNull(rst);
                if (rst != null)
                {
                    CreatedODataResult<UserHabitRecord> rstrecord = (CreatedODataResult<UserHabitRecord>)rst.Result;
                    Assert.NotNull(rstrecord);
                }
            }

            // Check on DB directly
            var dbrecords = (from dbrecord in context.UserHabitRecords
                             where dbrecord.HabitID == nNewHabitID
                             select dbrecord).ToList();
            Assert.Equal(testData.RecordCount, dbrecords.Count);

            // Ensure rule is assigned correctly
            if (testData.TargetRuleDateList.Count > 0)
            {
                var rulecnt = 0;
                dbrecords.ForEach(dbr =>
                {
                    if (dbr.RuleID != null)
                    {
                        rulecnt++;

                        var ridx = testData.TargetRuleDateList.FindIndex(rd => rd.Date == dbr.RecordDate.Date);
                        Assert.NotEqual(-1, ridx);
                    }
                });
                Assert.Equal(testData.TargetRuleDateList.Count, rulecnt);
            }

            DataSetupUtility.ClearUserHabitData(context, nNewHabitID);
            context.SaveChanges();

            await context.DisposeAsync();
        }
    }
}

