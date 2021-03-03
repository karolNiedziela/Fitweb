using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Settings
{
    public class GeneralSettings
    {
        public string Name { get; set; }

        public string AppDomain { get; set; }

        public string EmailConfirmation { get; set; }

        public string ClientURL { get; set; }

        public bool SeedData { get; set; }
    }
}
