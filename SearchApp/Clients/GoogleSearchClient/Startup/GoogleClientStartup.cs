using AutoMapper;
using Domain.Core.Searching;
using GoogleSearchClient.Internal;
using GoogleSearchClient.Internal.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleSearchClient.Startup
{
    public static class GoogleClientStartup
    {
        /// <summary>
        /// Startup extension for Google client
        /// </summary>
        public static void AddGoogleClient(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection("Google").Get<GoogleClientSettings>();

            services.AddSingleton(settings);
            services.AddHttpClient<ISearchClient, GoogleClient>();
            services.AddSingleton<Profile, AutoMapperProfile>();
        }
    }
}