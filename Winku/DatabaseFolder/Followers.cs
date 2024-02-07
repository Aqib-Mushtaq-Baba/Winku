using System.ComponentModel.DataAnnotations.Schema;

namespace Winku.DatabaseFolder
{
    public class Followers
    {
        public int? Id { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string? FollowingId { get; set; }
        public ApplicationUser? ApplicationUser1 { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string? FollowerId { get; set; }
        public ApplicationUser? ApplicationUser2 { get; set; }
        public int? Status { get; set; }

        public DateTime? RequestSendTime { get; set; }
        public DateTime? RequestAcceptedTime { get; set; }
        public DateTime? RequestRejectTime { get; set; }
    }
}
