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
    public class UserHabitRecordsControllerTestData_DailyNoOfCount : IXunitSerializable
    {
        public List<UserHabitRecord> RecordList { get; set; }
        public List<UserHabitRule> RuleList { get; set; }
        public int CompleteCondition { get; set; }
        public List<UserHabitRecord> ExpectedRecordList { get; set; }

        public UserHabitRecordsControllerTestData_DailyNoOfCount()
        {
            RecordList = new List<UserHabitRecord>();
            RuleList = new List<UserHabitRule>();
            ExpectedRecordList = new List<UserHabitRecord>();
        }

        public UserHabitRecordsControllerTestData_DailyNoOfCount(
            int completeCondition,
            List<UserHabitRecord> listRecords,
            List<UserHabitRule> listRules,
            List<UserHabitRecord> listExpectedRecords) : this()
        {
            CompleteCondition = completeCondition;
            if (listRules.Count > 0)
                RuleList.AddRange(listRules);
            if (listRecords.Count > 0)
                RecordList.AddRange(listRecords);
            if (listExpectedRecords.Count > 0)
                ExpectedRecordList.AddRange(listExpectedRecords);
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            String val = info.GetValue<String>("Value");
            UserHabitRecordsControllerTestData_DailyNoOfCount other = JsonSerializer.Deserialize<UserHabitRecordsControllerTestData_DailyNoOfCount>(val);

            CompleteCondition = other.CompleteCondition;
            if (other.RuleList.Count > 0)
                RuleList.AddRange(other.RuleList);
            if (other.RecordList.Count > 0)
                RecordList.AddRange(other.RecordList);
            if (other.ExpectedRecordList.Count > 0)
                ExpectedRecordList.AddRange(other.ExpectedRecordList);
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            String val = JsonSerializer.Serialize(this);
            info.AddValue("Value", val, typeof(String));
        }
    }

    [Collection("KBAPI_UnitTests#1")]
    public class UserHabitRecordsControllerTest_DailyNOCount : UserHabitRecordsControllerTestAbtract
    {
        public UserHabitRecordsControllerTest_DailyNOCount(SqliteDatabaseFixture fixture)
            : base(fixture)
        {
        }

        public static TheoryData<UserHabitRecordsControllerTestData_DailyNoOfCount> InputtedData =>
            new TheoryData<UserHabitRecordsControllerTestData_DailyNoOfCount>
            {
                // Continuous days 1
                new UserHabitRecordsControllerTestData_DailyNoOfCount(
                    100,
                    new List<UserHabitRecord> { 
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, CompleteFact = 20 } 
                    },
                    new List<UserHabitRule> {
                        new UserHabitRule { ContinuousRecordFrom = 1, ContinuousRecordTo = 2, RuleID = 1, Point = 1 }
                    },
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, }
                    }),
                new UserHabitRecordsControllerTestData_DailyNoOfCount(
                    100,
                    new List<UserHabitRecord> { 
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, CompleteFact = 100 } 
                    },
                    new List<UserHabitRule> {
                        new UserHabitRule { ContinuousRecordFrom = 1, ContinuousRecordTo = 2, RuleID = 1, Point = 1 }
                    },
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1,  RuleID = 1, ContinuousCount = 1 },
                    }),
                new UserHabitRecordsControllerTestData_DailyNoOfCount(
                    100,
                    new List<UserHabitRecord> { 
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, CompleteFact = 120 } 
                    },
                    new List<UserHabitRule> {
                        new UserHabitRule { ContinuousRecordFrom = 1, ContinuousRecordTo = 2, RuleID = 1, Point = 1 }
                    },
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, RuleID = 1, ContinuousCount = 1 },
                    }),
                new UserHabitRecordsControllerTestData_DailyNoOfCount(
                    100,
                    new List<UserHabitRecord> { 
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, CompleteFact = 40 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 2, CompleteFact = 60 },
                    },
                    new List<UserHabitRule> {
                        new UserHabitRule { ContinuousRecordFrom = 1, ContinuousRecordTo = 2, RuleID = 1, Point = 1 }
                    },
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 2, RuleID = 1, ContinuousCount = 1 },
                    }),

                // Continuous days 2
                new UserHabitRecordsControllerTestData_DailyNoOfCount(
                    100,
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, CompleteFact = 20 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, CompleteFact = 20 },
                    },
                    new List<UserHabitRule> {
                        new UserHabitRule { ContinuousRecordFrom = 1, ContinuousRecordTo = 2, RuleID = 1, Point = 1 }
                    },
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, }
                    }),
                new UserHabitRecordsControllerTestData_DailyNoOfCount(
                    100,
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, CompleteFact = 20 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, CompleteFact = 100 },
                    },
                    new List<UserHabitRule> {
                        new UserHabitRule { ContinuousRecordFrom = 1, ContinuousRecordTo = 2, RuleID = 1, Point = 1 }
                    },
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, RuleID = 1, ContinuousCount = 1 }
                    }),
                new UserHabitRecordsControllerTestData_DailyNoOfCount(
                    100,
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, CompleteFact = 20 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, CompleteFact = 20 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 2, CompleteFact = 90 },
                    },
                    new List<UserHabitRule> {
                        new UserHabitRule { ContinuousRecordFrom = 1, ContinuousRecordTo = 2, RuleID = 1, Point = 1 }
                    },
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 2, RuleID = 1, ContinuousCount = 1 }
                    }),

                // Continuous days 3
                new UserHabitRecordsControllerTestData_DailyNoOfCount(
                    100,
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, CompleteFact = 20 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, CompleteFact = 20 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), SubID = 1, CompleteFact = 20 },
                    },
                    new List<UserHabitRule> {
                        new UserHabitRule { ContinuousRecordFrom = 1, ContinuousRecordTo = 2, RuleID = 1, Point = 1 }
                    },
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), SubID = 1, },
                    }),
                new UserHabitRecordsControllerTestData_DailyNoOfCount(
                    100,
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, CompleteFact = 20 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, CompleteFact = 100 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), SubID = 1, CompleteFact = 20 },
                    },
                    new List<UserHabitRule> {
                        new UserHabitRule { ContinuousRecordFrom = 1, ContinuousRecordTo = 2, RuleID = 1, Point = 1 }
                    },
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, RuleID = 1, ContinuousCount = 1 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), SubID = 1, },
                    }),

                // Continuous days 5
                new UserHabitRecordsControllerTestData_DailyNoOfCount(
                    100,
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, CompleteFact = 20 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, CompleteFact = 20 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 2, CompleteFact = 90 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), SubID = 1, CompleteFact = 30 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), SubID = 2, CompleteFact = 70 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 4), SubID = 1, CompleteFact = 120 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 5), SubID = 1, CompleteFact = 20 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 5), SubID = 2, CompleteFact = 60 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 5), SubID = 3, CompleteFact = 20 },
                    },
                    new List<UserHabitRule> {
                        new UserHabitRule { ContinuousRecordFrom = 1, ContinuousRecordTo = 2, RuleID = 1, Point = 1 },
                        new UserHabitRule { ContinuousRecordFrom = 2, ContinuousRecordTo = 999, RuleID = 2, Point = 1 }
                    },
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 2, RuleID = 1, ContinuousCount = 1 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), SubID = 1, },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), SubID = 2, RuleID = 2, ContinuousCount = 2 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 4), SubID = 1, RuleID = 2, ContinuousCount = 3 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 5), SubID = 1, },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 5), SubID = 2, },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 5), SubID = 3, RuleID = 2, ContinuousCount = 4 },
                    }),
                new UserHabitRecordsControllerTestData_DailyNoOfCount(
                    100,
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, CompleteFact = 20 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, CompleteFact = 20 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 2, CompleteFact = 90 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), SubID = 1, CompleteFact = 30 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), SubID = 2, CompleteFact = 70 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 4), SubID = 1, CompleteFact = 20 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 5), SubID = 1, CompleteFact = 20 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 5), SubID = 2, CompleteFact = 60 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 5), SubID = 3, CompleteFact = 20 },
                    },
                    new List<UserHabitRule> {
                        new UserHabitRule { ContinuousRecordFrom = 1, ContinuousRecordTo = 2, RuleID = 1, Point = 1 },
                        new UserHabitRule { ContinuousRecordFrom = 2, ContinuousRecordTo = 999, RuleID = 2, Point = 1 }
                    },
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), SubID = 1, },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 1, },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), SubID = 2, RuleID = 1, ContinuousCount = 1 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), SubID = 1, },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), SubID = 2, RuleID = 2, ContinuousCount = 2 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 4), SubID = 1, },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 5), SubID = 1, },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 5), SubID = 2, },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 5), SubID = 3, RuleID = 1, ContinuousCount = 1 },
                    }),
            };

        [Theory]
        [MemberData(nameof(InputtedData))]
        public async Task CalculatePoints_Daily_NumberOfCount(UserHabitRecordsControllerTestData_DailyNoOfCount testData)
        {
            var context = this.fixture.GetCurrentDataContext();
            UserHabitRecordsController control = new(context);

            // Add Habit, Habit Rule
            UserHabit habit = new UserHabit();
            habit.TargetUser = DataSetupUtility.UserA;
            habit.ValidFrom = new DateTime(2021, 1, 1);
            habit.ValidTo = new DateTime(2022, 12, 31);
            habit.Name = "Habit_Daily_1";
            habit.Category = HabitCategory.Positive;
            habit.Comment = habit.Name;
            habit.Frequency = HabitFrequency.Daily;
            habit.CompleteCategory = HabitCompleteCategory.NumberOfCount;
            habit.CompleteCondition = testData.CompleteCondition;
            context.UserHabits.Add(habit);
            context.SaveChanges();
            Int32 nNewHabitID = habit.ID;

            foreach (var rule in testData.RuleList)
            {
                rule.HabitID = habit.ID;
                context.UserHabitRules.Add(rule);
            }
            context.SaveChanges();

            // Add user record.
            foreach (var nrecord in testData.RecordList)
            {
                nrecord.HabitID = habit.ID;
                nrecord.Comment = "Test1";
                var rst = control.Post(nrecord);
                Assert.NotNull(rst);
                if (rst != null)
                {
                    CreatedODataResult<UserHabitRecord> rstrecord = (CreatedODataResult<UserHabitRecord>)rst.Result;
                    Assert.NotNull(rstrecord);
                }
            }

            // Check on DB directly
            var dbrecords = (from dbrecord in context.UserHabitRecords
                             where dbrecord.HabitID == habit.ID
                             orderby dbrecord.RecordDate ascending
                             select dbrecord).ToList();
            Assert.Equal(testData.ExpectedRecordList.Count, dbrecords.Count);

            // Ensure rule is assigned correctly
            if (testData.ExpectedRecordList.Count > 0)
            {
                foreach (var dbrecord in dbrecords)
                {
                    var ridx = testData.ExpectedRecordList.FindIndex(rd => rd.RecordDate == dbrecord.RecordDate
                                                                        && rd.SubID == dbrecord.SubID);
                    Assert.NotEqual(-1, ridx);

                    Assert.Equal(testData.ExpectedRecordList[ridx].RuleID, dbrecord.RuleID);
                    Assert.Equal(testData.ExpectedRecordList[ridx].ContinuousCount, dbrecord.ContinuousCount);
                }
            }

            DataSetupUtility.ClearUserHabitData(context, nNewHabitID);
            context.SaveChanges();

            await context.DisposeAsync();
        }
    }
}
