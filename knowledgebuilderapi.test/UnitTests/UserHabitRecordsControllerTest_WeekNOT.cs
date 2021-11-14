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

namespace knowledgebuilderapi.test.UnitTests
{
    //public class UserHabitRecordsControllerTestData_WeekNoOfTimes: IXunitSerializable
    //{
    //    public DayOfWeek Dow { get; set; }
    //    public List<DateTime> RecordDatesList{ get; set; }
    //    public int CompleteCondition { get; set; }
    //    public int RecordCount { get; set; }
    //    public List<DateTime> TargetRuleDateList { get; set; }
    //    private Guid InstanceID { get; }

    //    public UserHabitRecordsControllerTestData_WeekNoOfTimes()
    //    {
    //        this.RecordDatesList = new List<DateTime>();
    //        this.TargetRuleDateList = new List<DateTime>();
    //        this.InstanceID = new Guid();
    //    }
    //    public UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek dow,
    //        List<DateTime> listRecordDates, int completeCondition, int recordCount,
    //        List<DateTime> listTargetRuleDate) : this()
    //    {
    //        this.Dow = dow;
    //        this.CompleteCondition = completeCondition;
    //        this.RecordCount = recordCount;
    //        foreach(DateTime dt in listRecordDates)
    //            this.RecordDatesList.Add(dt);
    //        foreach (DateTime dt in listTargetRuleDate)
    //            this.TargetRuleDateList.Add(dt);
    //    }

    //    public void Deserialize(IXunitSerializationInfo info)
    //    {
    //        Dow = info.GetValue<DayOfWeek>(nameof(Dow));
    //        RecordDatesList = info.GetValue<List<DateTime>>(nameof(RecordDatesList));
    //        CompleteCondition = info.GetValue<int>(nameof(CompleteCondition));
    //        RecordCount = info.GetValue<int>(nameof(RecordCount));
    //        TargetRuleDateList = info.GetValue<List<DateTime>>(nameof(TargetRuleDateList));
    //    }

    //    public void Serialize(IXunitSerializationInfo info)
    //    {
    //        info.AddValue(nameof(Dow), Dow, typeof(DayOfWeek));
    //        info.AddValue(nameof(RecordDatesList), RecordDatesList, typeof(List<DateTime>));
    //        info.AddValue(nameof(CompleteCondition), CompleteCondition, typeof(int));
    //        info.AddValue(nameof(RecordCount), RecordCount, typeof(int));
    //        info.AddValue(nameof(TargetRuleDateList), TargetRuleDateList, typeof(List<DateTime>));
    //    }

    //    public override string ToString()
    //    {
    //        return this.InstanceID.ToString("N");
    //    }
    //}

    [Collection("KBAPI_UnitTests#1")]
    public class UserHabitRecordsControllerTest_WeekNOT : UserHabitRecordsControllerTestAbtract
    {
        public UserHabitRecordsControllerTest_WeekNOT(SqliteDatabaseFixture fixture)
            : base(fixture)
        {
        }

        //    public static IEnumerable<object[]> GetInputtedData()
        //    {
        //        // Target: 1 time per week
        //        yield return new object[] { 
        //            new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
        //            new List<DateTime> { new DateTime(2021, 11, 1) },
        //            1, 1, new List<DateTime> { new DateTime(2021, 11, 1) }) };
        //        yield return new object[] {
        //            new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Tuesday,
        //            new List<DateTime> { new DateTime(2021, 11, 2) },
        //            1, 1, new List<DateTime> { new DateTime(2021, 11, 2) }) };
        //        yield return new object[] {
        //            new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Wednesday,
        //            new List<DateTime> { new DateTime(2021, 11, 3) },
        //            1, 1, new List<DateTime> { new DateTime(2021, 11, 3) }) };
        //        yield return new object[] {
        //            new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Thursday,
        //            new List<DateTime> { new DateTime(2021, 11, 4) },
        //            1, 1, new List<DateTime> { new DateTime(2021, 11, 4) }) };
        //        yield return new object[] {
        //            new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Friday,
        //            new List<DateTime> { new DateTime(2021, 11, 5) },
        //            1, 1, new List<DateTime> { new DateTime(2021, 11, 5) }) };
        //        yield return new object[] {
        //            new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Saturday,
        //            new List<DateTime> { new DateTime(2021, 11, 6) },
        //            1, 1, new List<DateTime> { new DateTime(2021, 11, 6) }) };
        //        yield return new object[] {
        //            new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Sunday,
        //            new List<DateTime> { new DateTime(2021, 11, 7) },
        //            1, 1, new List<DateTime> { new DateTime(2021, 11, 7) }) };


