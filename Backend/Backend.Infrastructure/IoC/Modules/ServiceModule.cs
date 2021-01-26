using Autofac;
using Backend.Core.Factories;
using Backend.Core.Services;
using Backend.Infrastructure.Auth;
using Backend.Infrastructure.Services;
using Backend.Infrastructure.Services.External;
using Backend.Infrastructure.Services.Logger;
using Backend.Infrastructure.Utilities.Csv;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Backend.Infrastructure.IoC.Modules
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly)
                   .Where(t => t.Name.EndsWith("Service"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType<DateTimeProvider>()
                   .As<IDateTimeProvider>()
                   .SingleInstance();

            builder.RegisterType<Rng>()
                   .As<IRng>()
                   .SingleInstance();

            builder.RegisterType<RefreshTokenFactory>()
                   .As<IRefreshTokenFactory>()
                   .SingleInstance();

            builder.RegisterType<JwtHandler>()
                   .As<IJwtHandler>()
                   .SingleInstance();

            builder.RegisterType<PasswordHandler>()
                   .As<IPasswordHandler>()
                   .SingleInstance();

            builder.RegisterType<PasswordHasher<IPasswordHandler>>()
                   .As<IPasswordHasher<IPasswordHandler>>()
                   .SingleInstance();

            builder.RegisterType<LoggerManager>()
                   .As<ILoggerManager>()
                   .SingleInstance();

            builder.RegisterAssemblyTypes(assembly)
                   .Where(t => t.Name.EndsWith("Initializer"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(CsvLoader<,>))
                   .As(typeof(ICsvLoader<,>))
                   .InstancePerLifetimeScope();

            builder.RegisterType<FacebookAuthService>()
                   .As<IFacebookAuthService>()
                   .SingleInstance();
        }
    }
}
