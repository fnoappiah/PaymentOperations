using Microsoft.EntityFrameworkCore;
using PaymentOperations.Models;

namespace PaymentOperations.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<PaymentFile> PaymentFiles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure PaymentFile entity
            modelBuilder.Entity<PaymentFile>(entity =>
            {
                entity.HasKey(e => e.PaymentFileID);
                entity.Property(e => e.FileName).IsRequired();
                entity.Property(e => e.UploadDate).IsRequired();
                entity.Property(e => e.Path).IsRequired();
                entity.Property(e => e.UploaderName).IsRequired();
                entity.Property(e => e.AppName).IsRequired();
                entity.Property(e => e.NCRTested).IsRequired();
                entity.Property(e => e.BankTested).IsRequired();
                entity.Property(e => e.BankProduce).IsRequired();
                entity.Property(e => e.Creator).IsRequired();
                entity.Property(e => e.Description);
            });
            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserID);
                entity.Property(e => e.UserName).IsRequired();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Position).IsRequired();
                entity.Property(e => e.Assignments).IsRequired();
            });
        }
    }
}
