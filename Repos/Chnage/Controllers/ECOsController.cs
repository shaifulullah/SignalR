using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Chnage.Helpers;
using Chnage.Models;
using Chnage.Public_Classes;
using Chnage.Repository;
using Chnage.Services;
using Chnage.ViewModel;
using Chnage.ViewModel.ECO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SelectPdf;
namespace Chnage.Controllers
{
    public class ECOsController : Controller
    {
        #region Variables
        private readonly MyECODBContext _context; //context of the database
        private readonly IUserRole userRoleRepository; //repository for UserRole functions
        private readonly IMiddleTables middleTablesRepository; //repository for all middle table functions
        private readonly IAuditLog auditLogRepository; //repository for the AuditLog functions
        private readonly INotificationSender notificationSenderRepository; //repository for the notifications
        private readonly IProduct productRepository;
        private readonly IUser userRepository;
        //bools to check when to send notifications
        private bool sendNotificationOnDescriptionChange = false;
        private bool sendNotificationOnReasonChange = false;
        private bool sendNotificationOnImplementationDateChange = false;
        private bool sendNotificationOnApproversChange = false;
        private bool sendNotificationStatusChange = false;
        private User loggedUser;
        private string action = "";
        private string controller = "";
        private readonly HttpContext _currentContext;
        private string changedDescriptionECO, reasonForChangeECO, implementationDateChangedECO, ChangeStatusECO;
        List<string> approversChangedECO;
        #endregion

        #region Constructor and Override

        /// <summary>
        /// Constructor. Dependency Injection happens with the parameters.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userRole"></param>
        /// <param name="middleTables"></param>
        /// <param name="auditLog"></param>
        /// <param name="notificationSender"></param>
        public ECOsController(MyECODBContext context, IUserRole userRole, IUser user, IMiddleTables middleTables,
            IAuditLog auditLog, INotificationSender notificationSender, IProduct product,
            IHttpContextAccessor _IHttpContextAccessor) //sets the readonly fields with dependency injection from StartUp.cs
        {
            _context = context;
            userRoleRepository = userRole;
            middleTablesRepository = middleTables;
            auditLogRepository = auditLog;
            notificationSenderRepository = notificationSender;
            productRepository = product;
            _currentContext = _IHttpContextAccessor.HttpContext;
            userRepository = user;
        }

        public override void OnActionExecuting(ActionExecutingContext context) //when any action in the controller is called, do this
        {
            base.OnActionExecuting(context);
            action = this.ControllerContext.RouteData.Values["action"].ToString();
            controller = this.ControllerContext.RouteData.Values["controller"].ToString();
            if (!action.ToLower().Contains("print"))
            {
                loggedUser = GetLoggedInUser();
            }//assigns a value to the loggedUser variable
        }
        #endregion

