namespace StarterTemplate.Models
{
    public class MailWelcomeModel
    {
        public string To { get; set; }
    }

    public class MailResetPasswordModel
    {
        public string To { get; set; }
        public string NewPassword { get; set; }
    }
}