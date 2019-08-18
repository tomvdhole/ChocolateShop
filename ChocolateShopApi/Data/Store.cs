using Microsoft.EntityFrameworkCore;
using ChocolateShopApi.Models;

namespace ChocolateShopApi.Data
{
    public class Store : DbContext
    {
        public Store (DbContextOptions<Store> options)
            : base(options){}

        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}

