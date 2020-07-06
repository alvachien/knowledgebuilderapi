﻿using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace knowledgebuilderapi.test
{
    public class SqliteDatabaseFixture : IDisposable
    {
        public SqliteDatabaseFixture()
        {
            // Open connections
            DBConnection = new SqliteConnection("DataSource=:memory:");
            DBConnection.Open();

            try
            {
                // Create the schema in the database
                var context = GetCurrentDataContext();
                if (!context.Database.IsSqlite()
                    || context.Database.IsSqlServer())
                {
                    throw new Exception("Faield!");
                }

                // Create tables and views
                DataSetupUtility.CreateDatabaseTables(context.Database);
                DataSetupUtility.CreateDatabaseViews(context.Database);

                context.Database.EnsureCreated();

                // Setup the tables
                // Create initial values

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

        public void Dispose()
        {
            if (DBConnection != null)
            {
                DBConnection.Close();
                DBConnection = null;
            }
        }

        protected SqliteConnection DBConnection { get; private set; }

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

        internal void DeleteKnowledgeItem(kbdataContext context, int kid)
        {
            context.Database.ExecuteSqlRaw("DELETE FROM KnowledgeItem WHERE ID = " + kid.ToString());

        }
    }
}
