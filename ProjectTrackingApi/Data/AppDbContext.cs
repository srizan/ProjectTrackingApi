using Microsoft.EntityFrameworkCore;
using ProjectTrackingApi.Models;

namespace ProjectTrackingApi.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<Project> Projects { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
