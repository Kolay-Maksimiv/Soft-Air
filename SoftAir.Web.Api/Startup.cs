using AutoMapper;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SoftAir.Data;
using SoftAir.Data.Extensions;
using SoftAir.Data.Repositories;
using SoftAir.Data.Repositories.Interfaces;
using SoftAir.Services;
using SoftAir.Services.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SoftAir.Web.Api
{
    /// <summary>
    /// Startup project
    /// </summary>
    public class Startup
    {
        private readonly string CorsPolicyName = "SoftAirCorsPolicy";
        /// <summary>
        /// Configuration startup project
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Inject project configuration
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Database Context
            services.AddDatabase(Configuration);

            services.AddScoped<IAircraftRepository, AircraftRepository>();

            services.AddScoped<IAircraftService, AircraftService>();

            services.AddControllers();

            // Add auto mapper 
            var assemblies = Assembly
                .GetEntryAssembly()
                ?.GetReferencedAssemblies()
                .Select(Assembly.Load).ToList();

            assemblies.Add(Assembly.GetExecutingAssembly());

            try
            {
                services.AddAutoMapper(assemblies);

                //The code that causes the error goes here.
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null)
                    {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                string errorMessage = sb.ToString();
                Console.WriteLine(errorMessage);
                //Display or log the error based on your application.
            }

            var allowedOrigins = new string[] {
                "http://localhost:51929"
            };

            services.AddSingleton<ICorsPolicyService>((container) =>
            {
                var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();
                return new DefaultCorsPolicyService(logger)
                {
                    AllowedOrigins = allowedOrigins
                };
            });
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName, builder =>
                {
                    builder.WithOrigins(allowedOrigins)
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SoftAir.Web.Api", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SoftAir.Web.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(CorsPolicyName);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
