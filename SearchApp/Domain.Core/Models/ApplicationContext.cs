using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        /// <summary>
        /// Save results to DB
        /// </summary>
        /// <param name="results"></param>
        public async Task SaveResultsAsync(List<SearchResultModel> results)
        {
            var modelsToRemove = SearchResults.Where(x => x.SearchTerm == results.FirstOrDefault().SearchTerm);
            
            if (modelsToRemove.Any())
                SearchResults.RemoveRange(modelsToRemove);
            SearchResults.AddRange(results);
            await this.SaveChangesAsync();
        }

        /// <summary>
        /// Get results from Db by search term
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public List<SearchResultModel> ShowResults(string searchTerm)
        {
            return SearchResults.Where(x => x.SearchTerm == searchTerm).ToList();
        }
    }
}
