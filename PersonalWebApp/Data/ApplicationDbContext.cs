using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalWebApp.Models;

namespace PersonalWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<BlogPost> BlogPost { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) 
        {
        }
    }
}
