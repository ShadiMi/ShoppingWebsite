using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingWebsite.Areas.Identity.Data; // Ensure this using directive is correct
using ShoppingWebsite.Models;

namespace ShoppingWebsite.Data
{
    public class ApplicationDbContext : IdentityDbContext<ShoppingWebsiteUser> // Change here
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PaymentInfo> PaymentInfos { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Custom configurations remain the same
        }

    }
}
