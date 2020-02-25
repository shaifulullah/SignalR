namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For SecurityRightsForSecurityRole
    /// </summary>
    [Table("vw_SecurityRightsForSecurityRole")]
    public class SecurityRightsForSecurityRole
    {
        /// <summary>
        /// Gets or sets AccessDisplay
        /// </summary>
        public string AccessDisplay { get; set; }

        /// <summary>
        /// Gets or sets CanAccess
        /// </summary>
        public bool CanAccess { get; set; }

        /// <summary>
        /// Gets or sets CanCreate
        /// </summary>
        public bool CanCreate { get; set; }

        /// <summary>
        /// Gets or sets CanDelete
        /// </summary>
        public bool CanDelete { get; set; }

        /// <summary>
        /// Gets or sets CanExecute
        /// </summary>
        public bool CanExecute { get; set; }

        /// <summary>
        /// Gets or sets CanRead
        /// </summary>
        public bool CanRead { get; set; }

        /// <summary>
        /// Gets or sets CanUpdate
        /// </summary>
        public bool CanUpdate { get; set; }

        /// <summary>
        /// Gets or sets CreateDisplay
        /// </summary>
        public string CreateDisplay { get; set; }

        /// <summary>
        /// Gets or sets DeleteDisplay
        /// </summary>
        public string DeleteDisplay { get; set; }

        /// <summary>
        /// Gets or sets ExecuteDisplay
        /// </summary>
        public string ExecuteDisplay { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LnSecurityRolesId
        /// </summary>
        public int LnSecurityRolesId { get; set; }

        /// <summary>
        /// Gets or sets ReadDisplay
        /// </summary>
        public string ReadDisplay { get; set; }

        /// <summary>
        /// Gets or sets Rights
        /// </summary>
        public string Rights { get; set; }

        /// <summary>
        /// Gets or sets SecurityItemCode
        /// </summary>
        public string SecurityItemCode { get; set; }

        /// <summary>
        /// Gets or sets SecurityRoleCode
        /// </summary>
        public string SecurityRoleCode { get; set; }

        /// <summary>
        /// Gets or sets SecurityTypeDescription
        /// </summary>
        public string SecurityTypeDescription { get; set; }

        /// <summary>
        /// Gets or sets UpdateDisplay
        /// </summary>
        public string UpdateDisplay { get; set; }
    }
}