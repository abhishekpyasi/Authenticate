using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        public LoginModel(SignInManager<IdentityUser> signInsManager)
        {
            SignInManager = signInsManager;
        }

        [BindProperty]
        public CredentialViewModel Credential { get; set; }
        public SignInManager<IdentityUser> SignInManager { get; }

        public void OnGet()
        {            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {

                return Page();
            }   

            var result = await 
                SignInManager.PasswordSignInAsync(this.Credential.Email, this.Credential.Password, 
                this.Credential.RememberMe, false);

            if(result.Succeeded)
            {

                return RedirectToPage("/Index");
            }

            else
            {

                if (result.IsLockedOut)
                {

                    ModelState.AddModelError("Login", "You are locked out");


                }

                else
                {
                    ModelState.AddModelError("Login", "failed to login");


                }

                return Page();
            }
        }
    }

    public class CredentialViewModel
    {
        [Required]        
        [Display(Name = "User Name")]
        public string Email { get; set; }

        [Required]        
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remeber Me")]
        public bool RememberMe { get; set; }
    }
}
