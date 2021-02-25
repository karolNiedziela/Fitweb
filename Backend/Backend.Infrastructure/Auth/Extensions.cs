using Autofac;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Backend.Infrastructure.Auth
{
    public static class Extensions
    {
        public static IServiceCollection AddJwt(this IServiceCollection services)
        {
            IConfiguration configuration;

            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

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
            }).AddJwtBearer(opt =>
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

            return services;
        }
    }
}
