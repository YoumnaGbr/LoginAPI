using System.ComponentModel.DataAnnotations;

namespace LoginAPI.Models
{
    public class ResetPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Otp { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
