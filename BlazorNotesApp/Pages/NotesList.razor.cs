using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using BlazorNotesApp;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using BlazorNotesApp.Model;
using BlazorNotesApp.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorNotesApp.Pages
{
    public partial class NotesList
    {
		[CascadingParameter] Task<AuthenticationState> authenticationStateTask { get; set; }
		[Inject] public IDatabaseManager db { get; set; }
        [Parameter] public List<Note> Notes { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			var user = (await authenticationStateTask).User;

			if (!user.Identity.IsAuthenticated) return;

			Notes = db.GetNotesFromUser(int.Parse(user.FindFirst("id").Value)).ToList();
		}
	}
}