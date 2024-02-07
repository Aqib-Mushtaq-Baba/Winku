using Winku.DatabaseFolder;

namespace Winku.Interfaces
{
    public interface ILikeInterface
    {
        Like Addlike(Like like);

        Like GetCheck(string id, int idd);

        Like DeleteLike(string id, int idd);

        //int LikeCount(int id);
    }
}
