namespace MDA.Security.Service
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract(Namespace = "MDA.Security.Service")]
    public interface IUserAccountService
    {
        /// <summary>
        /// Get UserAccount For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        [OperationContract]
        UserAccount GetUserAccountForCode(string code);

        /// <summary>
        /// Get UserAccount For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        [OperationContract]
        UserAccountDetails GetUserAccountForId(int id);

        /// <summary>
        /// Get UserAccount For User Name And Domain
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="domain">Domain</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Business Entity</returns>
        [OperationContract]
        UserAccount GetUserAccountForUserNameAndDomain(string userName, string domain, string applicationCode);

        /// <summary>
        /// Get UserAccount List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [OperationContract]
        IEnumerable<UserAccountDetails> GetUserAccountList(IEnumerable<Sort> sort, LinqFilter filter, bool isHideTerminated);

        /// <summary>
        /// Get UserAccount List For Employee Id List
        /// </summary>
        /// <param name="employeeIdList">Employee Id List</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [OperationContract]
        IEnumerable<UserAccountDetails> GetUserAccountListForEmployeeIdList(IList<int> employeeIdList, bool isHideTerminated);

        /// <summary>
        /// Get UserSecurityRights List For Application Code And User Account Id
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [OperationContract]
        IEnumerable<UserSecurityRights> GetUserSecurityRightsListForApplicationCodeAndUserAccountId(string applicationCode, int userAccountId, int companyId);

        /// <summary>
        /// Get UserSecurityRights List For Filter And Application Code
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [OperationContract]
        IEnumerable<UserSecurityRights> GetUserSecurityRightsListForFilterAndApplicationCode(LinqFilter filter, string applicationCode, int userAccountId, int companyId);
    }
}