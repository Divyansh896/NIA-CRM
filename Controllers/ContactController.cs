using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NIA_CRM.Data;
using NIA_CRM.Models;
using NIA_CRM.Utilities;

namespace NIA_CRM.Controllers
{
    public class ContactController : Controller
    {
        private readonly NIACRMContext _context;

        public ContactController(NIACRMContext context)
        {
            _context = context;
        }

        // GET: Contact
        public async Task<IActionResult> Index(int?page,string? Departments, string? Titles, bool IsVIP, string? SearchString, string? actionButton,
                                               string sortDirection = "asc", string sortField = "Contact Name")
        {
            PopulateDropdownLists();
            string[] sortOptions = new[] { "Contact Name" };  // You can add more sort options if needed

            ViewData["Filtering"] = "btn-outline-secondary";
            int numberFilters = 0;
            var contacts = _context.Contacts.AsQueryable();

            if (Departments != null)
            {
                contacts = contacts.Where(c => c.Department == Departments);
                numberFilters++;
            }
            if (Titles != null)
            {
                contacts = contacts.Where(c => c.Title == Titles);
                numberFilters++;
            }
            if (IsVIP)
            {
                contacts = contacts.Where(c => c.IsVIP);
                numberFilters++;
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

            if (sortField == "Contact Name")
            {
                if (sortDirection == "desc")
                {
                    contacts = contacts
                        .OrderByDescending(p => p.ContactFirstName);
                }
                else
                {
                    contacts = contacts
                        .OrderBy(p => p.ContactFirstName);

                }
            }

            if (!String.IsNullOrEmpty(SearchString))
            {
                contacts = contacts.Where(p => p.ContactLastName.ToUpper().Contains(SearchString.ToUpper())
                                       || p.ContactFirstName.ToUpper().Contains(SearchString.ToUpper()));
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

            // Handle paging
            int pageSize = 5; // Change as needed
            var pagedData = await PaginatedList<Contact>.CreateAsync(contacts, page ?? 1, pageSize);

            return View(pagedData);
        }

        // GET: Contact/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.ID == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contact/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contact/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ContactName,Title,Department,EMail,Phone,LinkedinUrl,IsVIP")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
            return View(contact);
        }

        // POST: Contact/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {

            var ContactToUpdate = await _context.Contacts.FirstOrDefaultAsync(m => m.ID == id);


            if (ContactToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Contact>(ContactToUpdate, "", 
                c => c.ContactFirstName, c=> c.ContactMiddleName, c=> c.ContactLastName, c => c.Title, c => c.Department,
                c => c.EMail, c => c.Phone, c => c.LinkedinUrl, c => c.IsVIP))
            {
                try
                {
                    _context.Update(ContactToUpdate);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(ContactToUpdate.ID))
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
                .FirstOrDefaultAsync(m => m.ID == id);
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
            return _context.Contacts.Any(e => e.ID == id);
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
            var contact = _context.Contacts.Include(c => c.ContactOrganizations)
            .ThenInclude(co => co.Organization).FirstOrDefault(c => c.ID == id);
            if (contact == null)
            {
                return NotFound();
            }
            return PartialView("_ContactPreview", contact);  // Ensure the partial view name is correct
        }

        


    }
}
