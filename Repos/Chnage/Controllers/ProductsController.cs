using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Chnage.Models;
using Chnage.Repository;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Chnage.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Controllers
{
    public class ProductsController : Controller
    {
        private readonly MyECODBContext _context;
        private readonly IMiddleTables middleTablesRepository;

        public ProductsController(MyECODBContext context, IMiddleTables middleTables)
        {
            _context = context;
            middleTablesRepository = middleTables;
        }

        // GET: MyECOArea/Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: MyECOArea/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);

            IEnumerable<ProductECR> relatedECRs = middleTablesRepository.GetProductECRsByProduct(product.Id);
            List<ECR> listECRs = new List<ECR>();

            foreach (ProductECR ecr in relatedECRs)
            {
                listECRs.Add(ecr.ECR);
            }

            IEnumerable<ProductECO> relatedECOs = middleTablesRepository.GetProductECOsByProduct(product.Id);
            List<ECO> listECOs = new List<ECO>();
            foreach (ProductECO eco  in relatedECOs)
            {
                listECOs.Add(eco.ECO);
            }

            ViewData["relatedECRs"] = listECRs;
            ViewData["relatedECOs"] = listECOs;

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: MyECOArea/Products/Create
        public IActionResult Create()
        {
            IQueryable<IGrouping<string, Product>> categoriesList = _context.Products.GroupBy(p => p.Category).Distinct();
            ViewData["Categories"] = new SelectList(categoriesList,"Key","Key");
            return View();
        }

        // POST: MyECOArea/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Category")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: MyECOArea/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);

            IQueryable<IGrouping<string, Product>> categoriesList = _context.Products.GroupBy(p => p.Category).Distinct();
            ViewData["Categories"] = new SelectList(categoriesList, "Key", "Key");

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: MyECOArea/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Category")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: MyECOArea/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: MyECOArea/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
