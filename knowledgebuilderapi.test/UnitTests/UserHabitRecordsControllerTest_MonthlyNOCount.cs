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

namespace knowledgebuilderapi.test.UnitTests
{
    public class UserHabitRecordsControllerTestData_MonthNoOfCount : IXunitSerializable
    {
        public int DateInMonth { get; set; }
        public List<UserHabitRecord> RecordList { get; set; }
        public int CompleteCondition { get; set; }
        public int RecordCount { get; set; }
        public List<DateTime> ExpectedRuleDateList { get; set; }

        public UserHabitRecordsControllerTestData_MonthNoOfCount()
        {
            RecordList = new List<UserHabitRecord>();
            ExpectedRuleDateList = new List<DateTime>();
        }

        public UserHabitRecordsControllerTestData_MonthNoOfCount(int dateInMonth,
            List<UserHabitRecord> listRecords, int completeCondition,
            List<UserHabitRule> ruleList,
            List<UserHabitRecord> listExpectedRecords) : this()
        {
            DateInMonth = dateInMonth;
            CompleteCondition = completeCondition;
            if (listRecordDates.Count > 0)
                this.RecordDateList.AddRange(listRecordDates);
            if (ruleList.Count > 0)
                RuleList.AddRange(ruleList);
            if (listExpectedRecords.Count > 0)
                this.ExpectedRecordList.AddRange(listExpectedRecords);
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            String val = info.GetValue<String>("Value");
            UserHabitRecordsControllerTestData_MonthNoOfTimes other = JsonSerializer.Deserialize<UserHabitRecordsControllerTestData_MonthNoOfTimes>(val);

            DateInMonth = other.DateInMonth;
            CompleteCondition = other.CompleteCondition;
            if (other.RecordDateList.Count > 0)
                RecordDateList.AddRange(other.RecordDateList);
            if (other.ExpectedRecordList.Count > 0)
                ExpectedRecordList.AddRange(other.ExpectedRecordList);
            if (other.RuleList.Count > 0)
                RuleList.AddRange(other.RuleList);
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
            };

        [Theory]
        [MemberData(nameof(InputtedData), MemberType = typeof(UserHabitRecordsControllerTestData_MonthNoOfCount))]
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
            habit.Frequency = HabitFrequency.Weekly;
            habit.CompleteCategory = HabitCompleteCategory.NumberOfCount;
            habit.CompleteCondition = testData.CompleteCondition;
            // habit.StartDate = (int)testData.Dow;
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
