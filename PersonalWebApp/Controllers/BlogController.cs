using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalWebApp.Data;
using PersonalWebApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace PersonalWebApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public BlogController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private readonly ApplicationDbContext _context;

        public async Task<IActionResult> Index()
        {
            var blogPosts = await _context.BlogPost.Include(p => p.Author).OrderByDescending(p => p.DateCreated).ToListAsync();
            return View(blogPosts);
        }

        [HttpGet]
        [Authorize(Policy = "CanWriteBlogPosts")]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanWriteBlogPosts")]
        public async Task<IActionResult> Create(BlogPost blogPost)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            
            

            if (ModelState.IsValid)
            {
                
                blogPost.DateCreated = DateTime.UtcNow;
                //blogPost.AuthorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                blogPost.AuthorId = currentUser.Id;
                blogPost.Author = currentUser;

                _context.Add(blogPost);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errors = ModelState.Values.SelectMany(p => p.Errors);
            }
            return View(blogPost);
        }
    }
}
