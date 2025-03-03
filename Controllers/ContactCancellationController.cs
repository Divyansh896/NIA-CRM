using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NIA_CRM.CustomControllers;
using NIA_CRM.Data;
using NIA_CRM.Models;
using NIA_CRM.Utilities;

namespace NIA_CRM.Controllers
{
    public class ContactCancellationController : ElephantController
    {
        private readonly NIACRMContext _context;

        public ContactCancellationController(NIACRMContext context)
        {
            _context = context;
        }

        // GET: ContactCancellation
        public async Task<IActionResult> Index(int? page, int? pageSizeID, DateTime? dateFrom, DateTime? dateTo)
        {
            int numberFilters = 0;
            var contactCancellations = _context.ContactCancellations.Include(c => c.Contact).AsQueryable();

            // Filtering by date range (CancellationDate between dateFrom and dateTo)
            if (dateFrom.HasValue && dateTo.HasValue)
            {
                contactCancellations = contactCancellations
                    .Where(c => c.CancellationDate.Date >= dateFrom.Value.Date && c.CancellationDate.Date <= dateTo.Value.Date);
                numberFilters++;
                ViewData["DateFilterFrom"] = dateFrom.Value.ToString("yyyy-MM-dd");
                ViewData["DateFilterTo"] = dateTo.Value.ToString("yyyy-MM-dd");
            }
            else if (dateFrom.HasValue) // If only 'From' date is selected
            {
                contactCancellations = contactCancellations.Where(c => c.CancellationDate.Date >= dateFrom.Value.Date);
                numberFilters++;
                ViewData["DateFilterFrom"] = dateFrom.Value.ToString("yyyy-MM-dd");
            }
            else if (dateTo.HasValue) // If only 'To' date is selected
            {
                contactCancellations = contactCancellations.Where(c => c.CancellationDate.Date <= dateTo.Value.Date);
                numberFilters++;
                ViewData["DateFilterTo"] = dateTo.Value.ToString("yyyy-MM-dd");
            }

            // Give feedback about applied filters
            if (numberFilters != 0)
            {
                ViewData["Filtering"] = " btn-danger";
                ViewData["numberFilters"] = $"({numberFilters} Filter{(numberFilters > 1 ? "s" : "")} Applied)";
                ViewData["ShowFilter"] = " show";
            }

            ViewData["numberFilters"] = numberFilters;
            ViewData["records"] = $"Records Found: {contactCancellations.Count()}";

            // Handle paging
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<ContactCancellation>.CreateAsync(contactCancellations.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);
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

        public async Task<IActionResult> GetCancellationPreview(int id)
        {
            var member = await _context.ContactCancellations
                .Include(m => m.Contact).ThenInclude(m => m.MemberContacts).ThenInclude(m => m.Member).FirstOrDefaultAsync(m => m.ID == id); // Use async version for better performance

            if (member == null)
            {
                return NotFound(); // Return 404 if the member doesn't exist
            }

            return PartialView("_ContactCancellationPreview", member); // Ensure the partial view name matches
        }

        [HttpPost]
        public async Task<IActionResult> SaveCancellationNote(int id, string note)
        {
            var memberToUpdate = await _context.ContactCancellations.FirstOrDefaultAsync(m => m.ID == id);

            if (memberToUpdate == null)
            {
                return Json(new { success = false, message = "Cancellation Contact not found." });
            }

            // Update MemberNote
            memberToUpdate.CancellationNote = note;

            try
            {
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Note saved successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }


    }
}
