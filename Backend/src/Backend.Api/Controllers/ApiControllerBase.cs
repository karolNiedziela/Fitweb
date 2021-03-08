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
        private readonly IDispatcher _dispatcher;


        private int? _userId;

        protected int UserId
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    _userId = int.Parse(User.Identity.Name ?? null);
                }

                return Convert.ToInt32(_userId);
            }
        }

        protected ApiControllerBase(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        protected async Task DispatchAsync<T>(T command) where T : ICommand
        {

            await _dispatcher.DispatchAsync(command);
        }

        protected async Task<T> QueryAsync<T>(IQuery<T> query)
        {
            return await _dispatcher.QueryAsync(query);
        }
    }
}
