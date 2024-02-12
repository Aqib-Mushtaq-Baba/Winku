using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Winku.DatabaseFolder;
using Winku.ViewModels;
using Winku.Repositories;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;

namespace Winku.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly EmailRepository emailRepository;

        public AdministrationController(UserManager<ApplicationUser> userManager,
                                EmailRepository emailRepository)
        {
            this.userManager = userManager;
            this.emailRepository = emailRepository;
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(EmailVM model)
        {
            if(ModelState.IsValid)
            {
                var Email = new Email
                {
                    EmailFrom = model.EmailFrom,
                    EmailTo = "aqibmushtaqbaba@gmail.com",
                    Subject = model.Subject,
                    EmailBody = model.EmailBody,
                    SendTime = DateTime.Now
                };
                emailRepository.AddEmail(Email);
                
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress(model.EmailFrom, "aqibmushtaqbaba@gmail.com"));
                email.To.Add(new MailboxAddress("Receiver Name", "aqibmushtaqbaba@gmail.com"));

                email.Subject = model.Subject;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text =model.EmailBody
                };

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, false);

                    // Note: only needed if the SMTP server requires authentication
                    smtp.Authenticate("aqibmushtaqbaba@gmail.com", "iepu oisl aeci aaro");

                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


    }
}
