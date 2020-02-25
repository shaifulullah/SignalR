namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;

    public class SecurityUserRightsBll : ISecurityUserRightsBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteSecurityUserRights(int id, string userName)
        {
            var securityUserRightsDal = new SecurityUserRightsDal();
            return securityUserRightsDal.DeleteSecurityUserRights(id, userName);
        }

        /// <summary>
        /// Get SecurityUserRights For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public SecurityUserRights GetSecurityUserRightsForId(int id)
        {
            var securityUserRightsDal = new SecurityUserRightsDal();
            return securityUserRightsDal.GetSecurityUserRightsForId(id);
        }

        /// <summary>
        /// Get SecurityUserRights List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityRightsForUserAccount> GetSecurityUserRightsList(IEnumerable<Sort> sort, LinqFilter filter, int userInCompanyInApplicationId)
        {
            var securityUserRightsDal = new SecurityUserRightsDal();
            return securityUserRightsDal.GetSecurityUserRightsList(sort, filter, userInCompanyInApplicationId);
        }

        /// <summary>
        /// Get SecurityUserRights List For Application Id And Company Id
        /// </summary>
        /// <param name="applicationId">Application Id</param>
        /// <param name="companyId">Company Id</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityRightsForUserAccount> GetSecurityUserRightsListForApplicationIdAndCompanyId(int applicationId, int companyId, int userAccountId)
        {
            var securityUserRightsDal = new SecurityUserRightsDal();
            return securityUserRightsDal.GetSecurityUserRightsListForApplicationIdAndCompanyId(applicationId, companyId, userAccountId);
        }

        /// <summary>
        /// Get SecurityUserRights Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<SecurityRightsForUserAccount> GetSecurityUserRightsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userInCompanyInApplicationId)
        {
            var securityUserRightsDal = new SecurityUserRightsDal();
            return securityUserRightsDal.GetSecurityUserRightsPage(take, skip, sort, filter, userInCompanyInApplicationId);
        }

        /// <summary>
        /// Get UserSecurityRights List For Application Code And User Account Id
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserSecurityRights> GetUserSecurityRightsListForApplicationCodeAndUserAccountId(string applicationCode, int userAccountId, int companyId)
        {
            var securityUserRightsDal = new SecurityUserRightsDal();
            return securityUserRightsDal.GetUserSecurityRightsListForApplicationCodeAndUserAccountId(applicationCode, userAccountId, companyId);
        }

        /// <summary>
        /// Get UserSecurityRights List For Filter And Application Code
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserSecurityRights> GetUserSecurityRightsListForFilterAndApplicationCode(LinqFilter filter, string applicationCode, int userAccountId, int companyId)
        {
            var securityUserRightsDal = new SecurityUserRightsDal();
            return securityUserRightsDal.GetUserSecurityRightsListForFilterAndApplicationCode(filter, applicationCode, userAccountId, companyId);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityUserRights">SecurityUserRights Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertSecurityUserRights(SecurityUserRights securityUserRights, string userName)
        {
            var securityUserRightsDal = new SecurityUserRightsDal();
            return securityUserRightsDal.InsertSecurityUserRights(securityUserRights, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityUserRights">SecurityUserRights Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateSecurityUserRights(SecurityUserRights securityUserRights, string userName)
        {
            var securityUserRightsDal = new SecurityUserRightsDal();
            return securityUserRightsDal.UpdateSecurityUserRights(securityUserRights, userName);
        }
    }
}