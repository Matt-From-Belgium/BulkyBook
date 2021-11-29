using Microsoft.EntityFrameworkCore;
using BulkyBookWeb.Models;

namespace BulkyBookWeb.Data
{
    public class ApplicationDbContext:DbContext
    {
        DbSet<Category> Categories { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }
    }
}
