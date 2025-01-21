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
    public class CancellationsController : Controller
    {
        private readonly NIACRMContext _context;

        public CancellationsController(NIACRMContext context)
        {
            _context = context;
        }

        // GET: Cancellations
        public async Task<IActionResult> Index()
        {
            var nIACRMContext = _context.Cancellations.Include(c => c.Member);
            return View(await nIACRMContext.ToListAsync());
        }

        // GET: Cancellations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cancellation = await _context.Cancellations
                .Include(c => c.Member)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cancellation == null)
            {
                return NotFound();
            }

            return View(cancellation);
        }

        // GET: Cancellations/Create
        public IActionResult Create()
        {
            ViewData["MemberID"] = new SelectList(_context.Members, "ID", "MemberName");
            return View();
        }

        // POST: Cancellations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CancellationDate,Canceled,CancellationNote,MemberID")] Cancellation cancellation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cancellation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberID"] = new SelectList(_context.Members, "ID", "MemberName", cancellation.MemberID);
            return View(cancellation);
        }

        // GET: Cancellations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cancellation = await _context.Cancellations.FindAsync(id);
            if (cancellation == null)
            {
                return NotFound();
            }
            ViewData["MemberID"] = new SelectList(_context.Members, "ID", "MemberName", cancellation.MemberID);
            return View(cancellation);
        }

        // POST: Cancellations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            // Fetch the cancellation entry from the database
            var cancellationToUpdate = await _context.Cancellations.FirstOrDefaultAsync(i => i.ID == id);

            // If the cancellation is not found, return NotFound
            if (cancellationToUpdate == null)
            {
                return NotFound();
            }

            // Try to update the model with the new values
            if (await TryUpdateModelAsync<Cancellation>(cancellationToUpdate, "",
                c => c.CancellationDate, c => c.Canceled, c => c.CancellationNote, c => c.MemberID))
            {
                try
                {
                    // Save the changes in the context
                    _context.Update(cancellationToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CancellationExists(cancellationToUpdate.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                // Redirect to Index action after successful update
                return RedirectToAction(nameof(Index));
            }

            // If the model update fails, return to the edit view with the current cancellation data
            ViewData["MemberID"] = new SelectList(_context.Members, "ID", "MemberName", cancellationToUpdate.MemberID);
            return View(cancellationToUpdate);
        }


        // GET: Cancellations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cancellation = await _context.Cancellations
                .Include(c => c.Member)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cancellation == null)
            {
                return NotFound();
            }

            return View(cancellation);
        }

        // POST: Cancellations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cancellation = await _context.Cancellations.FindAsync(id);
            if (cancellation != null)
            {
                _context.Cancellations.Remove(cancellation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CancellationExists(int id)
        {
            return _context.Cancellations.Any(e => e.ID == id);
        }
    }
}
