using System.ComponentModel.DataAnnotations;

namespace Talapat.DTOs.Register
{
    public class RegistersDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$",
            ErrorMessage = "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, and one number.")]
        public string Password { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
       
    }
}
