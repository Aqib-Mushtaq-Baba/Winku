using System.ComponentModel.DataAnnotations.Schema;

namespace Winku.DatabaseFolder
{
    public class Like
    {
        public int Id { get; set; }

        public DateTime LikeTime { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
