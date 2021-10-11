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
using Microsoft.AspNetCore.OData;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using Microsoft.OData.Edm;
using knowledgebuilderapi.Models;
using Microsoft.IdentityModel.Tokens;

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
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            if (Environment.EnvironmentName == "Development")
            {
                this.ConnectionString = Configuration["KBAPI.ConnectionString"];
                services.AddDbContext<kbdataContext>(options =>
                    options.UseSqlServer(this.ConnectionString));

                // TBD: Authentication
                services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", options =>
                    {
                        options.Authority = "https://localhost:44353";
                        options.RequireHttpsMetadata = true;
                        options.SaveToken = true;
                        options.IncludeErrorDetails = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false
                        };

                        options.Audience = "knowledgebuilder.api";
                    });

                services.AddCors(options =>
                {
                    options.AddPolicy(MyAllowSpecificOrigins, builder =>
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

                // Test
                var name = Configuration["UserMapping:Alva"];
                System.Diagnostics.Debug.Write(name);
            }
            else if (Environment.EnvironmentName == "Production")
            {
                this.ConnectionString = Configuration.GetConnectionString("AliyunConnection");
                services.AddDbContext<kbdataContext>(options => options.UseSqlServer(this.ConnectionString));

                // TBD: Authentication
                //services.AddAuthentication("Bearer")
                //    .AddJwtBearer("Bearer", options =>
                //    {
                //        options.Authority = "https://www.alvachien.com/idserver";
                //        options.RequireHttpsMetadata = true;
                //        options.SaveToken = true;
                //        options.IncludeErrorDetails = true;
                //        options.TokenValidationParameters = new TokenValidationParameters
                //        {
                //            ValidateAudience = false
                //        };

                //        options.Audience = "knowledgebuilder.api";
                //    });

                services.AddCors(options =>
                {
                    options.AddPolicy(MyAllowSpecificOrigins, builder =>
                    {
                        builder.WithOrigins(
                            "https://www.alvachien.com/math"
                            )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
                });
            }

            services.AddHttpContextAccessor();

            IEdmModel model = EdmModelBuilder.GetEdmModel();

            services.AddControllers().AddOData(opt => opt.Count().Filter().Expand().Select().OrderBy().SetMaxTop(100)
                .AddRouteComponents(model)
                .AddRouteComponents("v1", model)
                // .AddModel("v2{data}", model2, builder => builder.AddService<ODataBatchHandler, DefaultODataBatchHandler>(Microsoft.OData.ServiceLifetime.Singleton))
                // .ConfigureRoute(route => route.EnableQualifiedOperationCall = false) // use this to configure the built route template
                );

            // TBD: Authorization
            //services.AddAuthorization();            

            // Response Caching
            services.AddResponseCaching();
            // Memory cache
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseODataBatching();
            app.UseRouting();

            // TBD: Authentication, Authorization
            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseResponseCaching();

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
