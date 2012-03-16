using System.ComponentModel.DataAnnotations;
using StarterTemplate.Core.Validation;

namespace StarterTemplate.Models
{
    public class AuthIndexModel
    {
        public AuthIndexModel()
        {
            LogInModel = new AuthLogInModel();
            SignUpModel = new AuthSignUpModel();
            ForgotPasswordModel = new AuthForgotPasswordModel();
            ChangePasswordModel = new AuthChangePasswordModel();
        }

        public string ActivePanel { get; set; }
        public string ReturnUrl { get; set; }

        public AuthLogInModel LogInModel { get; set; }
        public AuthSignUpModel SignUpModel { get; set; }
        public AuthForgotPasswordModel ForgotPasswordModel { get; set; }
        public AuthChangePasswordModel ChangePasswordModel { get; set; }
    }

    public class AuthLogInModel
    {
        [Required]
        public string UsernameOrEmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class AuthSignUpModel
    {
        [Required]
        [Username]
        [UsernameIsAvailable]
        public string Username { get; set; }

        [Required]
        [Email]
        [EmailIsAvailable]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        [Password]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class AuthForgotPasswordModel
    {
        [Required]
        [Email]
        [EmailIsRegistered]
        public string EmailAddress { get; set; }
    }

    public class AuthChangePasswordModel
    {
        [Required]
        [Password]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
