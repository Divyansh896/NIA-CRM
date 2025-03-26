using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NIA_CRM.Data;
using NIA_CRM.Models;

namespace NIA_CRM.Controllers
{
    [Authorize]
    public class NAICSCodesController : Controller
    {
        private readonly NIACRMContext _context;

        public NAICSCodesController(NIACRMContext context)
        {
            _context = context;
        }

        // GET: NAICSCodes
        public async Task<IActionResult> Index()
        {
            return View(await _context.NAICSCodes.ToListAsync());
        }

        // GET: NAICSCodes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nAICSCode = await _context.NAICSCodes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nAICSCode == null)
            {
                return NotFound();
            }

            return View(nAICSCode);
        }

        // GET: NAICSCodes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NAICSCodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Description")] NAICSCode nAICSCode)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nAICSCode);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nAICSCode);
        }

        // GET: NAICSCodes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nAICSCode = await _context.NAICSCodes.FindAsync(id);
            if (nAICSCode == null)
            {
                return NotFound();
            }
            return View(nAICSCode);
        }

        // POST: NAICSCodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Description")] NAICSCode nAICSCode)
        {
            if (id != nAICSCode.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nAICSCode);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NAICSCodeExists(nAICSCode.Id))
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
            return View(nAICSCode);
        }

        // GET: NAICSCodes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nAICSCode = await _context.NAICSCodes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nAICSCode == null)
            {
                return NotFound();
            }

            return View(nAICSCode);
        }

        // POST: NAICSCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nAICSCode = await _context.NAICSCodes.FindAsync(id);
            if (nAICSCode != null)
            {
                _context.NAICSCodes.Remove(nAICSCode);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NAICSCodeExists(int id)
        {
            return _context.NAICSCodes.Any(e => e.Id == id);
        }
    }
}
