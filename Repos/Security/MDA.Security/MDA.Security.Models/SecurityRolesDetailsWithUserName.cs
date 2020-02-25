namespace MDA.Security.Models
{
    public class SecurityRolesDetailsWithUserName
    {
        /// <summary>
        /// Gets or sets Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public int Id { get; set; }

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

        /// <summary>
        /// Gets or sets UserAccountId
        /// </summary>
        public int UserAccountId { get; set; }

        /// <summary>
        /// Gets or sets UserName
        /// </summary>
        public string UserName { get; set; }
    }
}