namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class UserAccountBll : IUserAccountBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteUserAccount(int id, string userName)
        {
            var userAccountDal = new UserAccountDal();
            return userAccountDal.DeleteUserAccount(id, userName);
        }

        /// <summary>
        /// Get Available UserAccount Page For Company In Application Id
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<UserAccountDetails> GetAvailableUserAccountPageForCompanyInApplicationId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId, bool isHideTerminated)
        {
            IUserInCompanyInApplicationBll iUserInCompanyInApplicationBll = new UserInCompanyInApplicationBll();
            var userInCompanyInApplicationListForCompanyInApplicationId = iUserInCompanyInApplicationBll.GetUserInCompanyInApplicationListForCompanyInApplicationId(companyInApplicationId);

            var userAccountIdList = userInCompanyInApplicationListForCompanyInApplicationId == null ? new List<int>() :
                userInCompanyInApplicationListForCompanyInApplicationId.Select(x => x.LnUserAccountId).ToList();

            var userAccountDal = new UserAccountDal();
            return userAccountDal.GetAvailableUserAccountPageForUserAccountIdList(take, skip, sort, filter, userAccountIdList, isHideTerminated);
        }

        /// <summary>
        /// Get UserAccountDetails Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<UserAccountDetails> GetUserAccountDetailsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, bool isHideTerminated)
        {
            var userAccountDal = new UserAccountDal();
            return userAccountDal.GetUserAccountDetailsPage(take, skip, sort, filter, isHideTerminated);
        }

        /// <summary>
        /// Get UserAccount For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public UserAccount GetUserAccountForCode(string code)
        {
            var userAccountDal = new UserAccountDal();
            return userAccountDal.GetUserAccountForCode(code);
        }

        /// <summary>
        /// Get UserAccount For Employee Id
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Business Entity</returns>
        public UserAccountDetails GetUserAccountForEmployeeId(int employeeId)
        {
            var userAccountDal = new UserAccountDal();
            return userAccountDal.GetUserAccountForEmployeeId(employeeId);
        }

        /// <summary>
        /// Get UserAccount For External Person Id
        /// </summary>
        /// <param name="externalPersonId">External Person Id</param>
        /// <returns>Business Entity</returns>
        public UserAccountDetails GetUserAccountForExternalPersonId(int externalPersonId)
        {
            var userAccountDal = new UserAccountDal();
            return userAccountDal.GetUserAccountForExternalPersonId(externalPersonId);
        }

        /// <summary>
        /// Get UserAccount For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public UserAccountDetails GetUserAccountForId(int id)
        {
            var userAccountDal = new UserAccountDal();
            return userAccountDal.GetUserAccountForId(id);
        }

        /// <summary>
        /// Get UserAccount For User Name And Domain
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="domain">Domain</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Business Entity</returns>
        public UserAccount GetUserAccountForUserNameAndDomain(string userName, string domain, string applicationCode)
        {
            var userAccountDal = new UserAccountDal();
            return userAccountDal.GetUserAccountForUserNameAndDomain(userName, domain, applicationCode);
        }

        /// <summary>
        /// Get UserAccount Id List For Is Hide Terminated
        /// </summary>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<int> GetUserAccountIdListForIsHideTerminated(bool isHideTerminated)
        {
            var userAccountDal = new UserAccountDal();
            return userAccountDal.GetUserAccountIdListForIsHideTerminated(isHideTerminated);
        }

        /// <summary>
        /// Get UserAccount List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserAccountDetails> GetUserAccountList(IEnumerable<Sort> sort, LinqFilter filter, bool isHideTerminated)
        {
            var userAccountDal = new UserAccountDal();
            return userAccountDal.GetUserAccountList(sort, filter, isHideTerminated);
        }

        /// <summary>
        /// Get UserAccount List For Employee Id List
        /// </summary>
        /// <param name="employeeIdList">Employee Id List</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserAccountDetails> GetUserAccountListForEmployeeIdList(IList<int> employeeIdList, bool isHideTerminated)
        {
            var userAccountDal = new UserAccountDal();
            return userAccountDal.GetUserAccountListForEmployeeIdList(employeeIdList, isHideTerminated);
        }

        /// <summary>
        /// Get UserAccount Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<UserAccountDetails> GetUserAccountPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, bool isHideTerminated)
        {
            var userAccountDal = new UserAccountDal();
            return userAccountDal.GetUserAccountPage(take, skip, sort, filter, isHideTerminated);
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
            // Get Role Rights
            ISecurityRoleRightsBll iSecurityRoleRightsBll = new SecurityRoleRightsBll();
            var securityRoleRightsList = iSecurityRoleRightsBll.GetUserSecurityRightsListForApplicationCodeAndUserAccountId(applicationCode, userAccountId, companyId);

            // Get User Rights
            ISecurityUserRightsBll iSecurityUserRightsBll = new SecurityUserRightsBll();
            var securityUserRightsList = iSecurityUserRightsBll.GetUserSecurityRightsListForApplicationCodeAndUserAccountId(applicationCode, userAccountId, companyId);

            return securityRoleRightsList.Union(securityUserRightsList).ToList();
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
            // Get Security Role Rights
            ISecurityRoleRightsBll iSecurityRoleRightsBll = new SecurityRoleRightsBll();
            var securityRoleRightsList = iSecurityRoleRightsBll.GetUserSecurityRightsListForFilterAndApplicationCode(filter, applicationCode, userAccountId, companyId);

            // Get Security User Rights
            ISecurityUserRightsBll iSecurityUserRightsBll = new SecurityUserRightsBll();
            var securityUserRightsList = iSecurityUserRightsBll.GetUserSecurityRightsListForFilterAndApplicationCode(filter, applicationCode, userAccountId, companyId);

            return securityRoleRightsList.Union(securityUserRightsList).ToList();
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userAccount">UserAccount Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertUserAccount(UserAccount userAccount, string userName)
        {
            var userAccountDal = new UserAccountDal();
            return userAccountDal.InsertUserAccount(userAccount, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="userAccount">UserAccount Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateUserAccount(UserAccount userAccount, string userName)
        {
            var userAccountDal = new UserAccountDal();
            return userAccountDal.UpdateUserAccount(userAccount, userName);
        }
    }
}