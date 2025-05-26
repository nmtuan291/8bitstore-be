using _8bitstore_be.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _8bitstore_be.Data
{
    public class _8bitstoreContext: IdentityDbContext<User>
    {
        public _8bitstoreContext(DbContextOptions<_8bitstoreContext> options): base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<PaymentVnPay> PaymentVnPays { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
