using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Simstagram2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simstagram2._0
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Interaction> Interactions { get; set; }
        public UserContext(DbContextOptions<UserContext> options)
           : base(options)
        {
        }

        public UserContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Server=DESKTOP-HTN63BU;Database=Simstagram2.0;Trusted_Connection=True;")
                .UseLoggerFactory(LoggerFactory.Create(c => c.AddConsole()));
            base.OnConfiguring(optionsBuilder);
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Interaction>().HasKey(x => new { x.PhotoId, x.UserId });
            base.OnModelCreating(builder);            
        }
    }
}
