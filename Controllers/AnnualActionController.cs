using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(annualAction);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (RetryLimitExceededException /* dex */)//This is a Transaction in the Database!
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. " +
                    "Try again, and if the problem persists, see your system administrator.");
            }
            catch (DbUpdateException dex)
            {
                string message = dex.GetBaseException().Message;
                
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                
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
        public async Task<IActionResult> Edit(int id, Byte[] RowVersion)
        {
           

            // Fetch the existing entity from the database
            var actionToUpdate = await _context.AnnualActions.FirstOrDefaultAsync(a => a.ID == id);

            

            if (actionToUpdate == null)
            {
                return NotFound();
            }

            // Attach the RowVersion for concurrency tracking
            _context.Entry(actionToUpdate).Property("RowVersion").OriginalValue = RowVersion;

            if (await TryUpdateModelAsync<AnnualAction>(actionToUpdate, "",
                a => a.Name, a => a.Note, a => a.Date, a => a.Asignee, a => a.AnnualStatus))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again later.");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (AnnualAction)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();

                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "The record was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (AnnualAction)databaseEntry.ToObject();

                        if (databaseValues.Name != clientValues.Name)
                            ModelState.AddModelError("Name", $"Current value: {databaseValues.Name}");
                        if (databaseValues.Note != clientValues.Note)
                            ModelState.AddModelError("Note", $"Current value: {databaseValues.Note}");
                        if (databaseValues.Date != clientValues.Date)
                            ModelState.AddModelError("Date", $"Current value: {databaseValues.Date:d}");
                        if (databaseValues.Asignee != clientValues.Asignee)
                            ModelState.AddModelError("Asignee", $"Current value: {databaseValues.Asignee}");
                        if (databaseValues.AnnualStatus != clientValues.AnnualStatus)
                            ModelState.AddModelError("AnnualStatus", $"Current value: {databaseValues.AnnualStatus}");

                        ModelState.AddModelError("", "The record was modified by another user after you started editing. " +
                            "If you still want to save your changes, click the Save button again.");

                        // Update RowVersion for the next attempt
                        actionToUpdate.RowVersion = databaseValues.RowVersion ?? Array.Empty<byte>();
                        ModelState.Remove("RowVersion");
                    }
                }
                catch (DbUpdateException dex)
                {
                    string message = dex.GetBaseException().Message;
                    ModelState.AddModelError("", $"Unable to save changes: {message}");
                }
            }

            return View(actionToUpdate);
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
