using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommonDrawing.Models;
using CommonDrawing.Services;

namespace CommonDrawing.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IPrincipalProvider _principalProvider;

        [BindProperty]
        public string UserName { get; set; }

        public LoginModel(IPrincipalProvider principalProvider)
        {
            _principalProvider = principalProvider;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            User user = new(UserName);

            var principal = _principalProvider.GeneratePrincipal(user);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToPage(nameof(Index));
        }
    }
}
