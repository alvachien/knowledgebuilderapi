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
using System.Text.Json;
using knowledgebuilderapi.test.common;

namespace knowledgebuilderapi.test.unittest
{
    public class UserHabitRecordsControllerTestData_WeekNoOfCount : IXunitSerializable
    {
        // public static int CreateSequenceID { get; set; }
        public DayOfWeek Dow { get; set; }
        public List<UserHabitRecord> RecordList { get; set; }
        public int CompleteCondition { get; set; }
        public int RecordCount { get; set; }
        public List<DateTime> ExpectedRuleDateList { get; set; }

        public UserHabitRecordsControllerTestData_WeekNoOfCount()
        {
            this.RecordList = new List<UserHabitRecord>();
            this.ExpectedRuleDateList = new List<DateTime>();
            // this.CaseID = Guid.NewGuid();
        }
        public UserHabitRecordsControllerTestData_WeekNoOfCount(
            DayOfWeek dow,
            List<UserHabitRecord> records, 
            int completeCondition, int recordCount,
            List<DateTime> arTargetRuleDate) : this()
        {
            this.Dow = dow;
            this.RecordList.AddRange(records);
            this.CompleteCondition = completeCondition;
            this.RecordCount = recordCount;
            this.ExpectedRuleDateList.AddRange(arTargetRuleDate);
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            String val = info.GetValue<String>("Value");
            UserHabitRecordsControllerTestData_WeekNoOfCount other = JsonSerializer.Deserialize<UserHabitRecordsControllerTestData_WeekNoOfCount>(val);

            // CaseID = other.CaseID;
            Dow = other.Dow;
            CompleteCondition = other.CompleteCondition;
            RecordCount = other.RecordCount;
            if (other.RecordList.Count > 0)
                RecordList.AddRange(other.RecordList);
            if (other.ExpectedRuleDateList.Count > 0)
                ExpectedRuleDateList.AddRange(other.ExpectedRuleDateList);
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            String val = JsonSerializer.Serialize(this);
            info.AddValue("Value", val, typeof(String));
        }

        //public override string ToString()
        //{
        //    return this.CaseID.ToString();
        //}
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
                    100, 1,
                    new List<DateTime> { new DateTime(2021, 11, 1) }
                    ),
                new UserHabitRecordsControllerTestData_WeekNoOfCount(
                    DayOfWeek.Monday,
                    new List<UserHabitRecord> { new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1 ), CompleteFact = 90, Comment = "Test1" } },
                    100, 1,
                    new List<DateTime> {} 
                    ),
                new UserHabitRecordsControllerTestData_WeekNoOfCount(
                    DayOfWeek.Monday,
                    new List<UserHabitRecord> { new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1 ), CompleteFact = 110, Comment = "Test1" } },
                    100, 1,
                    new List<DateTime> { new DateTime(2021, 11, 1) } 
                    ),
                new UserHabitRecordsControllerTestData_WeekNoOfCount(
                    DayOfWeek.Monday,
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, CompleteFact = 30, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, CompleteFact = 30, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), SubID = 1, CompleteFact = 40, Comment = "Test1" }, },
                    100, 3, 
                    new List<DateTime> { new DateTime(2021, 11, 3) }
                    ),
                new UserHabitRecordsControllerTestData_WeekNoOfCount(
                    DayOfWeek.Monday,
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, CompleteFact = 30, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, CompleteFact = 30, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 2, CompleteFact = 40, Comment = "Test1" }, },
                    100, 3, new List<DateTime> { new DateTime(2021, 11, 2) }
                    ),
                new UserHabitRecordsControllerTestData_WeekNoOfCount(
                    DayOfWeek.Monday,
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, CompleteFact = 30, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, CompleteFact = 30, Comment = "Test2" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 2, CompleteFact = 40, Comment = "Test2" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), SubID = 1, CompleteFact = 20, Comment = "Test3" }, },
                    100, 4, new List<DateTime> { new DateTime(2021, 11, 3) }
                    ),
                new UserHabitRecordsControllerTestData_WeekNoOfCount(
                    DayOfWeek.Monday,
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, CompleteFact = 30, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, CompleteFact = 30, Comment = "Test2" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 2, CompleteFact = 40, Comment = "Test2" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), SubID = 1, CompleteFact = 20, Comment = "Test3" },

                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 8), SubID = 1, CompleteFact = 30, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 9), SubID = 1, CompleteFact = 30, Comment = "Test2" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 9), SubID = 2, CompleteFact = 40, Comment = "Test2" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 10), SubID = 1, CompleteFact = 20, Comment = "Test3" }, },
                    100, 8, new List<DateTime> { new DateTime(2021, 11, 3), new DateTime(2021, 11, 10) }
                    ),
                new UserHabitRecordsControllerTestData_WeekNoOfCount(
                    DayOfWeek.Tuesday,
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, CompleteFact = 30, Comment = "Test2" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 2, CompleteFact = 40, Comment = "Test2" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), SubID = 1, CompleteFact = 80, Comment = "Test3" },

                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 9), SubID = 1, CompleteFact = 30, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 10), SubID = 1, CompleteFact = 30, Comment = "Test2" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 10), SubID = 2, CompleteFact = 40, Comment = "Test2" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 11), SubID = 1, CompleteFact = 20, Comment = "Test3" }, },
                    100, 7, new List<DateTime> { new DateTime(2021, 11, 3), new DateTime(2021, 11, 11) }
                    )
            };

        [Theory]
        [MemberData(nameof(InputtedData))]
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
            if (testData.ExpectedRuleDateList.Count > 0)
            {
                var rulecnt = 0;
                dbrecords.ForEach(dbr =>
                {
                    if (dbr.RuleID != null)
                    {
                        rulecnt++;

                        var ridx = testData.ExpectedRuleDateList.FindIndex(rd => rd.Date == dbr.RecordDate.Date);
                        Assert.NotEqual(-1, ridx);
                    }
                });
                Assert.Equal(testData.ExpectedRuleDateList.Count, rulecnt);
            }

            DataSetupUtility.ClearUserHabitData(context, nNewHabitID);
            context.SaveChanges();

            await context.DisposeAsync();
        }
    }
}

