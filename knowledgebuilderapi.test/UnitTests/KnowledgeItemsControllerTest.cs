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
using Microsoft.AspNet.OData.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;

namespace knowledgebuilderapi.test.UnitTests
{
    [Collection("KBAPI_UnitTests#1")]
    public class KnowledgeItemsControllerTest : IDisposable
    {
        SqliteDatabaseFixture fixture = null;
        private List<Int32> objectsCreated = new List<Int32>();

        public KnowledgeItemsControllerTest(SqliteDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        public void Dispose()
        {
            CleanupCreatedEntries();
        }

        [Fact]
        public async Task TestCase1()
        {
            var context = this.fixture.GetCurrentDataContext();
            KnowledgeItemsController control = new KnowledgeItemsController(context);

            // Step 1. Read all - 0
            var rsts = control.Get();
            var rstscnt = await rsts.CountAsync();
            Assert.Equal(0, rstscnt);

            // Step 2. Create one know ledge item
            var ki = new KnowledgeItem()
            {
                Category = KnowledgeItemCategory.Concept,
                Title = "Test 1",
                Content  = "Test 1 Content",
            };
            var rst = await control.Post(ki);
            Assert.NotNull(rst);
            var rst2 = Assert.IsType<CreatedODataResult<KnowledgeItem>>(rst);
            Assert.Equal(rst2.Entity.Title, ki.Title);
            Assert.Equal(rst2.Entity.Content, ki.Content);
            var firstid = rst2.Entity.ID;
            Assert.True(firstid > 0);
            objectsCreated.Add(firstid);

            // Step 3. Read all - 1
            rsts = control.Get();
            rstscnt = await rsts.CountAsync();
            Assert.Equal(1, rstscnt);

            // Step 5. Delete
            rst = await control.Delete(firstid);

            rsts = control.Get();
            rstscnt = await rsts.CountAsync();
            Assert.Equal(0, rstscnt);

            //// Step 9. Read it again via OData way
            //var httpContext = new DefaultHttpContext(); // or mock a `HttpContext`
            //httpContext.Request.Path = "/api/KnowledgeItems";
            //httpContext.Request.QueryString = new QueryString("?$select=ID, Title");
            //httpContext.Request.Method = "GET";
            //var routeData = new RouteData();
            //routeData.Values.Add("odataPath", "KnowledgeItems");
            //routeData.Values.Add("action", "GET");

            //// Controller needs a controller context 
            //var controllerContext = new ControllerContext()
            //{
            //    RouteData = routeData,
            //    HttpContext = httpContext,
            //};
            //// Assign context to controller
            //control = new KnowledgeItemsController(context)
            //{
            //    ControllerContext = controllerContext,
            //};
            //rsts = control.Get();
            //Assert.NotNull(rsts);

            await context.DisposeAsync();
        }

        private void CleanupCreatedEntries()
        {
            if (objectsCreated.Count > 0)
            {
                var context = this.fixture.GetCurrentDataContext();
                foreach (var kid in objectsCreated)
                    fixture.DeleteKnowledgeItem(context, kid);

                objectsCreated.Clear();
                context.SaveChanges();
            }
        }
    }
}
