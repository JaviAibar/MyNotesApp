using BlazorNotesApp.Model;

namespace BlazorNotesApp.Services
{
    public class NotesState : INotesState
    {
        public List<Note> Notes { get; private set; } = new List<Note>();

        public event Action OnChange;

        public void SetNotes(List<Note> notes) // Read
        {
            Notes = notes;
            NotifyStateChanged();
        }

        public void SetNote(Note note) // Update
        {
            int? noteIndex = SearchNote(note);
            if (noteIndex == null) return;

            Notes[noteIndex.Value].Name = note.Name;
            Notes[noteIndex.Value].Content = note.Content;
            NotifyStateChanged();
        }

        public void RemoveNote(Note note) // Delete
        {
            int? noteIndex = SearchNote(note);
            if (noteIndex == null) return;

            Notes.Remove(Notes[noteIndex.Value]);
            NotifyStateChanged();
        }

        public void AddNote(Note note) // Create
        {
            Notes.Add(note);
            NotifyStateChanged();
        }

        private int? SearchNote(Note note)
        {
            for (int i = 0; i < Notes.Count; i++)
            {
                if (Notes[i].Id != note.Id) continue;
                return i;
            }
            return null;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
