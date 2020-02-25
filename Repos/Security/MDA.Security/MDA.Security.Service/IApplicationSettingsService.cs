namespace MDA.Security.Service
{
    using Models;
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract(Namespace = "MDA.Security.Service")]
    public interface IApplicationSettingsService
    {
        /// <summary>
        /// Get ApplicationSettings List For Company Id And Application Code
        /// </summary>
        /// <param name="companyId">Company Id</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [OperationContract]
        IEnumerable<ApplicationSettings> GetApplicationSettingsListForCompanyIdAndApplicationCode(int companyId, string applicationCode);
    }
}