namespace MDA.Security.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For Audit
    /// </summary>
    [Table("Audit")]
    public class Audit
    {
        /// <summary>
        /// Gets or sets Application
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// Gets or sets FieldName
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets FieldType
        /// </summary>
        public string FieldType { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets NewValue
        /// </summary>
        public string NewValue { get; set; }

        /// <summary>
        /// Gets or sets OldValue
        /// </summary>
        public string OldValue { get; set; }

        /// <summary>
        /// Gets or sets PrimaryKeyField
        /// </summary>
        public string PrimaryKeyField { get; set; }

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
        /// Gets or sets TableName
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets UpdateDate
        /// </summary>
        public Nullable<DateTime> UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets UserName
        /// </summary>
        public string UserName { get; set; }
    }
}