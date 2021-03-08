using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.Services.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers
{
    public class ForgotPasswordHandler : ICommandHandler<ForgotPassword>
    {
        private readonly IAccountService _accountService;

        public ForgotPasswordHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task HandleAsync(ForgotPassword command)
        {
            await _accountService.GenerateForgotPasswordTokenAsync(command.Email);
        }
    }
}
