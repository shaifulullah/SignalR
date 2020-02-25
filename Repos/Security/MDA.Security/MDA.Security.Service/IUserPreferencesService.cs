namespace MDA.Security.Service
{
    using Models;
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract(Namespace = "MDA.Security.Service")]
    public interface IUserPreferencesService
    {
        /// <summary>
        /// Get UserPreferences List For Code And Application Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [OperationContract]
        IEnumerable<UserPreferences> GetUserPreferencesListForCodeAndApplicationCode(string code, string applicationCode, int companyId);

        /// <summary>
        /// Get UserPreferences List For User In Company In Application Id
        /// </summary>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [OperationContract]
        IEnumerable<UserPreferences> GetUserPreferencesListForUserInCompanyInApplicationId(int userInCompanyInApplicationId);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userPreferences">UserPreferences Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        [OperationContract]
        bool InsertUserPreferences(UserPreferences userPreferences, string userName);

        /// <summary>
        /// Save Preferences
        /// </summary>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <param name="securityItemCode">Security Item Code</param>
        /// <param name="preferences">Preferences</param>
        /// <param name="userName">UserName of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        [OperationContract]
        bool SavePreferences(int companyInApplicationId, int userInCompanyInApplicationId, string securityItemCode, string preferences, string userName);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="userPreferences">UserPreferences Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        [OperationContract]
        bool UpdateUserPreferences(UserPreferences userPreferences, string userName);
    }
}