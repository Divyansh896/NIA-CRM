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
using OfficeOpenXml;

namespace NIA_CRM.Controllers
{
    public class ContactCancellationController : Controller
    {
        private readonly NIACRMContext _context;

        public ContactCancellationController(NIACRMContext context)
        {
            _context = context;
        }

        // GET: ContactCancellation
        public async Task<IActionResult> Index()
        {
            var nIACRMContext = _context.ContactCancellations.Include(c => c.Contact);
            return View(await nIACRMContext.ToListAsync());
        }

        public IActionResult ExportToExcel()
        {
            // Get the data you want to export
            var cancellations = _context.ContactCancellations
                .Include(c => c.Contact) // Ensure related Contact data is included
                .ToList();

            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Contact Cancellations");

                // Add the header row
                worksheet.Cells[1, 1].Value = "Cancellation Date";
                worksheet.Cells[1, 2].Value = "Cancellation Note";
                worksheet.Cells[1, 3].Value = "Is Cancelled";
                worksheet.Cells[1, 4].Value = "Contact First Name";
                worksheet.Cells[1, 5].Value = "Contact Last Name";

                // Add data rows
                for (int i = 0; i < cancellations.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = cancellations[i].CancellationDate.ToString("yyyy-MM-dd");
                    worksheet.Cells[i + 2, 2].Value = cancellations[i].CancellationNote;
                    worksheet.Cells[i + 2, 3].Value = cancellations[i].IsCancelled;
                    worksheet.Cells[i + 2, 4].Value = cancellations[i].Contact.FirstName;
                    worksheet.Cells[i + 2, 5].Value = cancellations[i].Contact.LastName;
                }

                // Set the response headers for the Excel file
                var fileContents = package.GetAsByteArray();
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ContactCancellations.xlsx");
            }
        }

        // GET: ContactCancellation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactCancellation = await _context.ContactCancellations
                .Include(c => c.Contact)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (contactCancellation == null)
            {
                return NotFound();
            }

            return View(contactCancellation);
        }

        // GET: ContactCancellation/Create
        public IActionResult Create()
        {
            ViewData["ContactID"] = new SelectList(_context.Contacts, "Id", "FirstName");
            return View();
        }

        // POST: ContactCancellation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CancellationDate,CancellationNote,IsCancelled,ContactID")] ContactCancellation contactCancellation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactCancellation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContactID"] = new SelectList(_context.Contacts, "Id", "FirstName", contactCancellation.ContactID);
            return View(contactCancellation);
        }

        // GET: ContactCancellation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactCancellation = await _context.ContactCancellations.FindAsync(id);
            if (contactCancellation == null)
            {
                return NotFound();
            }
            ViewData["ContactID"] = new SelectList(_context.Contacts, "Id", "FirstName", contactCancellation.ContactID);
            return View(contactCancellation);
        }

        // POST: ContactCancellation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, byte[] RowVersion)
        {
            
            // Fetch the existing ContactCancellation record from the database
            var contactCancellationToUpdate = await _context.ContactCancellations
                .FirstOrDefaultAsync(c => c.ID == id);

            if (contactCancellationToUpdate == null)
            {
                return NotFound();
            }

            // Attach the RowVersion for concurrency tracking
            _context.Entry(contactCancellationToUpdate).Property("RowVersion").OriginalValue = RowVersion;

            if (await TryUpdateModelAsync<ContactCancellation>(contactCancellationToUpdate, "",
                c => c.CancellationDate, c => c.CancellationNote, c => c.IsCancelled, c => c.ContactID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Please try again later.");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (ContactCancellation)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();

                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "The record was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (ContactCancellation)databaseEntry.ToObject();

                        if (databaseValues.CancellationDate != clientValues.CancellationDate)
                            ModelState.AddModelError("CancellationDate", $"Current value: {databaseValues.CancellationDate:d}");
                        if (databaseValues.CancellationNote != clientValues.CancellationNote)
                            ModelState.AddModelError("CancellationNote", $"Current value: {databaseValues.CancellationNote}");
                        if (databaseValues.IsCancelled != clientValues.IsCancelled)
                            ModelState.AddModelError("IsCancelled", $"Current value: {databaseValues.IsCancelled}");
                        if (databaseValues.ContactID != clientValues.ContactID)
                        {
                            var databaseContact = await _context.Contacts
                                .FirstOrDefaultAsync(c => c.Id == databaseValues.ContactID);
                            ModelState.AddModelError("ContactID", $"Current value: {databaseContact?.FirstName}");
                        }

                        ModelState.AddModelError("", "The record was modified by another user after you started editing. " +
                            "If you still want to save your changes, click the Save button again.");

                        // Update RowVersion for the next attempt
                        contactCancellationToUpdate.RowVersion = databaseValues.RowVersion ?? Array.Empty<byte>();
                        ModelState.Remove("RowVersion");
                    }
                }
                catch (DbUpdateException dex)
                {
                    string message = dex.GetBaseException().Message;
                    ModelState.AddModelError("", $"Unable to save changes: {message}");
                }
            }

            ViewData["ContactID"] = new SelectList(_context.Contacts, "Id", "FirstName", contactCancellationToUpdate.ContactID);
            return View(contactCancellationToUpdate);
        }


        // GET: ContactCancellation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactCancellation = await _context.ContactCancellations
                .Include(c => c.Contact)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (contactCancellation == null)
            {
                return NotFound();
            }

            return View(contactCancellation);
        }

        // POST: ContactCancellation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactCancellation = await _context.ContactCancellations.FindAsync(id);
            if (contactCancellation != null)
            {
                _context.ContactCancellations.Remove(contactCancellation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactCancellationExists(int id)
        {
            return _context.ContactCancellations.Any(e => e.ID == id);
        }
    }
}
