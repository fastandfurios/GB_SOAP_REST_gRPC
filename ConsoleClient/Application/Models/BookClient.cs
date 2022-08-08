namespace ConsoleClient.Application.Models
{
    public class BookClient
    {
        public string Title { get; set; }

        public string Category { get; set; }

        public string Lang { get; set; }

        public int Pages { get; set; }

        public int AgeLimit { get; set; }

        public AuthorClient[] Authors { get; set; }

        public int PublicationDate { get; set; }
    }
}
