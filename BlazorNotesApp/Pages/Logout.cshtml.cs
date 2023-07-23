using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BlazorNotesApp.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            LogOut();
            return Redirect("/");
        }

        public async Task LogOut()
        {
            await HttpContext.SignOutAsync("loginScheme");
        }
    }
}
