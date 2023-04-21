using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
namespace PersonalWebApp.ViewComponents
{
    public class CreateBlogPostLinkViewComponent : ViewComponent
    {
        private readonly UserManager<IdentityUser> _userManager;

        public CreateBlogPostLinkViewComponent(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var canWriteBlogPosts = false;
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync((ClaimsPrincipal)User);
                canWriteBlogPosts = (await _userManager.GetClaimsAsync(user)).Any();
            }
            return View("Default", canWriteBlogPosts);
        }
    }
}
