using AutoMapper;
using Domain.Core.Models;
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
        private readonly IMapper _mapper;
        private readonly ApplicationContext _context;

        public IndexModel(IEnumerable<ISearchClient> searchClients, ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _searchClients = searchClients;
            _mapper = mapper;
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
            SaveNewResultsToDatabase(searchTerm);
        }

        private void SaveNewResultsToDatabase(string searchTerm)
        {
            _context.SearchResults.RemoveRange(_context.SearchResults.Where(x => x.SearchTerm == searchTerm));
            _context.SearchResults.AddRange(DisplayedResults);
            _context.SaveChanges();
        }
    }
}
