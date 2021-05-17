using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Backend.Infrastructure.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Backend.Infrastructure.Settings;
using Microsoft.Extensions.Logging;

namespace Backend.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            IConfiguration configuration;

            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            services.AddDbContext<FitwebContext>(options =>
            {
                //options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddMemoryCache();
            services.AddAuth();
            services.AddHttpClient();

            return services;
        }
    }
}
