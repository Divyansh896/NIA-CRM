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
    public class MEventController : Controller
    {
        private readonly NIACRMContext _context;

        public MEventController(NIACRMContext context)
        {
            _context = context;
        }

        // GET: MEvent
        public async Task<IActionResult> Index()
        {
            return View(await _context.MEvents.ToListAsync());
        }

        // GET: MEvent/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mEvent = await _context.MEvents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mEvent == null)
            {
                return NotFound();
            }

            return View(mEvent);
        }

        // GET: MEvent/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MEvent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventName,EventDescription,EventLocation,EventDate")] MEvent mEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mEvent);
        }

        // GET: MEvent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mEvent = await _context.MEvents.FindAsync(id);
            if (mEvent == null)
            {
                return NotFound();
            }
            return View(mEvent);
        }

        // POST: MEvent/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Byte[] RowVersion)
        {
            var mEventToUpdate = await _context.MEvents
                                                .FirstOrDefaultAsync(m => m.Id == id);

            if(mEventToUpdate == null)
            {
                return NotFound();
            }

            // Attach RowVersion for concurrency tracking
            _context.Entry(mEventToUpdate).Property("RowVersion").OriginalValue = RowVersion;

            if (ModelState.IsValid)
            {
                // Try updating the model with user input
                if (await TryUpdateModelAsync<MEvent>(
                    mEventToUpdate, "",
                    m => m.EventName, m => m.EventDescription, m => m.EventLocation, m => m.EventDate))
                {
                    try
                    {
                       
                        // Update the event record in the database
                        _context.Update(mEventToUpdate);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        var exceptionEntry = ex.Entries.Single();
                        var clientValues = (MEvent)exceptionEntry.Entity;
                        var databaseEntry = exceptionEntry.GetDatabaseValues();

                        if (databaseEntry == null)
                        {
                            ModelState.AddModelError("", "The event was deleted by another user.");
                        }
                        else
                        {
                            var databaseValues = (MEvent)databaseEntry.ToObject();
                            // Compare each field and provide feedback on changes
                            if (databaseValues.EventName != clientValues.EventName)
                                ModelState.AddModelError("EventName", $"Current value: {databaseValues.EventName}");
                            if (databaseValues.EventDescription != clientValues.EventDescription)
                                ModelState.AddModelError("EventDescription", $"Current value: {databaseValues.EventDescription}");
                            if (databaseValues.EventLocation != clientValues.EventLocation)
                                ModelState.AddModelError("EventLocation", $"Current value: {databaseValues.EventLocation}");
                            if (databaseValues.EventDate != clientValues.EventDate)
                                ModelState.AddModelError("EventDate", $"Current value: {databaseValues.EventDate}");

                            ModelState.AddModelError("", "The record was modified by another user after you started editing. If you still want to save your changes, click the Save button again.");
                            mEventToUpdate.RowVersion = databaseValues.RowVersion ?? Array.Empty<byte>();
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

            return View(mEventToUpdate);
        }

        // GET: MEvent/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mEvent = await _context.MEvents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mEvent == null)
            {
                return NotFound();
            }

            return View(mEvent);
        }

        // POST: MEvent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mEvent = await _context.MEvents.FindAsync(id);
            if (mEvent != null)
            {
                _context.MEvents.Remove(mEvent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MEventExists(int id)
        {
            return _context.MEvents.Any(e => e.Id == id);
        }
    }
}
