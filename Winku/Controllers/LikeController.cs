using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Winku.DatabaseFolder;
using Winku.Interfaces;

namespace Winku.Controllers
{
    public class LikeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPostInterface postRepository;
        private readonly ILikeInterface likeRepository;
        private readonly AppDbContext context;

        public LikeController(UserManager<ApplicationUser> userManager,
                                IPostInterface postRepository,
                                ILikeInterface likeRepository,
                                AppDbContext context)
        {
            this.userManager = userManager;
            this.postRepository = postRepository;
            this.likeRepository = likeRepository;
            this.context = context;
        }
        [HttpGet]
        public JsonResult LikePost(int id)
        {
            var currentPostId = id;
            var currentUserId = userManager.GetUserId(User);
            var result = postRepository.GetSinglePost(id);
            if (result != null)
            {
                var check = likeRepository.GetCheck(currentUserId, currentPostId);
                if (check == null)
                {
                    Like like = new Like()
                    {
                        LikeTime = DateTime.Now,
                        UserId = currentUserId,
                        PostId = id
                    };

                    likeRepository.Addlike(like);
                    var LikeCount = context.Likes.Where(x => x.PostId == currentPostId).Count();
                    return Json(LikeCount + " Liked");
                }
                else
                {
                    likeRepository.DeleteLike(currentUserId, currentPostId);
                    var LikeCount = context.Likes.Where(x => x.PostId == currentPostId).Count();
                    return Json(LikeCount + " Likes");
                }
            }
            return Json("BIG ERROR,,,,,,Dislike");
        }
    }
}
