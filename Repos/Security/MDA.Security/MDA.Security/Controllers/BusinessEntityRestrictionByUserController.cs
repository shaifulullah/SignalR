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
    public class BusinessEntityRestrictionByUserController : Controller
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
                    x.CreateMap<BusinessEntityRestrictionByUserViewModel, BusinessEntityRestrictionByUser>();
                    x.CreateMap<BusinessEntityRestrictionByUserAccounts, BusinessEntityRestrictionByUserViewModel>()
                        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                        .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));
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
            var businessEntityRestrictionByUserViewModel = GetViewModel(selectedId, dataOperation);
            return PartialView("MaintainBusinessEntityRestrictionByUser", businessEntityRestrictionByUserViewModel);
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByUser Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>BusinessEntityRestrictionByUser Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_BUSINESSENTITYRESTRICTIONBYUSER)]
        public ActionResult BusinessEntityRestrictionByUserPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            IBusinessEntityRestrictionByUserBll iBusinessEntityRestrictionByUserBll = new BusinessEntityRestrictionByUserBll();
            var businessEntityRestrictionByUserPage = iBusinessEntityRestrictionByUserBll.GetBusinessEntityRestrictionByUserPage(take, skip, sort, filter, ApplicationGlobals.SelectedSecurityBusinessEntities.Id, ApplicationGlobals.IsHideTerminated);

            return Content(JsonConvert.SerializeObject(businessEntityRestrictionByUserPage));
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="ids">Selected Id's</param>
        /// <returns>True on Success, False on Failure</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.DELETE, SecurityItemCode = PageId.PAGE_BUSINESSENTITYRESTRICTIONBYUSER)]
        public JsonResult Delete(int[] ids)
        {
            IBusinessEntityRestrictionByUserBll iBusinessEntityRestrictionByUserBll = new BusinessEntityRestrictionByUserBll();
            var bActionPass = ids.Aggregate(true, (current, id) => current & iBusinessEntityRestrictionByUserBll.DeleteBusinessEntityRestrictionByUser(id, ApplicationGlobals.UserName));

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Delete));
        }

        /// <summary>
        /// Export to PDF
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "BusinessEntityRestrictionByUserForm", MatchFormValue = "Export to PDF")]
        public ActionResult ExportToPdf()
        {
            GenerateReport(ReportFormat.PDF, ApplicationGlobals.Sort("gridBusinessEntityRestrictionByUser"), ApplicationGlobals.Filter("gridBusinessEntityRestrictionByUser"));
            return View("Index");
        }

        /// <summary>
        /// Export to Xls
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "BusinessEntityRestrictionByUserForm", MatchFormValue = "Export to XLS")]
        public ActionResult ExportToXls()
        {
            GenerateReport(ReportFormat.XLS, ApplicationGlobals.Sort("gridBusinessEntityRestrictionByUser"), ApplicationGlobals.Filter("gridBusinessEntityRestrictionByUser"));
            return View("Index");
        }

        /// <summary>
        /// Get UserAccount Data
        /// </summary>
        /// <param name="id">Selected Id</param>
        /// <returns>UserAccount Data</returns>
        public JsonResult GetUserAccountData(int id)
        {
            IUserAccountBll iUserAccountBll = new UserAccountBll();
            var userAccount = iUserAccountBll.GetUserAccountForId(id);

            return Json(new { UserName = userAccount.UserName, FullName = userAccount.FullName }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Index Action
        /// </summary>
        /// <returns>Index View</returns>
        [IsNewSession]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_BUSINESSENTITYRESTRICTIONBYUSER)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Insert
        /// Called when the Submit button is clicked on the Insert Page
        /// </summary>
        /// <param name="businessEntityRestrictionByUserViewModel">BusinessEntityRestrictionByUserViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_BUSINESSENTITYRESTRICTIONBYUSER)]
        public JsonResult Insert(BusinessEntityRestrictionByUserViewModel businessEntityRestrictionByUserViewModel)
        {
            IBusinessEntityRestrictionByUserBll iBusinessEntityRestrictionByUserBll = new BusinessEntityRestrictionByUserBll();
            var bActionPass = ModelState.IsValid && iBusinessEntityRestrictionByUserBll.InsertBusinessEntityRestrictionByUser(GetModel(businessEntityRestrictionByUserViewModel), ApplicationGlobals.UserName);

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_New_Record_Insert));
        }

        /// <summary>
        /// Check for Value Duplicates
        /// The OutputCacheAttribute attribute is required in order to prevent
        /// ASP.NET MVC from caching the results of the validation method.
        /// </summary>
        /// <param name="businessEntityRestrictionByUserViewModel">BusinessEntityRestrictionByUserViewModel Object</param>
        /// <returns>True or False</returns>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult IsValueDuplicate(BusinessEntityRestrictionByUserViewModel businessEntityRestrictionByUserViewModel)
        {
            IBusinessEntityRestrictionByUserBll iBusinessEntityRestrictionByUserBll = new BusinessEntityRestrictionByUserBll();
            var businessEntityRestrictionByUser = iBusinessEntityRestrictionByUserBll.GetBusinessEntityRestrictionByUserForValueAndUserAccountId(businessEntityRestrictionByUserViewModel.Value.Trim(), businessEntityRestrictionByUserViewModel.LnUserAccountId, ApplicationGlobals.SelectedSecurityBusinessEntities.Id);

            var isValueDuplicate = (businessEntityRestrictionByUser != null && businessEntityRestrictionByUserViewModel.Id == businessEntityRestrictionByUser.Id) || businessEntityRestrictionByUser == null;
            if (isValueDuplicate && businessEntityRestrictionByUser == null)
            {
                IBusinessEntityAccessByUserBll iBusinessEntityAccessByUserBll = new BusinessEntityAccessByUserBll();
                var businessEntityAccessByUser = iBusinessEntityAccessByUserBll.GetBusinessEntityAccessByUserForValueAndUserAccountId(businessEntityRestrictionByUserViewModel.Value.Trim(), businessEntityRestrictionByUserViewModel.LnUserAccountId, ApplicationGlobals.SelectedSecurityBusinessEntities.Id);

                isValueDuplicate = businessEntityAccessByUser == null;
            }

            return Json(isValueDuplicate, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update
        /// Called when the Submit button is clicked on the Edit Page
        /// </summary>
        /// <param name="businessEntityRestrictionByUserViewModel">BusinessEntityRestrictionByUserViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.UPDATE, SecurityItemCode = PageId.PAGE_BUSINESSENTITYRESTRICTIONBYUSER)]
        public JsonResult Update(BusinessEntityRestrictionByUserViewModel businessEntityRestrictionByUserViewModel)
        {
            IBusinessEntityRestrictionByUserBll iBusinessEntityRestrictionByUserBll = new BusinessEntityRestrictionByUserBll();
            var bActionPass = ModelState.IsValid && iBusinessEntityRestrictionByUserBll.UpdateBusinessEntityRestrictionByUser(GetModel(businessEntityRestrictionByUserViewModel), ApplicationGlobals.UserName);

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Edit_View));
        }

        /// <summary>
        /// Get UserAccountDetails Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>UserAccountDetails Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operations = new[] { DataOperation.CREATE, DataOperation.UPDATE }, SecurityItemCode = PageId.PAGE_BUSINESSENTITYRESTRICTIONBYUSER)]
        public ActionResult UserAccountDetailsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            IUserAccountBll iUserAccountBll = new UserAccountBll();
            var userAccountDetailsPage = iUserAccountBll.GetUserAccountDetailsPage(take, skip, sort, filter, ApplicationGlobals.IsHideTerminated);

            return Content(JsonConvert.SerializeObject(userAccountDetailsPage));
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
            Reports<BusinessEntityRestrictionByUserAccounts> reports;
            switch (reportFormat)
            {
                case ReportFormat.PDF:
                    reports = new PdfReport<BusinessEntityRestrictionByUserAccounts>();
                    break;

                case ReportFormat.XLS:
                    reports = new ExcelReport<BusinessEntityRestrictionByUserAccounts>();
                    break;

                default:
                    throw new Exception("Invalid Report Format");
            }

            // Set Report Details
            reports.ReportTitle = ResourceStrings.Text_BusinessEntityRestrictionByUser;
            reports.UserName = ApplicationGlobals.UserName;

            reports.ReportFilePath = Server.MapPath(ApplicationGlobals.TempDirectory);
            reports.CompanyLogo = Server.MapPath(ApplicationGlobals.LogoDirectory);

            // Set the Output Fields
            reports.OutputColumns = new[] { "UserName", "FullName", "Value" };
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
            IBusinessEntityRestrictionByUserBll iBusinessEntityRestrictionByUserBll = new BusinessEntityRestrictionByUserBll();
            reports.ReportData = iBusinessEntityRestrictionByUserBll.GetBusinessEntityRestrictionByUserList(sort, filter, ApplicationGlobals.SelectedSecurityBusinessEntities.Id, ApplicationGlobals.IsHideTerminated);

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
        /// <param name="businessEntityRestrictionByUserViewModel">BusinessEntityRestrictionByUserViewModel Object</param>
        /// <returns>Business Entity</returns>
        private BusinessEntityRestrictionByUser GetModel(BusinessEntityRestrictionByUserViewModel businessEntityRestrictionByUserViewModel)
        {
            return MapperInstance.Map<BusinessEntityRestrictionByUser>(businessEntityRestrictionByUserViewModel);
        }

        /// <summary>
        /// Get View Model
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="dataOperation">Data Operation</param>
        /// <returns>Business Entity</returns>
        private BusinessEntityRestrictionByUserViewModel GetViewModel(int id, DataOperation dataOperation = 0)
        {
            IBusinessEntityRestrictionByUserBll iBusinessEntityRestrictionByUserBll = new BusinessEntityRestrictionByUserBll();
            var businessEntityRestrictionByUser = iBusinessEntityRestrictionByUserBll.GetBusinessEntityRestrictionByUserForId(id);

            var businessEntityRestrictionByUserViewModel = businessEntityRestrictionByUser == null ?
                new BusinessEntityRestrictionByUserViewModel() : MapperInstance.Map<BusinessEntityRestrictionByUserViewModel>(businessEntityRestrictionByUser);

            businessEntityRestrictionByUserViewModel.LnSecurityBusinessEntitiesId = ApplicationGlobals.SelectedSecurityBusinessEntities.Id;

            businessEntityRestrictionByUserViewModel.DataAction = dataOperation;
            return businessEntityRestrictionByUserViewModel;
        }
    }
}