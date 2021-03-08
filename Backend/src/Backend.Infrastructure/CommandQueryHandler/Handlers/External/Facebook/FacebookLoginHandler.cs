using Backend.Core.Factories;
using Backend.Core.Repositories;
using Backend.Infrastructure.Auth;
using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Services;
using Backend.Infrastructure.Services.External;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers
{
    public class FacebookLoginHandler : ICommandHandler<FacebookLogin>
    {
        private readonly IMemoryCache _cache;
        private readonly IExternalLoginService _externalLoginService;

        public FacebookLoginHandler(IMemoryCache cache, IExternalLoginService externalLoginService)
        {
            _cache = cache;
            _externalLoginService = externalLoginService;
        }

        public async Task HandleAsync(FacebookLogin command)
        {
            var jwt = await _externalLoginService.LoginWithFacebookAsync(command.AccessToken);
            _cache.SetJwt(command.TokenId, jwt);
        }

       
    }
}
