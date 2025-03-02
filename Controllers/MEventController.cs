using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NIA_CRM.CustomControllers;
using NIA_CRM.Data;
using NIA_CRM.Models;
using NIA_CRM.Utilities;
using OfficeOpenXml;
using static Microsoft.IO.RecyclableMemoryStreamManager;

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
        public async Task<IActionResult> Index(int? page, int? pageSizeID)
        {
            var events = _context.MEvents
                .Include(d => d.MemberEvents).ThenInclude(d => d.Member)
                .AsNoTracking();

            //Handle Paging
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<MEvent>.CreateAsync(events.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);
        }
        // Export to Excel Action
        public IActionResult ExportToExcel()
        {
            // Get the data you want to export
            var events = _context.MEvents.ToList();

            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Events");

                // Add the header row
                worksheet.Cells[1, 1].Value = "Event Name";
                worksheet.Cells[1, 2].Value = "Event Description";
                worksheet.Cells[1, 3].Value = "Event Location";
                worksheet.Cells[1, 4].Value = "Event Date";

                // Add data rows
                for (int i = 0; i < events.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = events[i].EventName;
                    worksheet.Cells[i + 2, 2].Value = events[i].EventDescription;
                    worksheet.Cells[i + 2, 3].Value = events[i].EventLocation;
                    worksheet.Cells[i + 2, 4].Value = events[i].EventDate.ToString("yyyy-MM-dd"); // Format the Date
                }

                // Set the response headers
                var fileContents = package.GetAsByteArray();
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Events.xlsx");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ImportFromExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please select a valid Excel file.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension.Rows;

                        List<MEvent> events = new List<MEvent>();

                        for (int row = 2; row <= rowCount; row++) // Skip header row
                        {
                            events.Add(new MEvent
                            {
                                EventName = worksheet.Cells[row, 1].Value?.ToString(),
                                EventDescription = worksheet.Cells[row, 2].Value?.ToString(),
                                EventLocation = worksheet.Cells[row, 3].Value?.ToString(),
                                EventDate = worksheet.Cells[row, 4].Value != null && DateTime.TryParse(worksheet.Cells[row, 4].Value?.ToString(), out DateTime date) ? date
                                 : DateTime.MinValue// Default value if invalid or empty

                            });
                        }

                        _context.MEvents.AddRange(events);
                        await _context.SaveChangesAsync();
                    }
                }

                TempData["Success"] = "Events imported successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error importing data: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int id)
        {
            var eventModel = await _context.MEvents
                .Include(e => e.MemberEvents)
                .ThenInclude(me => me.Member)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventModel == null)
            {
                return NotFound();
            }

            // Fetch all members
            ViewBag.Members = await _context.Members
                .Select(m => new { Id = m.ID, m.MemberName })
                .ToListAsync();

            // Pre-select members assigned to this event
            ViewBag.SelectedMembers = eventModel.MemberEvents
                .Select(me => me.MemberId)
                .ToList();

            return View(eventModel);
        }

        // POST: MEvent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Byte[] RowVersion, List<int> SelectedMembers)
        {
            var mEventToUpdate = await _context.MEvents
                                               .Include(e => e.MemberEvents)
                                               .FirstOrDefaultAsync(e => e.Id == id);

            if (mEventToUpdate == null)
            {
                return NotFound();
            }

            // Attach RowVersion for concurrency tracking
            _context.Entry(mEventToUpdate).Property("RowVersion").OriginalValue = RowVersion;

            if (ModelState.IsValid)
            {
                if (await TryUpdateModelAsync<MEvent>(
                    mEventToUpdate, "",
                    m => m.EventName, m => m.EventDescription, m => m.EventLocation, m => m.EventDate))
                {
                    try
                    {
                        // ✅ Remove existing Member-Event relations before adding new ones
                        _context.MemberEvents.RemoveRange(mEventToUpdate.MemberEvents);
                        await _context.SaveChangesAsync();  // ✅ Save first to ensure clean state

                        // ✅ Add new selections only if `SelectedMembers` is not empty
                        if (SelectedMembers != null && SelectedMembers.Any())
                        {
                            foreach (var memberId in SelectedMembers)
                            {
                                _context.MemberEvents.Add(new MemberEvent { MEventID = mEventToUpdate.Id, MemberId = memberId });
                            }
                        }

                        await _context.SaveChangesAsync(); // ✅ Save after adding new members
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



        public async Task<IActionResult> GetEventPreview(int id)
        {
            var opportunity = await _context.MEvents.Include(m => m.MemberEvents).ThenInclude(m => m.Member).FirstOrDefaultAsync(m => m.Id == id); // Use async version for better performance

            if (opportunity == null)
            {
                return NotFound(); // Return 404 if the member doesn't exist
            }

            return PartialView("_EventPreview", opportunity); // Ensure the partial view name matches
        }

    }
}
