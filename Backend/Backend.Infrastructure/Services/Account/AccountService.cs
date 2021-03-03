using Backend.Core.Entities;
using Backend.Core.Factories;
using Backend.Core.Repositories;
using Backend.Infrastructure.Auth;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHandler _passwordHandler;
        private readonly IJwtHandler _jwtHandler;
        private readonly IRefreshTokenFactory _refreshTokenFactory;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailSender;

        public AccountService(IUserRepository userRepository, IPasswordHandler passwordHandler,
            IJwtHandler jwtHandler, IRefreshTokenFactory refreshTokenFactory, IRefreshTokenRepository refreshTokenRepository,
            UserManager<User> userManager, IConfiguration configuration, IEmailService emailSender,
            RoleManager<Role> roleManager)
        {
            _userRepository = userRepository;
            _passwordHandler = passwordHandler;
            _jwtHandler = jwtHandler;
            _refreshTokenFactory = refreshTokenFactory;
            _refreshTokenRepository = refreshTokenRepository;
            _configuration = configuration;
            _userManager = userManager;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        public async Task<int> SignUpAsync(string username, string email, string password, string role = "User")
        {
            var user = new User(username, email, password);

            var roleFound = await _roleManager.FindByNameAsync(role);

            user.UserRoles.Add(new UserRole
            {
                User = user,
                RoleId = roleFound.Id
            });

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new ServiceException(result.Errors.FirstOrDefault().Code, result.Errors.FirstOrDefault().Description);
            }

            await GenerateEmailConfirmationTokenAsync(user);

            return user.Id;
        }

        public async Task GenerateEmailConfirmationTokenAsync(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            if (string.IsNullOrEmpty(token))
            {
                throw new ServiceException(ErrorCodes.InvalidValue, "Invalid email confirmation token.");
            }

            await SendConfirmationEmail(user, token);
        }

        public async Task SendConfirmationEmail(User user, string token)
        {
            var generalSettings = _configuration.GetSettings<GeneralSettings>();

            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var url = string.Format(generalSettings.AppDomain + generalSettings.EmailConfirmation, user.Id, code);

            await _emailSender.SendEmailAsync(user.Email, "Email Confirmation", $"Please confirm your account by" +
                $" <a href='{HtmlEncoder.Default.Encode(url)}'> clicking here </a>.");
        }

        public async Task<JwtDto> SignInAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                throw new ServiceException(ErrorCodes.InvalidCredentials, "Invalid credentials");
            }

            var isPasswordValid = _passwordHandler.IsValid(user.PasswordHash, password);
            if (!isPasswordValid)
            {
                throw new ServiceException(ErrorCodes.InvalidCredentials, "Invalid credentials");
            }

            if (!user.EmailConfirmed)
            {
                throw new ServiceException(ErrorCodes.InvalidValue, "Email not confirmed. Confirm email to get access.");
            }

            var jwt = _jwtHandler.CreateToken(user.Id, user.UserName, 
                user.UserRoles.Select(ur => ur.Role.Name.ToString()).FirstOrDefault());
            jwt.RefreshToken = await CreateRefreshTokenAsync(user.Id);

            return jwt;
        }

        public async Task ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetOrFailAsync(userId);
            if (user == null)
            {
                throw new ServiceException(ErrorCodes.UserNotFound, $"User with id: '{userId}' was not found.");
            }

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            if (!result.Succeeded)
            {
                throw new ServiceException(result.Errors.FirstOrDefault().Code, result.Errors.FirstOrDefault().Description);
            }
        }

        private async Task<string> CreateRefreshTokenAsync(int userId)
        {
            var refreshToken = _refreshTokenFactory.Create(userId);
            await _refreshTokenRepository.AddAsync(refreshToken);

            return refreshToken.Token;
        }
    }
}
