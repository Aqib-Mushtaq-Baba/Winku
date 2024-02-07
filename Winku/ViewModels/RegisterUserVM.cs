using System.ComponentModel.DataAnnotations;

namespace Winku.ViewModels
{
    public class RegisterUserVM
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        public IFormFile? ProfilePicture { get; set; } 
    }
}
