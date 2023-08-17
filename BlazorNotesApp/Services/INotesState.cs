using BlazorNotesApp.Model;

namespace BlazorNotesApp.Services
{
    public interface INotesState
    {
        List<Note> Notes { get; }

        event Action OnChange;

        void AddNote(Note note);
        void RemoveNote(Note note);
        void SetNote(Note note);
        void SetNotes(List<Note> notes);
    }
}