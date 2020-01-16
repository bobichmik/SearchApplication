namespace Domain.Core.Models
{
    /// <summary>
    /// Search Result Model
    /// </summary>
    public class SearchResultModel
    {
        /// <summary>
        /// Model identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Search Term for model
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Title of model
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Url of model
        /// </summary>
        public string Url { get; set; }
    }
}