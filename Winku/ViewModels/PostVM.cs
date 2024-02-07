using System.ComponentModel.DataAnnotations.Schema;
using Winku.DatabaseFolder;

namespace Winku.ViewModels
{
    public class PostVM
    {
        public string? Description { get; set; }
        public IFormFile? PostImage { get; set; }
    }
}
