using Autofac;
using Backend.Api.Framework;
using Backend.Infrastructure.EF;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.IoC;
using Backend.Infrastructure.Services;
using Backend.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddUserSecrets<Startup>()
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = true;
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;                 
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                });

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddInfrastructure();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fitweb API", Version = "v1" });
                c.DescribeAllParametersInCamelCase();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Authorization with JSON Web Token Example: Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
               {
                 new OpenApiSecurityScheme
                 {
                   Reference = new OpenApiReference
                   {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                   }
                  },
                  new string[] { }
                }
              });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("FitwebOrigin", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ContainerModule(Configuration));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fitweb API v1"));
            }

            app.UseCors("FitwebOrigin");


            app.UseMyExceptionHandler();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseDefaultFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            var generalSettings = app.ApplicationServices.GetService<GeneralSettings>();
            if (generalSettings.SeedData)
            {
                var dataInitializer = app.ApplicationServices.GetService<IGlobalSeeder>();
                dataInitializer.SeedAsync();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
