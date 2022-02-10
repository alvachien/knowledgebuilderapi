using System;
using System.Collections;
using System.Linq;
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
using Xunit;
using knowledgebuilderapi.test.common;

namespace knowledgebuilderapi.test.unittest
{
    public class UserHabitRecordsControllerTestData_MonthNoOfCount : IXunitSerializable
    {
        public int DateInMonth { get; set; }
        public List<UserHabitRecord> RecordList { get; set; }
        public List<UserHabitRule> RuleList { get; set; }
        public int CompleteCondition { get; set; }
        public int RecordCount { get; set; }
        public List<DateTime> ExpectedRuleDateList { get; set; }

        public UserHabitRecordsControllerTestData_MonthNoOfCount()
        {
            RecordList = new List<UserHabitRecord>();
            RuleList = new List<UserHabitRule>();
            ExpectedRuleDateList = new List<DateTime>();
        }

        public UserHabitRecordsControllerTestData_MonthNoOfCount(
            int dateInMonth,
            List<UserHabitRecord> listRecords, 
            List<UserHabitRule> ruleList,
            int completeCondition,
            int recordCount,
            List<DateTime> expectedRuleDate) : this()
        {
            DateInMonth = dateInMonth;
            CompleteCondition = completeCondition;
            if (ruleList.Count > 0)
                RuleList.AddRange(ruleList);
            if (listRecords.Count > 0)
                RecordList.AddRange(listRecords);
            RecordCount = recordCount; 
            if (expectedRuleDate.Count > 0)
                ExpectedRuleDateList.AddRange(expectedRuleDate);
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            String val = info.GetValue<String>("Value");
            UserHabitRecordsControllerTestData_MonthNoOfCount other = JsonSerializer.Deserialize<UserHabitRecordsControllerTestData_MonthNoOfCount>(val);

            DateInMonth = other.DateInMonth;
            CompleteCondition = other.CompleteCondition;
            if (other.RuleList.Count > 0)
                RuleList.AddRange(other.RuleList);
            if (other.RecordList.Count > 0)
                RecordList.AddRange(other.RecordList);
            RecordCount = other.RecordCount;
            if (other.ExpectedRuleDateList.Count > 0)
                ExpectedRuleDateList.AddRange(other.ExpectedRuleDateList);
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            String val = JsonSerializer.Serialize(this);
            info.AddValue("Value", val, typeof(String));
        }
    }

    [Collection("KBAPI_UnitTests#1")]

    public class UserHabitRecordsControllerTest_MonthlyNOCount : UserHabitRecordsControllerTestAbtract
    {
        public UserHabitRecordsControllerTest_MonthlyNOCount(SqliteDatabaseFixture fixture)
            : base(fixture)
        {
        }

