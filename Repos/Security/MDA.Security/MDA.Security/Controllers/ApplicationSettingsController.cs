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
    using System.Web.UI;
    using ViewModels;

    [SessionTimeout]
    [HandleAndLogError(ExceptionType = typeof(UnauthorizedException), View = "_UnauthorizedAccess", Order = 2)]
    [HandleAndLogError(ExceptionType = typeof(Exception), View = "_Error", Order = 1)]
    public class ApplicationSettingsController : Controller
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
                    x.CreateMap<ApplicationSettingsViewModel, ApplicationSettings>();
                    x.CreateMap<ApplicationSettings, ApplicationSettingsViewModel>();
                });

                return mapperConfiguration.CreateMapper();
            }
        }

        /// <summary>
        /// Get ApplicationSettings Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>ApplicationSettings Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_APPLICATIONSETTINGS)]
        public ActionResult ApplicationSettingsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            IApplicationSettingsBll iApplicationSettingsBll = new ApplicationSettingsBll();
            var applicationSettingsPage = iApplicationSettingsBll.GetApplicationSettingsPage(take, skip, sort, filter, ApplicationGlobals.SelectedCompanyInApplication.Id);

            return Content(JsonConvert.SerializeObject(applicationSettingsPage));
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
            var applicationSettingsViewModel = GetViewModel(selectedId, dataOperation);
            return PartialView("MaintainApplicationSettings", applicationSettingsViewModel);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="ids">Selected Id's</param>
        /// <returns>True on Success, False on Failure</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.DELETE, SecurityItemCode = PageId.PAGE_APPLICATIONSETTINGS)]
        public JsonResult Delete(int[] ids)
        {
            IApplicationSettingsBll iApplicationSettingsBll = new ApplicationSettingsBll();
            var bActionPass = ids.Aggregate(true, (current, id) => current & iApplicationSettingsBll.DeleteApplicationSettings(id, ApplicationGlobals.UserName));

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Delete));
        }

        /// <summary>
        /// Export to PDF
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "ApplicationSettingsForm", MatchFormValue = "Export to PDF")]
        public ActionResult ExportToPdf()
        {
            GenerateReport(ReportFormat.PDF, ApplicationGlobals.Sort("gridApplicationSettings"), ApplicationGlobals.Filter("gridApplicationSettings"));
            return View("Index");
        }

        /// <summary>
        /// Export to Xls
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "ApplicationSettingsForm", MatchFormValue = "Export to XLS")]
        public ActionResult ExportToXls()
        {
            GenerateReport(ReportFormat.XLS, ApplicationGlobals.Sort("gridApplicationSettings"), ApplicationGlobals.Filter("gridApplicationSettings"));
            return View("Index");
        }

        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>Index View</returns>
        [IsNewSession]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_APPLICATIONSETTINGS)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Insert
        /// Called when the Submit button is clicked on the Insert Page
        /// </summary>
        /// <param name="applicationSettingsViewModel">ApplicationSettingsViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_APPLICATIONSETTINGS)]
        public JsonResult Insert(ApplicationSettingsViewModel applicationSettingsViewModel)
        {
            IApplicationSettingsBll iApplicationSettingsBll = new ApplicationSettingsBll();
            var bActionPass = ModelState.IsValid && iApplicationSettingsBll.InsertApplicationSettings(GetModel(applicationSettingsViewModel), ApplicationGlobals.UserName);

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_New_Record_Insert));
        }

        /// <summary>
        /// Check for Key Name Duplicates
        /// The OutputCacheAttribute attribute is required in order to prevent
        /// ASP.NET MVC from caching the results of the validation method.
        /// </summary>
        /// <param name="applicationSettingsViewModel">ApplicationSettingsViewModel Object</param>
        /// <returns>True or False</returns>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult IsKeyNameDuplicate(ApplicationSettingsViewModel applicationSettingsViewModel)
        {
            IApplicationSettingsBll iApplicationSettingsBll = new ApplicationSettingsBll();
            var applicationSettings = iApplicationSettingsBll.GetApplicationSettingsForKeyNameAndCompanyInApplicationId(applicationSettingsViewModel.KeyName.Trim(), ApplicationGlobals.SelectedCompanyInApplication.Id);

            var isKeyNameDuplicate = (applicationSettings != null && applicationSettingsViewModel.Id == applicationSettings.Id) || applicationSettings == null;
            return Json(isKeyNameDuplicate, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update
        /// Called when the Submit button is clicked on the Edit Page
        /// </summary>
        /// <param name="applicationSettingsViewModel">ApplicationSettingsViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.UPDATE, SecurityItemCode = PageId.PAGE_APPLICATIONSETTINGS)]
        public JsonResult Update(ApplicationSettingsViewModel applicationSettingsViewModel)
        {
            IApplicationSettingsBll iApplicationSettingsBll = new ApplicationSettingsBll();
            var bActionPass = ModelState.IsValid && iApplicationSettingsBll.UpdateApplicationSettings(GetModel(applicationSettingsViewModel), ApplicationGlobals.UserName);

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
            Reports<ApplicationSettings> reports;
            switch (reportFormat)
            {
                case ReportFormat.PDF:
                    reports = new PdfReport<ApplicationSettings>();
                    break;

                case ReportFormat.XLS:
                    reports = new ExcelReport<ApplicationSettings>();
                    break;

                default:
                    throw new Exception("Invalid Report Format");
            }

            // Set Report Details
            reports.ReportTitle = ResourceStrings.Text_ApplicationSettings;
            reports.UserName = ApplicationGlobals.UserName;

            reports.ReportFilePath = Server.MapPath(ApplicationGlobals.TempDirectory);
            reports.CompanyLogo = Server.MapPath(ApplicationGlobals.LogoDirectory);

            // Set the Output Fields
            reports.OutputColumns = new[] { "KeyName", "Separator", "Value" };
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
            IApplicationSettingsBll iApplicationSettingsBll = new ApplicationSettingsBll();
            reports.ReportData = iApplicationSettingsBll.GetApplicationSettingsList(sort, filter, ApplicationGlobals.SelectedCompanyInApplication.Id);

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
        /// <param name="applicationSettingsViewModel">ApplicationSettingsViewModel Object</param>
        /// <returns>Business Entity</returns>
        private ApplicationSettings GetModel(ApplicationSettingsViewModel applicationSettingsViewModel)
        {
            return MapperInstance.Map<ApplicationSettings>(applicationSettingsViewModel);
        }

        /// <summary>
        /// Get View Model
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="dataOperation">Data Operation</param>
        /// <returns>Business Entity</returns>
        private ApplicationSettingsViewModel GetViewModel(int id, DataOperation dataOperation = 0)
        {
            IApplicationSettingsBll iApplicationSettingsBll = new ApplicationSettingsBll();
            var applicationSettings = iApplicationSettingsBll.GetApplicationSettingsForId(id);

            var applicationSettingsViewModel = applicationSettings == null ?
                new ApplicationSettingsViewModel() : MapperInstance.Map<ApplicationSettingsViewModel>(applicationSettings);

            applicationSettingsViewModel.LnCompanyInApplicationId = ApplicationGlobals.SelectedCompanyInApplication.Id;

            applicationSettingsViewModel.DataAction = dataOperation;
            return applicationSettingsViewModel;
        }
    }
}