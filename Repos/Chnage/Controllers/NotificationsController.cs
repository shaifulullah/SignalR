using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Chnage.Models;
using Chnage.Repository;

namespace Chnage.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly MyECODBContext _context;
        private User loggedUser;
        public NotificationsController(MyECODBContext context)
        {
            _context = context;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            loggedUser = GetLoggedInUser();
        }
        /// <summary>
        /// Access the User.Identity to get the logged user's email
        /// </summary>
        /// <returns>An Email @geotab.com</returns>
        private string GetLoggedInUserEmail()
        {
            string userEmail = "";
            if (User.Identity.IsAuthenticated)
            {
                userEmail = (User.Identity as System.Security.Claims.ClaimsIdentity)?.Claims.ElementAt(2).Value;
            }
            return userEmail;
        }

        /// <summary>
        /// Gets the User from the database using the unique email. maybe add some exception handling later
        /// </summary>
        /// <returns>Existing User from Db</returns>
        private User GetLoggedInUser()
        {
            return _context.Users.Where(u => u.Email == GetLoggedInUserEmail()).Single();
        }

        // GET: Notifications
        public async Task<IActionResult> Index()
        {
            IIncludableQueryable<Notifications, User> myECODBContext = _context.Notifications.Include(n => n.ECN).Include(n => n.ECO).Include(n => n.ECR).Include(n => n.User);
            return View(await myECODBContext.ToListAsync());
        }

        public IActionResult Manage()
        {
            ViewData["LoggedUser"] = loggedUser;
            IEnumerable<Notifications> view = _context.Notifications.Where(n=>n.UserId == loggedUser.Id).Include(n => n.ECN).Include(n => n.ECO).Include(n => n.ECR).Include(n => n.User);
            return View(view);
        }

        // GET: Notifications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notifications = await _context.Notifications.Where(n => n.Id == id).Include(n => n.ECN).Include(n => n.ECO).Include(n => n.ECR).Include(n => n.User).FirstOrDefaultAsync();
            if (notifications == null)
            {
                return NotFound();
            }
            return View(notifications);
        }

        // POST: Notifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ECRId,ECOId,ECNId,Option")] Notifications notifications)
        {
            if (id != notifications.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notifications);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationsExists(notifications.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Manage));
            }
            return View(notifications);
        }

        // GET: Notifications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notifications = await _context.Notifications
                .Include(n => n.ECN)
                .Include(n => n.ECO)
                .Include(n => n.ECR)
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notifications == null)
            {
                return NotFound();
            }

            return View(notifications);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notifications = await _context.Notifications.FindAsync(id);
            _context.Notifications.Remove(notifications);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificationsExists(int id)
        {
            return _context.Notifications.Any(e => e.Id == id);
        }
    }
}
