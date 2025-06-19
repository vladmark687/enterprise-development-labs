using DistrictStatisticsManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace DistrictStatisticsManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Enterprise> Enterprises { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Supply> Supplies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка первичных ключей
            modelBuilder.Entity<Enterprise>().HasKey(e => e.RegistrationNumber);
            modelBuilder.Entity<Supplier>().HasKey(s => s.Id);
            modelBuilder.Entity<Supply>().HasKey(s => s.Id);

            // Настройка отношений
            modelBuilder.Entity<Supply>()
                .HasOne(s => s.Enterprise)
                .WithMany(e => e.Supplies)
                .HasForeignKey(s => s.EnterpriseRegistrationNumber)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Supply>()
                .HasOne(s => s.Supplier)
                .WithMany(s => s.Supplies)
                .HasForeignKey(s => s.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);

            // Дополнительная конфигурация для Enterprise
            modelBuilder.Entity<Enterprise>(entity =>
            {
                entity.Property(e => e.RegistrationNumber)
                    .HasMaxLength(50)
                    .IsRequired();
                
                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsRequired();
                
                entity.Property(e => e.Address)
                    .HasMaxLength(300)
                    .IsRequired();
                
                entity.Property(e => e.Phone)
                    .HasMaxLength(20);
                
                entity.Property(e => e.TotalArea)
                    .HasColumnType("decimal(18,2)");
                
                // Индексы для улучшения производительности
                entity.HasIndex(e => e.IndustryType);
                entity.HasIndex(e => e.OwnershipType);
                entity.HasIndex(e => e.Name);
            });

            // Дополнительная конфигурация для Supplier
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.Property(s => s.Name)
                    .HasMaxLength(200)
                    .IsRequired();
                
                entity.Property(s => s.Address)
                    .HasMaxLength(300)
                    .IsRequired();
                
                entity.Property(s => s.Phone)
                    .HasMaxLength(20);
                
                // Индекс для улучшения производительности
                entity.HasIndex(s => s.Name);
            });

            // Дополнительная конфигурация для Supply
            modelBuilder.Entity<Supply>(entity =>
            {
                entity.Property(s => s.ProductName)
                    .HasMaxLength(200)
                    .IsRequired();
                
                entity.Property(s => s.Cost)
                    .HasColumnType("decimal(18,2)");
                
                // Индексы для улучшения производительности запросов
                entity.HasIndex(s => s.SupplyDate);
                entity.HasIndex(s => s.EnterpriseRegistrationNumber);
                entity.HasIndex(s => s.SupplierId);
                entity.HasIndex(s => new { s.SupplyDate, s.SupplierId });
            });
        }
    }
}