namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For SecurityItem
    /// </summary>
    [Table("SecurityItem")]
    public class SecurityItem
    {
        /// <summary>
        /// Gets or sets Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets CompanyInApplicationObj
        /// Foreign Key LnCompanyInApplicationId (CompanyInApplication)
        /// </summary>
        [ForeignKey("LnCompanyInApplicationId")]
        public CompanyInApplication CompanyInApplicationObj { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LnCompanyInApplicationId
        /// </summary>
        public int LnCompanyInApplicationId { get; set; }

        /// <summary>
        /// Gets or sets LnSecurityTypeId
        /// </summary>
        public int LnSecurityTypeId { get; set; }

        /// <summary>
        /// Gets or sets SecurityTypeObj
        /// Foreign Key LnSecurityTypeId (SecurityType)
        /// </summary>
        [ForeignKey("LnSecurityTypeId")]
        public SecurityType SecurityTypeObj { get; set; }
    }
}