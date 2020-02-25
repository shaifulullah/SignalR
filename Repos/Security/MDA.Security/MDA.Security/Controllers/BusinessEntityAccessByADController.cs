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
    public class BusinessEntityAccessByADController : Controller
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
                    x.CreateMap<BusinessEntityAccessByADViewModel, BusinessEntityAccessByAD>();
                    x.CreateMap<BusinessEntityAccessByAD, BusinessEntityAccessByADViewModel>();
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
            var businessEntityAccessByADViewModel = GetViewModel(selectedId, dataOperation);
            return PartialView("MaintainBusinessEntityAccessByAD", businessEntityAccessByADViewModel);
        }

        /// <summary>
        /// Get BusinessEntityAccessByAD Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>BusinessEntityAccessByAD Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_BUSINESSENTITYACCESSBYAD)]
        public ActionResult BusinessEntityAccessByADPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            IBusinessEntityAccessByADBll iBusinessEntityAccessByADBll = new BusinessEntityAccessByADBll();
            var businessEntityAccessByADPage = iBusinessEntityAccessByADBll.GetBusinessEntityAccessByADPage(take, skip, sort, filter, ApplicationGlobals.SelectedSecurityBusinessEntities.Id);

            return Content(JsonConvert.SerializeObject(businessEntityAccessByADPage));
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="ids">Selected Id's</param>
        /// <returns>True on Success, False on Failure</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.DELETE, SecurityItemCode = PageId.PAGE_BUSINESSENTITYACCESSBYAD)]
        public JsonResult Delete(int[] ids)
        {
            IBusinessEntityAccessByADBll iBusinessEntityAccessByADBll = new BusinessEntityAccessByADBll();
            var bActionPass = ids.Aggregate(true, (current, id) => current & iBusinessEntityAccessByADBll.DeleteBusinessEntityAccessByAD(id, ApplicationGlobals.UserName));

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Delete));
        }

        /// <summary>
        /// Export to PDF
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "BusinessEntityAccessByADForm", MatchFormValue = "Export to PDF")]
        public ActionResult ExportToPdf()
        {
            GenerateReport(ReportFormat.PDF, ApplicationGlobals.Sort("gridBusinessEntityAccessByAD"), ApplicationGlobals.Filter("gridBusinessEntityAccessByAD"));
            return View("Index");
        }

        /// <summary>
        /// Export to Xls
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "BusinessEntityAccessByADForm", MatchFormValue = "Export to XLS")]
        public ActionResult ExportToXls()
        {
            GenerateReport(ReportFormat.XLS, ApplicationGlobals.Sort("gridBusinessEntityAccessByAD"), ApplicationGlobals.Filter("gridBusinessEntityAccessByAD"));
            return View("Index");
        }

        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>Index View</returns>
        [IsNewSession]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_BUSINESSENTITYACCESSBYAD)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Insert
        /// Called when the Submit button is clicked on the Insert Page
        /// </summary>
        /// <param name="businessEntityAccessByADViewModel">BusinessEntityAccessByADViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_BUSINESSENTITYACCESSBYAD)]
        public JsonResult Insert(BusinessEntityAccessByADViewModel businessEntityAccessByADViewModel)
        {
            IBusinessEntityAccessByADBll iBusinessEntityAccessByADBll = new BusinessEntityAccessByADBll();
            var bActionPass = ModelState.IsValid && iBusinessEntityAccessByADBll.InsertBusinessEntityAccessByAD(GetModel(businessEntityAccessByADViewModel), ApplicationGlobals.UserName);

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_New_Record_Insert));
        }

        /// <summary>
        /// Check for Code Duplicates
        /// The OutputCacheAttribute attribute is required in order to prevent
        /// ASP.NET MVC from caching the results of the validation method.
        /// </summary>
        /// <param name="businessEntityAccessByADViewModel">BusinessEntityAccessByADViewModel Object</param>
        /// <returns>True or False</returns>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult IsCodeDuplicate(BusinessEntityAccessByADViewModel businessEntityAccessByADViewModel)
        {
            IBusinessEntityAccessByADBll iBusinessEntityAccessByADBll = new BusinessEntityAccessByADBll();
            var businessEntityAccessByAD = iBusinessEntityAccessByADBll.GetBusinessEntityAccessByADForCode(businessEntityAccessByADViewModel.Value.Trim());

            var isCodeDuplicate = (businessEntityAccessByAD != null && businessEntityAccessByADViewModel.Id == businessEntityAccessByAD.Id) || businessEntityAccessByAD == null;
            return Json(isCodeDuplicate, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update
        /// Called when the Submit button is clicked on the Edit Page
        /// </summary>
        /// <param name="businessEntityAccessByADViewModel">BusinessEntityAccessByADViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.UPDATE, SecurityItemCode = PageId.PAGE_BUSINESSENTITYACCESSBYAD)]
        public JsonResult Update(BusinessEntityAccessByADViewModel businessEntityAccessByADViewModel)
        {
            IBusinessEntityAccessByADBll iBusinessEntityAccessByADBll = new BusinessEntityAccessByADBll();
            var bActionPass = ModelState.IsValid && iBusinessEntityAccessByADBll.UpdateBusinessEntityAccessByAD(GetModel(businessEntityAccessByADViewModel), ApplicationGlobals.UserName);

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
            Reports<BusinessEntityAccessByAD> reports;
            switch (reportFormat)
            {
                case ReportFormat.PDF:
                    reports = new PdfReport<BusinessEntityAccessByAD>();
                    break;

                case ReportFormat.XLS:
                    reports = new ExcelReport<BusinessEntityAccessByAD>();
                    break;

                default:
                    throw new Exception("Invalid Report Format");
            }

            // Set Report Details
            reports.ReportTitle = ResourceStrings.Text_BusinessEntityAccessByAD;
            reports.UserName = ApplicationGlobals.UserName;

            reports.ReportFilePath = Server.MapPath(ApplicationGlobals.TempDirectory);
            reports.CompanyLogo = Server.MapPath(ApplicationGlobals.LogoDirectory);

            // Set the Output Fields
            reports.OutputColumns = new[] { "LnActiveDirectoryGroupName", "Value" };
            reports.ColumnHeaders = ApplicationGlobals.VisibleGridColumnHeaders;

            // Set Report Headers
            reports.HeaderList = new[]
            {
                new ReportHeader(string.Empty, DateTime.Now.ToString(ApplicationGlobals.ShortDateTimePattern)),
                new ReportHeader(ApplicationGlobals.ApplicationName, string.Empty),
                new ReportHeader(string.Format(ResourceStrings.Text_Version, ApplicationGlobals.ApplicationVersion), string.Empty),
                new ReportHeader(ApplicationGlobals.GenerateReportHeaderDetails("SecurityBusinessEntities"), string.Empty)
            };

            // Get the Report Data
            IBusinessEntityAccessByADBll iBusinessEntityAccessByADBll = new BusinessEntityAccessByADBll();
            reports.ReportData = iBusinessEntityAccessByADBll.GetBusinessEntityAccessByADList(sort, filter, ApplicationGlobals.SelectedSecurityBusinessEntities.Id);

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
        /// <param name="businessEntityAccessByADViewModel">BusinessEntityAccessByADViewModel Object</param>
        /// <returns>Business Entity</returns>
        private BusinessEntityAccessByAD GetModel(BusinessEntityAccessByADViewModel businessEntityAccessByADViewModel)
        {
            return MapperInstance.Map<BusinessEntityAccessByAD>(businessEntityAccessByADViewModel);
        }

        /// <summary>
        /// Get View Model
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="dataOperation">Data Operation</param>
        /// <returns>Business Entity</returns>
        private BusinessEntityAccessByADViewModel GetViewModel(int id, DataOperation dataOperation = 0)
        {
            IBusinessEntityAccessByADBll iBusinessEntityAccessByADBll = new BusinessEntityAccessByADBll();
            var businessEntityAccessByAD = iBusinessEntityAccessByADBll.GetBusinessEntityAccessByADForId(id);

            var businessEntityAccessByADViewModel = businessEntityAccessByAD == null ?
                new BusinessEntityAccessByADViewModel() : MapperInstance.Map<BusinessEntityAccessByADViewModel>(businessEntityAccessByAD);

            businessEntityAccessByADViewModel.LnSecurityBusinessEntitiesId = ApplicationGlobals.SelectedSecurityBusinessEntities.Id;

            businessEntityAccessByADViewModel.DataAction = dataOperation;
            return businessEntityAccessByADViewModel;
        }
    }
}