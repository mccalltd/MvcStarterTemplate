using ActionMailer.Net.Mvc;
using StarterTemplate.Models;

namespace StarterTemplate.Controllers
{
    public class MailController : MailerBase
    {
        public EmailResult SendWelcomeMessage(MailWelcomeModel model)
        {
            To.Add(model.To);
            Subject = "Welcome to StarterTemplate";
            return Email("Welcome", model);
        }

        public EmailResult SendResetPasswordMessage(MailResetPasswordModel model)
        {
            To.Add(model.To);
            Subject = "Your new password";
            return Email("ResetPassword", model);
        }
    }
}
