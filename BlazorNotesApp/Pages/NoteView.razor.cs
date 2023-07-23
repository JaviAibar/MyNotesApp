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
using Microsoft.AspNetCore.Mvc;
using BlazorNotesApp.Services;
using BlazorNotesApp.Model;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;


namespace BlazorNotesApp.Pages
{
	public partial class NoteView
	{
		[CascadingParameter] Task<AuthenticationState> authenticationStateTask { get; set; }

		public int UserId { get; set; }
		[Parameter]
		public int NoteId { get; set; }

		public string? Name { get; set; }

		public string? Content { get; set; }
		private System.Timers.Timer? Timer { get; set; }

		[Inject] public IDatabaseManager db { get; set; }
		[Inject] public IDialogService dialog { get; set; }
		[Inject] public NavigationManager navManager { get; set; }

		public override async Task SetParametersAsync(ParameterView parameters)
		{
			base.SetParametersAsync(parameters);
			var user = (await authenticationStateTask).User;

			if (!user.Identity.IsAuthenticated) return;
			UserId = int.Parse(user.FindFirst("id").Value);

			if (NoteId == 0)
			{
				NoteId = db.CreateNewNote(new Note(), UserId);
				// TODO: Add to Notes list in NotesList view
			}
			else
			{
				Note note = db.GetNoteById(NoteId);
				Name = note.Name;
				Content = note.Content;
			}
			if (Timer == null)
			{
				Timer = new System.Timers.Timer(2000);
			}
			Timer.Elapsed -= OnSaveNote;
			Timer.Elapsed += OnSaveNote;
			Timer.Start();

		}
		private void OnSaveNote(Object source, System.Timers.ElapsedEventArgs e)
		{
            Console.WriteLine("El tiempo a pasado, se intenta guardar");
            SaveNote();
		}

		public void SaveNote()
		{
			db.UpdateNote(new Note(NoteId, Name, Content));
		}

		public async void OpenDeleteModal()
		{
			bool? result = await dialog.ShowMessageBox(
		   "Warning",
		   "Deleting can not be undone!",
		   yesText: "Delete!", cancelText: "Cancel");
			if (result == null) return;
			DeleteNote();
			StateHasChanged();
		}

		public void DeleteNote()
		{
			db.DeleteNote(NoteId);
			navManager.NavigateTo("/");
		}
	}
}