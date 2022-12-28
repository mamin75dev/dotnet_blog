using Microsoft.EntityFrameworkCore;
using SimpleBlog.Models;

namespace SimpleBlog.Context
{
    public class DataContext : DbContext
    {
        public DbSet<ApplicationUser> Users { get; set; }

        public DataContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseMySQL("server=localhost;database=dotnet_blog;uid=root;pwd=Mohamad1375");
        }
    }
}
