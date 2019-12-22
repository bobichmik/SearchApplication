using Microsoft.AspNetCore.Mvc.RazorPages;
using SearchApp.Models;
using System.Collections.Generic;

namespace SearchApp.Pages
{
    public class DbSearchModel : PageModel
    {
        public string Message { get; set; }

        public List<ResultModel> DisplayedResults { get; set; }

        private readonly ApplicationContext _context;

        public DbSearchModel(ApplicationContext context)
        {
            _context = context;
            DisplayedResults = null;
        }

        public void OnGet()
        {
            Message = "Enter Search Term";
        }

        public void OnPostAsync(string searchTerm)
        {
            Message = $"Search term = {searchTerm}";
            DisplayedResults = _context.ShowResults(searchTerm);
        }
    }
}