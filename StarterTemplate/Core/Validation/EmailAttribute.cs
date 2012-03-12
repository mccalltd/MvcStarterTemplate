using System.ComponentModel.DataAnnotations;

namespace StarterTemplate.Core.Validation
{
    public class EmailAttribute : RegularExpressionAttribute
    {
        private const string DefaultErrorMessage = "Invalid email address.";

        public EmailAttribute()
            : base(@"^.+?@.+?\..+$")
        {
            ErrorMessage = DefaultErrorMessage;
        }
    }
}
