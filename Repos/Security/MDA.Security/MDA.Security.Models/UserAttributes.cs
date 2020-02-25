namespace MDA.Security.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("UserAttribute")]
    public class UserAttributes
    {
        /// <summary>
        /// Gets or sets CompanyObj
        /// Foreign Key LnDefaultCompanyId (Company)
        /// </summary>
        [ForeignKey("LnUserId")]
        public UserAccount UserAccountObject { get; set; }

        /// <summary>
        /// Gets or sets Domain
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        public int LnUserId { get; set; }
    }
}
