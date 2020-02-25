namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;

    public interface ISecurityRolesBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteSecurityRoles(int id, string userName);

        /// <summary>
        /// Get Available SecurityRoles Page For User Account Id
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<SecurityRoles> GetAvailableSecurityRolesPageForUserAccountId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userAccountId);

        /// <summary>
        /// Get Available SecurityRoles Page For User In Company In Application Id
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<SecurityRoles> GetAvailableSecurityRolesPageForUserInCompanyInApplicationId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userInCompanyInApplicationId);

        /// <summary>
        /// Get SecurityRolesDetails List For Company Code And Application Code
        /// </summary>
        /// <param name="companyCode">Company Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<SecurityRolesDetails> GetSecurityRolesDetailsListForCompanyCodeAndApplicationCode(string companyCode, string applicationCode);

        /// <summary>
        /// Get SecurityRolesDetailsWithUserName List For Company Code And Application Code
        /// </summary>
        /// <param name="companyCode">Company Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<SecurityRolesDetailsWithUserName> GetSecurityRolesDetailsWithUserNameListForCompanyCodeAndApplicationCode(string companyCode, string applicationCode);

        /// <summary>
        /// Get SecurityRoles For Code And Company In Application Id
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Business Entity</returns>
        SecurityRoles GetSecurityRolesForCodeAndCompanyInApplicationId(string code, int companyInApplicationId);

        /// <summary>
        /// Get SecurityRoles For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        SecurityRoles GetSecurityRolesForId(int id);

        /// <summary>
        /// Get SecurityRoles List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<SecurityRoles> GetSecurityRolesList(IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId);

        /// <summary>
        /// Get SecurityRoles List For Application Code And Company Id
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<SecurityRoles> GetSecurityRolesListForApplicationCodeAndCompanyId(string applicationCode, int companyId);

        /// <summary>
        /// Get SecurityRoles Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<SecurityRoles> GetSecurityRolesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId);

        /// <summary>
        /// Get SecurityRoles Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<SecurityRoles> GetSecurityRolesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityRoles">SecurityRoles Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertSecurityRoles(SecurityRoles securityRoles, string userName);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityRoles">SecurityRoles Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool UpdateSecurityRoles(SecurityRoles securityRoles, string userName);
    }
}