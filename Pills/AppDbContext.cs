using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pills.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using Pills.Identity;

namespace Pills
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<PillsTaken> PillsTaken { get; set; }
        public DbSet<PillsTypes> PillsTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PillsTypes>().HasData(
                new PillsTypes { Id = 1, Name = "Magnesium" },
                new PillsTypes { Id = 2, Name = "VitaminD" },
                new PillsTypes { Id = 3, Name = "Denicit", MaxAllowed = 5 }
            );
        }
    }
}
