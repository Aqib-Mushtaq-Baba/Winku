using Winku.DatabaseFolder;
using Winku.Interfaces;

namespace Winku.Repositories
{
    public class LikeRepository : ILikeInterface
    {
        private readonly AppDbContext context;

        public LikeRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Like GetCheck(string userId, int postId)
        {
            return context.Likes.FirstOrDefault(like => like.UserId == userId && like.PostId == postId);
        }

        public Like DeleteLike(string userId, int postId)
        {
            var result = context.Likes.FirstOrDefault(like => like.UserId == userId && like.PostId == postId);
            context.Likes.Remove(result);
            context.SaveChanges();
            return result;
        }

        public Like Addlike(Like like)
        {
            context.Likes.Add(like);
            context.SaveChanges();
            return like;
        }

        //public int LikeCount(int id)
        //{
        //    return context.Likes.Where(x=>x.PostId== id).Count();
        //}
    }
}
