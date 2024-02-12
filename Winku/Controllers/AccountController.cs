using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Runtime.Intrinsics.Arm;
using Winku.CustomServices;
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
        private readonly EmailService emailService;

        public AccountController(UserManager<ApplicationUser> userManager,
                                    SignInManager<ApplicationUser> signInManager,
                                    IWebHostEnvironment webHostEnvironment,
                                    FreindRepository freindRepository,
                                    EmailService emailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.webHostEnvironment = webHostEnvironment;
            this.freindRepository = freindRepository;
            this.emailService = emailService;
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
                        var token = await userManager.GenerateEmailConfirmationTokenAsync(dpUser);

                        var confirmationLink = Url.Action("ConfirmEmail", "Account",
                            new { userId = dpUser.Id, token = token }, Request.Scheme);

                        emailService.emailSent(dpUser.Email, confirmationLink);

                        ViewBag.ErrorTitle = "Registration successful";
                        ViewBag.ErrorMessage = "Before you can Login, please confirm your " +
                                "email, by clicking on the confirmation link we have emailed you";
                        return View("Error");

                        //TempData["RegisterSuccessMessage"] = "Registered &&&&& Logged in Peacefully";
                        //        await signInManager.SignInAsync(dpUser, isPersistent: false);
                        //        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError(string.Empty, "Some error occured");
                }
                return View();            
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("index", "home");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User ID {userId} is invalid";
                return View("NotFound");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }

            ViewBag.ErrorTitle = "Email cannot be confirmed";
            return View("Error");
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
                //var user =await userManager.FindByNameAsync(model.UserName);
                var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);


                //if (result.Succeeded && user.EmailConfirmed)
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await userManager.FindByEmailAsync(model.Email);
                // If the user is found AND Email is confirmed
                //if (user != null && await userManager.IsEmailConfirmedAsync(user))
                if (user != null)
                {
                    // Generate the reset password token
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);

                    // Build the password reset link
                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                            new { email = model.Email, token = token }, Request.Scheme);

                    emailService.emailSent(user.Email,passwordResetLink);

                    // Log the password reset link
                    //logger.Log(LogLevel.Warning, passwordResetLink);

                    // Send the user to Forgot Password Confirmation view
                    return View("ForgotPasswordConfirmation");
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist or is not confirmed
                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            // If password reset token or email is null, most likely the
            // user tried to tamper the password reset link
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    // reset the user password
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        // Upon successful password reset and if the account is lockedout, set
                        // the account lockout end date to current UTC date time, so the user
                        // can login with the new password
                        if (await userManager.IsLockedOutAsync(user))
                        {
                            await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                        }
                        return View("ResetPasswordConfirmation");
                    }
                    // Display validation errors. For example, password reset token already
                    // used to change the password or password complexity rules not met
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist
                return View("ResetPasswordConfirmation");
            }
            // Display validation errors if model state is not valid
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            //var user = await userManager.GetUserAsync(User);

            //var userHasPassword = await userManager.HasPasswordAsync(user);

            //if (!userHasPassword)
            //{
            //    return RedirectToAction("AddPassword");
            //}

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                // ChangePasswordAsync changes the user password
                var result = await userManager.ChangePasswordAsync(user,
                    model.CurrentPassword, model.NewPassword);

                // The new password did not meet the complexity rules or
                // the current password is incorrect. Add these errors to
                // the ModelState and rerender ChangePassword view
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }

                // Upon successfully changing the password refresh sign-in cookie
                await signInManager.RefreshSignInAsync(user);
                return View("ChangePasswordConfirmation");
            }
            return View(model);
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
