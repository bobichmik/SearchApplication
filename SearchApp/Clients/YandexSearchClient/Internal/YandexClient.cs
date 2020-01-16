using AutoMapper;
using Domain.Core.Models;
using Domain.Core.Searching;
using Flurl;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using YandexSearchClient.Internal.Models;

namespace YandexSearchClient.Internal
{
    /// <summary>
    /// Yandex search client
    /// </summary>
    public class YandexClient : ISearchClient
    {
        private readonly HttpClient _httpClient;
        private readonly YandexClientSettings _settings;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public YandexClient(HttpClient httpClient, YandexClientSettings settings, IMapper mapper, ILogger<YandexClient> logger)
        {
            _httpClient = httpClient;
            _settings = settings;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<SearchResultModel>> GetSearchInfoAsync(string searchTerm)
        {
            try
            {
                var path = _settings.Url.SetQueryParams(new { key = _settings.Key, user = _settings.User, query = searchTerm });
                using (var response = await _httpClient.GetAsync(path).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    var streamData = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var serializer = new XmlSerializer(typeof(YandexResponseModel));
                    var responseData = (YandexResponseModel)serializer.Deserialize(streamData);

                    var modelsToReturn = _mapper.Map<List<SearchResultModel>>(responseData.Response.Results.Grouping.Group);
                    foreach (var model in modelsToReturn)
                    {
                        model.SearchTerm = searchTerm;
                    }

                    return modelsToReturn;
                }
            }
            catch (Exception ex)
            {
                var exMessage = $"Getting search results from Yandex client failed for {searchTerm}";
                _logger.LogError(ex, exMessage);
                throw new InvalidOperationException(exMessage, ex);
            }
        }
    }
}
