namespace MDA.Security.Service
{
    using Models;
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract(Namespace = "MDA.Security.Service")]
    public interface IUserInCompanyInApplicationDetailsService
    {
        /// <summary>
        /// Get UserInCompanyInApplicationDetails List For Application Code And Is Hide Terminated
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [OperationContract]
        IEnumerable<UserInCompanyInApplicationDetails> GetUserInCompanyInApplicationDetailsListForApplicationCodeAndIsHideTerminated(string applicationCode, bool isHideTerminated);

        /// <summary>
        /// Get UserInCompanyInApplicationDetails List For Company Code And Application Code
        /// </summary>
        /// <param name="companyCode">Company Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [OperationContract]
        IEnumerable<UserInCompanyInApplicationDetails> GetUserInCompanyInApplicationDetailsListForCompanyCodeAndApplicationCode(string companyCode, string applicationCode, bool isHideTerminated);
    }
}