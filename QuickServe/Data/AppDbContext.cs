using Microsoft.EntityFrameworkCore;
using QuickServe.Models;

namespace QuickServe.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Menu ↔ OrderItem: One-to-Many relationship
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Menu)
                .WithMany(m => m.OrderItems)
                .HasForeignKey(oi => oi.MenuID);

            // Menu ↔ CartItem: One-to-Many relationship
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Menu)
                .WithMany(m => m.CartItems)
                .HasForeignKey(ci => ci.MenuID);

            // User ↔ Order: One-to-Many relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserID);

            // User ↔ Cart: One-to-One relationship
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserID);

            // User ↔ Rating: One-to-Many relationship
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserID);

            // Restaurant ↔ Rating: One-to-Many relationship
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Restaurant)
                .WithMany(res => res.Ratings)
                .HasForeignKey(r => r.RestaurantID);

            // Order ↔ Payment: One-to-One relationship
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderID);

            // Order ↔ OrderItem: One-to-Many relationship
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderID)
                .OnDelete(DeleteBehavior.NoAction);

            // Cart ↔ CartItem: One-to-Many relationship
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartID)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete for CartItems

            // Set precision for decimal properties
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

            // Add Indexes for frequently queried fields (e.g., UserID in Carts)
            modelBuilder.Entity<Cart>()
                .HasIndex(c => c.UserID)
                .IsUnique(); // Enforcing one active cart per user

            modelBuilder.Entity<CartItem>()
                .HasIndex(ci => ci.CartID);

            modelBuilder.Entity<OrderItem>()
                .HasIndex(oi => oi.OrderID);

            base.OnModelCreating(modelBuilder);

        }
    }
}
