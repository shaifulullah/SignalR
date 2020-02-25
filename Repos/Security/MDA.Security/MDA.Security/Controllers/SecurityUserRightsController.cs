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
    public class SecurityUserRightsController : Controller
    {
        /// <summary>
        /// Get Available SecurityItem Page For User In Company In Application
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Available SecurityItem Page For User In Company In Application</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_SECURITYUSERRIGHTS)]
        public ActionResult AvailableSecurityItemPageForUserInCompanyInApplication(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            ISecurityItemBll iSecurityItemBll = new SecurityItemBll();
            var availableSecurityItemPageForUserInCompanyInApplicationId = iSecurityItemBll.GetAvailableSecurityItemPageForUserInCompanyInApplicationId(take, skip, sort, filter, ApplicationGlobals.SelectedUserInCompanyInApplication.Id, ApplicationGlobals.SelectedCompanyInApplication.Id);

            return Content(JsonConvert.SerializeObject(availableSecurityItemPageForUserInCompanyInApplicationId));
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="ids">Selected Id's</param>
        /// <returns>True on Success, False on Failure</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.DELETE, SecurityItemCode = PageId.PAGE_SECURITYUSERRIGHTS)]
        public JsonResult Delete(int[] ids)
        {
            ISecurityUserRightsBll iSecurityUserRightsBll = new SecurityUserRightsBll();
            var bActionPass = ids.Aggregate(true, (current, id) => current & iSecurityUserRightsBll.DeleteSecurityUserRights(id, ApplicationGlobals.UserName));

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Remove));
        }

        /// <summary>
        /// Export to PDF
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "SecurityUserRightsForm", MatchFormValue = "Export to PDF")]
        public ActionResult ExportToPdf()
        {
            GenerateReport(ReportFormat.PDF, ApplicationGlobals.Sort("gridSecurityUserRights"), ApplicationGlobals.Filter("gridSecurityUserRights"));
            return View("Index");
        }

        /// <summary>
        /// Export to Xls
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "SecurityUserRightsForm", MatchFormValue = "Export to XLS")]
        public ActionResult ExportToXls()
        {
            GenerateReport(ReportFormat.XLS, ApplicationGlobals.Sort("gridSecurityUserRights"), ApplicationGlobals.Filter("gridSecurityUserRights"));
            return View("Index");
        }

        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>Index View</returns>
        [IsNewSession]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_SECURITYUSERRIGHTS)]
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
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_SECURITYUSERRIGHTS)]
        public JsonResult Insert(int[] ids)
        {
            ISecurityUserRightsBll iSecurityUserRightsBll = new SecurityUserRightsBll();
            var bActionPass = ids.Aggregate(true, (current, id) => current & iSecurityUserRightsBll.InsertSecurityUserRights(new SecurityUserRights { LnSecurityItemId = id, LnUserInCompanyInApplicationId = ApplicationGlobals.SelectedUserInCompanyInApplication.Id, LnSecurityLevelId = 1 }, ApplicationGlobals.UserName));

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Add));
        }

        /// <summary>
        /// Get SecurityUserRights Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>SecurityUserRights Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_SECURITYUSERRIGHTS)]
        public ActionResult SecurityUserRightsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            ISecurityUserRightsBll iSecurityUserRightsBll = new SecurityUserRightsBll();
            var securityUserRightsPage = iSecurityUserRightsBll.GetSecurityUserRightsPage(take, skip, sort, filter, ApplicationGlobals.SelectedUserInCompanyInApplication.Id);

            return Content(JsonConvert.SerializeObject(securityUserRightsPage));
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="Id">Id</param>
        /// <param name="SecurityItemCode">Security Item Code</param>
        /// <param name="SecurityTypeDescription">Security Type</param>
        /// <param name="Rights">Security Level Code</param>
        /// <param name="CanCreate">Can Create</param>
        /// <param name="CanRead">Can Read</param>
        /// <param name="CanUpdate">Can Update</param>
        /// <param name="CanDelete">Can Delete</param>
        /// <param name="CanExecute">Can Execute</param>
        /// <param name="CanAccess">Can Access</param>
        [AuthorizeUser(Operation = DataOperation.UPDATE, SecurityItemCode = PageId.PAGE_SECURITYUSERRIGHTS)]
        public void Update(int Id, string SecurityItemCode, string SecurityTypeDescription, string Rights, bool CanCreate, bool CanRead, bool CanUpdate, bool CanDelete, bool CanExecute, bool CanAccess)
        {
            Rights = string.Format("R-{0}{1}{2}{3}{4}{5}", CanCreate ? "C" : "_", CanRead ? "R" : "_", CanUpdate ? "U" : "_", CanDelete ? "D" : "_",
                CanExecute ? "E" : "_", CanAccess ? "A" : "_");

            IConfigurationBll<SecurityLevel> iSecurityLevelBll = new ConfigurationBll<SecurityLevel>();
            var securityLevel = iSecurityLevelBll.GetConfigurationForCode(Rights);

            ISecurityUserRightsBll iSecurityUserRightsBll = new SecurityUserRightsBll();
            var securityUserRights = iSecurityUserRightsBll.GetSecurityUserRightsForId(Id);

            securityUserRights.LnSecurityLevelId = securityLevel.Id;
            iSecurityUserRightsBll.UpdateSecurityUserRights(securityUserRights, ApplicationGlobals.UserName);
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
            Reports<SecurityRightsForUserAccount> reports;
            switch (reportFormat)
            {
                case ReportFormat.PDF:
                    reports = new PdfReport<SecurityRightsForUserAccount>();
                    break;

                case ReportFormat.XLS:
                    reports = new ExcelReport<SecurityRightsForUserAccount>();
                    break;

                default:
                    throw new Exception("Invalid Report Format");
            }

            // Set Column Widths
            reports.ColumnWidthInPercent = new float[] { 35, 17, 8, 8, 8, 8, 8, 8 };

            // Set Report Details
            reports.ReportTitle = ResourceStrings.Text_SecurityUserRights;
            reports.UserName = ApplicationGlobals.UserName;

            reports.ReportFilePath = Server.MapPath(ApplicationGlobals.TempDirectory);
            reports.CompanyLogo = Server.MapPath(ApplicationGlobals.LogoDirectory);

            // Set the Output Fields
            reports.OutputColumns = new[] { "SecurityItemCode", "SecurityTypeDescription", "CreateDisplay", "ReadDisplay", "UpdateDisplay", "DeleteDisplay", "ExecuteDisplay", "AccessDisplay" };
            reports.ColumnHeaders = ApplicationGlobals.VisibleGridColumnHeaders;

            // Set Report Headers
            reports.HeaderList = new[]
            {
                new ReportHeader(string.Empty, DateTime.Now.ToString(ApplicationGlobals.ShortDateTimePattern)),
                new ReportHeader(ApplicationGlobals.ApplicationName, string.Empty),
                new ReportHeader(string.Format(ResourceStrings.Text_Version, ApplicationGlobals.ApplicationVersion), string.Empty),
                new ReportHeader(ApplicationGlobals.GenerateReportHeaderDetails("Application"), string.Empty),
                new ReportHeader(ApplicationGlobals.GenerateReportHeaderDetails("CompanyInApplication"), string.Empty),
                new ReportHeader(ApplicationGlobals.GenerateReportHeaderDetails("UserInCompanyInApplication"), string.Empty)
            };

            // Get the Report Data
            ISecurityUserRightsBll iSecurityUserRightsBll = new SecurityUserRightsBll();
            reports.ReportData = iSecurityUserRightsBll.GetSecurityUserRightsList(sort, filter, ApplicationGlobals.SelectedUserInCompanyInApplication.Id);

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