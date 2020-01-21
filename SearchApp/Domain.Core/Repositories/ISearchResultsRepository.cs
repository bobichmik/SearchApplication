using Domain.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Core.Repositories
{
    public interface ISearchResultsRepository
    {
        IEnumerable<SearchResultModel> GetAll();

        Task<SearchResultModel> GetById(int entityId);

        Task Add(SearchResultModel entity);

        Task AddRange(List<SearchResultModel> entities);

        Task Update(SearchResultModel entity);

        Task Delete(int id);

        Task DeleteRange(List<SearchResultModel> entities);

        IEnumerable<SearchResultModel> GetBySearchTerm(string searchTerm);
    }
}