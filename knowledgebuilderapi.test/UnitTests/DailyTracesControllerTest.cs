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
    public class DailyTracesControllerTest : IDisposable
    {
        SqliteDatabaseFixture fixture = null;
        private List<Int32> objectsCreated = new List<Int32>();

        public DailyTracesControllerTest(SqliteDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        public void Dispose()
        {
            //CleanupCreatedEntries();
        }

        [Fact]
        public async Task TestCase1()
        {
            var context = this.fixture.GetCurrentDataContext();
            DailyTracesController control = new(context);
            // RuleMatrix
            // Days      [20 - 21]   [21 - 22] [22 - 24]
            // [1,1]        1           -1          -5
            // [2,2]        2           -2          -10
            // [3,3]        5           -5          -15
            // [4,4]        10          -10         -20
            // [5,INFIN]    20          -20         -25
            //
            // [20-21]
            AwardRule r1 = new()
            {
                RuleType = AwardRuleType.GoToBedTime,
                TargetUser = "AAA",
                TimeStart = 20,
                TimeEnd = 21,
                DaysFrom = 1,
                DaysTo = 1,
                Point = 1
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.GoToBedTime,
                TargetUser = "AAA",
                TimeStart = 20,
                TimeEnd = 21,
                DaysFrom = 2,
                DaysTo = 2,
                Point = 2
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.GoToBedTime,
                TargetUser = "AAA",
                TimeStart = 20,
                TimeEnd = 21,
                DaysFrom = 3,
                DaysTo = 3,
                Point = 5
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.GoToBedTime,
                TargetUser = "AAA",
                TimeStart = 20,
                TimeEnd = 21,
                DaysFrom = 4,
                DaysTo = 4,
                Point = 10
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.GoToBedTime,
                TargetUser = "AAA",
                TimeStart = 20,
                TimeEnd = 21,
                DaysFrom = 5,
                DaysTo = 9999,
                Point = 20
            };
            context.Add(r1);
            // [21-22]
            r1 = new()
            {
                RuleType = AwardRuleType.GoToBedTime,
                TargetUser = "AAA",
                TimeStart = 21,
                TimeEnd = 22,
                DaysFrom = 1,
                DaysTo = 1,
                Point = -1
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.GoToBedTime,
                TargetUser = "AAA",
                TimeStart = 21,
                TimeEnd = 22,
                DaysFrom = 2,
                DaysTo = 2,
                Point = -2
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.GoToBedTime,
                TargetUser = "AAA",
                TimeStart = 21,
                TimeEnd = 22,
                DaysFrom = 3,
                DaysTo = 3,
                Point = -5
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.GoToBedTime,
                TargetUser = "AAA",
                TimeStart = 21,
                TimeEnd = 22,
                DaysFrom = 4,
                DaysTo = 4,
                Point = -10
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.GoToBedTime,
                TargetUser = "AAA",
                TimeStart = 21,
                TimeEnd = 22,
                DaysFrom = 5,
                DaysTo = 9999,
                Point = -20
            };
            context.Add(r1);
            // [22-24]
            r1 = new()
            {
                RuleType = AwardRuleType.GoToBedTime,
                TargetUser = "AAA",
                TimeStart = 22,
                TimeEnd = 24,
                DaysFrom = 1,
                DaysTo = 1,
                Point = -5
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.GoToBedTime,
                TargetUser = "AAA",
                TimeStart = 22,
                TimeEnd = 24,
                DaysFrom = 2,
                DaysTo = 2,
                Point = -10
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.GoToBedTime,
                TargetUser = "AAA",
                TimeStart = 22,
                TimeEnd = 24,
                DaysFrom = 3,
                DaysTo = 3,
                Point = -15
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.GoToBedTime,
                TargetUser = "AAA",
                TimeStart = 22,
                TimeEnd = 24,
                DaysFrom = 4,
                DaysTo = 4,
                Point = -20
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.GoToBedTime,
                TargetUser = "AAA",
                TimeStart = 22,
                TimeEnd = 24,
                DaysFrom = 5,
                DaysTo = 9999,
                Point = -25
            };
            context.Add(r1);

            // Add one daily trace
            DailyTrace dt = new()
            {
                RecordDate = new DateTime(2021, 5, 1),
                TargetUser = "AAA",
                GoToBedTime = (decimal)20.5
            };
            var points = control.CalculatePoints(dt);
            Assert.Single(points);
            Assert.Equal(1, points[0].Point);
        }
    }
}
