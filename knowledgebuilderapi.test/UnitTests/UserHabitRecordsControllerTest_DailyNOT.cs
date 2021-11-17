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

namespace knowledgebuilderapi.test.UnitTests
{
    public class UserHabitRecordsControllerTestData_DailyNoOfTimes : IXunitSerializable
    {
        public List<DateTime> RecordDatesList { get; set; }
        public List<UserHabitRule> RuleList { get; set; }
        //public int CompleteCondition { get; set; }
        //public int ExpectedRecordCount { get; set; }
        public List<UserHabitRecord> ExpectedRecordList { get; set; }

        public UserHabitRecordsControllerTestData_DailyNoOfTimes()
        {
            RecordDatesList = new List<DateTime>();
            RuleList = new List<UserHabitRule>();
            ExpectedRecordList = new List<UserHabitRecord>();
        }

        public UserHabitRecordsControllerTestData_DailyNoOfTimes(
            List<DateTime> listRecordDates, 
            List<UserHabitRule> listRules,
            List<UserHabitRecord> listRecords) : this()
        {
            // this.CompleteCondition = completeCondition;
            // this.ExpectedRecordCount = recordCount;
            if (listRules.Count > 0)
                this.RuleList.AddRange(listRules);
            if (listRecordDates.Count > 0)
                this.RecordDatesList.AddRange(listRecordDates);
            if (listRecords.Count > 0)
                this.ExpectedRecordList.AddRange(listRecords);
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            String val = info.GetValue<String>("Value");
            UserHabitRecordsControllerTestData_DailyNoOfTimes other = JsonSerializer.Deserialize<UserHabitRecordsControllerTestData_DailyNoOfTimes>(val);

            // CompleteCondition = other.CompleteCondition;
            // ExpectedRecordCount = other.ExpectedRecordCount;
            if (other.RuleList.Count > 0)
                RuleList.AddRange(other.RuleList);
            if (other.RecordDatesList.Count > 0)
                RecordDatesList.AddRange(other.RecordDatesList);
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
    public class UserHabitRecordsControllerTest_DailyNOT : UserHabitRecordsControllerTestAbtract
    {
        public UserHabitRecordsControllerTest_DailyNOT(SqliteDatabaseFixture fixture)
            : base(fixture)
        {
        }

        public static TheoryData<UserHabitRecordsControllerTestData_DailyNoOfTimes> InputtedData =>
            new TheoryData<UserHabitRecordsControllerTestData_DailyNoOfTimes>
            {
                // Continuous days 1
                new UserHabitRecordsControllerTestData_DailyNoOfTimes(
                    new List<DateTime> { new DateTime(2021, 11, 1) },
                    new List<UserHabitRule> { 
                        new UserHabitRule { ContinuousRecordFrom = 1, ContinuousRecordTo = 2, RuleID = 1, Point = 1 } 
                    },
                    new List<UserHabitRecord> { 
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), RuleID = 1, ContinuousCount = 1 }
                    }),
                new UserHabitRecordsControllerTestData_DailyNoOfTimes(
                    new List<DateTime> { new DateTime(2021, 11, 1), new DateTime(2021, 11, 3) },
                    new List<UserHabitRule> {
                        new UserHabitRule { ContinuousRecordFrom = 1, ContinuousRecordTo = 2, RuleID = 1, Point = 1 }
                    },
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), RuleID = 1, ContinuousCount = 1 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), RuleID = 1, ContinuousCount = 1 }
                    }),

                // Continuous days 2
                new UserHabitRecordsControllerTestData_DailyNoOfTimes(
                    new List<DateTime> { new DateTime(2021, 11, 1) },
                    new List<UserHabitRule> {
                        new UserHabitRule { ContinuousRecordFrom = 1, ContinuousRecordTo = 2, RuleID = 1, Point = 1 },
                        new UserHabitRule { ContinuousRecordFrom = 2, ContinuousRecordTo = 3, RuleID = 2, Point = 2 },
                    },
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), RuleID = 1, ContinuousCount = 1 },
                    }),
                new UserHabitRecordsControllerTestData_DailyNoOfTimes(
                    new List<DateTime> { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), },
                    new List<UserHabitRule> {
                        new UserHabitRule { ContinuousRecordFrom = 1, ContinuousRecordTo = 2, RuleID = 1, Point = 1 },
                        new UserHabitRule { ContinuousRecordFrom = 2, ContinuousRecordTo = 3, RuleID = 2, Point = 2 },
                    },
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), RuleID = 1, ContinuousCount = 1 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), RuleID = 2, ContinuousCount = 2 },
                    }),
                new UserHabitRecordsControllerTestData_DailyNoOfTimes(
                    new List<DateTime> { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), new DateTime(2021, 11, 4), },
                    new List<UserHabitRule> {
                        new UserHabitRule { ContinuousRecordFrom = 1, ContinuousRecordTo = 2, RuleID = 1, Point = 1 },
                        new UserHabitRule { ContinuousRecordFrom = 2, ContinuousRecordTo = 3, RuleID = 2, Point = 2 },
                    },
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), RuleID = 1, ContinuousCount = 1 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), RuleID = 2, ContinuousCount = 2 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 4), RuleID = 1, ContinuousCount = 1 },
                    }),

                // Continuous days 4
                new UserHabitRecordsControllerTestData_DailyNoOfTimes(
                    new List<DateTime> { 
                        new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), 
                        new DateTime(2021, 11, 3), new DateTime(2021, 11, 4), },
                    new List<UserHabitRule> {
                        new UserHabitRule { ContinuousRecordFrom = 1, ContinuousRecordTo = 2, RuleID = 1, Point = 1 },
                        new UserHabitRule { ContinuousRecordFrom = 2, ContinuousRecordTo = 3, RuleID = 2, Point = 2 },
                        new UserHabitRule { ContinuousRecordFrom = 3, ContinuousRecordTo = 9, RuleID = 3, Point = 5 },
                        new UserHabitRule { ContinuousRecordFrom = 9, ContinuousRecordTo = 999, RuleID = 4, Point = 10 },
                    },
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), RuleID = 1, ContinuousCount = 1 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), RuleID = 2, ContinuousCount = 2 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), RuleID = 3, ContinuousCount = 3 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 4), RuleID = 3, ContinuousCount = 4 },
                    }),
                new UserHabitRecordsControllerTestData_DailyNoOfTimes(
                    new List<DateTime> {
                        new DateTime(2021, 11, 1), new DateTime(2021, 11, 2),
                        new DateTime(2021, 11, 3), new DateTime(2021, 11, 4), 
                        new DateTime(2021, 11, 5),
                    },
                    new List<UserHabitRule> {
                        new UserHabitRule { ContinuousRecordFrom = 1, ContinuousRecordTo = 2, RuleID = 1, Point = 1 },
                        new UserHabitRule { ContinuousRecordFrom = 2, ContinuousRecordTo = 3, RuleID = 2, Point = 2 },
                        new UserHabitRule { ContinuousRecordFrom = 3, ContinuousRecordTo = 9, RuleID = 3, Point = 5 },
                        new UserHabitRule { ContinuousRecordFrom = 9, ContinuousRecordTo = 999, RuleID = 4, Point = 10 },
                    },
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), RuleID = 1, ContinuousCount = 1 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), RuleID = 2, ContinuousCount = 2 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), RuleID = 3, ContinuousCount = 3 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 4), RuleID = 3, ContinuousCount = 4 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 5), RuleID = 3, ContinuousCount = 5 },
                    }),
                new UserHabitRecordsControllerTestData_DailyNoOfTimes(
                    new List<DateTime> {
                        new DateTime(2021, 11, 1), new DateTime(2021, 11, 2),
                        new DateTime(2021, 11, 3), new DateTime(2021, 11, 4),
                        new DateTime(2021, 11, 5), new DateTime(2021, 11, 7),
                    },
                    new List<UserHabitRule> {
                        new UserHabitRule { ContinuousRecordFrom = 1, ContinuousRecordTo = 2, RuleID = 1, Point = 1 },
                        new UserHabitRule { ContinuousRecordFrom = 2, ContinuousRecordTo = 3, RuleID = 2, Point = 2 },
                        new UserHabitRule { ContinuousRecordFrom = 3, ContinuousRecordTo = 9, RuleID = 3, Point = 5 },
                        new UserHabitRule { ContinuousRecordFrom = 9, ContinuousRecordTo = 999, RuleID = 4, Point = 10 },
                    },
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), RuleID = 1, ContinuousCount = 1 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), RuleID = 2, ContinuousCount = 2 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), RuleID = 3, ContinuousCount = 3 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 4), RuleID = 3, ContinuousCount = 4 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 5), RuleID = 3, ContinuousCount = 5 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 7), RuleID = 1, ContinuousCount = 1 },
                    }),
                new UserHabitRecordsControllerTestData_DailyNoOfTimes(
                    new List<DateTime> {
                        new DateTime(2021, 11, 1), new DateTime(2021, 11, 2),
                        new DateTime(2021, 11, 3), new DateTime(2021, 11, 4),
                        new DateTime(2021, 11, 5), 
                        new DateTime(2021, 11, 7), new DateTime(2021, 11, 8),
                    },
                    new List<UserHabitRule> {
                        new UserHabitRule { ContinuousRecordFrom = 1, ContinuousRecordTo = 2, RuleID = 1, Point = 1 },
                        new UserHabitRule { ContinuousRecordFrom = 2, ContinuousRecordTo = 3, RuleID = 2, Point = 2 },
                        new UserHabitRule { ContinuousRecordFrom = 3, ContinuousRecordTo = 9, RuleID = 3, Point = 5 },
                        new UserHabitRule { ContinuousRecordFrom = 9, ContinuousRecordTo = 999, RuleID = 4, Point = 10 },
                    },
                    new List<UserHabitRecord> {
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 1), RuleID = 1, ContinuousCount = 1 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 2), RuleID = 2, ContinuousCount = 2 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 3), RuleID = 3, ContinuousCount = 3 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 4), RuleID = 3, ContinuousCount = 4 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 5), RuleID = 3, ContinuousCount = 5 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 7), RuleID = 1, ContinuousCount = 1 },
                        new UserHabitRecord { RecordDate = new DateTime(2021, 11, 8), RuleID = 2, ContinuousCount = 2 },
                    }),
            };

        [Theory]
        [MemberData(nameof(InputtedData))]
        public async Task CalculatePoints_Daily_NumberOfTimes(UserHabitRecordsControllerTestData_DailyNoOfTimes testData)
        {
            var context = this.fixture.GetCurrentDataContext();
            UserHabitRecordsController control = new(context);

            // Add Habit, Habit Rule
            UserHabit habit = new UserHabit();
            habit.TargetUser = test_user1;
            habit.ValidFrom = new DateTime(2021, 1, 1);
            habit.ValidTo = new DateTime(2022, 12, 31);
            habit.Name = "Habit_Daily_1";
            habit.Category = HabitCategory.Positive;
            habit.Comment = habit.Name;
            habit.Frequency = HabitFrequency.Daily;
            habit.CompleteCategory = HabitCompleteCategory.NumberOfTimes;
            context.UserHabits.Add(habit);
            context.SaveChanges();
            Int32 nNewHabitID = habit.ID;

            foreach(var rule in testData.RuleList)
            {
                rule.HabitID = habit.ID;
                context.UserHabitRules.Add(rule);
            }
            context.SaveChanges();

            // Add user record.
            foreach (DateTime dt in testData.RecordDatesList)
            {
                UserHabitRecord record = new UserHabitRecord();
                record.HabitID = habit.ID;
                record.RecordDate = dt;
                record.Comment = "Test1";
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
                             where dbrecord.HabitID == habit.ID
                             orderby dbrecord.RecordDate ascending
                             select dbrecord).ToList();
            Assert.Equal(testData.ExpectedRecordList.Count, dbrecords.Count);

            // Ensure rule is assigned correctly
            if (testData.ExpectedRecordList.Count > 0)
            {
                foreach(var dbrecord in dbrecords)
                {
                    var ridx = testData.ExpectedRecordList.FindIndex(rd => rd.RecordDate == dbrecord.RecordDate);
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
