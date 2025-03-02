using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
                ViewData["SearchString"] = SearchString;

            }
            if (cancelled)
            {
                cancellations = cancellations.Where(c => c.IsCancelled);
                numberFilters++;
                ViewData["cancelledFilter"] = "Applied";

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
                // Assuming you have a Members entity or lookup to fetch the name by ID
                var member = _context.Members.FirstOrDefault(m => m.ID == Members.Value);

                if (member != null)
                {
                    cancellations = cancellations.Where(p => p.MemberID == Members.Value);
                    numberFilters++;
                    ViewData["MembersFilter"] = member.MemberName; // Set the member's name in ViewData
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
            ViewData["MemberID"] = new SelectList(_context.Members, "ID", "MemberName");
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
            ViewData["MemberID"] = new SelectList(_context.Members, "ID", "MemberName", cancellation.MemberID);
            return View(cancellation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Archive(int id)
        {
            var member = await _context.Members.FindAsync(id);

            if (member == null)
            {
                return NotFound(); // Return 404 if member not found
            }

            var cancellation = new Cancellation
            {
                MemberID = member.ID,
                CancellationDate = DateTime.UtcNow, // Set default cancellation date
                IsCancelled = true, // Mark as canceled
                CancellationNote = "Archived via system." // Optional note
            };

            _context.Cancellations.Add(cancellation);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Member archived successfully!" });
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
            ViewData["MemberID"] = new SelectList(_context.Members, "ID", "MemberName", cancellation.MemberID);
            return View(cancellation);
        }

        // POST: Cancellation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Byte[] RowVersion)
        {
            
            // Fetch the existing cancellation record from the database
            var cancellationToUpdate = await _context.Cancellations.FirstOrDefaultAsync(c => c.ID == id);

            if (cancellationToUpdate == null)
            {
                return NotFound();
            }

            // Attach the RowVersion for concurrency tracking
            _context.Entry(cancellationToUpdate).Property("RowVersion").OriginalValue = RowVersion;

            if (await TryUpdateModelAsync<Cancellation>(cancellationToUpdate, "",
                c => c.CancellationDate, c => c.IsCancelled, c => c.CancellationNote, c => c.MemberID))
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
                    var clientValues = (Cancellation)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();

                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "The record was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Cancellation)databaseEntry.ToObject();

                        if (databaseValues.CancellationDate != clientValues.CancellationDate)
                            ModelState.AddModelError("CancellationDate", $"Current value: {databaseValues.CancellationDate:d}");
                        if (databaseValues.IsCancelled != clientValues.IsCancelled)
                            ModelState.AddModelError("Canceled", $"Current value: {databaseValues.IsCancelled}");
                        if (databaseValues.CancellationNote != clientValues.CancellationNote)
                            ModelState.AddModelError("CancellationNote", $"Current value: {databaseValues.CancellationNote}");
                        if (databaseValues.MemberID != clientValues.MemberID)
                        {
                            var databaseMember = await _context.Members
                                .FirstOrDefaultAsync(m => m.ID == databaseValues.MemberID);
                            ModelState.AddModelError("MemberID", $"Current value: {databaseMember?.MemberName}");
                        }

                        ModelState.AddModelError("", "The record was modified by another user after you started editing. " +
                            "If you still want to save your changes, click the Save button again.");

                        // Update RowVersion for the next attempt
                        cancellationToUpdate.RowVersion = databaseValues.RowVersion ?? Array.Empty<byte>();
                        ModelState.Remove("RowVersion");
                    }
                }
                catch (DbUpdateException dex)
                {
                    string message = dex.GetBaseException().Message;
                    ModelState.AddModelError("", $"Unable to save changes: {message}");
                }
            }

            ViewData["MemberID"] = new SelectList(_context.Members, "ID", "MemberName", cancellationToUpdate.MemberID);
            return View(cancellationToUpdate);
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


        public async Task<IActionResult> GetMemberPreview(int id)
        {
            var member = await _context.Members
                .Include(m => m.Addresses) // Include the related Address
                .Include(m => m.MemberThumbnail)
                .Include(m => m.MemberMembershipTypes)
                .ThenInclude(mm => mm.MembershipType)
                .Include(m => m.MemberContacts).ThenInclude(m => m.Contact)
                .Include(m => m.IndustryNAICSCodes).ThenInclude(m => m.NAICSCode)
                .FirstOrDefaultAsync(m => m.ID == id); // Use async version for better performance

            if (member == null)
            {
                return NotFound(); // Return 404 if the member doesn't exist
            }

            return PartialView("_MemberPreview", member); // Ensure the partial view name matches
        }
    }
}
