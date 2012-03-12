using System.ComponentModel.DataAnnotations;
using StarterTemplate.Core.Validation;

namespace StarterTemplate.Models
{
    public class HomeContactModel
    {
        [Required]
        [Display(Name = "Your Name")]
        public string Name { get; set; }

        [Required]
        [Email]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}