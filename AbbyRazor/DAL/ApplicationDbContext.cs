using AbbyRazor.Models;
using Microsoft.EntityFrameworkCore;

namespace AbbyRazor.DAL
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
    }
}
