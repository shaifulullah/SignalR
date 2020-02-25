namespace MDA.Security.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For UserAccountDetails
    /// </summary>
    [Table("vw_UserAccountDetails")]
    public class UserAccountDetails
    {
        /// <summary>
        /// Gets or sets Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets Company
        /// </summary>
        public string Company { get; set; }

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
        /// Gets or sets IsRecordDeleted
        /// </summary>
        public bool IsRecordDeleted { get; set; }

        /// <summary>
        /// Gets or sets LnDefaultCompanyId
        /// </summary>
        public int LnDefaultCompanyId { get; set; }

        /// <summary>
        /// Gets or sets LnEmployeeId
        /// </summary>
        public Nullable<int> LnEmployeeId { get; set; }

        /// <summary>
        /// Gets or sets LnExternalPersonId
        /// </summary>
        public Nullable<int> LnExternalPersonId { get; set; }

        /// <summary>
        /// Gets or sets SecurityRightsForUserAccount List
        /// </summary>
        public IEnumerable<SecurityRightsForUserAccount> SecurityRightsForUserAccountList { get; set; }

        /// <summary>
        /// Get the Security Roles as a Comma Delimited string
        /// </summary>
        public string SecurityRoles
        {
            get
            {
                var securityRoles = string.Empty;
                if (SecurityUserInRolesList != null)
                {
                    foreach (var securityRole in SecurityUserInRolesList)
                    {
                        securityRoles += string.Format("{0}{1}", securityRole.SecurityRolesObj.Description, Environment.NewLine);
                    }

                    return securityRoles.TrimEnd(Environment.NewLine.ToCharArray());
                }

                return securityRoles;
            }
        }

        /// <summary>
        /// Gets or sets SecurityUserInRoles List
        /// </summary>
        public IEnumerable<SecurityUserInRoles> SecurityUserInRolesList { get; set; }

        /// <summary>
        /// Gets or sets SkillCode
        /// </summary>
        public string SkillCode { get; set; }

        /// <summary>
        /// Gets or sets StatusValue
        /// </summary>
        public int StatusValue { get; set; }

        /// <summary>
        /// Gets or sets UserAccountType
        /// </summary>
        public string UserAccountType { get; set; }

        /// <summary>
        /// Gets or sets UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets the User Security Rights as a Comma Delimited string
        /// </summary>
        public string UserSecurityRights
        {
            get
            {
                var userSecurityRights = string.Empty;
                if (SecurityRightsForUserAccountList != null)
                {
                    foreach (var securityRightsForUserAccount in SecurityRightsForUserAccountList)
                    {
                        userSecurityRights += string.Format("{0} - {1}{2}", securityRightsForUserAccount.SecurityItemCode, securityRightsForUserAccount.Rights.Replace('_', '-'), Environment.NewLine);
                    }

                    return userSecurityRights.TrimEnd(Environment.NewLine.ToCharArray());
                }

                return userSecurityRights;
            }
        }
    }
}