namespace MDA.Security.Models
{
    public class ServiceDetails
    {
        /// <summary>
        /// Initializes a new instance of the ServiceDetails class.
        /// </summary>
        public ServiceDetails()
        {
            ServiceName = string.Empty;
            ServiceUrl = string.Empty;
            Host = string.Empty;
            Scheme = string.Empty;
        }

        /// <summary>
        /// Gets or Sets Host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets Scheme
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// Gets or Sets Service Name
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or Sets Service URL
        /// </summary>
        public string ServiceUrl { get; set; }
    }
}