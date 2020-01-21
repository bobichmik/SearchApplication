using Domain.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Repositories.Startup
{
    public static class RepositoriesStartup
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ISearchResultsRepository, SearchResultsRepository>();
        }
    }
}