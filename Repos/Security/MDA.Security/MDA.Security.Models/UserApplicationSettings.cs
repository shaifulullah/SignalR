namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For UserApplicationSettings
    /// </summary>
    [Table("UserApplicationSettings")]
    public class UserApplicationSettings
    {
        /// <summary>
        /// Gets or sets CompanyObj
        /// Foreign Key LnCompanyId (Company)
        /// </summary>
        [ForeignKey("LnCompanyId")]
        public Company CompanyObj { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Language
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets LnCompanyId
        /// </summary>
        public int LnCompanyId { get; set; }

        /// <summary>
        /// Gets or sets LnUserAccountId
        /// </summary>
        public int LnUserAccountId { get; set; }

        /// <summary>
        /// Gets or sets Project
        /// </summary>
        public string Project { get; set; }

        /// <summary>
        /// Gets or sets UserAccountObj
        /// Foreign Key LnUserAccountId (UserAccount)
        /// </summary>
        [ForeignKey("LnUserAccountId")]
        public UserAccount UserAccountObj { get; set; }

        /// <summary>
        /// Gets or sets UserMenuLocation
        /// </summary>
        public string UserMenuLocation { get; set; }
    }
}