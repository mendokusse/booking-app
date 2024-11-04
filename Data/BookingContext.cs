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
            base.OnModelCreating(modelBuilder);

            // Настройка связи многие-ко-многим между Booking и Service
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
        }
    }
}
