using Microsoft.EntityFrameworkCore;

namespace SimpleUrlShorten.Models.Context
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<Url> Urls { get; set; }

    }
}
