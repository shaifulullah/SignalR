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
    public class UserInCompanyInApplicationSecurityRolesController : Controller
    {
        /// <summary>
        /// Get Available SecurityRoles
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Available SecurityRoles</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_SECURITYUSERINROLES)]
        public ActionResult AvailableSecurityRolesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            ISecurityRolesBll iSecurityRolesBll = new SecurityRolesBll();
            var availableSecurityRolesForUserInCompanyInApplicationId = iSecurityRolesBll.GetAvailableSecurityRolesPageForUserInCompanyInApplicationId(take, skip, sort, filter, ApplicationGlobals.SelectedUserInCompanyInApplication.Id);

            return Content(JsonConvert.SerializeObject(availableSecurityRolesForUserInCompanyInApplicationId));
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="ids">Selected Id's</param>
        /// <returns>True on Success, False on Failure</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.DELETE, SecurityItemCode = PageId.PAGE_SECURITYUSERINROLES)]
        public JsonResult Delete(int[] ids)
        {
            ISecurityUserInRolesBll iSecurityUserInRolesBll = new SecurityUserInRolesBll();
            var bActionPass = ids.Aggregate(true, (current, id) => current & iSecurityUserInRolesBll.DeleteSecurityUserInRoles(id, ApplicationGlobals.UserName));

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Remove));
        }

        /// <summary>
        /// Export to PDF
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "UserInCompanyInApplicationSecurityRolesForm", MatchFormValue = "Export to PDF")]
        public ActionResult ExportToPdf()
        {
            GenerateReport(ReportFormat.PDF, ApplicationGlobals.Sort("gridUserInCompanyInApplicationSecurityRoles"), ApplicationGlobals.Filter("gridUserInCompanyInApplicationSecurityRoles"));
            return View("Index");
        }

        /// <summary>
        /// Export to Xls
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "UserInCompanyInApplicationSecurityRolesForm", MatchFormValue = "Export to XLS")]
        public ActionResult ExportToXls()
        {
            GenerateReport(ReportFormat.XLS, ApplicationGlobals.Sort("gridUserInCompanyInApplicationSecurityRoles"), ApplicationGlobals.Filter("gridUserInCompanyInApplicationSecurityRoles"));
            return View("Index");
        }

        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>Index View</returns>
        [IsNewSession]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_SECURITYUSERINROLES)]
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
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_SECURITYUSERINROLES)]
        public JsonResult Insert(int[] ids)
        {
            ISecurityUserInRolesBll iSecurityUserInRolesBll = new SecurityUserInRolesBll();
            var bActionPass = ids.Aggregate(true, (current, id) => current & iSecurityUserInRolesBll.InsertSecurityUserInRoles(new SecurityUserInRoles { LnUserInCompanyInApplicationId = ApplicationGlobals.SelectedUserInCompanyInApplication.Id, LnSecurityRolesId = id }, ApplicationGlobals.UserName));

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Add));
        }

        /// <summary>
        /// Get UserInCompanyInApplicationSecurityRoles Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>UserInCompanyInApplicationSecurityRoles Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_SECURITYUSERINROLES)]
        public ActionResult UserInCompanyInApplicationSecurityRolesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            ISecurityUserInRolesBll iSecurityUserInRolesBll = new SecurityUserInRolesBll();
            var userInCompanyInApplicationSecurityRolesPage = iSecurityUserInRolesBll.GetSecurityUserInRolesPageForUserInCompanyInApplicationId(take, skip, sort, filter, ApplicationGlobals.SelectedUserInCompanyInApplication.Id);

            return Content(JsonConvert.SerializeObject(userInCompanyInApplicationSecurityRolesPage));
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
            Reports<SecurityRolesDetails> reports;
            switch (reportFormat)
            {
                case ReportFormat.PDF:
                    reports = new PdfReport<SecurityRolesDetails>();
                    break;

                case ReportFormat.XLS:
                    reports = new ExcelReport<SecurityRolesDetails>();
                    break;

                default:
                    throw new Exception("Invalid Report Format");
            }

            // Set Column Widths
            reports.ColumnWidthInPercent = new float[] { 20, 30, 30, 20 };

            // Set Report Details
            reports.ReportTitle = ResourceStrings.Text_UserInCompanyInApplicationSecurityRoles;
            reports.UserName = ApplicationGlobals.UserName;

            reports.ReportFilePath = Server.MapPath(ApplicationGlobals.TempDirectory);
            reports.CompanyLogo = Server.MapPath(ApplicationGlobals.LogoDirectory);

            // Set the Output Fields
            reports.OutputColumns = new[] { "Code", "Description", "LnActiveDirectoryGroupName", "LnSkillCode" };
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
            ISecurityUserInRolesBll iSecurityUserInRolesBll = new SecurityUserInRolesBll();
            reports.ReportData = iSecurityUserInRolesBll.GetSecurityUserInRolesListForUserInCompanyInApplicationId(sort, filter, ApplicationGlobals.SelectedUserInCompanyInApplication.Id);

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