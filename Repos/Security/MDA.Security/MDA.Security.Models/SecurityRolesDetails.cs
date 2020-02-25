namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For SecurityRolesDetails
    /// </summary>
    [Table("vw_SecurityRolesDetails")]
    public class SecurityRolesDetails
    {
        /// <summary>
        /// Gets or sets Application
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// Gets or sets ApplicationCode
        /// </summary>
        public string ApplicationCode { get; set; }

        /// <summary>
        /// Gets or sets Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets Company
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets CompanyCode
        /// </summary>
        public string CompanyCode { get; set; }

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
        /// Gets or sets LnApplicationId
        /// </summary>
        public int LnApplicationId { get; set; }

        /// <summary>
        /// Gets or sets LnCompanyId
        /// </summary>
        public int LnCompanyId { get; set; }

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