using Microsoft.EntityFrameworkCore;
using BookingApp.Models;

namespace BookingApp.Data
{
    public class BookingContext : DbContext
    {
        public BookingContext(DbContextOptions<BookingContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Cabin> Cabins { get; set; }
        public DbSet<CabinPhoto> CabinPhotos { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<BookingService> BookingServices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Russian_CI_AS");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookingService>()
                .HasKey(bs => new { bs.BookingId, bs.ServiceId });

            modelBuilder.Entity<BookingService>()
                .HasOne(bs => bs.Booking)
                .WithMany(b => b.BookingServices)
                .HasForeignKey(bs => bs.BookingId);

            modelBuilder.Entity<BookingService>()
                .HasOne(bs => bs.Service)
                .WithMany(s => s.BookingServices)
                .HasForeignKey(bs => bs.ServiceId);

            modelBuilder.Entity<Cabin>()
                .Property(c => c.PricePerNight)
                .HasColumnType("decimal(10, 2)"); 

            modelBuilder.Entity<Service>()
                .Property(s => s.Price)
                .HasColumnType("decimal(10, 2)"); 

            modelBuilder.Entity<Booking>()
                .Property(u => u.Status)
                .HasColumnType("NVARCHAR(50)")
                .UseCollation("Russian_CI_AS");

            modelBuilder.Entity<Cabin>()
                .Property(u => u.Name)
                .HasColumnType("NVARCHAR(255)")
                .UseCollation("Russian_CI_AS");

            modelBuilder.Entity<Cabin>()
                .Property(u => u.ShortDescription)
                .HasColumnType("NVARCHAR(100)")
                .UseCollation("Russian_CI_AS");
                
            modelBuilder.Entity<Cabin>()
                .Property(u => u.LongDescription)
                .HasColumnType("NVARCHAR(255)")
                .UseCollation("Russian_CI_AS");

            modelBuilder.Entity<CabinPhoto>()
                .Property(u => u.Url)
                .HasColumnType("NVARCHAR(255)")
                .UseCollation("Russian_CI_AS");

            modelBuilder.Entity<Role>()
                .Property(u => u.Name)
                .HasColumnType("NVARCHAR(100)")
                .UseCollation("Russian_CI_AS");

            modelBuilder.Entity<Service>()
                .Property(u => u.Name)
                .HasColumnType("NVARCHAR(100)")
                .UseCollation("Russian_CI_AS");
            
            modelBuilder.Entity<Service>()
                .Property(u => u.Description)
                .HasColumnType("NVARCHAR(255)")
                .UseCollation("Russian_CI_AS");

            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .HasColumnType("NVARCHAR(100)")
                .UseCollation("Russian_CI_AS");

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasColumnType("NVARCHAR(255)")
                .UseCollation("Russian_CI_AS");

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .HasColumnType("NVARCHAR(255)")
                .UseCollation("Russian_CI_AS");
        }
    }
}
