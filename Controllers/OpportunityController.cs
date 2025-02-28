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
    public class OpportunityController : Controller
    {
        private readonly NIACRMContext _context;

        public OpportunityController(NIACRMContext context)
        {
            _context = context;
        }

        // GET: Opportunity
        public async Task<IActionResult> Index()
        {
            return View(await _context.Opportunities.ToListAsync());
        }

        // GET: Opportunity/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunity = await _context.Opportunities
                .FirstOrDefaultAsync(m => m.ID == id);
            if (opportunity == null)
            {
                return NotFound();
            }

            return View(opportunity);
        }

        // GET: Opportunity/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Opportunity/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,OpportunityName,OpportunityAction,POC,Account,Interaction,LastContact,OpportunityStatus,OpportunityPriority")] Opportunity opportunity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(opportunity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(opportunity);
        }

        // GET: Opportunity/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunity = await _context.Opportunities.FindAsync(id);
            if (opportunity == null)
            {
                return NotFound();
            }
            return View(opportunity);
        }

        // POST: Opportunity/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, byte[] RowVersion)
        {
            var opportunityToUpdate = await _context.Opportunities
                .FirstOrDefaultAsync(o => o.ID == id);

            if (opportunityToUpdate == null)
            {
                return NotFound();
            }

            // Attach RowVersion for concurrency tracking
            _context.Entry(opportunityToUpdate).Property("RowVersion").OriginalValue = RowVersion;

            // Try updating the model with user input
            if (ModelState.IsValid)
            {
                if (await TryUpdateModelAsync<Opportunity>(
                    opportunityToUpdate, "",
                    o => o.OpportunityName, o => o.OpportunityAction, o => o.POC, o => o.Account,
                    o => o.Interaction, o => o.LastContact, o => o.OpportunityStatus, o => o.OpportunityPriority))
                {
                    try
                    {
                        // Update the opportunity record in the database
                        _context.Update(opportunityToUpdate);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        var exceptionEntry = ex.Entries.Single();
                        var clientValues = (Opportunity)exceptionEntry.Entity;
                        var databaseEntry = exceptionEntry.GetDatabaseValues();

                        if (databaseEntry == null)
                        {
                            ModelState.AddModelError("", "The opportunity was deleted by another user.");
                        }
                        else
                        {
                            var databaseValues = (Opportunity)databaseEntry.ToObject();
                            // Compare each field and provide feedback on changes
                            if (databaseValues.OpportunityName != clientValues.OpportunityName)
                                ModelState.AddModelError("OpportunityName", $"Current value: {databaseValues.OpportunityName}");
                            if (databaseValues.OpportunityAction != clientValues.OpportunityAction)
                                ModelState.AddModelError("OpportunityAction", $"Current value: {databaseValues.OpportunityAction}");
                            if (databaseValues.POC != clientValues.POC)
                                ModelState.AddModelError("POC", $"Current value: {databaseValues.POC}");
                            if (databaseValues.Account != clientValues.Account)
                                ModelState.AddModelError("Account", $"Current value: {databaseValues.Account}");
                            if (databaseValues.Interaction != clientValues.Interaction)
                                ModelState.AddModelError("Interaction", $"Current value: {databaseValues.Interaction}");
                            if (databaseValues.LastContact != clientValues.LastContact)
                                ModelState.AddModelError("LastContact", $"Current value: {databaseValues.LastContact}");
                            if (databaseValues.OpportunityStatus != clientValues.OpportunityStatus)
                                ModelState.AddModelError("OpportunityStatus", $"Current value: {databaseValues.OpportunityStatus}");
                            if (databaseValues.OpportunityPriority != clientValues.OpportunityPriority)
                                ModelState.AddModelError("OpportunityPriority", $"Current value: {databaseValues.OpportunityPriority}");

                            ModelState.AddModelError("", "The record was modified by another user after you started editing. If you still want to save your changes, click the Save button again.");
                            opportunityToUpdate.RowVersion = databaseValues.RowVersion ?? Array.Empty<byte>();
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

            return View(opportunityToUpdate);
        }


        // GET: Opportunity/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunity = await _context.Opportunities
                .FirstOrDefaultAsync(m => m.ID == id);
            if (opportunity == null)
            {
                return NotFound();
            }

            return View(opportunity);
        }

        // POST: Opportunity/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var opportunity = await _context.Opportunities.FindAsync(id);
            if (opportunity != null)
            {
                _context.Opportunities.Remove(opportunity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OpportunityExists(int id)
        {
            return _context.Opportunities.Any(e => e.ID == id);
        }
    }
}
