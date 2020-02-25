namespace MDA.Security.Service
{
    using BLL;
    using Core.Service.Filters;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.ServiceModel;
    using System.Web.Mvc;

    [HandleAndLogError]
    public class SecurityService : IApplicationSettingsService, IUserApplicationFavouritesService, IUserLogService, IUserAccountService, IUserPreferencesService,
        IUserSettingsService, ICompanyInApplicationService, IApplicationService, IUserApplicationSettingsService, ISecurityRolesService, ICompanyService,
        IServiceDetails, IUserInCompanyInApplicationDetailsService, ISecurityUserInRolesService, IRO_EmployeeService, IRO_EmployeeAddInfoService,
        IRO_DepartmentService, IRO_SkillsService, IRO_EmployeeDetailsService
    {
        #region Application

        /// <summary>
        /// Get Application For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETAPPLICATIONFORCODE")]
        [PrincipalPermission(SecurityAction.Demand, Role = "APPLICATION_ALL")]
        public Application GetApplicationForCode(string code)
        {
            IApplicationBll iApplicationBll = new ApplicationBll();
            return iApplicationBll.GetApplicationForCode(code);
        }

        /// <summary>
        /// Get Available Application Page For User Account Id
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETAVAILABLEAPPLICATIONPAGEFORUSERACCOUNTID")]
        [PrincipalPermission(SecurityAction.Demand, Role = "APPLICATION_ALL")]
        public DataSourceResult<Application> GetAvailableApplicationPageForUserAccountId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userAccountId)
        {
            IApplicationBll iApplicationBll = new ApplicationBll();
            return iApplicationBll.GetAvailableApplicationPageForUserAccountId(take, skip, sort, filter, userAccountId);
        }

        #endregion Application

        #region ApplicationSettings

        /// <summary>
        /// Get ApplicationSettings List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETAPPLICATIONSETTINGSLIST")]
        [PrincipalPermission(SecurityAction.Demand, Role = "APPLICATIONSETTINGS_ALL")]
        public IEnumerable<ApplicationSettings> GetApplicationSettingsList(IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId)
        {
            IApplicationSettingsBll iApplicationSettingsBll = new ApplicationSettingsBll();
            return iApplicationSettingsBll.GetApplicationSettingsList(sort, filter, companyInApplicationId);
        }

        /// <summary>
        /// Get ApplicationSettings List For Company Id And Application Code
        /// </summary>
        /// <param name="companyId">Company Id</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETAPPLICATIONSETTINGSLISTFORCOMPANYIDANDAPPLICATIONCODE")]
        [PrincipalPermission(SecurityAction.Demand, Role = "APPLICATIONSETTINGS_ALL")]
        public IEnumerable<ApplicationSettings> GetApplicationSettingsListForCompanyIdAndApplicationCode(int companyId, string applicationCode)
        {
            IApplicationSettingsBll iApplicationSettingsBll = new ApplicationSettingsBll();
            return iApplicationSettingsBll.GetApplicationSettingsListForCompanyIdAndApplicationCode(companyId, applicationCode);
        }

        /// <summary>
        /// Get ApplicationSettings Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETAPPLICATIONSETTINGSPAGE")]
        [PrincipalPermission(SecurityAction.Demand, Role = "APPLICATIONSETTINGS_ALL")]
        public DataSourceResult<ApplicationSettings> GetApplicationSettingsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId)
        {
            IApplicationSettingsBll iApplicationSettingsBll = new ApplicationSettingsBll();
            return iApplicationSettingsBll.GetApplicationSettingsPage(take, skip, sort, filter, companyInApplicationId);
        }

        #endregion ApplicationSettings

        #region Company

        /// <summary>
        /// Get DropDown List of Company
        /// </summary>
        /// <returns>DropDown List</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETCOMPANYDROPDOWNLIST")]
        [PrincipalPermission(SecurityAction.Demand, Role = "COMPANY_ALL")]
        public IEnumerable<SelectListItem> GetCompanyDropDownList()
        {
            ICompanyBll iCompanyBll = new CompanyBll();
            return iCompanyBll.GetCompanyDropDownList();
        }

        /// <summary>
        /// Get Company For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETCOMPANYFORCODE")]
        [PrincipalPermission(SecurityAction.Demand, Role = "COMPANY_ALL")]
        public Company GetCompanyForCode(string code)
        {
            ICompanyBll iCompanyBll = new CompanyBll();
            return iCompanyBll.GetCompanyForCode(code);
        }

        #endregion Company

        #region UserLog

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userLog">UserLog Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "INSERTUSERLOG")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERLOG_ALL")]
        public bool InsertUserLog(UserLog userLog, string userName)
        {
            IUserLogBll iUserLogBll = new UserLogBll();
            return iUserLogBll.InsertUserLog(userLog, userName);
        }

        #endregion UserLog

        #region UserPreferences

        /// <summary>
        /// Get UserPreferences List For Code And Application Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETUSERPREFERENCESLISTFORCODEANDAPPLICATIONCODE")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERPREFERENCES_ALL")]
        public IEnumerable<UserPreferences> GetUserPreferencesListForCodeAndApplicationCode(string code, string applicationCode, int companyId)
        {
            IUserPreferencesBll iUserPreferencesBll = new UserPreferencesBll();
            return iUserPreferencesBll.GetUserPreferencesListForCodeAndApplicationCode(code, applicationCode, companyId);
        }

        /// <summary>
        /// Get UserPreferences List For User In Company In Application Id
        /// </summary>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETUSERPREFERENCESLISTFORUSERINCOMPANYINAPPLICATIONID")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERPREFERENCES_ALL")]
        public IEnumerable<UserPreferences> GetUserPreferencesListForUserInCompanyInApplicationId(int userInCompanyInApplicationId)
        {
            IUserPreferencesBll iUserPreferencesBll = new UserPreferencesBll();
            return iUserPreferencesBll.GetUserPreferencesListForUserInCompanyInApplicationId(userInCompanyInApplicationId);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userPreferences">UserPreferences Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "INSERTUSERPREFERENCES")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERPREFERENCES_ALL")]
        public bool InsertUserPreferences(UserPreferences userPreferences, string userName)
        {
            IUserPreferencesBll iUserPreferencesBll = new UserPreferencesBll();
            return iUserPreferencesBll.InsertUserPreferences(userPreferences, userName);
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
        [PrincipalPermission(SecurityAction.Demand, Role = "SAVEPREFERENCES")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERPREFERENCES_ALL")]
        public bool SavePreferences(int companyInApplicationId, int userInCompanyInApplicationId, string securityItemCode, string preferences, string userName)
        {
            IUserPreferencesBll iUserPreferencesBll = new UserPreferencesBll();
            return iUserPreferencesBll.SavePreferences(companyInApplicationId, userInCompanyInApplicationId, securityItemCode, preferences, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="userPreferences">UserPreferences Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "UPDATEUSERPREFERENCES")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERPREFERENCES_ALL")]
        public bool UpdateUserPreferences(UserPreferences userPreferences, string userName)
        {
            IUserPreferencesBll iUserPreferencesBll = new UserPreferencesBll();
            return iUserPreferencesBll.UpdateUserPreferences(userPreferences, userName);
        }

        #endregion UserPreferences

        #region UserSettings

        /// <summary>
        /// Get UserSettings For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETUSERSETTINGSFORID")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERSETTINGS_ALL")]
        public UserSettings GetUserSettingsForId(int id)
        {
            IUserSettingsBll iUserSettingsBll = new UserSettingsBll();
            return iUserSettingsBll.GetUserSettingsForId(id);
        }

        /// <summary>
        /// Get UserSettings List For User Account Id And Company Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="companyId">Company Id</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETUSERSETTINGSLISTFORUSERACCOUNTIDANDCOMPANYID")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERSETTINGS_ALL")]
        public IEnumerable<UserSettings> GetUserSettingsListForUserAccountIdAndCompanyId(int userAccountId, int companyId, string applicationCode)
        {
            IUserSettingsBll iUserSettingsBll = new UserSettingsBll();
            return iUserSettingsBll.GetUserSettingsListForUserAccountIdAndCompanyId(userAccountId, companyId, applicationCode);
        }

        #endregion UserSettings

        #region UserAccount

        /// <summary>
        /// Get UserAccount For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETUSERACCOUNTFORCODE")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERACCOUNT_ALL")]
        public UserAccount GetUserAccountForCode(string code)
        {
            IUserAccountBll iUserAccountBll = new UserAccountBll();
            return iUserAccountBll.GetUserAccountForCode(code);
        }

        /// <summary>
        /// Get UserAccount For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETUSERACCOUNTFORID")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERACCOUNT_ALL")]
        public UserAccountDetails GetUserAccountForId(int id)
        {
            IUserAccountBll iUserAccountBll = new UserAccountBll();
            return iUserAccountBll.GetUserAccountForId(id);
        }

        /// <summary>
        /// Get UserAccount For User Name And Domain
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="domain">Domain</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Business Entity</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETUSERACCOUNTFORUSERNAMEANDDOMAIN")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERACCOUNT_ALL")]
        public UserAccount GetUserAccountForUserNameAndDomain(string userName, string domain, string applicationCode)
        {
            IUserAccountBll iUserAccountBll = new UserAccountBll();
            return iUserAccountBll.GetUserAccountForUserNameAndDomain(userName, domain, applicationCode);
        }

        /// <summary>
        /// Get UserAccount List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETUSERACCOUNTFORCODE")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERACCOUNT_ALL")]
        public IEnumerable<UserAccountDetails> GetUserAccountList(IEnumerable<Sort> sort, LinqFilter filter, bool isHideTerminated)
        {
            IUserAccountBll iUserAccountBll = new UserAccountBll();
            return iUserAccountBll.GetUserAccountList(sort, filter, isHideTerminated);
        }

        /// <summary>
        /// Get UserAccount List For Employee Id List
        /// </summary>
        /// <param name="employeeIdList">Employee Id List</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETUSERACCOUNTLISTFOREMPLOYEEIDLIST")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERACCOUNT_ALL")]
        public IEnumerable<UserAccountDetails> GetUserAccountListForEmployeeIdList(IList<int> employeeIdList, bool isHideTerminated)
        {
            IUserAccountBll iUserAccountBll = new UserAccountBll();
            return iUserAccountBll.GetUserAccountListForEmployeeIdList(employeeIdList, isHideTerminated);
        }

        /// <summary>
        /// Get UserSecurityRights List For Application Code And User Account Id
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETUSERSECURITYRIGHTSLISTFORAPPLICATIONCODEANDUSERACCOUNTID")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERACCOUNT_ALL")]
        public IEnumerable<UserSecurityRights> GetUserSecurityRightsListForApplicationCodeAndUserAccountId(string applicationCode, int userAccountId, int companyId)
        {
            IUserAccountBll iUserAccountBll = new UserAccountBll();
            return iUserAccountBll.GetUserSecurityRightsListForApplicationCodeAndUserAccountId(applicationCode, userAccountId, companyId);
        }

        /// <summary>
        /// Get UserSecurityRights List For Filter And Application Code
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETUSERSECURITYRIGHTSLISTFORFILTERANDAPPLICATIONCODE")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERACCOUNT_ALL")]
        public IEnumerable<UserSecurityRights> GetUserSecurityRightsListForFilterAndApplicationCode(LinqFilter filter, string applicationCode, int userAccountId, int companyId)
        {
            IUserAccountBll iUserAccountBll = new UserAccountBll();
            return iUserAccountBll.GetUserSecurityRightsListForFilterAndApplicationCode(filter, applicationCode, userAccountId, companyId);
        }

        #endregion UserAccount

        #region UserApplicationFavourites

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "DELETEUSERAPPLICATIONFAVOURITES")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERAPPLICATIONFAVOURITES_ALL")]
        public bool DeleteUserApplicationFavourites(int id, string userName)
        {
            IUserApplicationFavouritesBll iUserApplicationFavouritesBll = new UserApplicationFavouritesBll();
            return iUserApplicationFavouritesBll.DeleteUserApplicationFavourites(id, userName);
        }

        /// <summary>
        /// Get Available UserApplicationFavourites List For User Account Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETAVAILABLEUSERAPPLICATIONFAVOURITESLISTFORUSERACCOUNTID")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERAPPLICATIONFAVOURITES_ALL")]
        public IEnumerable<UserApplicationFavourites> GetAvailableUserApplicationFavouritesListForUserAccountId(int userAccountId, string applicationCode)
        {
            IUserApplicationFavouritesBll iUserApplicationFavouritesBll = new UserApplicationFavouritesBll();
            return iUserApplicationFavouritesBll.GetAvailableUserApplicationFavouritesListForUserAccountId(userAccountId, applicationCode);
        }

        /// <summary>
        /// Get UserApplicationFavourites List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETUSERAPPLICATIONFAVOURITESLIST")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERAPPLICATIONFAVOURITES_ALL")]
        public IEnumerable<UserApplicationFavourites> GetUserApplicationFavouritesList(IEnumerable<Sort> sort, LinqFilter filter, int userAccountId)
        {
            IUserApplicationFavouritesBll iUserApplicationFavouritesBll = new UserApplicationFavouritesBll();
            return iUserApplicationFavouritesBll.GetUserApplicationFavouritesList(sort, filter, userAccountId);
        }

        /// <summary>
        /// Get UserApplicationFavourites List For User Account Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETUSERAPPLICATIONFAVOURITESLISTFORUSERACCOUNTID")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERAPPLICATIONFAVOURITES_ALL")]
        public IEnumerable<UserApplicationFavourites> GetUserApplicationFavouritesListForUserAccountId(int userAccountId)
        {
            IUserApplicationFavouritesBll iUserApplicationFavouritesBll = new UserApplicationFavouritesBll();
            return iUserApplicationFavouritesBll.GetUserApplicationFavouritesListForUserAccountId(userAccountId);
        }

        /// <summary>
        /// Get UserApplicationFavourites Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETUSERAPPLICATIONFAVOURITESPAGE")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERAPPLICATIONFAVOURITES_ALL")]
        public DataSourceResult<UserApplicationFavourites> GetUserApplicationFavouritesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userAccountId)
        {
            IUserApplicationFavouritesBll iUserApplicationFavouritesBll = new UserApplicationFavouritesBll();
            return iUserApplicationFavouritesBll.GetUserApplicationFavouritesPage(take, skip, sort, filter, userAccountId);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userApplicationFavourites">UserApplicationFavourites Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "INSERTUSERAPPLICATIONFAVOURITES")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERAPPLICATIONFAVOURITES_ALL")]
        public bool InsertUserApplicationFavourites(UserApplicationFavourites userApplicationFavourites, string userName)
        {
            IUserApplicationFavouritesBll iUserApplicationFavouritesBll = new UserApplicationFavouritesBll();
            return iUserApplicationFavouritesBll.InsertUserApplicationFavourites(userApplicationFavourites, userName);
        }

        #endregion UserApplicationFavourites

        #region UserApplicationSettings

        /// <summary>
        /// Get UserApplicationSettings For User Account Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Business Entity</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETUSERAPPLICATIONSETTINGSFORUSERACCOUNTID")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERAPPLICATIONSETTINGS_ALL")]
        public UserApplicationSettings GetUserApplicationSettingsForUserAccountId(int userAccountId)
        {
            IUserApplicationSettingsBll iUserApplicationSettingsBll = new UserApplicationSettingsBll();
            return iUserApplicationSettingsBll.GetUserApplicationSettingsForUserAccountId(userAccountId);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userApplicationSettings">UserApplicationSettings Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "INSERTUSERAPPLICATIONSETTINGS")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERAPPLICATIONSETTINGS_ALL")]
        public bool InsertUserApplicationSettings(UserApplicationSettings userApplicationSettings, string userName)
        {
            IUserApplicationSettingsBll iUserApplicationSettingsBll = new UserApplicationSettingsBll();
            return iUserApplicationSettingsBll.InsertUserApplicationSettings(userApplicationSettings, userName);
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
        [PrincipalPermission(SecurityAction.Demand, Role = "INSERTUSERAPPLICATIONSETTINGSFORUSERACCOUNTIDANDCOMPANYID")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERAPPLICATIONSETTINGS_ALL")]
        public bool InsertUserApplicationSettingsForUserAccountIdAndCompanyId(int userAccountId, int companyId, string language, string project, string userMenuLocation, string userName)
        {
            IUserApplicationSettingsBll iUserApplicationSettingsBll = new UserApplicationSettingsBll();
            return iUserApplicationSettingsBll.InsertUserApplicationSettingsForUserAccountIdAndCompanyId(userAccountId, companyId, language, project, userMenuLocation, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="userApplicationSettings">UserApplicationSettings Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "UPDATEUSERAPPLICATIONSETTINGS")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERAPPLICATIONSETTINGS_ALL")]
        public bool UpdateUserApplicationSettings(UserApplicationSettings userApplicationSettings, string userName)
        {
            IUserApplicationSettingsBll iUserApplicationSettingsBll = new UserApplicationSettingsBll();
            return iUserApplicationSettingsBll.UpdateUserApplicationSettings(userApplicationSettings, userName);
        }

        #endregion UserApplicationSettings

        #region CompanyInApplication

        /// <summary>
        /// Get CompanyInApplication List For Application Code And User Account Id
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETCOMPANYINAPPLICATIONLISTFORAPPLICATIONCODEANDUSERACCOUNTID")]
        [PrincipalPermission(SecurityAction.Demand, Role = "COMPANYINAPPLICATION_ALL")]
        public IEnumerable<CompanyInApplication> GetCompanyInApplicationListForApplicationCodeAndUserAccountId(string applicationCode, int userAccountId)
        {
            ICompanyInApplicationBll iCompanyInApplicationBll = new CompanyInApplicationBll();
            return iCompanyInApplicationBll.GetCompanyInApplicationListForApplicationCodeAndUserAccountId(applicationCode, userAccountId);
        }

        #endregion CompanyInApplication

        #region SecurityRoles

        /// <summary>
        /// Get SecurityRolesDetails List For Company Code And Application Code
        /// </summary>
        /// <param name="companyCode">Company Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETSECURITYROLESDETAILSLISTFORCOMPANYCODEANDAPPLICATIONCODE")]
        [PrincipalPermission(SecurityAction.Demand, Role = "SECURITYROLES_ALL")]
        public IEnumerable<SecurityRolesDetails> GetSecurityRolesDetailsListForCompanyCodeAndApplicationCode(string companyCode, string applicationCode)
        {
            ISecurityRolesBll iSecurityRolesBll = new SecurityRolesBll();
            return iSecurityRolesBll.GetSecurityRolesDetailsListForCompanyCodeAndApplicationCode(companyCode, applicationCode);
        }

        /// <summary>
        /// Get SecurityRolesDetailsWithUserName List For Company Code And Application Code
        /// </summary>
        /// <param name="companyCode">Company Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETSECURITYROLESDETAILSWITHUSERNAMELISTFORCOMPANYCODEANDAPPLICATIONCODE")]
        [PrincipalPermission(SecurityAction.Demand, Role = "SECURITYROLES_ALL")]
        public IEnumerable<SecurityRolesDetailsWithUserName> GetSecurityRolesDetailsWithUserNameListForCompanyCodeAndApplicationCode(string companyCode, string applicationCode)
        {
            ISecurityRolesBll iSecurityRolesBll = new SecurityRolesBll();
            return iSecurityRolesBll.GetSecurityRolesDetailsWithUserNameListForCompanyCodeAndApplicationCode(companyCode, applicationCode);
        }

        /// <summary>
        /// Get SecurityRoles List For Application Code And Company Id
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETSECURITYROLESLISTFORAPPLICATIONCODEANDCOMPANYID")]
        [PrincipalPermission(SecurityAction.Demand, Role = "SECURITYROLES_ALL")]
        public IEnumerable<SecurityRoles> GetSecurityRolesListForApplicationCodeAndCompanyId(string applicationCode, int companyId)
        {
            ISecurityRolesBll iSecurityRolesBll = new SecurityRolesBll();
            return iSecurityRolesBll.GetSecurityRolesListForApplicationCodeAndCompanyId(applicationCode, companyId);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityRoles">SecurityRoles Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "UPDATESECURITYROLES")]
        [PrincipalPermission(SecurityAction.Demand, Role = "SECURITYROLES_ALL")]
        public bool UpdateSecurityRoles(SecurityRoles securityRoles, string userName)
        {
            ISecurityRolesBll iSecurityRolesBll = new SecurityRolesBll();
            return iSecurityRolesBll.UpdateSecurityRoles(securityRoles, userName);
        }

        #endregion SecurityRoles

        #region SecurityUserInRoles

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "DELETESECURITYUSERINROLES")]
        [PrincipalPermission(SecurityAction.Demand, Role = "SECURITYUSERINROLES_ALL")]
        public bool DeleteSecurityUserInRoles(int id, string userName)
        {
            ISecurityUserInRolesBll iSecurityUserInRolesBll = new SecurityUserInRolesBll();
            return iSecurityUserInRolesBll.DeleteSecurityUserInRoles(id, userName);
        }

        /// <summary>
        /// Delete SecurityUserInRoles For Security Roles Id
        /// </summary>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "DELETESECURITYUSERINROLESFORSECURITYROLESID")]
        [PrincipalPermission(SecurityAction.Demand, Role = "SECURITYUSERINROLES_ALL")]
        public bool DeleteSecurityUserInRolesForSecurityRolesId(int securityRolesId, string userName)
        {
            ISecurityUserInRolesBll iSecurityUserInRolesBll = new SecurityUserInRolesBll();
            return iSecurityUserInRolesBll.DeleteSecurityUserInRolesForSecurityRolesId(securityRolesId, userName);
        }

        /// <summary>
        /// Get SecurityUserInRoles For Security Role Code And Application Code
        /// </summary>
        /// <param name="securityRoleCode">Security Role Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyCode">Company Code</param>
        /// <returns>Business Entity</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETSECURITYUSERINROLESFORSECURITYROLECODEANDAPPLICATIONCODE")]
        [PrincipalPermission(SecurityAction.Demand, Role = "SECURITYUSERINROLES_ALL")]
        public SecurityUserInRoles GetSecurityUserInRolesForSecurityRoleCodeAndApplicationCode(string securityRoleCode, string applicationCode, string companyCode)
        {
            ISecurityUserInRolesBll iSecurityUserInRolesBll = new SecurityUserInRolesBll();
            return iSecurityUserInRolesBll.GetSecurityUserInRolesForSecurityRoleCodeAndApplicationCode(securityRoleCode, applicationCode, companyCode);
        }

        /// <summary>
        /// Get SecurityUserInRoles List For Application Code And Company Id
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyId">Company Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETSECURITYUSERINROLESLISTFORAPPLICATIONCODEANDCOMPANYID")]
        [PrincipalPermission(SecurityAction.Demand, Role = "SECURITYUSERINROLES_ALL")]
        public IEnumerable<SecurityUserInRoles> GetSecurityUserInRolesListForApplicationCodeAndCompanyId(string applicationCode, int companyId, bool isHideTerminated)
        {
            ISecurityUserInRolesBll iSecurityUserInRolesBll = new SecurityUserInRolesBll();
            return iSecurityUserInRolesBll.GetSecurityUserInRolesListForApplicationCodeAndCompanyId(applicationCode, companyId, isHideTerminated);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityUserInRoles">SecurityUserInRoles Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "INSERTSECURITYUSERINROLES")]
        [PrincipalPermission(SecurityAction.Demand, Role = "SECURITYUSERINROLES_ALL")]
        public bool InsertSecurityUserInRoles(SecurityUserInRoles securityUserInRoles, string userName)
        {
            ISecurityUserInRolesBll iSecurityUserInRolesBll = new SecurityUserInRolesBll();
            return iSecurityUserInRolesBll.InsertSecurityUserInRoles(securityUserInRoles, userName);
        }

        /// <summary>
        /// Is SecurityUserInRoles Exists For Security Role Code And User Account Id
        /// </summary>
        /// <param name="securityRoleCode">Security Role Code</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyCode">Company Code</param>
        /// <returns>True if SecurityUserInRoles Exists, else False</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "ISSECURITYUSERINROLESEXISTSFORSECURITYROLECODEANDUSERACCOUNTID")]
        [PrincipalPermission(SecurityAction.Demand, Role = "SECURITYUSERINROLES_ALL")]
        public bool IsSecurityUserInRolesExistsForSecurityRoleCodeAndUserAccountId(string securityRoleCode, int userAccountId, string applicationCode, string companyCode)
        {
            ISecurityUserInRolesBll iSecurityUserInRolesBll = new SecurityUserInRolesBll();
            return iSecurityUserInRolesBll.IsSecurityUserInRolesExistsForSecurityRoleCodeAndUserAccountId(securityRoleCode, userAccountId, applicationCode, companyCode);
        }

        #endregion SecurityUserInRoles

        #region ServiceDetails

        /// <summary>
        /// Get ServiceDetails
        /// </summary>
        /// <returns>Business Entity</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETSERVICEDETAILS")]
        [PrincipalPermission(SecurityAction.Demand, Role = "SERVICEDETAILS_ALL")]
        public ServiceDetails GetServiceDetails()
        {
            var channel = OperationContext.Current.Channel;
            return new ServiceDetails
            {
                ServiceUrl = channel.LocalAddress.Uri.AbsoluteUri,
                Host = channel.LocalAddress.Uri.Host,
                Scheme = channel.LocalAddress.Uri.Scheme,
                ServiceName = channel.LocalAddress.Uri.Segments[channel.LocalAddress.Uri.Segments.Length - 1]
            };
        }

        #endregion ServiceDetails

        #region UserInCompanyInApplicationDetails

        /// <summary>
        /// Get UserInCompanyInApplicationDetails List For Application Code And Is Hide Terminated
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETUSERINCOMPANYINAPPLICATIONDETAILSLISTFORAPPLICATIONCODEANDISHIDETERMINATED")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERINCOMPANYINAPPLICATIONDETAILS_ALL")]
        public IEnumerable<UserInCompanyInApplicationDetails> GetUserInCompanyInApplicationDetailsListForApplicationCodeAndIsHideTerminated(string applicationCode, bool isHideTerminated)
        {
            IUserInCompanyInApplicationDetailsBll iUserInCompanyInApplicationDetailsBll = new UserInCompanyInApplicationDetailsBll();
            return iUserInCompanyInApplicationDetailsBll.GetUserInCompanyInApplicationDetailsListForApplicationCodeAndIsHideTerminated(applicationCode, isHideTerminated);
        }

        /// <summary>
        /// Get UserInCompanyInApplicationDetails List For Company Code And Application Code
        /// </summary>
        /// <param name="companyCode">Company Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETUSERINCOMPANYINAPPLICATIONDETAILSLISTFORCOMPANYCODEANDAPPLICATIONCODE")]
        [PrincipalPermission(SecurityAction.Demand, Role = "USERINCOMPANYINAPPLICATIONDETAILS_ALL")]
        public IEnumerable<UserInCompanyInApplicationDetails> GetUserInCompanyInApplicationDetailsListForCompanyCodeAndApplicationCode(string companyCode, string applicationCode, bool isHideTerminated)
        {
            IUserInCompanyInApplicationDetailsBll iUserInCompanyInApplicationDetailsBll = new UserInCompanyInApplicationDetailsBll();
            return iUserInCompanyInApplicationDetailsBll.GetUserInCompanyInApplicationDetailsListForCompanyCodeAndApplicationCode(companyCode, applicationCode, isHideTerminated);
        }

        #endregion UserInCompanyInApplicationDetails

        #region Skills

        /// <summary>
        /// Get Skills List
        /// </summary>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETSKILLSLIST")]
        [PrincipalPermission(SecurityAction.Demand, Role = "SKILLS_ALL")]
        public IEnumerable<RO_Skills> GetSkillsList()
        {
            IRO_SkillsBll iRO_SkillsBll = new RO_SkillsBll();
            return iRO_SkillsBll.GetSkillsList();
        }

        #endregion Skills

        #region Department

        /// <summary>
        /// Get Department List
        /// </summary>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETDEPARTMENTLIST")]
        [PrincipalPermission(SecurityAction.Demand, Role = "RO_DEPARTMENT_ALL")]
        public IEnumerable<RO_Department> GetDepartmentList()
        {
            IRO_DepartmentBll iRO_DepartmentBll = new RO_DepartmentBll();
            return iRO_DepartmentBll.GetDepartmentList();
        }

        /// <summary>
        /// Get Department List For Filter
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETDEPARTMENTLISTFORFILTER")]
        [PrincipalPermission(SecurityAction.Demand, Role = "RO_DEPARTMENT_ALL")]
        public IEnumerable<RO_Department> GetDepartmentListForFilter(LinqFilter filter)
        {
            IRO_DepartmentBll iRO_DepartmentBll = new RO_DepartmentBll();
            return iRO_DepartmentBll.GetDepartmentListForFilter(filter);
        }

        #endregion Department

        #region Employee

        /// <summary>
        /// Get Employee List For Is Hide Terminated And Filter
        /// </summary>
        /// <param name="isHideTerminated">Hide Terminated Employees</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETEMPLOYEELISTFORISHIDETERMINATEDANDFILTER")]
        [PrincipalPermission(SecurityAction.Demand, Role = "RO_EMPLOYEE_ALL")]
        public IEnumerable<RO_Employee> GetEmployeeListForIsHideTerminatedAndFilter(bool isHideTerminated, LinqFilter filter)
        {
            IRO_EmployeeBll iRO_EmployeeBll = new RO_EmployeeBll();
            return iRO_EmployeeBll.GetEmployeeListForIsHideTerminatedAndFilter(isHideTerminated, filter);
        }

        #endregion Employee

        #region EmployeeAddInfo

        /// <summary>
        /// Get EmployeeAddInfo List For Filter
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETEMPLOYEEADDINFOLISTFORFILTER")]
        [PrincipalPermission(SecurityAction.Demand, Role = "RO_EMPLOYEEADDINFO_ALL")]
        public IEnumerable<RO_EmployeeAddInfo> GetEmployeeAddInfoListForFilter(LinqFilter filter)
        {
            IRO_EmployeeAddInfoBll iRO_EmployeeAddInfoBll = new RO_EmployeeAddInfoBll();
            return iRO_EmployeeAddInfoBll.GetEmployeeAddInfoListForFilter(filter);
        }

        #endregion EmployeeAddInfo

        #region EmployeeDetails

        /// <summary>
        /// Get EmployeeDetails List For Is Hide Terminated And Filter
        /// </summary>
        /// <param name="isHideTerminated">Hide Terminated Employees</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = "GETEMPLOYEEDETAILSLISTFORISHIDETERMINATEDANDFILTER")]
        [PrincipalPermission(SecurityAction.Demand, Role = "RO_EMPLOYEEDETAILS_ALL")]
        public IEnumerable<RO_EmployeeDetails> GetEmployeeDetailsListForIsHideTerminatedAndFilter(bool isHideTerminated, LinqFilter filter)
        {
            IRO_EmployeeDetailsBll iRO_EmployeeDetailsBll = new RO_EmployeeDetailsBll();
            return iRO_EmployeeDetailsBll.GetEmployeeDetailsListForIsHideTerminatedAndFilter(isHideTerminated, filter);
        }

        #endregion EmployeeDetails
    }
}