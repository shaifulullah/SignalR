namespace MDA.Security.Controllers
{
    using AutoMapper;
    using BLL;
    using Core.Exception;
    using Core.Filters;
    using Filters;
    using Globals;
    using IBLL;
    using Linq;
    using Models;
    using Newtonsoft.Json;
    using Reports;
    using Resources;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using ViewModels;

    [SessionTimeout]
    [HandleAndLogError(ExceptionType = typeof(UnauthorizedException), View = "_UnauthorizedAccess", Order = 2)]
    [HandleAndLogError(ExceptionType = typeof(Exception), View = "_Error", Order = 1)]
    public class UserDelegateController : Controller
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
                    x.CreateMap<UserDelegateViewModel, UserDelegate>();

                    x.CreateMap<UserDelegate, UserDelegateViewModel>()
                        .ForMember(dest => dest.DelegateUserName, opt => opt.MapFrom(src => src.DelegateUserAccountObj.UserName))
                        .ForMember(dest => dest.SecurityRoles, opt => opt.MapFrom(src => src.SecurityRolesObj.Description)); ;
                });

                return mapperConfiguration.CreateMapper();
            }
        }

        /// <summary>
        /// Available Delegate Page For Security Role
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Available Delegate Page For Security Role</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_USERDELEGATE)]
        public ActionResult AvailableDelegatePageForSecurityRole(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            ISecurityUserInRolesBll iSecurityUserInRolesBll = new SecurityUserInRolesBll();
            var userAccountIdList = iSecurityUserInRolesBll.GetUserAccountIdListForSecurityRolesId(ApplicationGlobals.TempIdNumber, ApplicationGlobals.SelectedUserAccount.Id);

            IUserDelegateBll iUserDelegateBll = new UserDelegateBll();
            var availableUserDelegatePage = iUserDelegateBll.GetAvailableUserDelegatePage(take, skip, sort, filter, userAccountIdList, ApplicationGlobals.IsHideTerminated);

            return Json(availableUserDelegatePage);
        }

        /// <summary>
        /// Get Available SecurityRoles Page For User Account
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Available SecurityRoles Page For User Account</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_USERDELEGATE)]
        public ActionResult AvailableSecurityRolesPageForUserAccount(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            ISecurityRolesBll iSecurityRolesBll = new SecurityRolesBll();
            var securityRolesPage = iSecurityRolesBll.GetAvailableSecurityRolesPageForUserAccountId(take, skip, sort, filter, ApplicationGlobals.SelectedUserAccount.Id);

            return Content(JsonConvert.SerializeObject(securityRolesPage));
        }

        /// <summary>
        /// Bind Data
        /// </summary>
        /// <param name="selectedId">Selected Id</param>
        /// <param name="dataOperation">Data Operation</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BindData(int selectedId, DataOperation dataOperation)
        {
            var userDelegateViewModel = GetViewModel(selectedId, dataOperation);

            IUserDelegateBll iUserDelegateBll = new UserDelegateBll();
            var delegateCount = iUserDelegateBll.GetUserDelegateCountForUserAccountId(ApplicationGlobals.SelectedUserAccount.Id);

            userDelegateViewModel.DelegateFlagIsApprover = delegateCount == 0;

            return PartialView("MaintainUserDelegate", userDelegateViewModel);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="ids">Selected Id's</param>
        /// <returns>True on Success, False on Failure</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.DELETE, SecurityItemCode = PageId.PAGE_USERDELEGATE)]
        public JsonResult Delete(int[] ids)
        {
            IUserDelegateBll iUserDelegateBll = new UserDelegateBll();
            var bActionPass = ids.Aggregate(true, (current, id) => current & iUserDelegateBll.DeleteUserDelegate(id, ApplicationGlobals.UserName));

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Delete));
        }

        /// <summary>
        /// Export to PDF
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "UserDelegateForm", MatchFormValue = "Export to PDF")]
        public ActionResult ExportToPdf()
        {
            GenerateReport(ReportFormat.PDF, ApplicationGlobals.Sort("gridUserDelegate"), ApplicationGlobals.Filter("gridUserDelegate"));
            return View("Index");
        }

        /// <summary>
        /// Export to Xls
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "UserDelegateForm", MatchFormValue = "Export to XLS")]
        public ActionResult ExportToXls()
        {
            GenerateReport(ReportFormat.XLS, ApplicationGlobals.Sort("gridUserDelegate"), ApplicationGlobals.Filter("gridUserDelegate"));
            return View("Index");
        }

        /// <summary>
        /// Get Delegate UserName
        /// </summary>
        /// <param name="id">Selected Id</param>
        /// <returns>Delegate UserName</returns>
        public JsonResult GetDelegateUserName(int id)
        {
            IUserAccountBll iUserAccountBll = new UserAccountBll();
            var userAccountName = iUserAccountBll.GetUserAccountForId(id).UserName;

            return Json(userAccountName, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get SecurityRoles Description
        /// </summary>
        /// <param name="id">Selected Id</param>
        /// <returns>SecurityRoles Description</returns>
        public JsonResult GetSecurityRolesDescription(int id)
        {
            ISecurityRolesBll iSecurityRolesBll = new SecurityRolesBll();
            var securityRolesDescription = iSecurityRolesBll.GetSecurityRolesForId(id).Description;

            return Json(securityRolesDescription, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>Index View</returns>
        [IsNewSession]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_USERDELEGATE)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Insert
        /// Called when the Submit button is clicked on the Insert Page
        /// </summary>
        /// <param name="userDelegateViewModel">UserDelegateViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_USERDELEGATE)]
        public JsonResult Insert(UserDelegateViewModel userDelegateViewModel)
        {
            IUserDelegateBll iUserDelegateBll = new UserDelegateBll();
            var bActionPass = ModelState.IsValid && iUserDelegateBll.InsertUserDelegate(GetModel(userDelegateViewModel), ApplicationGlobals.UserName);

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_New_Record_Insert));
        }

        /// <summary>
        /// Update
        /// Called when the Submit button is clicked on the Edit Page
        /// </summary>
        /// <param name="userDelegateViewModel">UserDelegateViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.UPDATE, SecurityItemCode = PageId.PAGE_USERDELEGATE)]
        public JsonResult Update(UserDelegateViewModel userDelegateViewModel)
        {
            IUserDelegateBll iUserDelegateBll = new UserDelegateBll();
            var bActionPass = ModelState.IsValid && iUserDelegateBll.UpdateUserDelegate(GetModel(userDelegateViewModel), ApplicationGlobals.UserName);

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Edit_View));
        }

        /// <summary>
        /// Get UserDelegate Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>UserDelegate Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_USERDELEGATE)]
        public ActionResult UserDelegatePage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            IUserDelegateBll iUserDelegateBll = new UserDelegateBll();
            var userDelegatePage = iUserDelegateBll.GetUserDelegatePage(take, skip, sort, filter, ApplicationGlobals.SelectedUserAccount.Id);

            return Content(JsonConvert.SerializeObject(userDelegatePage));
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
            Reports<UserDelegate> reports;
            switch (reportFormat)
            {
                case ReportFormat.PDF:
                    reports = new PdfReport<UserDelegate>();
                    break;

                case ReportFormat.XLS:
                    reports = new ExcelReport<UserDelegate>();
                    break;

                default:
                    throw new Exception("Invalid Report Format");
            }

            // Set Column Widths
            reports.ColumnWidthInPercent = new float[] { 19, 15, 15, 15, 13, 13, 10 };

            // Set Report Details
            reports.ReportTitle = ResourceStrings.Text_UserDelegate;
            reports.UserName = ApplicationGlobals.UserName;

            reports.ReportFilePath = Server.MapPath(ApplicationGlobals.TempDirectory);
            reports.CompanyLogo = Server.MapPath(ApplicationGlobals.LogoDirectory);

            // Set the Output Fields
            reports.OutputColumns = new[] { "DelegateUserAccountObj.UserName", "SecurityRolesObj.Description", "SecurityRolesObj.CompanyInApplicationObj.ApplicationObj.Description", "SecurityRolesObj.CompanyInApplicationObj.CompanyObj.Description", "DateTimeStart", "DateTimeEnd", "FlagIsApprover" };
            reports.ColumnHeaders = ApplicationGlobals.VisibleGridColumnHeaders;

            // Set Date Format
            reports.Attributes.DateFormat = ApplicationGlobals.ShortDatePattern;

            // Set Report Headers
            reports.HeaderList = new[]
            {
                new ReportHeader(string.Empty, DateTime.Now.ToString(ApplicationGlobals.ShortDateTimePattern)),
                new ReportHeader(ApplicationGlobals.ApplicationName, string.Empty),
                new ReportHeader(string.Format(ResourceStrings.Text_Version, ApplicationGlobals.ApplicationVersion), string.Empty),
                new ReportHeader(ApplicationGlobals.GenerateReportHeaderDetails("UserAccount"), string.Empty)
            };

            // Get the Report Data
            IUserDelegateBll iUserDelegateBll = new UserDelegateBll();
            reports.ReportData = iUserDelegateBll.GetUserDelegateList(sort, filter, ApplicationGlobals.SelectedUserAccount.Id);

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
        /// Get Model
        /// </summary>
        /// <param name="userDelegateViewModel">UserDelegateViewModel Object</param>
        /// <returns>Business Entity</returns>
        private UserDelegate GetModel(UserDelegateViewModel userDelegateViewModel)
        {
            return MapperInstance.Map<UserDelegate>(userDelegateViewModel);
        }

        /// <summary>
        /// Get View Model
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="dataOperation">Data Operation</param>
        /// <returns>Business Entity</returns>
        private UserDelegateViewModel GetViewModel(int id, DataOperation dataOperation = 0)
        {
            IUserDelegateBll iUserDelegateBll = new UserDelegateBll();
            var userDelegate = iUserDelegateBll.GetUserDelegateForId(id);

            var userDelegateViewModel = userDelegate == null ?
                new UserDelegateViewModel() : MapperInstance.Map<UserDelegateViewModel>(userDelegate);

            userDelegateViewModel.LnUserAccountId = ApplicationGlobals.SelectedUserAccount.Id;

            userDelegateViewModel.DataAction = dataOperation;
            return userDelegateViewModel;
        }
    }
}