        public static TheoryData<UserHabitRecordsControllerTestData_MonthNoOfCount> InputtedData =>
            new TheoryData<UserHabitRecordsControllerTestData_MonthNoOfCount>
            {
                // Expect: 100 exercises per Month
                // Continuous day 1
                new UserHabitRecordsControllerTestData_MonthNoOfCount(
                    1,
                    new List<UserHabitRecord>
                    {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1 ), CompleteFact = 100, Comment = "Test1" },
                    }, 
                    new List<UserHabitRule>
                    {
                        new UserHabitRule { RuleID = 1, ContinuousRecordFrom = 1, ContinuousRecordTo = 2, Point = 1, },
                    },
                    100, 1, 
                    new List<DateTime> { 
                        new DateTime(2021, 11, 1),
                    }),
                new UserHabitRecordsControllerTestData_MonthNoOfCount(
                    1,
                    new List<UserHabitRecord>
                    {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1 ), CompleteFact = 10, Comment = "Test1" },
                    },
                    new List<UserHabitRule>
                    {
                        new UserHabitRule { RuleID = 1, ContinuousRecordFrom = 1, ContinuousRecordTo = 2, Point = 1, },
                    },
                    100, 1,
                    new List<DateTime> {
                    }),
                new UserHabitRecordsControllerTestData_MonthNoOfCount(
                    1,
                    new List<UserHabitRecord>
                    {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1 ), CompleteFact = 150, Comment = "Test1" },
                    },
                    new List<UserHabitRule>
                    {
                        new UserHabitRule { RuleID = 1, ContinuousRecordFrom = 1, ContinuousRecordTo = 2, Point = 1, },
                    },
                    100, 1,
                    new List<DateTime> {
                        new DateTime(2021, 11, 1),
                    }),
                new UserHabitRecordsControllerTestData_MonthNoOfCount(
                    1,
                    new List<UserHabitRecord>
                    {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1 ), CompleteFact = 50, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 11 ), CompleteFact = 20, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 21 ), CompleteFact = 50, Comment = "Test1" },
                    },
                    new List<UserHabitRule>
                    {
                        new UserHabitRule { RuleID = 1, ContinuousRecordFrom = 1, ContinuousRecordTo = 2, Point = 1, },
                    },
                    100, 3,
                    new List<DateTime> {
                        new DateTime(2021, 11, 21),
                    }),
                new UserHabitRecordsControllerTestData_MonthNoOfCount(
                    1,
                    new List<UserHabitRecord>
                    {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1 ), CompleteFact = 50, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 11 ), CompleteFact = 20, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 21 ), CompleteFact = 20, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 22 ), CompleteFact = 5, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 30 ), CompleteFact = 5, Comment = "Test1" },
                    },
                    new List<UserHabitRule>
                    {
                        new UserHabitRule { RuleID = 1, ContinuousRecordFrom = 1, ContinuousRecordTo = 2, Point = 1, },
                    },
                    100, 5,
                    new List<DateTime> {
                        new DateTime(2021, 11, 30),
                    }),
                new UserHabitRecordsControllerTestData_MonthNoOfCount(
                    1,
                    new List<UserHabitRecord>
                    {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1 ), CompleteFact = 50, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 11 ), CompleteFact = 20, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 21 ), CompleteFact = 10, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 30 ), SubID = 1, CompleteFact = 5, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 30 ), SubID = 2, CompleteFact = 15, Comment = "Test1" },
                    },
                    new List<UserHabitRule>
                    {
                        new UserHabitRule { RuleID = 1, ContinuousRecordFrom = 1, ContinuousRecordTo = 2, Point = 1, },
                    },
                    100, 5,
                    new List<DateTime> {
                        new DateTime(2021, 11, 30),
                    }),

                // Continuous day 2
                new UserHabitRecordsControllerTestData_MonthNoOfCount(
                    1,
                    new List<UserHabitRecord>
                    {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1 ), CompleteFact = 10, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 12, 1 ), CompleteFact = 100, Comment = "Test1" },
                    },
                    new List<UserHabitRule>
                    {
                        new UserHabitRule { RuleID = 1, ContinuousRecordFrom = 1, ContinuousRecordTo = 2, Point = 1, },
                        new UserHabitRule { RuleID = 2, ContinuousRecordFrom = 2, ContinuousRecordTo = 99, Point = 2, },
                    },
                    100, 2,
                    new List<DateTime> {
                        new DateTime(2021, 12, 1),
                    }),
                new UserHabitRecordsControllerTestData_MonthNoOfCount(
                    1,
                    new List<UserHabitRecord>
                    {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1 ), CompleteFact = 100, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 12, 1 ), CompleteFact = 100, Comment = "Test1" },
                    },
                    new List<UserHabitRule>
                    {
                        new UserHabitRule { RuleID = 1, ContinuousRecordFrom = 1, ContinuousRecordTo = 2, Point = 1, },
                        new UserHabitRule { RuleID = 2, ContinuousRecordFrom = 2, ContinuousRecordTo = 99, Point = 2, },
                    },
                    100, 2,
                    new List<DateTime> {
                        new DateTime(2021, 11, 1),
                        new DateTime(2021, 12, 1),
                    }),
                new UserHabitRecordsControllerTestData_MonthNoOfCount(
                    1,
                    new List<UserHabitRecord>
                    {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1 ), CompleteFact = 50, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 11 ), CompleteFact = 20, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 21 ), CompleteFact = 10, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 30 ), SubID = 1, CompleteFact = 5, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 30 ), SubID = 2, CompleteFact = 15, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 12, 12 ), CompleteFact = 50, Comment = "Test1" },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 12, 31 ), SubID = 1, CompleteFact = 60, Comment = "Test1" },
                    },
                    new List<UserHabitRule>
                    {
                        new UserHabitRule { RuleID = 1, ContinuousRecordFrom = 1, ContinuousRecordTo = 2, Point = 1, },
                        new UserHabitRule { RuleID = 2, ContinuousRecordFrom = 2, ContinuousRecordTo = 99, Point = 2, },
                    },
                    100, 7,
                    new List<DateTime> {
                        new DateTime(2021, 11, 30),
                        new DateTime(2021, 12, 31),
                    }),
            };

        [Theory]
        [MemberData(nameof(InputtedData))]
        public async Task CalculatePoints(UserHabitRecordsControllerTestData_MonthNoOfCount testData)
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
            habit.Frequency = HabitFrequency.Monthly;
            habit.CompleteCategory = HabitCompleteCategory.NumberOfCount;
            habit.CompleteCondition = testData.CompleteCondition;
            habit.StartDate = testData.DateInMonth;
            context.UserHabits.Add(habit);
            context.SaveChanges();
            Int32 nNewHabitID = habit.ID;

            foreach(var rule in testData.RuleList)
            {
                rule.HabitID = nNewHabitID;
                context.UserHabitRules.Add(rule);
            }
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
