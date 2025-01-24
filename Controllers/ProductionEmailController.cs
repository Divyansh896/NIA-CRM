using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NIA_CRM.Data;
using NIA_CRM.Models;

namespace NIA_CRM.Controllers
{
    public class ProductionEmailController : Controller
    {
        private readonly NIACRMContext _context;

        public ProductionEmailController(NIACRMContext context)
        {
            _context = context;
        }

        // GET: ProductionEmail
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductionEmails.ToListAsync());
        }

        // GET: ProductionEmail/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productionEmail = await _context.ProductionEmails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productionEmail == null)
            {
                return NotFound();
            }

            return View(productionEmail);
        }

        // GET: ProductionEmail/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductionEmail/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmailType,Subject,Body")] ProductionEmail productionEmail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productionEmail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productionEmail);
        }

        // GET: ProductionEmail/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productionEmail = await _context.ProductionEmails.FindAsync(id);
            if (productionEmail == null)
            {
                return NotFound();
            }
            return View(productionEmail);
        }

        // POST: ProductionEmail/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmailType,Subject,Body")] ProductionEmail productionEmail)
        {
            if (id != productionEmail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productionEmail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductionEmailExists(productionEmail.Id))
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
            return View(productionEmail);
        }

        // GET: ProductionEmail/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productionEmail = await _context.ProductionEmails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productionEmail == null)
            {
                return NotFound();
            }

            return View(productionEmail);
        }

        // POST: ProductionEmail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productionEmail = await _context.ProductionEmails.FindAsync(id);
            if (productionEmail != null)
            {
                _context.ProductionEmails.Remove(productionEmail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductionEmailExists(int id)
        {
            return _context.ProductionEmails.Any(e => e.Id == id);
        }
    }
}
