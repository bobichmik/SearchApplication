using Domain.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearcherApp.Interfaces
{
    /// <summary>
    /// Interface of search client
    /// </summary>
    public interface ISearchClient
    {
        /// <summary>
        /// Get Results from client's search by search term
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns></returns>
        Task<List<ResponseModel>> GetSearchInfoAsync(string searchTerm);
    }
}