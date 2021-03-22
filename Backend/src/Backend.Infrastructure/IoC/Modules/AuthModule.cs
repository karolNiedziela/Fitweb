using Autofac;
using Backend.Core.Factories;
using Backend.Core.Services;
using Backend.Infrastructure.Auth;
using Microsoft.AspNetCore.Identity;

namespace Backend.Infrastructure.IoC.Modules
{
    public class AuthModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
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

            builder.RegisterType<EmailService>()
                   .As<IEmailService>()
                   .InstancePerDependency();
        }
    }
}
