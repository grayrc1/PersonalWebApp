using Microsoft.AspNetCore.Identity;

namespace PersonalWebApp.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string AuthorId { get; set; }
        public IdentityUser Author { get; set; }
    }
}
