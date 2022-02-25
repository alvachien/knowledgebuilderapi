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
    public class UserCollectionsControllerTest: IDisposable
    {
        SqliteDatabaseFixture fixture = null;
        private List<Int32> objectsCreated = new List<Int32>();

        public UserCollectionsControllerTest(SqliteDatabaseFixture fixture)
        {
            this.fixture = fixture;
            this.fixture.InitializeTestData();
        }

        public void Dispose()
        {
            if (objectsCreated.Count > 0)
            {
                var context = this.fixture.GetCurrentDataContext();
                foreach (var kid in objectsCreated)
                    DataSetupUtility.DeleteUserCollection(context, kid);

                objectsCreated.Clear();
                context.SaveChanges();
            }
        }

        [Fact]
        public void TestCase_UnauthorizeRead()
        {
            var context = fixture.GetCurrentDataContext();

            var control = new UserCollectionsController(context);
            try
            {
                control.Get();
            }
            catch(Exception ex)
            {
                Assert.IsType<UnauthorizedAccessException>(ex);
            }

            try
            {
                control.Get(1);
            }
            catch (Exception ex)
            {
                Assert.IsType<UnauthorizedAccessException>(ex);
            }

            context.Dispose();
        }

        [Theory]
        [InlineData(DataSetupUtility.UserA)]
        [InlineData(DataSetupUtility.UserB)]
        public async Task TestCase_CURD(string usr)
        {
            var context = fixture.GetCurrentDataContext();
            UserCollectionsController control = new UserCollectionsController(context);
            var userclaim = DataSetupUtility.GetClaimForUser(usr);
            control.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = userclaim }
            };

            // Step 1. Get all
            var getallrst = control.Get();
            Assert.NotNull(getallrst);
            var collindb = context.UserCollections.AsNoTracking().Where(p => p.User == usr).ToList();
            Assert.Equal(collindb.Count, getallrst.Count());

            // Step 2. Create one
            var createdone = new UserCollection()
            {
                Name = "Test 1" + usr,
                Comment = "Test",
                User = usr,
            };
            var createditem = new UserCollectionItem()
            {
                RefType = TagRefType.KnowledgeItem,
                RefID = DataSetupUtility.Knowledge1ID
            };
            createdone.Items.Add(createditem);
            var postrst = await control.Post(createdone);
            Assert.NotNull(postrst);
            var postcreatedrst = Assert.IsType<CreatedODataResult<UserCollection>>(postrst);
            var crtid = postcreatedrst.Entity.ID;
            objectsCreated.Add(crtid);
            Assert.Equal(createdone.User, postcreatedrst.Entity.User);
            Assert.Equal(createdone.Name, postcreatedrst.Entity.Name);
            Assert.Equal(createdone.Comment, postcreatedrst.Entity.Comment);
            var collitemindb = context.UserCollectionItems.AsNoTracking().Where(p => p.ID == crtid).ToList();
            Assert.Single(collitemindb);
            Assert.Equal(createditem.RefType, collitemindb[0].RefType);
            Assert.Equal(createditem.RefID, collitemindb[0].RefID);

            // Step 3. Get single
            var getsinglerst = control.Get(crtid);
            Assert.NotNull(getsinglerst);
            var getrstresult = Assert.IsType<SingleResult<UserCollection>>(getsinglerst);
            var readitem = getrstresult.Queryable.ToList().ElementAtOrDefault(0);
            Assert.NotNull(readitem);
            Assert.Equal(crtid, readitem.ID);
            Assert.Equal(createdone.User, readitem.User);
            Assert.Equal(createdone.Name, readitem.Name);
            Assert.Equal(createdone.Comment, readitem.Comment);

            // Step 4. Change via PUT
            readitem.Comment += "Changed";
            readitem.Items.Clear();
            createditem = new UserCollectionItem()
            {
                RefID = DataSetupUtility.Exercise1ID,
                RefType = TagRefType.ExerciseItem,
                ID = crtid,
                Collection = readitem,
            };
            readitem.Items.Add(createditem);
            var putrst = await control.Put(crtid, readitem);
            Assert.NotNull(putrst);
            var putokrst = Assert.IsType<OkObjectResult>(putrst);
            Assert.NotNull(putokrst);
            var putokrstobj = Assert.IsType<UserCollection>(putokrst.Value);
            Assert.Equal(readitem.ID, putokrstobj.ID);
            Assert.Equal(readitem.User, putokrstobj.User);
            Assert.Equal(readitem.Name, putokrstobj.Name);
            Assert.Equal(readitem.Comment, putokrstobj.Comment);
            collindb = context.UserCollections.AsNoTracking().Where(p => p.User == usr).ToList();
            collitemindb = context.UserCollectionItems.AsNoTracking().Where(p => p.ID == crtid).ToList();
            Assert.Single(collindb);
            Assert.Single(collitemindb);
            Assert.Equal(readitem.Comment, collindb[0].Comment);
            Assert.Equal(createditem.RefID, collitemindb[0].RefID);
            Assert.Equal(createditem.RefType, collitemindb[0].RefType);

            // Step 5. Delete
            var delrst = await control.Delete(crtid);
            Assert.NotNull(delrst);
            var delrtn = Assert.IsType<StatusCodeResult>(delrst);
            objectsCreated.Remove(crtid);

            await context.DisposeAsync();
        }

        [Fact]
        public async Task TestCase_InvalidPost()
        {
            var context = fixture.GetCurrentDataContext();
            UserCollectionsController control = new UserCollectionsController(context);
            control.ModelState.AddModelError("Title", "The Title field is required.");

            var postrst = control.Post(new UserCollection { ID = 1, });
            Assert.NotNull(postrst);
            Assert.IsType<BadRequestObjectResult>(postrst.Result);

            await context.DisposeAsync();
        }

        [Fact]
        public async Task TestCase_PutWithInvalidModelState()
        {
            var context = fixture.GetCurrentDataContext();
            UserCollectionsController control = new UserCollectionsController(context);
            control.ModelState.AddModelError("Title", "The Title field is required.");

            var putrst = control.Put(999, null);
            Assert.NotNull(putrst);
            Assert.IsType<BadRequestObjectResult>(putrst.Result);

            await context.DisposeAsync();
        }

        [Fact]
        public async Task TestCase_PutWithInvalidID()
        {
            var context = fixture.GetCurrentDataContext();
            UserCollectionsController control = new UserCollectionsController(context);

            var putrst = control.Put(999, new UserCollection { ID = 1 });
            Assert.NotNull(putrst);
            Assert.IsType<BadRequestObjectResult>(putrst.Result);

            await context.DisposeAsync();
        }

        [Fact]
        public async Task TestCase_PutWithUnauthorized()
        {
            var context = fixture.GetCurrentDataContext();
            UserCollectionsController control = new UserCollectionsController(context);

            var putrst = control.Put(999, new UserCollection { ID = 999 });
            Assert.NotNull(putrst);
            Assert.IsType<UnauthorizedResult>(putrst.Result);

            await context.DisposeAsync();
        }

        [Fact]
        public async Task TestCase_DeleteWithNonExistID()
        {
            var context = fixture.GetCurrentDataContext();
            UserCollectionsController control = new UserCollectionsController(context);

            var deleterst = control.Delete(999);
            Assert.NotNull(deleterst);
            Assert.IsType<NotFoundResult>(deleterst.Result);

            await context.DisposeAsync();
        }
    }
}
