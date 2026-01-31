using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pills.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using Pills.Identity;
using System.Security.Claims;
using Org.BouncyCastle.Asn1.X509.Qualified;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Pills.Services.Interfaces;
using Microsoft.CodeAnalysis;
using Pills.Common.Interfaces;

namespace Pills
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserService _userService;
        public AppDbContext(DbContextOptions<AppDbContext> options, IDateTimeProvider dateTimeProvider,
            IUserService userService) : base(options)
        {
            _dateTimeProvider = dateTimeProvider;
            _userService = userService;
        }
        public DbSet<PillsTaken> PillsTaken { get; set; }
        public DbSet<PillsTypes> PillsTypes { get; set; }
        public DbSet<DailyPillReport> DailyPillReport { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public bool IgnoreSoftDelete { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach(var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if(typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(AppDbContext)
                        .GetMethod(nameof(ApplySoftDeleteFilter),
                            System.Reflection.BindingFlags.NonPublic |
                            System.Reflection.BindingFlags.Static)?
                        .MakeGenericMethod(entityType.ClrType);

                    method?.Invoke(null, new object[] { modelBuilder, this });
                }
            }

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PillsTypes>().HasData(
                new PillsTypes
                {
                    Id = 1,
                    Name = "Magnesium",
                    CreatedAt = new DateTime(2025, 12, 1),
                    CreatedBy = "System",
                    IsDeleted = false,
                    MaxAllowed = 1
                },
                new PillsTypes 
                { 
                    Id = 2, 
                    Name = "VitaminD", 
                    CreatedAt = new DateTime(2025, 12, 1),
                    CreatedBy = "System",
                    IsDeleted = false,
                    MaxAllowed = 1
                },
                new PillsTypes 
                { 
                    Id = 3, 
                    Name = "Denicit", 
                    CreatedAt = new DateTime(2025, 12, 1),
                    CreatedBy = "System",
                    IsDeleted = false,
                    MaxAllowed = 5,
                }
            );

            modelBuilder.Entity<PillsTypes>()
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<PillsTypes>()
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _userService.UserId;

            var now = _dateTimeProvider.UtcNow;

            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            {
                switch(entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.IsDeleted = false;
                        entry.Entity.CreatedBy = userId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.EditedAt = now;
                        entry.Entity.EditedBy = userId;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedAt = now;
                        entry.Entity.DeletedBy = userId;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        private static void ApplySoftDeleteFilter<TEntity>(ModelBuilder modelBuilder, AppDbContext appDbContext)
            where TEntity : class, ISoftDeletable
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted || appDbContext.IgnoreSoftDelete);
        }
    }
}
