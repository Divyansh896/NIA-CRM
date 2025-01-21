using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NIA_CRM.Data;
using NIA_CRM.Models;

namespace NIA_CRM.Controllers
{
    public class InteractionController : Controller
    {
        private readonly NIACRMContext _context;

        public InteractionController(NIACRMContext context)
        {
            _context = context;
        }

        // GET: Interaction
        public async Task<IActionResult> Index()
        {
            // Retrieve all interactions with their related entities (Contact, Member, and Opportunity)
            var interactions = await _context.Interactions
                .Include(i => i.Contact)
                .Include(i => i.Member)
                .Include(i => i.Opportunity)
                .ToListAsync();

            // Pass the interactions list to the view
            return View(interactions);
        }

        // GET: Interaction/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interaction = await _context.Interactions
                .Include(i => i.Contact)
                .Include(i => i.Member)
                .Include(i => i.Opportunity)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (interaction == null)
            {
                return NotFound();
            }

            return View(interaction);
        }

        // GET: Interaction/Create
        public IActionResult Create()
        {
            ViewData["ContactID"] = new SelectList(_context.Contacts, "ID", "ContactName");
            ViewData["MemberID"] = new SelectList(_context.Members, "ID", "MemberName");
            ViewData["OpportunityID"] = new SelectList(_context.Opportunities, "ID", "OpportunityName");
            return View();
        }

        // POST: Interaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,InteractionDate,InteractionNote,ContactID,MemberID,OpportunityID")] Interaction interaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(interaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContactID"] = new SelectList(_context.Contacts, "ID", "ContactName", interaction.ContactID);
            ViewData["MemberID"] = new SelectList(_context.Members, "ID", "MemberName", interaction.MemberID);
            ViewData["OpportunityID"] = new SelectList(_context.Opportunities, "ID", "OpportunityName", interaction.OpportunityID);
            return View(interaction);
        }

        // GET: Interaction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interaction = await _context.Interactions.FindAsync(id);
            if (interaction == null)
            {
                return NotFound();
            }
            ViewData["ContactID"] = new SelectList(_context.Contacts, "ID", "ContactName", interaction.ContactID);
            ViewData["MemberID"] = new SelectList(_context.Members, "ID", "MemberName", interaction.MemberID);
            ViewData["OpportunityID"] = new SelectList(_context.Opportunities, "ID", "OpportunityName", interaction.OpportunityID);
            return View(interaction);
        }

        // POST: Interaction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,InteractionDate,InteractionNote,ContactID,MemberID,OpportunityID")] Interaction interaction)
        {
            if (id != interaction.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(interaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InteractionExists(interaction.ID))
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
            ViewData["ContactID"] = new SelectList(_context.Contacts, "ID", "ContactName", interaction.ContactID);
            ViewData["MemberID"] = new SelectList(_context.Members, "ID", "MemberName", interaction.MemberID);
            ViewData["OpportunityID"] = new SelectList(_context.Opportunities, "ID", "OpportunityName", interaction.OpportunityID);
            return View(interaction);
        }

        // GET: Interaction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interaction = await _context.Interactions
                .Include(i => i.Contact)
                .Include(i => i.Member)
                .Include(i => i.Opportunity)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (interaction == null)
            {
                return NotFound();
            }

            return View(interaction);
        }

        // POST: Interaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var interaction = await _context.Interactions.FindAsync(id);
            if (interaction != null)
            {
                _context.Interactions.Remove(interaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InteractionExists(int id)
        {
            return _context.Interactions.Any(e => e.ID == id);
        }


        public async Task<IActionResult> Notes()
        {
            var notes = await _context.Interactions
                .Include(i => i.Contact)  // Include related Contact
                .Include(i => i.Member)    // Include related Member
                .Select(i => new
                {
                    i.ID,  // Add the ID
                    i.InteractionNote,
                    i.Contact.ContactName,
                    i.Member.MemberName
                })
                .ToListAsync();

            return View(notes);  // Passing the anonymous type
        }




    }
}
