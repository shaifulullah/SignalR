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
    using System.DirectoryServices;
    using System.DirectoryServices.ActiveDirectory;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI;
    using ViewModels;

    [SessionTimeout]
    [HandleAndLogError(ExceptionType = typeof(UnauthorizedException), View = "_UnauthorizedAccess", Order = 2)]
    [HandleAndLogError(ExceptionType = typeof(Exception), View = "_Error", Order = 1)]
    public class SecurityRolesController : Controller
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
                    x.CreateMap<SecurityRolesViewModel, SecurityRoles>();
                    x.CreateMap<SecurityRoles, SecurityRolesViewModel>();
                    x.CreateMap<SearchResult, ActiveDirectoryGroupViewModel>().ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Properties["Name"][0]));
                });

                return mapperConfiguration.CreateMapper();
            }
        }

        /// <summary>
        /// Get ActiveDirectoryGroupName Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>ActiveDirectoryGroupName Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operations = new[] { DataOperation.CREATE, DataOperation.UPDATE }, SecurityItemCode = PageId.PAGE_SECURITYROLES)]
        public ActionResult ActiveDirectoryGroupNamePage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            var domains = Forest.GetCurrentForest().Domains.Cast<Domain>();
            var activeDirectoryGroupNameList = new List<ActiveDirectoryGroupViewModel>();

            foreach (var domain in domains)
            {
                var root = new DirectoryEntry(string.Format("LDAP://{0}", domain.Name));
                var directorySearcherFilter = "(&(objectCategory=group)(|(cn=CSTAPP_*)))";

                DirectorySearcher directorySearcher = new DirectorySearcher(root) { Filter = directorySearcherFilter };
                SearchResultCollection searchResultCollection = directorySearcher.FindAll();

                var searchResultList = Enumerable.Cast<SearchResult>(searchResultCollection);
                activeDirectoryGroupNameList.AddRange(MapperInstance.Map<IEnumerable<ActiveDirectoryGroupViewModel>>(searchResultList));

                activeDirectoryGroupNameList.ForEach(x => x.DomainName = domain.Name);
            }

            var activeDirectoryGroupNamePage = activeDirectoryGroupNameList.ToDataSourceResult(take, skip, sort, filter);
            return Content(JsonConvert.SerializeObject(activeDirectoryGroupNamePage));
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
            var securityRolesViewModel = GetViewModel(selectedId, dataOperation);
            return PartialView("MaintainSecurityRoles", securityRolesViewModel);
        }

        /// <summary>
        /// Can Assign Users
        /// </summary>
        /// <param name="id">Selected Id</param>
        /// <returns>True if allowed to Assign Users, else False</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult CanAssignUsers(int id)
        {
            ISecurityRolesBll iSecurityRolesBll = new SecurityRolesBll();
            var securityRoles = iSecurityRolesBll.GetSecurityRolesForId(id);

            return Json((string.IsNullOrEmpty(securityRoles.LnActiveDirectoryGroupName) && string.IsNullOrEmpty(securityRoles.LnSkillCode)), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Selected Id</param>
        /// <returns>True on Success, False on Failure</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.DELETE, SecurityItemCode = PageId.PAGE_SECURITYROLES)]
        public JsonResult Delete(int id)
        {
            ISecurityRolesBll iSecurityRolesBll = new SecurityRolesBll();
            var bActionPass = iSecurityRolesBll.DeleteSecurityRoles(id, ApplicationGlobals.UserName);

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Delete));
        }

        /// <summary>
        /// Export to PDF
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "SecurityRolesForm", MatchFormValue = "Export to PDF")]
        public ActionResult ExportToPdf()
        {
            GenerateReport(ReportFormat.PDF, ApplicationGlobals.Sort("gridSecurityRoles"), ApplicationGlobals.Filter("gridSecurityRoles"));
            return View("Index");
        }

        /// <summary>
        /// Export to Xls
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "SecurityRolesForm", MatchFormValue = "Export to XLS")]
        public ActionResult ExportToXls()
        {
            GenerateReport(ReportFormat.XLS, ApplicationGlobals.Sort("gridSecurityRoles"), ApplicationGlobals.Filter("gridSecurityRoles"));
            return View("Index");
        }

        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>Index View</returns>
        [IsNewSession]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_SECURITYROLES)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Insert
        /// Called when the Submit button is clicked on the Insert Page
        /// </summary>
        /// <param name="securityRolesViewModel">SecurityRolesViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_SECURITYROLES)]
        public JsonResult Insert(SecurityRolesViewModel securityRolesViewModel)
        {
            ISecurityRolesBll iSecurityRolesBll = new SecurityRolesBll();
            var bActionPass = ModelState.IsValid && iSecurityRolesBll.InsertSecurityRoles(GetModel(securityRolesViewModel), ApplicationGlobals.UserName);

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_New_Record_Insert));
        }

        /// <summary>
        /// Check for Code Duplicates
        /// The OutputCacheAttribute attribute is required in order to prevent
        /// ASP.NET MVC from caching the results of the validation method.
        /// </summary>
        /// <param name="securityRolesViewModel">SecurityRolesViewModel Object</param>
        /// <returns>True or False</returns>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult IsCodeDuplicate(SecurityRolesViewModel securityRolesViewModel)
        {
            ISecurityRolesBll iSecurityRolesBll = new SecurityRolesBll();
            var securityRoles = iSecurityRolesBll.GetSecurityRolesForCodeAndCompanyInApplicationId(securityRolesViewModel.Code.Trim(), ApplicationGlobals.SelectedCompanyInApplication.Id);

            var isCodeDuplicate = (securityRoles != null && securityRolesViewModel.Id == securityRoles.Id) || securityRoles == null;
            return Json(isCodeDuplicate, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Is Deleted
        /// </summary>
        /// <param name="id">Selected Id</param>
        /// <returns>True if Is Deleted, else False</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult IsDeleted(int id)
        {
            ISecurityRolesBll iSecurityRolesBll = new SecurityRolesBll();
            var securityRoles = iSecurityRolesBll.GetSecurityRolesForId(id);

            return Json(securityRoles.IsDeleted, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Security Items
        /// </summary>
        /// <param name="formCollection">Form Collection</param>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "SecurityRolesForm", MatchFormValue = "Security Items")]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_SECURITYROLERIGHTS)]
        public RedirectToRouteResult SecurityItems(FormCollection formCollection)
        {
            ApplicationGlobals.SelectedSecurityRoles = GetViewModel(int.Parse(formCollection["hidGridCheckedRowsSecurityRoles"]));
            return RedirectToAction("Index", "SecurityRoleRights");
        }

        /// <summary>
        /// Get SecurityRoles Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>SecurityRoles Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_SECURITYROLES)]
        public ActionResult SecurityRolesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            ISecurityRolesBll iSecurityRolesBll = new SecurityRolesBll();
            var securityRolesPage = iSecurityRolesBll.GetSecurityRolesPage(take, skip, sort, filter, ApplicationGlobals.SelectedCompanyInApplication.Id);

            return Content(JsonConvert.SerializeObject(securityRolesPage));
        }

        /// <summary>
        /// Get Skills Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Skills Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operations = new[] { DataOperation.CREATE, DataOperation.UPDATE }, SecurityItemCode = PageId.PAGE_SECURITYROLES)]
        public ActionResult SkillsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            IRO_SkillsBll iRO_SkillsBll = new RO_SkillsBll();
            var skillsPage = iRO_SkillsBll.GetSkillsPage(take, skip, sort, filter);

            return Content(JsonConvert.SerializeObject(skillsPage));
        }

        /// <summary>
        /// Update
        /// Called when the Submit button is clicked on the Edit Page
        /// </summary>
        /// <param name="securityRolesViewModel">SecurityRolesViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.UPDATE, SecurityItemCode = PageId.PAGE_SECURITYROLES)]
        public JsonResult Update(SecurityRolesViewModel securityRolesViewModel)
        {
            ISecurityRolesBll iSecurityRolesBll = new SecurityRolesBll();
            var bActionPass = ModelState.IsValid && iSecurityRolesBll.UpdateSecurityRoles(GetModel(securityRolesViewModel), ApplicationGlobals.UserName);

            ISecurityUserInRolesBll iSecurityUserInRolesBll = new SecurityUserInRolesBll();
            var securityUserInRolesList = iSecurityUserInRolesBll.GetSecurityUserInRolesListForSecurityRolesId(securityRolesViewModel.Id);

            if (bActionPass && securityUserInRolesList.Any() && (!string.IsNullOrEmpty(securityRolesViewModel.LnActiveDirectoryGroupName) || !string.IsNullOrEmpty(securityRolesViewModel.LnSkillCode)))
            {
                var securityUserInRolesIds = securityUserInRolesList.Select(x => x.Id).ToList();
                bActionPass &= securityUserInRolesIds.Aggregate(true, (current, id) => current & iSecurityUserInRolesBll.DeleteSecurityUserInRoles(id, ApplicationGlobals.UserName));
            }

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Edit_View));
        }

        /// <summary>
        /// Users
        /// </summary>
        /// <param name="formCollection">Form Collection</param>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "SecurityRolesForm", MatchFormValue = "Users")]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_SECURITYUSERINROLES)]
        public RedirectToRouteResult Users(FormCollection formCollection)
        {
            ApplicationGlobals.SelectedSecurityRoles = GetViewModel(int.Parse(formCollection["hidGridCheckedRowsSecurityRoles"]));
            return RedirectToAction("Index", "SecurityUserInRoles");
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
            Reports<SecurityRoles> reports;
            switch (reportFormat)
            {
                case ReportFormat.PDF:
                    reports = new PdfReport<SecurityRoles>();
                    break;

                case ReportFormat.XLS:
                    reports = new ExcelReport<SecurityRoles>();
                    break;

                default:
                    throw new Exception("Invalid Report Format");
            }

            // Set Column Widths
            reports.ColumnWidthInPercent = new float[] { 15, 30, 30, 18, 7 };

            // Set Report Details
            reports.ReportTitle = ResourceStrings.Text_SecurityRoles;
            reports.UserName = ApplicationGlobals.UserName;

            reports.ReportFilePath = Server.MapPath(ApplicationGlobals.TempDirectory);
            reports.CompanyLogo = Server.MapPath(ApplicationGlobals.LogoDirectory);

            // Set the Output Fields
            reports.OutputColumns = new[] { "Code", "Description", "LnActiveDirectoryGroupName", "LnSkillCode", "IsDeleted" };
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
            ISecurityRolesBll iSecurityRolesBll = new SecurityRolesBll();
            reports.ReportData = iSecurityRolesBll.GetSecurityRolesList(sort, filter, ApplicationGlobals.SelectedCompanyInApplication.Id);

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
        /// <param name="securityRolesViewModel">SecurityRolesViewModel Object</param>
        /// <returns>Business Entity</returns>
        private SecurityRoles GetModel(SecurityRolesViewModel securityRolesViewModel)
        {
            return MapperInstance.Map<SecurityRoles>(securityRolesViewModel);
        }

        /// <summary>
        /// Get View Model
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="dataOperation">Data Operation</param>
        /// <returns>Business Entity</returns>
        private SecurityRolesViewModel GetViewModel(int id, DataOperation dataOperation = 0)
        {
            ISecurityRolesBll iSecurityRolesBll = new SecurityRolesBll();
            var securityRoles = iSecurityRolesBll.GetSecurityRolesForId(id);

            var securityRolesViewModel = securityRoles == null ?
                new SecurityRolesViewModel() : MapperInstance.Map<SecurityRolesViewModel>(securityRoles);

            securityRolesViewModel.LnCompanyInApplicationId = ApplicationGlobals.SelectedCompanyInApplication.Id;

            securityRolesViewModel.DataAction = dataOperation;
            return securityRolesViewModel;
        }
    }
}