using AutoMapper;
using BingSearchClient.Internal;
using FluentAssertions;
using GoogleSearchClient.Internal;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using YandexSearchClient.Internal;
using Bing = BingSearchClient.Internal.Mapping;
using Google = GoogleSearchClient.Internal.Mapping;
using SearchResultModel = Domain.Core.Models.SearchResultModel;
using Yandex = YandexSearchClient.Internal.Mapping;

namespace ClientsTests
{
    public class SearchClientsTests
    {
        [Fact]
        public async Task BingClientTestAsync()
        {
            var bingClient = SetUpBingClient();

            var results = await bingClient.GetSearchInfoAsync("test");

            var expectedResults = new List<SearchResultModel>()
            {
                new SearchResultModel { SearchTerm = "test", Title = "name1", Url = "url1" },
                new SearchResultModel { SearchTerm = "test", Title = "name2", Url = "url2" },
                new SearchResultModel { SearchTerm = "test", Title = "name3", Url = "url3" },
            };

            results.Should().BeEquivalentTo(expectedResults);
        }

        [Fact]
        public async Task GoogleClientTestAsync()
        {
            var goggleClient = SetUpGoogleClient();

            var results = await goggleClient.GetSearchInfoAsync("test2");

            var expectedResults = new List<SearchResultModel>()
            {
                new SearchResultModel { SearchTerm = "test2", Title = "title1", Url = "link1" },
                new SearchResultModel { SearchTerm = "test2", Title = "title2", Url = "link2" },
            };

            results.Should().BeEquivalentTo(expectedResults);
        }

        [Fact]
        public async Task YandexClientTestAsync()
        {
            var yandexClient = SetUpYandexClient();

            var results = await yandexClient.GetSearchInfoAsync("test3");

            var expectedResults = new List<SearchResultModel>()
            {
                new SearchResultModel { SearchTerm = "test3", Title = "title1", Url = "url1" },
                new SearchResultModel { SearchTerm = "test3", Title = "title2", Url = "url2" },
                new SearchResultModel { SearchTerm = "test3", Title = "title3", Url = "url3" },
            };

            results.Should().BeEquivalentTo(expectedResults);
        }

        private BingClient SetUpBingClient()
        {
            var services = SetUpBingServiceCollection();

            var httpClient = SetUpBingHttpClient();

            var nullLogger = Microsoft.Extensions.Logging.Abstractions.NullLogger<BingClient>.Instance;
            var serviceProvider = services.BuildServiceProvider();
            var mapper = serviceProvider.GetRequiredService<IMapper>();

            var bingClient = new BingClient(httpClient, serviceProvider.GetRequiredService<BingClientSettings>(), mapper, nullLogger);

            return bingClient;
        }

        private ServiceCollection SetUpBingServiceCollection()
        {
            var bingSettings = new BingClientSettings()
            {
                Url = "http://test.com"
            };

            var services = new ServiceCollection();
            services.AddSingleton(bingSettings);
            services.AddSingleton<Profile, Bing.AutoMapperProfile>();
            services.AddSingleton(
                sp =>
                {
                    var profiles = sp.GetServices<Profile>();
                    return new MapperConfiguration(
                            config =>
                            {
                                foreach (var profile in profiles)
                                {
                                    config.AddProfile(profile);
                                }
                            })
                        .CreateMapper(sp.GetService);
                });

            return services;
        }

        private HttpClient SetUpBingHttpClient()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"webPages\":{\"value\":[{\"name\":\"name1\",\"url\":\"url1\"}," +
                    "{\"name\":\"name2\",\"url\":\"url2\"},{\"name\":\"name3\",\"url\":\"url3\"}]}}")
                });

            var httpClient = new HttpClient(handlerMock.Object);
            return httpClient;
        }

        private GoogleClient SetUpGoogleClient()
        {
            var services = SetUpGoogleServiceCollection();

            var httpClient = SetUpGoogleHttpClient();

            var nullLogger = Microsoft.Extensions.Logging.Abstractions.NullLogger<GoogleClient>.Instance;
            var serviceProvider = services.BuildServiceProvider();
            var mapper = serviceProvider.GetRequiredService<IMapper>();

            var bingClient = new GoogleClient(httpClient, serviceProvider.GetRequiredService<GoogleClientSettings>(), mapper, nullLogger);

            return bingClient;
        }

        private ServiceCollection SetUpGoogleServiceCollection()
        {
            var googleSettings = new GoogleClientSettings()
            {
                Url = "http://test.com"
            };

            var services = new ServiceCollection();
            services.AddSingleton(googleSettings);
            services.AddSingleton<Profile, Google.AutoMapperProfile>();
            services.AddSingleton(
                sp =>
                {
                    var profiles = sp.GetServices<Profile>();
                    return new MapperConfiguration(
                            config =>
                            {
                                foreach (var profile in profiles)
                                {
                                    config.AddProfile(profile);
                                }
                            })
                        .CreateMapper(sp.GetService);
                });

            return services;
        }

        private HttpClient SetUpGoogleHttpClient()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"items\":[{\"title\":\"title1\",\"link\":\"link1\"},{\"title\":\"title2\",\"link\":\"link2\"}]}")
                });

            var httpClient = new HttpClient(handlerMock.Object);
            return httpClient;
        }

        private YandexClient SetUpYandexClient()
        {
            var services = SetUpYandexServiceCollection();

            var httpClient = SetUpYandexHttpClient();

            var nullLogger = Microsoft.Extensions.Logging.Abstractions.NullLogger<YandexClient>.Instance;
            var serviceProvider = services.BuildServiceProvider();
            var mapper = serviceProvider.GetRequiredService<IMapper>();

            var yandexClient = new YandexClient(httpClient, serviceProvider.GetRequiredService<YandexClientSettings>(), mapper, nullLogger);

            return yandexClient;
        }

        private ServiceCollection SetUpYandexServiceCollection()
        {
            var yandexSettings = new YandexClientSettings()
            {
                Url = "http://test.com"
            };

            var services = new ServiceCollection();
            services.AddSingleton(yandexSettings);
            services.AddSingleton<Profile, Yandex.AutoMapperProfile>();
            services.AddSingleton(
                sp =>
                {
                    var profiles = sp.GetServices<Profile>();
                    return new MapperConfiguration(
                            config =>
                            {
                                foreach (var profile in profiles)
                                {
                                    config.AddProfile(profile);
                                }
                            })
                        .CreateMapper(sp.GetService);
                });

            return services;
        }

        private HttpClient SetUpYandexHttpClient()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("<?xml version=\"1.0\" encoding=\"utf-8\"?><yandexsearch version=\"1.0\"><response><results><grouping><group><doc><url>url1</url><title>" +
                    "<hlword>title</hlword>1</title></doc></group><group><doc><url>url2</url><title>title<hlword>2</hlword></title>" +
                    "</doc></group><group><doc><url>url3</url><title>titl<hlword>e</hlword>3</title></doc></group></grouping></results></response></yandexsearch>")
                });

            var httpClient = new HttpClient(handlerMock.Object);
            return httpClient;
        }
    }
}