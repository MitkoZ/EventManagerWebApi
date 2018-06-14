using EventManager.Helpers;
using System.ComponentModel.DataAnnotations;

namespace EventManager.ViewModels.Home
{
    public class RegisterViewModel
    {
        [Required]
        [MinLength(5, ErrorMessage = "Username must be at least 5 symbols")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Password]
        [MinLength(6, ErrorMessage = "Password must be at least 6 symbols")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Password]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}