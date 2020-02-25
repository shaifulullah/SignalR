namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;

    public class SecurityUserInRolesBll : ISecurityUserInRolesBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteSecurityUserInRoles(int id, string userName)
        {
            var securityUserInRolesDal = new SecurityUserInRolesDal();
            return securityUserInRolesDal.DeleteSecurityUserInRoles(id, userName);
        }

        /// <summary>
        /// Delete SecurityUserInRoles For Security Roles Id
        /// </summary>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteSecurityUserInRolesForSecurityRolesId(int securityRolesId, string userName)
        {
            var securityUserInRolesDal = new SecurityUserInRolesDal();
            return securityUserInRolesDal.DeleteSecurityUserInRolesForSecurityRolesId(securityRolesId, userName);
        }

        /// <summary>
        /// Get SecurityUserInRoles For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public SecurityUserInRoles GetSecurityUserInRolesForId(int id)
        {
            var securityUserInRolesDal = new SecurityUserInRolesDal();
            return securityUserInRolesDal.GetSecurityUserInRolesForId(id);
        }

        /// <summary>
        /// Get SecurityUserInRoles For Security Role Code And Application Code
        /// </summary>
        /// <param name="securityRoleCode">Security Role Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyCode">Company Code</param>
        /// <returns>Business Entity</returns>
        public SecurityUserInRoles GetSecurityUserInRolesForSecurityRoleCodeAndApplicationCode(string securityRoleCode, string applicationCode, string companyCode)
        {
            var securityUserInRolesDal = new SecurityUserInRolesDal();
            return securityUserInRolesDal.GetSecurityUserInRolesForSecurityRoleCodeAndApplicationCode(securityRoleCode, applicationCode, companyCode);
        }

        /// <summary>
        /// Get SecurityUserInRoles List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserAccountDetails> GetSecurityUserInRolesList(IEnumerable<Sort> sort, LinqFilter filter, int securityRolesId, bool isHideTerminated)
        {
            var securityUserInRolesDal = new SecurityUserInRolesDal();
            return securityUserInRolesDal.GetSecurityUserInRolesList(sort, filter, securityRolesId, isHideTerminated);
        }

        /// <summary>
        /// Get SecurityUserInRoles List For Application Code And Company Id
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyId">Company Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityUserInRoles> GetSecurityUserInRolesListForApplicationCodeAndCompanyId(string applicationCode, int companyId, bool isHideTerminated)
        {
            IUserAccountBll iUserAccountBll = new UserAccountBll();
            var userAccountIdList = iUserAccountBll.GetUserAccountIdListForIsHideTerminated(isHideTerminated);

            var userAccountIds = userAccountIdList ?? new List<int>();

            var securityUserInRolesDal = new SecurityUserInRolesDal();
            return securityUserInRolesDal.GetSecurityUserInRolesListForApplicationCodeAndCompanyId(applicationCode, companyId, userAccountIds);
        }

        /// <summary>
        /// Get SecurityUserInRoles List For Application Id And Company Id
        /// </summary>
        /// <param name="applicationId">Application Id</param>
        /// <param name="companyId">Company Id</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityUserInRoles> GetSecurityUserInRolesListForApplicationIdAndCompanyId(int applicationId, int companyId, int userAccountId)
        {
            var securityUserInRolesDal = new SecurityUserInRolesDal();
            return securityUserInRolesDal.GetSecurityUserInRolesListForApplicationIdAndCompanyId(applicationId, companyId, userAccountId);
        }

        /// <summary>
        /// Get SecurityUserInRoles List For Security Roles Id
        /// </summary>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityUserInRoles> GetSecurityUserInRolesListForSecurityRolesId(int securityRolesId)
        {
            var securityUserInRolesDal = new SecurityUserInRolesDal();
            return securityUserInRolesDal.GetSecurityUserInRolesListForSecurityRolesId(securityRolesId);
        }

        /// <summary>
        /// Get SecurityUserInRoles List For User In Company In Application Id
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityRolesDetails> GetSecurityUserInRolesListForUserInCompanyInApplicationId(IEnumerable<Sort> sort, LinqFilter filter, int userInCompanyInApplicationId)
        {
            var securityUserInRolesDal = new SecurityUserInRolesDal();
            return securityUserInRolesDal.GetSecurityUserInRolesListForUserInCompanyInApplicationId(sort, filter, userInCompanyInApplicationId);
        }

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
        public DataSourceResult<UserAccountDetails> GetSecurityUserInRolesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityRolesId, bool isHideTerminated)
        {
            var securityUserInRolesDal = new SecurityUserInRolesDal();
            return securityUserInRolesDal.GetSecurityUserInRolesPage(take, skip, sort, filter, securityRolesId, isHideTerminated);
        }

        /// <summary>
        /// Get SecurityUserInRoles Page For User In Company In Application Id
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<SecurityRolesDetails> GetSecurityUserInRolesPageForUserInCompanyInApplicationId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userInCompanyInApplicationId)
        {
            var securityUserInRolesDal = new SecurityUserInRolesDal();
            return securityUserInRolesDal.GetSecurityUserInRolesPageForUserInCompanyInApplicationId(take, skip, sort, filter, userInCompanyInApplicationId);
        }

        /// <summary>
        /// Get UserAccount Id List For Security Roles Id
        /// </summary>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<int> GetUserAccountIdListForSecurityRolesId(int securityRolesId, int userAccountId)
        {
            var securityUserInRolesDal = new SecurityUserInRolesDal();
            return securityUserInRolesDal.GetUserAccountIdListForSecurityRolesId(securityRolesId, userAccountId);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityUserInRoles">SecurityUserInRoles Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertSecurityUserInRoles(SecurityUserInRoles securityUserInRoles, string userName)
        {
            var securityUserInRolesDal = new SecurityUserInRolesDal();
            return securityUserInRolesDal.InsertSecurityUserInRoles(securityUserInRoles, userName);
        }

        /// <summary>
        /// Is SecurityUserInRoles Exists For Security Role Code And User Account Id
        /// </summary>
        /// <param name="securityRoleCode">Security Role Code</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyCode">Company Code</param>
        /// <returns>True if SecurityUserInRoles Exists, else False</returns>
        public bool IsSecurityUserInRolesExistsForSecurityRoleCodeAndUserAccountId(string securityRoleCode, int userAccountId, string applicationCode, string companyCode)
        {
            var securityUserInRolesDal = new SecurityUserInRolesDal();
            return securityUserInRolesDal.IsSecurityUserInRolesExistsForSecurityRoleCodeAndUserAccountId(securityRoleCode, userAccountId, applicationCode, companyCode);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityUserInRoles">SecurityUserInRoles Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateSecurityUserInRoles(SecurityUserInRoles securityUserInRoles, string userName)
        {
            var securityUserInRolesDal = new SecurityUserInRolesDal();
            return securityUserInRolesDal.UpdateSecurityUserInRoles(securityUserInRoles, userName);
        }
    }
}