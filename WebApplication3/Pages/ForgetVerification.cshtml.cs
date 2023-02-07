using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Model;

namespace WebApplication3.Pages
{
    public class ForgetVerificationModel : PageModel
    {
        private UserManager<ApplicationUser> userManager { get; }

        public ForgetVerificationModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("1") != null)
            {
                return RedirectToPage("Index");
            }
            if (HttpContext.Session.GetString("code") == "smth")
            {
                return RedirectToPage("Login");
            }
            return Page();

        }
        public async Task<IActionResult> OnPost(string code)
        {
            var code1 = HttpContext.Session.GetString("code");
            var email = HttpContext.Session.GetString("find");


            if (code1 == code)
            {
                ApplicationUser user = await userManager.FindByEmailAsync(email);
                var result = await userManager.RemovePasswordAsync(user);
                if (result.Succeeded)
                {
                    result = await userManager.AddPasswordAsync(user, "m#P52s@ap$Vaa");
                    return RedirectToPage("ResetPassword");
                }




            }
            else
            {
                ViewData["isEmailConfirmed"] = "You have entered wrong verification";
            }
            return Page();
        }
    }
}
