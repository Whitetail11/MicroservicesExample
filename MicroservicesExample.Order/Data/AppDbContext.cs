using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;

namespace MicroservicesExample.Order.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Entities.Product> Products { get; set; }
        public DbSet<Entities.Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Entities.Product>()
                .HasOne(e => e.Order)
                .WithMany(c => c.Products)
                .HasForeignKey(e => e.OrderId);
        }

    }
}
