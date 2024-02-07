using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Winku.DatabaseFolder;
using Winku.ViewModels;

namespace Winku.Controllers
{
    public class Administration : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public Administration(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public IEnumerable<AllUsersVM> ViewUse()
        {
            List<AllUsersVM> i = new List<AllUsersVM>();
            var users=userManager.Users;
            foreach (var user in users)
            {
                var r = new AllUsersVM();
                {
                    r.Id = user.Id;
                    r.Name = user.UserName;
                    i.Add(r);
                };
            }
            
            return i;
        }

        [HttpGet]
        public IActionResult ViewUsers()
        {
            var result = ViewUse();
            return View(result);
        }


    }
}
