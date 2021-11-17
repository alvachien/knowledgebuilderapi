using System;
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
    public class UserHabitRecordsControllerTestData_MonthNoOfTimes : IXunitSerializable
    {
        public int DateInMonth { get; set; }
        public List<DateTime> RecordDateList { get; set; }
        public List<UserHabitRule> RuleList { get; set; }
        public int CompleteCondition { get; set; }
        public List<UserHabitRecord> ExpectedRecordList { get; set; }

        public UserHabitRecordsControllerTestData_MonthNoOfTimes()
        {
            RecordDateList = new List<DateTime>();
            RuleList = new List<UserHabitRule>();
            ExpectedRecordList = new List<UserHabitRecord>();
        }

        public UserHabitRecordsControllerTestData_MonthNoOfTimes(int dateInMonth,
            List<DateTime> listRecordDates, int completeCondition,
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

    public class UserHabitRecordsControllerTest_MonthlyNOT : UserHabitRecordsControllerTestAbtract
    {
        public UserHabitRecordsControllerTest_MonthlyNOT(SqliteDatabaseFixture fixture)
            : base(fixture)
        {
        }

        public static TheoryData<UserHabitRecordsControllerTestData_MonthNoOfTimes> InputtedData =>
            new TheoryData<UserHabitRecordsControllerTestData_MonthNoOfTimes> {
                new UserHabitRecordsControllerTestData_MonthNoOfTimes(
                    1, 
                    new List<DateTime> { new DateTime(2021, 11, 1), }, 
                    1, 
                    new List<UserHabitRule>
                    {
                        new UserHabitRule { ContinuousRecordFrom = 1, ContinuousRecordTo = 2, RuleID = 1, }
                    },
                    new List<UserHabitRecord>
                    {
                        new UserHabitRecord { 
                            RecordDate = new DateTime(2021, 11, 1),
                        },
                    }),
            };


        [Theory]
        [MemberData(nameof(InputtedData))]
        public async Task CalculatePoints_NumberOfCount(UserHabitRecordsControllerTestData_MonthNoOfTimes testData)
        {
            var context = this.fixture.GetCurrentDataContext();
            UserHabitRecordsController control = new(context);

            // Add Habit, Habit Rule
            UserHabit habit = new UserHabit();
            habit.TargetUser = test_user1;
            habit.ValidFrom = new DateTime(2021, 1, 1);
            habit.ValidTo = new DateTime(2022, 12, 31);
            habit.Name = "Habit_Monthly_1";
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
            foreach (var nrecorddate in testData.RecordDateList)
            {
                var nrecord = new UserHabitRecord();
                nrecord.RecordDate = nrecorddate.Date;
                nrecord.HabitID = habit.ID;
                nrecord.Comment = "Test1";
                nrecord.SubID = 1;
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
