using Backend.Infrastructure.CommandQueryHandler.Queries;
using Backend.Infrastructure.Services.Account;
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
        private readonly IAccountService _accountService;

        public ConfirmEmailHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IdentityResult> HandleAsync(ConfirmEmail query)
        {
            return await _accountService.ConfirmEmailAsync(Convert.ToInt32(query.UId), query.Code);
        }
    }
}
