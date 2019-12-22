using AutoMapper;
using Domain.Core.Models;
using Flurl;
using BingSearchClient.Internal.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SearcherApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BingSearchClient.Internal
{
    /// <summary>
    /// Bing search client
    /// </summary>
    public class BingClient : ISearchClient
    {
        private readonly HttpClient _httpClient;
        private readonly BingClientSettings _settings;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public BingClient(HttpClient httpClient, BingClientSettings settings, IMapper mapper, ILogger<BingClient> logger)
        {
            _httpClient = httpClient;
            _settings = settings;
            _mapper = mapper;
            _logger = logger;

            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Ocp-Apim-Subscription-Key", _settings.Key);
        }

        public async Task<List<ResponseModel>> GetSearchInfoAsync(string searchTerm)
        {
            try
            {
                var path = _settings.Url.SetQueryParam("q", searchTerm);
                using (var response = await _httpClient.GetAsync(path).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    var jsonData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var responseData = JsonConvert.DeserializeObject<BingResponseModel>(jsonData);

                    var modelsToReturn = _mapper.Map<List<ResponseModel>>(responseData.WebPages.Value);
                    foreach (var model in modelsToReturn)
                    {
                        model.SearchTerm = searchTerm;
                    }

                    return modelsToReturn;
                }
            }
            catch (Exception ex)
            {
                var exMessage = $"Getting search results from Google client failed for {searchTerm}";
                _logger.LogError(ex, exMessage);
                throw new InvalidOperationException(exMessage, ex);
            }
        }
    }
}