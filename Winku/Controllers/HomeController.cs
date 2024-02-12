using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Winku.DatabaseFolder;
using Winku.Repositories;

namespace Winku.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly PostRepository postRepository;

        public HomeController(UserManager<ApplicationUser> userManager,
                                PostRepository postRepository)
        {
            this.userManager = userManager;
            this.postRepository = postRepository;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var userId = userManager.GetUserId(User);
            var posts = await postRepository.GetAllPostsAsync(userId);
            var postCount = posts.Count();
            TempData["postCount"] = postCount;
            return View();
        }
    }
}
