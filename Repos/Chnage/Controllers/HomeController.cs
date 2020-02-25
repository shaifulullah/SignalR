using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Chnage.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Chnage.Repository;
using Chnage.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Chnage.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Chnage.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyECODBContext _context;
        private readonly IECR ecrRepository;
        private readonly IECO ecoRepository;
        private readonly IECN ecnRepository;
        private readonly IUser userRepository;
        private User loggedUser;
        private string controller = "";
        private string action = "";
        public HomeController(MyECODBContext context, IUser user, IECR ecr, IECO eco, IECN ecn)
        {
            _context = context;
            ecrRepository = ecr;
            ecoRepository = eco;
            ecnRepository = ecn;
            userRepository = user;
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
            viewModelError.User = loggedUser.Email;

            if (Ex.InnerException != null)
            {
                viewModelError.InnerExceptionMessage = Ex.InnerException.Message;
                viewModelError.InnerExceptionType = Ex.InnerException.GetType().FullName;
                viewModelError.InnerToString = Ex.InnerException.ToString();
            }
            return viewModelError;
        }



        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            loggedUser = GetLoggedInUser();
            controller = this.ControllerContext.RouteData.Values["controller"].ToString();
            action = this.ControllerContext.RouteData.Values["action"].ToString();
        }
        private User GetLoggedInUser()
        {
            return _context.Users.Where(u => u.Email == GetLoggedInUserEmail()).SingleOrDefault();
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Help()
        {
            return View();
        }

        public int GetDraftsCount(IEnumerable<ECR> yourECRDrafts, IEnumerable<ECO> yourECODrafts, IEnumerable<ECN> yourECNDrafts)
        {
            int draftsCount = yourECRDrafts.Count() + yourECODrafts.Count() + yourECNDrafts.Count();
            if (draftsCount > 0)
            {
                return draftsCount;
            }
            else
            {
                return 0;
            }
        }

        public IActionResult Drafts()
        {
            try
            {
                Dictionary<string, IEnumerable<MainPageViewModel>> vm = new Dictionary<string, IEnumerable<MainPageViewModel>>();
                List<MainPageViewModel> DraftsList = new List<MainPageViewModel>();

                IEnumerable<ECR> yourECRDrafts = ecrRepository.GetAllECRsByStatus(StatusOptions.Draft, loggedUser.Id); //get all drafts
                IEnumerable<ECO> yourECODrafts = ecoRepository.GetAllECOsByStatus(StatusOptions.Draft, loggedUser.Id);
                IEnumerable<ECN> yourECNDrafts = ecnRepository.GetAllECNsByStatus(StatusOptions.Draft, loggedUser.Id);

                ViewData["DraftsCount"] = GetDraftsCount(yourECRDrafts, yourECODrafts, yourECNDrafts);
                foreach (ECR ecr in yourECRDrafts)
                {
                    DraftsList.Add(new MainPageViewModel(ecr)); //add to the list of MainPageViewModel using the ECR
                }
                foreach (ECO eco in yourECODrafts)
                {
                    DraftsList.Add(new MainPageViewModel(eco));
                }
                foreach (ECN ecn in yourECNDrafts)
                {
                    DraftsList.Add(new MainPageViewModel(ecn));
                }

                vm.Add("Drafts", DraftsList); //add to the Dictionary
                using (AdminsController ac = new AdminsController(_context))
                {
                    ViewData["AdminEmails"] = ac.GetAdminEmailsList();
                }
                return View("Main", vm);
            }
            catch (Exception ex)
            {
                ViewModelError vm = SetViewModelError(ex);
                return RedirectToAction("ErrorPage", "ErrorHandler", vm);
            }
        }

        public async Task<IActionResult> Main()
        {
            try
            {
                Dictionary<string, IEnumerable<MainPageViewModel>> vm = new Dictionary<string, IEnumerable<MainPageViewModel>>();

                using (AdminsController ac = new AdminsController(_context))
                {
                    ViewData["AdminEmails"] = ac.GetAdminEmailsList();
                }

                User current = GetLoggedInUser();
                if (current != null)
                {
                    var yourECRDrafts = ecrRepository.GetAllECRsByStatus(StatusOptions.Draft, loggedUser.Id); //get all changes
                    var yourECODrafts = ecoRepository.GetAllECOsByStatus(StatusOptions.Draft, loggedUser.Id);
                    var yourECNDrafts = ecnRepository.GetAllECNsByStatus(StatusOptions.Draft, loggedUser.Id);

                    ViewData["DraftsCount"] = GetDraftsCount(yourECRDrafts, yourECODrafts, yourECNDrafts);

                    var UserRoleECRForLoggedUser = _context.UserRoleECRs.Where(u => u.UserRole.UserId == loggedUser.Id && u.Approval == null).ToList(); //userroleecrs that have not approved the change yet.
                    var UserRoleECOForLoggedUser = _context.UserRoleECOs.Where(u => u.UserRole.UserId == loggedUser.Id && u.Approval == null).ToList(); //userroleecos that have not approved the change yet.
                    var UserRoleECNForLoggedUser = _context.UserRoleECNs.Where(u => u.UserRole.UserId == loggedUser.Id && u.Approval == null).ToList(); //userroleecns that have not approved the change yet.

                    List<MainPageViewModel> pendingOnYou = new List<MainPageViewModel>(); //first list that appears in the page
                    foreach (var UserRoleECR in UserRoleECRForLoggedUser)
                    {
                        List<MainPageViewModel> tempPendingonyou = new List<MainPageViewModel>();
                        var ecrList = _context.ECRs.Where(e => e.Id == UserRoleECR.ECRId &&
                        (e.Status != StatusOptions.Draft &&
                        e.Status != StatusOptions.RejectedApproval &&
                        e.Status != StatusOptions.RejectedValidation)).Include(e => e.ChangeType)
                        .Include(e => e.Originator).FirstOrDefault();
                        if (ecrList != null)
                        {
                            var ecrs = new MainPageViewModel(ecrList);
                            if (pendingOnYou.Count > 0)
                            {
                                foreach (MainPageViewModel mpvm in pendingOnYou)
                                {
                                    if (ecrs.Id != mpvm.Id)
                                    {
                                        tempPendingonyou.Add(ecrs); //adds to the list of pending changes (ecr)
                                        break;
                                    }
                                }

                                foreach (var eco in tempPendingonyou)
                                {
                                    pendingOnYou.Add(eco);
                                }
                            }
                            else
                            {
                                pendingOnYou.Add(ecrs); //adds to the list of pending changes (ecr)
                            }
                        }
                    }
                    foreach (var UserRoleECO in UserRoleECOForLoggedUser)
                    {
                        List<MainPageViewModel> tempPendingonyou = new List<MainPageViewModel>();
                        var ecoList = _context.ECOs.Where(e => e.Id == UserRoleECO.ECOId &&
                        (e.Status != StatusOptions.Draft &&
                        e.Status != StatusOptions.RejectedApproval &&
                        e.Status != StatusOptions.RejectedValidation)).Include(e => e.ChangeType)
                        .Include(e => e.Originator).FirstOrDefault();
                        if (ecoList != null)
                        {
                            var ecos = new MainPageViewModel(ecoList);

                            if (pendingOnYou.Count > 0)
                            {
                                foreach (MainPageViewModel mpvm in pendingOnYou)
                                {
                                    if (ecos.Id != mpvm.Id)
                                    {
                                        tempPendingonyou.Add(ecos); //adds to the list of pending changes (eco)
                                        break;
                                    }
                                }
                                foreach (var eco in tempPendingonyou)
                                {
                                    pendingOnYou.Add(eco);
                                }
                            }
                            else
                            {
                                pendingOnYou.Add(ecos); //adds to the list of pending changes (eco)
                            }
                        }
                    }
                    foreach (var UserRoleECN in UserRoleECNForLoggedUser)
                    {
                        List<MainPageViewModel> tempPendingonyou = new List<MainPageViewModel>();
                        var ecnList = _context.ECNs.Where(e => e.Id == UserRoleECN.ECNId &&
                        (e.Status != StatusOptions.Draft &&
                        e.Status != StatusOptions.RejectedApproval &&
                        e.Status != StatusOptions.RejectedValidation)).Include(e => e.ChangeType)
                        .Include(e => e.Originator).FirstOrDefault();
                        if (ecnList != null)
                        {
                            var ecns = new MainPageViewModel(ecnList);
                            if (pendingOnYou.Count > 0)
                            {
                                foreach (MainPageViewModel mpvm in pendingOnYou)
                                {
                                    if (ecns.Id != mpvm.Id)
                                    {
                                        tempPendingonyou.Add(ecns); //adds to the list of pending changes (ecr)
                                        break;
                                    }

                                }
                                foreach (var eco in tempPendingonyou)
                                {
                                    pendingOnYou.Add(eco);
                                }
                            }
                            else
                            {
                                pendingOnYou.Add(ecns); //adds to the list of pending changes (ecr)
                            }
                        }
                    }
                    vm.Add("Waiting on You", pendingOnYou);

                    List<MainPageViewModel> createdButNotApproved = new List<MainPageViewModel>(); //another list of the ViewModel

                    var createdByYouNotApprovedECR = await _context.ECRs.Where(ec => ec.OriginatorId == current.Id &&
                    (ec.Status != StatusOptions.Draft &&
                    ec.Status != StatusOptions.Approved &&
                    ec.Status != StatusOptions.RejectedApproval &&
                    ec.Status != StatusOptions.RejectedValidation)).Include(e => e.ChangeType)
                    .OrderByDescending(ec => ec.Status)
                    .ThenByDescending(ec => ec.CreatedDate)
                    .ToListAsync(); //gets all ECRs that were created by the current logged user that have not been approved and are not drafts

                    foreach (var item in createdByYouNotApprovedECR)
                    {
                        createdButNotApproved.Add(new MainPageViewModel(item));
                    }

                    var createdByYouNotApprovedECO = await _context.ECOs.Where(ec => ec.OriginatorId == current.Id &&
                    (ec.Status != StatusOptions.Draft &&
                    ec.Status != StatusOptions.Approved &&
                    ec.Status != StatusOptions.RejectedApproval &&
                    ec.Status != StatusOptions.RejectedValidation)).Include(e => e.ChangeType)
                    .OrderByDescending(ec => ec.Status)
                    .ThenByDescending(ec => ec.CreatedDate)
                    .ToListAsync(); //gets all ECOs that were created by the current logged user that have not been approved or rejected and are not drafts

                    foreach (var item in createdByYouNotApprovedECO)
                    {
                        createdButNotApproved.Add(new MainPageViewModel(item));
                    }

                    var createdByYouNotApprovedECN = await _context.ECNs.Where(ec => ec.OriginatorId == current.Id &&
                    (ec.Approvers.Select(a => a.Approval == false).Any() &&
                    ec.Status != StatusOptions.Draft &&
                    ec.Status != StatusOptions.Approved &&
                    ec.Status != StatusOptions.RejectedApproval &&
                    ec.Status != StatusOptions.RejectedValidation)).Include(e => e.ChangeType)
                    .OrderByDescending(ec => ec.CreatedDate)
                    .ThenByDescending(ec => ec.DateOfNotice)
                    .ToListAsync(); //gets all ECNs that were created by the current logged user that have not been approved and are not drafts

                    foreach (var item in createdByYouNotApprovedECN)
                    {
                        createdButNotApproved.Add(new MainPageViewModel(item));
                    }

                    vm.Add("Your Changes (Waiting for Approval)", createdButNotApproved); //another entry to the dictionary

                    List<MainPageViewModel> allChanges = new List<MainPageViewModel>();

                    var allECR = _context.ECRs.Where(e => !e.Status.Equals(StatusOptions.Draft))
                        .Include(e => e.Approvers)
                            .ThenInclude(e => e.UserRole)
                                .ThenInclude(ur => ur.User)
                        .Include(e => e.ChangeType)
                        .Include(e => e.Originator)
                        .OrderByDescending(e => e.ClosedDate)
                        .ThenByDescending(e => e.Status != StatusOptions.RejectedApproval && e.Status != StatusOptions.RejectedValidation)
                            .ThenByDescending(e => e.Status)
                                .ThenByDescending(e => e.CreatedDate) //get all fields for the ECRs that are not drafts
                        .ToList();
                    foreach (var item in allECR)
                    {
                        allChanges.Add(new MainPageViewModel(item));
                    }

                    var allECO = _context.ECOs.Where(e => !e.Status.Equals(StatusOptions.Draft))
                        .Include(e => e.Approvers)
                            .ThenInclude(e => e.UserRole)
                                .ThenInclude(ur => ur.User)
                        .Include(e => e.ChangeType)
                        .Include(e => e.Originator)
                        .OrderByDescending(e => e.ClosedDate)
                        .ThenByDescending(e => e.Status != StatusOptions.RejectedApproval && e.Status != StatusOptions.RejectedValidation)
                            .ThenByDescending(e => e.Status)
                                .ThenByDescending(e => e.CreatedDate)
                        .ToList();

                    foreach (var item in allECO)
                    {
                        allChanges.Add(new MainPageViewModel(item));
                    }

                    var allECN = _context.ECNs.Where(e => !e.Status.Equals(StatusOptions.Draft))
                        .Include(e => e.Approvers)
                            .ThenInclude(e => e.UserRole)
                                .ThenInclude(ur => ur.User)
                        .Include(e => e.ChangeType)
                        .Include(e => e.Originator)
                        .OrderByDescending(e => e.ClosedDate)
                        .ThenByDescending(e => e.Status != StatusOptions.RejectedApproval && e.Status != StatusOptions.RejectedValidation)
                            .ThenByDescending(e => e.Status)
                                .ThenByDescending(e => e.CreatedDate)
                        .ToList();

                    foreach (var item in allECN)
                    {
                        allChanges.Add(new MainPageViewModel(item));
                    }

                    vm.Add("Search All", allChanges);
                }
                return View(vm);
            }
            catch (Exception ex)
            {
                ViewModelError vm = SetViewModelError(ex);
                return RedirectToAction("ErrorPage", "ErrorHandler", vm);
            }
        }
        [Route("admin")]
        public IActionResult Admin()
        {
            return View();
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
        //public IActionResult Login()
        //{
        //    try
        //    {
        //        bool isGeotab = false;
        //        string loggedUserName = GetLoggedInUsername();
        //        string loggedUserEmail = GetLoggedInUserEmail();
        //        ViewModelUserLogin viewModel = new ViewModelUserLogin() { Email = loggedUserEmail, Name = loggedUserName };

        //        User found = userRepository.GetUserFromEmail(loggedUserEmail);
        //        if (found != null)
        //        {
        //            HttpContext.Session.SetObjectAsJson("User", found);
        //            return RedirectToAction("Main", "Home");
        //        }
        //        else
        //        {
        //            if (User.Identity.IsAuthenticated)
        //            {
        //                string geotabDomain = "geotab.com";
        //                string userEmail = (User.Identity as System.Security.Claims.ClaimsIdentity)?.Claims.ElementAt(2).Value;
        //                string[] x = userEmail.Split("@");
        //                string userEmailDomain = x[1];
        //                isGeotab = userEmailDomain == geotabDomain;
        //            }
        //            if (isGeotab)
        //            {
        //                User newUser = new User() { Name = loggedUserName, Email = loggedUserEmail, isActive = true };
        //                userRepository.Add(newUser);
        //                _context.SaveChanges();
        //                HttpContext.Session.SetObjectAsJson("User", newUser);
        //            }
        //            return RedirectToAction("Main", "Home");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewModelError vm = SetViewModelError(ex);
        //        return RedirectToAction("ErrorPage", "ErrorHandler", vm);
        //    }
        //}

        //[HttpPost]
        //public IActionResult Login([Bind("Name,Email")] User user)
        //{
        //    try
        //    {
        //        if (user.Name != null && user.Email != null)
        //        {
        //            User found = _context.Users.Where(u => u.Email.Equals(user.Email) && u.Name.Equals(user.Name)).FirstOrDefault();
        //            if (found != null)
        //            {
        //                HttpContext.Session.SetObjectAsJson("User", found);
        //                return RedirectToAction("Index", "Home");
        //            }
        //            else
        //            {
        //                ViewData["Result"] = "Error logging in. No user found.";
        //                ModelState.AddModelError("ErrorMessage", "No user found with the credentials entered.");
        //                return View("Login");
        //            }
        //        }
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewModelError vm = SetViewModelError(ex);
        //        return RedirectToAction("ErrorPage", "ErrorHandler", vm);
        //    }
        //}
        //public IActionResult SignIn(bool loggingOut = false, bool initialLogin = true)
        //{
        //    if (loggingOut)
        //    {
        //        return View();
        //    }
        //    if (!User.Identity.IsAuthenticated && initialLogin)
        //    {
        //        return RedirectToAction("SignIn", "Authentication");
        //    }
        //    return RedirectToAction("Login", "Home");
        //}
    }
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
