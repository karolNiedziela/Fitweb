using Backend.Infrastructure.CommandHandler.Commands;
using Backend.Infrastructure.CommandHandler.Commands.Users;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandHandler.Handlers.Users
{
    public class UpdateUserHandler : ICommandHandler<UpdateUser>
    {
        private readonly IUserService _userService;

        public UpdateUserHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task HandleAsync(UpdateUser command)
        {
            await _userService.UpdateAsync(command.Id, command.Username, command.Email, command.Password);
        }
    }
}
