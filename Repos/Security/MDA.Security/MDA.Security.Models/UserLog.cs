namespace MDA.Security.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For UserLog
    /// </summary>
    [Table("UserLog")]
    public class UserLog
    {
        /// <summary>
        /// Gets or sets ActionDateTime
        /// </summary>
        public Nullable<DateTime> ActionDateTime { get; set; }

        /// <summary>
        /// Gets or sets Application
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// Gets or sets ClientIPAddress
        /// </summary>
        public string ClientIPAddress { get; set; }

        /// <summary>
        /// Gets or sets Domain
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets HasLonSucceeded
        /// </summary>
        public Nullable<bool> HasLonSucceeded { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LnLogActionId
        /// </summary>
        public Nullable<int> LnLogActionId { get; set; }

        /// <summary>
        /// Gets or sets LnUserAccountId
        /// </summary>
        public int LnUserAccountId { get; set; }

        /// <summary>
        /// Gets or sets LogActionObj
        /// Foreign Key LnLogActionId (LogAction)
        /// </summary>
        [ForeignKey("LnLogActionId")]
        public LogAction LogActionObj { get; set; }

        /// <summary>
        /// Gets or sets NetUser
        /// </summary>
        public string NetUser { get; set; }

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
        /// Gets or sets UserAccountObj
        /// Foreign Key LnUserAccountId (UserAccount)
        /// </summary>
        [ForeignKey("LnUserAccountId")]
        public UserAccount UserAccountObj { get; set; }
    }
}