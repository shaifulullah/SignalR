namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For UserInCompanyInApplicationDetails
    /// </summary>
    [Table("vw_UserInCompanyInApplicationDetails")]
    public class UserInCompanyInApplicationDetails
    {
        /// <summary>
        /// Gets or sets ApplicationCode
        /// </summary>
        public string ApplicationCode { get; set; }

        /// <summary>
        /// Gets or sets CompanyCode
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        /// Gets or sets CompanyDescription
        /// </summary>
        public string CompanyDescription { get; set; }

        /// <summary>
        /// Gets or sets Domain
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets eMail
        /// </summary>
        public string eMail { get; set; }

        /// <summary>
        /// Gets or sets FullName
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }

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
        /// Gets or sets LnEmployeeId
        /// </summary>
        public int LnEmployeeId { get; set; }

        /// <summary>
        /// Gets or sets LnExternalPersonId
        /// </summary>
        public int LnExternalPersonId { get; set; }

        /// <summary>
        /// Gets or sets LnSecurityRolesId
        /// </summary>
        public int LnSecurityRolesId { get; set; }

        /// <summary>
        /// Gets or sets LnUserAccountId
        /// </summary>
        public int LnUserAccountId { get; set; }

        /// <summary>
        /// Gets or sets StatusValue
        /// </summary>
        public int StatusValue { get; set; }

        /// <summary>
        /// Gets or sets UserDetails
        /// </summary>
        public string UserDetails { get; set; }

        /// <summary>
        /// Gets or sets UserName
        /// </summary>
        public string UserName { get; set; }
    }
}