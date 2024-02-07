using System.ComponentModel.DataAnnotations.Schema;
using Winku.DatabaseFolder;

namespace Winku.ViewModels
{
    public class AllPostVM
    {
        public AllPostVM()
        {
            Comments = new List<string>();
        }
        public int Id { get; set; }
        public string Usernname { get; set; }
        public string? Description { get; set; }
        public string? PostImagePath { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        
        public int LikeCount { get; set; }

        public bool IsLiked { get; set; }   

        public List<string>? Comments { get; set; }
    }
}
