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
    public class UserHabitRecordsControllerTestData_WeekNoOfTimes : IXunitSerializable
    {
        public DayOfWeek Dow { get; set; }
        public List<DateTime> RecordDatesList { get; set; }
        public int CompleteCondition { get; set; }
        public int RecordCount { get; set; }
        public List<DateTime> TargetRuleDateList { get; set; }

        public UserHabitRecordsControllerTestData_WeekNoOfTimes()
        {
            this.RecordDatesList = new List<DateTime>();
            this.TargetRuleDateList = new List<DateTime>();
        }

        public UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek dow,
            List<DateTime> listRecordDates, int completeCondition, int recordCount,
            List<DateTime> listTargetRuleDate) : this()
        {
            this.Dow = dow;
            this.CompleteCondition = completeCondition;
            this.RecordCount = recordCount;
            if (listRecordDates.Count > 0)
                this.RecordDatesList.AddRange(listRecordDates);
            if (listTargetRuleDate.Count > 0)
                this.TargetRuleDateList.AddRange(listTargetRuleDate);
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            String val = info.GetValue<String>("Value");
            UserHabitRecordsControllerTestData_WeekNoOfTimes other = JsonSerializer.Deserialize<UserHabitRecordsControllerTestData_WeekNoOfTimes>(val);

            // CaseID = other.CaseID;
            Dow = other.Dow;
            CompleteCondition = other.CompleteCondition;
            RecordCount = other.RecordCount;
            if (other.RecordDatesList.Count > 0)
                RecordDatesList.AddRange(other.RecordDatesList);
            if (other.TargetRuleDateList.Count > 0)
                TargetRuleDateList.AddRange(other.TargetRuleDateList);
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            String val = JsonSerializer.Serialize(this);
            info.AddValue("Value", val, typeof(String));
        }
    }

    [Collection("KBAPI_UnitTests#1")]
    public class UserHabitRecordsControllerTest_WeekNOT : UserHabitRecordsControllerTestAbtract
    {
        public UserHabitRecordsControllerTest_WeekNOT(SqliteDatabaseFixture fixture)
            : base(fixture)
        {
        }

        public static TheoryData<UserHabitRecordsControllerTestData_WeekNoOfTimes> InputtedData =>
            new TheoryData<UserHabitRecordsControllerTestData_WeekNoOfTimes>
            {
                // Target: 1 time per week
                new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
                    new List<DateTime> { new DateTime(2021, 11, 1) },
                    1, 1, new List<DateTime> { new DateTime(2021, 11, 1) }),
                new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Tuesday,
                    new List<DateTime> { new DateTime(2021, 11, 2) },
                    1, 1, new List<DateTime> { new DateTime(2021, 11, 2) }),
                new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Wednesday,
                    new List<DateTime> { new DateTime(2021, 11, 3) },
                    1, 1, new List<DateTime> { new DateTime(2021, 11, 3) }),
                new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Thursday,
                    new List<DateTime> { new DateTime(2021, 11, 4) },
                    1, 1, new List<DateTime> { new DateTime(2021, 11, 4) }),
                new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Friday,
                    new List<DateTime> { new DateTime(2021, 11, 5) },
                    1, 1, new List<DateTime> { new DateTime(2021, 11, 5) }),
                new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Saturday,
                    new List<DateTime> { new DateTime(2021, 11, 6) },
                    1, 1, new List<DateTime> { new DateTime(2021, 11, 6) }),
                new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Sunday,
                    new List<DateTime> { new DateTime(2021, 11, 7) },
                    1, 1, new List<DateTime> { new DateTime(2021, 11, 7) }),

                // Target: 2 times per week
                new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
                    new List<DateTime> { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2) },
                    2, 2, new List<DateTime> {} ),
                new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
                    new List<DateTime> { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2) },
                    2, 2, new List<DateTime> { new DateTime(2021, 11, 2)  } ),
                new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
                    new List<DateTime> { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), new DateTime(2021, 11, 3) },
                    2, 3, new List<DateTime> { new DateTime(2021, 11, 3)  } ),
                new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
                    new List<DateTime> { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), new DateTime(2021, 11, 3), new DateTime(2021, 11, 4) },
                    2, 4, new List<DateTime> { new DateTime(2021, 11, 4)  } ),
                new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
                    new List<DateTime> { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), new DateTime(2021, 11, 3),
                                         new DateTime(2021, 11, 8), new DateTime(2021, 11, 9), new DateTime(2021, 11, 11), },
                    2, 6, new List<DateTime> { new DateTime(2021, 11, 3), new DateTime(2021, 11, 11) } ),
                new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
                    new List<DateTime> { new DateTime(2021, 11, 1),
                                         new DateTime(2021, 11, 8), new DateTime(2021, 11, 9), new DateTime(2021, 11, 11), },
                    2, 4, new List<DateTime> { new DateTime(2021, 11, 11) } ),

                // Target: 4 times per week
                new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
                    new List<DateTime> { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), new DateTime(2021, 11, 3), },
                    4, 3, new List<DateTime> {  } ),
                new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
                    new List<DateTime> { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), new DateTime(2021, 11, 3), new DateTime(2021, 11, 4), },
                    4, 4, new List<DateTime> { new DateTime(2021, 11, 4) } ),
                new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
                    new List<DateTime> { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), new DateTime(2021, 11, 3), new DateTime(2021, 11, 4), new DateTime(2021, 11, 5), },
                    4, 5, new List<DateTime> { new DateTime(2021, 11, 5) } ),
                new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
                    new List<DateTime> { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), new DateTime(2021, 11, 3), new DateTime(2021, 11, 4), new DateTime(2021, 11, 5), new DateTime(2021, 11, 6), },
                    4, 6, new List<DateTime> { new DateTime(2021, 11, 6) } ),

                // Target: 7 times per week
                new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
                    new List<DateTime> { new DateTime(2021, 11, 1), },
                    7, 1, new List<DateTime> {} ),
            };

        [Theory]
        [MemberData(nameof(InputtedData))]
        public async Task CalculatePoints_Weekly_NumberOfTimes(UserHabitRecordsControllerTestData_WeekNoOfTimes testData)
        {
            var context = this.fixture.GetCurrentDataContext();
            UserHabitRecordsController control = new(context);

            // Add Habit, Habit Rule
            UserHabit habit = new UserHabit();
            habit.TargetUser = test_user1;
            habit.ValidFrom = new DateTime(2021, 1, 1);
            habit.ValidTo = new DateTime(2022, 12, 31);
            habit.Name = "Habit_1";
            habit.Category = HabitCategory.Positive;
            habit.Comment = habit.Name;
            habit.Frequency = HabitFrequency.Weekly;
            habit.CompleteCategory = HabitCompleteCategory.NumberOfTimes;
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
            //Boolean lastRst = false;
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

                    //lastRst = rstrecord.Entity.RuleID.HasValue;
                }
            }

            // Check on DB directly
            var dbrecords = (from dbrecord in context.UserHabitRecords
                             where dbrecord.HabitID == habit.ID
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
