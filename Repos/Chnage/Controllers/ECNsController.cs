using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Chnage.Models;
using Chnage.Repository;
using Chnage.Services;
using Chnage.ViewModel;
using Chnage.ViewModel.ECN;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MyECO.Controllers
{
    public class ECNsController : Controller
    {
        #region Variables

        private readonly MyECODBContext _context;
        private readonly IUserRole userRoleRepository;
        private readonly IMiddleTables middleTablesRepository;
        private readonly IAuditLog auditLogRepository;
        private readonly INotificationSender notificationSenderRepository;
        private readonly IUser userRepository;
        private User loggedUser;
        private string controller = "";
        private string action = "";
        private readonly HttpContext _currentContext;
        private bool sendNotificationOnDescriptionChange = false;
        private bool sendNotificationOnApproversChange = false;
        private List<int> UsersToBeNotified = new List<int>();

        private string changedDescriptionECN, ChangeStatusECN;
        List<string> approversChangedECN;
        #endregion

        #region Constructor and Override
        public ECNsController(MyECODBContext context, IUserRole userRole, IUser user, IMiddleTables middleTables, IAuditLog auditLog, INotificationSender notificationSender, IHttpContextAccessor _IHttpContextAccessor)
        {
            _context = context;
            userRoleRepository = userRole;
            middleTablesRepository = middleTables;
            auditLogRepository = auditLog;
            notificationSenderRepository = notificationSender;
            userRepository = user;
            _currentContext = _IHttpContextAccessor.HttpContext;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            controller = this.ControllerContext.RouteData.Values["controller"].ToString();
            action = this.ControllerContext.RouteData.Values["action"].ToString();
            if (!action.ToLower().Contains("print"))
            {
                loggedUser = GetLoggedInUser();
            }
        }
        #endregion

        #region Gets!
        private User GetLoggedInUser()
        {

            bool isGeotab = false;
            string loggedUserName = GetLoggedInUsername();
            string loggedUserEmail = GetLoggedInUserEmail();
            var user = _context.Users.Where(u => u.Email == GetLoggedInUserEmail()).SingleOrDefault(); //simple query to return the User from the db
            if (user == null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    string geotabDomain = "geotab.com";
                    string userEmail = (User.Identity as System.Security.Claims.ClaimsIdentity)?.Claims.ElementAt(2).Value;
                    string[] x = userEmail.Split("@");
                    string userEmailDomain = x[1];
                    isGeotab = userEmailDomain == geotabDomain;
                }
                if (isGeotab)
                {
                    user = new User() { Name = loggedUserName, Email = loggedUserEmail, isActive = true };
                    userRepository.Add(user);
                    _context.SaveChanges();
                }
            }
            return user;
        }

        private string GetLoggedInUserEmail()
        {
            string userEmail = "";
            if (User.Identity.IsAuthenticated)
            {
                userEmail = (User.Identity as System.Security.Claims.ClaimsIdentity)?.Claims.ElementAt(2).Value;
            }
            return userEmail;
        }
        private string GetLoggedInUsername()
        {
            string name = "";
            if (User.Identity.IsAuthenticated)
            {
                name = (User.Identity as System.Security.Claims.ClaimsIdentity)?.Claims.ElementAt(1).Value;
            }
            return name;
        }
        #endregion

        #region Checks
        public bool CheckLoggedUserIsApprover(int ECNId)
        {
            UserRoleECN userrole = null;
            if (loggedUser != null)
            {
                userrole = middleTablesRepository.GetExistingUserRoleECNByUser(loggedUser.Id, ECNId);
            }
            if (userrole != null)
            {
                if (userrole.UserRole.RoleInt == Role.Approver)
                {
                    if (userrole.Approval == null)
                    {
                        return true;
                    }
                }
                return false;
            }
            return false;
        }
        public bool CheckLoggedUserIsOriginator(int ECNId)
        {
            var originatorId = _context.ECNs.Where(e => e.Id == ECNId).Select(a => a.OriginatorId).FirstOrDefault();
            if (loggedUser != null)
            {
                if (loggedUser.Id == originatorId)
                {
                    return true;
                }
            }
            return false;
        }

        private bool ECNExists(int id)
        {
            return _context.ECNs.Any(e => e.Id == id);
        }

        private bool CheckIfECNWasRejected(int id)
        {
            bool rejected = false;
            var isRejected = _context.ECNs.Where(ecn => ecn.Id == id).Select(r => r.Status).FirstOrDefault();
            if (isRejected == StatusOptions.RejectedApproval)
            {
                rejected = true;
            }
            return rejected;
        }

        public bool CheckIfUserInNotificationList(ECN ecn)
        {
            bool isUserInNotification = false;
            var loggedInUser = GetLoggedInUser();
            if (loggedInUser != null && ecn.Notifications != null)
            {
                foreach (var user in ecn.Notifications)
                {
                    if (user.UserId == loggedInUser.Id)
                    {
                        isUserInNotification = true;
                    }

                }
            }
            return isUserInNotification;
        }
        #endregion

        #region Details
        // GET: ECNs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var eCN = await _context.ECNs
                    .Include(e => e.ChangeType)
                    .Include(e => e.Originator)
                    .Include(e => e.RelatedECOs)
                        .ThenInclude(r => r.ECO)
                            .ThenInclude(e => e.ChangeType)
                    .Include(e => e.RelatedECOs)
                        .ThenInclude(r => r.ECO.Originator)
                    .Include(e => e.Notifications)
                        .ThenInclude(n => n.User)
                    .Include(e => e.Approvers)
                        .ThenInclude(a => a.UserRole)
                            .ThenInclude(u => u.User)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (eCN == null)
                {
                    return NotFound();
                }

                ViewData["LoggedUserIsOriginator"] = CheckLoggedUserIsOriginator(eCN.Id);
                ViewData["LoggedUserIsApprover"] = CheckLoggedUserIsApprover(eCN.Id);
                ViewData["ECNWasRejected"] = CheckIfECNWasRejected(eCN.Id);
                ViewData["IsUserInNotificatonList"] = CheckIfUserInNotificationList(eCN);

                if (!string.IsNullOrWhiteSpace(TempData["ReminderEmailConfirmation"] as string))
                {
                    ViewData["ReminderEmailConfirmation"] = TempData["ReminderEmailConfirmation"];
                }
                List<ECO> relatedECOs = new List<ECO>();
                foreach (var ecnHasEco in eCN.RelatedECOs)
                {
                    relatedECOs.Add(ecnHasEco.ECO);
                }
                ViewData["ECOs"] = relatedECOs;

                List<Audit> logsFromAuditTable = new List<Audit>();
                var logId = eCN.Id;


                #region Audit - Get ECN Table

                var logList = _context.Audits.Where(e => e.TableName == "ECNs" && e.EntityId == logId.ToString()).ToList()
                            .Where(g => g.OldData != g.NewData);

                foreach (var log in logList)
                {
                    if (log.TableName == "ECNs")
                    {
                        if (log.OldData == null && log.NewData != null)
                        {
                            List<string> ecoLog = new List<string>();
                            var newData = JsonConvert.DeserializeObject<ECN>(log.NewData);


                            if (newData.ClosedDate != null)
                            {
                                ecoLog.Add("ClosedDate:" + newData.ClosedDate.ToString());
                            }
                            else
                            { ecoLog.Add("ClosedDate:null"); }

                            if (newData.ChangeTypeId != -1)
                            {
                                var changedType = _context.RequestTypes.Where(i => i.Id == newData.ChangeTypeId).Select(i => i.Name).FirstOrDefault();
                                ecoLog.Add("ChangeType:" + changedType);
                            }
                            else
                            { ecoLog.Add("ChangeType:null"); }

                            if (newData.CreatedDate != null)
                            {
                                ecoLog.Add("CreatedDate:" + newData.CreatedDate.ToString());
                            }
                            else
                            { ecoLog.Add("CreatedDate:null"); }

                            if (newData.CurrentFirmwareVersion != null)
                            {
                                ecoLog.Add("CurrentFirmwareVersion:" + newData.CurrentFirmwareVersion.ToString());
                            }
                            else
                            { ecoLog.Add("CurrentFirmwareVersion:null"); }

                            if (newData.DateOfNotice != null)
                            {
                                ecoLog.Add("DateOfNotice:" + newData.DateOfNotice.ToString());
                            }
                            else
                            { ecoLog.Add("DateOfNotice:null"); }

                            if (newData.Description != null)
                            {
                                ecoLog.Add("Description:" + newData.CreatedDate.ToString());
                            }
                            else
                            { ecoLog.Add("Description:null"); }

                            if (newData.ImpactMissingReqApprovalDate != null)
                            {
                                ecoLog.Add("ImpactMissingReqApprovalDate:" + newData.ImpactMissingReqApprovalDate.ToString());
                            }
                            else
                            { ecoLog.Add("ImpactMissingReqApprovalDate:null"); }

                            if (newData.ModelName != null)
                            {
                                ecoLog.Add("ModelName:" + newData.ModelName.ToString());
                            }
                            else
                            { ecoLog.Add("ModelName:null"); }

                            if (newData.ModelNumber != null)
                            {
                                ecoLog.Add("ModelNumber:" + newData.ModelNumber.ToString());
                            }
                            else
                            { ecoLog.Add("ModelNumber:null"); }

                            if (newData.NewFirmwareVersion != null)
                            {
                                ecoLog.Add("NewFirmwareVersion:" + newData.NewFirmwareVersion.ToString());
                            }
                            else
                            { ecoLog.Add("NewFirmwareVersion:null"); }

                            if (newData.OriginatorId != -1)
                            {
                                var originatorName = _context.Users.Where(u => u.Id == newData.OriginatorId).Select(u => u.Name).FirstOrDefault();
                                ecoLog.Add("OriginatorName:" + originatorName);
                            }
                            else
                            { ecoLog.Add("OriginatorName:null"); }

                            if (newData.PTCRBResubmissionRequired)
                            {
                                ecoLog.Add("PTCRBResubmissionRequired:true");
                            }
                            else
                            { ecoLog.Add("PTCRBResubmissionRequired:false"); }

                            if (newData.RejectReason != null)
                            {
                                ecoLog.Add("RejectReason:" + newData.RejectReason.ToString());
                            }
                            else
                            { ecoLog.Add("RejectReason:null"); }

                            if (newData.Status != null)
                            {
                                ecoLog.Add("Status:" + newData.Status.ToString());
                            }

                            log.NewData = string.Join("<br/>", ecoLog);

                        }
                        if (log.OldData != null && log.NewData != null)
                        {

                            JObject sourceJObject = JsonConvert.DeserializeObject<JObject>(log.OldData);
                            JObject targetJObject = JsonConvert.DeserializeObject<JObject>(log.NewData);
                            var oldData = new System.Text.StringBuilder();
                            var newData = new System.Text.StringBuilder();

                            if (!JToken.DeepEquals(sourceJObject, targetJObject))
                            {
                                foreach (KeyValuePair<string, JToken> sourceProperty in sourceJObject)
                                {
                                    JProperty targetProp = targetJObject.Property(sourceProperty.Key);

                                    if (!JToken.DeepEquals(sourceProperty.Value, targetProp.Value))
                                    {
                                        var oldValue = sourceProperty.Value;
                                        oldData.Append(oldValue.Parent + " <br/>");
                                        var newValue = targetProp.Value;
                                        newData.Append(newValue.Parent + " <br/>");
                                    }
                                }
                            }
                            log.OldData = oldData.ToString();
                            log.NewData = newData.ToString();
                        }
                        log.TableName = "ECN";
                    }

                    logsFromAuditTable.Add(log);
                }
                #endregion

                #region Audit - Get Related ECO

                var relatedAuditECOs = _context.Audits.Where(e => e.TableName == "ECNHasECOs" && e.EntityId == logId.ToString()).ToList()
                .Where(g => g.OldData != g.NewData).GroupBy(g => new
                {
                    g.EntityId,
                    Date = g.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss"),
                    g.Action
                }).Select(g => new { key = g.Key, value = g.ToList() });

                foreach (var relatedAuditECO in relatedAuditECOs)
                {
                    foreach (var innerValue in relatedAuditECO.value)
                    {
                        if (innerValue.OldData != null)
                        {
                            var oldData = JsonConvert.DeserializeObject<ECNHasECO>(innerValue.OldData);
                            var ecoName = _context.ECOs.Where(e => e.Id == oldData.ECOId).Select(e => e.Description).FirstOrDefault();
                            innerValue.OldData = ecoName;
                        }
                        if (innerValue.NewData != null)
                        {
                            var newData = JsonConvert.DeserializeObject<ECNHasECO>(innerValue.NewData);
                            var ecoName = _context.ECOs.Where(e => e.Id == newData.ECOId).Select(e => e.Description).FirstOrDefault();
                            innerValue.NewData = ecoName;
                        }
                    }
                    var relatedECOAudit = new Audit()
                    {
                        UserName = relatedAuditECO.value.FirstOrDefault().UserName,
                        Action = relatedAuditECO.value.FirstOrDefault().Action,
                        TableName = "Related ECO(s)",
                        EntityId = relatedAuditECO.key.EntityId,
                        OldData = string.Join("<br/>", relatedAuditECO.value.ToList().Where(g => g.OldData != null).Select(g => g.OldData)),
                        NewData = string.Join("<br/>", relatedAuditECO.value.ToList().Where(g => g.NewData != null).Select(g => g.NewData)),
                        CreatedOn = DateTime.Parse(relatedAuditECO.key.Date)
                    };
                    logsFromAuditTable.Add(relatedECOAudit);
                }
                #endregion

                #region Audit - Get User Role Table

                var userRoleECNLogList = _context.Audits.Where(e => e.TableName == "UserRoleECNs" && e.EntityId == logId.ToString()).ToList()
                    .Where(g => g.OldData != g.NewData).GroupBy(g => new
                    {
                        g.EntityId,
                        Date = g.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss"),
                        g.Action

                    }).Select(g => new { key = g.Key, value = g.ToList() });
                foreach (var userRoleLog in userRoleECNLogList)
                {
                    foreach (var innerUserRoleLog in userRoleLog.value)
                    {
                        if (innerUserRoleLog.OldData != null)
                        {
                            var oldRequest = JsonConvert.DeserializeObject<UserRoleECN>(innerUserRoleLog.OldData);
                            var userId = _context.UserRoles.Where(u => u.Id == oldRequest.UserRoleId).Select(u => u.UserId).FirstOrDefault();
                            var userName = _context.Users.Where(u => u.Id == userId).Select(u => u.Name).FirstOrDefault();
                            var requestTypeId = _context.UserRoles.Where(u => u.Id == oldRequest.UserRoleId).Select(u => u.RequestTypeId).FirstOrDefault();
                            var affectedArea = _context.RequestTypes.Where(r => r.Id == requestTypeId).Select(r => r.Name).FirstOrDefault();
                            var userRoleType = _context.UserRoles.Where(u => u.Id == oldRequest.UserRoleId).Select(u => u.RoleInt).FirstOrDefault();
                            string approvalStatus;
                            if (oldRequest.Approval == true)
                            {
                                approvalStatus = "Approved";
                            }
                            else if (oldRequest.Approval == false)
                            {
                                approvalStatus = "Rejected";
                            }
                            else
                            {
                                approvalStatus = "Waiting";
                            }
                            var stringBuils = (userName + "-" + userRoleType + " for " + affectedArea + " ; " + "Approval Status: " + approvalStatus);
                            innerUserRoleLog.OldData = stringBuils;
                        }
                        if (innerUserRoleLog.NewData != null)
                        {
                            var newRequest = JsonConvert.DeserializeObject<UserRoleECN>(innerUserRoleLog.NewData);
                            var userId = _context.UserRoles.Where(u => u.Id == newRequest.UserRoleId).Select(u => u.UserId).FirstOrDefault();
                            var userName = _context.Users.Where(u => u.Id == userId).Select(u => u.Name).FirstOrDefault();
                            var requestTypeId = _context.UserRoles.Where(u => u.Id == newRequest.UserRoleId).Select(u => u.RequestTypeId).FirstOrDefault();
                            var affectedArea = _context.RequestTypes.Where(r => r.Id == requestTypeId).Select(r => r.Name).FirstOrDefault();
                            var userRoleType = _context.UserRoles.Where(u => u.Id == newRequest.UserRoleId).Select(u => u.RoleInt).FirstOrDefault();
                            string approvalStatus;
                            if (newRequest.Approval == true)
                            {
                                approvalStatus = "Approved";
                            }
                            else if (newRequest.Approval == false)
                            {
                                approvalStatus = "Rejected";
                            }
                            else
                            {
                                approvalStatus = "Waiting";
                            }
                            var stringBuild = (userName + "-" + userRoleType + " for " + affectedArea + " ; " + "Approval Status: " + approvalStatus);
                            innerUserRoleLog.NewData = stringBuild;
                        }
                    }
                    var UserRoleAudit = new Audit()
                    {
                        UserName = userRoleLog.value.FirstOrDefault().UserName,
                        Action = userRoleLog.value.FirstOrDefault().Action,
                        TableName = "Approver",
                        EntityId = userRoleLog.key.EntityId,
                        OldData = string.Join("<br/>", userRoleLog.value.ToList().Where(g => g.OldData != null).Select(g => g.OldData)),
                        NewData = string.Join("<br/>", userRoleLog.value.ToList().Where(g => g.NewData != null).Select(g => g.NewData)),
                        CreatedOn = DateTime.Parse(userRoleLog.key.Date)
                    };
                    logsFromAuditTable.Add(UserRoleAudit);
                }

                #endregion

                #region Audit - Get Notification List
                var notofications = _context.Notifications.Where(n => n.ECNId == logId).Select(x => x.Id.ToString()).ToList();
                List<Audit> notificationLog = new List<Audit>();
                var notificationList = _context.Audits.Where(x => notofications.Contains(x.EntityId) && x.TableName == "Notifications").ToList();

                foreach (var notification in notificationList)
                {

                    if (notification.NewData != null)
                    {
                        var newNotification = JsonConvert.DeserializeObject<Notifications>(notification.NewData);
                        var userName = _context.Users.Where(u => u.Id == newNotification.UserId).Select(u => u.Name).FirstOrDefault().ToString();
                        notification.NewData = userName;
                    }

                    if (notification.OldData != null)
                    {
                        var oldNotification = JsonConvert.DeserializeObject<Notifications>(notification.OldData);
                        var oldUserName = _context.Users.Where(u => u.Id == oldNotification.UserId).Select(u => u.Name).FirstOrDefault().ToString();
                        notification.OldData = oldUserName;
                    }

                    notificationLog.Add(notification);
                }
                var groupNotifications = notificationLog.GroupBy(g => new { Date = g.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss"), g.Action }).ToList();
                List<Audit> notificationForAudit = new List<Audit>();
                foreach (var groupNotification in groupNotifications)
                {
                    Audit notification = new Audit();
                    notification.CreatedOn = DateTime.Parse(groupNotification.Key.Date);
                    notification.UserName = groupNotification.FirstOrDefault().UserName;
                    notification.Action = groupNotification.Key.Action;
                    notification.OldData = string.Join("<br/>", groupNotification.Select(x => x.OldData));
                    notification.NewData = string.Join("<br/>", groupNotification.Select(x => x.NewData));
                    notification.TableName = groupNotification.FirstOrDefault().TableName;
                    notificationForAudit.Add(notification);
                }

                //logList = logList.Concat(notificationForAudit).OrderBy(e=>e.Id);
                logsFromAuditTable = logsFromAuditTable.Concat(notificationForAudit).ToList();

                #endregion


                if (logsFromAuditTable.Count == 0)
                {
                    List<AuditLog> auditLogs = new List<AuditLog>();
                    foreach (var log in auditLogRepository.GetAuditLogsECO(eCN.Id)) //sets a list of AuditLogs for the Partial
                    {
                        auditLogs.Add(log);
                    }
                    ViewData["AuditLogECN"] = auditLogs;
                }
                else
                {
                    ViewData["AuditLog"] = logsFromAuditTable;
                }

                return View(eCN);
            }
            catch (Exception ex)
            {
                ViewModelError vm = SetViewModelError(ex);
                return RedirectToAction("ErrorPage", "ErrorHandler", vm);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details([Bind("ApproverOption, UserRoleId, ChangeId, RejectReason")] ApproverViewModel vm)
        {
            try
            {

                UserRoleECN existingUserRoleECN = middleTablesRepository.GetExistingUserRoleECN(vm.UserRoleId, vm.ChangeId);
                existingUserRoleECN.Approval = vm.ApproverOption;
                existingUserRoleECN.AprovedDate = DateTime.Now.ToString();
                _context.Update(existingUserRoleECN);
                await _context.SaveChangesAsync();

                List<UserRoleECN> Approvers = middleTablesRepository.GetUserRolesForECN(vm.ChangeId).ToList();

                ECN currentECN = _context.ECNs.Where(c => c.Id == vm.ChangeId).FirstOrDefault();
                List<string> changes = new List<string>();
                StatusOptions currentStatus = currentECN.Status;
                currentECN.RejectReason = vm.RejectReason;
                if (Approvers.Any(a => a.Approval == null))
                {
                    currentECN.Status = StatusOptions.AwaitingApproval;
                }
                else if (Approvers.TrueForAll(a => a.Approval == false))
                {
                    currentECN.Status = StatusOptions.RejectedApproval;
                }
                else if (Approvers.TrueForAll(a => a.Approval == true))
                {
                    currentECN.Status = StatusOptions.Approved;
                    currentECN.ClosedDate = DateTime.Now;
                }

                if (currentECN.Status == StatusOptions.Approved)
                {
                    changes.Add($"ECN {currentECN.Id} has been approved.");
                    notificationSenderRepository.SendNotificationOnApprovalECN(currentECN.Id, $"ECN {currentECN.Id} has been Approved");
                }
                if (currentStatus != currentECN.Status)
                {
                    ChangeStatusECN = $"Current Status : {currentStatus} ; New Status : {currentECN.Status}";
                    changes.Add($"ECN status has changed - {ChangeStatusECN}");
                    notificationSenderRepository.SendNotificationOnStatusChangeECN(currentECN.Id, ChangeStatusECN);
                }

                notificationSenderRepository.SendNotificationOnAnyChangeECN(currentECN.Id, changes);

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = vm.ChangeId });
            }
            catch (Exception ex)
            {
                ViewModelError vmError = SetViewModelError(ex);
                return RedirectToAction("ErrorPage", "ErrorHandler", vmError);
            }
        }
        #endregion

        #region SetViewModel
        public ViewModelError SetViewModelError(Exception Ex)
        {
            ViewModelError viewModelError = new ViewModelError();

            viewModelError.ErrorId = Ex.HResult;
            viewModelError.ExceptionMessage = Ex.Message;
            viewModelError.ExceptionStackTrace = Ex.StackTrace;
            viewModelError.Email = "shaifulullah@gmail.com";
            viewModelError.ExceptionTypeString = Ex.GetType().FullName;
            viewModelError.ExceptionController = controller;
            viewModelError.ExceptionAction = action;
            if (loggedUser != null)
            {
                viewModelError.User = loggedUser.Email;
            }

            if (Ex.InnerException != null)
            {
                viewModelError.InnerExceptionMessage = Ex.InnerException.Message;
                viewModelError.InnerExceptionType = Ex.InnerException.GetType().FullName;
                viewModelError.InnerToString = Ex.InnerException.ToString();
            }
            return viewModelError;
        }
        //public ECNViewModel SetViewModel()
        //{
        //    ECNViewModel vm = new ECNViewModel(); //new viewmodel
        //    List<RequestType> allRequest = _context.RequestTypes.ToList(); //gets all possible request types

        //    List<SelectList> approversList = new List<SelectList>();
        //    foreach (var request in allRequest)
        //    {
        //        IQueryable<UserRole> Approvers = userRoleRepository.GetApproversWithType(request.Id, loggedUser.Id); //get the available approvers for the request, but not the logged user or any inactive users
        //        SelectListGroup RequestGroup = new SelectListGroup { Name = request.Name }; //the name of the Group they are a part of 
        //        List<SelectListItem> approvers = new List<SelectListItem>();
        //        approvers.AddRange(Approvers.Select(item => new SelectListItem() //adds the collection of Approvers to a List of SelectListItems
        //        {
        //            Text = item.User.Name,
        //            Value = item.Id.ToString(),
        //            Group = RequestGroup
        //        }).OrderBy(a => a.Group.Name).ToList());
        //        SelectList ddl = new SelectList(approvers, "Value", "Text", -1, "Group.Name"); //use the items to generate a SelectList for the <select> element
        //        approversList.Add(ddl);
        //    }
        //    vm.ApproversList = approversList; //ApproversList is a List<SelectList> just made ^ (fresh out of the foreach)

        //    return vm;
        //}
        #endregion

        #region Lists
        public List<UserRoleECN> SetUserRoleECN(int[] ApproversList, int ECNId)
        {
            List<UserRoleECN> returnList = new List<UserRoleECN>();
            if (ApproversList != null && ApproversList.Length > 0)
            {
                foreach (var userRoleId in ApproversList)
                {
                    if (userRoleId != 0)
                    {
                        var userRole = middleTablesRepository.GetExistingUserRoleECN(userRoleId, ECNId);
                        if (userRole != null)
                        {
                            userRole.Approval = null;
                            returnList.Add(userRole);
                        }
                        else
                        {
                            UserRole ExistingUserRole = userRoleRepository.GetUserRole(userRoleId);
                            UserRoleECN userRoleECN = new UserRoleECN() { ECNId = ECNId, UserRoleId = userRoleId, Approval = null, UserRole = ExistingUserRole };
                            returnList.Add(userRoleECN);
                        }
                    }
                }
            }
            return returnList;
        }
        public List<SelectList> SetListSelectListApprovers()
        {
            var RequestTypes = _context.RequestTypes.ToArray();
            List<SelectList> ListApprovers = new List<SelectList>();
            foreach (var request in RequestTypes)
            {
                var Approvers = userRoleRepository.GetApproversWithType(request.Id, loggedUser.Id);
                Approvers = Approvers.Where(ur => ur.UserId != loggedUser.Id);
                Approvers = Approvers.Where(u => u.User.isActive.Equals(true));
                var RequestGroup = new SelectListGroup { Name = request.Name };
                if (Approvers.Any())
                {
                    var DropDownList = new SelectList(Approvers.Select(item => new SelectListItem
                    {
                        Text = item.User.Name,
                        Value = item.Id.ToString(),
                        Group = RequestGroup
                    }).OrderBy(a => a.Group.Name).ToList(), "Value", "Text", -1, "Group.Name");
                    ListApprovers.Add(DropDownList);
                }
            }
            return ListApprovers;
        }
        public List<SelectList> SetSelectListsForApproversEdit(ECNViewModel vm)
        {
            var allRequest = _context.RequestTypes.ToList();
            List<SelectList> approversList = new List<SelectList>();
            foreach (var request in allRequest)
            {
                var Approvers = userRoleRepository.GetApproversWithType(request.Id, loggedUser.Id);
                Approvers = Approvers.Where(u => u.User.isActive.Equals(true));
                var RequestGroup = new SelectListGroup { Name = request.Name };
                var approvers = new List<SelectListItem>();
                approvers.AddRange(Approvers.Select(item => new SelectListItem
                {
                    Text = item.User.Name,
                    Value = item.Id.ToString(),
                    Group = RequestGroup
                }).OrderBy(a => a.Group.Name).ToList());

                SelectList DropDownList = new SelectList(approvers, "Value", "Text", null, "Group.Name");
                if (vm.Approvers != null)
                {
                    foreach (var selectedApprover in vm.Approvers)
                    {
                        var a = DropDownList.GetEnumerator();
                        while (a.MoveNext())
                        {
                            if (Convert.ToInt64(a.Current.Value) == selectedApprover.UserRoleId)
                            {
                                a.Current.Selected = true;
                            }
                        }
                    }
                }
                approversList.Add(DropDownList);
            }
            return approversList;
        }
        public SelectList SetSelectListForNotifications(ECNViewModel vm)
        {
            var allUsers = _context.Users.Where(u => u.isActive == true).ToList(); //get all users
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var user in allUsers)
            {
                items.Add(new SelectListItem()
                {
                    Text = user.Name,
                    Value = user.Id.ToString()
                });
            }
            SelectList ddl = new SelectList(items, "Value", "Text", null); //create a selectlist from the collection of users
            if (vm.Notifications != null)
            {
                foreach (var existingNotification in vm.Notifications) //if there are already notifications for this change, select them
                {
                    var a = ddl.GetEnumerator();
                    while (a.MoveNext())
                    {
                        if (Convert.ToInt64(a.Current.Value) == existingNotification.UserId)
                        {
                            a.Current.Selected = true;
                        }
                    }
                }
            }
            return ddl;
        }
        /// <summary>
        /// This sets a list of Notifications from the selected users to receive notifications.
        /// </summary>
        /// <param name="vm">the ViewModel that has the selected users</param>
        /// <param name="ECOId">Change Id</param>
        /// <returns>List of Notifications to be added/modified in the db.</returns>
        public List<Notifications> SetNotificationsECN(ECNViewModel vm, int ECNId, int[] ApproversList)
        {
            List<Notifications> returnList = new List<Notifications>();
            bool added = false;
            var notification = middleTablesRepository.GetNotificationByUserAndECN(vm.OriginatorId, ECNId); //if there already is a notification for the user in the db.
            if (notification != null)
            {
                returnList.Add(notification);
            }
            else
            {
                User NotificationUser = _context.Users.Where(u => u.Id == vm.OriginatorId).FirstOrDefault(); //get the User to access it's name later in the sendNotification
                Notifications NewNotification = new Notifications()
                {
                    ECNId = ECNId,
                    UserId = vm.OriginatorId,
                    User = NotificationUser,
                    Option = NotificationOption.AnyChange //the user will always be notified on any change, unless they change it in the main page
                };
                returnList.Add(NewNotification);
            }
            if (vm.UsersToBeNotified != null)
            {
                foreach (var userid in vm.UsersToBeNotified) //for all the users selected to receive notifications
                {
                    added = false;
                    notification = middleTablesRepository.GetNotificationByUserAndECN(userid, ECNId); //if there already is a notification for the user in the db.
                    if (notification != null)
                    {
                        foreach (Notifications nf in returnList)
                        {
                            if (nf.User == notification.User)
                            {
                                added = true;
                                break;
                            }
                        }
                        if (!added)
                        {
                            returnList.Add(notification);
                        }
                    }
                    else
                    {
                        User NotificationUser = _context.Users.Where(u => u.Id == userid).FirstOrDefault(); //get the User to access it's name later in the sendNotification
                        Notifications NewNotification = new Notifications()
                        {
                            ECNId = ECNId,
                            UserId = userid,
                            User = NotificationUser,
                            Option = NotificationOption.AnyChange //the user will always be notified on any change, unless they change it in the main page
                        };
                        foreach (Notifications nf in returnList)
                        {
                            if (nf.User == NewNotification.User)
                            {
                                added = true;
                                break;
                            }
                        }
                        if (!added)
                        {
                            returnList.Add(NewNotification);
                        }
                    }
                }
            }
            if (ApproversList != null)
            {
                foreach (var userRoleId in ApproversList)
                {
                    added = false;
                    var user = userRoleRepository.GetUserByUserRoleId(userRoleId);
                    notification = middleTablesRepository.GetNotificationByUserAndECN(user.Id, ECNId); //if there already is a notification for the user in the db.
                    if (notification != null)
                    {
                        foreach (Notifications nf in returnList)
                        {
                            if (nf.User == notification.User)
                            {
                                added = true;
                                break;
                            }
                        }
                        if (!added)
                        {
                            returnList.Add(notification);
                        }
                    }
                    else
                    {
                        User NotificationUser = _context.Users.Where(u => u.Id == user.Id).FirstOrDefault(); //get the User to access it's name later in the sendNotification
                        Notifications NewNotification = new Notifications()
                        {
                            ECNId = ECNId,
                            UserId = user.Id,
                            User = NotificationUser,
                            Option = NotificationOption.AnyChange //the user will always be notified on any change, unless they change it in the main page
                        };
                        foreach (Notifications nf in returnList)
                        {
                            if (nf.User == NewNotification.User)
                            {
                                added = true;
                                break;
                            }
                        }
                        if (!added)
                        {
                            returnList.Add(NewNotification);
                        }
                    }
                }
            }
            return returnList;
        }

        #endregion

        #region Create
        // GET: ECNs/Create
        public IActionResult Create()
        {
            var users = _context.Users.Where(ur => ur.isActive);
            ViewData["ChangeTypeId"] = new SelectList(_context.RequestTypes, "Id", "Name");
            ViewData["ECNApprovers"] = new SelectList(_context.UserRoles.Where(u => u.RoleInt == Role.ECNApprover), "Id", "Name");
            ViewData["ApproverLists"] = SetListSelectListApprovers();
            ViewData["AllUsers"] = new SelectList(users, "Id", "Name");
            ViewData["OriginatorId"] = new SelectList(_context.Users.Where(u => u == loggedUser), "Id", "Name");
            ViewData["RelatedECOs"] = GetApprovedECOs();
            return View();
        }

        // POST: ECNs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModelName,ModelNumber,DateOfNotice,UsersToBeNotified,ChangeTypeId,PTCRBResubmissionRequired,CurrentFirmwareVersion,NewFirmwareVersion,Description,ImpactMissingReqApprovalDate,Status,OriginatorId, RelatedECOIds, RelatedECOs")] ECNViewModel vmeCN,
            int[] UserRoleIds, string[] ApproversList)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    ECN newECN = new ECN(vmeCN);
                    _context.Add(newECN);
                    await _context.SaveChangesAsync();

                    int ecnId = newECN.Id;
                    List<Notifications> notifications = SetNotificationsECN(vmeCN, ecnId, UserRoleIds);
                    newECN.Notifications = notifications;
                    List<UserRoleECN> approvers = SetUserRoleECN(UserRoleIds, ecnId);
                    newECN.Approvers = approvers;
                    //auditLogRepository.Add(newECN, loggedUser);
                    if (vmeCN.RelatedECOIds != null)
                    {
                        List<ECNHasECO> relatedECOs = SetECNHasECO(vmeCN.RelatedECOIds, ecnId);
                        newECN.RelatedECOs = relatedECOs;
                    }
                    await _context.SaveChangesAsync();
                    if (newECN.Status != StatusOptions.Draft)
                    {
                        List<string> changes = new List<string>();
                        changes.Add($"ECN {ecnId} has been created.");
                        notificationSenderRepository.SendNotificationOnAnyChangeECN(ecnId, changes);
                    }
                    return RedirectToAction(nameof(Details), new { id = ecnId });
                }
                catch (Exception ex)
                {
                    ViewModelError vm = SetViewModelError(ex);
                    return RedirectToAction("ErrorPage", "ErrorHandler", vm);
                }
            }
            ViewData["ApproverLists"] = SetListSelectListApprovers();
            ViewData["ChangeTypeId"] = new SelectList(_context.RequestTypes, "Id", "Name", vmeCN.ChangeTypeId);
            return View(vmeCN);
        }
        #endregion

        #region Public Methods
        public List<ECNHasECO> SetECNHasECO(List<int> ECOIds, int ECNId)
        {
            List<ECNHasECO> returnList = new List<ECNHasECO>();
            if (ECOIds != null)
            {
                foreach (var ECOId in ECOIds)
                {
                    var relatedECO = middleTablesRepository.GetECNHasECORecord(ECOId, ECNId); //if the related ECO is alreay in the db
                    if (relatedECO != null)
                    {
                        returnList.Add(relatedECO);
                    }
                    else
                    {
                        ECNHasECO ECNHasECO = new ECNHasECO() { ECNId = ECNId, ECOId = ECOId };//create a new ECNHasECO
                        returnList.Add(ECNHasECO);
                    }
                }
            }
            return returnList;
        }

        public SelectList GetApprovedECOs()
        {
            List<ECO> approvedECOs = _context.ECOs.Where(eco => eco.Status == StatusOptions.Approved)
                .Include(e => e.ChangeType)
                .Include(e => e.Originator)
                .ToList();

            List<SelectListItem> ecos = new List<SelectListItem>();

            foreach (var item in approvedECOs)
            {
                SelectListGroup group = new SelectListGroup() { Name = item.ChangeType.Name };
                ecos.Add(new SelectListItem()
                {
                    Text = item.Id + " - " + item.Description + " - " + item.Originator.Name,
                    Value = item.Id.ToString(),
                    Group = group
                });
            }
            //List<SelectListItem> ecos = new List<SelectListItem>();
            //for (int i = 0; i < approvedECOs.Count; i++)
            //{
            //    ECO eco = approvedECOs[i];
            //    ecos.Add(new SelectListItem
            //    {
            //        Text = eco.Id + " - " + eco.Description + " - " + eco.Originator.Name,
            //        Value = eco.Id.ToString(),
            //        Group = new SelectListGroup { Name = eco.ChangeType.Name }
            //    });

            //}
            return new SelectList(ecos, "Value", "Text", -1, "Group.Name");
        }

        public SelectList SetSelectListForRelatedECOs(ECNViewModel vm)
        {
            var approvedECOs = _context.ECOs.Where(e => e.Status == StatusOptions.Approved)
                .Include(a => a.Originator); //get a list of ECOs that have been approved

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var item in approvedECOs)
            {
                SelectListGroup group = new SelectListGroup() { Name = item.ChangeType.Name };
                items.Add(new SelectListItem()
                {
                    Text = item.Id + " - " + item.Description + " - " + item.Originator.Name + " - " + item.ClosedDate.Value.ToShortDateString(),
                    Value = item.Id.ToString(),
                    Group = group
                });
            }
            SelectList ddl = new SelectList(items, "Value", "Text", null, "Group.Name");
            IEnumerable<ECNHasECO> relatedECOs = middleTablesRepository.GetRelatedECOByECNId(vm.Id); //gets a collection of ECNHasECO for the current change
            if (relatedECOs != null)
            {
                foreach (var existingECO in relatedECOs) //select the ones that are already in the db
                {
                    var a = ddl.GetEnumerator();
                    while (a.MoveNext())
                    {
                        if (Convert.ToInt64(a.Current.Value) == existingECO.ECOId)
                        {
                            a.Current.Selected = true;
                        }
                    }
                }
            }
            return ddl;
        }
        #endregion

        #region Update Middle Tables
        public void UpdateUserRoleECNTable(ICollection<UserRoleECN> newApprovers, int ECNId)
        {
            List<int> temp = newApprovers.Select(i => i.UserRoleId).ToList();
            var remove = _context.UserRoleECNs.Where(r => !temp.Contains(r.UserRoleId) && r.ECNId == ECNId);
            if (remove != null)
            {
                _context.UserRoleECNs.RemoveRange(remove);
            }
            _context.SaveChanges();
        }
        /// <summary>
        /// Method to update Notification table. Will remove any Notifications that were unselected by User in the View
        /// </summary>
        /// <param name="notifications">List of Notifications selected</param>
        /// <param name="ecnId">Current ECO being updated</param>
        public void UpdateNotificationTable(ICollection<Notifications> notifications, int ecnId)
        {
            List<int> temp = notifications.Select(i => i.Id).ToList();//get a list of the new Ids
            var remove = _context.Notifications.Where(r => !temp.Contains(r.Id) && r.ECNId == ecnId); //if the existing list of Notifications does not contain the ids of the new Notifications, put them in the remove list
            if (remove != null)
            {
                _context.Notifications.RemoveRange(remove);
            }
            _context.SaveChanges();
        }
        #endregion

        #region Edit
        // GET: ECNs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eCN = await _context.ECNs
                .Where(e => e.Id == id)
                .Include(e => e.RelatedECOs)
                .Include(a => a.Approvers)
                    .ThenInclude(a => a.UserRole)
                        .ThenInclude(u => u.User)
                .Include(e => e.Notifications)
                    .ThenInclude(n => n.User)
                .FirstOrDefaultAsync();
            if (eCN == null)
            {
                return NotFound();
            }
            ECNViewModel vm = new ECNViewModel(eCN);
            ViewData["Notifications"] = SetSelectListForNotifications(vm);
            ViewData["ChangeTypeId"] = new SelectList(_context.RequestTypes, "Id", "Name", vm.ChangeTypeId);
            ViewData["ApproverLists"] = SetSelectListsForApproversEdit(vm);
            ViewData["RelatedECOs"] = SetSelectListForRelatedECOs(vm);
            return View(vm);
        }
        // POST: ECNs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ModelName,ModelNumber,DateOfNotice,OriginatorId,ChangeTypeId,PTCRBResubmissionRequired,CurrentFirmwareVersion,NewFirmwareVersion,Description,ImpactMissingReqApprovalDate,UsersToBeNotified,Status,RelatedECOIds")] ECNViewModel vmeCN,
            int[] UserRoleIds, string[] ApproversList)
        {
            if (id != vmeCN.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ECN existingECN = _context.ECNs.Where(e => e.Id == vmeCN.Id)
                        .Include(a => a.Approvers).ThenInclude(b => b.UserRole).ThenInclude(u => u.User)
                        .Include(a => a.Approvers).ThenInclude(b => b.UserRole).ThenInclude(u => u.RequestType)
                        .Include(a => a.Notifications).ThenInclude(n => n.User)
                        .Include(e => e.RelatedECOs).ThenInclude(e => e.ECO).ThenInclude(c => c.ChangeType)
                        .Single();

                    string[] approvers = UserRoleIds.Select(x => x.ToString()).ToArray();

                    List<ECNHasECO> realatedECOListInVm = new List<ECNHasECO>();
                    if (vmeCN.RelatedECOIds != null)
                    {
                        foreach (var relatedECOId in vmeCN.RelatedECOIds)
                        {
                            ECNHasECO relatedECO = _context.ECNHasECOs.Where(e => e.ECOId == relatedECOId).FirstOrDefault();
                            if (relatedECO == null || relatedECO.ECO == null)
                            {
                                var eco = _context.ECOs.Where(e => e.Id == relatedECOId).FirstOrDefault();
                                relatedECO = new ECNHasECO
                                {
                                    ECOId = eco.Id,
                                    ECO = eco,
                                    ECNId = vmeCN.Id,
                                    ECN = existingECN
                                };
                            }

                            realatedECOListInVm.Add(relatedECO);
                        }
                    }
                    vmeCN.ECNHasECOs = realatedECOListInVm;

                    List<string> changes = FindChanges(existingECN, vmeCN, approvers);
                    vmeCN.Approvers = SetUserRoleECN(UserRoleIds, vmeCN.Id);
                    UpdateExistingECN(existingECN, vmeCN, UserRoleIds);
                    UpdateUserRoleECNTable(SetUserRoleECN(UserRoleIds, vmeCN.Id), vmeCN.Id);
                    _context.Update(existingECN);
                    //auditLogRepository.Add(existingECN, loggedUser);

                    await _context.SaveChangesAsync();
                    if (existingECN.Status != StatusOptions.Draft)
                    {
                        if (changes.Count > 0)
                        {
                            notificationSenderRepository.SendNotificationOnAnyChangeECN(vmeCN.Id, changes);
                        }

                        if (sendNotificationOnDescriptionChange)
                        {
                            notificationSenderRepository.SendNotificationOnDescriptionChangeECN(vmeCN.Id, changedDescriptionECN);
                        }
                        if (sendNotificationOnApproversChange)
                        {
                            notificationSenderRepository.SendNotificationOnApproversChangeECN(existingECN.Id, approversChangedECN);
                        }
                        if (existingECN.Status == StatusOptions.Approved)
                        {
                            notificationSenderRepository.SendNotificationOnApprovalECN(existingECN.Id, ($"{existingECN.Id} has been approved"));
                        }
                    }

                    return RedirectToAction(nameof(Details), new { id = vmeCN.Id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ECNExists(vmeCN.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ViewModelError vm = SetViewModelError(ex);
                    return RedirectToAction("ErrorPage", "ErrorHandler", vm);
                }
            }
            ViewData["ChangeTypeId"] = new SelectList(_context.RequestTypes, "Id", "Name", vmeCN.ChangeTypeId);
            ViewData["ApproverLists"] = SetSelectListsForApproversEdit(vmeCN);
            return View(vmeCN);
        }
        #endregion

        #region Clone Rejected ECN
        public async Task<IActionResult> Clone(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eCN = await _context.ECNs
                .Where(e => e.Id == id)
                .Include(e => e.RelatedECOs)
                .Include(a => a.Approvers)
                    .ThenInclude(a => a.UserRole)
                        .ThenInclude(u => u.User)
                .Include(e => e.Notifications)
                    .ThenInclude(n => n.User)
                .FirstOrDefaultAsync();
            if (eCN == null)
            {
                return NotFound();
            }
            ECNViewModel vm = new ECNViewModel(eCN);
            ViewData["AllUsers"] = SetSelectListForNotifications(vm);
            ViewData["ChangeTypeId"] = new SelectList(_context.RequestTypes, "Id", "Name", vm.ChangeTypeId);
            ViewData["ApproverLists"] = SetSelectListsForApproversEdit(vm);
            ViewData["RelatedECOs"] = SetSelectListForRelatedECOs(vm);
            return View("Create", vm);
        }
        #endregion

        #region FindChangesForNotification
        private List<string> FindChanges(ECN existingECN, ECNViewModel vmECN, string[] ApproversList)
        {
            List<string> changes = new List<string>();

            PropertyInfo[] existingProp = existingECN.GetType().GetProperties();
            PropertyInfo[] newProp = vmECN.GetType().GetProperties();
            foreach (PropertyInfo existprop in existingProp)
            {
                foreach (PropertyInfo nwprop in newProp)
                {
                    var newVal = nwprop.GetValue(vmECN, null);
                    var existVal = existprop.GetValue(existingECN, null);
                    if (existprop.Name == nwprop.Name ||
                        (existprop.Name.ToLower() == "relatedecos" && nwprop.Name.ToLower() == "ecnhasecos"))
                    {
                        if (existprop.Name.ToLower() == "approvers")
                        {
                            var currentApprovers = (ICollection<UserRoleECN>)existVal;
                            approversChangedECN = new List<string>();
                            List<string> existApprovers = new List<string>();
                            List<string> newApprovers = new List<string>();
                            foreach (var approver in currentApprovers)
                            {
                                existApprovers.Add(userRoleRepository.GetUserByUserRoleId(approver.UserRoleId).Name.ToString());
                            }
                            foreach (var approver in ApproversList)
                            {
                                newApprovers.Add(userRoleRepository.GetUserByUserRoleId(Convert.ToInt32(approver)).Name.ToString());
                            }
                            var removed = existApprovers.Except(newApprovers).ToList();
                            var added = newApprovers.Except(existApprovers).ToList();
                            if (newApprovers != existApprovers)
                            {
                                sendNotificationOnApproversChange = true;
                            }
                            string usersAdded = "";
                            string usersRemoved = "";
                            if (added.Count > 0)
                            {
                                foreach (var user in added)
                                {
                                    usersAdded += user + ",";
                                }
                                changes.Add(usersAdded + " have been added as approvers.");
                                approversChangedECN.Add(usersAdded + " have been added as approvers.");
                            }
                            if (removed.Count > 0)
                            {
                                foreach (var user in removed)
                                {
                                    usersRemoved += user + ",";
                                }
                                changes.Add(usersRemoved + " have been removed from approvers.");
                                approversChangedECN.Add(usersRemoved + " have been removed from approvers.");
                            }
                        }
                        else if (existprop.Name.ToLower() == "originator")
                        {
                            continue;
                        }
                        else if (existprop.Name.ToLower() == "relatedecos")
                        {
                            if (nwprop.Name.ToLower() == "ecnhasecos")
                            {
                                if (existVal != null && newVal != null)
                                {
                                    var currentRelatedECOs = (ICollection<ECNHasECO>)existVal;
                                    var newRelatedECOs = (ICollection<ECNHasECO>)newVal;
                                    List<string> existingRelatedECOs = new List<string>();
                                    List<string> latestRelatedECOs = new List<string>();

                                    foreach (var currentRelatedECO in currentRelatedECOs)
                                    {
                                        existingRelatedECOs.Add($"Id- {currentRelatedECO.ECO.Id}; Description- {currentRelatedECO.ECO.Description}");
                                    }
                                    foreach (var newRelatedECO in newRelatedECOs)
                                    {
                                        latestRelatedECOs.Add($"Id- {newRelatedECO.ECO.Id}; Description- {newRelatedECO.ECO.Description}");
                                    }
                                    var removed = existingRelatedECOs.Except(latestRelatedECOs).ToList();
                                    var added = latestRelatedECOs.Except(existingRelatedECOs).ToList();

                                    string ECOsAdded = "";
                                    string ECOsRemoved = "";
                                    if (added.Count > 0)
                                    {
                                        foreach (var ECO in added)
                                        {
                                            ECOsAdded += ECO + ", ";
                                        }
                                        changes.Add("Related ECOs added: " + ECOsAdded + "<br/>");
                                    }
                                    if (removed.Count > 0)
                                    {
                                        foreach (var product in removed)
                                        {
                                            ECOsRemoved += product + ", ";
                                        }
                                        changes.Add("Related ECOs Removed: " + ECOsRemoved + "<br/>");
                                    }
                                }
                                else if (newVal != null && existVal == null)
                                {
                                    var newECOs = (ICollection<ECNHasECO>)newVal;
                                    List<string> newECOList = new List<string>();

                                    foreach (var newECO in newECOs)
                                    {
                                        newECOList.Add(newECO.ECO.Description);
                                    }
                                    var added = newECOList.ToList();
                                    string ECOsAdded = "";
                                    if (added.Count > 0)
                                    {
                                        foreach (var ECO in added)
                                        {
                                            ECOsAdded += ECO + ", ";
                                        }
                                        changes.Add("Related ECOs added: " + ECOsAdded + "<br/>");
                                    }
                                }
                                else if (existVal != null && newVal == null)
                                {
                                    var currentECOs = (ICollection<ECNHasECO>)existVal;
                                    List<string> existECOs = new List<string>();

                                    foreach (var currentECO in currentECOs)
                                    {
                                        existECOs.Add(currentECO.ECO.Description);
                                    }

                                    var removed = existECOs.ToList();
                                    string ECOsRemoved = "";
                                    if (removed.Count > 0)
                                    {
                                        foreach (var product in removed)
                                        {
                                            ECOsRemoved += product + ", ";
                                        }
                                        changes.Add("Related ECOs Removed: " + ECOsRemoved + "<br/>");
                                    }
                                }
                            }
                        }
                        else if (existprop.Name.ToLower() == "notifications")
                        {
                            var currentnotifications = (ICollection<Notifications>)existVal;
                            List<string> existNotifications = new List<string>();
                            List<string> newNotifications = new List<string>();
                            foreach (var user in currentnotifications)
                            {
                                existNotifications.Add(user.User.Name.ToString());
                            }
                            foreach (var user in vmECN.UsersToBeNotified)
                            {
                                newNotifications.Add(userRepository.GetUserFromId(user).Name.ToString());
                            }
                            var removed = existNotifications.Except(newNotifications).ToList();
                            var added = newNotifications.Except(existNotifications).ToList();
                            string usersAdded = "";
                            string usersRemoved = "";
                            if (added.Count > 0)
                            {
                                foreach (var user in added)
                                {
                                    usersAdded += user + ",";
                                }
                                changes.Add(usersAdded + " have been added to recieve Notifications.");
                            }
                            if (removed.Count > 0)
                            {
                                foreach (var user in removed)
                                {
                                    usersRemoved += user + ",";
                                }
                                changes.Add(usersRemoved + " have been removed from receiving Notifications.");
                            }
                        }
                        else if (existprop.Name.ToLower() == "changetype")
                        {
                            if (newVal != null && existVal != null)
                            {
                                if (!newVal.Equals(existVal))
                                {
                                    changes.Add("Current " + existprop.Name.ToString() + " : " + existVal + "; New " + nwprop.Name.ToString() + " : " + newVal);
                                    break;
                                }
                            }
                        }
                        else if (existprop.Name.ToLower() == "createddate")
                        {
                            continue;
                        }
                        else
                        {
                            if (newVal != null && existVal != null)
                            {
                                if (!newVal.Equals(existVal))
                                {
                                    changes.Add("Current " + existprop.Name.ToString() + " : " + existVal + "; New " + nwprop.Name.ToString() + " : " + newVal);
                                    break;
                                }
                            }
                            else if (newVal != null && existVal == null)
                            {
                                changes.Add("New " + nwprop.Name.ToString() + " : " + newVal + " added.");
                                break;
                            }
                            else if (newVal == null && existVal != null)
                            {
                                changes.Add("Current " + existprop.Name.ToString() + " : " + existVal + " removed.");
                                break;
                            }
                        }
                    }
                }
            }
            return changes;
        }
        #endregion

        #region Update Record
        public void UpdateExistingECN(ECN existingECN, ECNViewModel newECN, int[] ApproversList)
        {
            int existingId = existingECN.Id;
            if (existingECN.Description != newECN.Description)
            {
                changedDescriptionECN = $"Current Description : {existingECN.Description} ; New Description : {newECN.Description} ";
                existingECN.Description = newECN.Description;
                sendNotificationOnDescriptionChange = true;
            }
            if (existingECN.Approvers != newECN.Approvers)
            {

            }
            existingECN.ModelName = newECN.ModelName;
            existingECN.ModelNumber = newECN.ModelNumber;
            existingECN.DateOfNotice = newECN.DateOfNotice;
            existingECN.ChangeTypeId = newECN.ChangeTypeId;
            existingECN.PTCRBResubmissionRequired = newECN.PTCRBResubmissionRequired;
            existingECN.CurrentFirmwareVersion = newECN.CurrentFirmwareVersion;
            existingECN.NewFirmwareVersion = newECN.NewFirmwareVersion;
            existingECN.Description = newECN.Description;
            existingECN.ImpactMissingReqApprovalDate = newECN.ImpactMissingReqApprovalDate;
            existingECN.Status = newECN.Status;
            existingECN.Approvers = newECN.Approvers;
            existingECN.RelatedECOs = SetECNHasECO(newECN.RelatedECOIds, existingId);
            existingECN.Notifications = SetNotificationsECN(newECN, existingId, ApproversList);
            UpdateNotificationTable(SetNotificationsECN(newECN, existingId, ApproversList), existingId);
        }
        #endregion

        #region Print
        public IActionResult Print(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var eCN = _context.ECNs
                      .Include(e => e.ChangeType)
                      .Include(e => e.Originator)
                      .Include(e => e.Approvers)
                          .ThenInclude(a => a.UserRole)
                              .ThenInclude(u => u.User)
                      .FirstOrDefault(m => m.Id == id);
                if (eCN == null)
                {
                    return NotFound();
                }

                return View(eCN);
            }
            catch (Exception ex)
            {
                ViewModelError vm = SetViewModelError(ex);
                return RedirectToAction("ErrorPage", "ErrorHandler", vm);
            }
        }
        public FileStreamResult PrintPDF(int? ID)
        {
            // Write True or False to a text file indicating whether the logged in user is Geotab or NOT
            // Unable to read the value in Print() because the view does not have a logged in user
            //(perhaps it is a different session)

            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.MaxPageLoadTime = 120;
            string url = GetBaseUrl()
                + "/ECNs/Print/"
                + ID.ToString();
            PdfDocument doc = converter.ConvertUrl(url);

            byte[] pdfBytes = doc.Save();
            MemoryStream ms = new MemoryStream(pdfBytes);
            string fileName = "ECN" + ID + ".pdf";
            return File(ms, "application/pdf", fileName);
            //return new FileStreamResult(ms, "application/pdf");
        }

        private string GetBaseUrl()
        {
            var request = _currentContext.Request;

            var host = request.Host.ToUriComponent();

            var pathBase = request.PathBase.ToUriComponent();
            var baseUrl = $"{request.Scheme}://{host}{pathBase}";
            return baseUrl;

        }
        #endregion

        #region Send Reminder Email
        public IActionResult ReminderEmail(int id)
        {
            var eCN = _context.ECNs
                .Where(e => e.Id == id)
                .Include(e => e.RelatedECOs)
                .Include(a => a.Approvers)
                    .ThenInclude(a => a.UserRole)
                        .ThenInclude(u => u.User)
                .Include(e => e.Notifications)
                    .ThenInclude(n => n.User)
                .Single();

            string message = $"Please check if there is any action needed for the ECN {id} ";
            notificationSenderRepository.SendReminderEmailECN(id, message);
            TempData["ReminderEmailConfirmation"] = "A reminder email has been sent to everyone associated with this ECN";
            return RedirectToAction("Details", eCN);
        }
        #endregion

        #region Add/Remove To Nofication List
        public async Task<IActionResult> AddToNotification(int id)
        {
            var loggedInUser = GetLoggedInUser();
            var notification = middleTablesRepository.GetNotificationByUserAndECN(loggedInUser.Id, id);

            if (notification == null)
            {
                User NotificationUser = _context.Users.Where(u => u.Id == loggedInUser.Id).FirstOrDefault(); //get the User to access it's name later in the sendNotification
                Notifications NewNotification = new Notifications()
                {
                    ECNId = id,
                    UserId = loggedInUser.Id,
                    User = NotificationUser,
                    Option = NotificationOption.AnyChange //the user will always be notified on any change, unless they change it in the main page
                };
                _context.Add(NewNotification);
                _context.SaveChanges();

                //List<string> message = new List<string>();
                //message.Add($"{loggedInUser.Name} has been added to the notification list");
                //notificationSenderRepository.SendNotificationOnAnyChangeECN(id, message);
            }
            else
            {
                var removeUser = _context.Notifications.Where(u => u.UserId == loggedInUser.Id && u.ECNId == id).FirstOrDefault();
                _context.Notifications.RemoveRange(removeUser);
                _context.SaveChanges();
                //List<string> message = new List<string>();
                //message.Add($"{loggedInUser.Name} has been removed from the notification list");
                //notificationSenderRepository.SendNotificationOnAnyChangeECN(id, message);
            }

            return RedirectToAction(nameof(Details), new { id = id });
        }
        #endregion

    }
}