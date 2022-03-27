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
using knowledgebuilderapi.test.common;

namespace knowledgebuilderapi.test.unittest
{
    [Collection("KBAPI_UnitTests#1")]
    public class AwardUsersControllerTest : IDisposable
    {
        SqliteDatabaseFixture fixture = null;

        public AwardUsersControllerTest(SqliteDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        public void Dispose()
        {
        }

        [Theory]
        [InlineData(null)]
        [InlineData(DataSetupUtility.UserA)]
        [InlineData(DataSetupUtility.UserB)]
        public async Task TestCase_GetList(String usr)
        {
            var context = fixture.GetCurrentDataContext();

            fixture.InitializeTestData();

            var control = new AwardUsersController(context);

            try
            {
                control.Get();
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }

            if (!String.IsNullOrEmpty(usr))
            {
                var userclaim = DataSetupUtility.GetClaimForUser(usr);
                control.ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = userclaim }
                };

                var getrst = control.Get();
                Assert.NotNull(getrst);

                if (usr == DataSetupUtility.UserA)
                    Assert.Equal(1, getrst.Count());
                else if (usr == DataSetupUtility.UserB)
                    Assert.Equal(0, getrst.Count());
            }

            await context.DisposeAsync();
        }
    }
}

