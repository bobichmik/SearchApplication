using Domain.Core.Repositories;
using Domain.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{
    public class SearchResultsRepository : ISearchResultsRepository
    {
        private ApplicationContext _context;

        public SearchResultsRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task Add(SearchResultModel entity)
        {
            await _context.SearchResults.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRange(List<SearchResultModel> entities)
        {
            await _context.SearchResults.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await _context.SearchResults.FindAsync(id);
            if (entity != null)
            {
                _context.SearchResults.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteRange(List<SearchResultModel> entities)
        {
            _context.SearchResults.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public Task<SearchResultModel> GetById(int entityId)
        {
            return _context.SearchResults.FindAsync(entityId);
        }

        public IEnumerable<SearchResultModel> GetAll()
        {
            return _context.SearchResults;
        }

        public async Task Update(SearchResultModel entity)
        {
            var result = _context.SearchResults.Attach(entity);
            result.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public IEnumerable<SearchResultModel> GetBySearchTerm(string searchTerm)
        {
            return _context.SearchResults.Where(x => x.SearchTerm == searchTerm);
        }
    }
}