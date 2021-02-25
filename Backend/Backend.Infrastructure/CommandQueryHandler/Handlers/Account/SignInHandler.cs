using Backend.Core.Factories;
using Backend.Core.Repositories;
using Backend.Infrastructure.Auth;
using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Services;
using Backend.Infrastructure.Services.Account;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers
{
    public class SigInHandler : ICommandHandler<SigIn>
    {
        private readonly IAccountService _accountService;
        private readonly IMemoryCache _cache;

        public SigInHandler(IAccountService accountService, IMemoryCache cache)
        {
            _accountService = accountService;
            _cache = cache;
        }
        public async Task HandleAsync(SigIn command)
        {
           var jwt = await _accountService.SignInAsync(command.Username, command.Password);
            _cache.SetJwt(command.TokenId, jwt);
        }
    }
}
