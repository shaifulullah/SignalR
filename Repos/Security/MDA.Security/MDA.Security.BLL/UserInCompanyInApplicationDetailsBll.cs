namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Models;
    using System.Collections.Generic;

    public class UserInCompanyInApplicationDetailsBll : IUserInCompanyInApplicationDetailsBll
    {
        /// <summary>
        /// Get UserInCompanyInApplicationDetails List For Application Code And Is Hide Terminated
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserInCompanyInApplicationDetails> GetUserInCompanyInApplicationDetailsListForApplicationCodeAndIsHideTerminated(string applicationCode, bool isHideTerminated)
        {
            var userInCompanyInApplicationDetailsDal = new UserInCompanyInApplicationDetailsDal();
            return userInCompanyInApplicationDetailsDal.GetUserInCompanyInApplicationDetailsListForApplicationCodeAndIsHideTerminated(applicationCode, isHideTerminated);
        }

        /// <summary>
        /// Get UserInCompanyInApplicationDetails List For Company Code And Application Code
        /// </summary>
        /// <param name="companyCode">Company Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserInCompanyInApplicationDetails> GetUserInCompanyInApplicationDetailsListForCompanyCodeAndApplicationCode(string companyCode, string applicationCode, bool isHideTerminated)
        {
            var userInCompanyInApplicationDetailsDal = new UserInCompanyInApplicationDetailsDal();
            return userInCompanyInApplicationDetailsDal.GetUserInCompanyInApplicationDetailsListForCompanyCodeAndApplicationCode(companyCode, applicationCode, isHideTerminated);
        }
    }
}