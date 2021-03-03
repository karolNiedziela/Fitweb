using Backend.Infrastructure.Auth;
using Backend.Infrastructure.CommandQueryHandler.Queries;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers
{
    public class ConfirmEmailHandler : IQueryHandler<ConfirmEmail, IdentityResult>
    {
        private readonly IEmailService _emailService;

        public ConfirmEmailHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<IdentityResult> HandleAsync(ConfirmEmail query)
        {
            return await _emailService.ConfirmEmailAsync(Convert.ToInt32(query.UId), query.Code);
        }
    }
}
