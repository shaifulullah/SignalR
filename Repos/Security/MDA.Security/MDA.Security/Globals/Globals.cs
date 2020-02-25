namespace MDA.Security.Globals
{
    using Linq;
    using Models;
    using Newtonsoft.Json;
    using Resources;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using ViewModels;

    /// <summary>
    /// Application Global Operations
    /// </summary>
    public static class ApplicationGlobals
    {
        /// <summary>
        /// Gets Application Code
        /// </summary>
        public static string ApplicationCode
        {
            get { return ConfigurationManager.AppSettings["ApplicationCode"]; }
        }

        /// <summary>
        /// Gets Application Domain List
        /// </summary>
        public static string ApplicationDomainList
        {
            get
            {
                var applicationSettings = GetApplicationSettingsForKeyName(ConfigurationSettingsKeyName.APPLICATIONDOMAINLIST);

                try
                {
                    return applicationSettings.Value;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets Application Name
        /// </summary>
        public static string ApplicationName
        {
            get { return ConfigurationManager.AppSettings["ApplicationName"]; }
        }

        /// <summary>
        /// Gets the Application Version
        /// </summary>
        public static string ApplicationVersion
        {
            get { return ConfigurationManager.AppSettings["ApplicationVersion"]; }
        }

        /// <summary>
        /// Gets or sets Can Display TreeMenu Vertical
        /// </summary>
        public static bool CanDisplayTreeMenuVertical
        {
            get
            {
                try
                {
                    return bool.Parse(HttpContext.Current.Request.Cookies["CANDISPLAYTREEMENUVERTICAL"].Value);
                }
                catch
                {
                    return true;
                }
            }

            set
            {
                HttpContext.Current.Response.Cookies["CANDISPLAYTREEMENUVERTICAL"].Value = value.ToString();
            }
        }

        /// <summary>
        /// Gets or sets Client Ip Address
        /// </summary>
        public static string ClientIpAddress
        {
            get
            {
                try
                {
                    return HttpContext.Current.Session["CLIENTIPADDRESS"].ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }

            set
            {
                HttpContext.Current.Session["CLIENTIPADDRESS"] = value;
            }
        }

        /// <summary>
        /// Gets or sets Configuration Type
        /// </summary>
        public static string ConfigurationType
        {
            get
            {
                try
                {
                    return HttpContext.Current.Session["CONFIGURATIONTYPE"].ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }

            set
            {
                HttpContext.Current.Session["CONFIGURATIONTYPE"] = value;
            }
        }

        /// <summary>
        /// Gets or sets Culture Info
        /// </summary>
        public static CultureInfo CultureInfo
        {
            get
            {
                try
                {
                    return HttpContext.Current.Session["CULTUREINFO"] != null ?
                        (CultureInfo)HttpContext.Current.Session["CULTUREINFO"] : new CultureInfo("en-CA");
                }
                catch
                {
                    return new CultureInfo("en-CA");
                }
            }

            set
            {
                HttpContext.Current.Session["CULTUREINFO"] = value;
            }
        }

        /// <summary>
        /// Gets or sets Default Company
        /// </summary>
        public static string DefaultCompany
        {
            get
            {
                try
                {
                    return HttpContext.Current.Session["DEFAULTCOMPANY"].ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }

            set
            {
                HttpContext.Current.Session["DEFAULTCOMPANY"] = value;
            }
        }

        /// <summary>
        /// Gets Grid Minimum Rows
        /// </summary>
        public static int GridMinRows
        {
            get
            {
                var applicationSettings = GetApplicationSettingsForKeyName(ConfigurationSettingsKeyName.GRIDMINROWS);

                try
                {
                    return int.Parse(applicationSettings.Value);
                }
                catch
                {
                    return 15;
                }
            }
        }

        /// <summary>
        /// Gets or sets Is Hide Terminated
        /// </summary>
        public static bool IsHideTerminated
        {
            get
            {
                var applicationSettings = GetApplicationSettingsForKeyName(ConfigurationSettingsKeyName.ISHIDETERMINATED);

                try
                {
                    return bool.Parse(applicationSettings.Value.ToLower());
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets or sets Ln Default Company Id
        /// </summary>
        public static int LnDefaultCompanyId
        {
            get
            {
                try
                {
                    return int.Parse(HttpContext.Current.Session["LNDEFAULTCOMPANYID"].ToString());
                }
                catch
                {
                    return 0;
                }
            }

            set
            {
                HttpContext.Current.Session["LNDEFAULTCOMPANYID"] = value;
            }
        }

        /// <summary>
        /// Gets Logo Directory
        /// </summary>
        public static string LogoDirectory
        {
            get { return ConfigurationManager.AppSettings["LogoDirectory"]; }
        }

        /// <summary>
        /// Gets or sets Selected Application
        /// </summary>
        public static ApplicationViewModel SelectedApplication
        {
            get
            {
                try
                {
                    return HttpContext.Current.Session["APPLICATION"] as ApplicationViewModel;
                }
                catch
                {
                    return null;
                }
            }

            set
            {
                HttpContext.Current.Session["APPLICATION"] = value;
            }
        }

        /// <summary>
        /// Gets or sets Selected CompanyInApplication
        /// </summary>
        public static CompanyInApplicationViewModel SelectedCompanyInApplication
        {
            get
            {
                try
                {
                    return HttpContext.Current.Session["COMPANYINAPPLICATION"] as CompanyInApplicationViewModel;
                }
                catch
                {
                    return null;
                }
            }

            set
            {
                HttpContext.Current.Session["COMPANYINAPPLICATION"] = value;
            }
        }

        /// <summary>
        /// Gets or sets Selected ExternalCompany
        /// </summary>
        public static ExternalCompanyViewModel SelectedExternalCompany
        {
            get
            {
                try
                {
                    return HttpContext.Current.Session["EXTERNALCOMPANY"] as ExternalCompanyViewModel;
                }
                catch
                {
                    return null;
                }
            }

            set
            {
                HttpContext.Current.Session["EXTERNALCOMPANY"] = value;
            }
        }

        /// <summary>
        /// Gets or sets Selected SecurityBusinessEntities
        /// </summary>
        public static SecurityBusinessEntitiesViewModel SelectedSecurityBusinessEntities
        {
            get
            {
                try
                {
                    return HttpContext.Current.Session["SECURITYBUSINESSENTITIES"] as SecurityBusinessEntitiesViewModel;
                }
                catch
                {
                    return null;
                }
            }

            set
            {
                HttpContext.Current.Session["SECURITYBUSINESSENTITIES"] = value;
            }
        }

        /// <summary>
        /// Gets or sets Selected SecurityRoles
        /// </summary>
        public static SecurityRolesViewModel SelectedSecurityRoles
        {
            get
            {
                try
                {
                    return HttpContext.Current.Session["SECURITYROLES"] as SecurityRolesViewModel;
                }
                catch
                {
                    return null;
                }
            }

            set
            {
                HttpContext.Current.Session["SECURITYROLES"] = value;
            }
        }

        /// <summary>
        /// Gets or sets Selected UserAccount
        /// </summary>
        public static UserAccountViewModel SelectedUserAccount
        {
            get
            {
                try
                {
                    return HttpContext.Current.Session["USERACCOUNT"] as UserAccountViewModel;
                }
                catch
                {
                    return null;
                }
            }

            set
            {
                HttpContext.Current.Session["USERACCOUNT"] = value;
            }
        }

        /// <summary>
        /// Gets or sets Selected UserInCompanyInApplication
        /// </summary>
        public static UserInCompanyInApplicationViewModel SelectedUserInCompanyInApplication
        {
            get
            {
                try
                {
                    return HttpContext.Current.Session["USERINCOMPANYINAPPLICATION"] as UserInCompanyInApplicationViewModel;
                }
                catch
                {
                    return null;
                }
            }

            set
            {
                HttpContext.Current.Session["USERINCOMPANYINAPPLICATION"] = value;
            }
        }

        /// <summary>
        /// Gets Short Date Pattern
        /// </summary>
        public static string ShortDatePattern
        {
            get
            {
                return CultureInfo.DateTimeFormat.ShortDatePattern;
            }
        }

        /// <summary>
        /// Gets Short Date Time Pattern
        /// </summary>
        public static string ShortDateTimePattern
        {
            get
            {
                return "dd-MMM-yyyy HH:mm:ss";
            }
        }

        /// <summary>
        /// Gets Temp Directory
        /// </summary>
        public static string TempDirectory
        {
            get { return ConfigurationManager.AppSettings["TempDirectory"]; }
        }

        /// <summary>
        /// Gets or sets Temp Id Number
        /// </summary>
        public static int TempIdNumber
        {
            get
            {
                try
                {
                    return int.Parse(HttpContext.Current.Request.Cookies["TEMPIDNUMBER"].Value);
                }
                catch
                {
                    return 0;
                }
            }

            set
            {
                HttpContext.Current.Request.Cookies["TEMPIDNUMBER"].Value = value.ToString();
            }
        }

        /// <summary>
        /// Gets or sets User Id
        /// </summary>
        public static int UserId
        {
            get
            {
                try
                {
                    return int.Parse(HttpContext.Current.Session["USERID"].ToString());
                }
                catch
                {
                    return 0;
                }
            }

            set
            {
                HttpContext.Current.Session["USERID"] = value;
            }
        }

        /// <summary>
        /// Gets or sets User Name
        /// </summary>
        public static string UserName
        {
            get
            {
                try
                {
                    return HttpContext.Current.Session["USERNAME"].ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }

            set
            {
                HttpContext.Current.Session["USERNAME"] = value;
            }
        }

        /// <summary>
        /// Gets Visible Grid Column Headers
        /// </summary>
        public static string[] VisibleGridColumnHeaders
        {
            get
            {
                try
                {
                    var value = HttpContext.Current.Request.Cookies.Get(string.Format("VISIBLEGRIDCOLUMNHEADERS")).Value;
                    return JsonConvert.DeserializeObject<string[]>(HttpUtility.UrlDecode(value));
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Download File on Client
        /// </summary>
        /// <param name="filePath">Physical File Path</param>
        /// <param name="downloadToken">Download Token</param>
        /// <returns>True or False</returns>
        public static bool DownloadFile(string filePath, string downloadToken = "")
        {
            FileInfo file = null;
            if (!string.IsNullOrEmpty(filePath))
            {
                file = new FileInfo(filePath);
                if (file.Exists)
                {
                    var response = HttpContext.Current.Response;
                    response.Clear();

                    response.AppendCookie(new HttpCookie("DOWNLOADTOKEN", downloadToken));
                    response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", file.Name));
                    response.AddHeader("Content-Length", file.Length.ToString(CultureInfo.InvariantCulture));

                    response.ContentType = "application/octet-stream";
                    response.WriteFile(file.FullName);

                    response.End();
                }
            }

            return file.Exists;
        }

        /// <summary>
        /// Gets Filter
        /// </summary>
        /// <param name="gridId">Grid Id</param>
        public static LinqFilter Filter(string gridId)
        {
            try
            {
                var value = HttpContext.Current.Request.Cookies.Get(string.Format("FILTER-{0}", gridId)).Value;
                return JsonConvert.DeserializeObject<LinqFilter>(HttpUtility.UrlDecode(value));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Generate Bread Crumbs
        /// </summary>
        /// <param name="htmlHelper">HTML Helper</param>
        /// <param name="breadCrumbs">Comma Seperated Value of BreadCrumbs to Build</param>
        /// <param name="viewName">View Name</param>
        /// <returns>Bread Crumbs</returns>
        public static HtmlString GenerateBreadCrumbs(this HtmlHelper htmlHelper, string breadCrumbs, string viewName)
        {
            var stringBuilder = new StringBuilder();
            var breadCrumbsList = breadCrumbs.Split(',');

            stringBuilder.Append(@"<div class=""bread-crumbs"">");
            foreach (var breadCrumb in breadCrumbsList)
            {
                switch (breadCrumb.Trim().ToUpper())
                {
                    case "APPLICATION":
                        stringBuilder.Append(string.Format("{0}&nbsp;&raquo;&nbsp;", htmlHelper.ActionLink(ResourceStrings.Text_Application, "Index", "Application")));
                        break;

                    case "COMPANYINAPPLICATION":
                        stringBuilder.Append(string.Format("{0}&nbsp;&raquo;&nbsp;", htmlHelper.ActionLink(ResourceStrings.Text_CompanyInApplication, "Index", "CompanyInApplication")));
                        break;

                    case "EXTERNALCOMPANY":
                        stringBuilder.Append(string.Format("{0}&nbsp;&raquo;&nbsp;", htmlHelper.ActionLink(ResourceStrings.Text_ExternalCompany, "Index", "ExternalCompany")));
                        break;

                    case "SECURITYBUSINESSENTITIES":
                        stringBuilder.Append(string.Format("{0}&nbsp;&raquo;&nbsp;", htmlHelper.ActionLink(ResourceStrings.Text_SecurityBusinessEntities, "Index", "SecurityBusinessEntities")));
                        break;

                    case "SECURITYROLES":
                        stringBuilder.Append(string.Format("{0}&nbsp;&raquo;&nbsp;", htmlHelper.ActionLink(ResourceStrings.Text_SecurityRoles, "Index", "SecurityRoles")));
                        break;

                    case "USERACCOUNT":
                        stringBuilder.Append(string.Format("{0}&nbsp;&raquo;&nbsp;", htmlHelper.ActionLink(ResourceStrings.Text_UserAccount, "Index", "UserAccount")));
                        break;

                    case "USERINCOMPANYINAPPLICATION":
                        stringBuilder.Append(string.Format("{0}&nbsp;&raquo;&nbsp;", htmlHelper.ActionLink(ResourceStrings.Text_UserInCompanyInApplication, "Index", "UserInCompanyInApplication")));
                        break;
                }
            }
            stringBuilder.Append(string.Format("{0}</div>", viewName));

            return new HtmlString(stringBuilder.ToString());
        }

        /// <summary>
        /// Generate Master Detail
        /// </summary>
        /// <param name="htmlHelper">HTML Helper</param>
        /// <param name="masterDetails">Comma Seperated Value of Master Details to Build</param>
        /// <returns>Master Detail</returns>
        public static HtmlString GenerateMasterDetail(this HtmlHelper htmlHelper, string masterDetails)
        {
            var stringBuilder = new StringBuilder();
            var masterDetailsList = masterDetails.Split(',');

            stringBuilder.Append(@"<div class=""padding-bottom-5"">");
            stringBuilder.Append(@"<div class=""master-detail"">");
            stringBuilder.Append("<table>");

            foreach (var masterDetail in masterDetailsList)
            {
                switch (masterDetail.Trim().ToUpper())
                {
                    case "APPLICATION":
                        stringBuilder.Append(string.Format(@"<tr><td style=""text-align: right""><i>{0}</i>&nbsp;:</td>", ResourceStrings.Text_Application));
                        stringBuilder.Append(string.Format("<td>{0}</td></tr>", htmlHelper.ActionLink(string.Format("{0} - {1}", SelectedApplication.Code, SelectedApplication.Description), "Index", "Application")));

                        break;

                    case "COMPANYINAPPLICATION":
                        stringBuilder.Append(string.Format(@"<tr><td style=""text-align: right""><i>{0}</i>&nbsp;:</td>", ResourceStrings.Text_CompanyInApplication));
                        stringBuilder.Append(string.Format("<td>{0}</td></tr>", htmlHelper.ActionLink(string.Format("{0} - {1}", SelectedCompanyInApplication.CompanyObj.Code, SelectedCompanyInApplication.CompanyObj.Description), "Index", "CompanyInApplication")));

                        break;

                    case "EXTERNALCOMPANY":
                        stringBuilder.Append(string.Format(@"<tr><td style=""text-align: right""><i>{0}</i>&nbsp;:</td>", ResourceStrings.Text_ExternalCompany));
                        stringBuilder.Append(string.Format("<td>{0}</td></tr>", htmlHelper.ActionLink(string.Format("{0} - {1}", SelectedExternalCompany.Code, SelectedExternalCompany.Description), "Index", "ExternalCompany")));

                        break;

                    case "SECURITYBUSINESSENTITIES":
                        stringBuilder.Append(string.Format(@"<tr><td style=""text-align: right""><i>{0}</i>&nbsp;:</td>", ResourceStrings.Text_SecurityBusinessEntities));
                        stringBuilder.Append(string.Format("<td>{0}</td></tr>", htmlHelper.ActionLink(string.Format("{0} - {1}", SelectedSecurityBusinessEntities.Code, SelectedSecurityBusinessEntities.Description), "Index", "SecurityBusinessEntities")));

                        break;

                    case "SECURITYROLES":
                        stringBuilder.Append(string.Format(@"<tr><td style=""text-align: right""><i>{0}</i>&nbsp;:</td>", ResourceStrings.Text_SecurityRoles));
                        stringBuilder.Append(string.Format("<td>{0}</td></tr>", htmlHelper.ActionLink(string.Format("{0} - {1}", SelectedSecurityRoles.Code, SelectedSecurityRoles.Description), "Index", "SecurityRoles")));

                        break;

                    case "USERACCOUNT":
                        stringBuilder.Append(string.Format(@"<tr><td style=""text-align: right""><i>{0}</i>&nbsp;:</td>", ResourceStrings.Text_UserAccount));
                        stringBuilder.Append(string.Format("<td>{0}</td></tr>", htmlHelper.ActionLink(string.Format("{0} - {1}", SelectedUserAccount.UserName, SelectedUserAccount.FullName), "Index", "UserAccount")));

                        break;

                    case "USERINCOMPANYINAPPLICATION":
                        stringBuilder.Append(string.Format(@"<tr><td style=""text-align: right""><i>{0}</i>&nbsp;:</td>", ResourceStrings.Text_UserInCompanyInApplication));
                        stringBuilder.Append(string.Format("<td>{0}</td></tr>", htmlHelper.ActionLink(string.Format("{0} - {1}", SelectedUserInCompanyInApplication.UserAccountDetailsObj.UserName, SelectedUserInCompanyInApplication.UserAccountDetailsObj.FullName), "Index", "UserInCompanyInApplication")));

                        break;
                }
            }

            stringBuilder.Append("</table>");
            stringBuilder.Append("</div>");
            stringBuilder.Append("</div>");

            return new HtmlString(stringBuilder.ToString());
        }

        /// <summary>
        /// Generate Report Header Details
        /// </summary>
        /// <param name="reportHeader">Report Header</param>
        /// <returns>Report Header Details</returns>
        public static string GenerateReportHeaderDetails(string reportHeader)
        {
            var stringBuilder = new StringBuilder();
            switch (reportHeader.Trim().ToUpper())
            {
                case "APPLICATION":
                    stringBuilder.Append(string.Format(@"{0}: {1} - {2}", ResourceStrings.Text_Application, SelectedApplication.Code, SelectedApplication.Description));
                    break;

                case "COMPANYINAPPLICATION":
                    stringBuilder.Append(string.Format(@"{0}: {1} - {2}", ResourceStrings.Text_CompanyInApplication, SelectedCompanyInApplication.CompanyObj.Code, SelectedCompanyInApplication.CompanyObj.Description));
                    break;

                case "EXTERNALCOMPANY":
                    stringBuilder.Append(string.Format(@"{0}: {1} - {2}", ResourceStrings.Text_ExternalCompany, SelectedExternalCompany.Code, SelectedExternalCompany.Description));
                    break;

                case "SECURITYBUSINESSENTITIES":
                    stringBuilder.Append(string.Format(@"{0}: {1} - {2}", ResourceStrings.Text_SecurityBusinessEntities, SelectedSecurityBusinessEntities.Code, SelectedSecurityBusinessEntities.Description));
                    break;

                case "SECURITYROLES":
                    stringBuilder.Append(string.Format(@"{0}: {1} - {2}", ResourceStrings.Text_SecurityRoles, SelectedSecurityRoles.Code, SelectedSecurityRoles.Description));
                    break;

                case "USERACCOUNT":
                    stringBuilder.Append(string.Format(@"{0}: {1} - {2}", ResourceStrings.Text_UserAccount, SelectedUserAccount.UserName, SelectedUserAccount.FullName));
                    break;

                case "USERINCOMPANYINAPPLICATION":
                    stringBuilder.Append(string.Format(@"{0}: {1} - {2}", ResourceStrings.Text_UserInCompanyInApplication, SelectedUserInCompanyInApplication.UserAccountDetailsObj.UserName, SelectedUserInCompanyInApplication.UserAccountDetailsObj.FullName));
                    break;
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Get Action Text from DataOperation
        /// </summary>
        /// <param name="dataOperation">Data Operation</param>
        /// <returns>Action Text</returns>
        public static string GetActionText(DataOperation dataOperation)
        {
            string actionText;
            switch (dataOperation)
            {
                case DataOperation.CREATE:
                    actionText = ResourceStrings.Text_New;
                    break;

                case DataOperation.READ:
                    actionText = ResourceStrings.Text_Read;
                    break;

                case DataOperation.UPDATE:
                case DataOperation.EDIT_VIEW:
                    actionText = ResourceStrings.Text_Edit_View;
                    break;

                case DataOperation.DELETE:
                    actionText = ResourceStrings.Text_Delete;
                    break;

                case DataOperation.EXECUTE:
                    actionText = ResourceStrings.Text_Execute;
                    break;

                case DataOperation.ACCESS:
                    actionText = ResourceStrings.Text_Identification;
                    break;

                default:
                    actionText = string.Empty;
                    break;
            }

            return actionText;
        }

        /// <summary>
        /// Get ApplicationSettings For Key Name
        /// </summary>
        /// <param name="keyName">Key Name</param>
        /// <returns>Business Entity</returns>
        public static ApplicationSettings GetApplicationSettingsForKeyName(string keyName)
        {
            try
            {
                return (HttpContext.Current.Session["APPLICATIONSETTINGS"] as IEnumerable<ApplicationSettings>).FirstOrDefault(x => x.KeyName.ToUpper() == keyName.ToUpper());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get UserSecurityRights List For Security Item Code
        /// </summary>
        /// <param name="securityItemCode">Security Item Code</param>
        /// <returns>User Security Rights List</returns>
        public static IEnumerable<UserSecurityRights> GetUserSecurityRightsListForSecurityItemCode(string securityItemCode)
        {
            try
            {
                return (HttpContext.Current.Session["USERSECURITYRIGHTS"] as IEnumerable<UserSecurityRights>).Where(x => x.Code.ToUpper() == securityItemCode.ToUpper());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Set ApplicationSettings List
        /// </summary>
        /// <param name="applicationSettingsList">Application Settings List</param>
        public static void SetApplicationSettingsList(IEnumerable<ApplicationSettings> applicationSettingsList)
        {
            HttpContext.Current.Session["APPLICATIONSETTINGS"] = applicationSettingsList;
        }

        /// <summary>
        /// Set UserSecurityRights List
        /// </summary>
        /// <param name="userSecurityRightsList">User Security Rights List</param>
        public static void SetSecurityRightsList(IEnumerable<UserSecurityRights> userSecurityRightsList)
        {
            HttpContext.Current.Session["USERSECURITYRIGHTS"] = userSecurityRightsList;
        }

        /// <summary>
        /// Gets Sort
        /// </summary>
        /// <param name="gridId">Grid Id</param>
        public static IEnumerable<Sort> Sort(string gridId)
        {
            try
            {
                var value = HttpContext.Current.Request.Cookies.Get(string.Format("SORT-{0}", gridId)).Value;
                return JsonConvert.DeserializeObject<IEnumerable<Sort>>(HttpUtility.UrlDecode(value));
            }
            catch
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Configuration Class
    /// </summary>
    public static class Configuration
    {
        public const string APPLICATION_COMPONENT = "APPLICATIONCOMPONENT";
        public const string LOG_ACTION = "LOGACTION";
        public const string SECURITY_LEVEL = "SECURITYLEVEL";
    }

    /// <summary>
    /// Configuration Settings Key Name Class
    /// </summary>
    public static class ConfigurationSettingsKeyName
    {
        public const string APPLICATIONDOMAINLIST = "APPLICATIONDOMAINLIST";
        public const string GRIDMINROWS = "GRIDMINROWS";
        public const string ISHIDETERMINATED = "ISHIDETERMINATED";
    }

    /// <summary>
    /// Page Id Class
    /// </summary>
    public static class PageId
    {
        public const string PAGE_APPLICATION = "PAGE_APPLICATION";
        public const string PAGE_APPLICATIONSETTINGS = "PAGE_APPLICATIONSETTINGS";
        public const string PAGE_BUSINESSENTITYACCESSBYAD = "PAGE_BUSINESSENTITYACCESSBYAD";
        public const string PAGE_BUSINESSENTITYACCESSBYROLE = "PAGE_BUSINESSENTITYACCESSBYROLE";
        public const string PAGE_BUSINESSENTITYACCESSBYUSER = "PAGE_BUSINESSENTITYACCESSBYUSER";
        public const string PAGE_BUSINESSENTITYRESTRICTIONBYAD = "PAGE_BUSINESSENTITYRESTRICTIONBYAD";
        public const string PAGE_BUSINESSENTITYRESTRICTIONBYROLE = "PAGE_BUSINESSENTITYRESTRICTIONBYROLE";
        public const string PAGE_BUSINESSENTITYRESTRICTIONBYUSER = "PAGE_BUSINESSENTITYRESTRICTIONBYUSER";
        public const string PAGE_COMPANY = "PAGE_COMPANY";
        public const string PAGE_COMPANYINAPPLICATION = "PAGE_COMPANYINAPPLICATION";
        public const string PAGE_CONFIGURATION = "PAGE_CONFIGURATION";
        public const string PAGE_EXTERNALCOMPANY = "PAGE_EXTERNALCOMPANY";
        public const string PAGE_EXTERNALPERSON = "PAGE_EXTERNALPERSON";
        public const string PAGE_SECURITYBUSINESSENTITIES = "PAGE_SECURITYBUSINESSENTITIES";
        public const string PAGE_SECURITYITEM = "PAGE_SECURITYITEM";
        public const string PAGE_SECURITYREPORT = "PAGE_SECURITYREPORT";
        public const string PAGE_SECURITYROLERIGHTS = "PAGE_SECURITYROLERIGHTS";
        public const string PAGE_SECURITYROLES = "PAGE_SECURITYROLES";
        public const string PAGE_SECURITYTYPE = "PAGE_SECURITYTYPE";
        public const string PAGE_SECURITYUSERINROLES = "PAGE_SECURITYUSERINROLES";
        public const string PAGE_SECURITYUSERRIGHTS = "PAGE_SECURITYUSERRIGHTS";
        public const string PAGE_USERACCOUNT = "PAGE_USERACCOUNT";
        public const string PAGE_USERAPPLICATIONFAVOURITES = "PAGE_USERAPPLICATIONFAVOURITES";
        public const string PAGE_USERAPPLICATIONSETTINGS = "PAGE_USERAPPLICATIONSETTINGS";
        public const string PAGE_USERDELEGATE = "PAGE_USERDELEGATE";
        public const string PAGE_USERINCOMPANYINAPPLICATION = "PAGE_USERINCOMPANYINAPPLICATION";
        public const string PAGE_USERSETTINGS = "PAGE_USERSETTINGS";
        public const string PAGE_USERATTRIBUTE = "PAGE_USERATTRIBUTE";
    }
}