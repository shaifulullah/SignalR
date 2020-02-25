namespace MDA.Security.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For UserAccount
    /// </summary>
    [Table("UserAccount")]
    public class UserAccount
    {
        /// <summary>
        /// Gets or sets CompanyObj
        /// Foreign Key LnDefaultCompanyId (Company)
        /// </summary>
        [ForeignKey("LnDefaultCompanyId")]
        public Company CompanyObj { get; set; }

        /// <summary>
        /// Gets or sets Domain
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets ExternalPersonObj
        /// Foreign Key LnExternalPersonId (ExternalPerson)
        /// </summary>
        [ForeignKey("LnExternalPersonId")]
        public ExternalPerson ExternalPersonObj { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets IsRecordDeleted
        /// </summary>
        public bool IsRecordDeleted { get; set; }

        /// <summary>
        /// Gets or sets LnDefaultCompanyId
        /// </summary>
        public int LnDefaultCompanyId { get; set; }

        /// <summary>
        /// Gets or sets LnEmployeeId
        /// </summary>
        public Nullable<int> LnEmployeeId { get; set; }

        /// <summary>
        /// Gets or sets LnExternalPersonId
        /// </summary>
        public Nullable<int> LnExternalPersonId { get; set; }

        /// <summary>
        /// Gets or sets UserName
        /// </summary>
        public string UserName { get; set; }
    }
}