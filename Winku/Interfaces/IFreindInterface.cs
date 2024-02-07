using Winku.DatabaseFolder;

namespace Winku.Interfaces
{
    public interface IFreindInterface
    {
        bool Check(string userid, string newUserId);
        bool AddCheck(string userid, string newUserId);
        bool DelCheck(string userid, string newUserId);
        string CheckStatus(string userid, string newUserId);
        Followers FollowFreind(Followers model);
        IEnumerable<Followers> MineFollowers(string id);
        Followers UnfollowFreind(string userid, string newUserId);

        IEnumerable<Followers> GetFreindRequets(string userid);

        Followers Edit(Followers model);

        Followers Get(string userid, string newUserId);
        Followers GetDecline(string userid, string newUserId);
    }
}
