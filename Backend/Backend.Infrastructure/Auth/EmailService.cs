using Backend.Core.Entities;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Auth
{
    public class EmailService : IEmailService
    {
        private readonly AuthMessageSenderSettings _senderSettings;
        private readonly UserManager<User> _userManager;

        public EmailService(AuthMessageSenderSettings senderSettings, UserManager<User> userManager)
        {
            _senderSettings = senderSettings;
            _userManager = userManager;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            await Execute(_senderSettings.SendGridKey, subject, message, email);
        }

        public async Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("fitwebwebapp@gmail.com", _senderSettings.SendGridUser),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            msg.AddTo(new EmailAddress(email));

            msg.SetClickTracking(false, false);

            await client.SendEmailAsync(msg);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(int userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new ServiceException(ErrorCodes.InvalidValue, $"User with id '{userId}' not found.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var result = await _userManager.ConfirmEmailAsync(user, code);

            return result;
        }
    }
}
