using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using listing.api.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace listing.api {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddControllers ();

            services.AddEntityFrameworkNpgsql ();
            services.AddDbContext<ListingApiDbContext> (options =>
                options.UseNpgsql (Environment.GetEnvironmentVariable ("DbConnectionString")));

            services.AddSwaggerGen (c => {
                c.SwaggerDoc ("v1", new OpenApiInfo {
                    Version = "v1",
                        Title = "Listing Api",
                        Description = "Api to get listing details ",
                });
            });

            services.AddCors (options => {
                options.AddPolicy ("AllowAllOrigins",
                    builder => {
                        builder.AllowAnyOrigin ()
                            .AllowAnyMethod ()
                            .AllowAnyHeader ();
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseHttpsRedirection ();

            app.UseRouting ();

            app.UseAuthorization ();

            app.UseSwagger ();
            app.UseSwaggerUI (c => {
                c.SwaggerEndpoint ("/swagger/v1/swagger.json", "Listing API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseCors ("AllowAllOrigins");

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
        }
    }
}