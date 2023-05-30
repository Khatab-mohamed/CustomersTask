using Microsoft.EntityFrameworkCore;

namespace CustomersManagementSystem.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
        : base(dbContextOptions)
        {
         /*   var x = Database.CanConnect();
            if (!x)
            {
                Database.Migrate();
            }
         */

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
    }
}
