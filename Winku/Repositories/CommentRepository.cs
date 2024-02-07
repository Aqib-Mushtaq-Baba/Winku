using Winku.DatabaseFolder;
using Winku.Interfaces;

namespace Winku.Repositories
{
    public class CommentRepository : ICommentInterface
    {
        private readonly AppDbContext context;

        public CommentRepository(AppDbContext context)
        {
            this.context = context;
        }
        public Comments AddComments(Comments comments)
        {
            context.Comments.Add(comments);
            context.SaveChanges();
            return comments;
        }

        public IEnumerable<Comments> GetAllComments(Comments comments)
        {
            throw new NotImplementedException();
        }
    }
}
