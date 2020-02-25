namespace MDA.Security.Helpers
{
    using BLL;
    using Core.Helpers;
    using Globals;
    using IBLL;
    using Models;
    using System;
    using System.Globalization;

    public static class ApplicationLogin
    {
        /// <summary>
        /// Validate Application Login
        /// </summary>
        /// <param name="userIdentityName">User Identity Name</param>
        /// <param name="userHostAddress">User Host Address</param>
        /// <returns>True on Success, False on Failure</returns>
        public static bool ValidateApplicationLogin(string userIdentityName, string userHostAddress)
        {
            var userNameArray = userIdentityName.Split('\\');
            if (userIdentityName.Contains("@"))
            {
                userNameArray = userIdentityName.Split('@');
                Array.Reverse(userNameArray);
            }

            var domain = userNameArray[0].ToUpper();
            ApplicationGlobals.UserName = userNameArray[1];

            // Get Client IP
            ApplicationGlobals.ClientIpAddress = userHostAddress;

            IUserAccountBll iUserAccountBll = new UserAccountBll();
            var userAccount = iUserAccountBll.GetUserAccountForUserNameAndDomain(ApplicationGlobals.UserName, domain, ApplicationGlobals.ApplicationCode);

            if (userAccount != null)
            {
                IUserApplicationSettingsBll iUserApplicationSettingsBll = new UserApplicationSettingsBll();
                var userApplicationSettings = iUserApplicationSettingsBll.GetUserApplicationSettingsForUserAccountId(userAccount.Id);

                ApplicationGlobals.DefaultCompany = userApplicationSettings == null ? userAccount.CompanyObj.Description : userApplicationSettings.CompanyObj.Description;
                ApplicationGlobals.LnDefaultCompanyId = userApplicationSettings == null ? userAccount.LnDefaultCompanyId : userApplicationSettings.LnCompanyId;
                ApplicationGlobals.CanDisplayTreeMenuVertical = userApplicationSettings == null || userApplicationSettings.UserMenuLocation == TreeMenuLocation.Vertical;
                ApplicationGlobals.CultureInfo = userApplicationSettings == null ? new CultureInfo(LanguageCode.English) : new CultureInfo(userApplicationSettings.Language);

                IApplicationSettingsBll iApplicationSettingsBll = new ApplicationSettingsBll();
                ApplicationGlobals.SetApplicationSettingsList(iApplicationSettingsBll.GetApplicationSettingsListForCompanyIdAndApplicationCode(ApplicationGlobals.LnDefaultCompanyId, ApplicationGlobals.ApplicationCode));

                ApplicationGlobals.UserId = userAccount.Id;
                InsertUserLog(domain);

                ApplicationGlobals.SetSecurityRightsList(iUserAccountBll.GetUserSecurityRightsListForApplicationCodeAndUserAccountId(ApplicationGlobals.ApplicationCode, userAccount.Id, ApplicationGlobals.LnDefaultCompanyId));
            }
            else
            {
                InsertUserLog(domain, false);
            }

            return userAccount != null;
        }

        /// <summary>
        /// Insert User Log
        /// </summary>
        /// <param name="domain">Domain</param>
        /// <param name="hasLonSucceeded">HasLonSucceeded</param>
        private static void InsertUserLog(string domain, bool hasLonSucceeded = true)
        {
            // Insert User Log
            IUserLogBll iUserLogBll = new UserLogBll();
            iUserLogBll.InsertUserLog(new UserLog
            {
                LnUserAccountId = ApplicationGlobals.UserId,
                ActionDateTime = DateTime.Now,
                LnLogActionId = 2,
                Domain = domain,
                HasLonSucceeded = hasLonSucceeded,
                ClientIPAddress = ApplicationGlobals.ClientIpAddress,
                Application = ApplicationGlobals.ApplicationName
            }, ApplicationGlobals.UserName);
        }
    }
}