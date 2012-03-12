using System.ComponentModel.DataAnnotations;

namespace StarterTemplate.Core.Validation
{
    public class PasswordAttribute : RegularExpressionAttribute
    {
        private const string DefaultErrorMessage = "The {0} must be at least 6 characters long, and may not contain spaces.";

        public PasswordAttribute()
            : base(@"^\S{6,}$")
        {
            ErrorMessage = DefaultErrorMessage;
        }

    }
}