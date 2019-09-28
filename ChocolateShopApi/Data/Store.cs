using Microsoft.EntityFrameworkCore;
using ChocolateShopApi.Models;

namespace ChocolateShopApi.Data
{
    public class Store : DbContext
    {
        public Store (DbContextOptions<Store> options)
            : base(options){}

        public DbSet<Brand> Brands { get; set;  }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BrandCategory>()
                 .HasKey(bc => new { bc.BrandId, bc.CategoryId });
            modelBuilder.Entity<BrandCategory>()
                .HasOne(bc => bc.Brand)
                .WithMany(b => b.BrandCategories)
                .HasForeignKey(bc => bc.BrandId);
            modelBuilder.Entity<BrandCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.BrandCategories)
                .HasForeignKey(bc => bc.CategoryId);

            modelBuilder.Entity<CategoryProduct>()
                .HasKey(cp => new { cp.CategoryId, cp.ProductId });
            modelBuilder.Entity<CategoryProduct>()
                .HasOne(cp => cp.Category)
                .WithMany(c => c.CategoryProducts)
                .HasForeignKey(cp => cp.CategoryId);
            modelBuilder.Entity<CategoryProduct>()
                .HasOne(cp => cp.Product)
                .WithMany(p => p.CategoryProducts)
                .HasForeignKey(cp => cp.ProductId);
        }
    }
}

