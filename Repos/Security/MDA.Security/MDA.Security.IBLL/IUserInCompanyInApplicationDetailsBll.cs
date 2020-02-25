namespace MDA.Security.IBLL
{
    using Models;
    using System.Collections.Generic;

    public interface IUserInCompanyInApplicationDetailsBll
    {
        /// <summary>
        /// Get UserInCompanyInApplicationDetails List For Application Code And Is Hide Terminated
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<UserInCompanyInApplicationDetails> GetUserInCompanyInApplicationDetailsListForApplicationCodeAndIsHideTerminated(string applicationCode, bool isHideTerminated);

        /// <summary>
        /// Get UserInCompanyInApplicationDetails List For Company Code And Application Code
        /// </summary>
        /// <param name="companyCode">Company Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<UserInCompanyInApplicationDetails> GetUserInCompanyInApplicationDetailsListForCompanyCodeAndApplicationCode(string companyCode, string applicationCode, bool isHideTerminated);
    }
}