namespace GoogleSearchClient.Internal
{
    public sealed class GoogleClientSettings
    {
        /// <summary>
        /// Google's search url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Search key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Identifier of custom search engine
        /// </summary>
        public string CustomSearchEngineId { get; set; }
    }
}