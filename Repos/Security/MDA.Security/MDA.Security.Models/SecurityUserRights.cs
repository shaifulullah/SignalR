namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For SecurityUserRights
    /// </summary>
    [Table("SecurityUserRights")]
    public class SecurityUserRights
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LnSecurityItemId
        /// </summary>
        public int LnSecurityItemId { get; set; }

        /// <summary>
        /// Gets or sets LnSecurityLevelId
        /// </summary>
        public int LnSecurityLevelId { get; set; }

        /// <summary>
        /// Gets or sets LnUserInCompanyInApplicationId
        /// </summary>
        public int LnUserInCompanyInApplicationId { get; set; }

        /// <summary>
        /// Gets or sets SecurityItemObj
        /// Foreign Key LnSecurityItemId (SecurityItem)
        /// </summary>
        [ForeignKey("LnSecurityItemId")]
        public SecurityItem SecurityItemObj { get; set; }

        /// <summary>
        /// Gets or sets SecurityLevelObj
        /// Foreign Key LnSecurityLevelId (SecurityLevel)
        /// </summary>
        [ForeignKey("LnSecurityLevelId")]
        public SecurityLevel SecurityLevelObj { get; set; }

        /// <summary>
        /// Gets or sets UserInCompanyInApplicationObj
        /// Foreign Key LnUserInCompanyInApplicationId (UserInCompanyInApplication)
        /// </summary>
        [ForeignKey("LnUserInCompanyInApplicationId")]
        public UserInCompanyInApplication UserInCompanyInApplicationObj { get; set; }
    }
}