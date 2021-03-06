﻿using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers
{
    public class DeleteUserHandler : ICommandHandler<DeleteUser>
    {
        private readonly IUserService _userService;

        public DeleteUserHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task HandleAsync(DeleteUser command)
        {
            await _userService.DeleteAsync(command.Id);
        }
    }
}
