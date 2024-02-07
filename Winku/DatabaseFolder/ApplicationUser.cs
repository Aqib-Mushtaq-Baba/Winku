using Microsoft.AspNetCore.Identity;

namespace Winku.DatabaseFolder
{
    public class ApplicationUser:IdentityUser
    {
        public string? ProfilePath { get; set; }

        public ICollection<Post> Post { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comments> Comments { get; set; }

        public ICollection<Followers> Followers1 { get; set; }
        public ICollection<Followers> Followers2 { get; set; }
    }
}
