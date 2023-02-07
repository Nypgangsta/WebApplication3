using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Model;
using WebApplication3.Service;

namespace WebApplication3.Pages
{
    public class ForgetPwModel : PageModel
    {
        private UserManager<ApplicationUser> userManager { get; }
        public ForgetPwModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("1") != null)
            {
                return RedirectToPage("Index");
            }
            return Page();

        }
        public async Task<IActionResult> OnPost(string email)
        {
            if (email == null)
            {
                return Page();
            }
            var checker = await userManager.FindByEmailAsync(email);
            if (checker != null)
            {
                int num = new Random().Next(100000, 999999);
                HttpContext.Session.SetString("code", num.ToString());
                HttpContext.Session.SetString("find", email);
                string subject = "Your OTP to reset your password.";
                var htmlMessage = Email.HtmlMessage(num.ToString(), "");
                Email.SendMail(email, subject, htmlMessage);
                return RedirectToPage("./ForgetVerification");
            }

            return Page();
        }

    }
}
