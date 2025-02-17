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
using OfficeOpenXml;

namespace NIA_CRM.Controllers
{
    public class MemberNoteController : ElephantController
    {
        private readonly NIACRMContext _context;

        public MemberNoteController(NIACRMContext context)
        {
            _context = context;
        }

        // GET: MemberNote
        public async Task<IActionResult> Index(int? page, int? pageSizeID)
        {
            var memberNotes = _context.MemberNotes.Include(m => m.Member);
            // Handle paging
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<MemberNote>.CreateAsync(memberNotes.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);


        }
        // Export Member Notes to Excel
        public IActionResult ExportToExcel()
        {
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial; 

            var memberNotes = _context.MemberNotes
                .Select(n => new
                {
                    n.Note,
                    CreatedAt = n.CreatedAt,// Format DateTime
                    Member = n.Member.MemberName
                })
                .ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Member Notes");

                // 🔹 Add Headers
                worksheet.Cells["A1"].Value = "Note";
                worksheet.Cells["B1"].Value = "Created At";
                worksheet.Cells["C1"].Value = "Member";

                using (var range = worksheet.Cells["A1:C1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // 🔹 Insert Data
                int row = 2;
                foreach (var note in memberNotes)
                {
                    worksheet.Cells[row, 1].Value = note.Note;
                    worksheet.Cells[row, 2].Value = note.CreatedAt;
                    worksheet.Cells[row, 3].Value = note.Member;
                    row++;
                }

                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MemberNotes.xlsx");
            }
        }

        // GET: MemberNote/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberNote = await _context.MemberNotes
                .Include(m => m.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (memberNote == null)
            {
                return NotFound();
            }

            return View(memberNote);
        }

        // GET: MemberNote/Create
        public IActionResult Create()
        {
            ViewData["MemberId"] = new SelectList(_context.Members, "ID", "MemberName");
            return View();
        }

        // POST: MemberNote/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MemberId,Note,CreatedAt")] MemberNote memberNote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(memberNote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Members, "ID", "MemberName", memberNote.MemberId);
            return View(memberNote);
        }

        // GET: MemberNote/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberNote = await _context.MemberNotes.FindAsync(id);
            if (memberNote == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_context.Members, "ID", "MemberName", memberNote.MemberId);
            return View(memberNote);
        }

        // POST: MemberNote/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MemberId,Note,CreatedAt")] MemberNote memberNote)
        {
            if (id != memberNote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(memberNote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberNoteExists(memberNote.Id))
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
            ViewData["MemberId"] = new SelectList(_context.Members, "ID", "MemberName", memberNote.MemberId);
            return View(memberNote);
        }

        // GET: MemberNote/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberNote = await _context.MemberNotes
                .Include(m => m.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (memberNote == null)
            {
                return NotFound();
            }

            return View(memberNote);
        }

        // POST: MemberNote/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var memberNote = await _context.MemberNotes.FindAsync(id);
            if (memberNote != null)
            {
                _context.MemberNotes.Remove(memberNote);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberNoteExists(int id)
        {
            return _context.MemberNotes.Any(e => e.Id == id);
        }
    }
}
