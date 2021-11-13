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

namespace knowledgebuilderapi.test.UnitTests
{
    [Collection("KBAPI_UnitTests#1")]
    public class UserHabitRecordsControllerTest : IDisposable
    {
        private SqliteDatabaseFixture fixture = null;
        //private List<Int32> objectsCreated = new List<Int32>();
        private const string test_manager = "TestManager_1";
        private const string test_user1 = "Tester_1";

        public UserHabitRecordsControllerTest(SqliteDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        public void Dispose()
        {
        }

        public static IEnumerable<object[]> WeeklyDates
        {
            get
            {
                // Or this could read from a file. :)
                return new[]
                {
                    // Target: 1 time per week
                    new object[] { 
                        DayOfWeek.Monday, 
                        new DateTime[] { new DateTime(2021, 11, 1) }, 
                        1, 1, new DateTime[] { new DateTime(2021, 11, 1) }
                    },
                    new object[] { 
                        DayOfWeek.Tuesday,
                        new DateTime[] { new DateTime(2021, 11, 2) },
                        1, 1, new DateTime[] { new DateTime(2021, 11, 2) }
                    },
                    new object[] { 
                        DayOfWeek.Wednesday,
                        new DateTime[] { new DateTime(2021, 11, 3) },
                        1, 1, new DateTime[] { new DateTime(2021, 11, 3) }
                    },
                    new object[] { 
                        DayOfWeek.Thursday,
                        new DateTime[] { new DateTime(2021, 11, 4) },
                        1, 1, new DateTime[] {  new DateTime(2021, 11, 4) }
                    },
                    new object[] { 
                        DayOfWeek.Friday,
                        new DateTime[] { new DateTime(2021, 11, 5) },
                        1, 1, new DateTime[] { new DateTime(2021, 11, 5) }
                    },
                    new object[] { 
                        DayOfWeek.Saturday,
                        new DateTime[] { new DateTime(2021, 11, 6) },
                        1, 1, new DateTime[] { new DateTime(2021, 11, 6) }
                    },
                    new object[] { 
                        DayOfWeek.Sunday,
                        new DateTime[] { new DateTime(2021, 11, 7) },
                        1, 1, new DateTime[] { new DateTime(2021, 11, 7) }
                    },

                    // Target: 2 times per week
                    new object[] { 
                        DayOfWeek.Monday,
                        new DateTime[] { new DateTime(2021, 11, 1) },
                        2, 1, new DateTime[] { }
                    },
                    new object[] {
                        DayOfWeek.Monday,
                        new DateTime[] { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2) },
                        2, 2, new DateTime[] {  new DateTime(2021, 11, 2) }
                    },
                    new object[] {
                        DayOfWeek.Monday,
                        new DateTime[] { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), new DateTime(2021, 11,3) },
                        2, 3, new DateTime[] { new DateTime(2021, 11, 3) }
                    },
                    new object[] {
                        DayOfWeek.Monday,
                        new DateTime[] { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), new DateTime(2021, 11,3), new DateTime(2021, 11, 4) },
                        2, 4, new DateTime[] { new DateTime(2021, 11, 4) }
                    },
                    new object[] {
                        DayOfWeek.Monday,
                        new DateTime[] { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), new DateTime(2021, 11, 3),
                                        new DateTime(2021, 11, 8), new DateTime(2021, 11, 9), new DateTime(2021, 11, 11), },
                        2, 6, new DateTime[] { new DateTime(2021, 11, 3), new DateTime(2021, 11, 11) }
                    },
                    new object[] {
                        DayOfWeek.Monday,
                        new DateTime[] { new DateTime(2021, 11, 1), 
                                        new DateTime(2021, 11, 8), new DateTime(2021, 11, 9), new DateTime(2021, 11, 11), },
                        2, 4, new DateTime[] { new DateTime(2021, 11, 11) }
                    },

                    // Target: 4 times per week
                    new object[] {
                        DayOfWeek.Monday,
                        new DateTime[] { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), new DateTime(2021, 11,3) },
                        4, 3, new DateTime[] { }
                    },
                    new object[] {
                        DayOfWeek.Monday,
                        new DateTime[] { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), new DateTime(2021, 11,3), new DateTime(2021, 11, 4) },
                        4, 4, new DateTime[] { new DateTime(2021, 11, 4) }
                    },
                    new object[] {
                        DayOfWeek.Monday,
                        new DateTime[] { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), new DateTime(2021, 11,3), new DateTime(2021, 11, 4), new DateTime(2021, 11, 5) },
                        4, 5, new DateTime[] { new DateTime(2021, 11, 5) }
                    },
                    new object[] {
                        DayOfWeek.Monday,
                        new DateTime[] { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), new DateTime(2021, 11,3), new DateTime(2021, 11, 4), new DateTime(2021, 11, 5), new DateTime(2021, 11, 6) },
                        4, 6, new DateTime[] { new DateTime(2021, 11, 6) }
                    },

                    // Target: 7 times per week
                    new object[] { 
                        DayOfWeek.Monday,
                        new DateTime[] { new DateTime(2021, 11, 1) },
                        7, 1, new DateTime[] { }
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(WeeklyDates))]
        public async Task CalculatePoints_Weekly_NumberOfTimes(DayOfWeek dow, DateTime[] arRecordDates, int completeCondition, int recordCount, 
            DateTime[] arTargetRuleDate)
        {
            var context = this.fixture.GetCurrentDataContext();
            UserHabitRecordsController control = new(context);

            // Add Invited User
            //
            DataSetupUtility.CreateInviteUser(context, test_manager, test_user1);

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
            habit.CompleteCondition = completeCondition;
            habit.StartDate = (int)dow;
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
            foreach(DateTime dt in arRecordDates)
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
            Assert.Equal(recordCount, dbrecords.Count);

            // Ensure rule is assigned correctly
            if (arTargetRuleDate.Length > 0)
            {
                var rulecnt = 0;
                dbrecords.ForEach(dbr =>
                {
                    if (dbr.RuleID != null)
                    {
                        rulecnt++;

                        var ridx = -1;
                        for(int i = 0; i < arTargetRuleDate.Length; i ++)
                        {
                            if (arTargetRuleDate[i].Date == dbr.RecordDate.Date)
                            {
                                ridx = i;
                                return;
                            }
                        }
                        Assert.NotEqual(-1, ridx);
                    }
                });
                Assert.Equal(arTargetRuleDate.Length, rulecnt);
            }

            DataSetupUtility.ClearUserHabitData(context, nNewHabitID);
            DataSetupUtility.DeleteInviteUser(context, test_manager, test_user1);
            context.SaveChanges();

            await context.DisposeAsync();
        }
    }
}