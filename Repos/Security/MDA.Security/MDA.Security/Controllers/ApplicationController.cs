namespace MDA.Security.Controllers
{
    using AutoMapper;
    using BLL;
    using Core.Exception;
    using Core.Filters;
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
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI;
    using ViewModels;

    [SessionTimeout]
    [HandleAndLogError(ExceptionType = typeof(UnauthorizedException), View = "_UnauthorizedAccess", Order = 2)]
    [HandleAndLogError(ExceptionType = typeof(Exception), View = "_Error", Order = 1)]
    public class ApplicationController : Controller
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
                    x.CreateMap<ApplicationViewModel, Application>();
                    x.CreateMap<Application, ApplicationViewModel>();
                });

                return mapperConfiguration.CreateMapper();
            }
        }

        /// <summary>
        /// Get Application Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Application Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_APPLICATION)]
        public ActionResult ApplicationPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            IApplicationBll iApplicationBll = new ApplicationBll();
            var applicationPage = iApplicationBll.GetApplicationPage(take, skip, sort, filter, ApplicationGlobals.UserId);

            return Content(JsonConvert.SerializeObject(applicationPage));
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
            var applicationViewModel = GetViewModel(selectedId, dataOperation);
            return PartialView("MaintainApplication", applicationViewModel);
        }

        /// <summary>
        /// Company
        /// </summary>
        /// <param name="formCollection">Form Collection</param>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "ApplicationForm", MatchFormValue = "Company")]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_COMPANYINAPPLICATION)]
        public RedirectToRouteResult Company(FormCollection formCollection)
        {
            ApplicationGlobals.SelectedApplication = GetViewModel(int.Parse(formCollection["hidGridCheckedRowsApplication"]));
            return RedirectToAction("Index", "CompanyInApplication");
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="ids">Selected Id's</param>
        /// <returns>True on Success, False on Failure</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.DELETE, SecurityItemCode = PageId.PAGE_APPLICATION)]
        public JsonResult Delete(int[] ids)
        {
            IApplicationBll iApplicationBll = new ApplicationBll();
            var bActionPass = ids.Aggregate(true, (current, id) => current & iApplicationBll.DeleteApplication(id, ApplicationGlobals.UserName));

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Delete));
        }

        /// <summary>
        /// Export to PDF
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "ApplicationForm", MatchFormValue = "Export to PDF")]
        public ActionResult ExportToPdf()
        {
            GenerateReport(ReportFormat.PDF, ApplicationGlobals.Sort("gridApplication"), ApplicationGlobals.Filter("gridApplication"));
            return View("Index");
        }

        /// <summary>
        /// Export to Xls
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "ApplicationForm", MatchFormValue = "Export to XLS")]
        public ActionResult ExportToXls()
        {
            GenerateReport(ReportFormat.XLS, ApplicationGlobals.Sort("gridApplication"), ApplicationGlobals.Filter("gridApplication"));
            return View("Index");
        }

        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>Index View</returns>
        [IsNewSession]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_APPLICATION)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Insert
        /// Called when the Submit button is clicked on the Insert Page
        /// </summary>
        /// <param name="applicationViewModel">ApplicationViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_APPLICATION)]
        public JsonResult Insert(ApplicationViewModel applicationViewModel)
        {
            IApplicationBll iApplicationBll = new ApplicationBll();
            var bActionPass = ModelState.IsValid && iApplicationBll.InsertApplication(GetModel(applicationViewModel), ApplicationGlobals.UserName);

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_New_Record_Insert));
        }

        /// <summary>
        /// Check for Code Duplicates
        /// The OutputCacheAttribute attribute is required in order to prevent
        /// ASP.NET MVC from caching the results of the validation method.
        /// </summary>
        /// <param name="applicationViewModel">ApplicationViewModel Object</param>
        /// <returns>True or False</returns>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult IsCodeDuplicate(ApplicationViewModel applicationViewModel)
        {
            IApplicationBll iApplicationBll = new ApplicationBll();
            var application = iApplicationBll.GetApplicationForCode(applicationViewModel.Code.Trim());

            var isCodeDuplicate = (application != null && applicationViewModel.Id == application.Id) || application == null;
            return Json(isCodeDuplicate, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update
        /// Called when the Submit button is clicked on the Edit Page
        /// </summary>
        /// <param name="applicationViewModel">ApplicationViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.UPDATE, SecurityItemCode = PageId.PAGE_APPLICATION)]
        public JsonResult Update(ApplicationViewModel applicationViewModel)
        {
            IApplicationBll iApplicationBll = new ApplicationBll();
            var bActionPass = ModelState.IsValid && iApplicationBll.UpdateApplication(GetModel(applicationViewModel), ApplicationGlobals.UserName);

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Edit_View));
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
            Reports<Application> reports;
            switch (reportFormat)
            {
                case ReportFormat.PDF:
                    reports = new PdfReport<Application>();
                    break;

                case ReportFormat.XLS:
                    reports = new ExcelReport<Application>();
                    break;

                default:
                    throw new Exception("Invalid Report Format");
            }

            // Set Column Widths
            reports.ColumnWidthInPercent = new float[] { 30, 30, 40 };

            // Set Report Details
            reports.ReportTitle = ResourceStrings.Text_Application;
            reports.UserName = ApplicationGlobals.UserName;

            reports.ReportFilePath = Server.MapPath(ApplicationGlobals.TempDirectory);
            reports.CompanyLogo = Server.MapPath(ApplicationGlobals.LogoDirectory);

            // Set the Output Fields
            reports.OutputColumns = new[] { "Code", "Description", "Url" };
            reports.ColumnHeaders = ApplicationGlobals.VisibleGridColumnHeaders;

            // Set Report Headers
            reports.HeaderList = new[]
            {
                new ReportHeader(string.Empty, DateTime.Now.ToString(ApplicationGlobals.ShortDateTimePattern)),
                new ReportHeader(ApplicationGlobals.ApplicationName, string.Empty),
                new ReportHeader(string.Format(ResourceStrings.Text_Version, ApplicationGlobals.ApplicationVersion), string.Empty)
            };

            // Get the Report Data
            IApplicationBll iApplicationBll = new ApplicationBll();
            reports.ReportData = iApplicationBll.GetApplicationList(sort, filter, ApplicationGlobals.UserId);

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
        /// <param name="applicationViewModel">ApplicationViewModel Object</param>
        /// <returns>Business Entity</returns>
        private Application GetModel(ApplicationViewModel applicationViewModel)
        {
            return MapperInstance.Map<Application>(applicationViewModel);
        }

        /// <summary>
        /// Get View Model
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="dataOperation">Data Operation</param>
        /// <returns>Business Entity</returns>
        private ApplicationViewModel GetViewModel(int id, DataOperation dataOperation = 0)
        {
            IApplicationBll iApplicationBll = new ApplicationBll();
            var application = iApplicationBll.GetApplicationForId(id);

            var applicationViewModel = application == null ?
                new ApplicationViewModel() : MapperInstance.Map<ApplicationViewModel>(application);

            applicationViewModel.DataAction = dataOperation;
            return applicationViewModel;
        }
    }
}