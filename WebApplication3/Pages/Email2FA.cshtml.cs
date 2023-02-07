using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WebApplication3.Model;
using WebApplication3.ViewModels;

namespace WebApplication3.Pages
{
    public class Email2FAModel : PageModel
    {
        private UserManager<ApplicationUser> userManager { get; }
        private SignInManager<ApplicationUser> signInManager { get; }
        private readonly RoleManager<IdentityRole> roleManager;

        [BindProperty]
        public Register RModel { get; set; }
        public Email2FAModel(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPost(string code)
        {
            var getSessionCode = HttpContext.Session.GetString("code");
            var getSessionEmail = HttpContext.Session.GetString("registering3");
            var credit = HttpContext.Session.GetString("registering4");
            var gender = HttpContext.Session.GetString("registering5");
            var deliver = HttpContext.Session.GetString("registering6");
            var address = HttpContext.Session.GetString("registering7");
            var name = HttpContext.Session.GetString("registering8");
            var about = HttpContext.Session.GetString("registering9");
            var pw = HttpContext.Session.GetString("registering10");
            var phone = HttpContext.Session.GetString("registering11");

            if (getSessionCode == code)
            {
                var user = new ApplicationUser()
                {
                    UserName = getSessionEmail,
                    FullName = name,
                    Email = getSessionEmail,
                    PhoneNumber = phone,
                    CreditCard = credit,
                    Gender = gender,
                    DeliveryAddress = deliver,
                    PhotoPath = address,
                    AboutMe = about,
                    AnotherBrowser = "true",

                };
                IdentityRole role = await roleManager.FindByIdAsync("Admin");
                if (role == null)
                {
                    IdentityResult result2 = await roleManager.CreateAsync(new IdentityRole("Admin"));
                    if (!result2.Succeeded)
                    {
                        ModelState.AddModelError("", "Create role admin failed");
                    }
                }

                var result = await userManager.CreateAsync(user, pw);
                if (result.Succeeded)
                {
                    HttpContext.Session.Clear();
                    result = await userManager.AddToRoleAsync(user, "Admin");
                    await signInManager.SignInAsync(user, false);

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
