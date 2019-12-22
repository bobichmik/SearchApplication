using AutoMapper;
using BingSearchClient.Internal;
using GoogleSearchClient.Internal;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using YandexSearchClient.Internal;
using Bing = BingSearchClient.Internal.Mapping;
using Google = GoogleSearchClient.Internal.Mapping;
using Yandex = YandexSearchClient.Internal.Mapping;

namespace ClientsTests
{
    public class SearchClientsTests
    {
        private readonly ServiceCollection Services;

        public SearchClientsTests()
        {
            var services = new ServiceCollection();
            var bingSettings = new BingClientSettings()
            {
                Url = "http://test.com"
            };
            var googleSettings = new GoogleClientSettings()
            {
                Url = "http://test.com"
            };
            var yandexSettings = new YandexClientSettings()
            {
                Url = "http://test.com"
            };

            services.AddSingleton(bingSettings);
            services.AddSingleton(googleSettings);
            services.AddSingleton(yandexSettings);
            services.AddSingleton<Profile, Bing.AutoMapperProfile>();
            services.AddSingleton<Profile, Google.AutoMapperProfile>();
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
            Services = services;
        }

        [Fact]
        public async Task BingClientTestAsync()
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

            var nullLogger = Microsoft.Extensions.Logging.Abstractions.NullLogger<BingClient>.Instance;
            var serviceProvider = Services.BuildServiceProvider();
            var mapper = serviceProvider.GetRequiredService<IMapper>();

            var bingClient = new BingClient(httpClient, serviceProvider.GetRequiredService<BingClientSettings>(), mapper, nullLogger);
            var results = await bingClient.GetSearchInfoAsync("test");
            var titles = results.Select(x => x.Title).ToList();
            var urls = results.Select(x => x.Url).ToList();

            Assert.Equal(3, results.Count);
            Assert.Equal(new List<string>() { "name1", "name2", "name3" }, titles);
            Assert.Equal(new List<string>() { "url1", "url2", "url3" }, urls);
        }

        [Fact]
        public async Task GoogleClientTestAsync()
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

            var nullLogger = Microsoft.Extensions.Logging.Abstractions.NullLogger<GoogleClient>.Instance;
            var serviceProvider = Services.BuildServiceProvider();
            var mapper = serviceProvider.GetRequiredService<IMapper>();

            var bingClient = new GoogleClient(httpClient, serviceProvider.GetRequiredService<GoogleClientSettings>(), mapper, nullLogger);
            var results = await bingClient.GetSearchInfoAsync("test");
            var titles = results.Select(x => x.Title).ToList();
            var urls = results.Select(x => x.Url).ToList();

            Assert.Equal(2, results.Count);
            Assert.Equal(new List<string>() { "title1", "title2" }, titles);
            Assert.Equal(new List<string>() { "link1", "link2" }, urls);
        }

        [Fact]
        public async Task YandexClientTestAsync()
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

            var nullLogger = Microsoft.Extensions.Logging.Abstractions.NullLogger<YandexClient>.Instance;
            var serviceProvider = Services.BuildServiceProvider();
            var mapper = serviceProvider.GetRequiredService<IMapper>();

            var yandexClient = new YandexClient(httpClient, serviceProvider.GetRequiredService<YandexClientSettings>(), mapper, nullLogger);
            var results = await yandexClient.GetSearchInfoAsync("test");
            var titles = results.Select(x => x.Title).ToList();
            var urls = results.Select(x => x.Url).ToList();

            Assert.Equal(3, results.Count);
            Assert.Equal(new List<string>() { "title1", "title2", "title3" }, titles);
            Assert.Equal(new List<string>() { "url1", "url2", "url3" }, urls);
        }
    }
}
