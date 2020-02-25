namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For BusinessEntityRestrictionByUser
    /// </summary>
    [Table("BusinessEntityRestrictionByUser")]
    public class BusinessEntityRestrictionByUser
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
        /// Gets or sets LnUserAccountId
        /// </summary>
        public int LnUserAccountId { get; set; }

        /// <summary>
        /// Gets or sets SecurityBusinessEntitiesObj
        /// Foreign Key LnSecurityBusinessEntitiesId (SecurityBusinessEntities)
        /// </summary>
        [ForeignKey("LnSecurityBusinessEntitiesId")]
        public SecurityBusinessEntities SecurityBusinessEntitiesObj { get; set; }

        /// <summary>
        /// Gets or sets UserAccountObj
        /// Foreign Key LnUserAccountId (UserAccount)
        /// </summary>
        [ForeignKey("LnUserAccountId")]
        public UserAccount UserAccountObj { get; set; }

        /// <summary>
        /// Gets or sets Value
        /// </summary>
        public string Value { get; set; }
    }
}