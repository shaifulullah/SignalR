namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;

    public class UserSettingsBll : IUserSettingsBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteUserSettings(int id, string userName)
        {
            var userSettingsDal = new UserSettingsDal();
            return userSettingsDal.DeleteUserSettings(id, userName);
        }

        /// <summary>
        /// Get UserSettings For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public UserSettings GetUserSettingsForId(int id)
        {
            var userSettingsDal = new UserSettingsDal();
            return userSettingsDal.GetUserSettingsForId(id);
        }

        /// <summary>
        /// Get UserSettings For Key Name And User In Company In Application Id
        /// </summary>
        /// <param name="keyName">Key Name</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Business Entity</returns>
        public UserSettings GetUserSettingsForKeyNameAndUserInCompanyInApplicationId(string keyName, int userInCompanyInApplicationId)
        {
            var userSettingsDal = new UserSettingsDal();
            return userSettingsDal.GetUserSettingsForKeyNameAndUserInCompanyInApplicationId(keyName, userInCompanyInApplicationId);
        }

        /// <summary>
        /// Get UserSettings List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserSettings> GetUserSettingsList(IEnumerable<Sort> sort, LinqFilter filter, int userInCompanyInApplicationId)
        {
            var userSettingsDal = new UserSettingsDal();
            return userSettingsDal.GetUserSettingsList(sort, filter, userInCompanyInApplicationId);
        }

        /// <summary>
        /// Get UserSettings List For User Account Id And Company Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="companyId">Company Id</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserSettings> GetUserSettingsListForUserAccountIdAndCompanyId(int userAccountId, int companyId, string applicationCode)
        {
            var userSettingsDal = new UserSettingsDal();
            return userSettingsDal.GetUserSettingsListForUserAccountIdAndCompanyId(userAccountId, companyId, applicationCode);
        }

        /// <summary>
        /// Get UserSettings Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<UserSettings> GetUserSettingsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userInCompanyInApplicationId)
        {
            var userSettingsDal = new UserSettingsDal();
            return userSettingsDal.GetUserSettingsPage(take, skip, sort, filter, userInCompanyInApplicationId);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userSettings">UserSettings Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertUserSettings(UserSettings userSettings, string userName)
        {
            var userSettingsDal = new UserSettingsDal();
            return userSettingsDal.InsertUserSettings(userSettings, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="userSettings">UserSettings Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateUserSettings(UserSettings userSettings, string userName)
        {
            var userSettingsDal = new UserSettingsDal();
            return userSettingsDal.UpdateUserSettings(userSettings, userName);
        }
    }
}