using BlazorNotesApp.Model;
using BlazorNotesApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazorNotesApp.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public User User { get; set; }
        public void OnPost([FromServices] IDatabaseManager db)
        {
            db.RegisterUser(User);
        }
    }
}
