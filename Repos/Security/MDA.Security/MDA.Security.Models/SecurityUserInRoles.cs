namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For SecurityUserInRoles
    /// </summary>
    [Table("SecurityUserInRoles")]
    public class SecurityUserInRoles
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LnSecurityRolesId
        /// </summary>
        public int LnSecurityRolesId { get; set; }

        /// <summary>
        /// Gets or sets LnUserInCompanyInApplicationId
        /// </summary>
        public int LnUserInCompanyInApplicationId { get; set; }

        /// <summary>
        /// Gets or sets SecurityRolesObj
        /// Foreign Key LnSecurityRolesId (SecurityRoles)
        /// </summary>
        [ForeignKey("LnSecurityRolesId")]
        public SecurityRoles SecurityRolesObj { get; set; }

        /// <summary>
        /// Gets or sets UserInCompanyInApplicationObj
        /// Foreign Key LnUserInCompanyInApplicationId (UserInCompanyInApplication)
        /// </summary>
        [ForeignKey("LnUserInCompanyInApplicationId")]
        public UserInCompanyInApplication UserInCompanyInApplicationObj { get; set; }
    }
}