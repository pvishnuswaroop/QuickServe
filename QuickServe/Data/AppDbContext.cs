using Microsoft.EntityFrameworkCore;
using QuickServe.Models;

namespace QuickServe.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets for all models
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RefreshToken>().ToTable("RefreshTokens");

            modelBuilder.Entity<User>()
            .Property(u => u.Roles)
            .IsRequired(false); // Mark the Roles collection as optional

            modelBuilder.Entity<RefreshToken>()
               .HasIndex(rt => rt.Username);

            // Decimal precision configuration for monetary values
            modelBuilder.Entity<Menu>()
                .Property(m => m.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.AmountPaid)
                .HasColumnType("decimal(18,2)");

            // Users
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique(); // Email must be unique
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserID)
                .OnDelete(DeleteBehavior.Restrict);  // Changed to Restrict
            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserID)
                .OnDelete(DeleteBehavior.Restrict); // Changed to Restrict
            modelBuilder.Entity<User>()
                .HasMany(u => u.Ratings)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Restrict); // Changed to Restrict

            // Restaurants
            modelBuilder.Entity<Restaurant>()
                .HasMany(r => r.Menus)
                .WithOne(m => m.Restaurant)
                .HasForeignKey(m => m.RestaurantID)
                .OnDelete(DeleteBehavior.Restrict);  // Changed to Restrict
            modelBuilder.Entity<Restaurant>()
                .HasMany(r => r.Orders)
                .WithOne(o => o.Restaurant)
                .HasForeignKey(o => o.RestaurantID)
                .OnDelete(DeleteBehavior.Restrict); // Changed to Restrict
            modelBuilder.Entity<Restaurant>()
                .HasMany(r => r.Ratings)
                .WithOne(rt => rt.Restaurant)
                .HasForeignKey(rt => rt.RestaurantID)
                .OnDelete(DeleteBehavior.Restrict); // Changed to Restrict

            // Menus
            modelBuilder.Entity<Menu>()
                .HasMany(m => m.OrderItems)
                .WithOne(oi => oi.Menu)
                .HasForeignKey(oi => oi.MenuID)
                .OnDelete(DeleteBehavior.Restrict); // Changed to Restrict
            modelBuilder.Entity<Menu>()
                .HasMany(m => m.CartItems)
                .WithOne(ci => ci.Menu)
                .HasForeignKey(ci => ci.MenuID)
                .OnDelete(DeleteBehavior.Restrict); // Changed to Restrict

            // Carts
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartID)
                .OnDelete(DeleteBehavior.Cascade);

            // Orders
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderID)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            // Ratings
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Order)
                .WithMany()
                .HasForeignKey(r => r.OrderID)
                .OnDelete(DeleteBehavior.SetNull);

            // Payments
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Restrict); // Changed to Restrict
        }

    }
}

