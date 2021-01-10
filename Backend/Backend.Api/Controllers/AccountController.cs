using Backend.Infrastructure.CommandHandler.Commands;
using Backend.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class AccountController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(ICommandDispatcher commandDispatcher, IUserService userService) 
            : base(commandDispatcher)
        {
            _userService = userService;
        }

        [HttpGet]
        //GET : /api/account
        public async Task<Object> GetUserAccount()
        { 
            var user = await _userService.GetAsync(UserId);

            return Json(user);
        }
    }
}
