using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkinsAdmin.Models;
using SkinsAdmin.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkinsAdmin.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public ApplicationDbContext()
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Apps>()
               .HasIndex(u => u.AppKey)
               .IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Category> Category { set; get; }
        public DbSet<Mods> Mods { set; get; }
        public DbSet<ModImages> ModImages { set; get; }
        public DbSet<Apps> Apps { set; get; }
    }
}
