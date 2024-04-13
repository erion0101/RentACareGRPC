
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using RentACareGRPC.SQL;
namespace RentACareGRPC.SQL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
        { }

        public DbSet<Cars> Cars { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Reservations> Reservations { get; set; }
        public DbSet<Adress> Adresses { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Permission> Permission { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Role>()
                .HasOne(u => u.Permission)
                .WithMany()
                .HasForeignKey(u => u.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Customers>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customers>()
                .HasOne(u => u.Adress)
                .WithMany()
                .HasForeignKey(u => u.AdresaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customers>()
                .HasOne(u => u.Gender)
                .WithMany()
                .HasForeignKey(u => u.GenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payments>()
                .HasOne(u => u.Reservations)
                .WithMany()
                .HasForeignKey(u => u.ReservationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservations>()
                .HasOne(e => e.Cars)
                .WithMany()
                .HasForeignKey(u => u.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservations>()
                .HasOne(r => r.Customers)
                .WithMany()
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cars>()
                .Property(c => c.PriceForDay)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payments>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Reservations>()
                .Property(r => r.TotalPrice)
                .HasColumnType("decimal(18,2)");

        }

    }

}
