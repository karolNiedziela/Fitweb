using Backend.Infrastructure.Commands;
using Backend.Infrastructure.Commands.Account;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Handlers.Account
{
    public class EditProfileHandler : ICommandHandler<EditProfile>
    {
        private readonly IUserAccountService _userAccountService;

        public EditProfileHandler(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        public async Task HandleAsync(EditProfile command)
        {
            await _userAccountService.EditProfile(command.UserId, command.Username, command.Email);
        }
    }
}
