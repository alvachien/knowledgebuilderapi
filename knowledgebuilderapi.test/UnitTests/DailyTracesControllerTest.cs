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
        public async Task CalculatePoints_GoToBedTimeRule()
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
            #region [20-21]
            AwardRule r1 = new()
            {
                RuleType = AwardRuleType.GoToBedTime,
                TargetUser = "AAA",
                TimeStart = 20,
                TimeEnd = 21,
                DaysFrom = 1,
                DaysTo = 1,
                Point = 1,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_1_1"
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
                Point = 2,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_2_1"
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
                Point = 5,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_3_1"
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
                Point = 10,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_4_1"
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
                Point = 20,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_5_1"
            };
            context.Add(r1);
            #endregion

            // [21-22]
            #region [21-22]
            r1 = new()
            {
                RuleType = AwardRuleType.GoToBedTime,
                TargetUser = "AAA",
                TimeStart = 21,
                TimeEnd = 22,
                DaysFrom = 1,
                DaysTo = 1,
                Point = -1,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_1_2"
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
                Point = -2,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_2_2"
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
                Point = -5,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_3_2"
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
                Point = -10,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_4_2"
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
                Point = -20,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_5_2"
            };
            context.Add(r1);
            #endregion

            // [22-24]
            #region [22-24]
            r1 = new()
            {
                RuleType = AwardRuleType.GoToBedTime,
                TargetUser = "AAA",
                TimeStart = 22,
                TimeEnd = 24,
                DaysFrom = 1,
                DaysTo = 1,
                Point = -5,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_1_3"
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
                Point = -10,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_2_3"
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
                Point = -15,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_3_3"
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
                Point = -20,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_4_3"
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
                Point = -25,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_5_3"
            };
            context.Add(r1);
            #endregion
            await context.SaveChangesAsync();

            // Add first daily trace on 2021-05-01
            DailyTrace dt = new()
            {
                RecordDate = new DateTime(2021, 5, 1),
                TargetUser = "AAA",
                GoToBedTime = (decimal)20.5
            };
            var points = control.CalculatePoints(dt);
            Assert.Single(points);
            Assert.Equal(1, points[0].Point);
            context.Add(points[0]);
            await context.SaveChangesAsync();

            // Add second daily trace on 2021-05-02
            dt = new()
            {
                RecordDate = new DateTime(2021, 5, 2),
                TargetUser = "AAA",
                GoToBedTime = (decimal)20.6
            };
            points = control.CalculatePoints(dt);
            Assert.Single(points);
            Assert.Equal(2, points[0].Point);
            context.Add(points[0]);
            await context.SaveChangesAsync();

            // Add third daily trace on 2021-05-03
            dt = new()
            {
                RecordDate = new DateTime(2021, 5, 3),
                TargetUser = "AAA",
                GoToBedTime = (decimal)20.7
            };
            points = control.CalculatePoints(dt);
            Assert.Single(points);
            Assert.Equal(5, points[0].Point);
            context.Add(points[0]);
            await context.SaveChangesAsync();

            // Add fourth daily trace on 2021-05-05
            dt = new()
            {
                RecordDate = new DateTime(2021, 5, 5),
                TargetUser = "AAA",
                GoToBedTime = (decimal)20.7
            };
            points = control.CalculatePoints(dt);
            Assert.Single(points);
            Assert.Equal(1, points[0].Point);
            context.Add(points[0]);
            await context.SaveChangesAsync();

            // Clean award data
            DataSetupUtility.DeleteAwardData(context);

            await context.DisposeAsync();
        }
        
        [Fact]
        public async Task CalculatePoints_SchoolWorkTimeRule()
        {
            var context = this.fixture.GetCurrentDataContext();
            DailyTracesController control = new(context);
            // RuleMatrix
            // Days      [13 - 18]   [18 - 20] [20 - 24]
            // [1,1]        1           -1          -5
            // [2,2]        2           -2          -10
            // [3,3]        5           -5          -15
            // [4,4]        10          -10         -20
            // [5,INFIN]    20          -20         -25
            //
            // [13-18]
            #region [13-18]
            AwardRule r1 = new()
            {
                RuleType = AwardRuleType.SchoolWorkTime,
                TargetUser = "AAA",
                TimeStart = 13,
                TimeEnd = 18,
                DaysFrom = 1,
                DaysTo = 1,
                Point = 1,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_1_1"
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.SchoolWorkTime,
                TargetUser = "AAA",
                TimeStart = 13,
                TimeEnd = 18,
                DaysFrom = 2,
                DaysTo = 2,
                Point = 2,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_2_1"
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.SchoolWorkTime,
                TargetUser = "AAA",
                TimeStart = 13,
                TimeEnd = 18,
                DaysFrom = 3,
                DaysTo = 3,
                Point = 5,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_3_1"
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.SchoolWorkTime,
                TargetUser = "AAA",
                TimeStart = 13,
                TimeEnd = 18,
                DaysFrom = 4,
                DaysTo = 4,
                Point = 10,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_4_1"
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.SchoolWorkTime,
                TargetUser = "AAA",
                TimeStart = 13,
                TimeEnd = 18,
                DaysFrom = 5,
                DaysTo = 9999,
                Point = 20,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_5_1"
            };
            context.Add(r1);
            #endregion

            // [18 - 20]
            #region [18 - 20]
            r1 = new()
            {
                RuleType = AwardRuleType.SchoolWorkTime,
                TargetUser = "AAA",
                TimeStart = 18,
                TimeEnd = 20,
                DaysFrom = 1,
                DaysTo = 1,
                Point = -1,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_1_2"
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.SchoolWorkTime,
                TargetUser = "AAA",
                TimeStart = 18,
                TimeEnd = 20,
                DaysFrom = 2,
                DaysTo = 2,
                Point = -2,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_2_2"
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.SchoolWorkTime,
                TargetUser = "AAA",
                TimeStart = 18,
                TimeEnd = 20,
                DaysFrom = 3,
                DaysTo = 3,
                Point = -5,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_3_2"
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.SchoolWorkTime,
                TargetUser = "AAA",
                TimeStart = 18,
                TimeEnd = 20,
                DaysFrom = 4,
                DaysTo = 4,
                Point = -10,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_4_2"
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.SchoolWorkTime,
                TargetUser = "AAA",
                TimeStart = 18,
                TimeEnd = 20,
                DaysFrom = 5,
                DaysTo = 9999,
                Point = -20,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_5_2"
            };
            context.Add(r1);
            #endregion

            // [20-24]
            #region [20-24]
            r1 = new()
            {
                RuleType = AwardRuleType.SchoolWorkTime,
                TargetUser = "AAA",
                TimeStart = 20,
                TimeEnd = 24,
                DaysFrom = 1,
                DaysTo = 1,
                Point = -5,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_1_3"
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.SchoolWorkTime,
                TargetUser = "AAA",
                TimeStart = 20,
                TimeEnd = 24,
                DaysFrom = 2,
                DaysTo = 2,
                Point = -10,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_2_3"
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.SchoolWorkTime,
                TargetUser = "AAA",
                TimeStart = 20,
                TimeEnd = 24,
                DaysFrom = 3,
                DaysTo = 3,
                Point = -15,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_3_3"
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.SchoolWorkTime,
                TargetUser = "AAA",
                TimeStart = 20,
                TimeEnd = 24,
                DaysFrom = 4,
                DaysTo = 4,
                Point = -20,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_4_3"
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.SchoolWorkTime,
                TargetUser = "AAA",
                TimeStart = 20,
                TimeEnd = 24,
                DaysFrom = 5,
                DaysTo = 9999,
                Point = -25,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Gotobed_5_3"
            };
            context.Add(r1);
            #endregion
            await context.SaveChangesAsync();

            // Add first daily trace on 2021-05-01
            DailyTrace dt = new()
            {
                RecordDate = new DateTime(2021, 5, 1),
                TargetUser = "AAA",
                SchoolWorkTime = (decimal)18.5
            };
            var points = control.CalculatePoints(dt);
            Assert.Single(points);
            Assert.Equal(-1, points[0].Point);
            context.Add(points[0]);
            await context.SaveChangesAsync();

            // Add second daily trace on 2021-05-02
            dt = new()
            {
                RecordDate = new DateTime(2021, 5, 2),
                TargetUser = "AAA",
                SchoolWorkTime = (decimal)19.6
            };
            points = control.CalculatePoints(dt);
            Assert.Single(points);
            Assert.Equal(-2, points[0].Point);
            context.Add(points[0]);
            await context.SaveChangesAsync();

            // Add third daily trace on 2021-05-03
            dt = new()
            {
                RecordDate = new DateTime(2021, 5, 3),
                TargetUser = "AAA",
                SchoolWorkTime = (decimal)16.7
            };
            points = control.CalculatePoints(dt);
            Assert.Single(points);
            Assert.Equal(1, points[0].Point);
            context.Add(points[0]);
            await context.SaveChangesAsync();

            // Add fourth daily trace on 2021-05-05
            dt = new()
            {
                RecordDate = new DateTime(2021, 5, 5),
                TargetUser = "AAA",
                SchoolWorkTime = (decimal)20.7
            };
            points = control.CalculatePoints(dt);
            Assert.Single(points);
            Assert.Equal(-5, points[0].Point);
            context.Add(points[0]);
            await context.SaveChangesAsync();

            // Clean award data
            DataSetupUtility.DeleteAwardData(context);

            await context.DisposeAsync();
        }
        
        [Fact]
        public async Task CalculatePoints_ErrorCollectionRule()
        {
            var context = this.fixture.GetCurrentDataContext();
            DailyTracesController control = new(context);
            // RuleMatrix
            // Days      [Yes]   [No] 
            // [1,1]        2     -5
            // [2,2]        5     -10
            // [3,3]        10    -20
            // [4,4]        15    -30
            // [5,INFIN]    20    -40
            //
            // [Yes]
            #region [Yes]
            AwardRule r1 = new()
            {
                RuleType = AwardRuleType.ErrorCollectionHabit,
                TargetUser = "AAA",
                DoneOfFact = true,
                DaysFrom = 1,
                DaysTo = 1,
                Point = 2,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Rule_1_1"
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.ErrorCollectionHabit,
                TargetUser = "AAA",
                DoneOfFact = true,
                DaysFrom = 2,
                DaysTo = 2,
                Point = 5,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Rule_2_1"
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.ErrorCollectionHabit,
                TargetUser = "AAA",
                DoneOfFact = true,
                DaysFrom = 3,
                DaysTo = 3,
                Point = 10,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Rule_3_1"
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.ErrorCollectionHabit,
                TargetUser = "AAA",
                DoneOfFact = true,
                DaysFrom = 4,
                DaysTo = 4,
                Point = 15,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Rule_4_1"
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.ErrorCollectionHabit,
                TargetUser = "AAA",
                DoneOfFact = true,
                DaysFrom = 5,
                DaysTo = 9999,
                Point = 20,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Rule_5_1"
            };
            context.Add(r1);
            #endregion

            #region [No]
            r1 = new()
            {
                RuleType = AwardRuleType.ErrorCollectionHabit,
                TargetUser = "AAA",
                DoneOfFact = false,
                DaysFrom = 1,
                DaysTo = 1,
                Point = -5,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Rule_1_1"
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.ErrorCollectionHabit,
                TargetUser = "AAA",
                DoneOfFact = false,
                DaysFrom = 2,
                DaysTo = 2,
                Point = -10,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Rule_2_1"
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.ErrorCollectionHabit,
                TargetUser = "AAA",
                DoneOfFact = false,
                DaysFrom = 3,
                DaysTo = 3,
                Point = -20,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Rule_3_1"
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.ErrorCollectionHabit,
                TargetUser = "AAA",
                DoneOfFact = false,
                DaysFrom = 4,
                DaysTo = 4,
                Point = -30,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Rule_4_1"
            };
            context.Add(r1);
            r1 = new AwardRule
            {
                RuleType = AwardRuleType.ErrorCollectionHabit,
                TargetUser = "AAA",
                DoneOfFact = false,
                DaysFrom = 5,
                DaysTo = 9999,
                Point = -40,
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTo = new DateTime(2021, 12, 31),
                Desp = "Rule_5_1"
            };
            context.Add(r1);
            #endregion

            await context.SaveChangesAsync();

            // Add first daily trace on 2021-05-01
            DailyTrace dt = new()
            {
                RecordDate = new DateTime(2021, 5, 1),
                TargetUser = "AAA",
                ErrorsCollection = true
            };
            var points = control.CalculatePoints(dt);
            Assert.Single(points);
            Assert.Equal(2, points[0].Point);
            context.Add(points[0]);
            await context.SaveChangesAsync();

            // Add second daily trace on 2021-05-02
            dt = new()
            {
                RecordDate = new DateTime(2021, 5, 2),
                TargetUser = "AAA",
                ErrorsCollection = true
            };
            points = control.CalculatePoints(dt);
            Assert.Single(points);
            Assert.Equal(5, points[0].Point);
            context.Add(points[0]);
            await context.SaveChangesAsync();

            // Add third daily trace on 2021-05-03
            dt = new()
            {
                RecordDate = new DateTime(2021, 5, 3),
                TargetUser = "AAA",
                ErrorsCollection = false
            };
            points = control.CalculatePoints(dt);
            Assert.Single(points);
            Assert.Equal(-5, points[0].Point);
            context.Add(points[0]);
            await context.SaveChangesAsync();

            // Add fourth daily trace on 2021-05-05
            dt = new()
            {
                RecordDate = new DateTime(2021, 5, 5),
                TargetUser = "AAA",
                ErrorsCollection = false
            };
            points = control.CalculatePoints(dt);
            Assert.Single(points);
            Assert.Equal(-5, points[0].Point);
            context.Add(points[0]);
            await context.SaveChangesAsync();

            // Clean award data
            DataSetupUtility.DeleteAwardData(context);

            await context.DisposeAsync();
        }
    }
}
