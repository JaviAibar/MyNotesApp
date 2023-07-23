namespace BlazorNotesApp.Model
{
	public class Note
	{
        public int? Id { get; set; }	
        public string? Name { get; set; }	
        public string? Content { get; set; }

        public Note()
        {
        }

		public Note(int? id, string name, string content)
		{
			Id = id;
			Name = name;
			Content = content;
		}

		public Note(string name, string content)
		{
			Name = name;
			Content = content;
		}
	}
}
