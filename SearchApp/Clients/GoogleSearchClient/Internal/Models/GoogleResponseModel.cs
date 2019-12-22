using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleSearchClient.Internal.Models
{
    internal class GoogleResponseModel
    {
        [JsonProperty("items")]
        public List<Item> Items { get; set; }
    }

    internal class Item
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }
}
