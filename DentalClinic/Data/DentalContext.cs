using DentalClinic.Models;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Data
{
    public class DentalContext : DbContext
    {
        public DbSet<Applic> Applications { get; set; }
        public DbSet<User> Users { get; set; }

        public DentalContext() : base()
        {
        }

        public DentalContext(DbContextOptions<DentalContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=dental.db");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Applic>()
                .HasOne(a => a.User)
                .WithMany(u => u.Applications)
                .HasForeignKey(a => a.UserId)
                .IsRequired(false);
        }
    }
}