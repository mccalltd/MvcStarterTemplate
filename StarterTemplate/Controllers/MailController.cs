using ActionMailer.Net.Mvc;
using StarterTemplate.Core.Domain;
using StarterTemplate.Core.Extensions;
using StarterTemplate.Models;

namespace StarterTemplate.Controllers
{
    public class MailController : MailerBase
    {
        public EmailResult SendWelcomeMessage(Member member)
        {
            var model = new MailWelcomeModel
            {
                To = member.EmailAddress
            };

            To.Add(model.To);
            Subject = "Welcome to StarterTemplate";
            return Email("Welcome", model);
        }

        public EmailResult SendResetPasswordMessage(string emailAddress, string newPassword)
        {
            var model = new MailResetPasswordModel
            {
                To = emailAddress,
                NewPassword = newPassword
            };

            To.Add(model.To);
            Subject = "Your new password";
            return Email("ResetPassword", model);
        }

        public EmailResult SendContactMessage(HomeContactModel model)
        {
            To.Add("support@domain.com");
            From = model.EmailAddress;
            ReplyTo.Add(model.EmailAddress);
            Subject = "{0} sent a message".FormatWith(model.Name);
            return Email("Contact", model);
        }
    }
}
