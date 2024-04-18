using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DBcontext
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Theme> Themes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var c0 = new Theme { Name = "DARK", Id = 1 };
            var c1 = new Theme { Name = "LIGHT", Id = 2 };


            modelBuilder.Entity<Theme>().HasData(new List<Theme> { c0, c1 });

            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=wikyProject2;Trusted_Connection=True;");
            }
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=wikyProject2;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
