using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Chnage.Models;
using Chnage.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers
{
    public class UsersController : Controller
    {
        private readonly MyECODBContext _context;
        private User loggedUser;

        public UsersController(MyECODBContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            loggedUser = GetLoggedInUser();
        }
        private User GetLoggedInUser()
        {
            return _context.Users.Where(u => u.Email == GetLoggedInUserEmail()).Single();
        }

        // GET: MyECOArea/Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        public async Task<IActionResult> Profile()
        {
            int userId = loggedUser.Id;
            //int userId = _context.Users.Where(u => u.Email == GetLoggedInUserEmail()).FirstOrDefault().Id;
            User UserInformation = await _context.Users.Where(u => u.Id == userId)
                .Include(u => u.ECRs)
                .Include(u => u.ECOs)
                .Include(u => u.ECNs)
                .SingleAsync();
            return View(UserInformation);
        }

        // GET: MyECOArea/Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: MyECOArea/Users/Create
        public IActionResult Create()
        {
            //User loggedUser = new User
            //{
            //    Name = GetLoggedInUsername(),
            //    Email = GetLoggedInUserEmail(),
            //    isActive = true
            //};
            return View();
        }

        // POST: MyECOArea/Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,isActive")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Users");
            }
            return View(user);
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

        // GET: MyECOArea/Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: MyECOArea/Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,isActive")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    if (!user.isActive)
                    {
                        var userRoles = _context.UserRoles.Where(ur => ur.UserId == user.Id);
                        UpdateUserRoles(userRoles);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }
        public void UpdateUserRoles(IQueryable<UserRole> userRoles)
        {
            foreach(UserRole role in userRoles)
            {
                role.isActive = false;
            }
            _context.UserRoles.UpdateRange(userRoles);
        }

        // GET: MyECOArea/Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: MyECOArea/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
