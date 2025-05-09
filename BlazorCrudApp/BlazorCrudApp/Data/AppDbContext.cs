using Microsoft.EntityFrameworkCore;
using BlazorCrudApp.Shared.Models;

namespace BlazorCrudApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
    }
}
