namespace MDA.Security.Service
{
    using Models;
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract(Namespace = "MDA.Security.Service")]
    public interface IUserSettingsService
    {
        /// <summary>
        /// Get UserSettings For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        [OperationContract]
        UserSettings GetUserSettingsForId(int id);

        /// <summary>
        /// Get UserSettings List For User Account Id And Company Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="companyId">Company Id</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [OperationContract]
        IEnumerable<UserSettings> GetUserSettingsListForUserAccountIdAndCompanyId(int userAccountId, int companyId, string applicationCode);
    }
}