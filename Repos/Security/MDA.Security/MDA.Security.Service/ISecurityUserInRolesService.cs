namespace MDA.Security.Service
{
    using Models;
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract(Namespace = "MDA.Security.Service")]
    public interface ISecurityUserInRolesService
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        [OperationContract]
        bool DeleteSecurityUserInRoles(int id, string userName);

        /// <summary>
        /// Delete SecurityUserInRoles For Security Roles Id
        /// </summary>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        [OperationContract]
        bool DeleteSecurityUserInRolesForSecurityRolesId(int securityRolesId, string userName);

        /// <summary>
        /// Get SecurityUserInRoles For Security Role Code And Application Code
        /// </summary>
        /// <param name="securityRoleCode">Security Role Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyCode">Company Code</param>
        /// <returns>Business Entity</returns>
        [OperationContract]
        SecurityUserInRoles GetSecurityUserInRolesForSecurityRoleCodeAndApplicationCode(string securityRoleCode, string applicationCode, string companyCode);

        /// <summary>
        /// Get SecurityUserInRoles List For Application Code And Company Id
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyId">Company Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [OperationContract]
        IEnumerable<SecurityUserInRoles> GetSecurityUserInRolesListForApplicationCodeAndCompanyId(string applicationCode, int companyId, bool isHideTerminated);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityUserInRoles">SecurityUserInRoles Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        [OperationContract]
        bool InsertSecurityUserInRoles(SecurityUserInRoles securityUserInRoles, string userName);

        /// <summary>
        /// Is SecurityUserInRoles Exists For Security Role Code And User Account Id
        /// </summary>
        /// <param name="securityRoleCode">Security Role Code</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyCode">Company Code</param>
        /// <returns>True if SecurityUserInRoles Exists, else False</returns>
        [OperationContract]
        bool IsSecurityUserInRolesExistsForSecurityRoleCodeAndUserAccountId(string securityRoleCode, int userAccountId, string applicationCode, string companyCode);
    }
}