using Domain.Core.Models;
using Domain.Core.Repositories;
using Domain.Core.Searching;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchApp.Pages
{
    public class IndexModel : PageModel
    {
        public string Message { get; set; }

        public List<SearchResultModel> DisplayedResults { get; set; }

        private readonly IEnumerable<ISearchClient> _searchClients;
        private readonly ISearchResultsRepository _repository;

        public IndexModel(IEnumerable<ISearchClient> searchClients, ISearchResultsRepository repository)
        {
            _repository = repository;
            _searchClients = searchClients;
        }

        public void OnGet()
        {
            Message = "Enter Search Term";
            DisplayedResults = null;
        }

        public async Task OnPostAsync(string searchTerm)
        {
            var tasks = _searchClients.Select(x => x.GetSearchInfoAsync(searchTerm));
            var result = await Task.WhenAny(tasks);

            Message = $"Search term = {searchTerm}";
            DisplayedResults = await result;
            await SaveNewResultsToDatabaseAsync(searchTerm);
        }

        private async Task SaveNewResultsToDatabaseAsync(string searchTerm)
        {
            var entitiesToDelete = _repository.GetBySearchTerm(searchTerm).ToList();
            await _repository.DeleteRange(entitiesToDelete);
            await _repository.AddRange(DisplayedResults);
        }
    }
}
