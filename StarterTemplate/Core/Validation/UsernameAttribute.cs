using System.ComponentModel.DataAnnotations;

namespace StarterTemplate.Core.Validation
{
    public class UsernameAttribute : RegularExpressionAttribute
    {
        private const string DefaultErrorMessage = "Username cannot contain whitespace.";

        public UsernameAttribute()
            : base(@"^\S*$")
        {
            ErrorMessage = DefaultErrorMessage;
        }
    }
}