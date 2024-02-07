using Microsoft.AspNetCore.Identity;
using Winku.DatabaseFolder;

namespace Winku.CustomServices
{
    public class LayoutClassService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public LayoutClassService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<string> GetUserProfile(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (string.IsNullOrEmpty(user.ProfilePath))
            {
                return "DummyImage.png";
            }
            else
            {
                return user.ProfilePath;
            }
        }
    }
}
