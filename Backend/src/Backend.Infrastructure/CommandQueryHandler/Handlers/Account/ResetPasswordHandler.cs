using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.Services.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers
{
    public class ResetPasswordHandler : ICommandHandler<ResetPassword>
    {
        private readonly IAccountService _accountService;

        public ResetPasswordHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }
        
        public async Task HandleAsync(ResetPassword command)
        {
            await _accountService.ResetPasswordAsync(command.UserId, command.Code, command.NewPassword);
        }
    }
}
