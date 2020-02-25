namespace MDA.Security.IBLL
{
    using Models;

    public interface IUserApplicationSettingsBll
    {
        /// <summary>
        /// Get UserApplicationSettings For User Account Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Business Entity</returns>
        UserApplicationSettings GetUserApplicationSettingsForUserAccountId(int userAccountId);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userApplicationSettings">UserApplicationSettings Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertUserApplicationSettings(UserApplicationSettings userApplicationSettings, string userName);

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
        bool InsertUserApplicationSettingsForUserAccountIdAndCompanyId(int userAccountId, int companyId, string language, string project, string userMenuLocation, string userName);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="userApplicationSettings">UserApplicationSettings Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool UpdateUserApplicationSettings(UserApplicationSettings userApplicationSettings, string userName);
    }
}