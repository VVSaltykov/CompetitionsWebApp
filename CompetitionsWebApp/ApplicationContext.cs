using CompetitionsWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CompetitionsWebApp
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }
    }
}
