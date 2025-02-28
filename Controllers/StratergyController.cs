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
    public class StratergyController : Controller
    {
        private readonly NIACRMContext _context;

        public StratergyController(NIACRMContext context)
        {
            _context = context;
        }

        // GET: Stratergy
        public async Task<IActionResult> Index()
        {
            return View(await _context.Strategys.ToListAsync());
        }

        // GET: Stratergy/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var strategy = await _context.Strategys
                .FirstOrDefaultAsync(m => m.ID == id);
            if (strategy == null)
            {
                return NotFound();
            }

            return View(strategy);
        }

        // GET: Stratergy/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stratergy/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,StrategyName,StrategyAssignee,StrategyNote,CreatedDate,StrategyTerm,StrategyStatus")] Strategy strategy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(strategy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(strategy);
        }

        // GET: Stratergy/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var strategy = await _context.Strategys.FindAsync(id);
            if (strategy == null)
            {
                return NotFound();
            }
            return View(strategy);
        }

        // POST: Stratergy/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Byte[] RowVersion)
        {
            var strategyToUpdate = await _context.Strategies
                .FirstOrDefaultAsync(s => s.ID == id);

            if (strategyToUpdate == null)
            {
                return NotFound();
            }

            // Attach RowVersion for concurrency tracking
            _context.Entry(strategyToUpdate).Property("RowVersion").OriginalValue = RowVersion;

            // Try updating the model with user input
            if (ModelState.IsValid)
            {
                if (await TryUpdateModelAsync<Strategy>(
                    strategyToUpdate, "",
                    s => s.StrategyName, s => s.StrategyAssignee, s => s.StrategyNote,
                    s => s.CreatedDate, s => s.StrategyTerm, s => s.StrategyStatus))
                {
                    try
                    {
                        // Update the strategy record in the database
                        _context.Update(strategyToUpdate);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        var exceptionEntry = ex.Entries.Single();
                        var clientValues = (Strategy)exceptionEntry.Entity;
                        var databaseEntry = exceptionEntry.GetDatabaseValues();

                        if (databaseEntry == null)
                        {
                            ModelState.AddModelError("", "The strategy was deleted by another user.");
                        }
                        else
                        {
                            var databaseValues = (Strategy)databaseEntry.ToObject();
                            // Compare each field and provide feedback on changes
                            if (databaseValues.StrategyName != clientValues.StrategyName)
                                ModelState.AddModelError("StrategyName", $"Current value: {databaseValues.StrategyName}");
                            if (databaseValues.StrategyAssignee != clientValues.StrategyAssignee)
                                ModelState.AddModelError("StrategyAssignee", $"Current value: {databaseValues.StrategyAssignee}");
                            if (databaseValues.StrategyNote != clientValues.StrategyNote)
                                ModelState.AddModelError("StrategyNote", $"Current value: {databaseValues.StrategyNote}");
                            if (databaseValues.CreatedDate != clientValues.CreatedDate)
                                ModelState.AddModelError("CreatedDate", $"Current value: {databaseValues.CreatedDate}");
                            if (databaseValues.StrategyTerm != clientValues.StrategyTerm)
                                ModelState.AddModelError("StrategyTerm", $"Current value: {databaseValues.StrategyTerm}");
                            if (databaseValues.StrategyStatus != clientValues.StrategyStatus)
                                ModelState.AddModelError("StrategyStatus", $"Current value: {databaseValues.StrategyStatus}");

                            ModelState.AddModelError("", "The record was modified by another user after you started editing. If you still want to save your changes, click the Save button again.");
                            strategyToUpdate.RowVersion = databaseValues.RowVersion ?? Array.Empty<byte>();
                            ModelState.Remove("RowVersion");
                        }
                    }
                    catch (DbUpdateException dex)
                    {
                        string message = dex.GetBaseException().Message;
                        ModelState.AddModelError("", $"Unable to save changes: {message}");
                    }
                }
            }

            return View(strategyToUpdate);
        }


        // GET: Stratergy/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var strategy = await _context.Strategys
                .FirstOrDefaultAsync(m => m.ID == id);
            if (strategy == null)
            {
                return NotFound();
            }

            return View(strategy);
        }

        // POST: Stratergy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var strategy = await _context.Strategys.FindAsync(id);
            if (strategy != null)
            {
                _context.Strategys.Remove(strategy);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StrategyExists(int id)
        {
            return _context.Strategys.Any(e => e.ID == id);
        }
    }
}
