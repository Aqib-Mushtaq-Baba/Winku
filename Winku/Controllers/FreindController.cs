using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Winku.DatabaseFolder;
using Winku.Repositories;
using Winku.ViewModels;

namespace Winku.Controllers
{
    public class FreindController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly FreindRepository freindRepository;
        private readonly AppDbContext context;

        public FreindController(UserManager<ApplicationUser> userManager,
                                FreindRepository freindRepository,
                               AppDbContext context)
        {
            this.userManager = userManager;
            this.freindRepository = freindRepository;
            this.context = context;
        }

        [HttpGet]
        public JsonResult Accept(string id)
        {
            var newUsersId = id;
            var currentUserId = userManager.GetUserId(User);

            var abc = freindRepository.Get(newUsersId, currentUserId);

            if (abc != null)
            {
                abc.RequestAcceptedTime = DateTime.Now;
                abc.Status = 1;

                freindRepository.Edit(abc);
                return Json("Request Accepted++++");
            }
            return Json("User Not Found");
        }

        [HttpGet]
        public JsonResult Decline(string id)
        {
            var newUsersId = id;
            var currentUserId = userManager.GetUserId(User);
            var abc = freindRepository.Get(newUsersId, currentUserId);
            if (abc != null)
            {
                abc.RequestRejectTime = DateTime.Now;
                abc.Status = 2;

                freindRepository.Edit(abc);
                return Json("Request Declined++++");
            }
            return Json("User Not Found");
        }

        [HttpGet]
        public JsonResult AddFreind(string id)
        {
            var newUsersId = id;
            var currentUserId = userManager.GetUserId(User);
            var result = freindRepository.Check(currentUserId, newUsersId);

            if (result == true)
            {
                var abc = freindRepository.AddCheck(currentUserId, newUsersId);
                if (abc == true)
                {
                    var xyz = freindRepository.Get(currentUserId, newUsersId);
                    if (xyz != null)
                    {
                        xyz.RequestSendTime = DateTime.Now;
                        xyz.Status = 0;
                        freindRepository.Edit(xyz);
                        return Json("Edit Request Sended++++");
                    }
                }
            }
            else
            {
                Followers followers = new Followers()
                {
                    FollowerId = currentUserId,
                    FollowingId = newUsersId,
                    RequestSendTime = DateTime.Now,
                    Status = 0
                };
                freindRepository.FollowFreind(followers);
                return Json("Request Sended++++");
            }
            return Json("Some ERROR++++");
        }

        [HttpGet]
        public JsonResult DelFreind(string id)
        {
            var newUsersId = id;
            var currentUserId = userManager.GetUserId(User);
            var result = freindRepository.Check(currentUserId, newUsersId);

            if (result == true)
            {
                var abc = freindRepository.DelCheck(currentUserId, newUsersId);
                if (abc == true)
                {
                    freindRepository.UnfollowFreind(currentUserId, newUsersId);
                    return Json("Entry Deleted++++");
                }
                else
                {
                    var xyz = freindRepository.Get(currentUserId, newUsersId);
                    if (xyz != null)
                    {
                        xyz.RequestRejectTime = DateTime.Now;
                        xyz.Status = 2;
                        freindRepository.Edit(xyz);
                        return Json("Edit Entry Deleted++++");
                    }
                }
            }            
            return Json("bIGGY ErrOr");
        }

        [HttpGet]
        public IActionResult FreindRequests()        
        {   
            List<FreindRequestsVM> freindRequests= new List<FreindRequestsVM>();   
            var currentUserId=userManager.GetUserId(User);
            var result=freindRepository.GetFreindRequets(currentUserId);
            
            if (result != null)
            {
                foreach (var item in result)
                {
                    var obj = new FreindRequestsVM();
                    obj.Id = item.FollowerId;
                    obj.UserName = item.ApplicationUser1.UserName;
                    obj.ProfileImage = item.ApplicationUser1.ProfilePath;
                    freindRequests.Add(obj);
                }
                return View(freindRequests);
            }
            return View();
        }

        [HttpGet]
        public JsonResult AjaxFreindRequests()
        {
            List<FreindRequestsVM> freindRequests = new List<FreindRequestsVM>();
            var currentUserId = userManager.GetUserId(User);
            var result = freindRepository.GetFreindRequets(currentUserId);

            if (result != null)
            {
                foreach (var item in result)
                {
                    var obj = new FreindRequestsVM();
                    obj.Id = item.FollowerId;
                    obj.UserName = item.ApplicationUser1.UserName;
                    if(item.ApplicationUser1.ProfilePath != null)
                    {
                        obj.ProfileImage = item.ApplicationUser1.ProfilePath;
                    }
                    else
                    {
                        obj.ProfileImage = "DummyImage.png";
                    }
                    
                    freindRequests.Add(obj);
                }
                return Json(freindRequests);
            }
            return Json("ERROR error ERROR");
        }


    }
}
