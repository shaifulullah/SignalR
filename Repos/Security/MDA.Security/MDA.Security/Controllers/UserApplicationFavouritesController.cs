namespace MDA.Security.Controllers
{
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
    public class UserApplicationFavouritesController : Controller
    {
        /// <summary>
        /// Get Available Application Page For User Account
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Available Application Page For User Account</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_USERAPPLICATIONFAVOURITES)]
        public ActionResult AvailableApplicationPageForUserAccount(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            IApplicationBll iApplicationBll = new ApplicationBll();
            var availableApplicationPageForUserAccount = iApplicationBll.GetAvailableApplicationPageForUserAccountId(take, skip, sort, filter, ApplicationGlobals.SelectedUserAccount.Id);

            return Json(availableApplicationPageForUserAccount);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="ids">Selected Id's</param>
        /// <returns>True on Success, False on Failure</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.DELETE, SecurityItemCode = PageId.PAGE_USERAPPLICATIONFAVOURITES)]
        public JsonResult Delete(int[] ids)
        {
            IUserApplicationFavouritesBll iUserApplicationFavouritesBll = new UserApplicationFavouritesBll();
            var bActionPass = ids.Aggregate(true, (current, id) => current & iUserApplicationFavouritesBll.DeleteUserApplicationFavourites(id, ApplicationGlobals.UserName));

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Remove));
        }

        /// <summary>
        /// Export to PDF
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "UserApplicationFavouritesForm", MatchFormValue = "Export to PDF")]
        public ActionResult ExportToPdf()
        {
            GenerateReport(ReportFormat.PDF, ApplicationGlobals.Sort("gridUserApplicationFavourites"), ApplicationGlobals.Filter("gridUserApplicationFavourites"));
            return View("Index");
        }

        /// <summary>
        /// Export to Xls
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "UserApplicationFavouritesForm", MatchFormValue = "Export to XLS")]
        public ActionResult ExportToXls()
        {
            GenerateReport(ReportFormat.XLS, ApplicationGlobals.Sort("gridUserApplicationFavourites"), ApplicationGlobals.Filter("gridUserApplicationFavourites"));
            return View("Index");
        }

        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>Index View</returns>
        [IsNewSession]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_USERAPPLICATIONFAVOURITES)]
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
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_USERAPPLICATIONFAVOURITES)]
        public JsonResult Insert(int[] ids)
        {
            IUserApplicationFavouritesBll iUserApplicationFavouritesBll = new UserApplicationFavouritesBll();
            var bActionPass = ids.Aggregate(true, (current, id) => current & iUserApplicationFavouritesBll.InsertUserApplicationFavourites(new UserApplicationFavourites { LnApplicationId = id, LnUserAccountId = ApplicationGlobals.SelectedUserAccount.Id }, ApplicationGlobals.UserName));

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Add));
        }

        /// <summary>
        /// Get UserApplicationFavourites Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>UserApplicationFavourites Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_USERAPPLICATIONFAVOURITES)]
        public ActionResult UserApplicationFavouritesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            IUserApplicationFavouritesBll iUserApplicationFavouritesBll = new UserApplicationFavouritesBll();
            var userApplicationFavouritesPage = iUserApplicationFavouritesBll.GetUserApplicationFavouritesPage(take, skip, sort, filter, ApplicationGlobals.SelectedUserAccount.Id);

            return Content(JsonConvert.SerializeObject(userApplicationFavouritesPage));
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
            Reports<UserApplicationFavourites> reports;
            switch (reportFormat)
            {
                case ReportFormat.PDF:
                    reports = new PdfReport<UserApplicationFavourites>();
                    break;

                case ReportFormat.XLS:
                    reports = new ExcelReport<UserApplicationFavourites>();
                    break;

                default:
                    throw new Exception("Invalid Report Format");
            }

            // Set Report Details
            reports.ReportTitle = ResourceStrings.Text_UserApplicationFavourites;
            reports.UserName = ApplicationGlobals.UserName;

            reports.ReportFilePath = Server.MapPath(ApplicationGlobals.TempDirectory);
            reports.CompanyLogo = Server.MapPath(ApplicationGlobals.LogoDirectory);

            // Set the Output Fields
            reports.OutputColumns = new[] { "ApplicationObj.Code", "ApplicationObj.Description" };
            reports.ColumnHeaders = ApplicationGlobals.VisibleGridColumnHeaders;

            // Set Report Headers
            reports.HeaderList = new[]
            {
                new ReportHeader(string.Empty, DateTime.Now.ToString(ApplicationGlobals.ShortDateTimePattern)),
                new ReportHeader(ApplicationGlobals.ApplicationName, string.Empty),
                new ReportHeader(string.Format(ResourceStrings.Text_Version, ApplicationGlobals.ApplicationVersion), string.Empty),
                new ReportHeader(ApplicationGlobals.GenerateReportHeaderDetails("UserAccount"), string.Empty)
            };

            // Get the Report Data
            IUserApplicationFavouritesBll iUserApplicationFavouritesBll = new UserApplicationFavouritesBll();
            reports.ReportData = iUserApplicationFavouritesBll.GetUserApplicationFavouritesList(sort, filter, ApplicationGlobals.SelectedUserAccount.Id);

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
    }
}