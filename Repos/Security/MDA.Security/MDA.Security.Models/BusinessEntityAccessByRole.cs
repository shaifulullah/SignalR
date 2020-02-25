namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For BusinessEntityAccessByRole
    /// </summary>
    [Table("BusinessEntityAccessByRole")]
    public class BusinessEntityAccessByRole
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LnSecurityBusinessEntitiesId
        /// </summary>
        public int LnSecurityBusinessEntitiesId { get; set; }

        /// <summary>
        /// Gets or sets LnSecurityRolesId
        /// </summary>
        public int LnSecurityRolesId { get; set; }

        /// <summary>
        /// Gets or sets SecurityBusinessEntitiesObj
        /// Foreign Key LnSecurityBusinessEntitiesId (SecurityBusinessEntities)
        /// </summary>
        [ForeignKey("LnSecurityBusinessEntitiesId")]
        public SecurityBusinessEntities SecurityBusinessEntitiesObj { get; set; }

        /// <summary>
        /// Gets or sets SecurityRolesObj
        /// Foreign Key LnSecurityRolesId (SecurityRoles)
        /// </summary>
        [ForeignKey("LnSecurityRolesId")]
        public SecurityRoles SecurityRolesObj { get; set; }

        /// <summary>
        /// Gets or sets Value
        /// </summary>
        public string Value { get; set; }
    }
}