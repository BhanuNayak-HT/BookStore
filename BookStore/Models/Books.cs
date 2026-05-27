using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Books
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public string? Isbn { get; set; }

    }
}
