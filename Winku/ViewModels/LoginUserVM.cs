using System.ComponentModel.DataAnnotations;

namespace Winku.ViewModels
{
    public class LoginUserVM
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
