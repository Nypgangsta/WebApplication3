using System.Security.Cryptography;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Model;
using WebApplication3.ViewModels;
using WebApplication3.Service;
using System.Reflection;
using System.Text.RegularExpressions;

namespace WebApplication3.Pages
{
    public class RegisterModel : PageModel
    {

        private UserManager<ApplicationUser> userManager { get; }

        [BindProperty]
        public Register RModel { get; set; }

        private IWebHostEnvironment _environment;

        public RegisterModel(UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
        {
            this.userManager = userManager;
            _environment = environment;
        }
        

       



        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {
            
            if (ModelState.IsValid)
            {
                var err = "";
                var check = false;
                var hasNumber = new Regex(@"[0-9]+");
                var hasUpperChar = new Regex(@"[A-Z]+");
                var hasLowerChar = new Regex(@"[a-z]+");
                var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

                if (!hasLowerChar.IsMatch(RModel.Password))
                {
                    err = "Password needs at least small caps";
                    ModelState.AddModelError("", err);
                    check = true;
                }
                if (!hasUpperChar.IsMatch(RModel.Password))
                {
                    err = "Passwords needs at least upper caps";
                    ModelState.AddModelError("", err);
                    check = true;

                }
                if (!hasNumber.IsMatch(RModel.Password))
                {
                    err = "Password needs at least one number";

                    ModelState.AddModelError("", err);
                    check = true;


                }

                if (!hasSymbols.IsMatch(RModel.Password))
                {
                    err = "Password needs at least one special character";
                    ModelState.AddModelError("", err);
                    check = true;

                }
                if (check == true)
                {

                    return Page();
                }

                var ImageURL = "";


                if (RModel.PhotoPath != null)
                {
                    if (RModel.PhotoPath.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload",
                        "File size cannot exceed 2MB.");
                        return Page();
                    }
                    var Upload = RModel.PhotoPath;
                    var uploadsFolder = "uploads";
                    var imageFile = Guid.NewGuid() + Path.GetExtension(
                    Upload.FileName);
                    var imagePath = Path.Combine(_environment.ContentRootPath,
                    "wwwroot", uploadsFolder, imageFile);
                    using var fileStream = new FileStream(imagePath,
                    FileMode.Create);
                    await Upload.CopyToAsync(fileStream);
                    ImageURL = string.Format("/{0}/{1}", uploadsFolder,
                    imageFile);
                }
                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");


                string wwwRootPath = _environment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(RModel.PhotoPath.FileName);
                string extention = Path.GetExtension(RModel.PhotoPath.FileName);
                if (extention != ".jpg")
                {
                    ViewData["1"] = "Photo should be in jpg";
                    return Page();
                }
                var checker = await userManager.FindByEmailAsync(RModel.Email);
                if (checker != null)
                {
                    ModelState.AddModelError("", "Email already registered");
                    return Page();

                }

                    
                int num = new Random().Next(100000, 999999);
                HttpContext.Session.SetString("code", num.ToString());
                HttpContext.Session.SetString("registering3", RModel.Email);
                HttpContext.Session.SetString("registering4", protector.Protect(RModel.CreditCardNo));
                HttpContext.Session.SetString("registering5", RModel.Gender);
                HttpContext.Session.SetString("registering6", RModel.DeliveryAddress);
                HttpContext.Session.SetString("registering7", ImageURL.ToString());
                HttpContext.Session.SetString("registering8", RModel.FullName);
                HttpContext.Session.SetString("registering9", RModel.AboutMe);
                HttpContext.Session.SetString("registering10", RModel.Password);
                HttpContext.Session.SetString("registering11", RModel.PhoneNumber);




                string subject = "Congratulations! Your account has been registered.";
                var htmlMessage = Email.HtmlMessage(num.ToString(), RModel.FullName);
                Email.SendMail(RModel.Email, subject, htmlMessage);
                return RedirectToPage("./Email2FA");
                
                //foreach (var error in result.Errors)
                //{
                 //   ModelState.AddModelError("", error.Description);
                //}
            }
            return Page();
        }







    }
}
