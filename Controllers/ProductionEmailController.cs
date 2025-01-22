using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NIA_CRM.Data;
using NIA_CRM.Models;

namespace NIA_CRM.Controllers
{
    public class ProductionEmailController(NIACRMContext context) : Controller
    {
        private readonly NIACRMContext _context = context;

        // GET: ProductionEmail
        public async Task<IActionResult> Index(int? EmailTypeID, string? actionButton,
      string sortDirection = "asc", string sortField = "Email Type")
        {
            // Populate the dropdown list (assuming ProductionEmailTypeSelectList is a method that returns a SelectList)
            ViewData["EmailTypeID"] = ProductionEmailTypeSelectList(null);

            string[] sortOptions = new[] { "Email Type" };  // You can add more sort options if needed

            // Declare the email list to be used in the view
            var emailsQuery = _context.ProductionEmails.AsQueryable();  // Use IQueryable for chaining queries

            // Filter by EmailTypeID if it's provided
            if (EmailTypeID.HasValue)
            {
                emailsQuery = emailsQuery.Where(e => e.Id == EmailTypeID.Value);
            }

            // Check if the form was submitted with a sorting request
            if (!string.IsNullOrEmpty(actionButton)) // Form Submitted!
            {
                if (sortOptions.Contains(actionButton)) // Change of sort is requested
                {
                    if (actionButton == sortField) // Reverse order on same field
                    {
                        sortDirection = sortDirection == "asc" ? "desc" : "asc";
                    }
                    sortField = actionButton; // Sort by the button clicked
                }
            }

            // Now we know which field and direction to sort by
            //Now we know which field and direction to sort by
            if (sortField == "Email Type")
            {
                if (sortDirection == "asc")
                {
                    emailsQuery = emailsQuery
                        .OrderByDescending(p => p.EmailType);
                }
                else
                {
                    emailsQuery = emailsQuery
                        .OrderBy(p => p.EmailType);

                }
            }


            // Execute the query and get the sorted result
            var emails = await emailsQuery.ToListAsync();
            // Pass sorting information to the view
            ViewData["SortDirection"] = sortDirection;
            ViewData["SortField"] = sortField;
            // Pass the emails to the view
            return View(emails);
        }

        // GET: ProductionEmail/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productionEmail = await _context.ProductionEmails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productionEmail == null)
            {
                return NotFound();
            }

            return View(productionEmail);
        }

        // GET: ProductionEmail/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductionEmail/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmailType,Subject,Body")] ProductionEmail productionEmail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(productionEmail);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException dex)
            {
                string message = dex.GetBaseException().Message;
                if (message.Contains("UNIQUE") && message.Contains("ProductionEmails.EmailType"))
                {
                    ModelState.AddModelError("EmailType", "Unable to save changes. Remember, " +
                        "you cannot have duplicate EmailType.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }

            return View(productionEmail);
        }

        // GET: ProductionEmail/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productionEmail = await _context.ProductionEmails.FindAsync(id);
            if (productionEmail == null)
            {
                return NotFound();
            }
            return View(productionEmail);
        }

        // POST: ProductionEmail/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            var EmailToUpdate = await _context.ProductionEmails.FirstOrDefaultAsync(e => e.Id == id);
            if (EmailToUpdate == null)
            {
                return NotFound();
            }


            if (await TryUpdateModelAsync<ProductionEmail>(EmailToUpdate, "", e => e.EmailType, e => e.Subject, e => e.Body))
            {
                try
                {
                    _context.Update(EmailToUpdate);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductionEmailExists(EmailToUpdate.Id))
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
                    if (message.Contains("UNIQUE") && message.Contains("ProductionEmails.EmailType"))
                    {
                        ModelState.AddModelError("EmailType", "Unable to save changes. Remember, " +
                            "you cannot have duplicate EmailType.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    }

                }
            }
            return View(EmailToUpdate);
        }

        // GET: ProductionEmail/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productionEmail = await _context.ProductionEmails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productionEmail == null)
            {
                return NotFound();
            }

            return View(productionEmail);
        }

        // POST: ProductionEmail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productionEmail = await _context.ProductionEmails.FindAsync(id);
            if (productionEmail != null)
            {
                _context.ProductionEmails.Remove(productionEmail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductionEmailExists(int id)
        {
            return _context.ProductionEmails.Any(e => e.Id == id);
        }

        private SelectList ProductionEmailTypeSelectList(int? selectedId)
        {
            // Query to fetch the email types ordered alphabetically
            var qry = _context.ProductionEmails
                               .OrderBy(e => e.EmailType)
                               .Select(e => new { e.Id, e.EmailType })
                               .AsNoTracking();

            // Return SelectList, passing the selectedId to indicate the pre-selected value
            return new SelectList(qry, "Id", "EmailType", selectedId);
        }

    }
}