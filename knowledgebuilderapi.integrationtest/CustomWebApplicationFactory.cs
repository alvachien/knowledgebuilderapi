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

namespace knowledgebuilderapi.test.integrationtest
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("IntegrationTest");

            builder.ConfigureServices(services =>
            {
                // In-memory database only exists while the connection is open
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                // // Identity
                // foreach(var srv in services)
                // {
                //     System.Diagnostics.Debug.WriteLine("===");
                //     System.Diagnostics.Debug.WriteLine(srv.ServiceType);
                //     System.Diagnostics.Debug.WriteLine(srv.ImplementationType);
                // }

                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(Microsoft.AspNetCore.Authentication.AuthenticationOptions));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add the Jwt bear back
                services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", options =>
                    {
                        options.Authority = "http://localhost:5005";
                        options.RequireHttpsMetadata = false;

                        options.Audience = "knowledgebuilder.api";
                    });

                // Remove the app's ApplicationDbContext registration.
                descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<kbdataContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add ApplicationDbContext using an in-memory database for testing.
                services.AddDbContext<kbdataContext>((options, context) =>
                    {
                        context.UseSqlite(connection);
                    });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (kbdataContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<kbdataContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    // Ensure the database is created.
                    db.Database.EnsureCreated();

                    try
                    {
                        // Seed the database with test data.
                        Utilities.InitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
}
