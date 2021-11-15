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

            this.createUser();
        }

        public void Dispose()
        {
            this.clearUser();
        }

        private void createUser()
        {
            // Create invite user
            var context = this.fixture.GetCurrentDataContext();

            DataSetupUtility.CreateInviteUser(context, test_manager, test_user1);
            context.SaveChanges();
        }
        private void clearUser()
        {
            var context = this.fixture.GetCurrentDataContext();
            DataSetupUtility.DeleteInviteUser(context, test_manager, test_user1);
            context.SaveChanges();
            context.Dispose();
        }

    }
}
