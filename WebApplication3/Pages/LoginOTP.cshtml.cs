using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Core;
using WebApplication3.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WebApplication3.ViewModels;
using System;

namespace WebApplication3.Pages
{
    public class LoginOTPModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        public LoginOTPModel(SignInManager<ApplicationUser> signInManager)
        {
            this.signInManager = signInManager;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost(string code)
        {
            var code1 = HttpContext.Session.GetString("code");
            var email = HttpContext.Session.GetString("email");
            var pw1 = HttpContext.Session.GetString("pw");


            if (code1 == code)
            {
                var identityResult = await signInManager.PasswordSignInAsync(email, pw1,
                false, false);
                if (identityResult.Succeeded)
                {
                    HttpContext.Session.Clear();
                    return RedirectToPage("Index");

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
