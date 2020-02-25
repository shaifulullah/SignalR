namespace MDA.Security.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For UserDelegate
    /// </summary>
    [Table("UserDelegate")]
    public class UserDelegate
    {
        /// <summary>
        /// Gets or sets DateTimeEnd
        /// </summary>
        public DateTime DateTimeEnd { get; set; }

        /// <summary>
        /// Gets or sets DateTimeStart
        /// </summary>
        public DateTime DateTimeStart { get; set; }

        /// <summary>
        /// Gets or sets DelegateUserAccountObj
        /// Foreign Key LnDelegateUserAccountId (UserAccount)
        /// </summary>
        [ForeignKey("LnDelegateUserAccountId")]
        public UserAccount DelegateUserAccountObj { get; set; }

        /// <summary>
        /// Gets or sets FlagIsApprover
        /// </summary>
        public bool FlagIsApprover { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LnDelegateUserAccountId
        /// </summary>
        public int LnDelegateUserAccountId { get; set; }

        /// <summary>
        /// Gets or sets LnSecurityRolesId
        /// </summary>
        public int LnSecurityRolesId { get; set; }

        /// <summary>
        /// Gets or sets LnUserAccountId
        /// </summary>
        public int LnUserAccountId { get; set; }

        /// <summary>
        /// Gets or sets SecurityRolesObj
        /// Foreign Key LnSecurityRolesId (SecurityRoles)
        /// </summary>
        [ForeignKey("LnSecurityRolesId")]
        public SecurityRoles SecurityRolesObj { get; set; }

        /// <summary>
        /// Gets or sets UserAccountObj
        /// Foreign Key LnUserAccountId (UserAccount)
        /// </summary>
        [ForeignKey("LnUserAccountId")]
        public UserAccount UserAccountObj { get; set; }
    }
}