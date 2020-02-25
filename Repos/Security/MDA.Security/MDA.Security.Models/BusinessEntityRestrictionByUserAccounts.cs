namespace MDA.Security.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For BusinessEntityRestrictionByUserAccounts
    /// </summary>
    [Table("vw_BusinessEntityRestrictionByUserAccounts")]
    public class BusinessEntityRestrictionByUserAccounts
    {
        /// <summary>
        /// Gets or sets FullName
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets IsRecordDeleted
        /// </summary>
        public bool IsRecordDeleted { get; set; }

        /// <summary>
        /// Gets or sets LnEmployeeId
        /// </summary>
        public Nullable<int> LnEmployeeId { get; set; }

        /// <summary>
        /// Gets or sets LnExternalPersonId
        /// </summary>
        public Nullable<int> LnExternalPersonId { get; set; }

        /// <summary>
        /// Gets or sets LnSecurityBusinessEntitiesId
        /// </summary>
        public int LnSecurityBusinessEntitiesId { get; set; }

        /// <summary>
        /// Gets or sets LnUserAccountId
        /// </summary>
        public int LnUserAccountId { get; set; }

        /// <summary>
        /// Gets or sets StatusValue
        /// </summary>
        public int StatusValue { get; set; }

        /// <summary>
        /// Gets or sets UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets Value
        /// </summary>
        public string Value { get; set; }
    }
}