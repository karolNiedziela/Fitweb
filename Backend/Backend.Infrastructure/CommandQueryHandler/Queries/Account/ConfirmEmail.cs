using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Queries
{
    public class ConfirmEmail : IQuery<IdentityResult>
    {
        public int? UId { get; set; }

        public string Code { get; set; }

    }
}
