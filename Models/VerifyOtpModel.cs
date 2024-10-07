using System.ComponentModel.DataAnnotations;

namespace LoginAPI.Models
{
    public class VerifyOtpModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Otp { get; set; }
    }

}
