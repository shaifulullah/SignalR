namespace MDA.Security.Controllers
{
    using BLL;
    using Core.Exception;
    using Core.Filters;
    using Filters;
    using Globals;
    using Helpers;
    using IBLL;
    using Models;
    using Reports;
    using Resources;
    using System;
    using System.Web.Mvc;
    using ViewModels;

    [SessionTimeout]
    [HandleAndLogError(ExceptionType = typeof(UnauthorizedException), View = "_UnauthorizedAccess", Order = 2)]
    [HandleAndLogError(ExceptionType = typeof(Exception), View = "_Error", Order = 1)]
    public class SecurityReportController : Controller
    {
        /// <summary>
        /// Export to Pdf
        /// </summary>
        /// <param name="securityReportViewModel">SecurityReportViewModel Object</param>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "SecurityReportForm", MatchFormValue = "Export to PDF")]
        [AuthorizeUser(Operation = DataOperation.EXECUTE, SecurityItemCode = PageId.PAGE_SECURITYREPORT)]
        public ActionResult ExportToPdf(SecurityReportViewModel securityReportViewModel)
        {
            GenerateReport(ReportFormat.PDF, securityReportViewModel);
            return View(GetViewModel());
        }

        /// <summary>
        /// Export to Xls
        /// </summary>
        /// <param name="securityReportViewModel">SecurityReportViewModel Object</param>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "SecurityReportForm", MatchFormValue = "Export to XLS")]
        [AuthorizeUser(Operation = DataOperation.EXECUTE, SecurityItemCode = PageId.PAGE_SECURITYREPORT)]
        public ActionResult ExportToXls(SecurityReportViewModel securityReportViewModel)
        {
            GenerateReport(ReportFormat.XLS, securityReportViewModel);
            return View(GetViewModel());
        }

        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>Index View</returns>
        [IsNewSession]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_SECURITYREPORT)]
        public ActionResult Index()
        {
            return View(GetViewModel());
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
        /// <param name="securityReportViewModel">SecurityReportViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        private bool GenerateReport(ReportFormat reportFormat, SecurityReportViewModel securityReportViewModel)
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
            reports.ColumnWidthInPercent = new float[] { 8, 10, 15, 15, 18, 10, 12, 12 };

            // Set Report Details
            reports.ReportTitle = ResourceStrings.Text_Security_Report;
            reports.UserName = ApplicationGlobals.UserName;

            reports.ReportFilePath = Server.MapPath(ApplicationGlobals.TempDirectory);
            reports.CompanyLogo = Server.MapPath(ApplicationGlobals.LogoDirectory);

            // Set Report Orientation
            reports.ReportOrientation = PageOrientation.LANDSCAPE;

            // Set the Output Fields
            reports.OutputColumns = new[] { "Code", "UserName", "FullName", "Domain", "eMail", "Company", "SecurityRoles", "UserSecurityRights" };

            reports.ColumnHeaders = new[] { ResourceStrings.Display_Name_SecurityReport_Code, ResourceStrings.Display_Name_SecurityReport_UserName,
                ResourceStrings.Display_Name_SecurityReport_FullName, ResourceStrings.Display_Name_SecurityReport_Domain,
                ResourceStrings.Display_Name_SecurityReport_eMail, ResourceStrings.Display_Name_SecurityReport_Company,
                ResourceStrings.Display_Name_SecurityReport_SecurityRoles, ResourceStrings.Display_Name_SecurityReport_UserSecurityRights };

            // Set Report Headers
            reports.HeaderList = new[]
            {
                new ReportHeader(string.Empty, DateTime.Now.ToString(ApplicationGlobals.ShortDateTimePattern)),
                new ReportHeader(ApplicationGlobals.ApplicationName, string.Empty),
                new ReportHeader(string.Format(ResourceStrings.Text_Version, ApplicationGlobals.ApplicationVersion), string.Empty)
            };

            // Get the Report Data
            IUserAccountDetailsBll iUserAccountDetailsBll = new UserAccountDetailsBll();
            reports.ReportData = iUserAccountDetailsBll.GetUserAccountDetailsListForApplicationIdAndCompanyId(securityReportViewModel.LnApplicationId, securityReportViewModel.LnCompanyId, ApplicationGlobals.IsHideTerminated);

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
        /// <returns>Business Entity</returns>
        private SecurityReportViewModel GetViewModel()
        {
            var securityReportViewModel = new SecurityReportViewModel();

            IApplicationBll iApplicationBll = new ApplicationBll();
            securityReportViewModel.ApplicationList = iApplicationBll.GetApplicationDropDownList(ApplicationGlobals.UserId);

            ICompanyBll iCompanyBll = new CompanyBll();
            securityReportViewModel.CompanyList = iCompanyBll.GetCompanyDropDownList();

            return securityReportViewModel;
        }
    }
}