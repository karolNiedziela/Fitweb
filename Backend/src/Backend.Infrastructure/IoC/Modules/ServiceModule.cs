using Autofac;
using Backend.Infrastructure.Services;
using Backend.Infrastructure.Services.External;
using Backend.Infrastructure.Services.Logger;
using Backend.Infrastructure.Utilities.Csv;
using System.Reflection;

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

            builder.RegisterType<LoggerManager>()
                   .As<ILoggerManager>()
                   .SingleInstance();

            builder.RegisterAssemblyTypes(assembly)
                   .Where(t => t.Name.EndsWith("Initializer"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType(typeof(DataInitializer))
                   .As<IDataInitializer>()
                   .SingleInstance();

            builder.RegisterGeneric(typeof(CsvLoader<,>))
                   .As(typeof(ICsvLoader<,>))
                   .InstancePerLifetimeScope();

            builder.RegisterType<FacebookAuthService>()
                   .As<IFacebookAuthService>()
                   .SingleInstance();
        }
    }
}
