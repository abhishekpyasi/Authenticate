using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Services;

namespace WebApp.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IEmailService emailService;

        public RegisterModel(UserManager<IdentityUser> userManager , IEmailService emailService)
        {
            UserManager = userManager;
            this.emailService = emailService;
        }

        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; }
        public UserManager<IdentityUser> UserManager { get; }

        public void OnGet()

        {
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid) return Page();

            // validate e mail address


            var user = new IdentityUser
            {

                Email = RegisterViewModel.Email,
                UserName = RegisterViewModel.Email
            };
            // create user

           var result =await this.UserManager.CreateAsync(user, RegisterViewModel.Password);
            if (result.Succeeded)
            {

                var confirmationToken = await this.UserManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmationLink = Url.PageLink(pageName: "/Account/ConfirmEmail", values: new { userId = user.Id ,token = confirmationToken});

                await emailService.sendAsync("pyasi.abhishek@gmail.com", user.Email, "Please confirm your email",
                    $"please click on link to confirm email: {confirmationLink}");

                /*using (var emailClient = new SmtpClient("smtp-relay.sendinblue.com", 587))
                {

                    emailClient.Credentials = new NetworkCredential
                        ("pyasi.abhishek@gmail.com", "Ym6O3IChVbkLGxRB");
                    await emailClient.SendMailAsync(message)*//*;
                }
*/

               return RedirectToPage("/Account/Login");
            }

            else
            {

                foreach(var error in result.Errors )
                {

                    ModelState.AddModelError("Register",error.Description );
                }

                return Page();
            }
        }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage ="Invalid Email")]
        public string Email { get; set; }
        [Required]
        [DataType(dataType:DataType.Password)]
        public string Password { get; set; }
    }
}
