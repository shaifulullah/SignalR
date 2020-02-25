namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For BusinessEntityAccessByAD
    /// </summary>
    [Table("BusinessEntityAccessByAD")]
    public class BusinessEntityAccessByAD
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LnActiveDirectoryGroupName
        /// </summary>
        public string LnActiveDirectoryGroupName { get; set; }

        /// <summary>
        /// Gets or sets LnSecurityBusinessEntitiesId
        /// </summary>
        public int LnSecurityBusinessEntitiesId { get; set; }

        /// <summary>
        /// Gets or sets SecurityBusinessEntitiesObj
        /// Foreign Key LnSecurityBusinessEntitiesId (SecurityBusinessEntities)
        /// </summary>
        [ForeignKey("LnSecurityBusinessEntitiesId")]
        public SecurityBusinessEntities SecurityBusinessEntitiesObj { get; set; }

        /// <summary>
        /// Gets or sets Value
        /// </summary>
        public string Value { get; set; }
    }
}