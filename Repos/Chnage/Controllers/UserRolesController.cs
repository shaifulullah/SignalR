using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Chnage.Models;
using Chnage.Repository;
using Chnage.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers
{
    public class UserRolesController : Controller
    {
        private readonly MyECODBContext _context;
        private readonly IUserRole userRoleRepository;
        public UserRolesController(MyECODBContext context, IUserRole userRole)
        {
            userRoleRepository = userRole;
            _context = context;
        }

        // GET: MyECOArea/UserRoles
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserRoles
               .Include(u=>u.User)
               .Include(t=>t.RequestType)
               .OrderBy(u=>u.User)
               .ToListAsync());
        }

        // GET: MyECOArea/UserRoles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRole = await _context.UserRoles.Include(r => r.RequestType).Include(u=>u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRole == null)
            {
                return NotFound();
            }

            return View(userRole);
        }

        // GET: MyECOArea/UserRoles/Create
        public IActionResult Create()
        {
            //variables with values from the database.
            var users = _context.Users.Select(u => new { UserId = u.Id, UserName = u.Name, IsActive = u.isActive })
                                      .Where(us => us.IsActive).ToList();
            var types = _context.RequestTypes.Select(t => new { TypeId = t.Id, TypeName = t.Name }).ToList();
            //adds the variables to the ViewData
            ViewData["users"] = new SelectList(users, "UserId", "UserName");
            ViewData["types"] = new SelectList(types, "TypeId", "TypeName");
            ViewData["roles"] = new SelectList(GetRoles(),"Value","Text");
            return View();
        }
        public List<SelectListItem> GetRoles()
        {
            /*_context.UserRoles.Select(ur => new { Int = ur.RoleInt, Role = ur.RoleInt.GetDisplayName() }).Distinct().ToList();*/
            List<SelectListItem> role = new List<SelectListItem>
            {
                new SelectListItem { Value = Role.Validator.ToString(), Text = "Validator" },
                new SelectListItem { Value = Role.Approver.ToString(), Text = "Approver" },
                new SelectListItem { Value = Role.ECNApprover.ToString(), Text = "ECN Approver" }
            }; 
            return role;
        }

        // POST: MyECOArea/UserRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,RequestTypeId,RoleInt,isActive")] UserRole _userRole)
        {
            if(_userRole.Id == 0)
            {
                if(_userRole.UserId != 0 && _userRole.RequestTypeId != 0 && _userRole.RoleInt != 0)
                {
                    _userRole.isActive = true;
                    string result = userRoleRepository.Add(_userRole);
                    ViewData["Result"] = result;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Create));
        }

        // GET: MyECOArea/UserRoles/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserRole userRole = _context.UserRoles.Where(ur => ur.Id == id).Include(u => u.User).Include(r=>r.RequestType).Single();
            if (userRole == null)
            {
                return NotFound();
            }
            ViewData["Roles"] = new SelectList(GetRoles(), "Value", "Text");
            ViewData["UserId"] = new SelectList(new List<SelectListItem>{ new SelectListItem { Text = userRole.User.Name, Value = userRole.UserId.ToString() } }, "Value", "Text");
            ViewData["RequestTypeId"] = new SelectList(new List<SelectListItem>{ new SelectListItem { Text = userRole.RequestType.Name, Value = userRole.RequestTypeId.ToString() } }, "Value", "Text");

            return View(userRole);
        }

        // POST: MyECOArea/UserRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId, RequestTypeId, RoleInt, isActive")] UserRole userRole)
        {
            if (id != userRole.Id)
            {
                return NotFound();
            }
            try
            {
                _context.Update(userRole);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRoleExists(userRole.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: MyECOArea/UserRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRole = await _context.UserRoles.Include(u=>u.User).Include(r=>r.RequestType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRole == null)
            {
                return NotFound();
            }

            return View(userRole);
        }

        // POST: MyECOArea/UserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userRole = await _context.UserRoles.FindAsync(id);
            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRoleExists(int id)
        {
            return _context.UserRoles.Any(e => e.Id == id);
        }
    }
}
