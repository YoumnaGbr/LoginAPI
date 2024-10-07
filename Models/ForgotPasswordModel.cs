using System.ComponentModel.DataAnnotations;

namespace LoginAPI.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
