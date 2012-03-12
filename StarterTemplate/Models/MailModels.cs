using StarterTemplate.Core.Domain;

namespace StarterTemplate.Models
{
    public class MailWelcomeModel
    {
        public MailWelcomeModel(Member member)
        {
            To = member.EmailAddress;
        }

        public string To { get; set; }
    }

    public class MailResetPasswordModel
    {
        public MailResetPasswordModel(string email, string newPassword)
        {
            To = email;
            NewPassword = newPassword;
        }

        public string To { get; set; }
        public string NewPassword { get; set; }
    }
}