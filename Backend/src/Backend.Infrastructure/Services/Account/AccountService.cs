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
using System;
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
        private readonly GeneralSettings _generalSettings;
        private readonly IEmailService _emailSender;

        public AccountService(IUserRepository userRepository, IPasswordHandler passwordHandler,
            IJwtHandler jwtHandler, IRefreshTokenFactory refreshTokenFactory, IRefreshTokenRepository refreshTokenRepository,
            UserManager<User> userManager, GeneralSettings generalSettings, IEmailService emailSender)
        {
            _userRepository = userRepository;
            _passwordHandler = passwordHandler;
            _jwtHandler = jwtHandler;
            _refreshTokenFactory = refreshTokenFactory;
            _refreshTokenRepository = refreshTokenRepository;
            _generalSettings = generalSettings;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<int> SignUpAsync(string username, string email, string password, string role = "User")
        {
            var user = new User(username, email, password);

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new IdentityResultException(result.Errors.FirstOrDefault().Description);
            }

            await _userManager.AddToRoleAsync(user, role);

            await GenerateEmailConfirmationTokenAsync(user);

            return user.Id;
        }

        public async Task<JwtDto> SignInAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user is null)
            {
                throw new InvalidCredentialsException();
            }

            var isPasswordValid = _passwordHandler.IsValid(user.PasswordHash, password);
            if (!isPasswordValid)
            {
                throw new InvalidCredentialsException();
            }

            if (_userManager.Options.SignIn.RequireConfirmedEmail)
            {
                if (!user.EmailConfirmed)
                {
                    throw new EmailNotConfirmedException();
                }
            }

            var jwt = _jwtHandler.CreateToken(user.Id, user.UserName,
                user.UserRoles.Select(ur => ur.Role.Name.ToString()).FirstOrDefault());
            jwt.RefreshToken = await CreateRefreshTokenAsync(user.Id);

            return jwt;
        }

        public async Task<IdentityResult> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetOrFailAsync(userId);

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            if (!result.Succeeded)
            {
                throw new IdentityResultException(result.Errors.FirstOrDefault().Description);
            }

            return result;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(int userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }

            var token = Decode(code);

            var result = await _userManager.ConfirmEmailAsync(user, token);

            return result;
        }

        public async Task GenerateEmailConfirmationTokenAsync(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidEmailTokenException();
            }

            await SendConfirmationEmailAsync(user, token);
        }

        public async Task SendConfirmationEmailAsync(User user, string token)
        {
            var code = Encode(token);

            var url = string.Format(_generalSettings.AppDomain + _generalSettings.EmailConfirmation, user.Id, code);

            await _emailSender.SendEmailAsync(user.Email, "Email Confirmation", $"Please confirm your account by" +
                $" <a href='{HtmlEncoder.Default.Encode(url)}'> clicking here </a>.");
        }      

        public async Task GenerateForgotPasswordTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidForgotPasswordTokenException();
            }

            await SendForgotPasswordEmailAsync(user, token);
        }

        public async Task SendForgotPasswordEmailAsync(User user, string token)
        {
            var code = Encode(token);

            var url = string.Format(_generalSettings.AppDomain + _generalSettings.ForgotPassword, user.Id, code);

            await _emailSender.SendEmailAsync(user.Email, "Forgot password", $"Reset password by" +
                $"<a href='{HtmlEncoder.Default.Encode(url)}'> clicking here </a>.");
        }


        public async Task<IdentityResult> ResetPasswordAsync(int userId, string code, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var token = Decode(code);
            var result =  await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                throw new IdentityResultException(result.Errors.FirstOrDefault().Description);
            }

            return result;
        }

        private async Task<string> CreateRefreshTokenAsync(int userId)
        {
            var refreshToken = _refreshTokenFactory.Create(userId);
            await _refreshTokenRepository.AddAsync(refreshToken);

            return refreshToken.Token;
        }

        private string Encode(string token)
        {
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        }

        private string Decode(string code)
        {
            return Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
    }
}
