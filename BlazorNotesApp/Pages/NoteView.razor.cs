using BlazorNotesApp.Model;
using BlazorNotesApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;


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
        [Inject] public INotesState NotesState { get; set; }
        [Inject] public IDatabaseManager db { get; set; }
        [Inject] public IDialogService dialog { get; set; }
        [Inject] public NavigationManager navManager { get; set; }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            base.SetParametersAsync(parameters);
            var user = (await authenticationStateTask).User;

            if (!user.Identity.IsAuthenticated)
            {
                navManager.NavigateTo($"/login?ReturnUrl={Uri.EscapeDataString(navManager.Uri)}", true);
                return;
            }

            UserId = int.Parse(user.FindFirst("id").Value);

            if (NoteId == 0)
            {
                Note note = new Note();
                Name = note.Name;
                Content = note.Content;
                NoteId = db.CreateNewNote(note, UserId);
                note.Id = NoteId;
                NotesState.AddNote(note);
                navManager.NavigateTo("/Note/" + NoteId.ToString());
            }
            else
            {
                Note note = db.GetNoteById(NoteId);
                Name = note.Name;
                Content = note.Content;
            }
            if (Timer == null)
                Timer = new System.Timers.Timer(2000);

            Timer.Elapsed -= (_, _) => SaveNote();
            Timer.Elapsed += (_, _) => SaveNote();
            Timer.Start();
        }

        public void SaveNote()
        {
            Note note = new Note(NoteId, Name, Content);
            db.UpdateNote(note);
            NotesState.SetNote(note);
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
            Note note = new Note(NoteId, Name, Content);
            db.DeleteNote(NoteId);
            NotesState.RemoveNote(note);
            navManager.NavigateTo("/");
        }
    }
}