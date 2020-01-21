using Microsoft.EntityFrameworkCore;

namespace Domain.Core.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<SearchResultModel> SearchResults { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            if (this.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                this.Database.Migrate();
            }
        }
    }
}
