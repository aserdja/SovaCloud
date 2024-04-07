using Microsoft.EntityFrameworkCore;
using SovaCloud.Models;

namespace SovaCloud.Data
{
    public class SovaCloudDbContext : DbContext
    {
        public SovaCloudDbContext(DbContextOptions<SovaCloudDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Models.File> Files { get; set; }
        public DbSet<FileOwnership> FileOwnerships { get; set; }
    }
}


