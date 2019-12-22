using AutoMapper;
using BingSearchClient.Internal.Mapping;
using BingSearchClient.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SearcherApp.Interfaces;

namespace BingSearchClient.Startup
{
    public static class BingClientStartup
    {
        /// <summary>
        /// Startup extension for Bing Client
        /// </summary>
        public static void AddBingClient(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection("Bing").Get<BingClientSettings>();

            services.AddSingleton(settings);
            services.AddHttpClient<ISearchClient, BingClient>();
            services.AddSingleton<Profile, AutoMapperProfile>();
        }
    }
}