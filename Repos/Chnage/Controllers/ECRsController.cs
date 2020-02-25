using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Chnage.Helpers;
using Chnage.Models;
using Chnage.Repository;
using Chnage.Services;
using Chnage.ViewModel;
using Chnage.ViewModel.ECR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Controllers
{
    public class ECRsController : Controller
    {
        #region Variables
        private readonly MyECODBContext _context; //context of the database
        private readonly IUserRole userRoleRepository; //repository for UserRole functions
        private readonly IMiddleTables middleTablesRepository; //repository for all middle table functions
        private readonly IAuditLog auditLogRepository; //repository for the AuditLog functions
        private readonly INotificationSender notificationSenderRepository; //repository for the notifications
        private readonly IUser userRepository;
        private readonly HttpContext _currentContext;
        private readonly IProduct productRepository;
        private User loggedUser;
        //bools to check when to send notifications
        private bool sendNotificationOnDescriptionChange = false;
        private bool sendNotificationOnReasonChange = false;
        private bool sendNotificationOnImplementationDateChange = false;
        private bool sendNotificationOnApproversChange = false;
        string action = "";
        string controller = "";
        private string changedDescriptionECR, reasonForChangeECR, implementationDateChangedECR, ChangeStatusECR;
        List<string> approversChangedECR;
        #endregion

        #region Constructor and Override
        /// <summary>
        /// Constructor. Dependency Injection happens with the params.
        /// </summary>
        /// <param name="context">The database context that we want to access</param>
        /// <param name="userRole">Repository for the UserRole functions</param>
        /// <param name="auditLog">Repository for the Audit Log functions</param>
        /// <param name="middleTables">Repository for the Middle Table functions</param>
        /// <param name="notificationSender">Repository for the Notifications functions</param>
        public ECRsController(MyECODBContext context, IUserRole userRole, IUser user, IMiddleTables middleTables, IAuditLog auditLog, IProduct product, INotificationSender notificationSender, IHttpContextAccessor _IHttpContextAccessor)
        {
            _context = context;
            userRoleRepository = userRole;
            middleTablesRepository = middleTables;
            auditLogRepository = auditLog;
            productRepository = product;
            notificationSenderRepository = notificationSender;
            userRepository = user;
            _currentContext = _IHttpContextAccessor.HttpContext;
        }
        public override void OnActionExecuting(ActionExecutingContext context)//when any action in the controller is called, do this
        {
            base.OnActionExecuting(context);
            action = this.ControllerContext.RouteData.Values["action"].ToString();
            controller = this.ControllerContext.RouteData.Values["controller"].ToString();
            if (!action.ToLower().Contains("print"))
            {
                loggedUser = GetLoggedInUser(); //assigns a value to the loggedUser variable
            }
        }
        #endregion

        #region Gets!
        /// <summary>
        /// Access the User.Identity to get the logged user's email
        /// </summary>
        /// <returns>an Email xxx@geotab.com</returns>
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
        /// <summary>
        /// Gets the User from the database using the unique email. maybe add some exception handling later
        /// </summary>
        /// <returns>Existing User from Db</returns>
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
        #endregion

        #region Checks
        /// <summary>
        /// Checks if the current user is an approver for the current ECR.
        /// </summary>
        /// <param name="ECRId">Change Id</param>
        /// <returns>bool</returns>
        public bool CheckLoggedUserIsActiveApprover(int ECRId)
        {
            UserRoleECR userrole = null;
            if (loggedUser != null)
            {
                userrole = middleTablesRepository.GetExistingUserRoleECRByUser(loggedUser.Id, ECRId);
            }
            if (userrole != null)
            {
                if (userrole.Approval == null)  //if the approver has not approved it yet
                {
                    return true;
                }
                return false; //this false means that he has made a choice
            }
            return false; //this false means that he is not an approver
        }
        /// <summary>
        /// Checks if the current logged user is the originator for the ECR
        /// </summary>
        /// <param name="ECRId">Change Id</param>
        /// <returns></returns>
        public bool CheckLoggedUserIsOriginator(int ECRId)
        {
            var originatorId = _context.ECRs.Where(e => e.Id == ECRId).Select(a => a.OriginatorId).FirstOrDefault();
            if (loggedUser != null)
            {
                if (loggedUser.Id == originatorId)
                {
                    return true;
                }
            }
            return false;
        }
        private bool ECRExists(int id)
        {
            return _context.ECRs.Any(e => e.Id == id);
        }

        private bool CheckIfECRWasRejected(int id)
        {
            bool rejected = false;
            var isRejected = _context.ECRs.Where(ecr => ecr.Id == id).Select(r => r.Status).FirstOrDefault();
            if (isRejected == StatusOptions.RejectedApproval)
            {
                rejected = true;
            }
            return rejected;
        }

        public bool CheckIfUserInNotificationList(ECR ecr)
        {
            bool isUserInNotification = false;
            var loggedInUser = GetLoggedInUser();
            if (loggedInUser != null && ecr.Notifications != null)
            {
                foreach (var user in ecr.Notifications)
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

        #region Error Page
        public IActionResult ErrorPage(Exception ex)
        {
            if (ex.InnerException == null)
            {
                //return errorHandlerController.ErrorPage(ex.HResult, ex.Message, ex.StackTrace, ex.GetType().FullName, "", "", "", this.ControllerContext.RouteData.Values["action"].ToString(), this.ControllerContext.RouteData.Values["controller"].ToString());
                return RedirectToAction("ErrorPage", "ErrorHandler", new
                {
                    ex.HResult,
                    ex.Message,
                    ex.StackTrace,
                    ex.GetType().FullName,
                    innerMessage = "",
                    innerType = "",
                    innerToString = "",
                    Controller = this.ControllerContext.RouteData.Values["action"].ToString(),
                    Action = this.ControllerContext.RouteData.Values["controller"].ToString()
                });

            }
            else
            {
                //return errorHandlerController.ErrorPage(ex.HResult, ex.Message, ex.StackTrace, ex.GetType().FullName, ex.InnerException.Message, ex.InnerException.GetType().FullName, ex.InnerException.ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), this.ControllerContext.RouteData.Values["controller"].ToString());
                return RedirectToAction("ErrorPage", "ErrorHandler", new
                {
                    ex.HResult,
                    ex.Message,
                    ex.StackTrace,
                    ex.GetType().FullName,
                    innerMessage = ex.InnerException.Message,
                    innerType = ex.InnerException.GetType().FullName,
                    innerToString = ex.InnerException.ToString(),
                    Controller = this.ControllerContext.RouteData.Values["action"].ToString(),
                    Action = this.ControllerContext.RouteData.Values["controller"].ToString()
                });
            }
        }
        #endregion

        #region Details
        // GET: MyECOArea/ECRs/Details/5
        /// <summary>
        /// Method to display the Details View using the Id of a Change Request
        /// </summary>
        /// <param name="id">ECR Id</param>
        /// <returns>the View</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                //long list of includes, most relationships are being pulled because we need to display them.
                var eCR = await _context.ECRs
                    .Include(e => e.ChangeType)
                    .Include(e => e.Originator)
                    .Include(e => e.AreasAffected)
                        .ThenInclude(e => e.RequestType) //then include access the navigation property inside a navigation property. neat.
                    .Include(e => e.RelatedECOs)
                        .ThenInclude(e => e.ECO)
                    .Include(e => e.Notifications)
                        .ThenInclude(n => n.User)
                    .Include(e => e.Approvers)
                        .ThenInclude(a => a.UserRole)
                            .ThenInclude(u => u.User)
                    .Include(e => e.AffectedProducts)
                        .ThenInclude(a => a.Product)
                    .FirstOrDefaultAsync(m => m.Id == id);

                bool loggedUserIsApprover = CheckLoggedUserIsActiveApprover(eCR.Id);
                bool loggedUserIsOriginator = CheckLoggedUserIsOriginator(eCR.Id);
                bool wasRejected = CheckIfECRWasRejected(eCR.Id);
                bool isUserInNotification = CheckIfUserInNotificationList(eCR);

                ViewData["LoggedUserIsApprover"] = loggedUserIsApprover;
                ViewData["LoggedUserIsOriginator"] = loggedUserIsOriginator;
                ViewData["ECRWasRejected"] = wasRejected;
                ViewData["IsUserInNotificatonList"] = isUserInNotification;

                if (!string.IsNullOrWhiteSpace(TempData["ReminderEmailConfirmation"] as string))
                {
                    ViewData["ReminderEmailConfirmation"] = TempData["ReminderEmailConfirmation"];
                }

                List<ECO> relatedECOs = new List<ECO>();
                foreach (var ecrhaseco in eCR.RelatedECOs) //sets a list of ECRs using the RelatedECRs field for the Partial in the view
                {
                    relatedECOs.Add(ecrhaseco.ECO);
                }

                ViewData["ECOs"] = relatedECOs; //simple viewbag for the relatedECOs Partial.

                List<Audit> logsFromAuditTable = new List<Audit>();
                var logId = eCR.Id;

                #region Audit - Get ECR Table

                var logList = _context.Audits.Where(e => e.TableName == "ECRs" && e.EntityId == logId.ToString()).ToList()
                    .Where(g => g.OldData != g.NewData);

                foreach (var log in logList)
                {
                    if (log.TableName == "ECRs")
                    {
                        if (log.OldData == null && log.NewData != null)
                        {
                            List<string> ecrLog = new List<string>();
                            var newData = JsonConvert.DeserializeObject<ECR>(log.NewData);

                            if (newData.ChangeTypeId != -1)
                            {
                                var changedType = _context.RequestTypes.Where(i => i.Id == newData.ChangeTypeId).Select(i => i.Name).FirstOrDefault();
                                ecrLog.Add("ChangeType:" + changedType);
                            }
                            else
                            { ecrLog.Add("ChangeType:null"); }

                            if (newData.LinkUrls != null)
                            {
                                foreach (var links in newData.LinkUrls)
                                {
                                    ecrLog.Add("LinkUrl:" + links.Value.ToString());
                                }
                            }
                            else
                            { ecrLog.Add("LinkUrl:null"); }

                            if (newData.OriginatorId != -1)
                            {
                                var originatorName = _context.Users.Where(u => u.Id == newData.OriginatorId).Select(u => u.Name).FirstOrDefault();
                                ecrLog.Add("OriginatorName:" + originatorName);
                            }
                            else
                            { ecrLog.Add("OriginatorName:null"); }

                            log.NewData = Helper.GetStructuredJsonInfo(newData);
                            log.NewData += string.Join("<br/>", ecrLog);

                        }
                        if (log.Action == EntityState.Modified.ToString())
                        {
                            List<string> oldECRlog = new List<string>();
                            List<string> newECRlog = new List<string>();
                            var oldData = JsonConvert.DeserializeObject<ECR>(log.OldData);
                            var newData = JsonConvert.DeserializeObject<ECR>(log.NewData);

                            var changedColumns = log.ChangedColumns.Split(',').ToList();

                            if (changedColumns.Contains("LinkUrls"))
                            {
                                if (newData.LinkUrls != null)
                                {
                                    foreach (var links in newData.LinkUrls)
                                    {
                                        newECRlog.Add("LinkUrl:" + links.Value.ToString());
                                    }
                                }
                                else
                                { newECRlog.Add("LinkUrl: "); }

                                if (oldData.LinkUrls != null)
                                {
                                    foreach (var links in oldData.LinkUrls)
                                    {
                                        newECRlog.Add("LinkUrl:" + links.Value.ToString());
                                    }
                                }
                                else
                                { newECRlog.Add("LinkUrl: "); }
                            }
                            //if (changedColumns.Contains("NotesForEachDepartment"))
                            //{
                            //    if (newData.NotesForEachDepartment != null)
                            //    {
                            //        foreach (var notes in newData.NotesForEachDepartment)
                            //        {
                            //            newECOlog.Add("NotesForEachDepartment:" + notes);
                            //        }
                            //    }
                            //    else
                            //    { newECOlog.Add("NotesForEachDepartment: "); }

                            //    if (oldData.NotesForEachDepartment != null)
                            //    {
                            //        foreach (var notes in newData.NotesForEachDepartment)
                            //        {
                            //            oldECOlog.Add("NotesForEachDepartment:" + notes);
                            //        }
                            //    }
                            //    else
                            //    { oldECOlog.Add("NotesForEachDepartment: "); }
                            //}
                            if (changedColumns.Contains("ChangeTypeId"))
                            {
                                if (newData.ChangeTypeId != -1)
                                {
                                    var changedType = _context.RequestTypes.Where(i => i.Id == newData.ChangeTypeId).Select(i => i.Name).FirstOrDefault();
                                    newECRlog.Add("ChangeType:" + changedType);
                                }
                                else
                                { newECRlog.Add("ChangeType: "); }

                                if (oldData.ChangeTypeId != -1)
                                {
                                    var changedType = _context.RequestTypes.Where(i => i.Id == oldData.ChangeTypeId).Select(i => i.Name).FirstOrDefault();
                                    newECRlog.Add("ChangeType:" + changedType);
                                }
                                else
                                { newECRlog.Add("ChangeType: "); }
                            }
                            if (changedColumns.Contains("OriginatorId"))
                            {
                                if (newData.OriginatorId != -1)
                                {
                                    var originatorName = _context.Users.Where(u => u.Id == newData.OriginatorId).Select(u => u.Name).FirstOrDefault();
                                    newECRlog.Add("OriginatorName:" + originatorName);
                                }
                                else
                                { newECRlog.Add("OriginatorName: "); }

                                if (oldData.OriginatorId != -1)
                                {
                                    var originatorName = _context.Users.Where(u => u.Id == oldData.OriginatorId).Select(u => u.Name).FirstOrDefault();
                                    oldECRlog.Add("OriginatorName:" + originatorName);
                                }
                                else
                                { oldECRlog.Add("OriginatorName: "); }
                            }
                            log.OldData = Helper.GetStructuredJsonInfo(oldData, changedColumns);
                            log.NewData = Helper.GetStructuredJsonInfo(newData, changedColumns);

                            log.OldData += string.Join("<br/>", oldECRlog);
                            log.NewData += string.Join("<br/>", newECRlog);
                        }
                        //if (log.OldData != null && log.NewData != null)
                        //{

                        //    JObject sourceJObject = JsonConvert.DeserializeObject<JObject>(log.OldData);
                        //    JObject targetJObject = JsonConvert.DeserializeObject<JObject>(log.NewData);
                        //    var oldData = new System.Text.StringBuilder();
                        //    var newData = new System.Text.StringBuilder();

                        //    if (!JToken.DeepEquals(sourceJObject, targetJObject))
                        //    {
                        //        foreach (KeyValuePair<string, JToken> sourceProperty in sourceJObject)
                        //        {
                        //            JProperty targetProp = targetJObject.Property(sourceProperty.Key);

                        //            if (!JToken.DeepEquals(sourceProperty.Value, targetProp.Value))
                        //            {
                        //                var oldValue = sourceProperty.Value;
                        //                oldData.Append(oldValue.Parent + " <br/>");
                        //                var newValue = targetProp.Value;
                        //                newData.Append(newValue.Parent + " <br/>");
                        //            }
                        //        }
                        //    }
                        //    log.OldData = oldData.ToString();
                        //    log.NewData = newData.ToString();
                        //}
                        log.TableName = "ECR";
                    }

                    logsFromAuditTable.Add(log);
                }
                #endregion

                #region Audit - Get User Role Table

                var userRoleECRLogList = _context.Audits.Where(e => e.TableName == "UserRoleECRs" && e.EntityId == logId.ToString()).ToList()
                    .Where(g => g.OldData != g.NewData).GroupBy(g => new
                    {
                        g.EntityId,
                        Date = g.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss"),
                        g.Action

                    }).Select(g => new { key = g.Key, value = g.ToList() });
                foreach (var userRoleLog in userRoleECRLogList)
                {
                    foreach (var innerUserRoleLog in userRoleLog.value)
                    {
                        if (innerUserRoleLog.OldData != null)
                        {
                            var oldRequest = JsonConvert.DeserializeObject<UserRoleECR>(innerUserRoleLog.OldData);
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
                            var newRequest = JsonConvert.DeserializeObject<UserRoleECR>(innerUserRoleLog.NewData);
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
                var notofications = _context.Notifications.Where(n => n.ECRId == logId).Select(x => x.Id.ToString()).ToList();
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

                #region Audit - Get Request Type ECR// Areas Effected
                var requestTypeECR = _context.Audits.Where(e => e.TableName == "RequestTypeECRs" && e.EntityId == logId.ToString()).ToList()
                    .Where(g => g.OldData != g.NewData).GroupBy(g => new
                    {
                        g.EntityId,
                        Date = g.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss"),
                        g.Action
                    }).Select(g => new { key = g.Key, value = g.ToList() });

                foreach (var log in requestTypeECR)
                {
                    foreach (var innerLog in log.value)
                    {
                        if (innerLog.OldData != null)
                        {
                            var oldRequest = JsonConvert.DeserializeObject<RequestTypeECR>(innerLog.OldData);
                            var oldRequestType = _context.RequestTypes.Where(r => r.Id == oldRequest.RequestTypeId).Select(r => r.Name).FirstOrDefault();
                            innerLog.OldData = oldRequestType;
                        }
                        if (innerLog.NewData != null)
                        {
                            var newRequest = JsonConvert.DeserializeObject<RequestTypeECR>(innerLog.NewData);
                            var newRequestType = _context.RequestTypes.Where(r => r.Id == newRequest.RequestTypeId).Select(r => r.Name).FirstOrDefault();
                            innerLog.NewData = newRequestType;
                        }
                    }

                    var Audit = new Audit()
                    {
                        UserName = log.value.FirstOrDefault().UserName,
                        Action = log.value.FirstOrDefault().Action,
                        TableName = "Affected Area(s)",
                        EntityId = log.key.EntityId,
                        OldData = string.Join(",", log.value.ToList().Where(g => g.OldData != null).Select(g => g.OldData)),
                        NewData = string.Join(",", log.value.ToList().Where(g => g.NewData != null).Select(g => g.NewData)),
                        CreatedOn = DateTime.Parse(log.key.Date)
                    };
                    logsFromAuditTable.Add(Audit);
                }
                #endregion

                #region Audit - Get Affected Product List
                var productECRLogList = _context.Audits.Where(e => e.TableName == "ProductECRs" && e.EntityId == logId.ToString()).ToList()
                    .Where(d => d.OldData != d.NewData).GroupBy(g => new
                    {
                        g.EntityId,
                        Date = g.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss"),
                        g.Action
                    }).Select(g => new { key = g.Key, value = g.ToList() });

                foreach (var log in productECRLogList)
                {
                    foreach (var innerLog in log.value)
                    {
                        if (innerLog.OldData != null)
                        {
                            var oldProduct = JsonConvert.DeserializeObject<ProductECR>(innerLog.OldData);
                            var oldProductName = _context.Products.Where(p => p.Id == oldProduct.ProductId).Select(p => p.Name + "-" + p.Category).FirstOrDefault();
                            innerLog.OldData = oldProductName;
                        }
                        if (innerLog.NewData != null)
                        {
                            var newProduct = JsonConvert.DeserializeObject<ProductECR>(innerLog.NewData);
                            var newProductName = _context.Products.Where(p => p.Id == newProduct.ProductId).Select(p => p.Name + "-" + p.Category).FirstOrDefault();
                            innerLog.NewData = newProductName;
                        }
                    }
                    var Audit = new Audit()
                    {
                        UserName = log.value.FirstOrDefault().UserName,
                        Action = log.value.FirstOrDefault().Action,
                        TableName = "Product",
                        EntityId = log.key.EntityId,
                        OldData = string.Join(",", log.value.ToList().Where(g => g.OldData != null).Select(g => g.OldData)),
                        NewData = string.Join(",", log.value.ToList().Where(g => g.NewData != null).Select(g => g.NewData)),
                        CreatedOn = DateTime.Parse(log.key.Date)
                    };
                    logsFromAuditTable.Add(Audit);
                }
                #endregion

                if (logsFromAuditTable.Count == 0)
                {
                    List<AuditLog> auditLogs = new List<AuditLog>();
                    foreach (var log in auditLogRepository.GetAuditLogsECO(eCR.Id)) //sets a list of AuditLogs for the Partial
                    {
                        auditLogs.Add(log);
                    }
                    ViewData["AuditLogECR"] = auditLogs;
                }
                else
                {
                    ViewData["AuditLog"] = logsFromAuditTable;
                }

                if (eCR == null)
                {
                    return NotFound();
                }
                return View(eCR);
            }
            catch (Exception ex)
            {
                ViewModelError vm = SetViewModelError(ex);
                return RedirectToAction("ErrorPage", "ErrorHandler", vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details([Bind("ApproverOption, UserRoleId, ChangeId, RejectReason")] ApproverViewModel vm)  //when an approver chooses an option (Reject or Approve) for the Change
        {
            try
            {
                List<UserRoleECR> Approvers = middleTablesRepository.GetUserRolesForECR(vm.ChangeId).ToList(); //gets all approvers for the ECR
                foreach (var approver in Approvers)
                {
                    UserRoleECR existingUserRoleECR = middleTablesRepository.GetExistingUserRoleECR(approver.UserRoleId, vm.ChangeId);
                    if (loggedUser.Id == userRoleRepository.GetUserByUserRoleId(existingUserRoleECR.UserRoleId).Id)
                    {
                        //UserRoleECR that made the choice
                        existingUserRoleECR.Approval = vm.ApproverOption; //sets the approval to the option
                        existingUserRoleECR.AprovedDate = DateTime.Now.ToString();
                        _context.Update(existingUserRoleECR);
                        await _context.SaveChangesAsync();
                    }
                }

                ECR currentECR = _context.ECRs.Where(c => c.Id == vm.ChangeId).FirstOrDefault(); //gets the ECR object
                List<string> changes = new List<string>();
                StatusOptions currentStatus = currentECR.Status; //sets a log for the Status (in case it changes)
                currentECR.RejectReason = vm.RejectReason;
                if (Approvers.Any(a => a.Approval == null)) //if any of the approvers hasn't approved yet
                {
                    currentECR.Status = StatusOptions.AwaitingApproval;
                }
                if (Approvers.Any(a => a.Approval == false)) //if any of the approvers rejected it, reject the change.
                {
                    currentECR.Status = StatusOptions.RejectedApproval;
                }
                else if (Approvers.TrueForAll(a => a.Approval == true)) //if all the approvers approved it, approve the change.
                {
                    currentECR.Status = StatusOptions.Approved;
                    currentECR.ClosedDate = DateTime.Now;  //set the ClosedDate to now
                }

                if (currentECR.Status == StatusOptions.Approved)
                {
                    changes.Add("ECR has been approved.");
                    notificationSenderRepository.SendNotificationOnApprovalECR(currentECR.Id, $"ECR {currentECR.Id} has been Approved");//send a notification if it has been approved
                }
                if (currentStatus != currentECR.Status)
                {
                    ChangeStatusECR = $"Current Status : {currentStatus} ; New Status : {currentECR.Status}";
                    changes.Add($"ECR status has changed - {ChangeStatusECR}");
                    notificationSenderRepository.SendNotificationOnStatusChangeECR(currentECR.Id, ChangeStatusECR);//send a notification if the status has changed
                }

                notificationSenderRepository.SendNotificationOnAnyChangeECR(currentECR.Id, changes);//send a notification on any change -> approver selected an option is a change

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = vm.ChangeId });//go back to the details page
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", ex);
            }
        }
        #endregion

        #region SetViewData and ViewModel
        /// <summary>
        /// This method is used to put information in the ViewData. The page will use it in a <select /asp-items="ViewBag.X"></select>
        /// X being the ViewData["X"]
        /// </summary>
        public void SetViewDataCreate()
        {
            var users = _context.Users.Where(ur => ur.isActive);
            ViewData["ChangeTypeId"] = new SelectList(_context.RequestTypes, "Id", "Name");
            ViewData["OriginatorId"] = new SelectList(_context.Users.Where(u => u == loggedUser), "Id", "Name");
            ViewData["AllUsers"] = new SelectList(users, "Id", "Name");
        }
        /// <summary>
        /// This will generate the ViewModel for most pages that need it.
        /// First we get the loggedUser using the application, then a List for the AreasAffected is created.
        /// Then a List<SelectList> is created for the available approvers.
        /// It works by iterating over the requests, then getting the approvers for each request and adding it to a SelectList
        /// The SelectList then is added to approversList.
        /// </summary>
        /// <returns></returns>
        public ECRViewModel SetViewModel()
        {
            ECRViewModel vm = new ECRViewModel(); //new viewmodel
            List<RequestTypeChangeCheckBox> r1 = new List<RequestTypeChangeCheckBox>();
            var allRequest = _context.RequestTypes.ToList(); //gets all possible request types
            for (int i = 0; i < allRequest.Count(); i++)
            {
                RequestTypeChangeCheckBox request = new RequestTypeChangeCheckBox(allRequest[i]) //create a new RequestTypeChangeCheckBox based on the current allRequest[i]
                {
                    Selected = false
                };
                r1.Add(request);
            }
            vm.AreasAffected = r1; //the AreasAffected is the list of checkboxes for the areas

            List<SelectList> approversList = new List<SelectList>();
            foreach (var request in allRequest)
            {
                var Approvers = userRoleRepository.GetApproversWithType(request.Id, loggedUser.Id); //get the available approvers for the request, but not the logged user or any inactive users
                var RequestGroup = new SelectListGroup { Name = request.Name }; //the name of the Group they are a part of
                var approvers = new List<SelectListItem>();
                approvers.AddRange(Approvers.Select(item => new SelectListItem //adds the collection of Approvers to a List of SelectListItems
                {
                    Text = item.User.Name,
                    Value = item.Id.ToString(),
                    Group = RequestGroup
                }).OrderBy(a => a.Group.Name).ToList());
                SelectList DropDownList = new SelectList(approvers, "Value", "Text", -1, "Group.Name"); //use the items to generate a SelectList for the <select> elemente
                approversList.Add(DropDownList);
            }
            vm.ApproversList = approversList; //ApproversList is a List<SelectList>

            vm.ProductList = SetProductDictionary(vm.Id);
            return vm;
        }

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

        #endregion

        #region Create Change
        // GET: MyECOArea/ECRs/Create
        /// <summary>
        /// Get Method to display the Create View. helps setting the viewdata and viewmodel
        /// </summary>
        /// <returns>Create Page</returns>
        public IActionResult Create()
        {
            SetViewDataCreate();
            return View(SetViewModel());
        }
        // POST: MyECOArea/ECRs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind("Id,PermanentChange,Description,ReasonForChange,ChangeTypeId,BOMRequired,ProductValidationTestingRequired,PlannedImplementationDate,CreatedDate,ClosedDate,CustomerImpact,PriorityLevel,OriginatorId,ImplementationType,Status,ECOsCompleted,AreasAffected")] ECR eCR
        public async Task<IActionResult> Create([Bind("PermanentChange,Description,ReasonForChange,ChangeTypeId,AreasAffected,BOMRequired,ProductValidationTestingRequired,PlannedImplementationDate," +
            "CustomerImpact,PriorityLevel,ImplementationType,Status,OriginatorId,UsersToBeNotified,LinkUrls,AffectedProductsIds,PreviousRevision,NewRevision")] ECRViewModel vmECR,
            string[] ApproversList)
        { //POST request with all values from the page
            if (ModelState.IsValid)
            {
                try
                {
                    ECR NewECR = new ECR(vmECR); //create a new ECR object from the values in vmECR
                    _context.ECRs.Add(NewECR); //add it to the ChangeTracker
                    await _context.SaveChangesAsync(); //save it to the database

                    int ECRId = NewECR.Id;

                    List<RequestTypeECR> changesList = SetAreasAffectedECR(vmECR, ECRId);

                    List<UserRoleECR> userRoleECRList = SetUserRoleECR(ApproversList, ECRId, false);

                    List<Notifications> notifications = SetNotificationsECR(vmECR, ECRId, ApproversList);

                    List<ProductECR> affectedProducts = SetAffectedProductsECR(vmECR.AffectedProductsIds, ECRId);

                    NewECR.Notifications = notifications;
                    NewECR.Approvers = userRoleECRList;
                    NewECR.AreasAffected = changesList;
                    NewECR.AffectedProducts = affectedProducts;

                    //auditLogRepository.Add(NewECR, loggedUser);
                    await _context.SaveChangesAsync();
                    if (NewECR.Status != StatusOptions.Draft)
                    {
                        List<string> changes = new List<string>();
                        changes.Add($"ECR {NewECR.Id} has created.");
                        notificationSenderRepository.SendNotificationOnAnyChangeECR(NewECR.Id, changes);
                    }
                    return RedirectToAction("Details", new { id = NewECR.Id });
                }
                catch (Exception ex)
                {
                    ViewModelError vm = SetViewModelError(ex);
                    return RedirectToAction("ErrorPage", "ErrorHandler", vm);
                }
            }
            SetViewDataCreate();
            var viewModelCreate = SetViewModel();
            vmECR.ApproversList = viewModelCreate.ApproversList;
            return View(vmECR);
        }
        #endregion

        #region SetReturnLists
        /// <summary>
        /// This sets a list of the areas affected selected by the user in the edit page.
        /// </summary>
        /// <param name="vm">The ViewModel with the current values</param>
        /// <param name="ECRId">Existing or New ECRId</param>
        /// <returns>List of RequestTypeECR to be set in the db</returns>
        public List<RequestTypeECR> SetAreasAffectedECR(ECRViewModel vm, int ECRId)
        {
            List<RequestTypeECR> returnList = new List<RequestTypeECR>();
            foreach (var typeChange in vm.AreasAffected.Where(a => a.Selected == true)) //get the areas affected that were selected
            {
                var request = middleTablesRepository.GetExistingRequestTypeECR(typeChange.TypeId, ECRId); //gets an existing RequestTypeECO if it exists using the FK match
                if (request != null)
                {
                    returnList.Add(request);
                }
                else
                {
                    RequestType requestType = _context.RequestTypes.Where(r => r.Id == typeChange.TypeId).FirstOrDefault(); //get the RequestType object for Navigation Property purposes
                    RequestTypeECR requestTypeChange = new RequestTypeECR() { ECRId = ECRId, RequestTypeId = typeChange.TypeId, RequestType = requestType }; //create a new RequestTypeECR with the ECRId and the TypeId
                    returnList.Add(requestTypeChange);
                }
            }
            return returnList;
        }
        /// <summary>
        /// This sets a list of the products affected by the change from the view.
        /// </summary>
        /// <param name="ProductIds">List of ProductIds that were selected</param>
        /// <param name="ECRId">Change Id</param>
        /// <returns></returns>
        public List<ProductECR> SetAffectedProductsECR(List<int> ProductIds, int ECRId)
        {
            List<ProductECR> returnList = new List<ProductECR>();
            if (ProductIds != null && ProductIds.Count > 0)
            {
                foreach (int productid in ProductIds) //for all the Ids in the List
                {
                    ProductECR existingProdECR = middleTablesRepository.GetExistingProductECR(productid, ECRId); //get an existing productECR record if it already exists
                    if (existingProdECR != null)
                    {
                        returnList.Add(existingProdECR);
                    }
                    else
                    {
                        ProductECR productECR = new ProductECR() //new productECR if it is a new affected product
                        {
                            ProductId = productid,
                            Product = productRepository.GetProduct(productid),
                            ECRId = ECRId
                        };
                        returnList.Add(productECR);
                    }
                }
            }
            return returnList;
        }
        /// <summary>
        /// This sets a list of the user roles (approvers) selected by the user in the edit page.
        /// </summary>
        /// <param name="ApproversList">A list of the user role Ids</param>
        /// <param name="ECRId">Existing or New ECRId</param>
        /// <returns></returns>
        public List<UserRoleECR> SetUserRoleECR(string[] ApproversList, int ECRId, bool ignoreApprovals)
        {
            List<UserRoleECR> returnList = new List<UserRoleECR>();
            if (ApproversList != null && ApproversList.Length > 0)
            {
                foreach (var userRoleId in ApproversList)
                {
                    if (Int32.TryParse(userRoleId, out int intUserRoleId))
                    {
                        var userRole = middleTablesRepository.GetExistingUserRoleECR(intUserRoleId, ECRId); //gets an existing UserRoleECO if it is already in the database
                        if (userRole != null)
                        {
                            if (!ignoreApprovals)
                            {
                                userRole.Approval = null; //sets the approval to null because a change was made
                                userRole.AprovedDate = null;
                            }
                            returnList.Add(userRole);
                        }
                        else
                        {
                            UserRoleECR userRoleECR = new UserRoleECR() { ECRId = ECRId, UserRoleId = intUserRoleId, Approval = null }; //creates a new UserRoleECO for the list
                            returnList.Add(userRoleECR);
                        }
                    }
                }
            }
            return returnList;
        }
        /// <summary>
        /// This sets a list of Notifications from the users selected in the edit page.
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="NewECRId"></param>
        /// <returns></returns>
        public List<Notifications> SetNotificationsECR(ECRViewModel vm, int ECRId, string[] ApproversList)
        {
            List<Notifications> returnList = new List<Notifications>();
            bool added = false;
            var notification = middleTablesRepository.GetNotificationByUserAndECR(vm.OriginatorId, ECRId); //if there already is a notification for the user in the db.
            if (notification != null)
            {
                returnList.Add(notification);
            }
            else
            {
                User NotificationUser = _context.Users.Where(u => u.Id == vm.OriginatorId).FirstOrDefault(); //get the User to access it's name later in the sendNotification
                Notifications NewNotification = new Notifications() { ECRId = ECRId, UserId = vm.OriginatorId, Option = NotificationOption.AnyChange, User = NotificationUser }; //the user will always be notified on any change, unless they change it in the main page
                returnList.Add(NewNotification);
            }
            if (vm.UsersToBeNotified != null)
            {
                foreach (var userid in vm.UsersToBeNotified) //for all the users selected to receive notifications
                {
                    added = false;
                    notification = middleTablesRepository.GetNotificationByUserAndECR(userid, ECRId); //if there already is a notification for the user in the db.
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
                            ECRId = ECRId,
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
            added = false;
            if (ApproversList != null)
            {
                foreach (var userRoleId in ApproversList)
                {
                    added = false;
                    if (Int32.TryParse(userRoleId, out int intUserRoleId))
                    {
                        var user = userRoleRepository.GetUserByUserRoleId(intUserRoleId);
                        notification = middleTablesRepository.GetNotificationByUserAndECR(user.Id, ECRId); //if there already is a notification for the user in the db.
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
                                ECRId = ECRId,
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
            }
            return returnList;
        }
        /// <summary>
        /// Translates to a Collection of Categories that contains a Collection of Families that contains a Collection of Products that can be selected or not
        /// </summary>
        /// <param name="ECRId">current ECRId</param>
        /// <returns>Returns a Dictionary in the format of: { "Category", {"Family", {Products, bool } } }</returns>
        public Dictionary<string, Dictionary<string, Dictionary<Product, bool>>> SetProductDictionary(int ECRId)
        {
            IQueryable<IGrouping<string, Product>> allProducts = _context.Products.GroupBy(p => p.Category); //get all possible products from db, grouped by Category

            List<ProductECR> affectedProducts = middleTablesRepository.GetProductECRsByECR(ECRId).ToList();  //gets a list of affected products already in the change

            Dictionary<string, Dictionary<string, Dictionary<Product, bool>>> returnDictionary = new Dictionary<string, Dictionary<string, Dictionary<Product, bool>>>(); //create a new dictionary for the return

            foreach (IGrouping<string, Product> category in allProducts)
            {
                IEnumerable<IGrouping<string, Product>> families = category.GroupBy(a => a.Name.Split('-')[0]); //separate the - and get the first element of the split -> this will group GO7, GO8, GO9, ATT, IOX etc


                Dictionary<string, Dictionary<Product, bool>> dictFamilies = new Dictionary<string, Dictionary<Product, bool>>(); //create a new Dictionary for the Families
                foreach (IGrouping<string, Product> products in families)//now foreach family in the collection
                {

                    Dictionary<Product, bool> productIncludeStatus = new Dictionary<Product, bool>();
                    foreach (Product p in products)
                    {
                        if (affectedProducts.Where(b => b.ProductId == p.Id).FirstOrDefault() != null) //checks if the change has the product as an affectedproduct
                        {
                            productIncludeStatus.Add(p, true);
                        }
                        else
                        {
                            productIncludeStatus.Add(p, false);
                        }
                    }
                    dictFamilies.Add(products.Key, productIncludeStatus); //add the previous dictonary to the family with the Key as the Family name
                }
                returnDictionary.Add(category.Key, dictFamilies); //add the familyDictionary to the Category dictionary with the Category name as the string
            }
            return returnDictionary; //return the built dictionary
        }
        #endregion

        #region Edit Change
        // GET: MyECOArea/ECRs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {


                SetViewDataCreate();

                var eCR = await _context.ECRs
                    .Include(e => e.ChangeType)
                    .Include(e => e.Originator)
                    .Include(e => e.AreasAffected)
                        .ThenInclude(e => e.RequestType)
                    .Include(e => e.RelatedECOs)
                        .ThenInclude(e => e.ECO)
                    .Include(e => e.Notifications)
                        .ThenInclude(n => n.User)
                    .Include(e => e.Approvers)
                        .ThenInclude(a => a.UserRole)
                            .ThenInclude(u => u.User)
                    .Include(e => e.AffectedProducts)
                    .FirstAsync(e => e.Id == id); //get all the necessary information from the db.

                ECRViewModel vm = new ECRViewModel(eCR);

                vm.ApproversList = SetSelectListsForApproversEdit(vm);

                vm.AreasAffected = SetAreasAffectedEdit(vm);

                vm.ProductList = SetProductDictionary(vm.Id);

                ViewData["Notifications"] = SetSelectListForNotifications(vm);

                if (eCR == null)
                {
                    return NotFound();
                }
                return View(vm);

            }
            catch (Exception ex)
            {
                ViewModelError vm = SetViewModelError(ex);
                return RedirectToAction("ErrorPage", "ErrorHandler", vm);
            }
        }
        // POST: MyECOArea/ECRs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,PermanentChange,Description,ReasonForChange,ChangeTypeId,AreasAffected,BOMRequired," +
            "ProductValidationTestingRequired,PlannedImplementationDate,CustomerImpact,PriorityLevel,ImplementationType,Status,OriginatorId,UsersToBeNotified,LinkUrls,AffectedProductsIds,PreviousRevision,NewRevision")] ECRViewModel vmECR,
            string[] ApproversList)
        { //POST method of the Edit Page
            if (ModelState.IsValid)
            {
                try
                {
                    ECR existingECR = _context.ECRs.Where(e => e.Id == vmECR.Id)
                        .Include(a => a.AreasAffected).ThenInclude(a => a.RequestType)
                        .Include(a => a.Approvers).ThenInclude(b => b.UserRole).ThenInclude(u => u.User)
                        .Include(a => a.Approvers).ThenInclude(b => b.UserRole).ThenInclude(u => u.RequestType)
                        .Include(a => a.AffectedProducts).ThenInclude(p => p.Product)
                        .Include(a => a.Notifications).ThenInclude(n => n.User)
                        .Single(); //gets the ECR from the database with all related info from middle tables
                    List<string> changes = FindChanges(existingECR, vmECR, ApproversList);

                    bool ignoreApprovals = false;
                    foreach (var change in changes)
                    {
                        if (change.Contains("Notifications"))
                        {
                            ignoreApprovals = true;
                        }
                    }
                    UpdateExistingECR(existingECR, vmECR, ApproversList); //sets the value of the new record
                    existingECR.Approvers = SetUserRoleECR(ApproversList, existingECR.Id, ignoreApprovals);

                    existingECR.Status = vmECR.Status;
                    UpdateUserRoleECRTable(SetUserRoleECR(ApproversList, existingECR.Id, ignoreApprovals), existingECR.Id); //will delete the records that were removed by the user. Addition happens above ^
                    _context.Update(existingECR);
                    //auditLogRepository.Add(existingECR, loggedUser); //add a new audit log and who made the change


                    await _context.SaveChangesAsync();
                    if (existingECR.Status != StatusOptions.Draft)
                    {
                        if (changes.Count > 0)
                        {
                            notificationSenderRepository.SendNotificationOnAnyChangeECR(existingECR.Id, changes); //functions to send notifications based on the bool
                        }
                        if (sendNotificationOnDescriptionChange)
                            notificationSenderRepository.SendNotificationOnDescriptionChangeECR(existingECR.Id, changedDescriptionECR);
                        if (sendNotificationOnReasonChange)
                            notificationSenderRepository.SendNotificationOnReasonChangeECR(existingECR.Id, reasonForChangeECR);
                        if (sendNotificationOnImplementationDateChange)
                            notificationSenderRepository.SendNotificationOnImplementationDateChangeECR(existingECR.Id, implementationDateChangedECR);
                        if (sendNotificationOnApproversChange)
                            notificationSenderRepository.SendNotificationOnApproversChangeECR(existingECR.Id, approversChangedECR);
                        if (existingECR.Status == StatusOptions.Approved)
                        {
                            notificationSenderRepository.SendNotificationOnApprovalECR(existingECR.Id, ($"{existingECR.Id} has been approved"));
                        }
                    }
                    return RedirectToAction("Details", new { id = existingECR.Id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ECRExists(Convert.ToInt32(vmECR.Id)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["ChangeTypeId"] = new SelectList(_context.RequestTypes, "Id", "Name", vmECR.ChangeTypeId);
            return View(vmECR);
        }

        #endregion

        #region Clone Rejected ECR
        public async Task<IActionResult> Clone(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {

                SetViewDataCreate();

                var eCR = await _context.ECRs
                    .Include(e => e.ChangeType)
                    .Include(e => e.Originator)
                    .Include(e => e.AreasAffected)
                        .ThenInclude(e => e.RequestType)
                    .Include(e => e.RelatedECOs)
                        .ThenInclude(e => e.ECO)
                    .Include(e => e.Notifications)
                        .ThenInclude(n => n.User)
                    .Include(e => e.Approvers)
                        .ThenInclude(a => a.UserRole)
                            .ThenInclude(u => u.User)
                    .Include(e => e.AffectedProducts)
                    .FirstAsync(e => e.Id == id); //get all the necessary information from the db.

                ECRViewModel vm = new ECRViewModel(eCR);

                vm.ApproversList = SetSelectListsForApproversEdit(vm);

                vm.AreasAffected = SetAreasAffectedEdit(vm);

                vm.ProductList = SetProductDictionary(vm.Id);

                ViewData["AllUsers"] = SetSelectListForNotifications(vm);

                if (eCR == null)
                {
                    return NotFound();
                }
                return View("Create", vm);

            }
            catch (Exception ex)
            {
                ViewModelError vm = SetViewModelError(ex);
                return RedirectToAction("ErrorPage", "ErrorHandler", vm);
            }
        }
        #endregion

        #region FindChangesForNotification
        private List<string> FindChanges(ECR existingECR, ECRViewModel vmECR, string[] ApproversList)
        {
            try
            {
                List<string> changes = new List<string>();

                PropertyInfo[] existingProp = existingECR.GetType().GetProperties();
                PropertyInfo[] newProp = vmECR.GetType().GetProperties();
                foreach (PropertyInfo existprop in existingProp)
                {
                    foreach (PropertyInfo nwprop in newProp)
                    {
                        var newVal = nwprop.GetValue(vmECR, null);
                        var existVal = existprop.GetValue(existingECR, null);
                        if (existprop.Name == nwprop.Name)
                        {
                            if (existprop.Name.ToLower() == "affectedproducts")
                            {
                                newVal = vmECR.AffectedProductsIds;
                                if (existVal != null && newVal != null)
                                {
                                    var currentProduct = (ICollection<ProductECR>)existVal;
                                    var newProduct = (List<int>)newVal;
                                    List<string> existProducts = new List<string>();
                                    List<string> newProducts = new List<string>();
                                    foreach (var products in currentProduct)
                                    {
                                        existProducts.Add(products.Product.Name.ToString());
                                    }
                                    foreach (var products in newProduct)
                                    {
                                        newProducts.Add(productRepository.GetProduct(products).Name.ToString());
                                    }
                                    var removed = existProducts.Except(newProducts).ToList();
                                    var added = newProducts.Except(existProducts).ToList();
                                    string productsAdded = "";
                                    string productsRemoved = "";
                                    if (added.Count > 0)
                                    {
                                        foreach (var product in added)
                                        {
                                            productsAdded += product + ",";
                                        }
                                        changes.Add("Products added: " + productsAdded + ".");
                                    }
                                    if (removed.Count > 0)
                                    {
                                        foreach (var product in removed)
                                        {
                                            productsRemoved += product + ",";
                                        }
                                        changes.Add("Products Removed: " + productsRemoved + ".");
                                    }
                                }
                                else if (newVal != null && existVal == null)
                                {
                                    var newProduct = (List<int>)newVal;
                                    List<string> newProducts = new List<string>();

                                    foreach (var products in newProduct)
                                    {
                                        newProducts.Add(productRepository.GetProduct(products).Name.ToString());
                                    }
                                    var added = newProducts.ToList();
                                    string productsAdded = "";
                                    if (added.Count > 0)
                                    {
                                        foreach (var product in added)
                                        {
                                            productsAdded += product + ",";
                                        }
                                        changes.Add("Products added: " + productsAdded + ".");
                                    }
                                }
                                else if (newVal == null && existVal != null)
                                {
                                    var currentProduct = (ICollection<ProductECR>)existVal;
                                    List<string> existProducts = new List<string>();

                                    foreach (var products in currentProduct)
                                    {
                                        existProducts.Add(products.Product.Name.ToString());
                                    }

                                    var removed = existProducts.ToList();
                                    string productsRemoved = "";
                                    if (removed.Count > 0)
                                    {
                                        foreach (var product in removed)
                                        {
                                            productsRemoved += product + ",";
                                        }
                                        changes.Add("Products Removed: " + productsRemoved + ".");
                                    }
                                }
                            }
                            else if (existprop.Name.ToLower() == "relatedecos")
                            {
                                if (existVal != null && newVal != null)
                                {
                                    var currentProduct = (ICollection<ECRHasECO>)existVal;
                                    var newProduct = (ICollection<ECRHasECO>)newVal;
                                    List<string> existProducts = new List<string>();
                                    List<string> newProducts = new List<string>();
                                    foreach (var products in currentProduct)
                                    {
                                        existProducts.Add(products.ECRId.ToString());
                                    }
                                    foreach (var products in newProduct)
                                    {
                                        newProducts.Add(products.ECRId.ToString());
                                    }
                                    var removed = existProducts.Except(newProducts).ToList();
                                    var added = newProducts.Except(existProducts).ToList();
                                    string productsAdded = "";
                                    string productsRemoved = "";
                                    if (added.Count > 0)
                                    {
                                        foreach (var product in added)
                                        {
                                            productsAdded += product + ",";
                                        }
                                        changes.Add("Products added: " + productsAdded + ".");
                                    }
                                    if (removed.Count > 0)
                                    {
                                        foreach (var product in removed)
                                        {
                                            productsRemoved += product + ",";
                                        }
                                        changes.Add("Products Removed: " + productsRemoved + ".");
                                    }
                                }
                                else if (newVal != null && existVal == null)
                                {
                                    var newProduct = (ICollection<ECRHasECO>)newVal;
                                    List<string> newProducts = new List<string>();

                                    foreach (var products in newProduct)
                                    {
                                        newProducts.Add(products.ECRId.ToString());
                                    }
                                    var added = newProducts.ToList();
                                    string productsAdded = "";
                                    if (added.Count > 0)
                                    {
                                        foreach (var product in added)
                                        {
                                            productsAdded += product + ",";
                                        }
                                        changes.Add("Products added: " + productsAdded + ".");
                                    }
                                }
                                else if (existVal != null && newVal == null)
                                {
                                    var currentProduct = (ICollection<ECRHasECO>)existVal;
                                    List<string> existProducts = new List<string>();

                                    foreach (var products in currentProduct)
                                    {
                                        existProducts.Add(products.ECRId.ToString());
                                    }

                                    var removed = existProducts.ToList();
                                    string productsRemoved = "";
                                    if (removed.Count > 0)
                                    {
                                        foreach (var product in removed)
                                        {
                                            productsRemoved += product + ",";
                                        }
                                        changes.Add("Products Removed: " + productsRemoved + ".");
                                    }
                                }
                            }
                            else if (existprop.Name.ToLower() == "areasaffected")
                            {
                                if (existVal != null && newVal != null)
                                {
                                    var currentArea = (ICollection<RequestTypeECR>)existVal;
                                    var newArea = (ICollection<RequestTypeChangeCheckBox>)newVal;
                                    List<string> existArea = new List<string>();
                                    List<string> newAreas = new List<string>();
                                    foreach (var approver in currentArea)
                                    {
                                        existArea.Add(approver.RequestType.Name.ToString());
                                    }
                                    foreach (var approver in newArea.Where(a => a.Selected == true))
                                    {
                                        newAreas.Add(approver.Name.ToString());
                                    }
                                    var removed = existArea.Except(newAreas).ToList();
                                    var added = newAreas.Except(existArea).ToList();
                                    string areasAdded = "";
                                    string areasRemoved = "";
                                    if (added.Count > 0)
                                    {
                                        foreach (var user in added)
                                        {
                                            areasAdded += user + ", ";
                                        }
                                        changes.Add(areasAdded + " have been added as Affected Areas.");
                                    }
                                    if (removed.Count > 0)
                                    {
                                        foreach (var user in removed)
                                        {
                                            areasRemoved += user + ", ";
                                        }
                                        changes.Add(areasRemoved + " have been removed from approvers.");
                                    }
                                }
                                else if (newVal != null && existVal == null)
                                {

                                    var newArea = (ICollection<RequestTypeECR>)newVal;
                                    List<string> newAreas = new List<string>();

                                    foreach (var approver in newArea)
                                    {
                                        newAreas.Add(approver.RequestType.Name.ToString());
                                    }
                                    var added = newAreas.ToList();
                                    string areasAdded = "";
                                    if (added.Count > 0)
                                    {
                                        foreach (var user in added)
                                        {
                                            areasAdded += user + ", ";
                                        }
                                        changes.Add(areasAdded + " have been added as Affected Areas.");
                                    }
                                }
                                else if (newVal == null && existVal != null)
                                {
                                    var currentArea = (ICollection<RequestTypeECR>)existVal;
                                    List<string> existArea = new List<string>();
                                    foreach (var approver in currentArea)
                                    {
                                        existArea.Add(approver.RequestType.Name.ToString());
                                    }

                                    var removed = existArea.ToList();
                                    string areasRemoved = "";

                                    if (removed.Count > 0)
                                    {
                                        foreach (var user in removed)
                                        {
                                            areasRemoved += user + ", ";
                                        }
                                        changes.Add(areasRemoved + " have been removed from approvers.");
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
                                foreach (var user in vmECR.UsersToBeNotified)
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
                            else if (existprop.Name.ToLower() == "approvers")
                            {
                                var currentApprovers = (ICollection<UserRoleECR>)existVal;
                                approversChangedECR = new List<string>();
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
                                    approversChangedECR.Add(usersAdded + " have been added as approvers.");
                                }
                                if (removed.Count > 0)
                                {
                                    foreach (var user in removed)
                                    {
                                        usersRemoved += user + ",";
                                    }
                                    changes.Add(usersRemoved + " have been removed from approvers.");
                                    approversChangedECR.Add(usersRemoved + " have been removed from approvers.");
                                }
                            }
                            else if (existprop.Name.ToLower() == "originator")
                            {
                                continue;
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
                            else if (existprop.Name.ToLower() == "linkurls")
                            {
                                var currentLink = (Dictionary<string, string>)existVal;
                                var newLink = (Dictionary<string, string>)newVal;

                                if (currentLink != null && newLink != null)
                                {
                                    var added = newLink.Except(currentLink).ToList();
                                    var removed = currentLink.Except(newLink).ToList();
                                    var linksRemoved = "";
                                    var linksAdded = "";
                                    if (added.Count > 0)
                                    {
                                        linksAdded = "The links below have been added to the ECO:<br/>";
                                        foreach (var user in added)
                                        {
                                            linksAdded += user + "<br/>";
                                        }
                                        changes.Add(linksAdded + "<br/>");
                                    }
                                    if (removed.Count > 0)
                                    {
                                        linksRemoved = "The links below have been removed from the ECO:<br/>";
                                        foreach (var user in removed)
                                        {
                                            linksRemoved += user + "<br/>";
                                        }
                                        changes.Add(linksRemoved + "<br/>");
                                    }
                                }
                                else if (currentLink == null && newLink != null)
                                {
                                    string links = "The links below have been added to the ECO:<br/>";
                                    foreach (var link in newLink.Values)
                                    {
                                        links += link + "<br/>";
                                    }
                                    changes.Add(links);
                                }
                                else if (currentLink != null && newLink == null)
                                {
                                    string links = "The links below have been removed from the ECO:<br/>";
                                    foreach (var link in currentLink.Values)
                                    {
                                        links += link + "<br/>";
                                    }
                                    changes.Add(links);
                                }

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
            catch
            {
                return null;
            }
        }
        #endregion

        #region SetSelectLists

        /// <summary>
        /// This sets a List of SelectLists for the approvers that were previously selected
        /// </summary>
        /// <param name="vm">Object storing the values</param>
        /// <returns></returns>
        public List<SelectList> SetSelectListsForApproversEdit(ECRViewModel vm)
        {
            var allRequest = _context.RequestTypes.ToList();
            List<SelectList> approversList = new List<SelectList>();
            foreach (var request in allRequest)
            {
                var Approvers = userRoleRepository.GetApproversWithType(request.Id, 0); //get available approvers for this change
                var RequestGroup = new SelectListGroup { Name = request.Name };
                var approvers = new List<SelectListItem>();
                approvers.AddRange(Approvers.Select(item => new SelectListItem
                {
                    Text = item.User.Name,
                    Value = item.Id.ToString(),
                    Group = RequestGroup
                }).OrderBy(a => a.Group.Name).ToList()); //order by RequestType.Name

                SelectList DropDownList = new SelectList(approvers, "Value", "Text", null, "Group.Name"); //create a selectlist from the collection
                if (vm.Approvers != null)
                {
                    foreach (var selectedApprover in vm.Approvers)
                    {
                        var a = DropDownList.GetEnumerator();
                        while (a.MoveNext())
                        {
                            if (Convert.ToInt64(a.Current.Value) == selectedApprover.UserRoleId)
                            {
                                a.Current.Selected = true; //select the ones that are already a part of the change
                            }
                        }
                    }
                }
                approversList.Add(DropDownList);
            }
            return approversList;
        }

        public SelectList SetSelectListForNotifications(ECRViewModel vm)
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
            return ddl;
        }

        public List<RequestTypeChangeCheckBox> SetAreasAffectedEdit(ECRViewModel vm)
        {
            List<RequestTypeChangeCheckBox> r1 = new List<RequestTypeChangeCheckBox>();
            var allRequest = _context.RequestTypes.ToList(); //gets a collection of RequestTypes
            for (int i = 0; i < allRequest.Count(); i++)
            {
                RequestTypeChangeCheckBox request = new RequestTypeChangeCheckBox(allRequest[i])
                {
                    Selected = false
                }; //new checkbox for Request Type 
                foreach (var item in vm.AreasAffected) //if areas affected include the current request type, select it
                {
                    if (request.Name == item.Name)
                    {
                        request.Selected = true;
                    }
                }
                r1.Add(request);
            }
            return r1;
        }
        #endregion

        #region Update Middle Tables

        /// <summary>
        /// Method to update RequestECR table. Will remove any AreasAffected that were unselected by User in the View.
        /// </summary>
        /// <param name="newAreas">List of Areas selected</param>
        /// <param name="ecrId">Current ECR being updated</param>
        public void UpdateRequestECRTable(ICollection<RequestTypeECR> newAreas, int ecrId)
        {
            List<int> temp = newAreas.Select(i => i.RequestTypeId).ToList();  //get a list of the new Ids
            IQueryable<RequestTypeECR> remove = _context.RequestTypeECRs.Where(r => !temp.Contains(r.RequestTypeId) && r.ECRId == ecrId); //if the existing list of UserRoleECR does not contain the ids of the new areas, put them in the remove list
            if (remove != null)
            {
                _context.RequestTypeECRs.RemoveRange(remove);
            }
            _context.SaveChanges();
        }
        /// <summary>
        /// Method to update UserRoleECR table. Will remove any Approvers that were unselected by User in the View.
        /// </summary>
        /// <param name="newApprovers">List of Approvers selected</param>
        /// <param name="ecrId">Current ECR being updated</param>
        public void UpdateUserRoleECRTable(ICollection<UserRoleECR> newApprovers, int ecrId)
        {
            var existingApproversECR = _context.UserRoleECRs.Where(e => e.ECRId == ecrId).ToList(); //get a list of the new Ids
            List<int> temp = newApprovers.Select(i => i.UserRoleId).ToList(); //if the existing list of UserRoleECR does not contain the ids of the new approvers, put them in the remove list
            var remove = _context.UserRoleECRs.Where(r => !temp.Contains(r.UserRoleId) && r.ECRId == ecrId);
            if (remove != null)
            {
                _context.UserRoleECRs.RemoveRange(remove);
            }
            _context.SaveChanges();
        }
        /// <summary>
        /// Method to update the ProductECR table. Will remove any Products that were unselected by the User in the View.
        /// </summary>
        /// <param name="newProducts">List of Products selected</param>
        /// <param name="ecrId">Change Id</param>
        public void UpdateProductECRTable(ICollection<ProductECR> newProducts, int ecrId)
        {
            List<int> temp = newProducts.Select(i => i.ProductId).ToList(); //get a list of ProductECRIds
            IQueryable<ProductECR> remove = _context.ProductECRs.Where(r => !temp.Contains(r.ProductId) && r.ECRId == ecrId); //if the existing list of ProductECR does not contain the ids of the new products, put them in the remove list
            if (remove != null)
            {
                _context.ProductECRs.RemoveRange(remove);
            }
            _context.SaveChanges();
        }
        /// <summary>
        /// Method to update Notification table. Will remove any Notifications that were unselected by User in the View
        /// </summary>
        /// <param name="notifications">List of Notifications selected</param>
        /// <param name="ecrId">Current ECR being updated</param>
        public void UpdateNotificationTable(ICollection<Notifications> notifications, int ecrId)
        {
            List<int> temp = notifications.Select(i => i.Id).ToList(); //get a list of the new Ids
            var remove = _context.Notifications.Where(r => !temp.Contains(r.Id) && r.ECRId == ecrId);  //if the existing list of Notifications does not contain the ids of the new Notifications, put them in the remove list
            if (remove != null)
            {
                _context.Notifications.RemoveRange(remove);
            }
            _context.SaveChanges();
        }
        #endregion

        #region Update Record

        /// <summary>
        /// Method to update an Existing ECR with the values from the View.
        /// </summary>
        /// <param name="existingECR">old ECR</param>
        /// <param name="vmECR">new ECR</param>
        public void UpdateExistingECR(ECR existingECR, ECRViewModel vmECR, string[] ApproversList)
        {
            int ECRId = existingECR.Id;
            //check for notifications to be sent
            if (existingECR.Description != vmECR.Description)
            {
                changedDescriptionECR = $"Current Description : {existingECR.Description} ; New Description : {vmECR.Description} ";
                existingECR.Description = vmECR.Description;
                sendNotificationOnDescriptionChange = true;
            }
            if (existingECR.ReasonForChange != vmECR.ReasonForChange)
            {
                reasonForChangeECR = $"Current Reason For Change : {existingECR.ReasonForChange} ; New Reason For Change : {vmECR.ReasonForChange} ";
                existingECR.ReasonForChange = vmECR.ReasonForChange;
                sendNotificationOnReasonChange = true;
            }
            if (existingECR.PlannedImplementationDate != vmECR.PlannedImplementationDate)
            {
                implementationDateChangedECR = $"Current Implementation Date : {existingECR.PlannedImplementationDate} ; New Implementation Date : {vmECR.PlannedImplementationDate} ";
                existingECR.PlannedImplementationDate = vmECR.PlannedImplementationDate;
                sendNotificationOnImplementationDateChange = true;
            }
            existingECR.ImplementationType = vmECR.ImplementationType;
            existingECR.OriginatorId = vmECR.OriginatorId;
            existingECR.PermanentChange = vmECR.PermanentChange;
            existingECR.PriorityLevel = vmECR.PriorityLevel;
            existingECR.ProductValidationTestingRequired = vmECR.ProductValidationTestingRequired;
            existingECR.CustomerImpact = vmECR.CustomerImpact;
            existingECR.BOMRequired = vmECR.BOMRequired;
            existingECR.LinkUrls = vmECR.LinkUrls;
            existingECR.AreasAffected = SetAreasAffectedECR(vmECR, ECRId); //call the method and not existingECR.AreasAffected because the changes were not saved yet
            existingECR.AffectedProducts = SetAffectedProductsECR(vmECR.AffectedProductsIds, ECRId);
            existingECR.PreviousRevision = vmECR.PreviousRevision;
            existingECR.NewRevision = vmECR.NewRevision;
            UpdateRequestECRTable(SetAreasAffectedECR(vmECR, ECRId), ECRId);
            existingECR.Notifications = SetNotificationsECR(vmECR, ECRId, ApproversList);
            UpdateNotificationTable(SetNotificationsECR(vmECR, ECRId, ApproversList), ECRId);
            UpdateProductECRTable(SetAffectedProductsECR(vmECR.AffectedProductsIds, ECRId), ECRId);
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
                //long list of includes, most relationships are being pulled because we need to display them.
                var eCR = _context.ECRs
                    .Include(e => e.ChangeType)
                    .Include(e => e.Originator)
                    .Include(e => e.AreasAffected)
                        .ThenInclude(e => e.RequestType) //then include access the navigation property inside a navigation property. neat.
                    .Include(e => e.RelatedECOs)
                        .ThenInclude(e => e.ECO)
                    .Include(e => e.Notifications)
                        .ThenInclude(n => n.User)
                    .Include(e => e.Approvers)
                        .ThenInclude(a => a.UserRole)
                            .ThenInclude(u => u.User)
                    .Include(e => e.AffectedProducts)
                        .ThenInclude(a => a.Product)
                    .FirstOrDefault(m => m.Id == id);

                List<ECO> relatedECOs = new List<ECO>();
                foreach (var ecrhaseco in eCR.RelatedECOs) //sets a list of ECRs using the RelatedECRs field for the Partial in the view
                {
                    relatedECOs.Add(ecrhaseco.ECO);
                }

                ViewData["ECOs"] = relatedECOs; //simple viewbag for the relatedECOs Partial.

                List<AuditLog> auditLogs = new List<AuditLog>();
                foreach (var log in auditLogRepository.GetAuditLogsECR(eCR.Id)) //sets a list of AuditLogs for the Partial
                {
                    auditLogs.Add(log);
                }
                ViewData["AuditLog"] = auditLogs;

                if (eCR == null)
                {
                    return NotFound();
                }
                return View(eCR);
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
                + "/ECRs/Print/"
                + ID.ToString();
            PdfDocument doc = converter.ConvertUrl(url);

            byte[] pdfBytes = doc.Save();
            MemoryStream ms = new MemoryStream(pdfBytes);
            string fileName = "ECR" + ID + ".pdf";
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
        public async Task<IActionResult> ReminderEmail(int id)
        {
            var eCR = await _context.ECRs
                .Include(e => e.ChangeType)
                .Include(e => e.Originator)
                .Include(e => e.AreasAffected)
                    .ThenInclude(e => e.RequestType) //then include access the navigation property inside a navigation property. neat.
                .Include(e => e.RelatedECOs)
                    .ThenInclude(e => e.ECO)
                .Include(e => e.Notifications)
                    .ThenInclude(n => n.User)
                .Include(e => e.Approvers)
                    .ThenInclude(a => a.UserRole)
                        .ThenInclude(u => u.User)
                .Include(e => e.AffectedProducts)
                    .ThenInclude(a => a.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            string message = $"Please check if there is any action needed for the ECR {id} ";
            notificationSenderRepository.SendReminderEmailECR(id, message);
            TempData["ReminderEmailConfirmation"] = "A reminder email has been sent to everyone associated with this ECR";
            return RedirectToAction("Details", eCR);
        }
        #endregion

        #region Add/Remove To Nofication List
        public async Task<IActionResult> AddToNotification(int id)
        {
            var loggedInUser = GetLoggedInUser();
            var notification = middleTablesRepository.GetNotificationByUserAndECR(loggedInUser.Id, id);

            if (notification == null)
            {
                User NotificationUser = _context.Users.Where(u => u.Id == loggedInUser.Id).FirstOrDefault(); //get the User to access it's name later in the sendNotification
                Notifications NewNotification = new Notifications()
                {
                    ECRId = id,
                    UserId = loggedInUser.Id,
                    User = NotificationUser,
                    Option = NotificationOption.AnyChange //the user will always be notified on any change, unless they change it in the main page
                };
                _context.Add(NewNotification);
                _context.SaveChanges();

                //List<string> message = new List<string>();
                //message.Add($"{loggedInUser.Name} has been added to the notification list");
                //notificationSenderRepository.SendNotificationOnAnyChangeECR(id, message);
            }
            else
            {
                var removeUser = _context.Notifications.Where(u => u.UserId == loggedInUser.Id && u.ECRId == id).FirstOrDefault();
                _context.Notifications.RemoveRange(removeUser);
                _context.SaveChanges();
                //List<string> message = new List<string>();
                //message.Add($"{loggedInUser.Name} has been removed from the notification list");
                //notificationSenderRepository.SendNotificationOnAnyChangeECR(id, message);
            }

            return RedirectToAction(nameof(Details), new { id = id });
        }
        #endregion

    }
}
