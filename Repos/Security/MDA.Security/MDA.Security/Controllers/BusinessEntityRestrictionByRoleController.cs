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
    public class BusinessEntityRestrictionByRoleController : Controller
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
                    x.CreateMap<BusinessEntityRestrictionByRoleViewModel, BusinessEntityRestrictionByRole>();

                    x.CreateMap<BusinessEntityRestrictionByRole, BusinessEntityRestrictionByRoleViewModel>()
                        .ForMember(dest => dest.SecurityRolesCode, opt => opt.MapFrom(src => src.SecurityRolesObj.Code))
                        .ForMember(dest => dest.SecurityRolesDescription, opt => opt.MapFrom(src => src.SecurityRolesObj.Description))
                        .ForMember(dest => dest.Application, opt => opt.MapFrom(src => src.SecurityRolesObj.CompanyInApplicationObj.ApplicationObj.Description))
                        .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.SecurityRolesObj.CompanyInApplicationObj.CompanyObj.Description));
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
            var businessEntityRestrictionByRoleViewModel = GetViewModel(selectedId, dataOperation);
            return PartialView("MaintainBusinessEntityRestrictionByRole", businessEntityRestrictionByRoleViewModel);
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByRole Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>BusinessEntityRestrictionByRole Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_BUSINESSENTITYRESTRICTIONBYROLE)]
        public ActionResult BusinessEntityRestrictionByRolePage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            IBusinessEntityRestrictionByRoleBll iBusinessEntityRestrictionByRoleBll = new BusinessEntityRestrictionByRoleBll();
            var businessEntityRestrictionByRolePage = iBusinessEntityRestrictionByRoleBll.GetBusinessEntityRestrictionByRolePage(take, skip, sort, filter, ApplicationGlobals.SelectedSecurityBusinessEntities.Id);

            return Content(JsonConvert.SerializeObject(businessEntityRestrictionByRolePage));
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="ids">Selected Id's</param>
        /// <returns>True on Success, False on Failure</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.DELETE, SecurityItemCode = PageId.PAGE_BUSINESSENTITYRESTRICTIONBYROLE)]
        public JsonResult Delete(int[] ids)
        {
            IBusinessEntityRestrictionByRoleBll iBusinessEntityRestrictionByRoleBll = new BusinessEntityRestrictionByRoleBll();
            var bActionPass = ids.Aggregate(true, (current, id) => current & iBusinessEntityRestrictionByRoleBll.DeleteBusinessEntityRestrictionByRole(id, ApplicationGlobals.UserName));

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Delete));
        }

        /// <summary>
        /// Export to PDF
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "BusinessEntityRestrictionByRoleForm", MatchFormValue = "Export to PDF")]
        public ActionResult ExportToPdf()
        {
            GenerateReport(ReportFormat.PDF, ApplicationGlobals.Sort("gridBusinessEntityRestrictionByRole"), ApplicationGlobals.Filter("gridBusinessEntityRestrictionByRole"));
            return View("Index");
        }

        /// <summary>
        /// Export to Xls
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "BusinessEntityRestrictionByRoleForm", MatchFormValue = "Export to XLS")]
        public ActionResult ExportToXls()
        {
            GenerateReport(ReportFormat.XLS, ApplicationGlobals.Sort("gridBusinessEntityRestrictionByRole"), ApplicationGlobals.Filter("gridBusinessEntityRestrictionByRole"));
            return View("Index");
        }

        /// <summary>
        /// Get SecurityRoles Data
        /// </summary>
        /// <param name="id">Selected Id</param>
        /// <returns>SecurityRoles Data</returns>
        public JsonResult GetSecurityRolesData(int id)
        {
            ISecurityRolesBll iSecurityRolesBll = new SecurityRolesBll();
            var securityRoles = iSecurityRolesBll.GetSecurityRolesForId(id);

            return Json(new
            {
                SecurityRolesCode = securityRoles.Code,
                SecurityRolesDescription = securityRoles.Description,
                Application = securityRoles.CompanyInApplicationObj.ApplicationObj.Description,
                Company = securityRoles.CompanyInApplicationObj.CompanyObj.Description
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>Index View</returns>
        [IsNewSession]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_BUSINESSENTITYRESTRICTIONBYROLE)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Insert
        /// Called when the Submit button is clicked on the Insert Page
        /// </summary>
        /// <param name="businessEntityRestrictionByRoleViewModel">BusinessEntityRestrictionByRoleViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_BUSINESSENTITYRESTRICTIONBYROLE)]
        public JsonResult Insert(BusinessEntityRestrictionByRoleViewModel businessEntityRestrictionByRoleViewModel)
        {
            IBusinessEntityRestrictionByRoleBll iBusinessEntityRestrictionByRoleBll = new BusinessEntityRestrictionByRoleBll();
            var bActionPass = ModelState.IsValid && iBusinessEntityRestrictionByRoleBll.InsertBusinessEntityRestrictionByRole(GetModel(businessEntityRestrictionByRoleViewModel), ApplicationGlobals.UserName);

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_New_Record_Insert));
        }

        /// <summary>
        /// Check for Value Duplicates
        /// The OutputCacheAttribute attribute is required in order to prevent
        /// ASP.NET MVC from caching the results of the validation method.
        /// </summary>
        /// <param name="businessEntityRestrictionByRoleViewModel">BusinessEntityRestrictionByRoleViewModel Object</param>
        /// <returns>True or False</returns>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult IsValueDuplicate(BusinessEntityRestrictionByRoleViewModel businessEntityRestrictionByRoleViewModel)
        {
            IBusinessEntityRestrictionByRoleBll iBusinessEntityRestrictionByRoleBll = new BusinessEntityRestrictionByRoleBll();
            var businessEntityRestrictionByRole = iBusinessEntityRestrictionByRoleBll.GetBusinessEntityRestrictionByRoleForValueAndSecurityRolesId(businessEntityRestrictionByRoleViewModel.Value.Trim(), businessEntityRestrictionByRoleViewModel.LnSecurityRolesId, ApplicationGlobals.SelectedSecurityBusinessEntities.Id);

            var isValueDuplicate = (businessEntityRestrictionByRole != null && businessEntityRestrictionByRoleViewModel.Id == businessEntityRestrictionByRole.Id) || businessEntityRestrictionByRole == null;
            if (isValueDuplicate && businessEntityRestrictionByRole == null)
            {
                IBusinessEntityAccessByRoleBll iBusinessEntityAccessByRoleBll = new BusinessEntityAccessByRoleBll();
                var businessEntityAccessByRole = iBusinessEntityAccessByRoleBll.GetBusinessEntityAccessByRoleForValueAndSecurityRolesId(businessEntityRestrictionByRoleViewModel.Value.Trim(), businessEntityRestrictionByRoleViewModel.LnSecurityRolesId, ApplicationGlobals.SelectedSecurityBusinessEntities.Id);

                isValueDuplicate = businessEntityAccessByRole == null;
            }

            return Json(isValueDuplicate, JsonRequestBehavior.AllowGet);
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
        [AuthorizeUser(Operations = new[] { DataOperation.CREATE, DataOperation.UPDATE }, SecurityItemCode = PageId.PAGE_BUSINESSENTITYRESTRICTIONBYROLE)]
        public ActionResult SecurityRolesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            ISecurityRolesBll iSecurityRolesBll = new SecurityRolesBll();
            var securityRolesPage = iSecurityRolesBll.GetSecurityRolesPage(take, skip, sort, filter);

            return Content(JsonConvert.SerializeObject(securityRolesPage));
        }

        /// <summary>
        /// Update
        /// Called when the Submit button is clicked on the Edit Page
        /// </summary>
        /// <param name="businessEntityRestrictionByRoleViewModel">BusinessEntityRestrictionByRoleViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.UPDATE, SecurityItemCode = PageId.PAGE_BUSINESSENTITYRESTRICTIONBYROLE)]
        public JsonResult Update(BusinessEntityRestrictionByRoleViewModel businessEntityRestrictionByRoleViewModel)
        {
            IBusinessEntityRestrictionByRoleBll iBusinessEntityRestrictionByRoleBll = new BusinessEntityRestrictionByRoleBll();
            var bActionPass = ModelState.IsValid && iBusinessEntityRestrictionByRoleBll.UpdateBusinessEntityRestrictionByRole(GetModel(businessEntityRestrictionByRoleViewModel), ApplicationGlobals.UserName);

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
            Reports<BusinessEntityRestrictionByRole> reports;
            switch (reportFormat)
            {
                case ReportFormat.PDF:
                    reports = new PdfReport<BusinessEntityRestrictionByRole>();
                    break;

                case ReportFormat.XLS:
                    reports = new ExcelReport<BusinessEntityRestrictionByRole>();
                    break;

                default:
                    throw new Exception("Invalid Report Format");
            }

            // Set Column Widths
            reports.ColumnWidthInPercent = new float[] { 10, 20, 20, 20, 30 };

            // Set Report Details
            reports.ReportTitle = ResourceStrings.Text_BusinessEntityRestrictionByRole;
            reports.UserName = ApplicationGlobals.UserName;

            reports.ReportFilePath = Server.MapPath(ApplicationGlobals.TempDirectory);
            reports.CompanyLogo = Server.MapPath(ApplicationGlobals.LogoDirectory);

            // Set the Output Fields
            reports.OutputColumns = new[] { "SecurityRolesObj.Code", "SecurityRolesObj.Description", "SecurityRolesObj.CompanyInApplicationObj.ApplicationObj.Description", "SecurityRolesObj.CompanyInApplicationObj.CompanyObj.Description", "Value" };
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
            IBusinessEntityRestrictionByRoleBll iBusinessEntityRestrictionByRoleBll = new BusinessEntityRestrictionByRoleBll();
            reports.ReportData = iBusinessEntityRestrictionByRoleBll.GetBusinessEntityRestrictionByRoleList(sort, filter, ApplicationGlobals.SelectedSecurityBusinessEntities.Id);

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
        /// <param name="businessEntityRestrictionByRoleViewModel">BusinessEntityRestrictionByRoleViewModel Object</param>
        /// <returns>Business Entity</returns>
        private BusinessEntityRestrictionByRole GetModel(BusinessEntityRestrictionByRoleViewModel businessEntityRestrictionByRoleViewModel)
        {
            return MapperInstance.Map<BusinessEntityRestrictionByRole>(businessEntityRestrictionByRoleViewModel);
        }

        /// <summary>
        /// Get View Model
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="dataOperation">Data Operation</param>
        /// <returns>Business Entity</returns>
        private BusinessEntityRestrictionByRoleViewModel GetViewModel(int id, DataOperation dataOperation = 0)
        {
            IBusinessEntityRestrictionByRoleBll iBusinessEntityRestrictionByRoleBll = new BusinessEntityRestrictionByRoleBll();
            var businessEntityRestrictionByRole = iBusinessEntityRestrictionByRoleBll.GetBusinessEntityRestrictionByRoleForId(id);

            var businessEntityRestrictionByRoleViewModel = businessEntityRestrictionByRole == null ?
                new BusinessEntityRestrictionByRoleViewModel() : MapperInstance.Map<BusinessEntityRestrictionByRoleViewModel>(businessEntityRestrictionByRole);

            businessEntityRestrictionByRoleViewModel.LnSecurityBusinessEntitiesId = ApplicationGlobals.SelectedSecurityBusinessEntities.Id;

            businessEntityRestrictionByRoleViewModel.DataAction = dataOperation;
            return businessEntityRestrictionByRoleViewModel;
        }
    }
}