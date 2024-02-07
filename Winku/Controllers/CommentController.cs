using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Winku.DatabaseFolder;
using Winku.Repositories;
using Winku.ViewModels;

namespace Winku.Controllers
{
    public class CommentController : Controller
    {
        private readonly CommentRepository commentRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext context;

        public CommentController(CommentRepository commentRepository,
                                UserManager<ApplicationUser> userManager,
                                AppDbContext context)
        {
            this.commentRepository = commentRepository;
            this.userManager = userManager;
            this.context = context;
        }
        public JsonResult Add_Cmnt(CommentVM model)
        {
            var user = userManager.GetUserId(User);
            Comments comment = new Comments()
            {
                PostId = model.PostId,
                
                Comment = model.Comment,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                UserId = user
            };
            commentRepository.AddComments(comment);
            return Json(comment);}

        public IActionResult AddComment(CommentVM model)
        {
            var result=Add_Cmnt(model);
            //var responseData = new { result = result };
            return Json(result);

        }
    }
}
