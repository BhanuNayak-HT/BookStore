using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    // Represents a book in the catalog.
    public class Books
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }

    }
}
