namespace MDA.Security.Service
{
    using Models;
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract(Namespace = "MDA.Security.Service")]
    public interface ICompanyInApplicationService
    {
        /// <summary>
        /// Get CompanyInApplication List For Application Code And User Account Id
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [OperationContract]
        IEnumerable<CompanyInApplication> GetCompanyInApplicationListForApplicationCodeAndUserAccountId(string applicationCode, int userAccountId);
    }
}