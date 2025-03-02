using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NIA_CRM.CustomControllers;
using NIA_CRM.Data;
using NIA_CRM.Models;
using NIA_CRM.Utilities;
using OfficeOpenXml;

namespace NIA_CRM.Controllers
{
    public class AnnualActionController : ElephantController
    {
        private readonly NIACRMContext _context;

        public AnnualActionController(NIACRMContext context)
        {
            _context = context;
        }

        // GET: AnnualAction
        public async Task<IActionResult> Index(int? page, int? pageSizeID)
        {
            var annualActions = _context.AnnualAction.AsNoTracking();

            // Handle paging
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<AnnualAction>.CreateAsync(annualActions, page ?? 1, pageSize);

            return View(pagedData);
        }

        // New action to export to Excel
        public IActionResult ExportToExcel()
        {
            // Get the data you want to export
            var annualActions = _context.AnnualActions.ToList();

            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Annual Actions");

                // Add the header row
                worksheet.Cells[1, 1].Value = "Name";
                worksheet.Cells[1, 2].Value = "Note";
                worksheet.Cells[1, 3].Value = "Date";
                worksheet.Cells[1, 4].Value = "Assignee";
                worksheet.Cells[1, 5].Value = "Annual Status";

                // Add data rows
                for (int i = 0; i < annualActions.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = annualActions[i].Name;
                    worksheet.Cells[i + 2, 2].Value = annualActions[i].Note;
                    worksheet.Cells[i + 2, 3].Value = annualActions[i].Date?.ToString("yyyy-MM-dd"); // Format the Date
                    worksheet.Cells[i + 2, 4].Value = annualActions[i].Asignee;
                    worksheet.Cells[i + 2, 5].Value = annualActions[i].AnnualStatus;
                }

                // Set the response headers
                var fileContents = package.GetAsByteArray();
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AnnualActions.xlsx");
            }
        }

