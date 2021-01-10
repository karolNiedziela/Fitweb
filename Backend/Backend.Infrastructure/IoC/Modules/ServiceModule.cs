using Autofac;
using Backend.Core.Factories;
using Backend.Infrastructure.Services;
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

            builder.RegisterAssemblyTypes(assembly)
                   .Where(t => t.Name.EndsWith("Provider"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

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

            builder.RegisterAssemblyTypes(assembly)
                   .Where(t => t.Name.EndsWith("Initializer"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();


            builder.RegisterGeneric(typeof(CsvLoader<,>))
                   .As(typeof(ICsvLoader<,>))
                   .InstancePerLifetimeScope();
        }
    }
}
