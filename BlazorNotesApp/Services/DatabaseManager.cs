using BlazorNotesApp.Model;
using Dapper;
using Microsoft.Data.Sqlite;

namespace BlazorNotesApp.Services
{
    public class DatabaseManager : IDatabaseManager
    {
        private SqliteConnection dbConnection = new SqliteConnection("Data Source=NotesDatabase.db");
        public DatabaseManager()
        {
            dbConnection.Open();
        }


        public void CreateDatabase()
        {
            dbConnection.Execute("CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Pass TEXT, Email TEXT)");
            dbConnection.Execute("CREATE TABLE IF NOT EXISTS Notes (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Content TEXT, UserId INTEGER,FOREIGN KEY(UserId) REFERENCES Users(Id))");
        }

        public int CreateNewNote(Note note, int userId)
        {
            int id = dbConnection.QuerySingle<int>(@"INSERT INTO Notes(Name, Content, UserId) Values(@name, @content, @userId); SELECT last_insert_rowid();", new
            {
                name = note.Name,
                content = note.Content,
                userId
            });
            return id;
        }

        public void DeleteNote(int noteId)
        {
            dbConnection.Execute("DELETE FROM Notes WHERE id=@noteId;", new
            {
                noteId
            });

        }

        public Note GetNoteById(int noteId)
        {
            return dbConnection.QueryFirstOrDefault<Note>(@"SELECT * FROM Notes Where Id=@noteId", new
            {
                noteId,
            });
        }

        public IEnumerable<Note> GetNotesFromUser(int id)
        {
            return dbConnection.Query<Note>(@"SELECT * FROM Notes Where UserId=@id", new
            {
                id,
            });
        }

        public User GetUser(string name, string pass)
        {
            return dbConnection.QueryFirstOrDefault<User>(@"SELECT * FROM Users Where Name=@name and Pass=@pass", new
            {
                name,
                pass
            });
        }

        public User RegisterUser(User user)
        {
            int id = dbConnection.QuerySingle<int>(@"INSERT INTO Users(Name, Pass, Email) Values(@name, @pass, @email); SELECT last_insert_rowid();", new
            {
                name = user.Name,
                pass = user.Pass,
                email = user.Email
            });
            user.Id = id;
            return user;
        }

        public void UpdateNote(Note note)
        {
            dbConnection.Execute(@"UPDATE Notes SET Name=@name, Content=@content WHERE Id=@noteId", new
            {
                name = note.Name,
                content = note.Content,
                noteId = note.Id
            });
        }
    }
}
