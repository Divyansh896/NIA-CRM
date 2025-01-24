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
    public class CancellationsController : ElephantController
    {
        private readonly NIACRMContext _context;

        public CancellationsController(NIACRMContext context)
        {
            _context = context;
        }

        // GET: Cancellations
        public async Task<IActionResult> Index(int? page, int? pageSizeID, string? CancellationDateFilter, string? CancellationNoteFilter, string? actionButton,
                                        string sortDirection = "asc", string sortField = "Cancellation Date")
        {
            string[] sortOptions = new[] { "Cancellation Date", "Cancellation Note", "Canceled", "Member" };

            // Query for cancellations
            var cancellations = _context.Cancellations
                .Include(c => c.Member)
                .AsQueryable();


            // Filtering
            if (!string.IsNullOrEmpty(CancellationDateFilter) && DateTime.TryParse(CancellationDateFilter, out var date))
            {
                cancellations = cancellations.Where(c => c.CancellationDate.Date == date);
            }
            if (!string.IsNullOrEmpty(CancellationNoteFilter))
            {
                cancellations = cancellations.Where(c => c.CancellationNote.Contains(CancellationNoteFilter));
            }

            // Sorting
            if (!string.IsNullOrEmpty(actionButton))
            {
                if (sortOptions.Contains(actionButton))
                {
                    page = 1; // Reset page to start
                    if (actionButton == sortField) // Reverse order on the same field
                    {
                        sortDirection = sortDirection == "asc" ? "desc" : "asc";
                    }
                    sortField = actionButton; // Update sort field
                }
            }

            // Apply sorting
            cancellations = sortField switch
            {
                "Cancellation Date" => sortDirection == "asc"
                    ? cancellations.OrderBy(c => c.CancellationDate)
                    : cancellations.OrderByDescending(c => c.CancellationDate),
                "Cancellation Note" => sortDirection == "asc"
                    ? cancellations.OrderBy(c => c.CancellationNote)
                    : cancellations.OrderByDescending(c => c.CancellationNote),
                "Canceled" => sortDirection == "asc"
                    ? cancellations.OrderBy(c => c.Canceled)
                    : cancellations.OrderByDescending(c => c.Canceled),
                "Member" => sortDirection == "asc"
                    ? cancellations.OrderBy(c => c.Member.Summary)
                    : cancellations.OrderByDescending(c => c.Member.Summary),
                _ => cancellations
            };

            // Handle paging
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Cancellation>.CreateAsync(cancellations.AsNoTracking(), page ?? 1, pageSize);

            // Pass data to the view
            ViewData["SortDirection"] = sortDirection;
            ViewData["SortField"] = sortField;
            ViewData["CancellationDateFilter"] = CancellationDateFilter;
            ViewData["CancellationNoteFilter"] = CancellationNoteFilter;

            return View(pagedData);
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