        //        // Target: 2 times per week
        //        yield return new object[] {
        //            new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
        //            new List<DateTime> { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2) },
        //            2, 1, new List<DateTime> {} ) };
        //        yield return new object[] {
        //            new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
        //            new List<DateTime> { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2) },
        //            2, 2, new List<DateTime> { new DateTime(2021, 11, 2)  } ) };
        //        yield return new object[] {
        //            new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
        //            new List<DateTime> { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), new DateTime(2021, 11, 3) },
        //            2, 3, new List<DateTime> { new DateTime(2021, 11, 3)  } ) };
        //        yield return new object[] {
        //            new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
        //            new List<DateTime> { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), new DateTime(2021, 11, 3), new DateTime(2021, 11, 4) },
        //            2, 4, new List<DateTime> { new DateTime(2021, 11, 4)  } ) };
        //        yield return new object[] {
        //            new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
        //            new List<DateTime> { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), new DateTime(2021, 11, 3),
        //                                 new DateTime(2021, 11, 8), new DateTime(2021, 11, 9), new DateTime(2021, 11, 11), },
        //            2, 6, new List<DateTime> { new DateTime(2021, 11, 3), new DateTime(2021, 11, 11) } ) };
        //        yield return new object[] {
        //            new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
        //            new List<DateTime> { new DateTime(2021, 11, 1),
        //                                 new DateTime(2021, 11, 8), new DateTime(2021, 11, 9), new DateTime(2021, 11, 11), },
        //            2, 4, new List<DateTime> { new DateTime(2021, 11, 11) } ) };


        //        // Target: 4 times per week
        //        yield return new object[] {
        //            new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
        //            new List<DateTime> { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), new DateTime(2021, 11, 3), },
        //            4, 3, new List<DateTime> {  } ) };
        //        yield return new object[] {
        //            new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
        //            new List<DateTime> { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), new DateTime(2021, 11, 3), new DateTime(2021, 11, 4), },
        //            4, 4, new List<DateTime> { new DateTime(2021, 11, 4) } ) };
        //        yield return new object[] {
        //            new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
        //            new List<DateTime> { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), new DateTime(2021, 11, 3), new DateTime(2021, 11, 4), new DateTime(2021, 11, 5), },
        //            4, 5, new List<DateTime> { new DateTime(2021, 11, 5) } ) };
        //        yield return new object[] {
        //            new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
        //            new List<DateTime> { new DateTime(2021, 11, 1), new DateTime(2021, 11, 2), new DateTime(2021, 11, 3), new DateTime(2021, 11, 4), new DateTime(2021, 11, 5), new DateTime(2021, 11, 6), },
        //            4, 6, new List<DateTime> { new DateTime(2021, 11, 6) } ) };

        //        // Target: 7 times per week
        //        yield return new object[] {
        //            new UserHabitRecordsControllerTestData_WeekNoOfTimes(DayOfWeek.Monday,
        //            new List<DateTime> { new DateTime(2021, 11, 1), },
        //            7, 1, new List<DateTime> {} ) };
        //    }

