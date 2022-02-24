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
    public class ExerciseItemsControllerTest : IDisposable
    {
        SqliteDatabaseFixture fixture = null;
        private List<Int32> objectsCreated = new List<Int32>();

        public ExerciseItemsControllerTest(SqliteDatabaseFixture fixture)
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
                    DataSetupUtility.DeleteExerciseItem(context, kid);

                objectsCreated.Clear();
                context.SaveChanges();
            }
        }

        [Fact]
        public async Task TestCase_CRUD()
        {
            var context = fixture.GetCurrentDataContext();
            ExerciseItemsController control = new ExerciseItemsController(context);

            // Step 1. Read all
            var rsts = control.Get();
            var rstscnt = await rsts.CountAsync();
            var existcnt = context.ExerciseItems.Count();
            Assert.Equal(existcnt, rstscnt);

            // Step 2. Create one know ledge item            
            var ki = new ExerciseItem()
            {
                ExerciseType = ExerciseItemType.Question,
                Content = "New Test 1 Content",
            };
            ki.Answer = new ExerciseItemAnswer
            {
                Content = "New Answer"
            };
            ki.Tags.Add(new ExerciseTag()
            {
                TagTerm = DataSetupUtility.Tag1,
            });
            var rst = await control.Post(ki);
            Assert.NotNull(rst);
            var rst2 = Assert.IsType<CreatedODataResult<ExerciseItem>>(rst);
            Assert.Equal(rst2.Entity.Content, ki.Content);
            var firstid = rst2.Entity.ID;
            Assert.True(firstid > 0);
            objectsCreated.Add(firstid);
            // Check the answer
            var answerctrl = new ExerciseItemAnswersController(context);
            var answergetrst = answerctrl.Get();
            Assert.NotNull(answergetrst);
            // Check the tag
            var exertagctrl = new ExerciseTagsController(context);
            var exertaggetrst = exertagctrl.Get();
            Assert.NotNull(exertaggetrst);

            // Step 3. Read all - 1
            rsts = control.Get();
            rstscnt = await rsts.CountAsync();
            Assert.Equal(existcnt + 1, rstscnt);

            await context.DisposeAsync();
        }

        [Fact]
        public async Task TestCase_InvalidPost()
        {
            var context = fixture.GetCurrentDataContext();
            ExerciseItemsController control = new ExerciseItemsController(context);
            control.ModelState.AddModelError("Title", "The Title field is required.");

            var postrst = control.Post(new ExerciseItem { ID = 1, });
            Assert.NotNull(postrst);
            Assert.IsType<BadRequestObjectResult>(postrst.Result);

            await context.DisposeAsync();
        }

        [Fact]
        public async Task TestCase_PutWithInvalidModelState()
        {
            var context = fixture.GetCurrentDataContext();
            ExerciseItemsController control = new ExerciseItemsController(context);
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
            ExerciseItemsController control = new ExerciseItemsController(context);

            var putrst = control.Put(999, new ExerciseItem { ID = 1 });
            Assert.NotNull(putrst);
            Assert.IsType<BadRequestObjectResult>(putrst.Result);

            await context.DisposeAsync();
        }

        [Fact]
        public async Task TestCase_PutWithNonExistID()
        {
            var context = fixture.GetCurrentDataContext();
            ExerciseItemsController control = new ExerciseItemsController(context);

            var putrst = control.Put(999, new ExerciseItem { ID = 999 });
            Assert.NotNull(putrst);
            Assert.IsType<NotFoundResult>(putrst.Result);

            await context.DisposeAsync();
        }

        [Fact]
        public async Task TestCase_DeleteWithNonExistID()
        {
            var context = fixture.GetCurrentDataContext();
            ExerciseItemsController control = new ExerciseItemsController(context);

            var deleterst = control.Delete(999);
            Assert.NotNull(deleterst);
            Assert.IsType<NotFoundResult>(deleterst.Result);

            await context.DisposeAsync();
        }

        [Fact]
        public async Task TestCase_PatchWithInvalidModelState()
        {
            var context = fixture.GetCurrentDataContext();
            ExerciseItemsController control = new ExerciseItemsController(context);
            control.ModelState.AddModelError("Title", "The Title field is required.");

            var putrst = control.Patch(999, null);
            Assert.NotNull(putrst);
            Assert.IsType<BadRequestObjectResult>(putrst.Result);

            await context.DisposeAsync();
        }

        [Fact]
        public async Task TestCase_PatchWithInvalidID()
        {
            var context = fixture.GetCurrentDataContext();
            ExerciseItemsController control = new ExerciseItemsController(context);

            var putrst = control.Patch(999, new Delta<ExerciseItem> { });
            Assert.NotNull(putrst);
            Assert.IsType<NotFoundResult>(putrst.Result);

            await context.DisposeAsync();
        }
    }
}
