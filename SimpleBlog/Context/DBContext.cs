using Microsoft.EntityFrameworkCore;
using SimpleBlog.Models;

namespace SimpleBlog.Context
{
    public class DBContext : DbContext
    {
        public DbSet<ApplicationUser> Users { get; set; }

        public DBContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseMySQL("server=localhost;database=dotnet_blog;uid=root;pwd=Mohamad1375");
        }
    }
}
