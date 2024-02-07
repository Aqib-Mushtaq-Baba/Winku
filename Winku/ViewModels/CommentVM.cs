using System.ComponentModel.DataAnnotations.Schema;
using Winku.DatabaseFolder;

namespace Winku.ViewModels
{
    public class CommentVM
    {
        public int Id { get; set; }
        public int PostId { get; set; }

        public string? Comment { get; set; }

    }
}
