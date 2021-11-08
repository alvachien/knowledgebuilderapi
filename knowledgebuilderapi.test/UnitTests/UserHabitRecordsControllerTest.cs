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

        [Fact]
        public async Task CalculatePoints_Weekly()
        {
            var context = this.fixture.GetCurrentDataContext();
            UserHabitRecordsController control = new(context);

            // Add Invited User
            //
            InvitedUser usr = new InvitedUser();
            usr.DisplayAs = test_manager;
            usr.InvitationCode = test_manager;
            usr.UserID = test_manager;
            usr.UserName = test_manager;
            context.InvitedUsers.Add(usr);

            usr = new InvitedUser();
            usr.DisplayAs = test_user1;
            usr.InvitationCode = test_user1;
            usr.UserID = test_user1;
            usr.UserName = test_user1;
            context.InvitedUsers.Add(usr);

            AwardUser aus = new AwardUser();
            aus.Supervisor = test_manager;
            aus.TargetUser = test_user1;
            context.AwardUsers.Add(aus);

            context.SaveChanges();

            // Add Habit, Habit Rule
            UserHabit habit = new UserHabit();
            habit.TargetUser = test_user1;
            habit.ValidFrom = new DateTime(2021, 1, 1);
            habit.ValidTo = new DateTime(2022, 12, 31);
            habit.Name = "Habit_1";
            habit.Category = HabitCategory.Positive;
            habit.Comment = habit.Name;
            habit.Frequency = HabitFrequency.Weekly;
            habit.DoneCriteria = 4;
            habit.StartDate = (int)DayOfWeek.Monday;
            context.UserHabits.Add(habit);
            context.SaveChanges();

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
            UserHabitRecord record = new UserHabitRecord();
            record.HabitID = habit.ID;
            record.RecordDate = new DateTime(2021, 11, 1);
            record.DoneCriteria = 1;
            record.Comment = "Test1";
            var rst = control.Post(record);
            if (rst != null)
            {

            }

            await context.DisposeAsync();
        }
    }
}