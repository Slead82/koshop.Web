using KoShop.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace KoShop.Web.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Core e-commerce tables
    public DbSet<User> Users => Set<User>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OrderStatusHistory> OrderStatusHistories => Set<OrderStatusHistory>();
    public DbSet<Banner> Banners => Set<Banner>();
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
    public DbSet<TicketMessage> TicketMessages => Set<TicketMessage>();
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
    public DbSet<Review> Reviews => Set<Review>();

    // Page-editor tables (Tbl_Setting, Tbl_Home, ...)
    public DbSet<SiteSetting> SiteSettings => Set<SiteSetting>();
    public DbSet<HomePageContent> HomePageContents => Set<HomePageContent>();
    public DbSet<AboutPageContent> AboutPageContents => Set<AboutPageContent>();
    public DbSet<ContactPageContent> ContactPageContents => Set<ContactPageContent>();
    public DbSet<FaqPageContent> FaqPageContents => Set<FaqPageContent>();
    public DbSet<TermsPageContent> TermsPageContents => Set<TermsPageContent>();
    public DbSet<PrivacyPageContent> PrivacyPageContents => Set<PrivacyPageContent>();
    public DbSet<ShopPageContent> ShopPageContents => Set<ShopPageContent>();
    public DbSet<SubmitOrderPageContent> SubmitOrderPageContents => Set<SubmitOrderPageContent>();
    public DbSet<TrackOrderPageContent> TrackOrderPageContents => Set<TrackOrderPageContent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ---------- Users ----------
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("Users");
            e.HasIndex(u => u.Username).IsUnique();
            e.HasIndex(u => u.Email).IsUnique();
            e.HasIndex(u => u.PhoneNumber).IsUnique();
        });

        // ---------- Addresses ----------
        modelBuilder.Entity<Address>(e =>
        {
            e.ToTable("Addresses");
            e.HasOne(a => a.User)
             .WithMany(u => u.Addresses)
             .HasForeignKey(a => a.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ---------- Categories (self-referencing) ----------
        modelBuilder.Entity<Category>(e =>
        {
            e.ToTable("Categories");
            e.HasOne(c => c.Parent)
             .WithMany(c => c.Children)
             .HasForeignKey(c => c.ParentId)
             .OnDelete(DeleteBehavior.Restrict); // avoid multiple-cascade-paths error in SQL Server
        });

        // ---------- Products ----------
        modelBuilder.Entity<Product>(e =>
        {
            e.ToTable("Products");
            e.HasOne(p => p.Category)
             .WithMany(c => c.Products)
             .HasForeignKey(p => p.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProductImage>(e =>
        {
            e.ToTable("ProductImages");
            e.HasOne(pi => pi.Product)
             .WithMany(p => p.Images)
             .HasForeignKey(pi => pi.ProductId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ---------- Orders ----------
        modelBuilder.Entity<Order>(e =>
        {
            e.ToTable("Orders");
            e.HasIndex(o => o.OrderCode).IsUnique();
            e.HasOne(o => o.User)
             .WithMany(u => u.Orders)
             .HasForeignKey(o => o.UserId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderItem>(e =>
        {
            e.ToTable("OrderItems");
            e.HasOne(oi => oi.Order)
             .WithMany(o => o.Items)
             .HasForeignKey(oi => oi.OrderId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(oi => oi.Product)
             .WithMany()
             .HasForeignKey(oi => oi.ProductId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<OrderStatusHistory>(e =>
        {
            e.ToTable("OrderStatusHistory");
            e.HasOne(h => h.Order)
             .WithMany(o => o.StatusHistory)
             .HasForeignKey(h => h.OrderId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(h => h.ChangedByAdmin)
             .WithMany()
             .HasForeignKey(h => h.ChangedByAdminId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ---------- Banners ----------
        modelBuilder.Entity<Banner>().ToTable("Banners");

        // ---------- Support tickets ----------
        modelBuilder.Entity<SupportTicket>(e =>
        {
            e.ToTable("SupportTickets");
            e.HasIndex(t => t.TicketCode).IsUnique();
            e.HasOne(t => t.User)
             .WithMany()
             .HasForeignKey(t => t.UserId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TicketMessage>(e =>
        {
            e.ToTable("TicketMessages");
            e.HasOne(m => m.Ticket)
             .WithMany(t => t.Messages)
             .HasForeignKey(m => m.TicketId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(m => m.Sender)
             .WithMany()
             .HasForeignKey(m => m.SenderId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ---------- Blog ----------
        modelBuilder.Entity<BlogPost>(e =>
        {
            e.ToTable("BlogPosts");
            e.HasOne(b => b.AuthorAdmin)
             .WithMany()
             .HasForeignKey(b => b.AuthorAdminId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ---------- Reviews ----------
        modelBuilder.Entity<Review>(e =>
        {
            e.ToTable("Reviews");
            e.HasOne(r => r.Product)
             .WithMany(p => p.Reviews)
             .HasForeignKey(r => r.ProductId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(r => r.User)
             .WithMany()
             .HasForeignKey(r => r.UserId)
             .OnDelete(DeleteBehavior.SetNull);
            e.HasOne(r => r.ModeratedByAdmin)
             .WithMany()
             .HasForeignKey(r => r.ModeratedByAdminId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