        // Action to handle Excel file import
        [HttpPost]
        public IActionResult ImportFromExcel(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets[0]; // Assume data is on the first worksheet
                            var rowCount = worksheet.Dimension.Rows;

                            for (int row = 2; row <= rowCount; row++) // Start at 2 to skip header row
                            {
                                var name = worksheet.Cells[row, 1].Text;
                                var note = worksheet.Cells[row, 2].Text;
                                var date = DateTime.TryParse(worksheet.Cells[row, 3].Text, out DateTime parsedDate) ? parsedDate : (DateTime?)null;
                                var assignee = worksheet.Cells[row, 4].Text;
                                var status = worksheet.Cells[row, 5].Text;

                                // Create a new AnnualAction object and populate it
                                var annualAction = new AnnualAction
                                {
                                    Name = name,
                                    Note = note,
                                    Date = date,
                                    Asignee = assignee,
                                    AnnualStatus = Enum.TryParse(worksheet.Cells[row, 5].Value?.ToString(), true, out AnnualStatus annualStatus) ? annualStatus : AnnualStatus.ToDo


                                };

                                // Save the new object to the database (your db context logic here)
                                _context.AnnualActions.Add(annualAction);
                            }

                            _context.SaveChanges(); // Save the changes to the database
                        }
                    }

                    return RedirectToAction("Index"); // Redirect back to the Index page after successful import
                }
                catch (Exception ex)
                {
                    // Handle the error (e.g., log it, show a message to the user, etc.)
                    ModelState.AddModelError("", "An error occurred while importing the file: " + ex.Message);
                }
            }

            return View("Index"); // If no file was uploaded, return to the index page
        }

        // GET: AnnualAction/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annualAction = await _context.AnnualAction
                .FirstOrDefaultAsync(m => m.ID == id);
            if (annualAction == null)
            {
                return NotFound();
            }

            return View(annualAction);
        }

        // GET: AnnualAction/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AnnualAction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Note,Date,Asignee,AnnualStatus")] AnnualAction annualAction)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(annualAction);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (RetryLimitExceededException /* dex */)//This is a Transaction in the Database!
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. " +
                    "Try again, and if the problem persists, see your system administrator.");
            }
            catch (DbUpdateException dex)
            {
                string message = dex.GetBaseException().Message;
                
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                
            }

            return View(annualAction);
        }

        // GET: AnnualAction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annualAction = await _context.AnnualAction.FindAsync(id);
            if (annualAction == null)
            {
                return NotFound();
            }
            return View(annualAction);
        }

        // POST: AnnualAction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Byte[] RowVersion)
        {
           

            // Fetch the existing entity from the database
            var actionToUpdate = await _context.AnnualActions.FirstOrDefaultAsync(a => a.ID == id);

            

            if (actionToUpdate == null)
            {
                return NotFound();
            }

            // Attach the RowVersion for concurrency tracking
            _context.Entry(actionToUpdate).Property("RowVersion").OriginalValue = RowVersion;

            if (await TryUpdateModelAsync<AnnualAction>(actionToUpdate, "",
                a => a.Name, a => a.Note, a => a.Date, a => a.Asignee, a => a.AnnualStatus))
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
                    var clientValues = (AnnualAction)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();

                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "The record was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (AnnualAction)databaseEntry.ToObject();

                        if (databaseValues.Name != clientValues.Name)
                            ModelState.AddModelError("Name", $"Current value: {databaseValues.Name}");
                        if (databaseValues.Note != clientValues.Note)
                            ModelState.AddModelError("Note", $"Current value: {databaseValues.Note}");
                        if (databaseValues.Date != clientValues.Date)
                            ModelState.AddModelError("Date", $"Current value: {databaseValues.Date:d}");
                        if (databaseValues.Asignee != clientValues.Asignee)
                            ModelState.AddModelError("Asignee", $"Current value: {databaseValues.Asignee}");
                        if (databaseValues.AnnualStatus != clientValues.AnnualStatus)
                            ModelState.AddModelError("AnnualStatus", $"Current value: {databaseValues.AnnualStatus}");

                        ModelState.AddModelError("", "The record was modified by another user after you started editing. " +
                            "If you still want to save your changes, click the Save button again.");

                        // Update RowVersion for the next attempt
                        actionToUpdate.RowVersion = databaseValues.RowVersion ?? Array.Empty<byte>();
                        ModelState.Remove("RowVersion");
                    }
                }
                catch (DbUpdateException dex)
                {
                    string message = dex.GetBaseException().Message;
                    ModelState.AddModelError("", $"Unable to save changes: {message}");
                }
            }

            return View(actionToUpdate);
        }


        // GET: AnnualAction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annualAction = await _context.AnnualAction
                .FirstOrDefaultAsync(m => m.ID == id);
            if (annualAction == null)
            {
                return NotFound();
            }

            return View(annualAction);
        }

        // POST: AnnualAction/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var mEvent = await _context.AnnualAction.FindAsync(id);

                if (mEvent == null)
                {
                    return Json(new { success = false, message = "Event not found!" });
                }

                _context.AnnualActions.Remove(mEvent);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Annual Action deleted successfully!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting event: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while deleting the event." });
            }
        }

        private bool AnnualActionExists(int id)
        {
            return _context.AnnualAction.Any(e => e.ID == id);
        }

        public async Task<IActionResult> GetAnnualActionPreview(int id)
        {
            var member = await _context.AnnualAction
                .FirstOrDefaultAsync(m => m.ID == id); // No .Include() needed

            if (member == null)
            {
                return NotFound(); // Return 404 if the member doesn't exist
            }

            return PartialView("_AnnualActionPreview", member); // Ensure the partial view name matches
        }


        [HttpPost]
        public async Task<IActionResult> SaveAnnualActionNote(int id, string note)
        {
            var annualActionToUpdate = await _context.AnnualActions.FirstOrDefaultAsync(m => m.ID == id);

            if (annualActionToUpdate == null)
            {
                return Json(new { success = false, message = "Annual Action not found." });
            }

            // Update MemberNote
            annualActionToUpdate.Note = note;

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