        //    [Theory]
        //    [MemberData(nameof(GetInputtedData))]
        //    public async Task CalculatePoints_Weekly_NumberOfTimes(UserHabitRecordsControllerTestData_WeekNoOfTimes testData)
        //    {
        //        var context = this.fixture.GetCurrentDataContext();
        //        UserHabitRecordsController control = new(context);

        //        // Add Habit, Habit Rule
        //        UserHabit habit = new UserHabit();
        //        habit.TargetUser = test_user1;
        //        habit.ValidFrom = new DateTime(2021, 1, 1);
        //        habit.ValidTo = new DateTime(2022, 12, 31);
        //        habit.Name = "Habit_1";
        //        habit.Category = HabitCategory.Positive;
        //        habit.Comment = habit.Name;
        //        habit.Frequency = HabitFrequency.Weekly;
        //        habit.CompleteCategory = HabitCompleteCategory.NumberOfTimes;
        //        habit.CompleteCondition = testData.CompleteCondition;
        //        habit.StartDate = (int)testData.Dow;
        //        context.UserHabits.Add(habit);
        //        context.SaveChanges();
        //        Int32 nNewHabitID = habit.ID;

        //        UserHabitRule rule1 = new UserHabitRule();
        //        rule1.HabitID = habit.ID;
        //        rule1.RuleID = 1;
        //        rule1.ContinuousRecordFrom = 1;
        //        rule1.ContinuousRecordTo = 2;
        //        rule1.Point = 1;
        //        context.UserHabitRules.Add(rule1);

        //        UserHabitRule rule2 = new UserHabitRule();
        //        rule2.HabitID = habit.ID;
        //        rule2.RuleID = 2;
        //        rule2.ContinuousRecordFrom = 2;
        //        rule1.ContinuousRecordTo = 3;
        //        rule2.Point = 2;
        //        context.UserHabitRules.Add(rule2);

        //        UserHabitRule rule3 = new UserHabitRule();
        //        rule3.HabitID = habit.ID;
        //        rule3.RuleID = 3;
        //        rule3.ContinuousRecordFrom = 3;
        //        rule3.Point = 4;
        //        context.UserHabitRules.Add(rule3);
        //        context.SaveChanges();

        //        // Add user record.
        //        //Boolean lastRst = false;
        //        foreach(DateTime dt in testData.RecordDatesList)
        //        {
        //            UserHabitRecord record = new UserHabitRecord();
        //            record.HabitID = habit.ID;
        //            record.RecordDate = dt;
        //            record.Comment = "Test1";
        //            var rst = control.Post(record);
        //            Assert.NotNull(rst);
        //            if (rst != null)
        //            {
        //                CreatedODataResult<UserHabitRecord> rstrecord = (CreatedODataResult<UserHabitRecord>)rst.Result;
        //                Assert.NotNull(rstrecord);

        //                //lastRst = rstrecord.Entity.RuleID.HasValue;
        //            }
        //        }

        //        // Check on DB directly
        //        var dbrecords = (from dbrecord in context.UserHabitRecords
        //                     where dbrecord.HabitID == habit.ID
        //                     select dbrecord).ToList();
        //        Assert.Equal(testData.RecordCount, dbrecords.Count);

        //        // Ensure rule is assigned correctly
        //        if (testData.TargetRuleDateList.Count > 0)
        //        {
        //            var rulecnt = 0;
        //            dbrecords.ForEach(dbr =>
        //            {
        //                if (dbr.RuleID != null)
        //                {
        //                    rulecnt++;

        //                    var ridx = testData.TargetRuleDateList.FindIndex(rd => rd.Date == dbr.RecordDate.Date);
        //                    Assert.NotEqual(-1, ridx);
        //                }
        //            });
        //            Assert.Equal(testData.TargetRuleDateList.Count, rulecnt);
        //        }

        //        DataSetupUtility.ClearUserHabitData(context, nNewHabitID);
        //        context.SaveChanges();

        //        await context.DisposeAsync();
        //    }
    }
}
