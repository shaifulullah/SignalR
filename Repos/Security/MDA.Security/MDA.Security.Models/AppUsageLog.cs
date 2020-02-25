namespace MDA.Security.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For AppUsageLog
    /// </summary>
    [Table("AppUsageLog")]
    public class AppUsageLog
    {
        /// <summary>
        /// Gets or sets Application
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// Gets or sets ApplicationComponentObj
        /// Foreign Key LnApplicationComponentId (ApplicationComponent)
        /// </summary>
        [ForeignKey("LnApplicationComponentId")]
        public ApplicationComponent ApplicationComponentObj { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets EndTime
        /// </summary>
        public Nullable<DateTime> EndTime { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LnApplicationComponentId
        /// </summary>
        public Nullable<int> LnApplicationComponentId { get; set; }

        /// <summary>
        /// Gets or sets ProxyUser
        /// </summary>
        public string ProxyUser { get; set; }

        /// <summary>
        /// Gets or sets Server
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets Service
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// Gets or sets StartTime
        /// </summary>
        public Nullable<DateTime> StartTime { get; set; }

        /// <summary>
        /// Gets or sets UserName
        /// </summary>
        public string UserName { get; set; }
    }
}