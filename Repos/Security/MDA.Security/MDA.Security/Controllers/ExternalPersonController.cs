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
    using System.Web.Mvc;
    using System.Web.UI;
    using ViewModels;

    [SessionTimeout]
    [HandleAndLogError(ExceptionType = typeof(UnauthorizedException), View = "_UnauthorizedAccess", Order = 2)]
    [HandleAndLogError(ExceptionType = typeof(Exception), View = "_Error", Order = 1)]
    public class ExternalPersonController : Controller
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
                    x.CreateMap<ExternalPersonViewModel, ExternalPerson>();
                    x.CreateMap<ExternalPerson, ExternalPersonViewModel>();
                });

                return mapperConfiguration.CreateMapper();
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
            var externalPersonViewModel = GetViewModel(selectedId, dataOperation);
            return PartialView("MaintainExternalPerson", externalPersonViewModel);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Selected Id</param>
        /// <returns>True on Success, False on Failure</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.DELETE, SecurityItemCode = PageId.PAGE_EXTERNALPERSON)]
        public JsonResult Delete(int id)
        {
            IExternalPersonBll iExternalPersonBll = new ExternalPersonBll();
            var bActionPass = iExternalPersonBll.DeleteExternalPerson(id, ApplicationGlobals.UserName);

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Delete));
        }

        /// <summary>
        /// Export to PDF
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "ExternalPersonForm", MatchFormValue = "Export to PDF")]
        public ActionResult ExportToPdf()
        {
            GenerateReport(ReportFormat.PDF, ApplicationGlobals.Sort("gridExternalPerson"), ApplicationGlobals.Filter("gridExternalPerson"));
            return View("Index");
        }

        /// <summary>
        /// Export to Xls
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "ExternalPersonForm", MatchFormValue = "Export to XLS")]
        public ActionResult ExportToXls()
        {
            GenerateReport(ReportFormat.XLS, ApplicationGlobals.Sort("gridExternalPerson"), ApplicationGlobals.Filter("gridExternalPerson"));
            return View("Index");
        }

        /// <summary>
        /// Get ExternalPerson Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>ExternalPerson Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_EXTERNALPERSON)]
        public ActionResult ExternalPersonPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            IExternalPersonBll iExternalPersonBll = new ExternalPersonBll();
            var externalPersonPage = iExternalPersonBll.GetExternalPersonPage(take, skip, sort, filter, ApplicationGlobals.SelectedExternalCompany.Id);

            return Content(JsonConvert.SerializeObject(externalPersonPage));
        }

        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>Index View</returns>
        [IsNewSession]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_EXTERNALPERSON)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Insert
        /// Called when the Submit button is clicked on the Insert Page
        /// </summary>
        /// <param name="externalPersonViewModel">ExternalPersonViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_EXTERNALPERSON)]
        public JsonResult Insert(ExternalPersonViewModel externalPersonViewModel)
        {
            IExternalPersonBll iExternalPersonBll = new ExternalPersonBll();
            var bActionPass = ModelState.IsValid && iExternalPersonBll.InsertExternalPerson(GetModel(externalPersonViewModel), ApplicationGlobals.UserName);

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_New_Record_Insert));
        }

        /// <summary>
        /// Is Active Record
        /// </summary>
        /// <param name="id">Selected Id</param>
        /// <returns>True if Active, else False</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult IsActiveRecord(int id)
        {
            IExternalPersonBll iExternalPersonBll = new ExternalPersonBll();
            var externalPerson = iExternalPersonBll.GetExternalPersonForId(id);

            return Json(externalPerson.IsActive, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Check for Code Duplicates
        /// The OutputCacheAttribute attribute is required in order to prevent
        /// ASP.NET MVC from caching the results of the validation method.
        /// </summary>
        /// <param name="externalPersonViewModel">ExternalPersonViewModel Object</param>
        /// <returns>True or False</returns>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult IsCodeDuplicate(ExternalPersonViewModel externalPersonViewModel)
        {
            IExternalPersonBll iExternalPersonBll = new ExternalPersonBll();
            var externalPerson = iExternalPersonBll.GetExternalPersonForCodeAndExternalCompanyId(externalPersonViewModel.PersonCode.Trim(), ApplicationGlobals.SelectedExternalCompany.Id);

            var isCodeDuplicate = (externalPerson != null && externalPersonViewModel.Id == externalPerson.Id) || externalPerson == null;
            return Json(isCodeDuplicate, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update
        /// Called when the Submit button is clicked on the Edit Page
        /// </summary>
        /// <param name="externalPersonViewModel">ExternalPersonViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.UPDATE, SecurityItemCode = PageId.PAGE_EXTERNALPERSON)]
        public JsonResult Update(ExternalPersonViewModel externalPersonViewModel)
        {
            IExternalPersonBll iExternalPersonBll = new ExternalPersonBll();
            var bActionPass = ModelState.IsValid && iExternalPersonBll.UpdateExternalPerson(GetModel(externalPersonViewModel), ApplicationGlobals.UserName);

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
            Reports<ExternalPerson> reports;
            switch (reportFormat)
            {
                case ReportFormat.PDF:
                    reports = new PdfReport<ExternalPerson>();
                    break;

                case ReportFormat.XLS:
                    reports = new ExcelReport<ExternalPerson>();
                    break;

                default:
                    throw new Exception("Invalid Report Format");
            }

            // Set Column Widths
            reports.ColumnWidthInPercent = new float[] { 15, 30, 45, 10 };

            // Set Report Details
            reports.ReportTitle = ResourceStrings.Text_ExternalPerson;
            reports.UserName = ApplicationGlobals.UserName;

            reports.ReportFilePath = Server.MapPath(ApplicationGlobals.TempDirectory);
            reports.CompanyLogo = Server.MapPath(ApplicationGlobals.LogoDirectory);

            // Set the Output Fields
            reports.OutputColumns = new[] { "PersonCode", "FullName", "eMail", "IsActive" };
            reports.ColumnHeaders = ApplicationGlobals.VisibleGridColumnHeaders;

            // Set Report Headers
            reports.HeaderList = new[]
            {
                new ReportHeader(string.Empty, DateTime.Now.ToString(ApplicationGlobals.ShortDateTimePattern)),
                new ReportHeader(ApplicationGlobals.ApplicationName, string.Empty),
                new ReportHeader(string.Format(ResourceStrings.Text_Version, ApplicationGlobals.ApplicationVersion), string.Empty),
                new ReportHeader(ApplicationGlobals.GenerateReportHeaderDetails("ExternalCompany"), string.Empty)
            };

            // Get the Report Data
            IExternalPersonBll iExternalPersonBll = new ExternalPersonBll();
            reports.ReportData = iExternalPersonBll.GetExternalPersonList(sort, filter, ApplicationGlobals.SelectedExternalCompany.Id);

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
        /// <param name="externalPersonViewModel">ExternalPersonViewModel Object</param>
        /// <returns>Business Entity</returns>
        private ExternalPerson GetModel(ExternalPersonViewModel externalPersonViewModel)
        {
            return MapperInstance.Map<ExternalPerson>(externalPersonViewModel);
        }

        /// <summary>
        /// Get View Model
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="dataOperation">Data Operation</param>
        /// <returns>Business Entity</returns>
        private ExternalPersonViewModel GetViewModel(int id, DataOperation dataOperation = 0)
        {
            IExternalPersonBll iExternalPersonBll = new ExternalPersonBll();
            var externalPerson = iExternalPersonBll.GetExternalPersonForId(id);

            var externalPersonViewModel = externalPerson == null ?
                new ExternalPersonViewModel() : MapperInstance.Map<ExternalPersonViewModel>(externalPerson);

            externalPersonViewModel.LnExternalCompanyId = ApplicationGlobals.SelectedExternalCompany.Id;

            externalPersonViewModel.DataAction = dataOperation;
            return externalPersonViewModel;
        }
    }
}