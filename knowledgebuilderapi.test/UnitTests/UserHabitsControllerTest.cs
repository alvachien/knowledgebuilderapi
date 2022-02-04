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
    public class UserHabitsControllerTest : IDisposable
    {
        SqliteDatabaseFixture fixture = null;
        private List<Int32> objectsCreated = new List<Int32>();

        public UserHabitsControllerTest(SqliteDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        public void Dispose()
        {
            CleanupCreatedEntries();
        }

        private void CleanupCreatedEntries()
        {
            if (objectsCreated.Count > 0)
            {
                var context = this.fixture.GetCurrentDataContext();
                foreach (var kid in objectsCreated)
                    DataSetupUtility.DeleteUserHabit(context, kid);

                objectsCreated.Clear();
                context.SaveChanges();
            }
        }

        [Fact]
        public async Task TestCase_InvalidValidity()
        {
            var context = fixture.GetCurrentDataContext();
            UserHabitsController control = new UserHabitsController(context);

            UserHabit habit = new UserHabit()
            {
                ValidFrom = new DateTime(2021, 11, 1),
                ValidTo = new DateTime(2020, 11, 1),
                Category = HabitCategory.Positive,
                Name = "Test1",
                TargetUser = "Tester1",
                Frequency = HabitFrequency.Daily,
            };

            var rst = await control.Post(habit);
            if (rst != null)
            {
                BadRequestObjectResult badrequest = (BadRequestObjectResult)rst;
                Assert.NotNull(badrequest);
                Assert.Equal("Invalid Validity", badrequest.Value);
            }
        }

        [Fact]
        public async Task TestCase_InvalidWeeklyStartDate()
        {
            var context = fixture.GetCurrentDataContext();
            UserHabitsController control = new UserHabitsController(context);

            UserHabit habit = new UserHabit()
            {
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2022, 12, 31),
                Category = HabitCategory.Positive,
                Name = "Test1",
                TargetUser = "Tester1",
                Frequency = HabitFrequency.Weekly,
                StartDate = 8,
            };

            var rst = await control.Post(habit);
            if (rst != null)
            {
                BadRequestObjectResult badrequest = (BadRequestObjectResult)rst;
                Assert.NotNull(badrequest);
                Assert.Equal("Invalid start date", badrequest.Value);
            }
        }

        [Fact]
        public async Task TestCase_InvalidMonthlyStartDate1()
        {
            var context = fixture.GetCurrentDataContext();
            UserHabitsController control = new UserHabitsController(context);

            UserHabit habit = new UserHabit()
            {
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2022, 12, 31),
                Category = HabitCategory.Positive,
                Name = "Test1",
                TargetUser = "Tester1",
                Frequency = HabitFrequency.Monthly,
                StartDate = 32,
            };

            try
            {
                var rst = await control.Post(habit);
            }
            catch (Exception ex)
            {

            }
            //if (rst != null)
            //{
            //    BadRequestObjectResult badrequest = (BadRequestObjectResult)rst;
            //    Assert.NotNull(badrequest);
            //    Assert.Equal("Invalid start date", badrequest.Value);
            //}
        }

        [Fact]
        public async Task TestCase_InvalidMonthlyStartDate2()
        {
            var context = fixture.GetCurrentDataContext();
            UserHabitsController control = new UserHabitsController(context);

            UserHabit habit = new UserHabit()
            {
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2022, 12, 31),
                Category = HabitCategory.Positive,
                Name = "Test1",
                TargetUser = "Tester1",
                Frequency = HabitFrequency.Monthly,
                StartDate = 0,
            };

            var rst = await control.Post(habit);
            if (rst != null)
            {
                BadRequestObjectResult badrequest = (BadRequestObjectResult)rst;
                Assert.NotNull(badrequest);
                Assert.Equal("Invalid start date", badrequest.Value);
            }
        }
    }
}
