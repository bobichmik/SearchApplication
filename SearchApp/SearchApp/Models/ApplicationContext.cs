using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace SearchApp.Models
{
    /// <summary>
    /// Application context for Data base
    /// </summary>
    public class ApplicationContext : DbContext
    {
        public DbSet<ResultModel> SearchResults { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Save results to DB
        /// </summary>
        /// <param name="results"></param>
        public void SaveResults(List<ResultModel> results)
        {
            var modelsToRemove = SearchResults.Where(x => x.SearchTerm == results.FirstOrDefault().SearchTerm);
            SearchResults.RemoveRange(modelsToRemove);
            SearchResults.AddRange(results);
            this.SaveChangesAsync();
        }

        /// <summary>
        /// Get results from Db by search term
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public List<ResultModel> ShowResults(string searchTerm)
        {
            return SearchResults.Where(x => x.SearchTerm == searchTerm).ToList();
        }
    }
}