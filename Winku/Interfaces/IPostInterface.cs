using Winku.DatabaseFolder;
using Winku.ViewModels;

namespace Winku.Interfaces
{
    public interface IPostInterface
    {
        Post Create(Post post);

        //IEnumerable<Post> GetAllPosts(string id);
        Task<IEnumerable<AllPostVM>> GetAllPostsAsync(string id);
        Task<IEnumerable<AllPostVM>> FollowersGetAllPostsAsync(string id);

        Post GetSinglePost(int id);

        Post EditPost(Post post);

        Post Delete(int id);


    }
}