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
    public class BusinessEntityAccessByUserController : Controller
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
                    x.CreateMap<BusinessEntityAccessByUserViewModel, BusinessEntityAccessByUser>();
                    x.CreateMap<BusinessEntityAccessByUserAccounts, BusinessEntityAccessByUserViewModel>()
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
            var businessEntityAccessByUserViewModel = GetViewModel(selectedId, dataOperation);
            return PartialView("MaintainBusinessEntityAccessByUser", businessEntityAccessByUserViewModel);
        }

        /// <summary>
        /// Get BusinessEntityAccessByUser Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>BusinessEntityAccessByUser Page</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_BUSINESSENTITYACCESSBYUSER)]
        public ActionResult BusinessEntityAccessByUserPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            IBusinessEntityAccessByUserBll iBusinessEntityAccessByUserBll = new BusinessEntityAccessByUserBll();
            var businessEntityAccessByUserPage = iBusinessEntityAccessByUserBll.GetBusinessEntityAccessByUserPage(take, skip, sort, filter, ApplicationGlobals.SelectedSecurityBusinessEntities.Id, ApplicationGlobals.IsHideTerminated);

            return Content(JsonConvert.SerializeObject(businessEntityAccessByUserPage));
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="ids">Selected Id's</param>
        /// <returns>True on Success, False on Failure</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.DELETE, SecurityItemCode = PageId.PAGE_BUSINESSENTITYACCESSBYUSER)]
        public JsonResult Delete(int[] ids)
        {
            IBusinessEntityAccessByUserBll iBusinessEntityAccessByUserBll = new BusinessEntityAccessByUserBll();
            var bActionPass = ids.Aggregate(true, (current, id) => current & iBusinessEntityAccessByUserBll.DeleteBusinessEntityAccessByUser(id, ApplicationGlobals.UserName));

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_Delete));
        }

        /// <summary>
        /// Export to PDF
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "BusinessEntityAccessByUserForm", MatchFormValue = "Export to PDF")]
        public ActionResult ExportToPdf()
        {
            GenerateReport(ReportFormat.PDF, ApplicationGlobals.Sort("gridBusinessEntityAccessByUser"), ApplicationGlobals.Filter("gridBusinessEntityAccessByUser"));
            return View("Index");
        }

        /// <summary>
        /// Export to Xls
        /// </summary>
        /// <returns>Index View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [SubmitButton(MatchFormKey = "BusinessEntityAccessByUserForm", MatchFormValue = "Export to XLS")]
        public ActionResult ExportToXls()
        {
            GenerateReport(ReportFormat.XLS, ApplicationGlobals.Sort("gridBusinessEntityAccessByUser"), ApplicationGlobals.Filter("gridBusinessEntityAccessByUser"));
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
        [AuthorizeUser(Operation = DataOperation.ACCESS, SecurityItemCode = PageId.PAGE_BUSINESSENTITYACCESSBYUSER)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Insert
        /// Called when the Submit button is clicked on the Insert Page
        /// </summary>
        /// <param name="businessEntityAccessByUserViewModel">BusinessEntityAccessByUserViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.CREATE, SecurityItemCode = PageId.PAGE_BUSINESSENTITYACCESSBYUSER)]
        public JsonResult Insert(BusinessEntityAccessByUserViewModel businessEntityAccessByUserViewModel)
        {
            IBusinessEntityAccessByUserBll iBusinessEntityAccessByUserBll = new BusinessEntityAccessByUserBll();
            var bActionPass = ModelState.IsValid && iBusinessEntityAccessByUserBll.InsertBusinessEntityAccessByUser(GetModel(businessEntityAccessByUserViewModel), ApplicationGlobals.UserName);

            return Json(string.Format(bActionPass ? ResourceStrings.Message_Success : ResourceStrings.Message_Fail, ResourceStrings.Text_New_Record_Insert));
        }

        /// <summary>
        /// Check for Value Duplicates
        /// The OutputCacheAttribute attribute is required in order to prevent
        /// ASP.NET MVC from caching the results of the validation method.
        /// </summary>
        /// <param name="businessEntityAccessByUserViewModel">BusinessEntityAccessByUserViewModel Object</param>
        /// <returns>True or False</returns>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult IsValueDuplicate(BusinessEntityAccessByUserViewModel businessEntityAccessByUserViewModel)
        {
            IBusinessEntityAccessByUserBll iBusinessEntityAccessByUserBll = new BusinessEntityAccessByUserBll();
            var businessEntityAccessByUser = iBusinessEntityAccessByUserBll.GetBusinessEntityAccessByUserForValueAndUserAccountId(businessEntityAccessByUserViewModel.Value.Trim(), businessEntityAccessByUserViewModel.LnUserAccountId, ApplicationGlobals.SelectedSecurityBusinessEntities.Id);

            var isValueDuplicate = (businessEntityAccessByUser != null && businessEntityAccessByUserViewModel.Id == businessEntityAccessByUser.Id) || businessEntityAccessByUser == null;
            if (isValueDuplicate && businessEntityAccessByUser == null)
            {
                IBusinessEntityRestrictionByUserBll iBusinessEntityRestrictionByUserBll = new BusinessEntityRestrictionByUserBll();
                var businessEntityRestrictionByUser = iBusinessEntityRestrictionByUserBll.GetBusinessEntityRestrictionByUserForValueAndUserAccountId(businessEntityAccessByUserViewModel.Value.Trim(), businessEntityAccessByUserViewModel.LnUserAccountId, ApplicationGlobals.SelectedSecurityBusinessEntities.Id);

                isValueDuplicate = businessEntityRestrictionByUser == null;
            }

            return Json(isValueDuplicate, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update
        /// Called when the Submit button is clicked on the Edit Page
        /// </summary>
        /// <param name="businessEntityAccessByUserViewModel">BusinessEntityAccessByUserViewModel Object</param>
        /// <returns>True on Success, False on Failure</returns>
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.UPDATE, SecurityItemCode = PageId.PAGE_BUSINESSENTITYACCESSBYUSER)]
        public JsonResult Update(BusinessEntityAccessByUserViewModel businessEntityAccessByUserViewModel)
        {
            IBusinessEntityAccessByUserBll iBusinessEntityAccessByUserBll = new BusinessEntityAccessByUserBll();
            var bActionPass = ModelState.IsValid && iBusinessEntityAccessByUserBll.UpdateBusinessEntityAccessByUser(GetModel(businessEntityAccessByUserViewModel), ApplicationGlobals.UserName);

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
        [AuthorizeUser(Operations = new[] { DataOperation.CREATE, DataOperation.UPDATE }, SecurityItemCode = PageId.PAGE_BUSINESSENTITYACCESSBYUSER)]
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
            Reports<BusinessEntityAccessByUserAccounts> reports;
            switch (reportFormat)
            {
                case ReportFormat.PDF:
                    reports = new PdfReport<BusinessEntityAccessByUserAccounts>();
                    break;

                case ReportFormat.XLS:
                    reports = new ExcelReport<BusinessEntityAccessByUserAccounts>();
                    break;

                default:
                    throw new Exception("Invalid Report Format");
            }

            // Set Report Details
            reports.ReportTitle = ResourceStrings.Text_BusinessEntityAccessByUser;
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
            IBusinessEntityAccessByUserBll iBusinessEntityAccessByUserBll = new BusinessEntityAccessByUserBll();
            reports.ReportData = iBusinessEntityAccessByUserBll.GetBusinessEntityAccessByUserList(sort, filter, ApplicationGlobals.SelectedSecurityBusinessEntities.Id, ApplicationGlobals.IsHideTerminated);

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
        /// <param name="businessEntityAccessByUserViewModel">BusinessEntityAccessByUserViewModel Object</param>
        /// <returns>Business Entity</returns>
        private BusinessEntityAccessByUser GetModel(BusinessEntityAccessByUserViewModel businessEntityAccessByUserViewModel)
        {
            return MapperInstance.Map<BusinessEntityAccessByUser>(businessEntityAccessByUserViewModel);
        }

        /// <summary>
        /// Get View Model
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="dataOperation">Data Operation</param>
        /// <returns>Business Entity</returns>
        private BusinessEntityAccessByUserViewModel GetViewModel(int id, DataOperation dataOperation = 0)
        {
            IBusinessEntityAccessByUserBll iBusinessEntityAccessByUserBll = new BusinessEntityAccessByUserBll();
            var businessEntityAccessByUser = iBusinessEntityAccessByUserBll.GetBusinessEntityAccessByUserForId(id);

            var businessEntityAccessByUserViewModel = businessEntityAccessByUser == null ?
                new BusinessEntityAccessByUserViewModel() : MapperInstance.Map<BusinessEntityAccessByUserViewModel>(businessEntityAccessByUser);

            businessEntityAccessByUserViewModel.LnSecurityBusinessEntitiesId = ApplicationGlobals.SelectedSecurityBusinessEntities.Id;

            businessEntityAccessByUserViewModel.DataAction = dataOperation;
            return businessEntityAccessByUserViewModel;
        }
    }
}