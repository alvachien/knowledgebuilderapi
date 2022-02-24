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
    public class TagCountsControllerTest
    {
        SqliteDatabaseFixture fixture = null;

        public TagCountsControllerTest(SqliteDatabaseFixture fixture)
        {
            this.fixture = fixture;
            this.fixture.InitializeTestData();
        }

        [Fact]
        public async Task TestCase_ReadInfo()
        {
            var context = this.fixture.GetCurrentDataContext();

            var control = new TagCountsController(context);
            var getrst = control.Get();
            Assert.NotNull(getrst);

            await context.DisposeAsync();
        }
    }
}
