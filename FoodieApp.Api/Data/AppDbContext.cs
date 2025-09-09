using Microsoft.EntityFrameworkCore;
using FoodieApp.Api.Models;

namespace FoodieApp.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<Review> Reviews => Set<Review>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // pre-seed 4 kategorii
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Drinki" },
                new Category { Id = 2, Name = "Piwa" },
                new Category { Id = 3, Name = "Śniadania/Desery" },
                new Category { Id = 4, Name = "Obiady" }
            );

            // decimal precision for price
            modelBuilder.Entity<Item>().Property(i => i.Price).HasColumnType("decimal(10,2)");
        }
    }
}
