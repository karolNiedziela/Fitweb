﻿using Autofac;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.IoC.Modules
{
    public class SettingsModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;

        public SettingsModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_configuration.GetSettings<GeneralSettings>())
                   .SingleInstance();
            builder.RegisterInstance(_configuration.GetSettings<SqlSettings>())
                   .SingleInstance();
            builder.RegisterInstance(_configuration.GetSettings<JwtSettings>())
                   .SingleInstance();
            builder.RegisterInstance(_configuration.GetSettings<FacebookAuthSettings>())
                   .SingleInstance();
        }
    }
}
