using Microsoft.EntityFrameworkCore;

namespace Bookstore.Audit
{
    public class BookstoreAuditContext : DbContext
    {
        public BookstoreAuditContext(DbContextOptions<BookstoreAuditContext> options) : base(options) { }
        public DbSet<UserAction> UserActions { get; set; }
    }
}