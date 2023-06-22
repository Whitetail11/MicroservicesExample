using Microsoft.EntityFrameworkCore;

namespace MicroservicesExample.Product.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }
        public DbSet<Entities.Product> Products { get; set; }

    }
}
