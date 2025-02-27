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
    public class AnnualActionController : Controller
    {
        private readonly NIACRMContext _context;

        public AnnualActionController(NIACRMContext context)
        {
            _context = context;
        }

        // GET: AnnualAction
        public async Task<IActionResult> Index()
        {
            return View(await _context.AnnualAction.ToListAsync());
        }

        // GET: AnnualAction/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annualAction = await _context.AnnualAction
                .FirstOrDefaultAsync(m => m.ID == id);
            if (annualAction == null)
            {
                return NotFound();
            }

            return View(annualAction);
        }

        // GET: AnnualAction/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AnnualAction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Note,Date,Asignee,AnnualStatus")] AnnualAction annualAction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(annualAction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(annualAction);
        }

        // GET: AnnualAction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annualAction = await _context.AnnualAction.FindAsync(id);
            if (annualAction == null)
            {
                return NotFound();
            }
            return View(annualAction);
        }

        // POST: AnnualAction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Note,Date,Asignee,AnnualStatus")] AnnualAction annualAction)
        {
            if (id != annualAction.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(annualAction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnualActionExists(annualAction.ID))
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
            return View(annualAction);
        }

        // GET: AnnualAction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annualAction = await _context.AnnualAction
                .FirstOrDefaultAsync(m => m.ID == id);
            if (annualAction == null)
            {
                return NotFound();
            }

            return View(annualAction);
        }

        // POST: AnnualAction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var annualAction = await _context.AnnualAction.FindAsync(id);
            if (annualAction != null)
            {
                _context.AnnualAction.Remove(annualAction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnnualActionExists(int id)
        {
            return _context.AnnualAction.Any(e => e.ID == id);
        }
    }
}
