using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Models
{
    public class BookStoreDBContext : IdentityDbContext<ApplicationUser>
    {
        public BookStoreDBContext(DbContextOptions<BookStoreDBContext> options): base(options) {
        
        }
        public DbSet<Books> books { get; set; }
        public DbSet<Author> author { get; set; }
    }
}
//(localdb)\MSSQLLocalDB
//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BookStore;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False
//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BookStore;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False