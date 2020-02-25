namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;

    public interface IUserAccountBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteUserAccount(int id, string userName);

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
        DataSourceResult<UserAccountDetails> GetAvailableUserAccountPageForCompanyInApplicationId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId, bool isHideTerminated);

        /// <summary>
        /// Get UserAccountDetails Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<UserAccountDetails> GetUserAccountDetailsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, bool isHideTerminated);

        /// <summary>
        /// Get UserAccount For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        UserAccount GetUserAccountForCode(string code);

        /// <summary>
        /// Get UserAccount For Employee Id
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Business Entity</returns>
        UserAccountDetails GetUserAccountForEmployeeId(int employeeId);

        /// <summary>
        /// Get UserAccount For External Person Id
        /// </summary>
        /// <param name="externalPersonId">External Person Id</param>
        /// <returns>Business Entity</returns>
        UserAccountDetails GetUserAccountForExternalPersonId(int externalPersonId);

        /// <summary>
        /// Get UserAccount For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        UserAccountDetails GetUserAccountForId(int id);

        /// <summary>
        /// Get UserAccount For User Name And Domain
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="domain">Domain</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Business Entity</returns>
        UserAccount GetUserAccountForUserNameAndDomain(string userName, string domain, string applicationCode);

        /// <summary>
        /// Get UserAccount Id List For Is Hide Terminated
        /// </summary>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<int> GetUserAccountIdListForIsHideTerminated(bool isHideTerminated);

        /// <summary>
        /// Get UserAccount List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<UserAccountDetails> GetUserAccountList(IEnumerable<Sort> sort, LinqFilter filter, bool isHideTerminated);

        /// <summary>
        /// Get UserAccount List For Employee Id List
        /// </summary>
        /// <param name="employeeIdList">Employee Id List</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<UserAccountDetails> GetUserAccountListForEmployeeIdList(IList<int> employeeIdList, bool isHideTerminated);

        /// <summary>
        /// Get UserAccount Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<UserAccountDetails> GetUserAccountPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, bool isHideTerminated);

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
        /// <param name="userAccount">UserAccount Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertUserAccount(UserAccount userAccount, string userName);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="userAccount">UserAccount Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool UpdateUserAccount(UserAccount userAccount, string userName);
    }
}