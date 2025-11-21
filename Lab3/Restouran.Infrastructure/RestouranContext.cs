using Microsoft.EntityFrameworkCore;
using Restouran.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Restouran.Infrastructure
{
    public class RestouranContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MainDish> MainDishes { get; set; }
        public DbSet<Dessert> Desserts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<MenuItemTag> MenuItemTags { get; set; }

        public RestouranContext()
        {
        }

        public RestouranContext(DbContextOptions<RestouranContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MenuItem>()
                .HasKey(m => m.IdItem);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.order)
                .WithOne(o => o.customer)
                .HasForeignKey<Order>(o => o.CustomerId);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.MenuItem) 
                .WithOne(mi => mi.order)  
                .HasForeignKey(mi => mi.OrderId); 

            modelBuilder.Entity<MenuItemTag>()
                .HasKey(mt => new { mt.MenuItemId, mt.TagId });

            modelBuilder.Entity<MenuItemTag>()
                .HasOne(mt => mt.MenuItem) 
                .WithMany(m => m.MenuItemTags) 
                .HasForeignKey(mt => mt.MenuItemId); 


            modelBuilder.Entity<MenuItemTag>()
                .HasOne(mt => mt.Tag) 
                .WithMany(t => t.MenuItemTags)
                .HasForeignKey(mt => mt.TagId);


            modelBuilder.Entity<MenuItem>().ToTable("MenuItems");
            modelBuilder.Entity<MainDish>().ToTable("MainDishes"); //
            modelBuilder.Entity<Dessert>().ToTable("Desserts");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=restouran.db");
            }
        }

    }
}
