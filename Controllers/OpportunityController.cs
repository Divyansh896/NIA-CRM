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
    public class OpportunityController : Controller
    {
        private readonly NIACRMContext _context;

        public OpportunityController(NIACRMContext context)
        {
            _context = context;
        }

        // GET: Opportunity
        public async Task<IActionResult> Index()
        {
            return View(await _context.Opportunities.ToListAsync());
        }

        // Export Opportunities to Excel
        public IActionResult ExportOpportunitiesToExcel()
        {
            // Set the license context for EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var opportunities = _context.Opportunities.ToList();

            // Initialize the Excel package
            var package = new ExcelPackage();

            // Add a worksheet to the package
            var worksheet = package.Workbook.Worksheets.Add("Opportunities");

            // Add headers to the worksheet
            worksheet.Cells[1, 1].Value = "Opportunity Name";
            worksheet.Cells[1, 2].Value = "Opportunity Action";
            worksheet.Cells[1, 3].Value = "POC";
            worksheet.Cells[1, 4].Value = "Account";
            worksheet.Cells[1, 5].Value = "Interaction";
            worksheet.Cells[1, 6].Value = "Last Contact";
            worksheet.Cells[1, 7].Value = "Opportunity Status";
            worksheet.Cells[1, 8].Value = "Opportunity Priority";

            // Populate data in the worksheet
            int row = 2;
            foreach (var opportunity in opportunities)
            {
                worksheet.Cells[row, 1].Value = opportunity.OpportunityName;
                worksheet.Cells[row, 2].Value = opportunity.OpportunityAction;
                worksheet.Cells[row, 3].Value = opportunity.POC;
                worksheet.Cells[row, 4].Value = opportunity.Account;
                worksheet.Cells[row, 5].Value = opportunity.Interaction;
                worksheet.Cells[row, 6].Value = opportunity.LastContact?.ToString("yyyy-MM-dd"); // Format the date
                worksheet.Cells[row, 7].Value = opportunity.OpportunityStatus;
                worksheet.Cells[row, 8].Value = opportunity.OpportunityPriority;
                row++;
            }

            // Auto-fit columns for better readability
            worksheet.Cells.AutoFitColumns();

            // Save the package to a memory stream
            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0; // Reset the stream position before returning it

            // Define the Excel file name
            string excelName = "Opportunities.xlsx";

            // Return the file as a download
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpPost]
        public IActionResult ImportOpportunitiesFromExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please upload a valid Excel file.";
                return RedirectToAction("Index");
            }

            try
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension.Rows;

                        List<Opportunity> opportunities = new List<Opportunity>();

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var opportunity = new Opportunity
                            {
                                OpportunityName = worksheet.Cells[row, 1].Value?.ToString(),
                                OpportunityAction = worksheet.Cells[row, 2].Value?.ToString(),
                                POC = worksheet.Cells[row, 3].Value?.ToString(),
                                Account = worksheet.Cells[row, 4].Value?.ToString(),
                                Interaction = worksheet.Cells[row, 5].Value?.ToString(),
                                LastContact = DateTime.TryParse(worksheet.Cells[row, 6].Value?.ToString(), out DateTime lastContact) ? lastContact : (DateTime?)null,
                                OpportunityStatus = Enum.TryParse(worksheet.Cells[row, 7].Value?.ToString(), true, out OpportunityStatus status) ? status : OpportunityStatus.Qualification,
                                OpportunityPriority = Enum.TryParse(worksheet.Cells[row, 8].Value?.ToString(), true, out OpportunityPriority priority) ? priority : OpportunityPriority.Low
                            };

                            opportunities.Add(opportunity);
                        }

                        _context.Opportunities.AddRange(opportunities);
                        _context.SaveChanges();
                        TempData["Success"] = "Opportunities imported successfully!";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error importing opportunities: {ex.Message}";
            }

            return RedirectToAction("Index");
        }


        // GET: Opportunity/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunity = await _context.Opportunities
                .FirstOrDefaultAsync(m => m.ID == id);
            if (opportunity == null)
            {
                return NotFound();
            }

            return View(opportunity);
        }

        // GET: Opportunity/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Opportunity/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,OpportunityName,OpportunityAction,POC,Account,Interaction,LastContact,OpportunityStatus,OpportunityPriority")] Opportunity opportunity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(opportunity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(opportunity);
        }

        // GET: Opportunity/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunity = await _context.Opportunities.FindAsync(id);
            if (opportunity == null)
            {
                return NotFound();
            }
            return View(opportunity);
        }

        // POST: Opportunity/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,OpportunityName,OpportunityAction,POC,Account,Interaction,LastContact,OpportunityStatus,OpportunityPriority")] Opportunity opportunity)
        {
            if (id != opportunity.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(opportunity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OpportunityExists(opportunity.ID))
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
            return View(opportunity);
        }

        // GET: Opportunity/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunity = await _context.Opportunities
                .FirstOrDefaultAsync(m => m.ID == id);
            if (opportunity == null)
            {
                return NotFound();
            }

            return View(opportunity);
        }

        // POST: Opportunity/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var opportunity = await _context.Opportunities.FindAsync(id);
            if (opportunity != null)
            {
                _context.Opportunities.Remove(opportunity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        private bool OpportunityExists(int id)
        {
            return _context.Opportunities.Any(e => e.ID == id);
        }
    }
}
