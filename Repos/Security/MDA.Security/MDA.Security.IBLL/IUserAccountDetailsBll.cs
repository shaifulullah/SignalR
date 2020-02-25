namespace MDA.Security.IBLL
{
    using Models;
    using System.Collections.Generic;

    public interface IUserAccountDetailsBll
    {
        /// <summary>
        /// Get UserAccountDetails List For Application Id And Company Id
        /// </summary>
        /// <param name="applicationId">Application Id</param>
        /// <param name="companyId">Company Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<UserAccountDetails> GetUserAccountDetailsListForApplicationIdAndCompanyId(int applicationId, int companyId, bool isHideTerminated);
    }
}