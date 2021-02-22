using Autofac;
using Backend.Infrastructure.CommandQueryHandler;
using Backend.Infrastructure.CommandQueryHandler.Commands;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Backend.Infrastructure.IoC.Modules
{
    public class CommandModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(typeof(ICommand).Assembly)
                   .AsClosedTypesOf(typeof(ICommandHandler<>))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(IQuery<>).Assembly)
                   .AsClosedTypesOf(typeof(IQueryHandler<,>))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType<Dispatcher>()
                    .As<IDispatcher>()
                    .InstancePerLifetimeScope();

        }
    }
}
