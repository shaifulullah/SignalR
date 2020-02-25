namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For SecurityRoles
    /// </summary>
    [Table("SecurityRoles")]
    public class SecurityRoles
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
        /// Gets or sets IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets LnActiveDirectoryGroupName
        /// </summary>
        public string LnActiveDirectoryGroupName { get; set; }

        /// <summary>
        /// Gets or sets LnCompanyInApplicationId
        /// </summary>
        public int LnCompanyInApplicationId { get; set; }

        /// <summary>
        /// Gets or sets LnSkillCode
        /// </summary>
        public string LnSkillCode { get; set; }
    }
}