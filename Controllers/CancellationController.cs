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
    public class CancellationController : ElephantController
    {
        private readonly NIACRMContext _context;

        public CancellationController(NIACRMContext context)
        {
            _context = context;
        }

        // GET: Cancellation
        public async Task<IActionResult> Index(int? page, int? pageSizeID, int? Members, string? SearchString, bool cancelled, string? actionButton,
                                              string sortDirection = "asc", string sortField = "Member")
        {
            PopulateDropdowns();
            string[] sortOptions = new[] { "Member" };

            var cancellations = _context.Cancellations.Include(c => c.Member).AsQueryable();

            int numberFilters = 0;

            if (!string.IsNullOrEmpty(SearchString))
            {
                cancellations = cancellations.Where(m =>
                    m.Member.MemberName.ToUpper().Contains(SearchString.ToUpper()));
                numberFilters++;
            }
            if (cancelled)
            {
                cancellations = cancellations.Where(c => c.Canceled);
                numberFilters++;
            }

            if (sortField == "Member")
            {
                if (sortDirection == "desc")
                {
                    cancellations = cancellations
                        .OrderByDescending(p => p.Member.MemberName)
                        .ThenByDescending(p => p.Member.MemberName);

                }
                else
                {
                    cancellations = cancellations
                        .OrderBy(p => p.Member.MemberName)
                        .ThenBy(p => p.Member.MemberName);

                }
            }

            if (Members.HasValue)
            {
                cancellations = cancellations.Where(p => p.MemberID == Members);
                numberFilters++;
            }
            //Give feedback about the state of the filters
            if (numberFilters != 0)
            {
                //Toggle the Open/Closed state of the collapse depending on if we are filtering
                ViewData["Filtering"] = " btn-danger";
                //Show how many filters have been applied
                ViewData["numberFilters"] = "(" + numberFilters.ToString()
                    + " Filter" + (numberFilters > 1 ? "s" : "") + " Applied)";
                //Keep the Bootstrap collapse open
                @ViewData["ShowFilter"] = " show";
            }

            ViewData["SortDirection"] = sortDirection;
            ViewData["SortField"] = sortField;
            ViewData["numberFilters"] = numberFilters;
            //ViewData["records"] = $"Records Found: {contacts.Count()}";
            // Handle paging
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Cancellation>.CreateAsync(cancellations.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);
        }

        // GET: Cancellation/Details/5
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

        // GET: Cancellation/Create
        public IActionResult Create()
        {
            ViewData["MemberID"] = new SelectList(_context.Members, "ID", "MemberFirstName");
            return View();
        }

        // POST: Cancellation/Create
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
            ViewData["MemberID"] = new SelectList(_context.Members, "ID", "MemberFirstName", cancellation.MemberID);
            return View(cancellation);
        }

        // GET: Cancellation/Edit/5
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
            ViewData["MemberID"] = new SelectList(_context.Members, "ID", "MemberFirstName", cancellation.MemberID);
            return View(cancellation);
        }

        // POST: Cancellation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CancellationDate,Canceled,CancellationNote,MemberID")] Cancellation cancellation)
        {
            if (id != cancellation.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cancellation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CancellationExists(cancellation.ID))
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
            ViewData["MemberID"] = new SelectList(_context.Members, "ID", "MemberFirstName", cancellation.MemberID);
            return View(cancellation);
        }

        // GET: Cancellation/Delete/5
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

        // POST: Cancellation/Delete/5
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

        private void PopulateDropdowns()
        {
            // Fetch Members for dropdown
            var members = _context.Members.ToList();
            ViewData["Members"] = new SelectList(members, "ID", "MemberName");

        }
    }
}
