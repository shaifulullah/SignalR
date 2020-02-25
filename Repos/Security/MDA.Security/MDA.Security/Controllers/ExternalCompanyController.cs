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
    public class ExternalCompanyController : Controller
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
                    x.CreateMap<ExternalCompanyViewModel, ExternalCompany>();
                    x.CreateMap<ExternalCompany, ExternalCompanyViewModel>();
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
            var externalCompanyViewModel = GetViewModel(selectedId, dataOperation);
            return PartialView("MaintainExternalCompany", externalCompanyViewModel);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="ids">Selected Id's</param>
        /// <returns>True on Success, False on Failure</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.DELETE, SecurityItemCode = PageId.PAGE_EXTERNALCOMPANY)]
        public JsonResult Delete(int[] ids)
        {
            IExternalCompanyBll iExternalCompanyBll = new ExternalCompanyBll();
            var bActionPass = ids.Aggregate(true, (current, id) => current & iExternalCompanyBll.DeleteExternalCompany(id, ApplicationGlobals.UserName));

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Delete));
        }

        /// <summary>
        /// Export to PDF
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "ExternalCompanyForm", MatchFormValue = "Export to PDF")]
        public ActionResult ExportToPdf()
        {
            GenerateReport(ReportFormat.PDF, ApplicationGlobals.Sort("gridExternalCompany"), ApplicationGlobals.Filter("gridExternalCompany"));
            return View("Index");
        }

        /// <summary>
        /// Export to Xls
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "ExternalCompanyForm", MatchFormValue = "Export to XLS")]
        public ActionResult ExportToXls()
        {
            GenerateReport(ReportFormat.XLS, ApplicationGlobals.Sort("gridExternalCompany"), ApplicationGlobals.Filter("gridExternalCompany"));
            return View("Index");
        }

        /// <summary>
        /// Get ExternalCompany Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>ExternalCompany Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_EXTERNALCOMPANY)]
        public ActionResult ExternalCompanyPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            IExternalCompanyBll iExternalCompanyBll = new ExternalCompanyBll();
            var externalCompanyPage = iExternalCompanyBll.GetExternalCompanyPage(take, skip, sort, filter);

            return Content(JsonConvert.SerializeObject(externalCompanyPage));
        }

        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>Index View</returns>
        [IsNewSession]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_EXTERNALCOMPANY)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Insert
        /// Called when the Submit button is clicked on the Insert Page
        /// </summary>
        /// <param name="externalCompanyViewModel">ExternalCompanyViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_EXTERNALCOMPANY)]
        public JsonResult Insert(ExternalCompanyViewModel externalCompanyViewModel)
        {
            IExternalCompanyBll iExternalCompanyBll = new ExternalCompanyBll();
            var bActionPass = ModelState.IsValid && iExternalCompanyBll.InsertExternalCompany(GetModel(externalCompanyViewModel), ApplicationGlobals.UserName);

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_New_Record_Insert));
        }

        /// <summary>
        /// Check for Code Duplicates
        /// The OutputCacheAttribute attribute is required in order to prevent
        /// ASP.NET MVC from caching the results of the validation method.
        /// </summary>
        /// <param name="externalCompanyViewModel">ExternalCompanyViewModel Object</param>
        /// <returns>True or False</returns>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult IsCodeDuplicate(ExternalCompanyViewModel externalCompanyViewModel)
        {
            IExternalCompanyBll iExternalCompanyBll = new ExternalCompanyBll();
            var externalCompany = iExternalCompanyBll.GetExternalCompanyForCode(externalCompanyViewModel.Code.Trim());

            var isCodeDuplicate = (externalCompany != null && externalCompanyViewModel.Id == externalCompany.Id) || externalCompany == null;
            return Json(isCodeDuplicate, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Persons
        /// </summary>
        /// <param name="formCollection">Form Collection</param>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "ExternalCompanyForm", MatchFormValue = "Persons")]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_EXTERNALPERSON)]
        public RedirectToRouteResult Persons(FormCollection formCollection)
        {
            ApplicationGlobals.SelectedExternalCompany = GetViewModel(int.Parse(formCollection["hidGridCheckedRowsExternalCompany"]));
            return RedirectToAction("Index", "ExternalPerson");
        }

        /// <summary>
        /// Update
        /// Called when the Submit button is clicked on the Edit Page
        /// </summary>
        /// <param name="externalCompanyViewModel">ExternalCompanyViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.UPDATE, SecurityItemCode = PageId.PAGE_EXTERNALCOMPANY)]
        public JsonResult Update(ExternalCompanyViewModel externalCompanyViewModel)
        {
            IExternalCompanyBll iExternalCompanyBll = new ExternalCompanyBll();
            var bActionPass = ModelState.IsValid && iExternalCompanyBll.UpdateExternalCompany(GetModel(externalCompanyViewModel), ApplicationGlobals.UserName);

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
            Reports<ExternalCompany> reports;
            switch (reportFormat)
            {
                case ReportFormat.PDF:
                    reports = new PdfReport<ExternalCompany>();
                    break;

                case ReportFormat.XLS:
                    reports = new ExcelReport<ExternalCompany>();
                    break;

                default:
                    throw new Exception("Invalid Report Format");
            }

            // Set Report Details
            reports.ReportTitle = ResourceStrings.Text_ExternalCompany;
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

            // Get the Report Data
            IExternalCompanyBll iExternalCompanyBll = new ExternalCompanyBll();
            reports.ReportData = iExternalCompanyBll.GetExternalCompanyList(sort, filter);

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
        /// <param name="externalCompanyViewModel">ExternalCompanyViewModel Object</param>
        /// <returns>Business Entity</returns>
        private ExternalCompany GetModel(ExternalCompanyViewModel externalCompanyViewModel)
        {
            return MapperInstance.Map<ExternalCompany>(externalCompanyViewModel);
        }

        /// <summary>
        /// Get View Model
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="dataOperation">Data Operation</param>
        /// <returns>Business Entity</returns>
        private ExternalCompanyViewModel GetViewModel(int id, DataOperation dataOperation = 0)
        {
            IExternalCompanyBll iExternalCompanyBll = new ExternalCompanyBll();
            var externalCompany = iExternalCompanyBll.GetExternalCompanyForId(id);

            var externalCompanyViewModel = externalCompany == null ?
                new ExternalCompanyViewModel() : MapperInstance.Map<ExternalCompanyViewModel>(externalCompany);

            externalCompanyViewModel.DataAction = dataOperation;
            return externalCompanyViewModel;
        }
    }
}