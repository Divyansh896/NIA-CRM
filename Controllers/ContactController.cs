using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class ContactController : ElephantController
    {
        private readonly NIACRMContext _context;

        public ContactController(NIACRMContext context)
        {
            _context = context;
        }

        // GET: Contact
        public async Task<IActionResult> Index(int? page, int? pageSizeID, string? Departments, string? Titles, bool IsVIP, string? SearchString, string? actionButton,
                                              string sortDirection = "asc", string sortField = "Contact Name")
        {
            PopulateDropdownLists();
            string[] sortOptions = new[] { "Contact Name" };  // You can add more sort options if needed

            int numberFilters = 0;



            var contacts = _context.Contacts.Include(c => c.Member).AsQueryable();
            if (Departments != null)
            {
                contacts = contacts.Where(c => c.Department == Departments);
                numberFilters++;
                ViewData["DepartmentsFilter"] = Departments;
            }
            if (Titles != null)
            {
                contacts = contacts.Where(c => c.Title == Titles);
                numberFilters++;
                ViewData["TitlesFilter"] = Titles;

            }
            if (IsVIP)
            {
                contacts = contacts.Where(c => c.IsVip);
                numberFilters++;
                ViewData["IsVIPFilter"] = "VIP"; // Set filter value for VIP

            }

            if (!String.IsNullOrEmpty(SearchString))
            {
                contacts = contacts.Where(p => p.LastName.ToUpper().Contains(SearchString.ToUpper())
                                       || p.FirstName.ToUpper().Contains(SearchString.ToUpper()));
                numberFilters++;
                ViewData["SearchString"] = SearchString;

            }

            // Check if the actionButton is "ExportExcel" BEFORE applying filters
            if (!string.IsNullOrEmpty(actionButton) && actionButton == "ExportExcel")
            {
                var exportData = await contacts.AsNoTracking().ToListAsync();
                return ExportContactsToExcel(exportData);
            }

            if (!String.IsNullOrEmpty(actionButton) ) //Form Submitted!
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

            if (sortField == "Contact Name")
            {
                if (sortDirection == "desc")
                {
                    contacts = contacts
                        .OrderByDescending(p => p.FirstName)
                        .ThenByDescending(p => p.LastName);

                }
                else
                {
                    contacts = contacts
                        .OrderBy(p => p.FirstName)
                        .ThenBy(p => p.LastName);

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
            ViewData["records"] = $"Records Found: {contacts.Count()}";

            // Handle paging
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Contact>.CreateAsync(contacts.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);

            

        }
        private IActionResult ExportContactsToExcel(List<Contact> contacts)
        {
            //ExcelPackage = LicenseContext;

            var package = new ExcelPackage(); // No 'using' block to avoid disposal
            var worksheet = package.Workbook.Worksheets.Add("Contacts");

            // Adding headers
            worksheet.Cells[1, 1].Value = "Name";
            worksheet.Cells[1, 2].Value = "Title";
            worksheet.Cells[1, 3].Value = "Department";
            worksheet.Cells[1, 4].Value = "Email";
            worksheet.Cells[1, 5].Value = "Phone";
            worksheet.Cells[1, 6].Value = "LinkedIn URL";
            worksheet.Cells[1, 7].Value = "Is VIP";
            worksheet.Cells[1, 8].Value = "Member Name";

            // Populating data
            int row = 2;
            foreach (var contact in contacts)
            {
                worksheet.Cells[row, 1].Value = contact.Summary;
                worksheet.Cells[row, 2].Value = contact.Title;
                worksheet.Cells[row, 3].Value = contact.Department;
                worksheet.Cells[row, 4].Value = contact.Email;
                worksheet.Cells[row, 5].Value = contact.PhoneFormatted;
                worksheet.Cells[row, 6].Value = contact.LinkedInUrl;
                worksheet.Cells[row, 7].Value = contact.IsVip ? "Yes" : "No";
                worksheet.Cells[row, 8].Value = contact.Member?.MemberName;
                row++;
            }

            // Auto-fit columns for better readability
            worksheet.Cells.AutoFitColumns();

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0; // Reset position before returning

            string excelName = $"Contacts.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }



        // GET: Contact/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(c => c.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contact/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            //ViewData["MemberId"] = new SelectList(_context.Members, "ID", "MemberFirstName");
            return View();
        }

        // POST: Contact/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,MiddleName,LastName,Title,Department,Email,Phone,LinkedInUrl,IsVip,MemberId")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDropdowns();
            //ViewData["MemberId"] = new SelectList(_context.Members, "ID", "MemberFirstName", contact.MemberId);
            return View(contact);
        }

        // GET: Contact/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_context.Members, "ID", "MemberFirstName", contact.MemberId);
            return View(contact);
        }

        // POST: Contact/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            var ContactToUpdate = await _context.Contacts.FirstOrDefaultAsync(m => m.Id == id);


            if (ContactToUpdate == null)
            {
                return NotFound();
            }

            // Try update model approach
            if (await TryUpdateModelAsync<Contact>(ContactToUpdate, "",
                c => c.FirstName, c => c.MiddleName, c => c.LastName, c => c.Title, c => c.Department,
                c => c.Email, c => c.Phone, c => c.LinkedInUrl, c => c.IsVip, c => c.MemberId))
            {
                try
                {
                    _context.Update(ContactToUpdate);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(ContactToUpdate.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException dex)
                {
                    string message = dex.GetBaseException().Message;

                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");

                }
            }
            return View(ContactToUpdate);
        }

        // GET: Contact/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(c => c.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }

        private void PopulateDropdownLists()
        {
            var departments = _context.Contacts
                                      .Select(c => c.Department)
                                      .Distinct()
                                      .OrderBy(d => d)
                                      .ToList();
            var titles = _context.Contacts
                .Select(c => c.Title)
                .Distinct()
                .OrderBy(d => d)
                .ToList();
            ViewData["Departments"] = new SelectList(departments);
            ViewData["Titles"] = new SelectList(titles);


        }
        public IActionResult GetContactPreview(int id)
        {
            // Fetch the contact by id, including related industries through ContactIndustries
            var contact = _context.Contacts
                .Include(c => c.Member)
                .Where(c => c.Id == id)  // Filter the contact by id
                .FirstOrDefault();  // Return the first result or null if not found

            // Check if contact was not found
            if (contact == null)
            {
                return NotFound();  // Return 404 if the contact does not exist
            }

            // Return the partial view with the contact data
            return PartialView("_ContactPreview", contact);  // Ensure the partial view name is correct
        }


        private void PopulateDropdowns()
        {
            // Fetch Members for dropdown
            var members = _context.Members.ToList();
            ViewData["Members"] = new SelectList(members, "ID", "MemberName");


        }
    }
}
