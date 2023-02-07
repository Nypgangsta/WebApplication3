using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Model;
using WebApplication3.ViewModels;
using WebApplication3.Core;
using WebApplication3.Service;

namespace WebApplication3.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Login LModel { get; set; }
        private readonly SignInManager<ApplicationUser> signInManager;
        private UserManager<ApplicationUser> userManager { get; }


        private readonly GoogleCaptchaService _captchaService;
        public LoginModel(SignInManager<ApplicationUser> signInManager, GoogleCaptchaService captchaService, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            _captchaService = captchaService;
            this.userManager = userManager;

        }

        public IActionResult OnGet()
        {
            HttpContext.Session.SetString("code", "smth");
            if (Request.Cookies["FailedLoginAttempts"] != null)
            {
                ViewData["isAccountLocked"] = "Your account has been locked for 1 mintues";
            }

            if (HttpContext.Session.GetString("1") != null)
            {
                return RedirectToPage("Index");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (Request.Cookies["FailedLoginAttempts"] != null)
            {
                var checkAttempts = Request.Cookies["FailedLoginAttempts"];
                if (Int32.Parse(checkAttempts!) == 3)
                {
                    ViewData["isAccountLocked"] = "Your account has been locked for 1 minute";
                    return Page();
                }
            }
            var captchaResult = await _captchaService.VerifyToken(LModel.Token);
            if (!captchaResult)
            {
                return Page();
            }


            if (ModelState.IsValid)
            {
                //var checkatt = Request.Cookies[FailedLoginAttempts]

                var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password,
                LModel.RememberMe, false);
                if (identityResult.Succeeded)
                {
                    Response.Cookies.Delete("FailedLoginAttempts");
                    await signInManager.SignOutAsync();
                    ApplicationUser user = await userManager.FindByEmailAsync(LModel.Email);
                    if (user.AnotherBrowser != "false")
                    {

                        ModelState.AddModelError("", "Your account is logged in from another browser");
                        return Page();
                    }
                    user.AnotherBrowser = "true";
                    var update = await userManager.UpdateAsync(user);



                    int num = new Random().Next(100000, 999999);
                    HttpContext.Session.SetString("code", num.ToString());
                    HttpContext.Session.SetString("email", LModel.Email);
                    HttpContext.Session.SetString("pw", LModel.Password);
                    HttpContext.Session.SetString("remember", LModel.RememberMe.ToString());




                    string subject = "Here is your OTP for verification. Do not share it with others.";

                    var htmlMessage = Email.HtmlMessage(num.ToString(), "");
                    Email.SendMail(LModel.Email, subject, htmlMessage);

                    return RedirectToPage("LoginOTP");
                }
                if (Request.Cookies["FailedLoginAttempts"] != null)
                {
                    var opt = new CookieOptions();
                    opt.Expires = DateTime.Now.AddMinutes(1);
                    int getCookie = int.Parse(Request.Cookies["FailedLoginAttempts"]!.ToString());
                    int updateAttempt = getCookie + 1;
                    Response.Cookies.Delete("FailedLoginAttempts");
                    Response.Cookies.Append("FailedLoginAttempts", updateAttempt.ToString(), opt);
                }
                else
                {
                    var optCookie = new CookieOptions();
                    optCookie.Expires = DateTime.Now.AddMinutes(1);
                    Response.Cookies.Append("FailedLoginAttempts", "1", optCookie);
                }
                ModelState.AddModelError("", "Username or Password incorrect");
            }
            return Page();
        }
    }
}
