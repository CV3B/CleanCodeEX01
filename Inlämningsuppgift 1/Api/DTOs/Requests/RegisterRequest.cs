using System.ComponentModel.DataAnnotations;

namespace Inlämningsuppgift_1.Api.DTOs.Requests
{
    public class RegisterRequest 
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = "";
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = ""; 
        [Required]
        [EmailAddress]
        public string Email { get; set; } = ""; 
    }

}
