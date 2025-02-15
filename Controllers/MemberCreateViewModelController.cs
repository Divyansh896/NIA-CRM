
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NIA_CRM.Data;
using NIA_CRM.Models;
using NIA_CRM.ViewModels;

namespace NIA_CRM.Controllers
{
    public class MemberCreateViewModelController : Controller
    {
        private readonly NIACRMContext _context;

        public MemberCreateViewModelController(NIACRMContext context)
        {
            _context = context;
        }

        // GET: MemberCreateViewModel
        public async Task<IActionResult> Index()
        {
            var members = await _context.Members
                .Include(m => m.MemberThumbnail)
                .Include(m => m.Addresses)
                .Include(m => m.MemberMembershipTypes).ThenInclude(m => m.MembershipType)
                .Include(m => m.MemberNotes)
                .Include(m => m.Contacts)
                .Include(m => m.IndustryNAICSCodes).ThenInclude(m => m.NAICSCode)
                .ToListAsync(); // Use await here

            return View(members);
        }




        // GET: MemberCreateViewModel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                                        .Include(m => m.IndustryNAICSCodes)
                                        .FirstOrDefaultAsync(m => m.ID == id);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: MemberCreateViewModel/Create
        public IActionResult Create()
        {
            var memberCreateViewModel = new MemberCreateViewModel
            {
                // Initialize single objects
                Member = new Member(),
                MemberNote = new MemberNote(),  // Single MemberNote object
                Address = new Address(),  // Single Address object
                Contact = new Contact(),  // Single Contact object
                ContactNote = new ContactNote(),  // Single Contact Note object
                NAICSCode = new NAICSCode(),  // Single NAICSCode object
                IndustryNAICSCode = new IndustryNAICSCode(),
                //MembershipType = new MembershipType(),
                MemberMembershipType = new MemberMembershipType()  // Single MemberMembershipType object

            };

            // Optionally populate the MembershipTypes dropdown if needed
            var membershipTypes = _context.MembershipTypes.ToList();
            ViewData["MembershipType"] = new SelectList(membershipTypes, "ID", "TypeName");

            var NAICSCode = _context.NAICSCodes.ToList();
            ViewData["NAICSCode"] = new SelectList(NAICSCode, "Id", "Code");

            return View(memberCreateViewModel);
        }



        // POST: MemberCreateViewModel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberCreateViewModel memberCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                // Ensure the Member entity is saved first
                _context.Members.Add(memberCreateViewModel.Member);
                await _context.SaveChangesAsync(); // Get Member ID after save

                int memberId = memberCreateViewModel.Member.ID;

                // Save MemberNote if it exists
                if (memberCreateViewModel.MemberNote != null)
                {
                    memberCreateViewModel.MemberNote.MemberId = memberId;
                    _context.MemberNotes.Add(memberCreateViewModel.MemberNote);
                }

                // Save Address if it exists
                if (memberCreateViewModel.Address != null)
                {
                    memberCreateViewModel.Address.MemberId = memberId;
                    _context.Addresses.Add(memberCreateViewModel.Address);
                }

                // Save Contact if it exists
                if (memberCreateViewModel.Contact != null)
                {
                    memberCreateViewModel.Contact.MemberId = memberId;
                    _context.Contacts.Add(memberCreateViewModel.Contact);
                    await _context.SaveChangesAsync(); // Ensure Contact ID is available
                }

                // Save Contact Note (only if Contact was created)
                if (memberCreateViewModel.ContactNote != null && memberCreateViewModel.Contact != null)
                {
                    memberCreateViewModel.ContactNote.ContactId = memberCreateViewModel.Contact.Id;
                    _context.ContactNotes.Add(memberCreateViewModel.ContactNote);
                }

                if (memberCreateViewModel.NAICSCode != null)
                {
                    var existingNAICSCode = await _context.NAICSCodes
                        .FirstOrDefaultAsync(n => n.Id == memberCreateViewModel.NAICSCode.Id);

                    if (existingNAICSCode != null && memberCreateViewModel.IndustryNAICSCode != null)
                    {
                        memberCreateViewModel.IndustryNAICSCode.MemberId = memberCreateViewModel.Member.ID;
                        memberCreateViewModel.IndustryNAICSCode.NAICSCodeId = existingNAICSCode.Id;
                        _context.IndustryNAICSCodes.Add(memberCreateViewModel.IndustryNAICSCode);
                    }
                }

                if (memberCreateViewModel.MemberMembershipType.MembershipTypeId > 0)
                {
                    // Retrieve the existing MembershipType based on the MembershipTypeId
                    var existingMembershipType = await _context.MembershipTypes
                        .FirstOrDefaultAsync(n => n.ID == memberCreateViewModel.MemberMembershipType.MembershipTypeId);

                    if (existingMembershipType != null && memberCreateViewModel.MemberMembershipType != null)
                    {
                        // Set the MemberId for the MemberMembershipType
                        memberCreateViewModel.MemberMembershipType.MemberId = memberCreateViewModel.Member.ID;
                        memberCreateViewModel.MemberMembershipType.MembershipTypeId = existingMembershipType.ID;
                        _context.MemberMembershipTypes.Add(memberCreateViewModel.MemberMembershipType);
                    }
                }



                // Save all related entities
                await _context.SaveChangesAsync();

                // Redirect to Index
                return RedirectToAction("Details", "Member", new { id = memberCreateViewModel.Member.ID });
            }

            // Re-populate dropdowns before returning the view
            ViewData["MembershipType"] = new SelectList(_context.MembershipTypes, "ID", "TypeName");
            ViewData["NAICSCode"] = new SelectList(_context.NAICSCodes, "Id", "Code");

            return View(memberCreateViewModel);
        }

        // Get: Edit
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
            return View(member);
        }

        // POST: MemberCreateViewModel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MemberCreateViewModel memberCreateViewModel)
        {
            if (id != memberCreateViewModel.Member.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var member = await _context.Members.FindAsync(id);
                    if (member == null)
                    {
                        return NotFound();
                    }

                    // Update Member fields
                    member.MemberName = memberCreateViewModel.Member.MemberName;
                    member.MemberSize = memberCreateViewModel.Member.MemberSize;
                    // Add other fields as necessary...

                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(id))
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

            return View(memberCreateViewModel);
        }


        // GET: MemberCreateViewModel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
    .FirstOrDefaultAsync(m => m.ID == id);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: MemberCreateViewModel/Delete/5
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

    }
}
