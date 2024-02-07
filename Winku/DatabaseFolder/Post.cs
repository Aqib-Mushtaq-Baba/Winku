using System.ComponentModel.DataAnnotations.Schema;

namespace Winku.DatabaseFolder
{
    public class Post
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? PostImagePath { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<Like> Likes { get; set; }
        public ICollection<Comments> Comments { get; set; }
    }
}
