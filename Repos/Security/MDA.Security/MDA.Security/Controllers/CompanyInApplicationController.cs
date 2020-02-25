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
    public class CompanyInApplicationController : Controller
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
                    x.CreateMap<CompanyInApplication, CompanyInApplicationViewModel>();
                });

                return mapperConfiguration.CreateMapper();
            }
        }

        /// <summary>
        /// Application Settings
        /// </summary>
        /// <param name="formCollection">Form Collection</param>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "CompanyInApplicationForm", MatchFormValue = "Application Settings")]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_APPLICATIONSETTINGS)]
        public RedirectToRouteResult ApplicationSettings(FormCollection formCollection)
        {
            ApplicationGlobals.SelectedCompanyInApplication = GetViewModel(int.Parse(formCollection["hidGridCheckedRowsCompanyInApplication"]));
            return RedirectToAction("Index", "ApplicationSettings");
        }

        /// <summary>
        /// Get Available Company Page For Application
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Available Company Page For Application</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_COMPANYINAPPLICATION)]
        public ActionResult AvailableCompanyPageForApplication(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            ICompanyBll iCompanyBll = new CompanyBll();
            var availableCompanyPageForApplication = iCompanyBll.GetAvailableCompanyPageForApplication(take, skip, sort, filter, ApplicationGlobals.SelectedApplication.Id);

            return Content(JsonConvert.SerializeObject(availableCompanyPageForApplication));
        }

        /// <summary>
        /// Get CompanyInApplication Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>CompanyInApplication Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_COMPANYINAPPLICATION)]
        public ActionResult CompanyInApplicationPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            ICompanyInApplicationBll iCompanyInApplicationBll = new CompanyInApplicationBll();
            var companyInApplicationPage = iCompanyInApplicationBll.GetCompanyInApplicationPage(take, skip, sort, filter, ApplicationGlobals.SelectedApplication.Id);

            return Content(JsonConvert.SerializeObject(companyInApplicationPage));
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="ids">Selected Id's</param>
        /// <returns>True on Success, False on Failure</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.DELETE, SecurityItemCode = PageId.PAGE_COMPANYINAPPLICATION)]
        public JsonResult Delete(int[] ids)
        {
            ICompanyInApplicationBll iCompanyInApplicationBll = new CompanyInApplicationBll();
            var bActionPass = ids.Aggregate(true, (current, id) => current & iCompanyInApplicationBll.DeleteCompanyInApplication(id, ApplicationGlobals.UserName));

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Remove));
        }

        /// <summary>
        /// Export to PDF
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "CompanyInApplicationForm", MatchFormValue = "Export to PDF")]
        public ActionResult ExportToPdf()
        {
            GenerateReport(ReportFormat.PDF, ApplicationGlobals.Sort("gridCompanyInApplication"), ApplicationGlobals.Filter("gridCompanyInApplication"));
            return View("Index");
        }

        /// <summary>
        /// Export to Xls
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "CompanyInApplicationForm", MatchFormValue = "Export to XLS")]
        public ActionResult ExportToXls()
        {
            GenerateReport(ReportFormat.XLS, ApplicationGlobals.Sort("gridCompanyInApplication"), ApplicationGlobals.Filter("gridCompanyInApplication"));
            return View("Index");
        }

        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>Index View</returns>
        [IsNewSession]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_COMPANYINAPPLICATION)]
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
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_COMPANYINAPPLICATION)]
        public JsonResult Insert(int[] ids)
        {
            ICompanyInApplicationBll iCompanyInApplicationBll = new CompanyInApplicationBll();
            var bActionPass = ids.Aggregate(true, (current, id) => current & iCompanyInApplicationBll.InsertCompanyInApplication(new CompanyInApplication { LnCompanyId = id, LnApplicationId = ApplicationGlobals.SelectedApplication.Id }, ApplicationGlobals.UserName));

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Add));
        }

        /// <summary>
        /// Roles
        /// </summary>
        /// <param name="formCollection">Form Collection</param>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "CompanyInApplicationForm", MatchFormValue = "Roles")]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_SECURITYROLES)]
        public RedirectToRouteResult Roles(FormCollection formCollection)
        {
            ApplicationGlobals.SelectedCompanyInApplication = GetViewModel(int.Parse(formCollection["hidGridCheckedRowsCompanyInApplication"]));
            return RedirectToAction("Index", "SecurityRoles");
        }

        /// <summary>
        /// Security Items
        /// </summary>
        /// <param name="formCollection">Form Collection</param>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "CompanyInApplicationForm", MatchFormValue = "Security Items")]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_SECURITYITEM)]
        public RedirectToRouteResult SecurityItems(FormCollection formCollection)
        {
            ApplicationGlobals.SelectedCompanyInApplication = GetViewModel(int.Parse(formCollection["hidGridCheckedRowsCompanyInApplication"]));
            return RedirectToAction("Index", "SecurityItem");
        }

        /// <summary>
        /// Users
        /// </summary>
        /// <param name="formCollection">Form Collection</param>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "CompanyInApplicationForm", MatchFormValue = "Users")]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_USERINCOMPANYINAPPLICATION)]
        public RedirectToRouteResult Users(FormCollection formCollection)
        {
            ApplicationGlobals.SelectedCompanyInApplication = GetViewModel(int.Parse(formCollection["hidGridCheckedRowsCompanyInApplication"]));
            return RedirectToAction("Index", "UserInCompanyInApplication");
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
            Reports<CompanyInApplication> reports;
            switch (reportFormat)
            {
                case ReportFormat.PDF:
                    reports = new PdfReport<CompanyInApplication>();
                    break;

                case ReportFormat.XLS:
                    reports = new ExcelReport<CompanyInApplication>();
                    break;

                default:
                    throw new Exception("Invalid Report Format");
            }

            // Set Report Details
            reports.ReportTitle = ResourceStrings.Text_CompanyInApplication;
            reports.UserName = ApplicationGlobals.UserName;

            reports.ReportFilePath = Server.MapPath(ApplicationGlobals.TempDirectory);
            reports.CompanyLogo = Server.MapPath(ApplicationGlobals.LogoDirectory);

            // Set the Output Fields
            reports.OutputColumns = new[] { "CompanyObj.Code", "CompanyObj.Description" };
            reports.ColumnHeaders = ApplicationGlobals.VisibleGridColumnHeaders;

            // Set Report Headers
            reports.HeaderList = new[]
            {
                new ReportHeader(string.Empty, DateTime.Now.ToString(ApplicationGlobals.ShortDateTimePattern)),
                new ReportHeader(ApplicationGlobals.ApplicationName, string.Empty),
                new ReportHeader(string.Format(ResourceStrings.Text_Version, ApplicationGlobals.ApplicationVersion), string.Empty),
                new ReportHeader(ApplicationGlobals.GenerateReportHeaderDetails("Application"), string.Empty)
            };

            // Get the Report Data
            ICompanyInApplicationBll iCompanyInApplicationBll = new CompanyInApplicationBll();
            reports.ReportData = iCompanyInApplicationBll.GetCompanyInApplicationList(sort, filter, ApplicationGlobals.SelectedApplication.Id);

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
        /// <param name="dataOperation">Data Operation</param>
        /// <returns>Business Entity</returns>
        private CompanyInApplicationViewModel GetViewModel(int id, DataOperation dataOperation = 0)
        {
            ICompanyInApplicationBll iCompanyInApplicationBll = new CompanyInApplicationBll();
            var companyInApplication = iCompanyInApplicationBll.GetCompanyInApplicationForId(id);

            var companyInApplicationViewModel = companyInApplication == null ?
                new CompanyInApplicationViewModel() : MapperInstance.Map<CompanyInApplicationViewModel>(companyInApplication);

            return companyInApplicationViewModel;
        }
    }
}