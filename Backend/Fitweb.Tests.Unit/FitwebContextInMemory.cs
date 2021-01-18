using Backend.Infrastructure.EF;
using Backend.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitweb.Tests.Unit
{
    public class FitwebContextInMemory
    {
        private readonly SqlSettings _sqlSettings;
        public readonly FitwebContext _context;
        private DbContextOptions<FitwebContext> _options;

        public FitwebContextInMemory()
        {
            _sqlSettings = Substitute.For<SqlSettings>();
            _sqlSettings.InMemory = true;
            _options = new DbContextOptionsBuilder<FitwebContext>().Options;
            _context = new FitwebContext(_options, _sqlSettings);
        }
    }
}
