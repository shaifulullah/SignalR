namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;

    public class SecurityRolesBll : ISecurityRolesBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteSecurityRoles(int id, string userName)
        {
            var securityRolesDal = new SecurityRolesDal();
            return securityRolesDal.DeleteSecurityRoles(id, userName);
        }

        /// <summary>
        /// Get Available SecurityRoles Page For User Account Id
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<SecurityRoles> GetAvailableSecurityRolesPageForUserAccountId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userAccountId)
        {
            var securityRolesDal = new SecurityRolesDal();
            return securityRolesDal.GetAvailableSecurityRolesPageForUserAccountId(take, skip, sort, filter, userAccountId);
        }

        /// <summary>
        /// Get Available SecurityRoles Page For User In Company In Application Id
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<SecurityRoles> GetAvailableSecurityRolesPageForUserInCompanyInApplicationId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userInCompanyInApplicationId)
        {
            var securityRolesDal = new SecurityRolesDal();
            return securityRolesDal.GetAvailableSecurityRolesPageForUserInCompanyInApplicationId(take, skip, sort, filter, userInCompanyInApplicationId);
        }

        /// <summary>
        /// Get SecurityRolesDetails List For Company Code And Application Code
        /// </summary>
        /// <param name="companyCode">Company Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityRolesDetails> GetSecurityRolesDetailsListForCompanyCodeAndApplicationCode(string companyCode, string applicationCode)
        {
            var securityRolesDal = new SecurityRolesDal();
            return securityRolesDal.GetSecurityRolesDetailsListForCompanyCodeAndApplicationCode(companyCode, applicationCode);
        }

        /// <summary>
        /// Get SecurityRolesDetailsWithUserName List For Company Code And Application Code
        /// </summary>
        /// <param name="companyCode">Company Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityRolesDetailsWithUserName> GetSecurityRolesDetailsWithUserNameListForCompanyCodeAndApplicationCode(string companyCode, string applicationCode)
        {
            var securityRolesDal = new SecurityRolesDal();
            return securityRolesDal.GetSecurityRolesDetailsWithUserNameListForCompanyCodeAndApplicationCode(companyCode, applicationCode);
        }

        /// <summary>
        /// Get SecurityRoles For Code And Company In Application Id
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Business Entity</returns>
        public SecurityRoles GetSecurityRolesForCodeAndCompanyInApplicationId(string code, int companyInApplicationId)
        {
            var securityRolesDal = new SecurityRolesDal();
            return securityRolesDal.GetSecurityRolesForCodeAndCompanyInApplicationId(code, companyInApplicationId);
        }

        /// <summary>
        /// Get SecurityRoles For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public SecurityRoles GetSecurityRolesForId(int id)
        {
            var securityRolesDal = new SecurityRolesDal();
            return securityRolesDal.GetSecurityRolesForId(id);
        }

        /// <summary>
        /// Get SecurityRoles List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityRoles> GetSecurityRolesList(IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId)
        {
            var securityRolesDal = new SecurityRolesDal();
            return securityRolesDal.GetSecurityRolesList(sort, filter, companyInApplicationId);
        }

        /// <summary>
        /// Get SecurityRoles List For Application Code And Company Id
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityRoles> GetSecurityRolesListForApplicationCodeAndCompanyId(string applicationCode, int companyId)
        {
            var securityRolesDal = new SecurityRolesDal();
            return securityRolesDal.GetSecurityRolesListForApplicationCodeAndCompanyId(applicationCode, companyId);
        }

        /// <summary>
        /// Get SecurityRoles Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<SecurityRoles> GetSecurityRolesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId)
        {
            var securityRolesDal = new SecurityRolesDal();
            return securityRolesDal.GetSecurityRolesPage(take, skip, sort, filter, companyInApplicationId);
        }

        /// <summary>
        /// Get SecurityRoles Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<SecurityRoles> GetSecurityRolesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            var securityRolesDal = new SecurityRolesDal();
            return securityRolesDal.GetSecurityRolesPage(take, skip, sort, filter);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityRoles">SecurityRoles Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertSecurityRoles(SecurityRoles securityRoles, string userName)
        {
            var securityRolesDal = new SecurityRolesDal();
            return securityRolesDal.InsertSecurityRoles(securityRoles, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityRoles">SecurityRoles Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateSecurityRoles(SecurityRoles securityRoles, string userName)
        {
            var securityRolesDal = new SecurityRolesDal();
            return securityRolesDal.UpdateSecurityRoles(securityRoles, userName);
        }
    }
}