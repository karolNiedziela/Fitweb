using Backend.Infrastructure.Commands;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{

    public class ApiControllerBase : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        protected int UserId => HttpContext.User.Identity is ClaimsIdentity identity ?
            Int32.Parse(identity.FindFirst("sub").Value) : Int32.Parse(null);
            
        protected ApiControllerBase(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        protected async Task DispatchAsync<T>(T command ) where T : ICommand
        {
            if(command is IAuthenticatedCommand authenticatedCommand)
            {
                authenticatedCommand.UserId = UserId;
            }

            await _commandDispatcher.DispatchAsync(command);
        }
    }
}
