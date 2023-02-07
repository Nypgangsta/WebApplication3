using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Model;
using WebApplication3.Service;

namespace WebApplication3.Pages
{
    public class ResetPasswordModel : PageModel
    {

        private UserManager<ApplicationUser> userManager { get; }

        public ResetPasswordModel(UserManager<ApplicationUser> userManager)
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
        public async Task<IActionResult> OnPost(string pw1,string pw2)
        {
            var err = "";
            var check = false;
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasLowerChar.IsMatch(pw1))
            {
                err = "Password needs at least small caps";
                ModelState.AddModelError("", err);
                check = true;
            }
            if (!hasUpperChar.IsMatch(pw1))
            {
                err = "Passwords needs at least upper caps";
                ModelState.AddModelError("", err);
                check = true;

            }
            if (!hasNumber.IsMatch(pw1))
            {
                err = "Password needs at least one number";
                ModelState.AddModelError("", err);
                check = true;


            }

            if (!hasSymbols.IsMatch(pw1))
            {
                err = "Password needs at least one special character";
                ModelState.AddModelError("", err);
                check = true;
            }
            if (check == true)
            {

                return Page();
            }
            var email = HttpContext.Session.GetString("find");

            ApplicationUser user = await userManager.FindByEmailAsync(email);
            var result = await userManager.RemovePasswordAsync(user);
            if (result.Succeeded)
            {
                HttpContext.Session.Clear();

                result = await userManager.AddPasswordAsync(user, pw1);
                //m#P52s@ap$Vaa
                return RedirectToPage("Login");
            }
            return Page();

        }
    }
}
