using Winku.DatabaseFolder;

namespace Winku.Interfaces
{
    public interface ICommentInterface
    {
        Comments AddComments(Comments comments);
        IEnumerable<Comments> GetAllComments(Comments comments);
    }
}
