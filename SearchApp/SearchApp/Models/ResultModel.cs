namespace SearchApp.Models
{
    /// <summary>
    /// Data Base result model
    /// </summary>
    public class ResultModel
    {
        /// <summary>
        /// Model identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Model's Search term
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Model's Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Model's Url
        /// </summary>
        public string Url { get; set; }
    }
}
