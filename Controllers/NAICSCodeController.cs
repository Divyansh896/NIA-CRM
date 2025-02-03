using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NIA_CRM.Data;
using NIA_CRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NIA_CRM.Controllers
{
    public class NAICSCodeController : Controller
    {
        private readonly NIACRMContext _context;
        private readonly NAICSApiHelper _apiHelper;

        public NAICSCodeController(NIACRMContext context, NAICSApiHelper apiHelper)
        {
            _context = context;
            _apiHelper = apiHelper;
        }

        // GET: NAICSCode
        public async Task<IActionResult> Index()
        {
            var naicsCodes = await _apiHelper.GetNAICSCodesAsync();

            if (naicsCodes == null || naicsCodes.Count == 0)
            {
                return View("Error"); // Handle empty data if needed
            }

            return View(naicsCodes);
        }

        // GET: NAICSCode/Details/5
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

        // GET: NAICSCode/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NAICSCode/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Label,Code,Description")] NAICSCode nAICSCode)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nAICSCode);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nAICSCode);
        }

        // GET: NAICSCode/Edit/5
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

        // POST: NAICSCode/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Label,Code,Description")] NAICSCode nAICSCode)
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

        // GET: NAICSCode/Delete/5
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

        // POST: NAICSCode/Delete/5
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
