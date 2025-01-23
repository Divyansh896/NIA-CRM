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
using NIA_CRM.ViewModels;

namespace NIA_CRM.Controllers
{
    public class MemberController : ElephantController
    {
        private readonly NIACRMContext _context;

        public MemberController(NIACRMContext context)
        {
            _context = context;
        }

        // GET: Member
        public async Task<IActionResult> Index(int?page, int? pageSizeID, string? SearchString, string? JoinDate, string? Organization)
        {
            var members = _context.Members
                .Include(m => m.Organization)
                .AsQueryable();

            // Filter by Member Name
            if (!string.IsNullOrEmpty(SearchString))
            {
                page = 1;//Reset page to start
                members = members.Where(m => m.Summary.Contains(SearchString));
            }

            // Filter by Join Date
            if (!string.IsNullOrEmpty(JoinDate) && DateTime.TryParse(JoinDate, out var parsedDate))
            {
                
                members = members.Where(m => m.JoinDate.HasValue && m.JoinDate.Value.Date == parsedDate.Date);
            }

            // Filter by Organization Name
            if (!string.IsNullOrEmpty(Organization))
            {
                members = members.Where(m => m.Organization.OrganizationName.Contains(Organization));
            }

            // Save filter values to ViewData for persistence
            ViewData["SearchString"] = SearchString;
            ViewData["JoinDate"] = JoinDate;
            ViewData["Organization"] = Organization;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Member>.CreateAsync(members.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);
        }


        // GET: Member/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.Organization)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Member/Create
        public IActionResult Create()
        {
            ViewData["OrganizationID"] = new SelectList(_context.Organizations, "ID", "OrganizationName");
            return View();
        }

        // POST: Member/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,MemberName,JoinDate,StandingStatus,OrganizationID")] Member member)
        {
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrganizationID"] = new SelectList(_context.Organizations, "ID", "OrganizationName", member.OrganizationID);
            return View(member);
        }

        // GET: Member/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            ViewData["OrganizationID"] = new SelectList(_context.Organizations, "ID", "OrganizationName", member.OrganizationID);
            return View(member);
        }

        // POST: Member/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {

            var memberToUpdate = await _context.Members.FirstOrDefaultAsync(m => m.ID == id);

            if (memberToUpdate == null)
            {
                return NotFound();
            }
            
            // Try update model approach


            if (await TryUpdateModelAsync<Member>(memberToUpdate, "", m => m.MemberFirstName, m => m.JoinDate, m => m.StandingStatus, m => m.OrganizationID))
         {
                try
                {
                    _context.Update(memberToUpdate);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(memberToUpdate.ID))
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

            ViewData["OrganizationID"] = new SelectList(_context.Organizations, "ID", "OrganizationName", memberToUpdate.OrganizationID);
            return View(memberToUpdate);
        }

        // GET: Member/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.Organization)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.ID == id);
        }

        public async Task<IActionResult> GetMemberPreview(int id)
        {
            var member = await _context.Members
                .Include(m => m.Organization)
                .Include(m => m.Address)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (member == null)
            {
                return NotFound();
            }

            return PartialView("_MemberContactPreview", member);
        }


        

    }
}
