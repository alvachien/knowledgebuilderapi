using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using knowledgebuilderapi.Models;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using knowledgebuilderapi.test.common;
using Microsoft.Extensions.Hosting;

namespace knowledgebuilderapi.test.integrationtest
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected SqliteConnection DBConnection { get; private set; }

        public CustomWebApplicationFactory()
        {
            DBConnection = new SqliteConnection("DataSource=:memory:");
            DBConnection.Open();

            try
            {
                // Create the schema in the database
                var context = GetCurrentDataContext();
                if (!context.Database.IsSqlite()
                    || context.Database.IsSqlServer())
                {
                    throw new Exception("Failed!");
                }

                // Create tables and views
                DataSetupUtility.CreateDatabaseTables(context.Database);
                DataSetupUtility.CreateDatabaseViews(context.Database);

                context.Database.EnsureCreated();

                // Setup the tables

                context.Dispose();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                // Error occurred
            }
            finally
            {
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (DBConnection != null)
            {
                DBConnection.Close();
                DBConnection = null;
            }
        }

        public kbdataContext GetCurrentDataContext()
        {
            var options = new DbContextOptionsBuilder<kbdataContext>()
                .UseSqlite(DBConnection, action =>
                {
                    action.UseRelationalNulls();
                })
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
                .EnableSensitiveDataLogging()
                .Options;

            var context = new kbdataContext(options, true);
            return context;
        }

        protected override IHostBuilder CreateHostBuilder() =>
            base.CreateHostBuilder();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("IntegrationTest");

            builder.ConfigureServices(services =>
            {
                //var descriptor = services.SingleOrDefault(
                //    d => d.ServiceType == typeof(Microsoft.AspNetCore.Authentication.AuthenticationOptions));
                //if (descriptor != null)
                //{
                //    services.Remove(descriptor);
                //}

                //// Add the Jwt bear back
                //services.AddAuthentication("Bearer")
                //    .AddJwtBearer("Bearer", options =>
                //    {
                //        options.Authority = "http://localhost:5005";
                //        options.RequireHttpsMetadata = false;

                //        options.Audience = "knowledgebuilder.api";
                //    });

                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<kbdataContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<kbdataContext>(options =>
                {
                    options.UseSqlite(DBConnection, action =>
                    {
                        action.UseRelationalNulls();
                    })
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
                    .EnableSensitiveDataLogging();
                });

                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<kbdataContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        // Utilities.InitializeDbForTests(db);
                    }
                    catch (Exception exp)
                    {

                        logger.LogError(exp, "An error occurred seeding the " +
                            "database with test messages. Error: {Message}", exp.Message);
                    }
                }
            });
        }
    }
}
