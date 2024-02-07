using System.ComponentModel.DataAnnotations.Schema;

namespace Winku.DatabaseFolder
{
    public class Comments
    {
        public int Id { get; set; }

        public string? Comment { get; set; } 

        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Post")]
        public int? PostId { get; set; }
        public Post Post { get; set; }


    }
}
