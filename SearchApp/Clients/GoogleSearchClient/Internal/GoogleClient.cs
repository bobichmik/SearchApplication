using AutoMapper;
using Domain.Core.Models;
using Domain.Core.Searching;
using Flurl;
using GoogleSearchClient.Internal.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoogleSearchClient.Internal
{
    /// <summary>
    /// Google search client
    /// </summary>
    public class GoogleClient : ISearchClient
    {
        private readonly HttpClient _httpClient;
        private readonly GoogleClientSettings _settings;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GoogleClient(HttpClient httpClient, GoogleClientSettings settings, IMapper mapper, ILogger<GoogleClient> logger)
        {
            _httpClient = httpClient;
            _settings = settings;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<SearchResultModel>> GetSearchInfoAsync(string searchQuery)
        {
            try
            {
                var path = _settings.Url.SetQueryParams(new { key = _settings.Key, cx = _settings.CustomSearchEngineId, q = searchQuery });
                using (var response = await _httpClient.GetAsync(path).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    var jsonData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var responseData = JsonConvert.DeserializeObject<GoogleResponseModel>(jsonData);
                    
                    var modelsToReturn = _mapper.Map<List<SearchResultModel>>(responseData.Items);
                    foreach (var model in modelsToReturn)
                    {
                        model.SearchTerm = searchQuery;
                    }

                    return modelsToReturn;
                }
            }
            catch (Exception ex)
            {
                var exMessage = $"Getting search results from Google client failed for {searchQuery}";
                _logger.LogError(ex, exMessage);
                throw new InvalidOperationException(exMessage, ex);
            }
        }
    }
}