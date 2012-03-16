using System;
using System.ComponentModel.DataAnnotations;
using StarterTemplate.Core.Validation;

namespace StarterTemplate.Core.Domain
{
    public class Member : MutableEntityBase
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [Email]
        public string EmailAddress { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public bool IsAdministrator { get; set; }

        public bool MustChangePasswordOnNextLogin { get; set; }
    }
}
