using Microsoft.AspNetCore.Identity;
using Winku.DatabaseFolder;
using Winku.Interfaces;
using Winku.ViewModels;

namespace Winku.Repositories
{
    public class PostRepository : IPostInterface
    {
        private readonly AppDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public PostRepository(AppDbContext context,
                                    UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public Post Create(Post posts)
        {
            context.Posts.Add(posts);
            context.SaveChanges();
            return posts;
        }

        public Post Delete(int id)
        {
            var result = context.Posts.Find(id);
            context.Posts.Remove(result);
            context.SaveChanges();
            return result;
        }

        public Post EditPost(Post post)
        {
            var result = context.Posts.Attach(post);
            result.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return post;
        }

        public async Task<IEnumerable<AllPostVM>> GetAllPostsAsync(string id)
        {
            List<AllPostVM> posts = new List<AllPostVM>();
            var all = context.Posts.Where(x => x.UserId == id);
            foreach (var post in all)
            {
                var obj = new AllPostVM();
                //obj.LikeCount = context.Likes.allcount();
                obj.IsLiked = context.Likes.Where(x => x.UserId == id && x.PostId == post.Id).Any();
                obj.LikeCount = context.Likes.Where(x => x.PostId == post.Id).Count();
                obj.PostImagePath = post.PostImagePath;
                var appUser = await userManager.FindByIdAsync(post.UserId);
                obj.Usernname = appUser.UserName;
                obj.Description = post.Description;
                obj.CreatedOn = post.CreatedOn;
                obj.Id = post.Id;
                if (post.Comments !=null && post.Comments.Count>0)
                {
                    foreach (var item in post.Comments)
                    {
                        obj.Comments.Add(item.Comment);
                    }
                }
                
                posts.Add(obj);
            }
            return posts;
        }
        //public IEnumerable<AllPostVM> GetAllPosts(string id)
        //{
        //    var posts = context.Posts
        //        .Where(x => x.UserId == id)
        //        .Select(post => new AllPostVM
        //        {
        //            IsLiked = context.Likes.Any(x => x.UserId == id && x.PostId == post.Id),
        //            LikeCount = context.Likes.Count(x => x.PostId == post.Id),
        //            PostImagePath = post.PostImagePath,
        //            Description = post.Description,
        //            Id = post.Id,
        //            Comments = post.Comments.Select(item => item.Comment).ToList()
        //        })
        //        .ToList();

        //    return posts;
        //}

        //public IEnumerable<AllPostVM> GetAllPosts(string id)
        //{
        //    return context.Posts;  //cant return posts
        //}

        public Post GetSinglePost(int id)
        {
            return context.Posts.Find(id);
        }

        //IEnumerable<AllPostVM> IPostRepository.GetAllPosts(string id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
