using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.Services;
using Backend.Infrastructure.Services.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers
{
    public class SignUpHandler : ICommandHandler<SignUp>
    {
        private readonly IAccountService _accountService;

        public SignUpHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task HandleAsync(SignUp command)
        {
            command.Id = await _accountService.SignUpAsync(command.Username, command.Email, command.Password, command.Role);
        }
    }
}
