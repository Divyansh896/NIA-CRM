using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,CancellationDate,CancellationNote,IsCancelled,ContactID")] ContactCancellation contactCancellation)
        {
            if (id != contactCancellation.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactCancellation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactCancellationExists(contactCancellation.ID))
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
            ViewData["ContactID"] = new SelectList(_context.Contacts, "Id", "FirstName", contactCancellation.ContactID);
            return View(contactCancellation);
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
