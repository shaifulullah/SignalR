namespace MDA.Security.Controllers
{
    using AutoMapper;
    using BLL;
    using Core.Exception;
    using Core.Filters;
    using Filters;
    using Globals;
    using Helpers;
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
    public class ConfigurationController : Controller
    {
        /// <summary>
        /// Gets Configuration Type Name
        /// </summary>
        public string ConfigurationTypeName
        {
            get
            {
                var resourceManager = new System.Resources.ResourceManager("MDA.Security.Resources.ResourceStrings", typeof(ResourceStrings).Assembly);
                return resourceManager.GetString(string.Format("Text_{0}", ApplicationGlobals.ConfigurationType));
            }
        }

        /// <summary>
        /// Gets Mapper Instance
        /// </summary>
        public IMapper MapperInstance
        {
            get
            {
                var mapperConfiguration = new MapperConfiguration(x =>
                {
                    x.CreateMap<ConfigurationViewModel, ApplicationComponent>();
                    x.CreateMap<ConfigurationViewModel, LogAction>();
                    x.CreateMap<ConfigurationViewModel, SecurityLevel>();

                    x.CreateMap<ApplicationComponent, ConfigurationViewModel>();
                    x.CreateMap<LogAction, ConfigurationViewModel>();
                    x.CreateMap<SecurityLevel, ConfigurationViewModel>();
                });

                return mapperConfiguration.CreateMapper();
            }
        }

        /// <summary>
        /// Get Configuration BLL
        /// </summary>
        /// <returns>Configuration BLL Object</returns>
        protected dynamic ConfigurationBll
        {
            get
            {
                switch (ApplicationGlobals.ConfigurationType.ToUpper())
                {
                    case Configuration.APPLICATION_COMPONENT:
                        return new ConfigurationBll<ApplicationComponent>();

                    case Configuration.LOG_ACTION:
                        return new ConfigurationBll<LogAction>();

                    case Configuration.SECURITY_LEVEL:
                        return new ConfigurationBll<SecurityLevel>();
                }

                return null;
            }
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
            var configurationViewModel = GetViewModel(selectedId, dataOperation);
            return PartialView("MaintainConfiguration", configurationViewModel);
        }

        /// <summary>
        /// Get Configuration Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Configuration Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_CONFIGURATION)]
        public ActionResult ConfigurationPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            var configurationPage = ConfigurationBll.GetConfigurationPage(take, skip, sort, filter);
            return Content(JsonConvert.SerializeObject(configurationPage));
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="ids">Selected Id's</param>
        /// <returns>True on Success, False on Failure</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.DELETE, SecurityItemCode = PageId.PAGE_CONFIGURATION)]
        public JsonResult Delete(int[] ids)
        {
            var bActionPass = ids.Aggregate(true, (current, id) => current & ConfigurationBll.DeleteConfiguration(id, ApplicationGlobals.UserName));
            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Delete));
        }

        /// <summary>
        /// Export to PDF
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "ConfigurationForm", MatchFormValue = "Export to PDF")]
        public ActionResult ExportToPdf()
        {
            GenerateReport(ReportFormat.PDF, ApplicationGlobals.Sort("gridConfiguration"), ApplicationGlobals.Filter("gridConfiguration"));
            return View("Index");
        }

        /// <summary>
        /// Export to Xls
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "ConfigurationForm", MatchFormValue = "Export to XLS")]
        public ActionResult ExportToXls()
        {
            GenerateReport(ReportFormat.XLS, ApplicationGlobals.Sort("gridConfiguration"), ApplicationGlobals.Filter("gridConfiguration"));
            return View("Index");
        }

        /// <summary>
        /// Index Action
        /// </summary>
        /// <param name="typeName">Configuration Type Name</param>
        /// <returns>Index View</returns>
        [IsNewSession]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_CONFIGURATION)]
        public ActionResult Index(string typeName)
        {
            ApplicationGlobals.ConfigurationType = typeName;
            return View();
        }

        /// <summary>
        /// Insert
        /// Called when the Submit button is clicked on the Insert Page
        /// </summary>
        /// <param name="configurationViewModel">ConfigurationViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_CONFIGURATION)]
        public JsonResult Insert(ConfigurationViewModel configurationViewModel)
        {
            var bActionPass = ModelState.IsValid && ConfigurationBll.InsertConfiguration(GetConfigurationData(configurationViewModel), ApplicationGlobals.UserName);
            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_New_Record_Insert));
        }

        /// <summary>
        /// Check for Code Duplicates The OutputCacheAttribute attribute is required in order to
        /// prevent ASP.NET MVC from caching the results of the validation method.
        /// </summary>
        /// <param name="configurationViewModel">ConfigurationViewModel Object</param>
        /// <returns>True or False</returns>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult IsCodeDuplicate(ConfigurationViewModel configurationViewModel)
        {
            var configuration = ConfigurationBll.GetConfigurationForCode(configurationViewModel.Code.Trim());

            var isCodeDuplicate = (configuration != null && configurationViewModel.Id == configuration.Id) || configuration == null;
            return Json(isCodeDuplicate, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update
        /// Called when the Submit button is clicked on the Edit Page
        /// </summary>
        /// <param name="configurationViewModel">ConfigurationViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.UPDATE, SecurityItemCode = PageId.PAGE_CONFIGURATION)]
        public JsonResult Update(ConfigurationViewModel configurationViewModel)
        {
            var bActionPass = ModelState.IsValid && ConfigurationBll.UpdateConfiguration(GetConfigurationData(configurationViewModel), ApplicationGlobals.UserName);
            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Edit_View));
        }

        /// <summary>
        /// Get Configuration Data
        /// </summary>
        /// <param name="configurationViewModel">ConfigurationViewModel Object</param>
        /// <returns>Configuration Data Object</returns>
        protected dynamic GetConfigurationData(ConfigurationViewModel configurationViewModel)
        {
            switch (ApplicationGlobals.ConfigurationType.ToUpper())
            {
                case Configuration.APPLICATION_COMPONENT:
                    return MapperInstance.Map<ApplicationComponent>(configurationViewModel);

                case Configuration.LOG_ACTION:
                    return MapperInstance.Map<LogAction>(configurationViewModel);

                case Configuration.SECURITY_LEVEL:
                    return MapperInstance.Map<SecurityLevel>(configurationViewModel);
            }

            return null;
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
            Reports<ConfigurationViewModel> reports;
            switch (reportFormat)
            {
                case ReportFormat.PDF:
                    reports = new PdfReport<ConfigurationViewModel>();
                    break;

                case ReportFormat.XLS:
                    reports = new ExcelReport<ConfigurationViewModel>();
                    break;

                default:
                    throw new Exception("Invalid Report Format");
            }

            // Set Report Details
            reports.ReportTitle = ConfigurationTypeName;
            reports.UserName = ApplicationGlobals.UserName;

            reports.ReportFilePath = Server.MapPath(ApplicationGlobals.TempDirectory);
            reports.CompanyLogo = Server.MapPath(ApplicationGlobals.LogoDirectory);

            // Set the Output Fields
            reports.OutputColumns = new[] { "Code", "Description" };
            reports.ColumnHeaders = ApplicationGlobals.VisibleGridColumnHeaders;

            // Set Report Headers
            reports.HeaderList = new[]
            {
                new ReportHeader(string.Empty, DateTime.Now.ToString(ApplicationGlobals.ShortDateTimePattern)),
                new ReportHeader(ApplicationGlobals.ApplicationName, string.Empty),
                new ReportHeader(string.Format(ResourceStrings.Text_Version, ApplicationGlobals.ApplicationVersion), string.Empty)
            };

            // Set the Report Data
            reports.ReportData = MapperInstance.Map<IEnumerable<ConfigurationViewModel>>(ConfigurationBll.GetConfigurationList(sort, filter));

            // Generate the Report
            var bActionPass = reports.GenerateReport();

            // Download the File
            ViewBag.ErrorMessage = string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Export);
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
        private ConfigurationViewModel GetViewModel(int id, DataOperation dataOperation = 0)
        {
            var configurationData = ConfigurationBll.GetConfigurationForId(id);
            var configurationViewModel = configurationData == null ? new ConfigurationViewModel() : MapperInstance.Map<ConfigurationViewModel>(configurationData);

            configurationViewModel.DataAction = dataOperation;
            configurationViewModel.ConfigurationTypeName = ConfigurationTypeName;

            return configurationViewModel;
        }
    }
}