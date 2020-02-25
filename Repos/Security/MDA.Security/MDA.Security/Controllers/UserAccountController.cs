namespace MDA.Security.Controllers
{
    using AutoMapper;
    using BLL;
    using Core.Exception;
    using Core.Filters;
    using Core.Helpers;
    using Filters;
    using Globals;
    using Helpers;
    using IBLL;
    using Linq;
    using Models;
    using Newtonsoft.Json;
    using Reports;
    using Resources;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI;
    using ViewModels;

    [SessionTimeout]
    [HandleAndLogError(ExceptionType = typeof(UnauthorizedException), View = "_UnauthorizedAccess", Order = 2)]
    [HandleAndLogError(ExceptionType = typeof(Exception), View = "_Error", Order = 1)]
    public class UserAccountController : Controller
    {
        /// <summary>
        /// Gets Mapper Instance
        /// </summary>
        public IMapper MapperInstance
        {
            get
            {
                var mapperConfiguration = new MapperConfiguration(x =>
                {
                    x.CreateMap<UserAccountViewModel, UserAccount>();
                    x.CreateMap<UserAccountDetails, UserAccountViewModel>()
                        .ForMember(dest => dest.EmployeeNumber, opt => opt.MapFrom(src => src.Code))
                        .ForMember(dest => dest.PersonCode, opt => opt.MapFrom(src => src.Code));

                    x.CreateMap<UserApplicationSettingsViewModel, UserApplicationSettings>();
                    x.CreateMap<UserApplicationSettings, UserApplicationSettingsViewModel>();
                });

                return mapperConfiguration.CreateMapper();
            }
        }

        /// <summary>
        /// Application Favourites
        /// </summary>
        /// <param name="formCollection">Form Collection</param>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "UserAccountForm", MatchFormValue = "Application Favourites")]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_USERAPPLICATIONFAVOURITES)]
        public RedirectToRouteResult ApplicationFavourites(FormCollection formCollection)
        {
            ApplicationGlobals.SelectedUserAccount = GetViewModel(int.Parse(formCollection["hidGridCheckedRowsUserAccount"]));
            return RedirectToAction("Index", "UserApplicationFavourites");
        }

        /// <summary>
        /// Get Available Company Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Available Company Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operations = new[] { DataOperation.CREATE, DataOperation.UPDATE }, SecurityItemCode = PageId.PAGE_USERACCOUNT)]
        public ActionResult AvailableCompanyPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            ICompanyBll iCompanyBll = new CompanyBll();
            var companyPage = iCompanyBll.GetCompanyPage(take, skip, sort, filter);

            return Json(companyPage);
        }

        /// <summary>
        /// Get Available Employees Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Available Employees Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operations = new[] { DataOperation.CREATE, DataOperation.UPDATE }, SecurityItemCode = PageId.PAGE_USERACCOUNT)]
        public ActionResult AvailableEmployeesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            IRO_EmployeeBll iRO_EmployeeBll = new RO_EmployeeBll();
            var availableEmployeesPage = iRO_EmployeeBll.GetAvailableEmployeesPage(take, skip, sort, filter, ApplicationGlobals.IsHideTerminated);

            return Json(availableEmployeesPage);
        }

        /// <summary>
        /// Get Available External Persons Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Available External Persons Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operations = new[] { DataOperation.CREATE, DataOperation.UPDATE }, SecurityItemCode = PageId.PAGE_USERACCOUNT)]
        public ActionResult AvailableExternalPersonsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            IExternalPersonBll iExternalPersonBll = new ExternalPersonBll();
            var externalPersonPage = iExternalPersonBll.GetAvailableExternalPersonPage(take, skip, sort, filter, ApplicationGlobals.IsHideTerminated);

            return Json(externalPersonPage);
        }

        /// <summary>
        /// Bind Data
        /// </summary>
        /// <param name="selectedId">Selected Id</param>
        /// <param name="dataOperation">Data Operation</param>
        /// <param name="userAccountType">User Account Type</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BindData(int selectedId, DataOperation dataOperation, UserAccountType userAccountType = UserAccountType.INTERNAL)
        {
            var userAccountViewModel = GetViewModel(selectedId, dataOperation, userAccountType);
            return PartialView(userAccountViewModel.UserAccountType == UserAccountType.INTERNAL ? "MaintainInternalUser" : "MaintainExternalUser", userAccountViewModel);
        }

        /// <summary>
        /// Bind UserApplicationSettings
        /// <param name="id">Id</param>
        /// </summary>
        /// <returns>MaintainUserApplicationSettings View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.UPDATE, SecurityItemCode = PageId.PAGE_USERAPPLICATIONSETTINGS)]
        public ActionResult BindUserApplicationSettings(int id)
        {
            IUserApplicationSettingsBll iUserApplicationSettingsBll = new UserApplicationSettingsBll();
            var userApplicationSettings = iUserApplicationSettingsBll.GetUserApplicationSettingsForUserAccountId(id);

            var userApplicationSettingsViewModel = userApplicationSettings == null ?
                new UserApplicationSettingsViewModel { LnUserAccountId = id } : MapperInstance.Map<UserApplicationSettingsViewModel>(userApplicationSettings);

            ICompanyBll iCompanyBll = new CompanyBll();
            userApplicationSettingsViewModel.CompanyList = iCompanyBll.GetCompanyDropDownListForUserAccountId(id);

            userApplicationSettingsViewModel.DataAction = userApplicationSettings == null ? DataOperation.CREATE : DataOperation.EDIT_VIEW;
            return PartialView("MaintainUserApplicationSettings", userApplicationSettingsViewModel);
        }

        /// <summary>
        /// Bind UserAttributes
        /// <param name="id">Id</param>
        /// </summary>
        /// <returns>MaintainUserAttribute View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        //[AuthorizeUser(Operations = new[] { DataOperation.CREATE, DataOperation.UPDATE }, SecurityItemCode = PageId.PAGE_USERATTRIBUTE)]
        public ActionResult BindUserAttribute(int id)
        {
            IUserAttributeBll iUserAttributeBll = new UserAttributeBll();
            var userAttribute = iUserAttributeBll.GetUserAttribute(id);

            UserAttributeViewModel userAttributeViewModel = new UserAttributeViewModel();
            userAttributeViewModel.UserAttribute = userAttribute;
            return PartialView("MaintainUserAttribute", userAttributeViewModel);
        }

        /// <summary>
        /// Delegate
        /// </summary>
        /// <param name="formCollection">Form Collection</param>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "UserAccountForm", MatchFormValue = "Delegate")]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_USERDELEGATE)]
        public RedirectToRouteResult Delegate(FormCollection formCollection)
        {
            ApplicationGlobals.SelectedUserAccount = GetViewModel(int.Parse(formCollection["hidGridCheckedRowsUserAccount"]));
            return RedirectToAction("Index", "UserDelegate");
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Selected Id</param>
        /// <returns>True on Success, False on Failure</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.DELETE, SecurityItemCode = PageId.PAGE_USERACCOUNT)]
        public JsonResult Delete(int id)
        {
            IUserAccountBll iUserAccountBll = new UserAccountBll();
            var bActionPass = iUserAccountBll.DeleteUserAccount(id, ApplicationGlobals.UserName);

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Delete));
        }

        /// <summary>
        /// Export to PDF
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "UserAccountForm", MatchFormValue = "Export to PDF")]
        public ActionResult ExportToPdf()
        {
            GenerateReport(ReportFormat.PDF, ApplicationGlobals.Sort("gridUserAccount"), ApplicationGlobals.Filter("gridUserAccount"));
            return View("Index");
        }

        /// <summary>
        /// Export to Xls
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "UserAccountForm", MatchFormValue = "Export to XLS")]
        public ActionResult ExportToXls()
        {
            GenerateReport(ReportFormat.XLS, ApplicationGlobals.Sort("gridUserAccount"), ApplicationGlobals.Filter("gridUserAccount"));
            return View("Index");
        }

        /// <summary>
        /// Get Company Description
        /// </summary>
        /// <param name="id">Selected Id</param>
        /// <returns>Company Description</returns>
        public JsonResult GetCompanyDescription(int id)
        {
            ICompanyBll iCompanyBll = new CompanyBll();
            var companyDescription = iCompanyBll.GetCompanyForId(id).Description;

            return Json(companyDescription, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Employee Data
        /// </summary>
        /// <param name="id">Selected Id</param>
        /// <returns>Employee Data</returns>
        public JsonResult GetEmployeeData(int id)
        {
            IRO_EmployeeBll iRO_EmployeeBll = new RO_EmployeeBll();
            var employee = iRO_EmployeeBll.GetEmployeeForId(id);

            var userName = string.Format("{0}{1}", employee.FirstName.Substring(0, 1),
                (employee.LastName.Length >= 7 ? employee.LastName.Substring(0, 7) : employee.LastName)).Trim().ToLower();

            return Json(new { EmpNo = employee.EmpNo, FullName = employee.FullName, UserName = userName }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get External Person Data
        /// </summary>
        /// <param name="id">Selected Id</param>
        /// <returns>External Person Data</returns>
        public JsonResult GetExternalPersonData(int id)
        {
            IExternalPersonBll iExternalPersonBll = new ExternalPersonBll();
            var externalPerson = iExternalPersonBll.GetExternalPersonForId(id);

            var userName = string.Format("{0}{1}", externalPerson.FirstName.Substring(0, 1),
                (externalPerson.LastName.Length >= 7 ? externalPerson.LastName.Substring(0, 7) : externalPerson.LastName)).Trim().ToLower();

            return Json(new { PersonCode = externalPerson.PersonCode, FullName = externalPerson.FullName, UserName = userName }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>Index View</returns>
        [IsNewSession]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_USERACCOUNT)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Insert
        /// Called when the Submit button is clicked on the Insert Page
        /// </summary>
        /// <param name="userAccountViewModel">UserAccountViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_USERACCOUNT)]
        public JsonResult Insert(UserAccountViewModel userAccountViewModel)
        {
            IUserAccountBll iUserAccountBll = new UserAccountBll();
            var bActionPass = ModelState.IsValid && iUserAccountBll.InsertUserAccount(GetModel(userAccountViewModel), ApplicationGlobals.UserName);

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_New_Record_Insert));
        }

        /// <summary>
        /// Check for Code Duplicates
        /// The OutputCacheAttribute attribute is required in order to prevent
        /// ASP.NET MVC from caching the results of the validation method.
        /// </summary>
        /// <param name="userAccountViewModel">UserAccountViewModel Object</param>
        /// <returns>True or False</returns>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult IsCodeDuplicate(UserAccountViewModel userAccountViewModel)
        {
            IUserAccountBll iUserAccountBll = new UserAccountBll();
            var userAccount = iUserAccountBll.GetUserAccountForCode(userAccountViewModel.UserName.Trim());

            var isCodeDuplicate = (userAccount != null && userAccountViewModel.Id == userAccount.Id) || userAccount == null;
            return Json(isCodeDuplicate, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Check for Employee Number Duplicates
        /// The OutputCacheAttribute attribute is required in order to prevent
        /// ASP.NET MVC from caching the results of the validation method.
        /// </summary>
        /// <param name="userAccountViewModel">UserAccountViewModel Object</param>
        /// <returns>True or False</returns>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult IsEmployeeNumberDuplicate(UserAccountViewModel userAccountViewModel)
        {
            IUserAccountBll iUserAccountBll = new UserAccountBll();
            var userAccount = iUserAccountBll.GetUserAccountForEmployeeId(userAccountViewModel.LnEmployeeId);

            var isEmployeeNumberDuplicate = (userAccount != null && userAccountViewModel.Id == userAccount.Id) || userAccount == null;
            return Json(isEmployeeNumberDuplicate, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Check for Person Code Duplicates The OutputCacheAttribute attribute is required in order
        /// to prevent ASP.NET MVC from caching the results of the validation method.
        /// </summary>
        /// <param name="userAccountViewModel">UserAccountViewModel Object</param>
        /// <returns>True or False</returns>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult IsPersonCodeDuplicate(UserAccountViewModel userAccountViewModel)
        {
            IUserAccountBll iUserAccountBll = new UserAccountBll();
            var userAccount = iUserAccountBll.GetUserAccountForExternalPersonId(userAccountViewModel.LnExternalPersonId);

            var isPersonCodeDuplicate = (userAccount != null && userAccountViewModel.Id == userAccount.Id) || userAccount == null;
            return Json(isPersonCodeDuplicate, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Is Record Deleted
        /// </summary>
        /// <param name="id">Selected Id</param>
        /// <returns>True if Record is Deleted, else False</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult IsRecordDeleted(int id)
        {
            IUserAccountBll iUserAccountBll = new UserAccountBll();
            var userAccount = iUserAccountBll.GetUserAccountForId(id);

            return Json(userAccount.IsRecordDeleted, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Projects Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Projects Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.UPDATE, SecurityItemCode = PageId.PAGE_USERAPPLICATIONSETTINGS)]
        public ActionResult ProjectsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            IProjectsBll iProjectBll = new ProjectsBll();
            var projectsPage = iProjectBll.GetProjectsPage(take, skip, sort, filter);

            return Content(JsonConvert.SerializeObject(projectsPage));
        }

        /// <summary>
        /// Save User Application Settings
        /// </summary>
        /// <param name="userApplicationSettingsViewModel">UserApplicationSettingsViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.UPDATE, SecurityItemCode = PageId.PAGE_USERAPPLICATIONSETTINGS)]
        public JsonResult SaveUserApplicationSettings(UserApplicationSettingsViewModel userApplicationSettingsViewModel)
        {
            IUserApplicationSettingsBll iUserApplicationSettingsBll = new UserApplicationSettingsBll();

            var bActionPass = userApplicationSettingsViewModel.DataAction == DataOperation.CREATE ?
                iUserApplicationSettingsBll.InsertUserApplicationSettings(GetUserApplicationSettingsModel(userApplicationSettingsViewModel), ApplicationGlobals.UserName) :
                iUserApplicationSettingsBll.UpdateUserApplicationSettings(GetUserApplicationSettingsModel(userApplicationSettingsViewModel), ApplicationGlobals.UserName);

            if (bActionPass && userApplicationSettingsViewModel.LnUserAccountId == ApplicationGlobals.UserId)
            {
                var userApplicationSettings = iUserApplicationSettingsBll.GetUserApplicationSettingsForUserAccountId(userApplicationSettingsViewModel.LnUserAccountId);

                ApplicationGlobals.DefaultCompany = userApplicationSettings.CompanyObj.Description;
                ApplicationGlobals.LnDefaultCompanyId = userApplicationSettings.LnCompanyId;
                ApplicationGlobals.CanDisplayTreeMenuVertical = userApplicationSettings.UserMenuLocation == TreeMenuLocation.Vertical;
                ApplicationGlobals.CultureInfo = new CultureInfo(userApplicationSettings.Language);
            }

            var actionName = string.Format("{0} {1}", ResourceStrings.Text_UserApplicationSettings, userApplicationSettingsViewModel.DataAction == DataOperation.CREATE ? ResourceStrings.Text_New_Record_Insert : ResourceStrings.Text_Edit_View);
            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, actionName));
        }

        /// <summary>
        /// Update
        /// Called when the Submit button is clicked on the Edit Page
        /// </summary>
        /// <param name="userAccountViewModel">UserAccountViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.UPDATE, SecurityItemCode = PageId.PAGE_USERACCOUNT)]
        public JsonResult Update(UserAccountViewModel userAccountViewModel)
        {
            IUserAccountBll iUserAccountBll = new UserAccountBll();
            var bActionPass = ModelState.IsValid && iUserAccountBll.UpdateUserAccount(GetModel(userAccountViewModel), ApplicationGlobals.UserName);

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Edit_View));
        }

        /// <summary>
        /// Get UserAccount Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>UserAccount Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_USERACCOUNT)]
        public ActionResult UserAccountPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            IUserAccountBll iUserAccountBll = new UserAccountBll();
            var userAccountPage = iUserAccountBll.GetUserAccountPage(take, skip, sort, filter, ApplicationGlobals.IsHideTerminated);

            return Content(JsonConvert.SerializeObject(userAccountPage));
        }

        /// <summary>
        /// Handle Unknown Action
        /// </summary>
        /// <param name="actionName">Action Name</param>
        protected override void HandleUnknownAction(string actionName)
        {
            throw new UnauthorizedAccessException(string.Format(ResourceStrings.Message_Unknown_Action, actionName));
        }

        /// <summary>
        /// Generates Report based on the Report Format
        /// </summary>
        /// <param name="reportFormat">Report Format PDF/XLS</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>True on Success, False on Failure</returns>
        private bool GenerateReport(ReportFormat reportFormat, IEnumerable<Sort> sort, LinqFilter filter)
        {
            Reports<UserAccountDetails> reports;
            switch (reportFormat)
            {
                case ReportFormat.PDF:
                    reports = new PdfReport<UserAccountDetails>();
                    break;

                case ReportFormat.XLS:
                    reports = new ExcelReport<UserAccountDetails>();
                    break;

                default:
                    throw new Exception("Invalid Report Format");
            }

            // Set Column Widths
            reports.ColumnWidthInPercent = new float[] { 10, 10, 14, 16, 25, 8, 10, 7 };

            // Set Report Details
            reports.ReportTitle = ResourceStrings.Text_UserAccount;
            reports.UserName = ApplicationGlobals.UserName;

            reports.ReportFilePath = Server.MapPath(ApplicationGlobals.TempDirectory);
            reports.CompanyLogo = Server.MapPath(ApplicationGlobals.LogoDirectory);

            // Set the Output Fields
            reports.OutputColumns = new[] { "Code", "UserName", "FullName", "Domain", "eMail", "UserAccountType", "Company", "IsRecordDeleted" };
            reports.ColumnHeaders = ApplicationGlobals.VisibleGridColumnHeaders;

            // Set Report Headers
            reports.HeaderList = new[]
            {
                new ReportHeader(string.Empty, DateTime.Now.ToString(ApplicationGlobals.ShortDateTimePattern)),
                new ReportHeader(ApplicationGlobals.ApplicationName, string.Empty),
                new ReportHeader(string.Format(ResourceStrings.Text_Version, ApplicationGlobals.ApplicationVersion), string.Empty)
            };

            // Get the Report Data
            IUserAccountBll iUserAccountBll = new UserAccountBll();
            reports.ReportData = iUserAccountBll.GetUserAccountList(sort, filter, ApplicationGlobals.IsHideTerminated);

            // Generate the Report
            bool bActionPass = reports.GenerateReport();

            // Download the File
            ViewBag.ErrorMessage = bActionPass ? string.Format(ResourceStrings.Message_Success, ResourceStrings.Text_Export) : ResourceStrings.Message_No_Data_Found;
            if (bActionPass)
            {
                ApplicationGlobals.DownloadFile(reports.ReportFilePath);
            }

            return bActionPass;
        }

        /// <summary>
        /// Get Domain List
        /// </summary>
        /// <param name="domain">Domain</param>
        /// <returns>Domain List</returns>
        private IEnumerable<SelectListItem> GetDomainList(string domain)
        {
            return string.IsNullOrEmpty(ApplicationGlobals.ApplicationDomainList) ?
                new List<SelectListItem> { new SelectListItem { Text = string.Empty, Value = string.Empty } } :
                ApplicationGlobals.ApplicationDomainList.Split(',').AsEnumerable()
                    .Select(x => new SelectListItem { Selected = !string.IsNullOrEmpty(domain) && (x == domain), Text = x.Trim(), Value = x.Trim() });
        }

        /// <summary>
        /// Get Model
        /// </summary>
        /// <param name="userAccountViewModel">UserAccountViewModel Object</param>
        /// <returns>Business Entity</returns>
        private UserAccount GetModel(UserAccountViewModel userAccountViewModel)
        {
            return MapperInstance.Map<UserAccount>(userAccountViewModel);
        }

        /// <summary>
        /// Get UserApplicationSettings Model
        /// </summary>
        /// <param name="userApplicationSettingsViewModel">UserApplicationSettingsViewModel Object</param>
        /// <returns>Business Entity</returns>
        private UserApplicationSettings GetUserApplicationSettingsModel(UserApplicationSettingsViewModel userApplicationSettingsViewModel)
        {
            return MapperInstance.Map<UserApplicationSettings>(userApplicationSettingsViewModel);
        }

        /// <summary>
        /// Get View Model
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="dataOperation">Data Operation</param>
        /// <param name="userAccountType">User Account Type</param>
        /// <returns>Business Entity</returns>
        private UserAccountViewModel GetViewModel(int id, DataOperation dataOperation = 0, UserAccountType userAccountType = UserAccountType.INTERNAL)
        {
            IUserAccountBll iUserAccountBll = new UserAccountBll();
            var userAccount = iUserAccountBll.GetUserAccountForId(id);

            var userAccountViewModel = userAccount == null ?
                new UserAccountViewModel() : MapperInstance.Map<UserAccountViewModel>(userAccount);

            if (dataOperation != 0)
            {
                userAccountType = userAccountViewModel.Id == 0 ? userAccountType :
                    userAccountViewModel.LnExternalPersonId == 0 ? UserAccountType.INTERNAL : UserAccountType.EXTERNAL;

                userAccountViewModel.DomainList = GetDomainList(userAccountViewModel.Domain);
            }

            userAccountViewModel.UserAccountType = userAccountType;
            userAccountViewModel.DataAction = dataOperation;

            return userAccountViewModel;
        }
    }
}