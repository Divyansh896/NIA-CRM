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
    public class MEventController : ElephantController
    {
        private readonly NIACRMContext _context;

        public MEventController(NIACRMContext context)
        {
            _context = context;
        }

        // GET: MEvent
        public async Task<IActionResult> Index(int? page, int? pageSizeID, DateTime? date, string? SearchString, string? actionButton,
                                              string sortDirection = "asc", string sortField = "Event Name")
        {
            string[] sortOptions = new[] { "Event Name" };  // You can add more sort options if needed
            int numberFilters = 0;

            var MEvents = _context.MEvents.AsQueryable();

            if (date.HasValue) // Check if date has a value instead of using TryParse
            {
                MEvents = MEvents.Where(c => c.EventDate.Date == date.Value.Date);
                numberFilters++;
                ViewData["DateFilter"] = date.Value.ToString("yyyy-MM-dd"); // Store in ViewData for UI persistence
            }


            if (!String.IsNullOrEmpty(SearchString))
            {
                MEvents = MEvents.Where(p => p.EventName.ToUpper().Contains(SearchString.ToUpper()));
                numberFilters++;
                ViewData["SearchString"] = SearchString;

            }

            if (!String.IsNullOrEmpty(actionButton)) //Form Submitted!
            {
                page = 1;//Reset page to start

                if (sortOptions.Contains(actionButton))//Change of sort is requested
                {
                    if (actionButton == sortField) //Reverse order on same field
                    {
                        sortDirection = sortDirection == "asc" ? "desc" : "asc";
                    }
                    sortField = actionButton;//Sort by the button clicked
                }
            }

            if (sortField == "Event Name")
            {
                if (sortDirection == "desc")
                {
                    MEvents = MEvents
                        .OrderByDescending(p => p.EventName);
                }
                else
                {
                    MEvents = MEvents
                        .OrderBy(p => p.EventName);

                }
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
            ViewData["records"] = $"Records Found: {MEvents.Count()}";

            // Handle paging
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<MEvent>.CreateAsync(MEvents.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,EventName,EventDescription,EventLocation,EventDate")] MEvent mEvent)
        {
            if (id != mEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MEventExists(mEvent.Id))
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
            return View(mEvent);
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