        #region Gets!
        /// <summary>
        /// Access the User.Identity to get the logged user's email
        /// </summary>
        /// <returns>an Email xxx@geotab.com</returns>
        private string GetLoggedInUserEmail() //gets the email of the current logged user
        {
            string userEmail = "shaifulullah@gmail.com";
            //string userEmail = "";
            //if (User.Identity.IsAuthenticated)
            //{
            //    userEmail = (User.Identity as System.Security.Claims.ClaimsIdentity)?.Claims.ElementAt(2).Value; //ElementAt(2) is email and (1) is Name
            //}
            return userEmail;
        }
        private string GetLoggedInUsername()
        {
            string name = "";
            if (User.Identity.IsAuthenticated)
            {
                name = "Shaiful Ullah";
                //name = (User.Identity as System.Security.Claims.ClaimsIdentity)?.Claims.ElementAt(1).Value;
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
                    string geotabDomain = "gmail.com";
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

        public bool CheckIfUserInNotificationList(ECO eco)
        {
            bool isUserInNotification = false;
            var loggedInUser = GetLoggedInUser();
            if (loggedInUser!= null && eco.Notifications != null)
            {
                foreach (var user in eco.Notifications)
                {
                    if (user.UserId == loggedInUser.Id)
                    {
                        isUserInNotification = true;
                    }

                }
            }
            return isUserInNotification;
        }
        /// <summary>
        /// Checks if the current user is an approver for the current ECO.
        /// </summary>
        /// <param name="ECOId">Id of the ECO</param>
        /// <returns></returns>
        public bool CheckLoggedUserIsApprover(int ECOId)
        {
            UserRoleECO userrole = null;
            if (loggedUser != null)
            {
                userrole = middleTablesRepository.GetExistingUserRoleECOByUser(loggedUser.Id, ECOId);
            }
            if (userrole != null)
            {
                if (userrole.UserRole.RoleInt == Role.Approver)
                {
                    if (userrole.Approval == null) //if the approver has not approved it yet
                    {
                        return true;
                    }
                }
                return false;  //this false means that he made a choice already
            }
            return false;
        }
        public bool CheckLoggedUserIsValidator(int ECOId)
        {
            UserRoleECO userrole = null;
            if (loggedUser != null)
            {
                userrole = middleTablesRepository.GetExistingUserRoleECOByUser(loggedUser.Id, ECOId);
            }
            if (userrole != null)
            {
                if (userrole.UserRole.RoleInt == Role.Validator)
                {
                    if (userrole.Approval == null) //if the approver has not approved it yet
                    {
                        return true;
                    }
                }
                return false;  //this false means that he made a choice already
            }
            return false;
        }
        /// <summary>
        /// Checks if the current logged user is the originator for the ECO
        /// </summary>
        /// <param name="ECOId">Id of the ECO</param>
        /// <returns></returns>
        public bool CheckLoggedUserIsOriginator(int ECOId)
        {
            var originatorId = _context.ECOs.Where(e => e.Id == ECOId).Select(a => a.OriginatorId).FirstOrDefault();
            if (loggedUser != null)
            {
                if (loggedUser.Id == originatorId)
                {
                    return true;
                }
            }
            return false;
        }
        private bool CheckECOisValidated(int id)
        {
            bool validated = true;
            var Validatorsgroup = _context.ECOs.Where(eco => eco.Id == id).Select(a => a.Approvers.Where(v => v.UserRole.RoleInt == Role.Validator));
            foreach (var validator in Validatorsgroup)
            {
                foreach (var val in validator)
                {
                    if (val.Approval != true)
                    {
                        validated = false;
                    }
                }
            }
            return validated;
        }
        private bool ECOExists(int id)
        {
            return _context.ECOs.Any(e => e.Id == id);
        }

        private bool CheckIfECOWasRejected(int id)
        {
            bool rejected = false;
            var isRejected = _context.ECOs.Where(eco => eco.Id == id).Select(r => r.Status).FirstOrDefault();
            if (isRejected == StatusOptions.RejectedApproval)
            {
                rejected = true;
            }
            return rejected;
        }
        #endregion

        #region Details
        // GET: ECOs/Details/5
        public async Task<IActionResult> Details(int? id) //Details page
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var eCO = await _context.ECOs
                    .Include(e => e.ChangeType)
                    .Include(e => e.Originator)
                    .Include(e => e.AreasAffected)
                        .ThenInclude(a => a.RequestType)
                    .Include(e => e.RelatedECRs)
                        .ThenInclude(r => r.ECR)
                    .Include(e => e.Notifications)
                        .ThenInclude(n => n.User)
                    .Include(e => e.Approvers)
                        .ThenInclude(a => a.UserRole)
                            .ThenInclude(u => u.User)
                    .Include(e => e.AffectedProducts)
                        .ThenInclude(a => a.Product)
                    .FirstOrDefaultAsync(m => m.Id == id); //gets all the related information from the connecting tables

                bool loggedUserIsApprover = CheckLoggedUserIsApprover(eCO.Id);
                bool loggedUserIsValidator = CheckLoggedUserIsValidator(eCO.Id);
                bool loggedUserIsOriginator = CheckLoggedUserIsOriginator(eCO.Id);
                bool validated = CheckECOisValidated(eCO.Id);
                bool wasRejected = CheckIfECOWasRejected(eCO.Id);
                bool isUserInNotification = CheckIfUserInNotificationList(eCO);

                ViewData["ChangeValidated"] = validated;
                ViewData["LoggedUserIsValidator"] = loggedUserIsValidator;
                ViewData["LoggedUserIsApprover"] = loggedUserIsApprover;
                ViewData["LoggedUserIsOriginator"] = loggedUserIsOriginator;
                ViewData["ECOWasRejected"] = wasRejected;
                ViewData["IsUserInNotificatonList"] = isUserInNotification;

                if (!string.IsNullOrWhiteSpace(TempData["ReminderEmailConfirmation"] as string))
                {
                    ViewData["ReminderEmailConfirmation"] = TempData["ReminderEmailConfirmation"];
                }

                List<ECR> relatedECRs = new List<ECR>();
                foreach (var ecohasecr in eCO.RelatedECRs) //sets a list of ECRs using the RelatedECRs field for the Partial in the view
                {
                    relatedECRs.Add(ecohasecr.ECR);
                }

                ViewData["ECRs"] = relatedECRs; //simple viewbag for the relatedECOs Partial.

                #region Populate Audit View

                List<Audit> logsFromAuditTable = new List<Audit>();
                var logId = eCO.Id;

                #region Audit - Get ECO Table

                var logList = _context.Audits.Where(e => e.TableName == "ECOs" && e.EntityId == logId.ToString()).ToList()
                            .Where(g => g.OldData != g.NewData);

                foreach (var log in logList)
                {
                    if (log.TableName == "ECOs")
                    {
                        if (log.OldData == null && log.NewData != null)
                        {
                            List<string> ecoLog = new List<string>();
                            var newData = JsonConvert.DeserializeObject<ECO>(log.NewData);

                            if (newData.ChangeTypeId != -1)
                            {
                                var changedType = _context.RequestTypes.Where(i => i.Id == newData.ChangeTypeId).Select(i => i.Name).FirstOrDefault();
                                ecoLog.Add("ChangeType:" + changedType);
                            }
                            else
                            { ecoLog.Add("ChangeType:null"); }

                            if (newData.LinkUrls != null)
                            {
                                foreach (var links in newData.LinkUrls)
                                {
                                    ecoLog.Add("LinkUrl:" + links.Value.ToString());
                                }
                            }
                            else
                            { ecoLog.Add("LinkUrl:null"); }

                            if (newData.NotesForApprover != null)
                            {
                                foreach (var notes in newData.NotesForApprover)
                                {
                                    ecoLog.Add("NotesForApprover:" + notes);
                                }
                            }
                            else
                            { ecoLog.Add("NotesForApprover:null"); }

                            if (newData.NotesForValidator != null)
                            {
                                foreach (var notes in newData.NotesForValidator)
                                {
                                    ecoLog.Add("NotesForValidator:" + notes);
                                }
                            }
                            else
                            { ecoLog.Add("NotesForValidator:null"); }

                            if (newData.OriginatorId != -1)
                            {
                                var originatorName = _context.Users.Where(u => u.Id == newData.OriginatorId).Select(u => u.Name).FirstOrDefault();
                                ecoLog.Add("OriginatorName:" + originatorName);
                            }
                            else
                            { ecoLog.Add("OriginatorName:null"); }

                            log.NewData = Helper.GetStructuredJsonInfo(newData);
                            log.NewData += string.Join("<br/>", ecoLog);

                        }
                        if (log.Action == EntityState.Modified.ToString())
                        {
                            List<string> oldECOlog = new List<string>();
                            List<string> newECOlog = new List<string>();
                            var oldData = JsonConvert.DeserializeObject<ECO>(log.OldData);
                            var newData = JsonConvert.DeserializeObject<ECO>(log.NewData);

                            var changedColumns = log.ChangedColumns.Split(',').ToList();

                            if (changedColumns.Contains("LinkUrls"))
                            {
                                if (newData.LinkUrls != null)
                                {
                                    foreach (var links in newData.LinkUrls)
                                    {
                                        newECOlog.Add("LinkUrl:" + links.Value.ToString());
                                    }
                                }
                                else
                                { newECOlog.Add("LinkUrl: "); }

                                if (oldData.LinkUrls != null)
                                {
                                    foreach (var links in oldData.LinkUrls)
                                    {
                                        oldECOlog.Add("LinkUrl:" + links.Value.ToString());
                                    }
                                }
                                else
                                { oldECOlog.Add("LinkUrl: "); }
                            }
                            if (changedColumns.Contains("NotesForApprover"))
                            {
                                if (newData.NotesForApprover != null)
                                {
                                    foreach (var notes in newData.NotesForApprover)
                                    {
                                        newECOlog.Add("NotesForApprover:" + notes);
                                    }
                                }
                                else
                                { newECOlog.Add("NotesForApprover: "); }

                                if (oldData.NotesForApprover != null)
                                {
                                    foreach (var notes in newData.NotesForApprover)
                                    {
                                        oldECOlog.Add("NotesForApprover:" + notes);
                                    }
                                }
                                else
                                { oldECOlog.Add("NotesForApprover: "); }
                            }
                            if (changedColumns.Contains("NotesForValidator"))
                            {
                                if (newData.NotesForValidator != null)
                                {
                                    foreach (var notes in newData.NotesForValidator)
                                    {
                                        newECOlog.Add("NotesForValidator:" + notes);
                                    }
                                }
                                else
                                { newECOlog.Add("NotesForValidator: "); }

                                if (oldData.NotesForValidator != null)
                                {
                                    foreach (var notes in newData.NotesForValidator)
                                    {
                                        oldECOlog.Add("NotesForValidator:" + notes);
                                    }
                                }
                                else
                                { oldECOlog.Add("NotesForValidator: "); }
                            }
                            if (changedColumns.Contains("ChangeTypeId"))
                            {
                                if (newData.ChangeTypeId != -1)
                                {
                                    var changedType = _context.RequestTypes.Where(i => i.Id == newData.ChangeTypeId).Select(i => i.Name).FirstOrDefault();
                                    newECOlog.Add("ChangeType:" + changedType);
                                }
                                else
                                { newECOlog.Add("ChangeType: "); }

                                if (oldData.ChangeTypeId != -1)
                                {
                                    var changedType = _context.RequestTypes.Where(i => i.Id == oldData.ChangeTypeId).Select(i => i.Name).FirstOrDefault();
                                    oldECOlog.Add("ChangeType:" + changedType);
                                }
                                else
                                { oldECOlog.Add("ChangeType: "); }
                            }
                            if (changedColumns.Contains("OriginatorId"))
                            {
                                if (newData.OriginatorId != -1)
                                {
                                    var originatorName = _context.Users.Where(u => u.Id == newData.OriginatorId).Select(u => u.Name).FirstOrDefault();
                                    newECOlog.Add("OriginatorName:" + originatorName);
                                }
                                else
                                { newECOlog.Add("OriginatorName: "); }

                                if (oldData.OriginatorId != -1)
                                {
                                    var originatorName = _context.Users.Where(u => u.Id == oldData.OriginatorId).Select(u => u.Name).FirstOrDefault();
                                    oldECOlog.Add("OriginatorName:" + originatorName);
                                }
                                else
                                { oldECOlog.Add("OriginatorName: "); }
                            }
                            log.OldData = Helper.GetStructuredJsonInfo(oldData, changedColumns);
                            log.NewData = Helper.GetStructuredJsonInfo(newData, changedColumns);

                            log.OldData += string.Join("<br/>", oldECOlog);
                            log.NewData += string.Join("<br/>", newECOlog);
                        }
                        log.TableName = "ECO";
                    }

                    logsFromAuditTable.Add(log);
                }
                #endregion

                #region Audit - Get Related ECR
                var relatedAuditECRs = _context.Audits.Where(e => e.TableName == "ECRHasECOs" && e.EntityId == logId.ToString()).ToList()
                .Where(g => g.OldData != g.NewData).GroupBy(g => new
                {
                    g.EntityId,
                    Date = g.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss"),
                    g.Action
                }).Select(g => new { key = g.Key, value = g.ToList() });

                foreach (var relatedAuditECR in relatedAuditECRs)
                {
                    foreach (var innerValue in relatedAuditECR.value)
                    {
                        if (innerValue.OldData != null)
                        {
                            var oldData = JsonConvert.DeserializeObject<ECRHasECO>(innerValue.OldData);
                            var ecrName = _context.ECRs.Where(e => e.Id == oldData.ECRId).Select(e => e.Description).FirstOrDefault();
                            innerValue.OldData = ecrName;
                        }
                        if (innerValue.NewData != null)
                        {
                            var newData = JsonConvert.DeserializeObject<ECRHasECO>(innerValue.NewData);
                            var ecrName = _context.ECRs.Where(e => e.Id == newData.ECRId).Select(e => e.Description).FirstOrDefault();
                            innerValue.NewData = ecrName;
                        }
                    }
                    var relatedECRAudit = new Audit()
                    {
                        UserName = relatedAuditECR.value.FirstOrDefault().UserName,
                        Action = relatedAuditECR.value.FirstOrDefault().Action,
                        TableName = "Related ECR(s)",
                        EntityId = relatedAuditECR.key.EntityId,
                        OldData = string.Join("<br/>", relatedAuditECR.value.ToList().Where(g => g.OldData != null).Select(g => g.OldData)),
                        NewData = string.Join("<br/>", relatedAuditECR.value.ToList().Where(g => g.NewData != null).Select(g => g.NewData)),
                        CreatedOn = DateTime.Parse(relatedAuditECR.key.Date)
                    };
                    logsFromAuditTable.Add(relatedECRAudit);
                }
                #endregion

                #region Audit - Get User Role Table

                var userRoleECOLogList = _context.Audits.Where(e => e.TableName == "UserRoleECOs" && e.EntityId == logId.ToString()).ToList()
                .Where(g => g.OldData != g.NewData).GroupBy(g => new
                {
                    g.EntityId,
                    Date = g.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss"),
                    g.Action

                }).Select(g => new { key = g.Key, value = g.ToList() });
                foreach (var userRoleLog in userRoleECOLogList)
                {
                    foreach (var innerUserRoleLog in userRoleLog.value)
                    {
                        if (innerUserRoleLog.OldData != null)
                        {
                            var oldRequest = JsonConvert.DeserializeObject<UserRoleECO>(innerUserRoleLog.OldData);
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
                            var newRequest = JsonConvert.DeserializeObject<UserRoleECO>(innerUserRoleLog.NewData);
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
                var notifications = _context.Notifications.Where(n => n.ECOId == logId).Select(x => x.Id.ToString()).ToList();
                List<Audit> notificationLog = new List<Audit>();
                var notificationList = _context.Audits.Where(x => notifications.Contains(x.EntityId) && x.TableName == "Notifications").ToList();

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

                #region Audit - Get Request Type ECO// Areas Effected

                var requestTypeECO = _context.Audits.Where(e => e.TableName == "RequestTypeECOs" && e.EntityId == logId.ToString()).ToList()
                        .Where(g => g.OldData != g.NewData).GroupBy(g => new
                        {
                            g.EntityId,
                            Date = g.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss"),
                            g.Action
                        }).Select(g => new { key = g.Key, value = g.ToList() });

                foreach (var log in requestTypeECO)
                {
                    foreach (var innerLog in log.value)
                    {
                        if (innerLog.OldData != null)
                        {
                            var oldRequest = JsonConvert.DeserializeObject<RequestTypeECO>(innerLog.OldData);
                            var oldRequestType = _context.RequestTypes.Where(r => r.Id == oldRequest.RequestTypeId).Select(r => r.Name).FirstOrDefault();
                            innerLog.OldData = oldRequestType;
                        }
                        if (innerLog.NewData != null)
                        {
                            var newRequest = JsonConvert.DeserializeObject<RequestTypeECO>(innerLog.NewData);
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
                        OldData = (string.Join(",", log.value.ToList().Where(g => g.OldData != null).Select(g => g.OldData))),
                        NewData = (string.Join(",", log.value.ToList().Where(g => g.NewData != null).Select(g => g.NewData))),
                        CreatedOn = DateTime.Parse(log.key.Date)
                    };
                    logsFromAuditTable.Add(Audit);
                }
                #endregion

                #region Audit - Get Affected Product List
                var productECOLogList = _context.Audits.Where(e => e.TableName == "ProductECOs" && e.EntityId == logId.ToString()).ToList()
                        .Where(g => g.OldData != g.NewData).GroupBy(g => new
                        {
                            g.EntityId,
                            Date = g.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss"),
                            g.Action

                        }).Select(g => new { key = g.Key, value = g.ToList() });

                //.Select(x => new Audit { UserName = x.FirstOrDefault().UserName, Action = x.FirstOrDefault().Action, 
                //    TableName = "ProductECO", EntityId = x.Key.EntityId, OldData = string.Join(":",x.ToList().Where(g=>g.OldData!=null).Select(g=>g.OldData)), 
                //    NewData = string.Join(";",x.ToList().Select(g=>g.NewData)), CreatedOn = DateTime.Parse(x.Key.Date)}).ToList();
                foreach (var log in productECOLogList)
                {
                    foreach (var innerLog in log.value)
                    {
                        if (innerLog.OldData != null)
                        {
                            var oldProduct = JsonConvert.DeserializeObject<ProductECO>(innerLog.OldData);
                            var oldProductName = _context.Products.Where(p => p.Id == oldProduct.ProductId).Select(p => p.Name + "-" + p.Category).FirstOrDefault();
                            innerLog.OldData = oldProductName;
                        }
                        if (innerLog.NewData != null)
                        {
                            var newProduct = JsonConvert.DeserializeObject<ProductECO>(innerLog.NewData);
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
                    foreach (var log in auditLogRepository.GetAuditLogsECO(eCO.Id)) //sets a list of AuditLogs for the Partial
                    {
                        auditLogs.Add(log);
                    }
                    ViewData["AuditLogECO"] = auditLogs;
                }
                else
                {
                    ViewData["AuditLog"] = logsFromAuditTable;
                }

                #endregion

                if (eCO == null)
                {
                    return NotFound();
                }

                return View(eCO);
            }
            catch (Exception ex)
            {
                ViewModelError vm = SetViewModelError(ex);
                return RedirectToAction("ErrorPage", "ErrorHandler", vm);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details([Bind("ApproverOption, UserRoleId, ChangeId, RejectReason")] ApproverViewModel vm) //when an approver chooses an option (Reject or Approve) for the Change
        {
            try
            {
                List<string> changes = new List<string>();
                List<UserRoleECO> Approvers = middleTablesRepository.GetUserRolesForECO(vm.ChangeId).ToList(); //gets all approvers for the ECO
                foreach (var approver in Approvers)
                {
                    UserRoleECO existingUserRoleECO = middleTablesRepository.GetExistingUserRoleECO(approver.UserRoleId, vm.ChangeId);
                    if (loggedUser.Id == userRoleRepository.GetUserByUserRoleId(existingUserRoleECO.UserRoleId).Id)
                    {
                        //UserRoleECR that made the choice
                        existingUserRoleECO.Approval = vm.ApproverOption; //sets the approval to the option
                        existingUserRoleECO.AprovedDate = DateTime.Now.ToString();
                        _context.Update(existingUserRoleECO);
                        await _context.SaveChangesAsync();
                        string status = vm.ApproverOption == true ? "approved" : "rejected";
                        changes.Add(loggedUser.Name + " has " + status + " The ECO");
                    }
                }

                ECO currentECO = _context.ECOs.Where(c => c.Id == vm.ChangeId).FirstOrDefault(); //gets the ECO object

                StatusOptions currentStatus = currentECO.Status; //sets a log for the Status (in case it changes)
                currentECO.RejectReason = vm.RejectReason;
                if (Approvers.Any(a => a.Approval == null)) //if any of the approvers hasn't approved yet
                {
                    currentECO.Status = StatusOptions.AwaitingApproval;
                }

                var app = Approvers.Where(a => a.UserRole.RoleInt == Role.Approver);
                var val = Approvers.Where(a => a.UserRole.RoleInt == Role.Validator);
                if (val.Any(a => a.Approval == false)) //if any of the approvers rejected it, reject the change.
                {
                    currentECO.Status = StatusOptions.RejectedValidation;
                }
                else if (app.Any(a => a.Approval == false)) //if any of the approvers rejected it, reject the change.
                {
                    currentECO.Status = StatusOptions.RejectedApproval;
                }
                else if (Approvers.TrueForAll(a => a.Approval == true)) //if all the approvers approved it, approve the change.
                {
                    currentECO.Status = StatusOptions.Approved;
                    currentECO.ClosedDate = DateTime.Now; //set the ClosedDate to now
                }

                if (currentECO.Status == StatusOptions.Approved)
                {
                    //changes.Add("Change was Approved");
                    notificationSenderRepository.SendNotificationOnApprovalECO(currentECO.Id, $"ECO {currentECO.Id} has been <span style='background-color:lightgreen'>Approved</span>"); //send a notification if it has been approved
                }
                if (currentStatus != currentECO.Status)
                {
                    string CurrentBackgroundColor = "white";
                    string NewBackgroundColor = "white";

                    if (currentStatus == StatusOptions.AwaitingApproval)
                    {
                        CurrentBackgroundColor = "orange";
                    }
                    else if (currentStatus == StatusOptions.RejectedApproval || currentStatus == StatusOptions.RejectedValidation)
                    {
                        CurrentBackgroundColor = "lightcoral";
                    }
                    else if (currentStatus == StatusOptions.Approved)
                    {
                        CurrentBackgroundColor = "lightgreen";
                    }
                    else
                    {
                        CurrentBackgroundColor = "white";
                    }
                    if (currentECO.Status == StatusOptions.AwaitingApproval)
                    {
                        NewBackgroundColor = "orange";
                    }
                    else if (currentECO.Status == StatusOptions.RejectedApproval || currentECO.Status == StatusOptions.RejectedValidation)
                    {
                        NewBackgroundColor = "lightcoral";
                    }
                    else if (currentECO.Status == StatusOptions.Approved)
                    {
                        NewBackgroundColor = "lightgreen";
                    }
                    else
                    {
                        NewBackgroundColor = "white";
                    }
                    ChangeStatusECO = $" Current Status : <span style='background-color:{CurrentBackgroundColor}'> {currentStatus} </span> ; New Status : <span style='background-color:{NewBackgroundColor}'> {currentECO.Status} </span>";
                    changes.Add($"ECO status has Changed - {ChangeStatusECO}");
                    notificationSenderRepository.SendNotificationOnStatusChangeECO(currentECO.Id, ChangeStatusECO); //send a notification if the status has changed
                }

                notificationSenderRepository.SendNotificationOnAnyChangeECO(currentECO.Id, changes); //send a notification on any change -> approver selected an option is a change

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = vm.ChangeId }); //go back to the details page
            }
            catch (Exception ex)
            {
                ViewModelError vmerror = SetViewModelError(ex);
                return RedirectToAction("ErrorPage", "ErrorHandler", vmerror);

            }
        }
        #endregion

        #region SetViewData and ViewModel
        /// <summary>
        /// This will generate the ViewModel for most pages that need it.
        /// First we get the loggedUser using the application, then a List for the AreasAffected is created.
        /// Then a List<SelectList> is created for the available approvers.
        /// It works by iterating over the requests, then getting the approvers for each request and adding it to a SelectList
        /// The SelectList then is added to approversList.
        /// </summary>
        /// <returns>ECOViewModel with information from related tables</returns>
        public ECOViewModel SetViewModel()
        {
            ECOViewModel vm = new ECOViewModel(); //new viewmodel
            List<RequestTypeChangeCheckBox> r1 = new List<RequestTypeChangeCheckBox>();
            List<RequestType> allRequest = _context.RequestTypes.ToList(); //gets all possible request types
            for (int i = 0; i < allRequest.Count; i++)
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
                IQueryable<UserRole> Approvers = userRoleRepository.GetApproversWithType(request.Id, loggedUser.Id); //get the available approvers for the request, but not the logged user or any inactive users
                SelectListGroup RequestGroup = new SelectListGroup { Name = request.Name }; //the name of the Group they are a part of 
                List<SelectListItem> approvers = new List<SelectListItem>();
                approvers.AddRange(Approvers.Select(item => new SelectListItem() //adds the collection of Approvers to a List of SelectListItems
                {
                    Text = item.User.Name,
                    Value = item.Id.ToString(),
                    Group = RequestGroup
                }).OrderBy(a => a.Group.Name).ToList());
                SelectList ddl = new SelectList(approvers, "Value", "Text", -1, "Group.Name"); //use the items to generate a SelectList for the <select> element
                approversList.Add(ddl);
            }
            vm.ApproversList = approversList; //ApproversList is a List<SelectList> just made ^ (fresh out of the foreach)

            List<SelectList> validatorsList = new List<SelectList>();
            foreach (var request in allRequest)
            {
                IQueryable<UserRole> Approvers = userRoleRepository.GetValidatorsWithType(request.Id, loggedUser.Id); //get the available approvers for the request, but not the logged user or any inactive users
                SelectListGroup RequestGroup = new SelectListGroup { Name = request.Name }; //the name of the Group they are a part of 
                List<SelectListItem> validators = new List<SelectListItem>();
                validators.AddRange(Approvers.Select(item => new SelectListItem() //adds the collection of Approvers to a List of SelectListItems
                {
                    Text = item.User.Name,
                    Value = item.Id.ToString(),
                    Group = RequestGroup
                }).OrderBy(a => a.Group.Name).ToList());
                SelectList ddl = new SelectList(validators, "Value", "Text", -1, "Group.Name"); //use the items to generate a SelectList for the <select> element
                validatorsList.Add(ddl);
            }
            vm.ValidatorsList = validatorsList;
            Dictionary<string, string> appkeyValuePairs = new Dictionary<string, string>();
            foreach (var dep in allRequest) //same deal of approvers but with only the Request as a Department
            {
                appkeyValuePairs.Add(dep.Name, "Notes for " + dep.Name + " Approvers");
            }
            vm.NotesForApproverIds = appkeyValuePairs;
            Dictionary<string, string> valkeyValuePairs = new Dictionary<string, string>();
            foreach (var dep in allRequest) //same deal of approvers but with only the Request as a Department
            {
                valkeyValuePairs.Add(dep.Name, "Notes for " + dep.Name + " Validators");
            }
            vm.NotesForValidatorIds = valkeyValuePairs;

            List<ECR> approvedECRs = _context.ECRs.Where(e => e.Status == StatusOptions.Approved).Include(e => e.ChangeType).Include(e => e.Originator).ToList(); // get all ECRS that have been approved
            List<SelectListItem> ecrs = new List<SelectListItem>();
            for (int i = 0; i < approvedECRs.Count; i++)
            {
                ECR ecr = approvedECRs[i];
                ecrs.Add(new SelectListItem
                {
                    Text = ecr.Id + " - " + ecr.Description + " - " + ecr.Originator.Name,
                    Value = ecr.Id.ToString(),
                    Group = new SelectListGroup { Name = ecr.ChangeType.Name }
                }); //adds to the list of SelectListItems
            }
            vm.RelatedECRs = new SelectList(ecrs, "Value", "Text", -1, "Group.Name"); //use the List of SelectListItems to create a new SelectList for the view

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

        // GET: ECOs/Create
        /// <summary>
        /// Get Method to display the Create View. helps setting the viewdata and viewmodel
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            var users = _context.Users.Where(ur => ur.isActive);
            ViewData["ChangeTypeId"] = new SelectList(_context.RequestTypes, "Id", "Name");
            ViewData["AllUsers"] = new SelectList(users, "Id", "Name");
            ViewData["OriginatorId"] = new SelectList(_context.Users.Where(u => u == loggedUser), "Id", "Name");
            ECOViewModel vm = SetViewModel(); //set the view model with all the information from different tables
            return View(vm);
        }

        // POST: ECOs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PermanentChange,Description,ReasonForChange,ChangeTypeId,BOMRequired,ProductValidationTestingRequired,PlannedImplementationDate,CreatedDate,ClosedDate,CustomerApproval," +
            "PriorityLevel,Originator,OriginatorId,ImplementationType,Status,NotesForApproverIds,NotesForValidatorIds,AreasAffected,UsersToBeNotified,RelatedECRIds,LinkUrls,AffectedProductsIds,PreviousRevision,NewRevision"+
            "DeviationSelected","DeviationQuantity","DeviationDate")] ECOViewModel vmECO, string[] ApproversList, string[] ValidatorsList)
        { //POST request with all values from the page
            if (ModelState.IsValid)
            {
                try
                {
                    vmECO.Originator = userRepository.GetUserFromId(vmECO.OriginatorId);
                    ECO NewECO = new ECO(vmECO); //create a new ECO object from the values provided
                    _context.Add(NewECO); //add to the ChangeTracker
                    await _context.SaveChangesAsync(); //save it to the database

                    int ECOId = NewECO.Id;

                    //going to the method will explain what it does

                    List<RequestTypeECO> requestList = SetAreasAffectedECO(vmECO, ECOId);

                    List<UserRoleECO> userRoleECOList = SetUserRoleECO(ApproversList, ECOId, ValidatorsList, false);

                    List<Notifications> notifications = SetNotificationsECO(vmECO, ECOId, ApproversList, ValidatorsList);

                    List<ProductECO> affectedProducts = SetAffectedProductsECO(vmECO.AffectedProductsIds, ECOId);

                    if (vmECO.RelatedECRIds != null)
                    {
                        List<ECRHasECO> relatedECRs = SetECRHasECO(vmECO.RelatedECRIds, ECOId);
                        NewECO.RelatedECRs = relatedECRs;
                    }
                    NewECO.AreasAffected = requestList; //don't call the method straight to the field parameter, most times it does not work
                    NewECO.Approvers = userRoleECOList;
                    NewECO.Notifications = notifications;
                    NewECO.AffectedProducts = affectedProducts;

                    //auditLogRepository.Add(NewECO, loggedUser);
                    await _context.SaveChangesAsync();
                    if (NewECO.Status != StatusOptions.Draft)
                    {
                        List<string> changes = new List<string>();
                        changes.Add($"ECO {NewECO.Id} has been created.");
                        notificationSenderRepository.SendNotificationOnAnyChangeECO(NewECO.Id, changes);
                    }
                    return RedirectToAction("Main", "Home");
                }
                catch (Exception ex)
                {
                    ViewModelError vm = SetViewModelError(ex);
                    return RedirectToAction("ErrorPage", "ErrorHandler", vm);
                }
            }
            ViewData["ChangeTypeId"] = new SelectList(_context.RequestTypes, "Id", "Id", vmECO.ChangeTypeId);
            var viewModelCreate = SetViewModel();
            vmECO.ApproversList = viewModelCreate.ApproversList;
            return View(vmECO);
        }
        #endregion

        #region SetReturnLists
        /// <summary>
        /// This sets a list of the areas affected selected by the user in the view.
        /// </summary>
        /// <param name="vm">The ViewModel with the current values</param>
        /// <param name="ECOId">Existing or New ECOId</param>
        /// <returns>List<RequestTypeECO> to be set in the database</returns>
        public List<RequestTypeECO> SetAreasAffectedECO(ECOViewModel vm, int ECOId)
        {
            List<RequestTypeECO> returnList = new List<RequestTypeECO>();
            foreach (var typeChange in vm.AreasAffected.Where(a => a.Selected == true))//get the areas affected that were selected
            {
                var request = middleTablesRepository.GetExistingRequestTypeECO(typeChange.TypeId, ECOId); //gets an existing RequestTypeECO if it exists using the FK match
                if (request != null)
                {
                    returnList.Add(request);
                }
                else
                {
                    RequestType requestType = _context.RequestTypes.Where(r => r.Id == typeChange.TypeId).FirstOrDefault(); //get the RequestType object for Navigation Property purposes 
                    RequestTypeECO requestTypeChange = new RequestTypeECO() { ECOId = ECOId, RequestTypeId = typeChange.TypeId, RequestType = requestType }; //create a new RequestTypeECO with the ECOId and the TypeId
                    returnList.Add(requestTypeChange);
                }
            }
            return returnList;
        }
        /// <summary>
        /// This sets a list of the products affected by the change from the view.
        /// </summary>
        /// <param name="ProductIds">List of ProductIds that were selected</param>
        /// <param name="ECOId">Change Id</param>
        /// <returns></returns>
        public List<ProductECO> SetAffectedProductsECO(List<int> ProductIds, int ECOId)
        {
            List<ProductECO> returnList = new List<ProductECO>();
            if (ProductIds != null && ProductIds.Count > 0)
            {
                foreach (int productid in ProductIds) //for all the Ids in the List
                {
                    ProductECO existingProdECO = middleTablesRepository.GetExistingProductECO(productid, ECOId); //get an existing productECO record if it already exists
                    if (existingProdECO != null)
                    {
                        returnList.Add(existingProdECO);
                    }
                    else
                    {
                        ProductECO productECO = new ProductECO() //new productECO if it is a new affected product
                        {
                            ProductId = productid,
                            Product = productRepository.GetProduct(productid),
                            ECOId = ECOId
                        };
                        returnList.Add(productECO);
                    }
                }
            }
            return returnList;
        }
        /// <summary>
        /// This sets a list of UserRoleECO from the approvers selected by the user in the view.
        /// </summary>
        /// <param name="ApproversList">the UserRoleIds of the selected approvers</param>
        /// <param name="ECOId">Change Id</param>
        /// <returns>List of UserRoleECO to be added/modified in the db.</returns>
        public List<UserRoleECO> SetUserRoleECO(string[] ApproversList, int ECOId, string[] ValidatorsList, bool ignoreApprovals)
        {
            List<UserRoleECO> returnList = new List<UserRoleECO>();
            if (ApproversList != null && ApproversList.Length > 0)
            {
                foreach (var userRoleId in ApproversList)
                {
                    if (Int32.TryParse(userRoleId, out int intUserRoleId))
                    {
                        var userRole = middleTablesRepository.GetExistingUserRoleECO(intUserRoleId, ECOId); //gets an existing UserRoleECO if it is already in the database
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
                            UserRoleECO userRoleECO = new UserRoleECO() { ECOId = ECOId, UserRoleId = intUserRoleId, Approval = null, AprovedDate = null }; //creates a new UserRoleECO for the list
                            returnList.Add(userRoleECO);
                        }
                    }
                }
            }
            foreach (var userRoleId in ValidatorsList)
            {
                if (Int32.TryParse(userRoleId, out int intUserRoleId))
                {
                    var userRole = middleTablesRepository.GetExistingUserRoleECO(intUserRoleId, ECOId); //gets an existing UserRoleECO if it is already in the database
                    if (userRole != null)
                    {
                        returnList.Add(userRole);
                    }
                    else
                    {
                        UserRoleECO userRoleECO = new UserRoleECO() { ECOId = ECOId, UserRoleId = intUserRoleId, Approval = null, AprovedDate = null }; //creates a new UserRoleECO for the list
                        returnList.Add(userRoleECO);
                    }
                }
            }
            return returnList;
        }
        /// <summary>
        /// This sets a list of Notifications from the selected users to receive notifications.
        /// </summary>
        /// <param name="vm">the ViewModel that has the selected users</param>
        /// <param name="ECOId">Change Id</param>
        /// <returns>List of Notifications to be added/modified in the db.</returns>
        public List<Notifications> SetNotificationsECO(ECOViewModel vm, int ECOId, string[] ApproversList, string[] ValidatorsList)
        {
            List<Notifications> returnList = new List<Notifications>();
            bool added = false;
            var notification = middleTablesRepository.GetNotificationByUserAndECO(vm.OriginatorId, ECOId); //if there already is a notification for the user in the db.
            if (notification != null)
            {
                returnList.Add(notification);
            }
            else
            {
                User NotificationUser = _context.Users.Where(u => u.Id == vm.OriginatorId).FirstOrDefault(); //get the User to access it's name later in the sendNotification
                Notifications NewNotification = new Notifications()
                {
                    ECOId = ECOId,
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
                    notification = middleTablesRepository.GetNotificationByUserAndECO(userid, ECOId); //if there already is a notification for the user in the db.
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
                            ECOId = ECOId,
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
                    if (Int32.TryParse(userRoleId, out int intUserRoleId))
                    {
                        var user = userRoleRepository.GetUserByUserRoleId(intUserRoleId);
                        notification = middleTablesRepository.GetNotificationByUserAndECO(user.Id, ECOId); //if there already is a notification for the user in the db.
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
                                ECOId = ECOId,
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
            if (ValidatorsList != null)
            {
                foreach (var userRoleId in ValidatorsList)
                {
                    added = false;
                    if (Int32.TryParse(userRoleId, out int intUserRoleId))
                    {
                        var user = userRoleRepository.GetUserByUserRoleId(intUserRoleId);
                        notification = middleTablesRepository.GetNotificationByUserAndECO(user.Id, ECOId); //if there already is a notification for the user in the db.
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
                                ECOId = ECOId,
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
        /// This sets a list of ECRHasECO for the relatedECR/ECO fields
        /// </summary>
        /// <param name="ECRIds">The possible related ECRIds</param>
        /// <param name="ECOId">Change Id</param>
        /// <returns></returns>
        public List<ECRHasECO> SetECRHasECO(List<int> ECRIds, int ECOId)
        {
            List<ECRHasECO> returnList = new List<ECRHasECO>();
            if (ECRIds != null)
            {
                foreach (var ECRId in ECRIds)
                {
                    var relatedECR = middleTablesRepository.GetRecordByCompKey(ECRId, ECOId); //if the related ECR is alreay in the db
                    if (relatedECR != null)
                    {
                        returnList.Add(relatedECR);
                    }
                    else
                    {
                        ECRHasECO ECRHasECO = new ECRHasECO() { ECRId = ECRId, ECOId = ECOId };//create a new ECRHasECO
                        returnList.Add(ECRHasECO);
                    }
                }
            }
            return returnList;
        }
        /// <summary>
        /// Translates to a Collection of Categories that contains a Collection of Families that contains a Collection of Products that can be selected or not
        /// </summary>
        /// <param name="ECOId">Current ECOId</param>
        /// <returns>Returns a Dictionary in the format of: { "Category", {"Family", {Products, bool } } }</returns>
        public Dictionary<string, Dictionary<string, Dictionary<Product, bool>>> SetProductDictionary(int ECOId)
        {
            IQueryable<IGrouping<string, Product>> allProducts = _context.Products.GroupBy(p => p.Category); //get all possible products from db, grouped by Category

            List<ProductECO> affectedProducts = middleTablesRepository.GetProductECOsByECO(ECOId).ToList(); //gets a list of affected products already in the change

            Dictionary<string, Dictionary<string, Dictionary<Product, bool>>> returnDictionary = new Dictionary<string, Dictionary<string, Dictionary<Product, bool>>>(); //create a new dictionary for the return

            foreach (IGrouping<string, Product> category in allProducts)
            {
                IEnumerable<IGrouping<string, Product>> families = category.GroupBy(a => a.Name.Split('-')[0]); //separate the - and get the first element of the split -> this will group GO7, GO8, GO9, ATT, IOX etc

                Dictionary<string, Dictionary<Product, bool>> dictFamilies = new Dictionary<string, Dictionary<Product, bool>>(); //create a new Dictionary for the Families
                foreach (IGrouping<string, Product> products in families) //now foreach family in the collection
                {
                    Dictionary<Product, bool> productIncludeStatus = new Dictionary<Product, bool>(); //new Dictionary of Products with status
                    foreach (Product p in products)//now foreach product in the family
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

        #region Edit
        // GET: ECOs/Edit/5
        public async Task<IActionResult> Edit(int? id) //display the Edit view
        {
            if (id == null)
            {
                return NotFound();
            }

            var eCO = await _context.ECOs
                .Include(e => e.ChangeType)
                .Include(e => e.Originator)
                .Include(e => e.AreasAffected)
                    .ThenInclude(e => e.RequestType)
                .Include(e => e.AffectedProducts)
                .Include(e => e.RelatedECRs)
                    .ThenInclude(r => r.ECR)
                .Include(e => e.Notifications)
                    .ThenInclude(n => n.User)
                .Include(e => e.Approvers)
                    .ThenInclude(a => a.UserRole)
                        .ThenInclude(u => u.User)
                .FirstAsync(e => e.Id == id); //get all the necessary information from the db.

            ECOViewModel vm = new ECOViewModel(eCO);

            vm.ApproversList = SetSelectListsForApproversEdit(vm);

            vm.ValidatorsList = SetSelectListsForValidatorsEdit(vm);

            vm.AreasAffected = SetAreasAffectedEdit(vm);

            vm.NotesForApproverIds = SetNotesForApprover(vm);

            vm.NotesForValidatorIds = SetNotesForValidator(vm);

            vm.RelatedECRs = SetSelectListRelatedECRs(vm);

            vm.ProductList = SetProductDictionary(vm.Id);

            vm.Originator = eCO.Originator;

            vm.OriginatorId = eCO.OriginatorId;

            vm.Status = eCO.Status;

            ViewData["Notifications"] = SetSelectListForNotifications(vm);
            ViewData["OriginatorId"] = new SelectList(_context.Users.Where(u => u == loggedUser), "Id", "Name");

            if (eCO == null)
            {
                return NotFound();
            }
            ViewData["ChangeTypeId"] = new SelectList(_context.RequestTypes, "Id", "Name", eCO.ChangeTypeId);
            return View(vm);
        }

        // POST: ECOs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PermanentChange,ECRId,Description,ReasonForChange,ChangeTypeId,BOMRequired,ProductValidationTestingRequired,PlannedImplementationDate,ClosedDate,CustomerApproval," +
            "PriorityLevel,ImplementationType,Status,NotesForApproverIds,NotesForValidatorIds,UsersToBeNotified,Id,AreasAffected,LinkUrls,RelatedECRIds,PreviousRevision,NewRevision,Originator,OriginatorId,AffectedProductsIds")] ECOViewModel vmECO,
            string[] ApproversList, string[] ValidatorsList)
        { //POST method of the Edit Page
            if (id != vmECO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ECO existingECO = _context.ECOs.Where(e => e.Id == id)
                        .Include(e => e.AreasAffected).ThenInclude(a => a.RequestType)
                        .Include(a => a.Approvers).ThenInclude(b => b.UserRole).ThenInclude(u => u.User)
                        .Include(a => a.Approvers).ThenInclude(b => b.UserRole).ThenInclude(u => u.RequestType)
                        .Include(e => e.AffectedProducts).ThenInclude(a => a.Product)
                        .Include(a => a.Notifications).ThenInclude(n => n.User)
                        .Include(e => e.RelatedECRs).ThenInclude(e => e.ECR)
                        .Single(); //gets the ECO from the database with all the related information from other tables too -> .Include().ThenInclude() -> Inner join basically

                    List<ECRHasECO> relatedECRListInVm = new List<ECRHasECO>();

                    if (vmECO.RelatedECRIds != null)
                    {
                        foreach (var relatedECRId in vmECO.RelatedECRIds)
                        {
                            var relatedECR = _context.ECRHasECOs.Where(e => e.ECRId == relatedECRId).FirstOrDefault();

                            if (relatedECR == null)
                            {
                                var ecr = _context.ECRs.Where(e => e.Id == relatedECRId).FirstOrDefault();

                                relatedECR = new ECRHasECO
                                {
                                    ECRId = ecr.Id,
                                    ECR = ecr,
                                    ECO = existingECO,
                                    ECOId = vmECO.Id
                                };
                            }
                            if (relatedECR.ECR == null)
                            {
                                var ecr = _context.ECRs.Where(e => e.Id == relatedECR.ECRId).FirstOrDefault();

                                relatedECR = new ECRHasECO
                                {
                                    ECRId = ecr.Id,
                                    ECR = ecr,
                                    ECO = existingECO,
                                    ECOId = vmECO.Id
                                };
                            }
                            relatedECRListInVm.Add(relatedECR);
                        }
                    }
                    vmECO.RelaredECRsForVm = relatedECRListInVm;

                    List<string> changes = FindChanges(existingECO, vmECO, ValidatorsList, ApproversList);

                    bool ignoreApprovals = false;
                    foreach (var change in changes)
                    {
                        if (change.Contains("Notifications"))
                        {
                            ignoreApprovals = true;
                        }
                    }

                    UpdateExistingECO(existingECO, vmECO, ApproversList, ValidatorsList); // sets the values for the new record
                    existingECO.Approvers = SetUserRoleECO(ApproversList, existingECO.Id, ValidatorsList, ignoreApprovals);
                    UpdateUserRoleECOTable(SetUserRoleECO(ApproversList, existingECO.Id, ValidatorsList, ignoreApprovals), existingECO.Id); //will delete the records that were removed by the user. Addition happens in the line above ^
                    _context.Update(existingECO); //Updates the record in the database
                    //auditLogRepository.Add(existingECO, loggedUser); //adds a new auditlog with the new data and the current user who made the change

                    await _context.SaveChangesAsync();

                    if (existingECO.Status != StatusOptions.Draft)
                    {
                        if (changes.Count > 0)
                        {
                            notificationSenderRepository.SendNotificationOnAnyChangeECO(existingECO.Id, changes); //functions to send notifications depending on the bool
                        }
                        if (sendNotificationOnDescriptionChange)
                            notificationSenderRepository.SendNotificationOnDescriptionChangeECO(existingECO.Id, changedDescriptionECO);
                        if (sendNotificationOnReasonChange)
                            notificationSenderRepository.SendNotificationOnReasonChangeECO(existingECO.Id, reasonForChangeECO);
                        if (sendNotificationOnImplementationDateChange)
                            notificationSenderRepository.SendNotificationOnImplementationDateChangeECO(existingECO.Id, implementationDateChangedECO);
                        if (sendNotificationOnApproversChange)
                            notificationSenderRepository.SendNotificationOnApproversChangeECO(existingECO.Id, approversChangedECO);
                        if (sendNotificationStatusChange)
                        {
                            notificationSenderRepository.SendNotificationOnStatusChangeECO(existingECO.Id, ChangeStatusECO);
                        }
                        if (existingECO.Status == StatusOptions.Approved)
                        {
                            notificationSenderRepository.SendNotificationOnApprovalECO(existingECO.Id, ($"<div style='background-color:green'>{existingECO.Id} has been approved </div>"));
                        }
                    }
                    return RedirectToAction("Main", "Home"); //goes back to the Home page
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!ECOExists(Convert.ToInt32(vmECO.Id)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ViewModelError vm = SetViewModelError(ex);
                        return RedirectToAction("ErrorPage", "ErrorHandler", vm);
                    }
                }
                catch (Exception ex)
                {
                    ViewModelError vm = SetViewModelError(ex);
                    return RedirectToAction("ErrorPage", "ErrorHandler", vm);
                }
            }
            ViewData["ChangeTypeId"] = new SelectList(_context.RequestTypes, "Id", "Id", vmECO.ChangeTypeId);
            return View(vmECO);
        }

        #endregion

        #region Clone Rejected ECO
        public async Task<IActionResult> Clone(int? id) //display the Edit view
        {
            var eCO = await _context.ECOs
                .Include(e => e.ChangeType)
                .Include(e => e.Originator)
                .Include(e => e.AreasAffected)
                    .ThenInclude(e => e.RequestType)
                .Include(e => e.AffectedProducts)
                .Include(e => e.RelatedECRs)
                    .ThenInclude(r => r.ECR)
                .Include(e => e.Notifications)
                    .ThenInclude(n => n.User)
                .Include(e => e.Approvers)
                    .ThenInclude(a => a.UserRole)
                        .ThenInclude(u => u.User)
                        .AsNoTracking()
                .FirstAsync(e => e.Id == id); //get all the necessary information from the db.

            ECOViewModel vm = new ECOViewModel(eCO);

            vm.ApproversList = SetSelectListsForApproversEdit(vm);

            vm.ValidatorsList = SetSelectListsForValidatorsEdit(vm);

            vm.AreasAffected = SetAreasAffectedEdit(vm);

            vm.NotesForApproverIds = SetNotesForApprover(vm);

            vm.NotesForValidatorIds = SetNotesForValidator(vm);

            vm.RelatedECRs = SetSelectListRelatedECRs(vm);

            vm.ProductList = SetProductDictionary(vm.Id);

            vm.Originator = loggedUser;

            vm.OriginatorId = loggedUser.Id;

            vm.Status = eCO.Status;

            ViewData["AllUsers"] = SetSelectListForNotifications(vm);
            ViewData["OriginatorId"] = new SelectList(_context.Users.Where(u => u == loggedUser), "Id", "Name");

            if (eCO == null)
            {
                return NotFound();
            }
            ViewData["ChangeTypeId"] = new SelectList(_context.RequestTypes, "Id", "Name", eCO.ChangeTypeId);
            return View("Create", vm);
        }
        #endregion

        #region FindChangesForNotification
        private List<string> FindChanges(ECO existingECO, ECOViewModel vmECO, string[] ValidatorsList, string[] ApproversList)
        {
            List<string> changes = new List<string>();

            PropertyInfo[] existingProp = existingECO.GetType().GetProperties();
            PropertyInfo[] newProp = vmECO.GetType().GetProperties();
            foreach (PropertyInfo existprop in existingProp)
            {
                foreach (PropertyInfo nwprop in newProp)
                {
                    var newVal = nwprop.GetValue(vmECO, null);
                    var existVal = existprop.GetValue(existingECO, null);
                    if (existprop.Name == nwprop.Name ||
                        (existprop.Name.ToLower() == "relatedecrs" && nwprop.Name.ToLower() == "relaredecrsforvm"))
                    {
                        if (existprop.Name.ToLower() == "affectedproducts")
                        {
                            newVal = vmECO.AffectedProductsIds;
                            if (existVal != null && newVal != null)
                            {
                                var currentProduct = (ICollection<ProductECO>)existVal;
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
                                var currentProduct = (ICollection<ProductECO>)existVal;
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
                        else if (existprop.Name.ToLower() == "relatedecrs")
                        {
                            if (nwprop.Name.ToLower() == "relaredecrsforvm")
                            {
                                if (existVal != null && newVal != null)
                                {
                                    var currentRelatedECRs = (ICollection<ECRHasECO>)existVal;
                                    var newRelatedECRs = (ICollection<ECRHasECO>)newVal;
                                    List<string> existingRelatedECRs = new List<string>();
                                    List<string> latestRelatedECRs = new List<string>();

                                    foreach (var currentRelatedECR in currentRelatedECRs)
                                    {
                                        existingRelatedECRs.Add($"Id- {currentRelatedECR.ECR.Id}; Description- {currentRelatedECR.ECR.Description}");
                                    }
                                    foreach (var newRelatedECR in newRelatedECRs)
                                    {
                                        latestRelatedECRs.Add($"Id- {newRelatedECR.ECR.Id}; Description- {newRelatedECR.ECR.Description}");
                                    }
                                    var removed = existingRelatedECRs.Except(latestRelatedECRs).ToList();
                                    var added = latestRelatedECRs.Except(existingRelatedECRs).ToList();

                                    string ECRsAdded = "";
                                    string ECRsRemoved = "";
                                    if (added.Count > 0)
                                    {
                                        foreach (var ECR in added)
                                        {
                                            ECRsAdded += ECR + ", ";
                                        }
                                        changes.Add("Related ECRs added: " + ECRsAdded + "<br/>");
                                    }
                                    if (removed.Count > 0)
                                    {
                                        foreach (var ecr in removed)
                                        {
                                            ECRsRemoved += ecr + ", ";
                                        }
                                        changes.Add("Related ECRs Removed: " + ECRsRemoved + "<br/>");
                                    }
                                }
                                else if (newVal != null && existVal == null)
                                {
                                    var newECRs = (ICollection<ECRHasECO>)newVal;
                                    List<string> newECRList = new List<string>();

                                    foreach (var newECR in newECRs)
                                    {
                                        newECRList.Add(newECR.ECR.Description);
                                    }
                                    var added = newECRList.ToList();
                                    string ECRsAdded = "";
                                    if (added.Count > 0)
                                    {
                                        foreach (var ECR in added)
                                        {
                                            ECRsAdded += ECR + ", ";
                                        }
                                        changes.Add("Related ECRs added: " + ECRsAdded + "<br/>");
                                    }
                                }
                                else if (existVal != null && newVal == null)
                                {
                                    var currentECRs = (ICollection<ECRHasECO>)existVal;
                                    List<string> existECRs = new List<string>();

                                    foreach (var currentECR in currentECRs)
                                    {
                                        existECRs.Add(currentECR.ECR.Description);
                                    }

                                    var removed = existECRs.ToList();
                                    string ECRsRemoved = "";
                                    if (removed.Count > 0)
                                    {
                                        foreach (var ecr in removed)
                                        {
                                            ECRsRemoved += ecr + ", ";
                                        }
                                        changes.Add("Related ECOs Removed: " + ECRsRemoved + "<br/>");
                                    }
                                }
                            }
                        }
                        else if (existprop.Name.ToLower() == "areasaffected")
                        {
                            if (existVal != null && newVal != null)
                            {
                                var currentArea = (ICollection<RequestTypeECO>)existVal;
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

                                var newArea = (ICollection<RequestTypeECO>)newVal;
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
                                var currentArea = (ICollection<RequestTypeECO>)existVal;
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
                            foreach (var user in vmECO.UsersToBeNotified)
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
                            var currentApprovers = (ICollection<UserRoleECO>)existVal;
                            approversChangedECO = new List<string>();
                            List<string> existApprovers = new List<string>();
                            List<string> existValidators = new List<string>();
                            List<string> newApprovers = new List<string>();
                            List<string> newValidators = new List<string>();
                            foreach (var approver in currentApprovers)
                            {
                                if (approver.UserRole.RoleInt == Role.Validator)
                                {
                                    existValidators.Add(userRoleRepository.GetUserByUserRoleId(approver.UserRoleId).Name.ToString());
                                }
                                else
                                    existApprovers.Add(userRoleRepository.GetUserByUserRoleId(approver.UserRoleId).Name.ToString());
                            }
                            foreach (var approver in ApproversList)
                            {
                                newApprovers.Add(userRoleRepository.GetUserByUserRoleId(Convert.ToInt32(approver)).Name.ToString());
                            }
                            foreach (var validator in ValidatorsList)
                            {
                                newValidators.Add(userRoleRepository.GetUserByUserRoleId(Convert.ToInt32(validator)).Name.ToString());
                            }
                            if (newApprovers != existApprovers || newValidators != existValidators)
                            {
                                sendNotificationOnApproversChange = true;
                            }
                            var approversRemoved = existApprovers.Except(newApprovers).ToList();
                            var approversAdded = newApprovers.Except(existApprovers).ToList();
                            var validatorsRemoved = existValidators.Except(newValidators).ToList();
                            var validatorsAdded = newValidators.Except(existValidators).ToList();
                            string approverAdded = "";
                            string approverRemoved = "";
                            string validatorAdded = "";
                            string validatorRemoved = "";
                            if (approversAdded.Count > 0)
                            {
                                foreach (var user in approversAdded)
                                {
                                    approverAdded += user + ",";
                                }
                                changes.Add(approverAdded + " have been added as approvers.");
                                approversChangedECO.Add(approverAdded + " have been added as approvers.");
                            }
                            if (approversRemoved.Count > 0)
                            {
                                foreach (var user in approversRemoved)
                                {
                                    approverRemoved += user + ",";
                                }
                                changes.Add(approverRemoved + " have been removed from approvers.");
                                approversChangedECO.Add(approverRemoved + " have been removed from approvers.");
                            }
                            if (validatorsAdded.Count > 0)
                            {
                                foreach (var user in validatorsAdded)
                                {
                                    validatorAdded += user + ",";
                                }
                                changes.Add(validatorAdded + " have been added as validators.");
                                approversChangedECO.Add(validatorAdded + " have been added as validators.");
                            }
                            if (validatorsRemoved.Count > 0)
                            {
                                foreach (var user in validatorsRemoved)
                                {
                                    validatorRemoved += user + ",";
                                }
                                changes.Add(validatorRemoved + " have been removed from validators.");
                                approversChangedECO.Add(validatorRemoved + " have been removed from validators.");
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
        #endregion

        #region SetSelectLists
        /// <summary>
        /// This sets a List of SelectLists for the approvers that were previously selected
        /// </summary>
        /// <param name="vm">Object storing the values</param>
        /// <returns></returns>
        public List<SelectList> SetSelectListsForApproversEdit(ECOViewModel vm)
        {
            var allRequest = _context.RequestTypes.ToList();
            List<SelectList> approversList = new List<SelectList>();
            foreach (var request in allRequest)
            {
                var Approvers = userRoleRepository.GetApproversWithType(request.Id, loggedUser.Id); //get available approvers for this change
                var RequestGroup = new SelectListGroup { Name = request.Name };
                var approvers = new List<SelectListItem>();
                approvers.AddRange(Approvers.Select(item => new SelectListItem
                {
                    Text = item.User.Name,
                    Value = item.Id.ToString(),
                    Group = RequestGroup
                }).OrderBy(a => a.Group.Name).ToList()); //adds them in order of RequestType.Name

                SelectList DropDownList = new SelectList(approvers, "Value", "Text", null, "Group.Name"); //create a selectlist from the collection
                if (vm.Approvers != null)
                {
                    foreach (var selectedApprover in vm.Approvers)
                    {
                        if (selectedApprover.UserRole.RoleInt == Role.Approver)
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
                }
                approversList.Add(DropDownList);
            }
            return approversList;
        }

        public List<SelectList> SetSelectListsForValidatorsEdit(ECOViewModel vm)
        {
            var allRequest = _context.RequestTypes.ToList();
            List<SelectList> validatorsList = new List<SelectList>();
            foreach (var request in allRequest)
            {
                var Validator = userRoleRepository.GetValidatorsWithType(request.Id, loggedUser.Id); //get available approvers for this change
                var RequestGroup = new SelectListGroup { Name = request.Name };
                var validators = new List<SelectListItem>();
                validators.AddRange(Validator.Select(item => new SelectListItem
                {
                    Text = item.User.Name,
                    Value = item.Id.ToString(),
                    Group = RequestGroup
                }).OrderBy(a => a.Group.Name).ToList()); //adds them in order of RequestType.Name

                SelectList DropDownList = new SelectList(validators, "Value", "Text", null, "Group.Name"); //create a selectlist from the collection
                if (vm.Approvers != null)
                {
                    foreach (var selectedValidator in vm.Approvers)
                    {
                        if (selectedValidator.UserRole.RoleInt == Role.Validator)
                        {
                            var a = DropDownList.GetEnumerator();
                            while (a.MoveNext())
                            {
                                if (Convert.ToInt64(a.Current.Value) == selectedValidator.UserRoleId)
                                {
                                    a.Current.Selected = true; //select the ones that are already a part of the change
                                }
                            }
                        }
                    }
                }
                validatorsList.Add(DropDownList);
            }
            return validatorsList;
        }
        public SelectList SetSelectListForNotifications(ECOViewModel vm)
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

        public SelectList SetSelectListRelatedECRs(ECOViewModel vm)
        {
            var approvedECRs = _context.ECRs.Where(e => e.Status == StatusOptions.Approved).Include(a => a.Originator); //get a list of ECRs that have been approved, inner join the User table on Originator
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in approvedECRs)
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
            IEnumerable<ECRHasECO> relatedECRs = middleTablesRepository.GetRelatedECRByECO(vm.Id); //gets a collection of ECRHasECO for the current change
            if (relatedECRs != null)
            {
                foreach (var existingECR in relatedECRs) //select the ones that are already in the db
                {
                    var a = ddl.GetEnumerator();
                    while (a.MoveNext())
                    {
                        if (Convert.ToInt64(a.Current.Value) == existingECR.ECRId)
                        {
                            a.Current.Selected = true;
                        }
                    }
                }
            }
            return ddl;
        }

        public List<RequestTypeChangeCheckBox> SetAreasAffectedEdit(ECOViewModel vm)
        {
            List<RequestTypeChangeCheckBox> r1 = new List<RequestTypeChangeCheckBox>();
            List<RequestType> allRequest = _context.RequestTypes.ToList(); //gets a collection of RequestTypes
            for (int i = 0; i < allRequest.Count(); i++)
            {
                RequestTypeChangeCheckBox request = new RequestTypeChangeCheckBox(allRequest[i])
                {
                    Selected = false
                }; //new checkbox for Request Type
                if (vm.AreasAffected != null)
                {
                    foreach (var item in vm.AreasAffected) //if areas affected include the current request type, select it
                    {
                        if (request.Name == item.Name)
                        {
                            request.Selected = true;
                        }
                    }
                }
                r1.Add(request);
            }
            return r1;
        }
        public Dictionary<string, string> SetNotesForApprover(ECOViewModel vm)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            var allRequest = _context.RequestTypes.ToList(); //get a list of request types
            foreach (var req in allRequest)
            {
                if (vm.NotesForApproverIds != null)
                {
                    if (vm.NotesForApproverIds.ContainsKey(req.Name)) //if the notes thas the request type name
                    {
                        keyValuePairs.Add(req.Name, vm.NotesForApproverIds[req.Name]);
                    }
                    else
                    {
                        keyValuePairs.Add(req.Name, "no value."); //new note
                    }
                }
                else
                {
                    keyValuePairs.Add(req.Name, "no value."); //new note
                }

            }
            return keyValuePairs;
        }
        public Dictionary<string, string> SetNotesForValidator(ECOViewModel vm)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            var allRequest = _context.RequestTypes.ToList(); //get a list of request types
            foreach (var req in allRequest)
            {
                if (vm.NotesForValidatorIds != null)
                {
                    if (vm.NotesForValidatorIds.ContainsKey(req.Name)) //if the notes thas the request type name
                    {
                        keyValuePairs.Add(req.Name, vm.NotesForValidatorIds[req.Name]);
                    }
                    else
                    {
                        keyValuePairs.Add(req.Name, "no value."); //new note
                    }
                }
                else
                {
                    keyValuePairs.Add(req.Name, "no value."); //new note
                }
            }
            return keyValuePairs;
        }

        #endregion

        #region Update Middle Tables

        /// <summary>
        /// Method to update RequestECR table. Will remove any AreasAffected that were unselected by User in the View.
        /// </summary>
        /// <param name="newAreas">List of Areas selected</param>
        /// <param name="ecrId">Current ECR being updated</param>
        public void UpdateRequestECOTable(ICollection<RequestTypeECO> newAreas, int ecoId)
        {
            List<int> temp = newAreas.Select(i => i.RequestTypeId).ToList(); //get a list of the new Ids
            IQueryable<RequestTypeECO> remove = _context.RequestTypeECOs.Where(r => !temp.Contains(r.RequestTypeId) && r.ECOId == ecoId); //if the existing RequestTypeECOs do not contain the ids of the new areas, put them in the remove list
            if (remove != null)
            {
                _context.RequestTypeECOs.RemoveRange(remove);
            }
            _context.SaveChanges();
        }
        /// <summary>
        /// Method to update UserRoleECR table. Will remove any Approvers that were unselected by User in the View.
        /// </summary>
        /// <param name="newApprovers">List of Approvers selected</param>
        /// <param name="ecrId">Current ECR being updated</param>
        public void UpdateUserRoleECOTable(ICollection<UserRoleECO> newApprovers, int ecoId)
        {
            List<int> temp = newApprovers.Select(i => i.UserRoleId).ToList(); //get a list of the new Ids
            var remove = _context.UserRoleECOs.Where(r => !temp.Contains(r.UserRoleId) && r.ECOId == ecoId); //if the existing list of UserRoleECO does not contain the ids of the new approvers, put them in the remove list
            if (remove != null)
            {
                _context.UserRoleECOs.RemoveRange(remove);
            }
            _context.SaveChanges();
        }
        /// <summary>
        /// Method to update the ProductECO table. Will remove any Products that were unselected by the User in the View.
        /// </summary>
        /// <param name="newProducts">List of Products selected</param>
        /// <param name="ecoId">Change Id</param>
        public void UpdateProductECOTable(ICollection<ProductECO> newProducts, int ecoId)
        {
            List<int> temp = newProducts.Select(i => i.ProductId).ToList();//get a list of the new Ids
            IQueryable<ProductECO> remove = _context.ProductECOs.Where(r => !temp.Contains(r.ProductId) && r.ECOId == ecoId); //if the existing list of ProductECO does not contain the ids of the new products, put them in the remove list
            if (remove != null)
            {
                _context.ProductECOs.RemoveRange(remove);
            }
            _context.SaveChanges();
        }

        /// <summary>
        /// Method to update Notification table. Will remove any Notifications that were unselected by User in the View
        /// </summary>
        /// <param name="notifications">List of Notifications selected</param>
        /// <param name="ecrId">Current ECO being updated</param>
        public void UpdateNotificationTable(ICollection<Notifications> notifications, int ecoId)
        {
            List<int> temp = notifications.Select(i => i.Id).ToList();//get a list of the new Ids
            var remove = _context.Notifications.Where(r => !temp.Contains(r.Id) && r.ECOId == ecoId); //if the existing list of Notifications does not contain the ids of the new Notifications, put them in the remove list
            if (remove != null)
            {
                _context.Notifications.RemoveRange(remove);
            }
            _context.SaveChanges();
        }

        public void UpdateECRHasECOTable(ICollection<ECRHasECO> eCRHasECOs, int ecoId)
        {
            List<int> temp = eCRHasECOs.Select(e => e.ECRId).ToList(); //get a list of ECRIds
            var remove = _context.ECRHasECOs.Where(r => !temp.Contains(r.ECRId) && r.ECOId == ecoId); //if the existing list of ECRHasECO does not contain the ids of the new related ecrs, put them in the remove list
            if (remove != null)
            {
                _context.ECRHasECOs.RemoveRange(remove);
            }
            _context.SaveChanges();
        }
        #endregion

        #region Update Record
        /// <summary>
        /// Takes the existingECO and updates it's values to the vmECO
        /// </summary>
        /// <param name="existingECO">oldValues</param>
        /// <param name="vmECO">newValues</param>
        public void UpdateExistingECO(ECO existingECO, ECOViewModel vmECO, string[] ApproversList, string[] ValidatorsList)
        {
            int existingId = existingECO.Id;
            if (existingECO.Description != vmECO.Description)
            {
                changedDescriptionECO = $"Current Description : {existingECO.Description} ; New Description : {vmECO.Description} ";
                existingECO.Description = vmECO.Description;
                sendNotificationOnDescriptionChange = true;
            }
            if (existingECO.ReasonForChange != vmECO.ReasonForChange)
            {
                reasonForChangeECO = $"Current Reason For Change : {existingECO.ReasonForChange} ; New Reason For Change : {vmECO.ReasonForChange} ";
                existingECO.ReasonForChange = vmECO.ReasonForChange;
                sendNotificationOnReasonChange = true;
            }
            if (existingECO.PlannedImplementationDate != vmECO.PlannedImplementationDate)
            {
                implementationDateChangedECO = $"Current Implementation Date : {existingECO.PlannedImplementationDate} ; New Implementation Date : {vmECO.PlannedImplementationDate} ";
                existingECO.PlannedImplementationDate = vmECO.PlannedImplementationDate;
                sendNotificationOnImplementationDateChange = true;
            }
            existingECO.ImplementationType = vmECO.ImplementationType;
            existingECO.OriginatorId = vmECO.OriginatorId;
            existingECO.Originator = userRepository.GetUserFromId(vmECO.OriginatorId);
            existingECO.PermanentChange = vmECO.PermanentChange;
            existingECO.PriorityLevel = vmECO.PriorityLevel;
            existingECO.ProductValidationTestingRequired = vmECO.ProductValidationTestingRequired;
            existingECO.BOMRequired = vmECO.BOMRequired;
            existingECO.PreviousRevision = vmECO.PreviousRevision;
            existingECO.NewRevision = vmECO.NewRevision;
            existingECO.LinkUrls = vmECO.LinkUrls;
            existingECO.AreasAffected = SetAreasAffectedECO(vmECO, existingId);
            UpdateRequestECOTable(SetAreasAffectedECO(vmECO, existingId), existingId); //call the method and not existingECO.AreasAffected because the changes were not saved yet
            existingECO.Notifications = SetNotificationsECO(vmECO, existingId, ApproversList, ValidatorsList);
            UpdateNotificationTable(SetNotificationsECO(vmECO, existingId, ApproversList, ValidatorsList), existingId);
            existingECO.AffectedProducts = SetAffectedProductsECO(vmECO.AffectedProductsIds, existingId);
            UpdateProductECOTable(SetAffectedProductsECO(vmECO.AffectedProductsIds, existingId), existingId);
            existingECO.NotesForApprover = vmECO.NotesForApproverIds;
            existingECO.NotesForValidator = vmECO.NotesForValidatorIds;
            existingECO.RelatedECRs = SetECRHasECO(vmECO.RelatedECRIds, existingId);
            UpdateECRHasECOTable(SetECRHasECO(vmECO.RelatedECRIds, existingId), existingId);
            //existingECO.Status = vmECO.Status;
            if (existingECO.Status != vmECO.Status)
            {
                ChangeStatusECO = $"<div style='color:lightblue'> Current Status : {existingECO.Status} ; New Status : {vmECO.Status} </div>";
                existingECO.Status = vmECO.Status;
                sendNotificationStatusChange = true;
            }
        }
        #endregion

        #region Send Reminder Email
        public IActionResult ReminderEmail(int id)
        {
            ECO existingECO = _context.ECOs.Where(e => e.Id == id)
                        .Include(e => e.AreasAffected).ThenInclude(a => a.RequestType)
                        .Include(a => a.Approvers).ThenInclude(b => b.UserRole).ThenInclude(u => u.User)
                        .Include(a => a.Approvers).ThenInclude(b => b.UserRole).ThenInclude(u => u.RequestType)
                        .Include(e => e.AffectedProducts).ThenInclude(a => a.Product)
                        .Include(a => a.Notifications).ThenInclude(n => n.User)
                        .Include(e => e.RelatedECRs).ThenInclude(e => e.ECR)
                        .Single();
            string message = $"Please check if there is any action needed for the ECO {id} ";
            notificationSenderRepository.SendReminderEmailECO(id, message);
            TempData["ReminderEmailConfirmation"] = "A reminder email has been sent to everyone associated with this ECO";
            return RedirectToAction("Details", existingECO);
        }
        #endregion

        #region Add/Remove To Nofication List
        public async Task<IActionResult> AddToNotification(int id)
        {
            var loggedInUser = GetLoggedInUser();
            var notification = middleTablesRepository.GetNotificationByUserAndECO(loggedInUser.Id, id);

            if (notification == null)
            {
                User NotificationUser = _context.Users.Where(u => u.Id == loggedInUser.Id).FirstOrDefault(); //get the User to access it's name later in the sendNotification
                Notifications NewNotification = new Notifications()
                {
                    ECOId = id,
                    UserId = loggedInUser.Id,
                    User = NotificationUser,
                    Option = NotificationOption.AnyChange //the user will always be notified on any change, unless they change it in the main page
                };
                _context.Add(NewNotification);
                _context.SaveChanges();

                //List<string> message = new List<string>();
                //message.Add($"{loggedInUser.Name} has been added to the notification list");
                //notificationSenderRepository.SendNotificationOnAnyChangeECO(id, message);
            }
            else
            {
                var removeUser = _context.Notifications.Where(u => u.UserId == loggedInUser.Id && u.ECOId == id).FirstOrDefault();
                _context.Notifications.RemoveRange(removeUser);
                _context.SaveChanges();
                //List<string> message = new List<string>();
                //message.Add($"{loggedInUser.Name} has been removed from the notification list");
                //notificationSenderRepository.SendNotificationOnAnyChangeECO(id, message);
            }

            return RedirectToAction(nameof(Details), new { id = id });
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
                var eCO = _context.ECOs
                    .Include(e => e.ChangeType)
                    .Include(e => e.Originator)
                    .Include(e => e.AreasAffected)
                        .ThenInclude(a => a.RequestType)
                    .Include(e => e.RelatedECRs)
                        .ThenInclude(r => r.ECR)
                    .Include(e => e.Notifications)
                        .ThenInclude(n => n.User)
                    .Include(e => e.Approvers)
                        .ThenInclude(a => a.UserRole)
                            .ThenInclude(u => u.User)
                    .Include(e => e.AffectedProducts)
                        .ThenInclude(a => a.Product)
                    .FirstOrDefault(m => m.Id == id); //gets all the related information from the connecting tables

                List<ECR> relatedECRs = new List<ECR>();
                foreach (var ecohasecr in eCO.RelatedECRs) //sets a list of ECRs using the RelatedECRs field for the Partial in the view
                {
                    relatedECRs.Add(ecohasecr.ECR);
                }

                ViewData["ECRs"] = relatedECRs; //simple viewbag for the relatedECOs Partial.

                List<AuditLog> auditLogs = new List<AuditLog>();
                foreach (var log in auditLogRepository.GetAuditLogsECO(eCO.Id)) //sets a list of AuditLogs for the Partial
                {
                    auditLogs.Add(log);
                }
                ViewData["AuditLog"] = auditLogs;

                if (eCO == null)
                {
                    return NotFound();
                }

                return View(eCO);
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
                + "/ECOs/Print/"
                + ID.ToString();
            PdfDocument doc = converter.ConvertUrl(url);

            byte[] pdfBytes = doc.Save();
            MemoryStream ms = new MemoryStream(pdfBytes);
            string fileName = "ECO" + ID + ".pdf";
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


    }
}
