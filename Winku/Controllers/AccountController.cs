using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.Arm;
using Winku.DatabaseFolder;
using Winku.Repositories;
using Winku.ViewModels;

namespace Winku.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly FreindRepository freindRepository;

        public AccountController(UserManager<ApplicationUser> userManager,
                                    SignInManager<ApplicationUser> signInManager,
                                    IWebHostEnvironment webHostEnvironment,
                                    FreindRepository freindRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.webHostEnvironment = webHostEnvironment;
            this.freindRepository = freindRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUserVM model)
        {
                if (ModelState.IsValid)
                {
                    string dp = upload(model);
                    var dpUser = new ApplicationUser
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        ProfilePath = dp
                    };

                    var result1 = await userManager.CreateAsync(dpUser, model.Password);

                    if (result1.Succeeded)
                    {
                        TempData["RegisterSuccessMessage"] = "Registered &&&&& Logged in Peacefully";
                        await signInManager.SignInAsync(dpUser, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError(string.Empty, "Some error occured");
                }
                return View();            
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginUserVM model, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);


                if (result.Succeeded)
                {
                    TempData["LoginSuccessMessage"] = "Logged in Peacefully";
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditUserDetails(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var model = new EditRegisterUserVM
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    ExistingProfilePicture = user.ProfilePath
                };
                return View(model);
            }
            else
            {

                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");

            }

        }

        [HttpPost]
        public async Task<IActionResult> EditUserDetails(EditRegisterUserVM model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            if (user != null)
            {

                user.UserName = model.UserName;
                user.Email = model.Email;
                if (model.ProfilePicture != null)
                {
                    if (model.ExistingProfilePicture != null)
                    {
                        string filePath1 = Path.Combine(webHostEnvironment.WebRootPath, "UserProfilePicture", model.ExistingProfilePicture);
                        System.IO.File.Delete(filePath1);
                    }

                    user.ProfilePath = upload(model);
                }

                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            else
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }

        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            TempData["LogoutMessage"] = $"Logged out Successfully {User.Identity.Name}";
            return RedirectToAction("Index", "Home");
        }

        public IEnumerable<SearchedUsersResponseVM> Search(string username)
        {
            List<SearchedUsersResponseVM> searchedUsersVM = new List<SearchedUsersResponseVM>();
            var allUsers=userManager.Users;
            var z = User.Identity.Name;
            var loggedInUser = userManager.GetUserId(User);
            foreach (var item in allUsers)
            {
                if (item.UserName.ToLower().Contains(username.ToLower()) && item.UserName != z)
                {
                    var obj = new SearchedUsersResponseVM();
                    obj.Id=item.Id;
                    obj.Username = item.UserName;
                    obj.ProfileImage = item.ProfilePath;

                    if (freindRepository.CheckStatus(loggedInUser, item.Id) == "Sended")
                    {
                        obj.IsAdded ="Sended";
                    }
                    else if (freindRepository.CheckStatus(loggedInUser, item.Id) == "Accepted")
                    {
                        obj.IsAdded = "Accepted";
                    }
                    //else if (freindRepository.CheckStatus(loggedInUser, item.Id) == "Rejected")
                    //{
                    //    obj.IsAdded = "Rejected";
                    //}
                    else
                    {
                        obj.IsAdded = "Rejected";
                    }

                    //obj.IsAdded = freindRepository.Check(loggedInUser,item.Id);
                    searchedUsersVM.Add(obj);
                }
            }
            return searchedUsersVM;
        }

        [HttpPost]
        public IActionResult SearchUsers(SearchedUsersResponseVM model)
        {
                var result = Search(model.Username);
                if (result != null)
                {
                    return View(result);
                }
                return View();
           
        }
        private string? upload(RegisterUserVM model)
        {
            string uniqueFileName = null;
            if (model.ProfilePicture != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "UserProfilePictures");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfilePicture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfilePicture.CopyTo(filestream);
                }
            }

            return uniqueFileName;
        }
    }
}
