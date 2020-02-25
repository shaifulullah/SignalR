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
    public class UserInCompanyInApplicationController : Controller
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
                    x.CreateMap<UserInCompanyInApplication, UserInCompanyInApplicationViewModel>();
                });

                return mapperConfiguration.CreateMapper();
            }
        }

        /// <summary>
        /// Get Available UserAccount Page For Company In Application
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Available UserAccount Page For Company In Application</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_USERINCOMPANYINAPPLICATION)]
        public ActionResult AvailableUserAccountPageForCompanyInApplication(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            IUserAccountBll iUserAccountBll = new UserAccountBll();
            var availableUserAccountPageForCompanyInApplication = iUserAccountBll.GetAvailableUserAccountPageForCompanyInApplicationId(take, skip, sort, filter, ApplicationGlobals.SelectedCompanyInApplication.Id, ApplicationGlobals.IsHideTerminated);

            return Content(JsonConvert.SerializeObject(availableUserAccountPageForCompanyInApplication));
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="ids">Selected Id's</param>
        /// <returns>True on Success, False on Failure</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.DELETE, SecurityItemCode = PageId.PAGE_USERINCOMPANYINAPPLICATION)]
        public JsonResult Delete(int[] ids)
        {
            IUserInCompanyInApplicationBll iUserInCompanyInApplicationBll = new UserInCompanyInApplicationBll();
            var bActionPass = ids.Aggregate(true, (current, id) => current & iUserInCompanyInApplicationBll.DeleteUserInCompanyInApplication(id, ApplicationGlobals.UserName));

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Remove));
        }

        /// <summary>
        /// Export to PDF
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "UserInCompanyInApplicationForm", MatchFormValue = "Export to PDF")]
        public ActionResult ExportToPdf()
        {
            GenerateReport(ReportFormat.PDF, ApplicationGlobals.Sort("gridUserInCompanyInApplication"), ApplicationGlobals.Filter("gridUserInCompanyInApplication"));
            return View("Index");
        }

        /// <summary>
        /// Export to Xls
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "UserInCompanyInApplicationForm", MatchFormValue = "Export to XLS")]
        public ActionResult ExportToXls()
        {
            GenerateReport(ReportFormat.XLS, ApplicationGlobals.Sort("gridUserInCompanyInApplication"), ApplicationGlobals.Filter("gridUserInCompanyInApplication"));
            return View("Index");
        }

        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>Index View</returns>
        [IsNewSession]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_USERINCOMPANYINAPPLICATION)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Insert
        /// Called when the Submit button is clicked on the Insert Page
        /// </summary>
        /// <param name="ids">Selected Id's</param>
        /// <returns>True on Success, False on Failure</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_USERINCOMPANYINAPPLICATION)]
        public JsonResult Insert(int[] ids)
        {
            IUserInCompanyInApplicationBll iUserInCompanyInApplicationBll = new UserInCompanyInApplicationBll();
            var bActionPass = ids.Aggregate(true, (current, id) => current & iUserInCompanyInApplicationBll.InsertUserInCompanyInApplication(new UserInCompanyInApplication { LnUserAccountId = id, LnCompanyInApplicationId = ApplicationGlobals.SelectedCompanyInApplication.Id }, ApplicationGlobals.UserName));

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Add));
        }

        /// <summary>
        /// Roles
        /// </summary>
        /// <param name="formCollection">Form Collection</param>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "UserInCompanyInApplicationForm", MatchFormValue = "Roles")]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_SECURITYUSERINROLES)]
        public RedirectToRouteResult Roles(FormCollection formCollection)
        {
            ApplicationGlobals.SelectedUserInCompanyInApplication = GetViewModel(int.Parse(formCollection["hidGridCheckedRowsUserInCompanyInApplication"]));
            return RedirectToAction("Index", "UserInCompanyInApplicationSecurityRoles");
        }

        /// <summary>
        /// Security Items
        /// </summary>
        /// <param name="formCollection">Form Collection</param>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "UserInCompanyInApplicationForm", MatchFormValue = "Security Items")]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_SECURITYUSERRIGHTS)]
        public RedirectToRouteResult SecurityItems(FormCollection formCollection)
        {
            ApplicationGlobals.SelectedUserInCompanyInApplication = GetViewModel(int.Parse(formCollection["hidGridCheckedRowsUserInCompanyInApplication"]));
            return RedirectToAction("Index", "SecurityUserRights");
        }

        /// <summary>
        /// Get UserInCompanyInApplication Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>UserInCompanyInApplication Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_USERINCOMPANYINAPPLICATION)]
        public ActionResult UserInCompanyInApplicationPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            IUserInCompanyInApplicationBll iUserInCompanyInApplicationBll = new UserInCompanyInApplicationBll();
            var userInCompanyInApplicationPage = iUserInCompanyInApplicationBll.GetUserInCompanyInApplicationPage(take, skip, sort, filter, ApplicationGlobals.SelectedCompanyInApplication.Id, ApplicationGlobals.IsHideTerminated);

            return Content(JsonConvert.SerializeObject(userInCompanyInApplicationPage));
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
            reports.ColumnWidthInPercent = new float[] { 15, 30, 35, 20 };

            // Set Report Details
            reports.ReportTitle = ResourceStrings.Text_UserInCompanyInApplication;
            reports.UserName = ApplicationGlobals.UserName;

            reports.ReportFilePath = Server.MapPath(ApplicationGlobals.TempDirectory);
            reports.CompanyLogo = Server.MapPath(ApplicationGlobals.LogoDirectory);

            // Set the Output Fields
            reports.OutputColumns = new[] { "UserName", "FullName", "eMail", "Company" };
            reports.ColumnHeaders = ApplicationGlobals.VisibleGridColumnHeaders;

            // Set Report Headers
            reports.HeaderList = new[]
            {
                new ReportHeader(string.Empty, DateTime.Now.ToString(ApplicationGlobals.ShortDateTimePattern)),
                new ReportHeader(ApplicationGlobals.ApplicationName, string.Empty),
                new ReportHeader(string.Format(ResourceStrings.Text_Version, ApplicationGlobals.ApplicationVersion), string.Empty),
                new ReportHeader(ApplicationGlobals.GenerateReportHeaderDetails("Application"), string.Empty),
                new ReportHeader(ApplicationGlobals.GenerateReportHeaderDetails("CompanyInApplication"), string.Empty)
            };

            // Get the Report Data
            IUserInCompanyInApplicationBll iUserInCompanyInApplicationBll = new UserInCompanyInApplicationBll();
            reports.ReportData = iUserInCompanyInApplicationBll.GetUserInCompanyInApplicationList(sort, filter, ApplicationGlobals.SelectedCompanyInApplication.Id, ApplicationGlobals.IsHideTerminated);

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
        /// Get View Model
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        private UserInCompanyInApplicationViewModel GetViewModel(int id)
        {
            IUserInCompanyInApplicationBll iUserInCompanyInApplicationBll = new UserInCompanyInApplicationBll();
            var userInCompanyInApplication = iUserInCompanyInApplicationBll.GetUserInCompanyInApplicationForId(id);

            var userInCompanyInApplicationViewModel = MapperInstance.Map<UserInCompanyInApplicationViewModel>(userInCompanyInApplication);

            IUserAccountBll iUserAccountBll = new UserAccountBll();
            userInCompanyInApplicationViewModel.UserAccountDetailsObj = iUserAccountBll.GetUserAccountForId(userInCompanyInApplication.LnUserAccountId);

            return userInCompanyInApplicationViewModel;
        }
    }
}