using Microsoft.EntityFrameworkCore;

namespace Domain.Core.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<SearchResultModel> SearchResults { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            this.Database.Migrate();
        }
    }
}
