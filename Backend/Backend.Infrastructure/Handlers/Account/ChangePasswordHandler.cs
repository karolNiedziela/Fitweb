using Backend.Infrastructure.Commands;
using Backend.Infrastructure.Commands.Account;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Handlers.Account
{
    public class ChangePasswordHandler : ICommandHandler<ChangePassword>
    {
        private readonly IUserAccountService _userAccountService;

        public ChangePasswordHandler(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        public async Task HandleAsync(ChangePassword command)
        {
            await _userAccountService.ChangePasswordAsync(command.UserId, command.NewPassword); 
        }
    }
}
