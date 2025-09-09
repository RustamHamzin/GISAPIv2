using GisZhkhAdmin.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GisZhkhAdmin.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ContractStatus> ContractStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Contract entity
            builder.Entity<Contract>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Number).IsRequired().HasMaxLength(50);
                entity.Property(e => e.SignDate).IsRequired();
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(e => e.Status)
                      .WithMany(s => s.Contracts)
                      .HasForeignKey(e => e.StatusId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure ContractStatus entity
            builder.Entity<ContractStatus>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(200);
            });

            // Seed data for ContractStatus
            builder.Entity<ContractStatus>().HasData(
                new ContractStatus { Id = 1, Name = "Ожидает", Description = "Не загружено в ГИС ЖКХ" },
                new ContractStatus { Id = 2, Name = "Загружено", Description = "Успешно загружено в ГИС ЖКХ" },
                new ContractStatus { Id = 3, Name = "Ошибка", Description = "Ошибка при загрузке в ГИС ЖКХ" },
                new ContractStatus { Id = 4, Name = "Активный", Description = "Активный договор" }
            );
        }
    }
}