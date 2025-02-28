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
                                EventDate = worksheet.Cells[row, 4].Value != null && DateTime.TryParse(worksheet.Cells[row, 4].Value?.ToString(), out DateTime date)  ? date
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
