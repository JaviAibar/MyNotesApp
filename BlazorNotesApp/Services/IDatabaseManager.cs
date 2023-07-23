using BlazorNotesApp.Model;

namespace BlazorNotesApp.Services
{
	public interface IDatabaseManager
	{
		void CreateDatabase();
		int CreateNewNote(Note note, int userId);
		void DeleteNote(int noteId);
		Note GetNoteById(int noteId);
		IEnumerable<Note> GetNotesFromUser(int id);
        User GetUser(string name, string pass);
        User RegisterUser(User user);
		void UpdateNote(Note note);
	}
}