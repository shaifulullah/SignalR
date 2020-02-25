namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Models;
    using System.Collections.Generic;

    public class UserPreferencesBll : IUserPreferencesBll
    {
        /// <summary>
        /// Get UserPreferences List For Code And Application Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserPreferences> GetUserPreferencesListForCodeAndApplicationCode(string code, string applicationCode, int companyId)
        {
            var userPreferencesDal = new UserPreferencesDal();
            return userPreferencesDal.GetUserPreferencesListForCodeAndApplicationCode(code, applicationCode, companyId);
        }

        /// <summary>
        /// Get UserPreferences List For User In Company In Application Id
        /// </summary>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserPreferences> GetUserPreferencesListForUserInCompanyInApplicationId(int userInCompanyInApplicationId)
        {
            var userPreferencesDal = new UserPreferencesDal();
            return userPreferencesDal.GetUserPreferencesListForUserInCompanyInApplicationId(userInCompanyInApplicationId);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userPreferences">UserPreferences Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertUserPreferences(UserPreferences userPreferences, string userName)
        {
            var userPreferencesDal = new UserPreferencesDal();
            return userPreferencesDal.InsertUserPreferences(userPreferences, userName);
        }

        /// <summary>
        /// Save Preferences
        /// </summary>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <param name="securityItemCode">Security Item Code</param>
        /// <param name="preferences">Preferences</param>
        /// <param name="userName">UserName of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool SavePreferences(int companyInApplicationId, int userInCompanyInApplicationId, string securityItemCode, string preferences, string userName)
        {
            ISecurityItemBll iSecurityItemBll = new SecurityItemBll();
            var securityItemId = iSecurityItemBll.GetSecurityItemForCodeAndCompanyInApplicationId(securityItemCode, companyInApplicationId).Id;

            var userPreferencesDal = new UserPreferencesDal();
            var userPreference = userPreferencesDal.GetUserPreferences(securityItemId, userInCompanyInApplicationId);

            var userPreferencesToUpdate = new UserPreferences
            {
                Id = userPreference == null ? 0 : userPreference.Id,
                LnSecurityItemId = securityItemId,
                LnUserInCompanyInApplicationId = userInCompanyInApplicationId,
                Preferences = preferences
            };

            return userPreference == null ?
                userPreferencesDal.InsertUserPreferences(userPreferencesToUpdate, userName) :
                userPreferencesDal.UpdateUserPreferences(userPreferencesToUpdate, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="userPreferences">UserPreferences Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateUserPreferences(UserPreferences userPreferences, string userName)
        {
            var userPreferencesDal = new UserPreferencesDal();
            return userPreferencesDal.UpdateUserPreferences(userPreferences, userName);
        }
    }
}