using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Batch;
using knowledgebuilderapi.Models;
using Microsoft.AspNetCore.Routing;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;

namespace knowledgebuilderapi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;

            UploadFolder = Path.Combine(env.ContentRootPath, UploadFolderName);
            if (!Directory.Exists(UploadFolder))
            {
                Directory.CreateDirectory(UploadFolder);
            }
        }

        internal const string UploadFolderName = @"uploads";
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public string ConnectionString { get; private set; }
        internal static String UploadFolder { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            this.ConnectionString = Configuration["KBAPI.ConnectionString"];

            if (Environment.EnvironmentName != "IntegrationTest")
            {
                services.AddDbContext<kbdataContext>(options =>
                    options.UseSqlServer(this.ConnectionString));
            }

            services.AddAuthorization();

            if (Environment.EnvironmentName == "IntegrationTest")
            {
                // Already Converted in IntegrationTest part.
                // 
                // services.AddAuthentication("Bearer")
                //     .AddJwtBearer("Bearer", options =>
                //     {
                //         options.Authority = "http://localhost:5005";
                //         options.RequireHttpsMetadata = false;

                //         options.Audience = "knowledgebuilder.api";
                //     });
            }
            else if (Environment.EnvironmentName == "Development")
            {                
                services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", options =>
                    {
                        options.Authority = "http://localhost:5005";
                        options.RequireHttpsMetadata = false;

                        options.Audience = "knowledgebuilder.api";
                    });

                services.AddCors(options =>
                {
                    options.AddPolicy("TEST", builder =>
                    {
                        builder.WithOrigins(
                            "http://localhost:5005",
                            "https://localhost:5005"
                            )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
                });
            }
            else if (Environment.EnvironmentName == "Production")
            {
                services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", options =>
                    {
                        options.Authority = "http://localhost:5005";
                        options.RequireHttpsMetadata = false;

                        options.Audience = "knowledgebuilder.api";
                    });

                services.AddCors(options =>
                {
                    options.AddPolicy("TEST", builder =>
                    {
                        builder.WithOrigins(
                            "http://localhost:5005",
                            "https://localhost:5005"
                            )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
                });
            }

            services.AddOData();
            services.AddMvc(options => options.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("TEST");
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseAuthentication();

            ODataModelBuilder modelBuilder = new ODataConventionModelBuilder(app.ApplicationServices);
            modelBuilder.EntitySet<KnowledgeItem>("KnowledgeItems");
            modelBuilder.EntitySet<ExerciseItem>("ExerciseItems");
            modelBuilder.EntitySet<ExerciseItemAnswer>("ExerciseItemAnswers");
            modelBuilder.EntitySet<KnowledgeTag>("KnowledgeTags");
            modelBuilder.EntitySet<ExerciseTag>("ExerciseTags");
            modelBuilder.EntitySet<Tag>("Tags");
            modelBuilder.EntitySet<TagCount>("TagCounts");
            modelBuilder.EntitySet<TagCountByRefType>("TagCountByRefTypes");
            modelBuilder.EntitySet<OverviewInfo>("OverviewInfos");
            modelBuilder.Namespace = typeof(KnowledgeItem).Namespace;

            var model = modelBuilder.GetEdmModel();
            app.UseODataBatching();

            app.UseMvc(routeBuilder =>
                {
                    // and this line to enable OData query option, for example $filter
                    routeBuilder.Select().Expand().Filter().OrderBy().MaxTop(100).Count();

                    routeBuilder.MapODataServiceRoute("ODataRoute", "odata", model);
                });

            var cachePeriod = "604800";
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(UploadFolder),
                RequestPath = "/" + UploadFolderName,
                OnPrepareResponse = ctx =>
                {
                    // Requires the following import:
                    // using Microsoft.AspNetCore.Http;
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}");
                }
            });
        }
    }
}
