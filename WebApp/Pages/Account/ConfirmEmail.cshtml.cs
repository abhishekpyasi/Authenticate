using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        public ConfirmEmailModel(UserManager<IdentityUser> userManager )
        {
            UserManager = userManager;
        }

        [BindProperty]
        public string message { get; set; }
        
        public UserManager<IdentityUser> UserManager { get; }
        public string Message { get => message; set => message = value; }

        public async Task<IActionResult> OnGetAsync(string userId, string token)
        {

            var user = await this.UserManager.FindByIdAsync(userId);

            if (user!= null)
            {

                var result = await this.UserManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {

                    this.message = "Email Address successfully verified";
                    return Page();
                }

            }

            this.Message = "Failed to validate e mail";
            return Page();

                }
    }
}
