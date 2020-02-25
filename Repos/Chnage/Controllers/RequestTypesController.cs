using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Chnage.Models;
using Chnage.Repository;

namespace Chnage.Controllers
{
    public class RequestTypesController : Controller
    {
        private readonly MyECODBContext _context;

        public RequestTypesController(MyECODBContext context)
        {
            _context = context;
        }

        // GET: RequestTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.RequestTypes.ToListAsync());
        }

        // GET: RequestTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestType = await _context.RequestTypes
                .Include(r => r.RequestTypeECRs)
                    .ThenInclude(e=>e.ECR)
                .Include(r => r.RequestTypeECOs)
                    .ThenInclude(e=>e.ECO)
                .FirstOrDefaultAsync(m => m.Id == id);

            SetViewDataPartials(requestType);

            if (requestType == null)
            {
                return NotFound();
            }

            return View(requestType);
        }

        public void SetViewDataPartials(RequestType requestType)
        {
            List<ECR> ECRsOfThisType = new List<ECR>();

            foreach (RequestTypeECR reqecr in requestType.RequestTypeECRs)
            {
                ECRsOfThisType.Add(reqecr.ECR);
            }
            List<ECO> ECOsOfThisType = new List<ECO>();

            foreach (RequestTypeECO reqECO in requestType.RequestTypeECOs)
            {
                ECOsOfThisType.Add(reqECO.ECO);
            }

            ViewData["ECRs"] = ECRsOfThisType;
            ViewData["ECOs"] = ECOsOfThisType;
        }


        // GET: RequestTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RequestTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] RequestType requestType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(requestType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(requestType);
        }

        // GET: RequestTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestType = await _context.RequestTypes.FindAsync(id);
            if (requestType == null)
            {
                return NotFound();
            }
            return View(requestType);
        }

        // POST: RequestTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] RequestType requestType)
        {
            if (id != requestType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requestType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestTypeExists(requestType.Id))
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
            return View(requestType);
        }

        // GET: RequestTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestType = await _context.RequestTypes
                .Include(r => r.RequestTypeECRs)
                    .ThenInclude(e => e.ECR)
                .Include(r => r.RequestTypeECOs)
                    .ThenInclude(e => e.ECO)
                .FirstOrDefaultAsync(m => m.Id == id);

            SetViewDataPartials(requestType);

            if (requestType == null)
            {
                return NotFound();
            }

            return View(requestType);
        }

        // POST: RequestTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requestType = await _context.RequestTypes.FindAsync(id);
            _context.RequestTypes.Remove(requestType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestTypeExists(int id)
        {
            return _context.RequestTypes.Any(e => e.Id == id);
        }
    }
}
