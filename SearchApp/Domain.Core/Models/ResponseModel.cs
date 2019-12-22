namespace Domain.Core.Models
{
    /// <summary>
    /// Response Model From Clients
    /// </summary>
    public class ResponseModel
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