using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.Services.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers
{
    public class ChangePasswordHandler : ICommandHandler<ChangePassword>
    {
        private readonly IAccountService _accountService;

        public ChangePasswordHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task HandleAsync(ChangePassword command)
        {
            await _accountService.ChangePasswordAsync(command.UserId, command.OldPassword, command.NewPassword);
        }
    }
}
