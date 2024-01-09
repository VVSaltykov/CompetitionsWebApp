using CompetitionsWebApp.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CompetitionsWebApp.API
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=competitionsdb; User Id=sa; Password=yourStrong(!)Password; TrustServerCertificate=True");
        }
    }
}
