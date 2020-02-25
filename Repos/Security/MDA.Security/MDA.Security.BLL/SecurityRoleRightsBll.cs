namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class SecurityRoleRightsBll : ISecurityRoleRightsBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteSecurityRoleRights(int id, string userName)
        {
            var securityRoleRightsDal = new SecurityRoleRightsDal();
            return securityRoleRightsDal.DeleteSecurityRoleRights(id, userName);
        }

        /// <summary>
        /// Get SecurityRoleRights For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public SecurityRoleRights GetSecurityRoleRightsForId(int id)
        {
            var securityRoleRightsDal = new SecurityRoleRightsDal();
            return securityRoleRightsDal.GetSecurityRoleRightsForId(id);
        }

        /// <summary>
        /// Get SecurityRoleRights List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityRightsForSecurityRole> GetSecurityRoleRightsList(IEnumerable<Sort> sort, LinqFilter filter, int securityRolesId)
        {
            var securityRoleRightsDal = new SecurityRoleRightsDal();
            return securityRoleRightsDal.GetSecurityRoleRightsList(sort, filter, securityRolesId);
        }

        /// <summary>
        /// Get SecurityRoleRights Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<SecurityRightsForSecurityRole> GetSecurityRoleRightsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityRolesId)
        {
            var securityRoleRightsDal = new SecurityRoleRightsDal();
            return securityRoleRightsDal.GetSecurityRoleRightsPage(take, skip, sort, filter, securityRolesId);
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
            var securityRoleRightsDal = new SecurityRoleRightsDal();
            var userSecurityRightsListForApplicationCodeAndUserAccountId = securityRoleRightsDal.GetUserSecurityRightsListForApplicationCodeAndUserAccountId(applicationCode, userAccountId, companyId);

            IUserAccountBll iUserAccountBll = new UserAccountBll();
            var userAccount = iUserAccountBll.GetUserAccountForId(userAccountId);

            var userSecurityRightsListForApplicationCodeAndSkillCode = string.IsNullOrEmpty(userAccount.SkillCode) ?
                new List<UserSecurityRights>() : securityRoleRightsDal.GetUserSecurityRightsListForApplicationCodeAndSkillCode(applicationCode, userAccount.SkillCode, companyId);

            //var activeDirectoryGroupName = string.Empty;
            //var domains = Forest.GetCurrentForest().Domains.Cast<Domain>();

            //foreach (var domain in domains)
            //{
            //    var root = new DirectoryEntry(string.Format("LDAP://{0}", domain.Name));
            //    var directorySearcherFilter = string.Format("(&(objectCategory=group)(|(samaccountname={0})))", userAccount.UserName);

            //    DirectorySearcher directorySearcher = new DirectorySearcher(root) { Filter = directorySearcherFilter };
            //    SearchResult searchResult = directorySearcher.FindOne();

            //    activeDirectoryGroupName = searchResult.Properties["cn"][0].ToString();
            //    break;
            //}

            //var userSecurityRightsListForApplicationCodeAndActiveDirectoryGroupName = securityRoleRightsDal.GetUserSecurityRightsListForApplicationCodeAndActiveDirectoryGroupName(applicationCode, activeDirectoryGroupName, companyId) ?? new List<UserSecurityRights>();

            var userSecurityRightsList = userSecurityRightsListForApplicationCodeAndUserAccountId.Any() ? userSecurityRightsListForApplicationCodeAndUserAccountId.ToList() : new List<UserSecurityRights>();

            userSecurityRightsList.AddRange(userSecurityRightsListForApplicationCodeAndSkillCode);
            //userSecurityRightsList.AddRange(userSecurityRightsListForApplicationCodeAndActiveDirectoryGroupName);

            return userSecurityRightsList.GroupBy(z => z.Code).Select(a => a.OrderByDescending(b => b.LnSecurityLevelId).FirstOrDefault()).ToList();
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
            var securityRoleRightsDal = new SecurityRoleRightsDal();
            return securityRoleRightsDal.GetUserSecurityRightsListForFilterAndApplicationCode(filter, applicationCode, userAccountId, companyId);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityRoleRights">SecurityRoleRights Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertSecurityRoleRights(SecurityRoleRights securityRoleRights, string userName)
        {
            var securityRoleRightsDal = new SecurityRoleRightsDal();
            return securityRoleRightsDal.InsertSecurityRoleRights(securityRoleRights, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityRoleRights">SecurityRoleRights Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateSecurityRoleRights(SecurityRoleRights securityRoleRights, string userName)
        {
            var securityRoleRightsDal = new SecurityRoleRightsDal();
            return securityRoleRightsDal.UpdateSecurityRoleRights(securityRoleRights, userName);
        }
    }
}