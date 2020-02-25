namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;

    public interface ISecurityRoleRightsBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteSecurityRoleRights(int id, string userName);

        /// <summary>
        /// Get SecurityRoleRights For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        SecurityRoleRights GetSecurityRoleRightsForId(int id);

        /// <summary>
        /// Get SecurityRoleRights List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<SecurityRightsForSecurityRole> GetSecurityRoleRightsList(IEnumerable<Sort> sort, LinqFilter filter, int securityRolesId);

        /// <summary>
        /// Get SecurityRoleRights Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<SecurityRightsForSecurityRole> GetSecurityRoleRightsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityRolesId);

        /// <summary>
        /// Get UserSecurityRights List For Application Code And User Account Id
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<UserSecurityRights> GetUserSecurityRightsListForApplicationCodeAndUserAccountId(string applicationCode, int userAccountId, int companyId);

        /// <summary>
        /// Get UserSecurityRights List For Filter And Application Code
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<UserSecurityRights> GetUserSecurityRightsListForFilterAndApplicationCode(LinqFilter filter, string applicationCode, int userAccountId, int companyId);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityRoleRights">SecurityRoleRights Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertSecurityRoleRights(SecurityRoleRights securityRoleRights, string userName);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityRoleRights">SecurityRoleRights Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool UpdateSecurityRoleRights(SecurityRoleRights securityRoleRights, string userName);
    }
}