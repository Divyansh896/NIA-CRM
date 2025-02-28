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
    public class StratergyController : Controller
    {
        private readonly NIACRMContext _context;

        public StratergyController(NIACRMContext context)
        {
            _context = context;
        }

        // GET: Stratergy
        public async Task<IActionResult> Index()
        {
            return View(await _context.Strategys.ToListAsync());
        }

        // Export to Excel Action
        public IActionResult ExportToExcel()
        {
            // Get the data you want to export
            var strategies = _context.Strategies.ToList();

            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Strategies");

                // Add the header row
                worksheet.Cells[1, 1].Value = "Strategy Name";
                worksheet.Cells[1, 2].Value = "Assignee";
                worksheet.Cells[1, 3].Value = "Note";
                worksheet.Cells[1, 4].Value = "Created Date";
                worksheet.Cells[1, 5].Value = "Term";
                worksheet.Cells[1, 6].Value = "Status";

                // Add data rows
                for (int i = 0; i < strategies.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = strategies[i].StrategyName;
                    worksheet.Cells[i + 2, 2].Value = strategies[i].StrategyAssignee;
                    worksheet.Cells[i + 2, 3].Value = strategies[i].StrategyNote;
                    worksheet.Cells[i + 2, 4].Value = strategies[i].CreatedDate.ToString("yyyy-MM-dd"); // Format the Date
                    worksheet.Cells[i + 2, 5].Value = strategies[i].StrategyTerm;
                    worksheet.Cells[i + 2, 6].Value = strategies[i].StrategyStatus;
                }

                // Set the response headers for the Excel file
                var fileContents = package.GetAsByteArray();
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Strategies.xlsx");
            }
        }


        [HttpPost]
        public IActionResult ImportData(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Get first worksheet
                    var rowCount = worksheet.Dimension.Rows; // Get row count

                    // Create a list to hold the imported data
                    List<Strategy> strategies = new List<Strategy>();

                    for (int row = 2; row <= rowCount; row++)  // Assuming first row is headers
                    {
                        var strategy = new Strategy
                        {
                            StrategyName = worksheet.Cells[row, 1].Text,  // Column 1: StrategyName
                            StrategyAssignee = worksheet.Cells[row, 2].Text,  // Column 2: StrategyAssignee
                            StrategyNote = worksheet.Cells[row, 3].Text,  // Column 3: StrategyNote
                            CreatedDate = DateTime.Parse(worksheet.Cells[row, 4].Text),  // Column 4: CreatedDate
                            StrategyTerm = Enum.TryParse(worksheet.Cells[row, 5].Text, true, out StrategyTerm term) ? term : StrategyTerm.ShortTerm,  // Column 5: StrategyTerm
                            StrategyStatus = Enum.TryParse(worksheet.Cells[row, 6].Text, true, out StrategyStatus status) ? status : StrategyStatus.ToDo // Column 6: StrategyStatus
                        };

                        strategies.Add(strategy);
                    }

                    // Now you can save these strategies to the database
                    foreach (var strategy in strategies)
                    {
                        // Add your database saving logic here, e.g., _context.Strategies.Add(strategy);
                    }

                    // Optional: Show a success message
                    TempData["SuccessMessage"] = "Data imported successfully!";

                    return RedirectToAction("Index");
                }
            }

            // If the file wasn't uploaded, show an error
            TempData["ErrorMessage"] = "Please upload a valid file.";
            return RedirectToAction("Index");
        }

        // GET: Stratergy/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var strategy = await _context.Strategys
                .FirstOrDefaultAsync(m => m.ID == id);
            if (strategy == null)
            {
                return NotFound();
            }

            return View(strategy);
        }

        // GET: Stratergy/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stratergy/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,StrategyName,StrategyAssignee,StrategyNote,CreatedDate,StrategyTerm,StrategyStatus")] Strategy strategy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(strategy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(strategy);
        }

        // GET: Stratergy/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var strategy = await _context.Strategys.FindAsync(id);
            if (strategy == null)
            {
                return NotFound();
            }
            return View(strategy);
        }

        // POST: Stratergy/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,StrategyName,StrategyAssignee,StrategyNote,CreatedDate,StrategyTerm,StrategyStatus")] Strategy strategy)
        {
            if (id != strategy.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(strategy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StrategyExists(strategy.ID))
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
            return View(strategy);
        }

        // GET: Stratergy/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var strategy = await _context.Strategys
                .FirstOrDefaultAsync(m => m.ID == id);
            if (strategy == null)
            {
                return NotFound();
            }

            return View(strategy);
        }

        // POST: Stratergy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var strategy = await _context.Strategys.FindAsync(id);
            if (strategy != null)
            {
                _context.Strategys.Remove(strategy);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StrategyExists(int id)
        {
            return _context.Strategys.Any(e => e.ID == id);
        }
    }
}
