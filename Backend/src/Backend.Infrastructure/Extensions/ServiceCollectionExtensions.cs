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

            services.AddMemoryCache();
            var sqlSettings = configuration.GetSettings<SqlSettings>();
            services.AddDbContext<FitwebContext>(options =>
            {
                options.UseSqlServer(sqlSettings.ConnectionString);
            });
            services.AddJwt();
            services.AddHttpClient();

            return services;
        }
    }
}
