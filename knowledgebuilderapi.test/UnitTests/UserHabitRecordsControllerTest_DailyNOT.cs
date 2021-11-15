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
        public DayOfWeek Dow { get; set; }
        public List<DateTime> RecordDatesList { get; set; }
        public int CompleteCondition { get; set; }
        public int RecordCount { get; set; }
        public List<DateTime> TargetRuleDateList { get; set; }

        public UserHabitRecordsControllerTestData_DailyNoOfTimes()
        {
            this.RecordDatesList = new List<DateTime>();
            this.TargetRuleDateList = new List<DateTime>();
        }

        public UserHabitRecordsControllerTestData_DailyNoOfTimes(DayOfWeek dow,
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
    public class UserHabitRecordsControllerTest_DailyNOT : UserHabitRecordsControllerTestAbtract
    {
        public UserHabitRecordsControllerTest_DailyNOT(SqliteDatabaseFixture fixture)
            : base(fixture)
        {
        }
    }
}
