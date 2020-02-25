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
    public class SecurityItemController : Controller
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
                    x.CreateMap<SecurityItemViewModel, SecurityItem>();
                    x.CreateMap<SecurityItem, SecurityItemViewModel>();
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
            var securityItemViewModel = GetViewModel(selectedId, dataOperation);
            return PartialView("MaintainSecurityItem", securityItemViewModel);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="ids">Selected Id's</param>
        /// <returns>True on Success, False on Failure</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.DELETE, SecurityItemCode = PageId.PAGE_SECURITYITEM)]
        public JsonResult Delete(int[] ids)
        {
            ISecurityItemBll iSecurityItemBll = new SecurityItemBll();
            var bActionPass = ids.Aggregate(true, (current, id) => current & iSecurityItemBll.DeleteSecurityItem(id, ApplicationGlobals.UserName));

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Delete));
        }

        /// <summary>
        /// Export to PDF
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "SecurityItemForm", MatchFormValue = "Export to PDF")]
        public ActionResult ExportToPdf()
        {
            GenerateReport(ReportFormat.PDF, ApplicationGlobals.Sort("gridSecurityItem"), ApplicationGlobals.Filter("gridSecurityItem"));
            return View("Index");
        }

        /// <summary>
        /// Export to Xls
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "SecurityItemForm", MatchFormValue = "Export to XLS")]
        public ActionResult ExportToXls()
        {
            GenerateReport(ReportFormat.XLS, ApplicationGlobals.Sort("gridSecurityItem"), ApplicationGlobals.Filter("gridSecurityItem"));
            return View("Index");
        }

        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>Index View</returns>
        [IsNewSession]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_SECURITYITEM)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Insert
        /// Called when the Submit button is clicked on the Insert Page
        /// </summary>
        /// <param name="securityItemViewModel">SecurityItemViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_SECURITYITEM)]
        public JsonResult Insert(SecurityItemViewModel securityItemViewModel)
        {
            ISecurityItemBll iSecurityItemBll = new SecurityItemBll();
            var bActionPass = ModelState.IsValid && iSecurityItemBll.InsertSecurityItem(GetModel(securityItemViewModel), ApplicationGlobals.UserName);

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_New_Record_Insert));
        }

        /// <summary>
        /// Check for Code Duplicates
        /// The OutputCacheAttribute attribute is required in order to prevent
        /// ASP.NET MVC from caching the results of the validation method.
        /// </summary>
        /// <param name="securityItemViewModel">SecurityItemViewModel Object</param>
        /// <returns>True or False</returns>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult IsCodeDuplicate(SecurityItemViewModel securityItemViewModel)
        {
            ISecurityItemBll iSecurityItemBll = new SecurityItemBll();
            var securityItem = iSecurityItemBll.GetSecurityItemForCodeAndCompanyInApplicationId(securityItemViewModel.Code.Trim(), ApplicationGlobals.SelectedCompanyInApplication.Id);

            var isCodeDuplicate = (securityItem != null && securityItemViewModel.Id == securityItem.Id) || securityItem == null;
            return Json(isCodeDuplicate, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get SecurityItem Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>SecurityItem Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_SECURITYITEM)]
        public ActionResult SecurityItemPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            ISecurityItemBll iSecurityItemBll = new SecurityItemBll();
            var securityItemPage = iSecurityItemBll.GetSecurityItemPage(take, skip, sort, filter, ApplicationGlobals.SelectedCompanyInApplication.Id);

            return Content(JsonConvert.SerializeObject(securityItemPage));
        }

        /// <summary>
        /// Update
        /// Called when the Submit button is clicked on the Edit Page
        /// </summary>
        /// <param name="securityItemViewModel">SecurityItemViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.UPDATE, SecurityItemCode = PageId.PAGE_SECURITYITEM)]
        public JsonResult Update(SecurityItemViewModel securityItemViewModel)
        {
            ISecurityItemBll iSecurityItemBll = new SecurityItemBll();
            var bActionPass = ModelState.IsValid && iSecurityItemBll.UpdateSecurityItem(GetModel(securityItemViewModel), ApplicationGlobals.UserName);

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
            Reports<SecurityItem> reports;
            switch (reportFormat)
            {
                case ReportFormat.PDF:
                    reports = new PdfReport<SecurityItem>();
                    break;

                case ReportFormat.XLS:
                    reports = new ExcelReport<SecurityItem>();
                    break;

                default:
                    throw new Exception("Invalid Report Format");
            }

            // Set Column Widths
            reports.ColumnWidthInPercent = new float[] { 35, 35, 30 };

            // Set Report Details
            reports.ReportTitle = ResourceStrings.Text_SecurityItem;
            reports.UserName = ApplicationGlobals.UserName;

            reports.ReportFilePath = Server.MapPath(ApplicationGlobals.TempDirectory);
            reports.CompanyLogo = Server.MapPath(ApplicationGlobals.LogoDirectory);

            // Set the Output Fields
            reports.OutputColumns = new[] { "Code", "Description", "SecurityTypeObj.Description" };
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
            ISecurityItemBll iSecurityItemBll = new SecurityItemBll();
            reports.ReportData = iSecurityItemBll.GetSecurityItemList(sort, filter, ApplicationGlobals.SelectedCompanyInApplication.Id);

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
        /// <param name="securityItemViewModel">SecurityItemViewModel Object</param>
        /// <returns>Business Entity</returns>
        private SecurityItem GetModel(SecurityItemViewModel securityItemViewModel)
        {
            return MapperInstance.Map<SecurityItem>(securityItemViewModel);
        }

        /// <summary>
        /// Get View Model
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="dataOperation">Data Operation</param>
        /// <returns>Business Entity</returns>
        private SecurityItemViewModel GetViewModel(int id, DataOperation dataOperation = 0)
        {
            ISecurityItemBll iSecurityItemBll = new SecurityItemBll();
            var securityItem = iSecurityItemBll.GetSecurityItemForId(id);

            var securityItemViewModel = securityItem == null ?
                new SecurityItemViewModel() : MapperInstance.Map<SecurityItemViewModel>(securityItem);

            ISecurityTypeBll iSecurityTypeBll = new SecurityTypeBll();
            securityItemViewModel.SecurityTypeList = iSecurityTypeBll.GetSecurityTypeDropDownList();

            securityItemViewModel.LnCompanyInApplicationId = ApplicationGlobals.SelectedCompanyInApplication.Id;

            securityItemViewModel.DataAction = dataOperation;
            return securityItemViewModel;
        }
    }
}