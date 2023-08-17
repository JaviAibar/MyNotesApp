using BlazorNotesApp.Model;
using BlazorNotesApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorNotesApp.Pages
{
    public partial class NotesList
    {
        [CascadingParameter] Task<AuthenticationState> authenticationStateTask { get; set; }
        [Inject] public IDatabaseManager db { get; set; }
        [Inject] public INotesState NotesState { get; set; }
        [Parameter] public List<Note> Notes { get; set; } = new List<Note>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var user = (await authenticationStateTask).User;

            if (!user.Identity.IsAuthenticated) return;
            NotesState.OnChange += () => NotesState_OnChangeAsync();
            NotesState.SetNotes(db.GetNotesFromUser(int.Parse(user.FindFirst("id").Value)).ToList());
        }

        private async Task NotesState_OnChangeAsync()
        {
            await InvokeAsync(() =>
            {
                Notes = NotesState.Notes;
                StateHasChanged();
            });
        }
    }
}