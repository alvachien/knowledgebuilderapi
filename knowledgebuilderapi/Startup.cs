using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Batch;
using knowledgebuilderapi.Models;
using Microsoft.AspNetCore.Routing;

namespace knowledgebuilderapi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public string ConnectionString { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            this.ConnectionString = Configuration["KBAPI.ConnectionString"];

            services.AddDbContext<kbdataContext>(options =>
                options.UseSqlServer(this.ConnectionString));

            // services.AddRouting();

            services.AddMvc(option => {
                option.EnableEndpointRouting = false;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddOData();

            // services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();
            // app.UseRouting();
            // app.UseAuthorization();


            ODataModelBuilder modelBuilder = new ODataConventionModelBuilder(app.ApplicationServices);
            modelBuilder.EntitySet<Knowledge>("Knowledges");
            modelBuilder.Namespace = typeof(Knowledge).Namespace;

            var model = modelBuilder.GetEdmModel();
            app.UseODataBatching();

            app.UseMvc(routeBuilder =>
                {
                    // and this line to enable OData query option, for example $filter
                    routeBuilder.Select().Expand().Filter().OrderBy().MaxTop(100).Count();

                    routeBuilder.MapODataServiceRoute("ODataRoute", "odata", model);
                });
            // var routes = new RouteBuilder(app);
            // routes.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
            // routes.MapODataServiceRoute("odata", "odata", model, new DefaultODataBatchHandler());
            // app.UseRouter(routes.Build());

            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapControllers();
            // });
        }
    }
}
