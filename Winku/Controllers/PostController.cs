using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using Winku.DatabaseFolder;
using Winku.Interfaces;
using Winku.Repositories;
using Winku.ViewModels;

namespace Winku.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostInterface postRepository;
        private readonly ILikeInterface likeRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext context;
        private readonly FreindRepository freindRepository;

        public PostController(IPostInterface postRepository,
                                ILikeInterface likeRepository,
                                IWebHostEnvironment webHostEnvironment,
                                UserManager<ApplicationUser> userManager,
                                AppDbContext context,
                                FreindRepository freindRepository)
        {
            this.postRepository = postRepository;
            this.likeRepository = likeRepository;
            this.webHostEnvironment = webHostEnvironment;
            this.userManager = userManager;
            this.context = context;
            this.freindRepository = freindRepository;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PostVM model)
        {
            var currentUserId = userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                string dpPost = upload(model);
                Post post = new Post()
                {
                    Description = model.Description,
                    PostImagePath = dpPost,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    UserId = currentUserId
                };

                postRepository.Create(post);
                return RedirectToAction("Viewposts", "post");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ViewPostsAsync()
        {
            var userId = userManager.GetUserId(User);
            var followers=freindRepository.MineFollowers(userId);
            //var following=freindRepository.MeFollowing(userId);
            var p = await postRepository.GetAllPostsAsync(userId);
            var posts = p.ToList();
            foreach (var item in followers)
            {
                var x =await postRepository.GetAllPostsAsync(item.FollowerId);
                //var y =await postRepository.GetAllPostsAsync(item.FollowingId);
                var followersPost = x.ToList();
                //var followersPost1 = y.ToList();
                posts.AddRange(followersPost);
                //posts.AddRange(followersPost1);
            }
            posts = posts.OrderByDescending(x => x.CreatedOn).ToList();
            return View(posts);
        }

        [HttpGet]
        public IActionResult ViewSinglePost(int id)
        {   
            var result=postRepository.GetSinglePost(id);
            return View(result);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var singlePost=postRepository.GetSinglePost(id);
            if (singlePost != null)
            {                
                var model = new EditPostVM
                {
                    Description = singlePost.Description,
                    ExistingPostImage =singlePost.PostImagePath
                };
                return View(model);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Edit(EditPostVM model)
        {
            var post = postRepository.GetSinglePost(model.Id);
            if (ModelState.IsValid)
            {
                post.Description = model.Description;
                if (model.PostImage != null)
                {
                    if (model.ExistingPostImage != null)
                    {
                        string filePath1 = Path.Combine(webHostEnvironment.WebRootPath, "UserProfilePicture", model.ExistingPostImage);
                        System.IO.File.Delete(filePath1);
                    }
                    post.PostImagePath = upload(model);
                }
                post.UpdatedOn = DateTime.Now;

                postRepository.EditPost(post);
                return RedirectToAction("Viewposts", "post");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            postRepository.Delete(id);
            return RedirectToAction("viewposts", "post");
        }

        
        private string? upload(PostVM model)
        {
            string uniqueFileName = null;
            if (model.PostImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "PostImages");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.PostImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    model.PostImage.CopyTo(filestream);
                }
            }

            return uniqueFileName;
        }
    }
}
