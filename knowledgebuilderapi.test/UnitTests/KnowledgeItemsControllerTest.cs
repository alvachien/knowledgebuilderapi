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
    public class KnowledgeItemsControllerTest : IDisposable
    {
        SqliteDatabaseFixture fixture = null;
        private List<Int32> objectsCreated = new List<Int32>();

        public KnowledgeItemsControllerTest(SqliteDatabaseFixture fixture)
        {
            this.fixture = fixture;
            this.fixture.InitializeTestData();
        }

        public void Dispose()
        {
            CleanupCreatedEntries();
        }

        [Fact]
        public async Task TestCase_CRUD()
        {
            var context = fixture.GetCurrentDataContext();
            KnowledgeItemsController control = new KnowledgeItemsController(context);
            var userclaim = DataSetupUtility.GetClaimForUser(DataSetupUtility.UserA);
            control.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = userclaim }
            };

            // Step 1. Read all - 0
            var rsts = control.Get();
            var rstscnt = await rsts.CountAsync();
            var existcnt = context.KnowledgeItems.Count();
            Assert.Equal(existcnt, rstscnt);

            // Step 2. Create one know ledge item            
            var ki = new KnowledgeItem()
            {
                Category = KnowledgeItemCategory.Concept,
                Title = "New Test 1",
                Content  = "New Test 1 Content",
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
            Assert.Equal(existcnt + 1, rstscnt);

            // Step 3a. Read single
            var getrst = control.Get(firstid);
            Assert.NotNull(getrst);
            var getrstresult = Assert.IsType<SingleResult<KnowledgeItem>>(getrst);
            var readitem = getrstresult.Queryable.ToList().ElementAtOrDefault(0);
            Assert.NotNull(readitem);
            Assert.Equal(firstid, readitem.ID);
            Assert.Equal(ki.Category, readitem.Category);
            Assert.Equal(ki.Title, readitem.Title);
            Assert.Equal(ki.Content, readitem.Content);

            // Step 4. Change the exist one by Put
            readitem.Content += "Updated by PUT";
            if (readitem.Tags == null)
                readitem.Tags = new List<KnowledgeTag>();
            readitem.Tags.Add(new KnowledgeTag
            {
                RefID = firstid,
                TagTerm = DataSetupUtility.Tag1
            });
            var rstPut = await control.Put(firstid, readitem);
            Assert.NotNull(rstPut);
            // Due to the fact that updated result is empty
            // Check the result in database directly
            var dbkis = context.KnowledgeItems.Where(p => p.ID == firstid).ToList<KnowledgeItem>();
            Assert.NotEmpty(dbkis);
            Assert.Equal(readitem.Content, dbkis[0].Content); // Check content only!

            // Check the tags view
            var tagcontrol = new KnowledgeTagsController(context);
            var tagsread = tagcontrol.Get();
            Assert.NotNull(tagsread);
            var tagsreadresult = tagsread.ToList();
            var findtag = tagsreadresult.Find(p => p.RefID == firstid && p.TagTerm == DataSetupUtility.Tag1);
            Assert.NotNull(findtag);

            // Step 5. PATCH .
            var delta = new Delta<KnowledgeItem>();
            delta.UpdatableProperties.Clear();
            delta.UpdatableProperties.Add("Content");
            readitem.Content += "Changed for PATCH";
            delta.TrySetPropertyValue("Content", readitem.Content);
            var patchresult = await control.Patch(firstid, delta);
            Assert.NotNull(patchresult);
            dbkis = context.KnowledgeItems.Where(p => p.ID == firstid).ToList<KnowledgeItem>();
            Assert.NotEmpty(dbkis);
            Assert.Equal(readitem.Content, dbkis[0].Content); // Check content only!

            // Step 5. Delete
            rst = await control.Delete(firstid);
            Assert.NotNull(rst);
            this.objectsCreated.Remove(firstid);

            rsts = control.Get();
            rstscnt = await rsts.CountAsync();
            Assert.Equal(existcnt, rstscnt);

            await context.DisposeAsync();
        }

        [Fact]
        public async Task TestCase_InvalidPost()
        {
            var context = fixture.GetCurrentDataContext();
            KnowledgeItemsController control = new KnowledgeItemsController(context);
            control.ModelState.AddModelError("Name", "The Name field is required.");

            var postrst = control.Post(new KnowledgeItem { ID = 1, });
            Assert.NotNull(postrst);
            Assert.IsType<BadRequestObjectResult>(postrst.Result);

            await context.DisposeAsync();
        }

        [Fact]
        public async Task TestCase_PutWithInvalidModelState()
        {
            var context = fixture.GetCurrentDataContext();
            KnowledgeItemsController control = new KnowledgeItemsController(context);
            control.ModelState.AddModelError("Name", "The Name field is required.");

            var putrst = control.Put(999, null);
            Assert.NotNull(putrst);
            Assert.IsType<BadRequestObjectResult>(putrst.Result);

            await context.DisposeAsync();
        }

        [Fact]
        public async Task TestCase_PutWithInvalidID()
        {
            var context = fixture.GetCurrentDataContext();
            KnowledgeItemsController control = new KnowledgeItemsController(context);

            var putrst = control.Put(999, new KnowledgeItem { ID = 1});
            Assert.NotNull(putrst);
            Assert.IsType<NotFoundResult>(putrst.Result);

            await context.DisposeAsync();
        }

        [Fact]
        public async Task TestCase_PutWithNonExistID()
        {
            var context = fixture.GetCurrentDataContext();
            KnowledgeItemsController control = new KnowledgeItemsController(context);
            var userclaim = DataSetupUtility.GetClaimForUser(DataSetupUtility.UserA);
            control.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = userclaim }
            };

            var putrst = control.Put(999, new KnowledgeItem { ID = 999 });
            Assert.NotNull(putrst);
            Assert.IsType<NotFoundResult>(putrst.Result);

            await context.DisposeAsync();
        }

        [Fact]
        public async Task TestCase_DeleteWithNonExistID()
        {
            var context = fixture.GetCurrentDataContext();
            KnowledgeItemsController control = new KnowledgeItemsController(context);

            var deleterst = control.Delete(999);
            Assert.NotNull(deleterst);
            Assert.IsType<NotFoundResult>(deleterst.Result);

            await context.DisposeAsync();
        }

        [Fact]
        public async Task TestCase_PatchWithInvalidModelState()
        {
            var context = fixture.GetCurrentDataContext();
            KnowledgeItemsController control = new KnowledgeItemsController(context);
            control.ModelState.AddModelError("Name", "The Name field is required.");

            var putrst = control.Patch(999, null);
            Assert.NotNull(putrst);
            Assert.IsType<BadRequestObjectResult>(putrst.Result);

            await context.DisposeAsync();
        }

        [Fact]
        public async Task TestCase_PatchWithInvalidID()
        {
            var context = fixture.GetCurrentDataContext();
            KnowledgeItemsController control = new KnowledgeItemsController(context);

            var putrst = control.Patch(999, new Delta<KnowledgeItem> { });
            Assert.NotNull(putrst);
            Assert.IsType<NotFoundResult>(putrst.Result);

            await context.DisposeAsync();
        }

        private void CleanupCreatedEntries()
        {
            if (objectsCreated.Count > 0)
            {
                var context = this.fixture.GetCurrentDataContext();
                foreach (var kid in objectsCreated)
                    DataSetupUtility.DeleteKnowledgeItem(context, kid);

                objectsCreated.Clear();
                context.SaveChanges();
            }
        }
    }
}
