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
    public class SecurityBusinessEntitiesController : Controller
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
                    x.CreateMap<SecurityBusinessEntitiesViewModel, SecurityBusinessEntities>();
                    x.CreateMap<SecurityBusinessEntities, SecurityBusinessEntitiesViewModel>();
                });

                return mapperConfiguration.CreateMapper();
            }
        }

        /// <summary>
        /// Access By AD
        /// </summary>
        /// <param name="formCollection">Form Collection</param>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "SecurityBusinessEntitiesForm", MatchFormValue = "Access By AD")]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_BUSINESSENTITYACCESSBYAD)]
        public RedirectToRouteResult AccessByAD(FormCollection formCollection)
        {
            ApplicationGlobals.SelectedSecurityBusinessEntities = GetViewModel(int.Parse(formCollection["hidGridCheckedRowsSecurityBusinessEntities"]));
            return RedirectToAction("Index", "BusinessEntityAccessByAD");
        }

        /// <summary>
        /// Access By Role
        /// </summary>
        /// <param name="formCollection">Form Collection</param>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "SecurityBusinessEntitiesForm", MatchFormValue = "Access By Role")]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_BUSINESSENTITYACCESSBYROLE)]
        public RedirectToRouteResult AccessByRole(FormCollection formCollection)
        {
            ApplicationGlobals.SelectedSecurityBusinessEntities = GetViewModel(int.Parse(formCollection["hidGridCheckedRowsSecurityBusinessEntities"]));
            return RedirectToAction("Index", "BusinessEntityAccessByRole");
        }

        /// <summary>
        /// Access By User
        /// </summary>
        /// <param name="formCollection">Form Collection</param>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "SecurityBusinessEntitiesForm", MatchFormValue = "Access By User")]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_BUSINESSENTITYACCESSBYUSER)]
        public RedirectToRouteResult AccessByUser(FormCollection formCollection)
        {
            ApplicationGlobals.SelectedSecurityBusinessEntities = GetViewModel(int.Parse(formCollection["hidGridCheckedRowsSecurityBusinessEntities"]));
            return RedirectToAction("Index", "BusinessEntityAccessByUser");
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
            var securityBusinessEntitiesViewModel = GetViewModel(selectedId, dataOperation);
            return PartialView("MaintainSecurityBusinessEntities", securityBusinessEntitiesViewModel);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="ids">Selected Id's</param>
        /// <returns>True on Success, False on Failure</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.DELETE, SecurityItemCode = PageId.PAGE_SECURITYBUSINESSENTITIES)]
        public JsonResult Delete(int[] ids)
        {
            ISecurityBusinessEntitiesBll iSecurityBusinessEntitiesBll = new SecurityBusinessEntitiesBll();
            var bActionPass = ids.Aggregate(true, (current, id) => current & iSecurityBusinessEntitiesBll.DeleteSecurityBusinessEntities(id, ApplicationGlobals.UserName));

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Delete));
        }

        /// <summary>
        /// Export to PDF
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "SecurityBusinessEntitiesForm", MatchFormValue = "Export to PDF")]
        public ActionResult ExportToPdf()
        {
            GenerateReport(ReportFormat.PDF, ApplicationGlobals.Sort("gridSecurityBusinessEntities"), ApplicationGlobals.Filter("gridSecurityBusinessEntities"));
            return View("Index");
        }

        /// <summary>
        /// Export to Xls
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "SecurityBusinessEntitiesForm", MatchFormValue = "Export to XLS")]
        public ActionResult ExportToXls()
        {
            GenerateReport(ReportFormat.XLS, ApplicationGlobals.Sort("gridSecurityBusinessEntities"), ApplicationGlobals.Filter("gridSecurityBusinessEntities"));
            return View("Index");
        }

        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>Index View</returns>
        [IsNewSession]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_SECURITYBUSINESSENTITIES)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Insert
        /// Called when the Submit button is clicked on the Insert Page
        /// </summary>
        /// <param name="securityBusinessEntitiesViewModel">SecurityBusinessEntitiesViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_SECURITYBUSINESSENTITIES)]
        public JsonResult Insert(SecurityBusinessEntitiesViewModel securityBusinessEntitiesViewModel)
        {
            ISecurityBusinessEntitiesBll iSecurityBusinessEntitiesBll = new SecurityBusinessEntitiesBll();
            var bActionPass = ModelState.IsValid && iSecurityBusinessEntitiesBll.InsertSecurityBusinessEntities(GetModel(securityBusinessEntitiesViewModel), ApplicationGlobals.UserName);

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_New_Record_Insert));
        }

        /// <summary>
        /// Check for Code Duplicates
        /// The OutputCacheAttribute attribute is required in order to prevent
        /// ASP.NET MVC from caching the results of the validation method.
        /// </summary>
        /// <param name="securityBusinessEntitiesViewModel">SecurityBusinessEntitiesViewModel Object</param>
        /// <returns>True or False</returns>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult IsCodeDuplicate(SecurityBusinessEntitiesViewModel securityBusinessEntitiesViewModel)
        {
            ISecurityBusinessEntitiesBll iSecurityBusinessEntitiesBll = new SecurityBusinessEntitiesBll();
            var securityBusinessEntities = iSecurityBusinessEntitiesBll.GetSecurityBusinessEntitiesForCode(securityBusinessEntitiesViewModel.Code.Trim());

            var isCodeDuplicate = (securityBusinessEntities != null && securityBusinessEntitiesViewModel.Id == securityBusinessEntities.Id) || securityBusinessEntities == null;
            return Json(isCodeDuplicate, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Restriction By AD
        /// </summary>
        /// <param name="formCollection">Form Collection</param>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "SecurityBusinessEntitiesForm", MatchFormValue = "Restriction By AD")]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_BUSINESSENTITYRESTRICTIONBYAD)]
        public RedirectToRouteResult RestrictionByAD(FormCollection formCollection)
        {
            ApplicationGlobals.SelectedSecurityBusinessEntities = GetViewModel(int.Parse(formCollection["hidGridCheckedRowsSecurityBusinessEntities"]));
            return RedirectToAction("Index", "BusinessEntityRestrictionByAD");
        }

        /// <summary>
        /// Restriction By Role
        /// </summary>
        /// <param name="formCollection">Form Collection</param>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "SecurityBusinessEntitiesForm", MatchFormValue = "Restriction By Role")]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_BUSINESSENTITYRESTRICTIONBYROLE)]
        public RedirectToRouteResult RestrictionByRole(FormCollection formCollection)
        {
            ApplicationGlobals.SelectedSecurityBusinessEntities = GetViewModel(int.Parse(formCollection["hidGridCheckedRowsSecurityBusinessEntities"]));
            return RedirectToAction("Index", "BusinessEntityRestrictionByRole");
        }

        /// <summary>
        /// Restriction By User
        /// </summary>
        /// <param name="formCollection">Form Collection</param>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "SecurityBusinessEntitiesForm", MatchFormValue = "Restriction By User")]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_BUSINESSENTITYRESTRICTIONBYUSER)]
        public RedirectToRouteResult RestrictionByUser(FormCollection formCollection)
        {
            ApplicationGlobals.SelectedSecurityBusinessEntities = GetViewModel(int.Parse(formCollection["hidGridCheckedRowsSecurityBusinessEntities"]));
            return RedirectToAction("Index", "BusinessEntityRestrictionByUser");
        }

        /// <summary>
        /// Get SecurityBusinessEntities Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>SecurityBusinessEntities Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_SECURITYBUSINESSENTITIES)]
        public ActionResult SecurityBusinessEntitiesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            ISecurityBusinessEntitiesBll iSecurityBusinessEntitiesBll = new SecurityBusinessEntitiesBll();
            var securityBusinessEntitiesPage = iSecurityBusinessEntitiesBll.GetSecurityBusinessEntitiesPage(take, skip, sort, filter);

            return Content(JsonConvert.SerializeObject(securityBusinessEntitiesPage));
        }

        /// <summary>
        /// Update
        /// Called when the Submit button is clicked on the Edit Page
        /// </summary>
        /// <param name="securityBusinessEntitiesViewModel">SecurityBusinessEntitiesViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.UPDATE, SecurityItemCode = PageId.PAGE_SECURITYBUSINESSENTITIES)]
        public JsonResult Update(SecurityBusinessEntitiesViewModel securityBusinessEntitiesViewModel)
        {
            ISecurityBusinessEntitiesBll iSecurityBusinessEntitiesBll = new SecurityBusinessEntitiesBll();
            var bActionPass = ModelState.IsValid && iSecurityBusinessEntitiesBll.UpdateSecurityBusinessEntities(GetModel(securityBusinessEntitiesViewModel), ApplicationGlobals.UserName);

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
            Reports<SecurityBusinessEntities> reports;
            switch (reportFormat)
            {
                case ReportFormat.PDF:
                    reports = new PdfReport<SecurityBusinessEntities>();
                    break;

                case ReportFormat.XLS:
                    reports = new ExcelReport<SecurityBusinessEntities>();
                    break;

                default:
                    throw new Exception("Invalid Report Format");
            }

            // Set Report Details
            reports.ReportTitle = ResourceStrings.Text_SecurityBusinessEntities;
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
            ISecurityBusinessEntitiesBll iSecurityBusinessEntitiesBll = new SecurityBusinessEntitiesBll();
            reports.ReportData = iSecurityBusinessEntitiesBll.GetSecurityBusinessEntitiesList(sort, filter);

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
        /// <param name="securityBusinessEntitiesViewModel">SecurityBusinessEntitiesViewModel Object</param>
        /// <returns>Business Entity</returns>
        private SecurityBusinessEntities GetModel(SecurityBusinessEntitiesViewModel securityBusinessEntitiesViewModel)
        {
            return MapperInstance.Map<SecurityBusinessEntities>(securityBusinessEntitiesViewModel);
        }

        /// <summary>
        /// Get View Model
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="dataOperation">Data Operation</param>
        /// <returns>Business Entity</returns>
        private SecurityBusinessEntitiesViewModel GetViewModel(int id, DataOperation dataOperation = 0)
        {
            ISecurityBusinessEntitiesBll iSecurityBusinessEntitiesBll = new SecurityBusinessEntitiesBll();
            var securityBusinessEntities = iSecurityBusinessEntitiesBll.GetSecurityBusinessEntitiesForId(id);

            var securityBusinessEntitiesViewModel = securityBusinessEntities == null ?
                new SecurityBusinessEntitiesViewModel() : MapperInstance.Map<SecurityBusinessEntitiesViewModel>(securityBusinessEntities);

            securityBusinessEntitiesViewModel.DataAction = dataOperation;
            return securityBusinessEntitiesViewModel;
        }
    }
}