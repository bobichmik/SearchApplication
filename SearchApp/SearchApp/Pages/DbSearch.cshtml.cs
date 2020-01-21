using Domain.Core.Repositories;
using Domain.Core.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace SearchApp.Pages
{
    public class DbSearchModel : PageModel
    {
        public string Message { get; set; }

        public List<SearchResultModel> DisplayedResults { get; set; }

        private ISearchResultsRepository _repository;

        public DbSearchModel(ISearchResultsRepository repository)
        {
            _repository = repository;
            DisplayedResults = null;
        }

        public void OnGet()
        {
            Message = "Enter Search Term";
        }

        public void OnPostAsync(string searchTerm)
        {
            Message = $"Search term = {searchTerm}";
            DisplayedResults = _repository.GetBySearchTerm(searchTerm).ToList();
        }
    }
}