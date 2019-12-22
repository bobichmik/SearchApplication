using Newtonsoft.Json;
using System.Collections.Generic;

namespace BingSearchClient.Internal.Models
{
    internal class BingResponseModel
    {
        [JsonProperty("webPages")]
        public WebPages WebPages { get; set; }
    }

    public class Value
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class WebPages
    {
        [JsonProperty("Value")]
        public List<Value> Value { get; set; }
    }
}