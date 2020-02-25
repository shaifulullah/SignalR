namespace MDA.Security.Models
{
    public class UserSecurityRights
    {
        /// <summary>
        /// Gets or sets Access Rights
        /// </summary>
        public bool Access { get { return Rights.Contains("A"); } set { } }

        /// <summary>
        /// For Report Display (Access)
        /// </summary>
        public string AccessDisplay { get { return Rights.Contains("A") ? "Y" : string.Empty; } set { } }

        /// <summary>
        /// Gets or sets ActiveDirectoryGroupName
        /// </summary>
        public string ActiveDirectoryGroupName { get; set; }

        /// <summary>
        /// Gets or sets Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets Create Rights
        /// </summary>
        public bool Create { get { return Rights.Contains("C"); } set { } }

        /// <summary>
        /// For Report Display (Create)
        /// </summary>
        public string CreateDisplay { get { return Rights.Contains("C") ? "Y" : string.Empty; } set { } }

        /// <summary>
        /// Gets or sets Delete Rights
        /// </summary>
        public bool Delete { get { return Rights.Contains("D"); } set { } }

        /// <summary>
        /// For Report Display (Delete)
        /// </summary>
        public string DeleteDisplay { get { return Rights.Contains("D") ? "Y" : string.Empty; } set { } }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Execute Rights
        /// </summary>
        public bool Execute { get { return Rights.Contains("E"); } set { } }

        /// <summary>
        /// For Report Display (Execute)
        /// </summary>
        public string ExecuteDisplay { get { return Rights.Contains("E") ? "Y" : string.Empty; } set { } }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LnSecurityLevelId
        /// </summary>
        public int LnSecurityLevelId { get; set; }

        /// <summary>
        /// Gets or sets Read Rights
        /// </summary>
        public bool Read { get { return Rights.Substring(2).Contains("R"); } set { } }

        /// <summary>
        /// For Report Display (Read)
        /// </summary>
        public string ReadDisplay { get { return Rights.Substring(2).Contains("R") ? "Y" : string.Empty; } set { } }

        /// <summary>
        /// Gets or sets Rights
        /// </summary>
        public string Rights { get; set; }

        /// <summary>
        /// Gets or sets Role
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets SkillCode
        /// </summary>
        public string SkillCode { get; set; }

        /// <summary>
        /// Gets or sets Update Rights
        /// </summary>
        public bool Update { get { return Rights.Contains("U"); } set { } }

        /// <summary>
        /// For Report Display (Update)
        /// </summary>
        public string UpdateDisplay { get { return Rights.Contains("U") ? "Y" : string.Empty; } set { } }
    }
}