using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SearcherApp.Interfaces;
using YandexSearchClient.Internal;
using YandexSearchClient.Internal.Mapping;

namespace YandexSearchClient.Startup
{
    public static class YandexClientStartup
    {
        /// <summary>
        /// Startup extension for yandex client
        /// </summary>
        public static void AddYandexClient(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection("Yandex").Get<YandexClientSettings>();

            services.AddSingleton(settings);
            services.AddHttpClient<ISearchClient, YandexClient>();
            services.AddSingleton<Profile, AutoMapperProfile>();
        }
    }
}