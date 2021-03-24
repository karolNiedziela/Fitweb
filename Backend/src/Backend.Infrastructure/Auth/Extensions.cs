using Autofac;
using Backend.Core.Entities;
using Backend.Infrastructure.EF;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System;
using System.Text;
using Microsoft.Extensions.Options;
using Backend.Core.Enums;

namespace Backend.Infrastructure.Auth
{
    public static class Extensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection services)
        {
            IConfiguration configuration;

            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            services.AddIdentity<User, Role>(
                options => options.SignIn.RequireConfirmedAccount = false
                )
                .AddEntityFrameworkStores<FitwebContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {

                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 1;

                // SigIn settings.
                options.SignIn.RequireConfirmedEmail = true;                

                // User settings.
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnoprstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            });

            var jwt = configuration.GetSettings<JwtSettings>();

            var tokenValidationParameters = new TokenValidationParameters
            {
                RequireAudience = jwt.RequireAudience,
                ValidIssuer = jwt.ValidIssuer,
                ValidIssuers = jwt.ValidIssuers,
                ValidateActor = jwt.ValidateActor,
                ValidAudience = jwt.ValidAudience,
                ValidAudiences = jwt.ValidAudiences,
                ValidateAudience = jwt.ValidateAudience,
                ValidateIssuer = jwt.ValidateIssuer,
                ValidateLifetime = jwt.ValidateLifetime,
                ValidateTokenReplay = jwt.ValidateTokenReplay,
                ValidateIssuerSigningKey = jwt.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwt.IssuerSigningKey)),
                SaveSigninToken = jwt.SaveSigninToken,
                RequireExpirationTime = jwt.RequireExpirationTime,
                RequireSignedTokens = jwt.RequireSignedTokens,
                ClockSkew = TimeSpan.Zero
            };        

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.Authority = jwt.Authority;
                opt.Audience = jwt.Audience;
                opt.MetadataAddress = jwt.MetadataAddress;
                opt.SaveToken = jwt.SaveToken;
                opt.RefreshOnIssuerKeyNotFound = jwt.RefreshOnIssuerKeyNotFound;
                opt.RequireHttpsMetadata = jwt.RequireHttpsMetadata;
                opt.IncludeErrorDetails = jwt.IncludeErrorDetails;
                opt.TokenValidationParameters = tokenValidationParameters;
                if (!string.IsNullOrWhiteSpace(jwt.Challenge))
                {
                    opt.Challenge = jwt.Challenge;
                }
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyNames.AdminOnly, policy => policy.RequireRole(RoleId.Admin.ToString()));
            });

            return services;
        }
    }
}
