using Autofac;
using Backend.Infrastructure.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.IoC.Modules
{
    public class FrameworkModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ErrorHandlerMiddleware>().SingleInstance();

            builder.RegisterType<ExceptionToResponseMapper>()
                .As<IExceptionToResponseMapper>()
                .SingleInstance();
        }
    }
}
