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

namespace knowledgebuilderapi.test.UnitTests
{
    public class KnowledgeItemsControllerTest
    {
        [Fact]
        public async Task Test_Read_Create_ReRead()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<kbdataContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new kbdataContext(options))
                {
                    context.Database.EnsureCreated();

                    KnowledgeItemsController control = new KnowledgeItemsController(context);
                    
                    // Step 1. Read - nothing
                    var rsts = control.Get();
                    var rstscnt = await rsts.CountAsync();
                    Assert.Equal(0, rstscnt);

                    // Step 2. Create a new one
                    var nmod = new KnowledgeItem() {
                        Title = "Test 1",
                        Category = KnowledgeItemCategory.Concept,
                        Content = "My test 1"
                    };
                    var result = control.Post(nmod);
                    var actionResult = Assert.IsType<Task<IActionResult>>(result);
                    var actResult = Assert.IsType<CreatedODataResult<KnowledgeItem>>(result.Result);
                    rstscnt = await context.KnowledgeItems.CountAsync();
                    Assert.Equal(1, rstscnt);
                    
                    var nid = actResult.Entity.ID;
                    var dbrst = await context.KnowledgeItems.SingleOrDefaultAsync(p => p.ID == nid);
                    Assert.Equal(dbrst.Title, nmod.Title);
                    Assert.Equal(dbrst.Content, nmod.Content);
                    Assert.Equal(dbrst.Category, nmod.Category);

                    // Step 3. Re-read
                    rsts = control.Get();
                    rstscnt = await rsts.CountAsync();
                    Assert.Equal(1, rstscnt);
                    var firstrst = rsts.ToList()[0];
                    Assert.Equal(firstrst.Title, nmod.Title);
                    Assert.Equal(firstrst.Content, nmod.Content);
                    Assert.Equal(firstrst.Category, nmod.Category);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
