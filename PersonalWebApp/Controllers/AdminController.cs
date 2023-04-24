using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalWebApp.Models;
using System.Security.Claims;

namespace PersonalWebApp.Controllers
{
    [Authorize(Policy ="RequireAdminRole")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var model = new List<AdminViewModel>();

            foreach (var user in users)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                var canCreateBlogPost = claims.Any(c => c.Type == "CanCreateBlogPosts");
                model.Add(new AdminViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    CanCreateBlogPost = canCreateBlogPost
                });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCreateBlogPostClaim(string userID, bool canCreateBlogPost)
        {
            var user = await _userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return NotFound();
            }
            if (canCreateBlogPost)
            {
                await _userManager.AddClaimAsync(user, new Claim("CanWriteBlogPosts", "true"));
            }
            else
            {
                var claim = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "CanWriteBlogPosts");
                if (claim == null)
                {
                    await _userManager.RemoveClaimAsync(user, claim);
                }
            }

            return Ok();
        }
    }
}
