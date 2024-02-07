using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Winku.DatabaseFolder;
using Winku.Interfaces;

namespace Winku.Repositories
{
    public class FreindRepository : IFreindInterface
    {
        private readonly AppDbContext context;

        public FreindRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Followers Get(string userid, string newUserId)
        {
            return context.Followers.FirstOrDefault(x => x.FollowerId == userid && x.FollowingId == newUserId);
        }
        public Followers GetDecline(string userid, string newUserId)
        {
            return context.Followers.FirstOrDefault(x => x.FollowingId == userid && x.FollowerId == newUserId);
        }

        public bool Check(string userid,string newUserId)
        {
            if (context.Followers.FirstOrDefault(x => x.FollowerId == userid && x.FollowingId == newUserId) is not null)
            {
                return true;
            }
            return false;
        }
        public bool DelCheck(string userid, string newUserId)
        {
            if (context.Followers.FirstOrDefault(x => x.FollowerId == userid && x.FollowingId == newUserId && x.Status==0) is not null)
            {
                return true;
            }
            return false;
        }
        public bool AddCheck(string userid, string newUserId)
        {
            if (context.Followers.FirstOrDefault(x => x.FollowerId == userid && x.FollowingId == newUserId && x.Status == 2) is not null)
            {
                return true;
            }
            return false;
        }
        public string CheckStatus(string loggedINuserid, string newUserId)
        {
            if (context.Followers.FirstOrDefault(x => x.FollowerId == loggedINuserid && x.FollowingId == newUserId && x.Status == 0) is not null)
            //if (context.Followers.FirstOrDefault(x => x.FollowerId == loggedINuserid && x.FollowingId == newUserId && x.Status == 0) is not null)
            {
                return "Sended";
            }
            else if (context.Followers.FirstOrDefault(x => (x.FollowerId == loggedINuserid && x.FollowingId == newUserId && x.Status == 1) || (x.FollowingId == loggedINuserid && x.FollowerId == newUserId && x.Status == 1)) is not null)
            //else if (context.Followers.FirstOrDefault(x => (x.FollowerId == loggedINuserid && x.FollowingId == newUserId) && (x.FollowingId == loggedINuserid && x.FollowerId == newUserId) && x.Status == 1) is not null)
            {
                return "Accepted";
            }

            else if (context.Followers.FirstOrDefault(x => x.FollowerId == loggedINuserid && x.FollowingId == newUserId && x.Status == 2) is not null)
            //else if (context.Followers.FirstOrDefault(x => (x.FollowerId == loggedINuserid && x.FollowingId == newUserId) && (x.FollowingId == loggedINuserid && x.FollowerId == newUserId) && x.Status == 2) is not null)
            {
                return "Rejected";
            }
            else
                return "Initial";
        }




        public Followers FollowFreind(Followers model)
        {
            context.Followers.Add(model);
            context.SaveChanges();
            return model;
        }

        public IEnumerable<Followers> GetFreindRequets(string userId)
        {
            var result=context.Followers.Where(x => x.FollowingId == userId && x.Status == 0).Include(nameof(Followers.ApplicationUser1));
            return result;
        }

        public Followers UnfollowFreind(string userid, string newUserId)
        {
            var result= context.Followers.FirstOrDefault(x => x.FollowerId == userid && x.FollowingId == newUserId);
            context.Followers.Remove(result);
            context.SaveChanges();
            return result;
        }

        public Followers Edit(Followers model)
        {
            var result = context.Followers.Attach(model);
            result.State =EntityState.Modified;
            context.SaveChanges();
            return model;
        }

        public IEnumerable<Followers> MineFollowers(string id)
        {
            return context.Followers.Where(x => x.FollowingId == id && x.Status==1);
        }
    }
}
