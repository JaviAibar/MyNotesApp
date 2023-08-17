using BlazorNotesApp.Model;
using BlazorNotesApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BlazorNotesApp.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty] public string Name { get; set; }
        [BindProperty] public string Pass { get; set; }
        [FromQuery] public string? ReturnUrl { get; set; }

        public async Task<IActionResult> OnPost([FromServices] IDatabaseManager db)
        {
            User user = db.GetUser(Name, Pass);
            if (user == null)
            {
                ModelState.AddModelError("User not existing", "User doesn't exist");
                return Page();
            }

            ClaimsIdentity claimId = new ClaimsIdentity(new Claim[]
            {
	// Esto es la info que queremos poder acceder del usuario en el resto de la app
	            new Claim(ClaimTypes.Name, Name),
                new Claim(ClaimTypes.Role, "miusuario"),
                new Claim("id", user.Id.ToString()),
                new Claim("BailaSamba", "true")
            }, "RegularLogin"); // Este string define de qué forma se ha logueado, es obligatorio para que se considere logueado de forma efectiva
            await HttpContext.SignInAsync(/*"loginScheme", */new ClaimsPrincipal(claimId), new AuthenticationProperties()
            );

            return Redirect(ReturnUrl ?? "/");
        }
    }
}
