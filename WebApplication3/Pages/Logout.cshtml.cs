using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Model;

namespace WebApplication3.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private UserManager<ApplicationUser> userManager { get; }

        public LogoutModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;

        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostLogoutAsync()
        {
            var email = HttpContext.Session.GetString("6");
            ApplicationUser user = await userManager.FindByEmailAsync(email);
            user.AnotherBrowser = "false";
            var update = await userManager.UpdateAsync(user);
            if (update != null)
            {
                await signInManager.SignOutAsync();
                HttpContext.Session.Clear();
            }

           


            return RedirectToPage("Login");


        }
        public async Task<IActionResult> OnPostDontLogoutAsync()
        {
            return RedirectToPage("Index");
        }
    }
}
