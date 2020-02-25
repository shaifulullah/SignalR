namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;

    public interface ISecurityUserInRolesBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteSecurityUserInRoles(int id, string userName);

        /// <summary>
        /// Delete SecurityUserInRoles For Security Roles Id
        /// </summary>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteSecurityUserInRolesForSecurityRolesId(int securityRolesId, string userName);

        /// <summary>
        /// Get SecurityUserInRoles For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        SecurityUserInRoles GetSecurityUserInRolesForId(int id);

        /// <summary>
        /// Get SecurityUserInRoles For Security Role Code And Application Code
        /// </summary>
        /// <param name="securityRoleCode">Security Role Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyCode">Company Code</param>
        /// <returns>Business Entity</returns>
        SecurityUserInRoles GetSecurityUserInRolesForSecurityRoleCodeAndApplicationCode(string securityRoleCode, string applicationCode, string companyCode);

        /// <summary>
        /// Get SecurityUserInRoles List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<UserAccountDetails> GetSecurityUserInRolesList(IEnumerable<Sort> sort, LinqFilter filter, int securityRolesId, bool isHideTerminated);

        /// <summary>
        /// Get SecurityUserInRoles List For Application Code And Company Id
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyId">Company Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<SecurityUserInRoles> GetSecurityUserInRolesListForApplicationCodeAndCompanyId(string applicationCode, int companyId, bool isHideTerminated);

        /// <summary>
        /// Get SecurityUserInRoles List For Application Id And Company Id
        /// </summary>
        /// <param name="applicationId">Application Id</param>
        /// <param name="companyId">Company Id</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<SecurityUserInRoles> GetSecurityUserInRolesListForApplicationIdAndCompanyId(int applicationId, int companyId, int userAccountId);

        /// <summary>
        /// Get SecurityUserInRoles List For Security Roles Id
        /// </summary>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<SecurityUserInRoles> GetSecurityUserInRolesListForSecurityRolesId(int securityRolesId);

        /// <summary>
        /// Get SecurityUserInRoles List For User In Company In Application Id
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<SecurityRolesDetails> GetSecurityUserInRolesListForUserInCompanyInApplicationId(IEnumerable<Sort> sort, LinqFilter filter, int userInCompanyInApplicationId);

        /// <summary>
        /// Get SecurityUserInRoles Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<UserAccountDetails> GetSecurityUserInRolesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityRolesId, bool isHideTerminated);

        /// <summary>
        /// Get SecurityUserInRoles Page For User In Company In Application Id
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<SecurityRolesDetails> GetSecurityUserInRolesPageForUserInCompanyInApplicationId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userInCompanyInApplicationId);

        /// <summary>
        /// Get UserAccount Id List For Security Roles Id
        /// </summary>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<int> GetUserAccountIdListForSecurityRolesId(int securityRolesId, int userAccountId);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityUserInRoles">SecurityUserInRoles Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertSecurityUserInRoles(SecurityUserInRoles securityUserInRoles, string userName);

        /// <summary>
        /// Is SecurityUserInRoles Exists For Security Role Code And User Account Id
        /// </summary>
        /// <param name="securityRoleCode">Security Role Code</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyCode">Company Code</param>
        /// <returns>True if SecurityUserInRoles Exists, else False</returns>
        bool IsSecurityUserInRolesExistsForSecurityRoleCodeAndUserAccountId(string securityRoleCode, int userAccountId, string applicationCode, string companyCode);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityUserInRoles">SecurityUserInRoles Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool UpdateSecurityUserInRoles(SecurityUserInRoles securityUserInRoles, string userName);
    }
}