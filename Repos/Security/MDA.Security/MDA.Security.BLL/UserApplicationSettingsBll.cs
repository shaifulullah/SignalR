namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Models;

    public class UserApplicationSettingsBll : IUserApplicationSettingsBll
    {
        /// <summary>
        /// Get UserApplicationSettings For User Account Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Business Entity</returns>
        public UserApplicationSettings GetUserApplicationSettingsForUserAccountId(int userAccountId)
        {
            var userApplicationSettingsDal = new UserApplicationSettingsDal();
            return userApplicationSettingsDal.GetUserApplicationSettingsForUserAccountId(userAccountId);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userApplicationSettings">UserApplicationSettings Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertUserApplicationSettings(UserApplicationSettings userApplicationSettings, string userName)
        {
            var userApplicationSettingsDal = new UserApplicationSettingsDal();
            return userApplicationSettingsDal.InsertUserApplicationSettings(userApplicationSettings, userName);
        }

        /// <summary>
        /// Insert UserApplicationSettings For User Account Id And Company Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="companyId">Company Id</param>
        /// <param name="language">Language</param>
        /// <param name="project">Project</param>
        /// <param name="userMenuLocation">User Menu Location</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertUserApplicationSettingsForUserAccountIdAndCompanyId(int userAccountId, int companyId, string language, string project, string userMenuLocation, string userName)
        {
            var userApplicationSettingsDal = new UserApplicationSettingsDal();
            return userApplicationSettingsDal.InsertUserApplicationSettings(new UserApplicationSettings { Language = language, LnCompanyId = companyId, LnUserAccountId = userAccountId, Project = project, UserMenuLocation = userMenuLocation }, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="userApplicationSettings">UserApplicationSettings Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateUserApplicationSettings(UserApplicationSettings userApplicationSettings, string userName)
        {
            var userApplicationSettingsDal = new UserApplicationSettingsDal();
            return userApplicationSettingsDal.UpdateUserApplicationSettings(userApplicationSettings, userName);
        }
    }
}