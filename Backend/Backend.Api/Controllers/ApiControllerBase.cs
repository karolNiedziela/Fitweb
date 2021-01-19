using Backend.Infrastructure.CommandQueryHandler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    public class ApiControllerBase : Controller
    {
        protected readonly IDispatcher _dispatcher;

        protected int UserId => HttpContext.User.Identity is ClaimsIdentity identity ?
            Int32.Parse(identity.FindFirst("sub").Value) : Int32.Parse(null);
            
        protected ApiControllerBase(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        protected async Task DispatchAsync<T>(T command) where T : ICommand
        {

            await _dispatcher.DispatchAsync(command);
        }

        protected async Task<T> QueryAsync<T>(T query) where T : IQuery<T>
        {
            return await _dispatcher.QueryAsync(query);
        }
    }
}
