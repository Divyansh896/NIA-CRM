
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NIA_CRM.CustomControllers;
using NIA_CRM.Data;
using NIA_CRM.Models;
using NIA_CRM.ViewModels;

namespace NIA_CRM.Controllers
{
    public class MemberCreateViewModelController : ElephantController
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
                .Include(m => m.MemberContacts).ThenInclude(m => m.Contact)
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

        // GET: Create
        public IActionResult Create()
        {
            var memberCreateViewModel = new MemberCreateViewModel
            {
                // Initialize single objects
                Member = new Member(),
                Address = new Address(),
                Contact = new Contact(),

                // Initialize lists to handle multiple selections
                IndustryNAICSCode = new List<IndustryNAICSCode>(),
                MemberMembershipTypes = new List<MemberMembershipType>(),
                MembershipTypes = _context.MembershipTypes.ToList(), // Available membership types
                NAICSCodes = _context.NAICSCodes.ToList()  // Available NAICS codes
            };

            // Optionally, initialize available NAICS codes for the dropdown
            memberCreateViewModel.AvailableNAICSCodes = _context.NAICSCodes.ToList();

            return View(memberCreateViewModel);
        }




        // POST: MemberCreateViewModel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberCreateViewModel memberCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                // Save Member first
                _context.Members.Add(memberCreateViewModel.Member);
                await _context.SaveChangesAsync(); // Get Member ID after save

                int memberId = memberCreateViewModel.Member.ID;

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

                // Save multiple Industry NAICS Codes
                if (memberCreateViewModel.SelectedNAICSCodeIds != null && memberCreateViewModel.SelectedNAICSCodeIds.Any())
                {
                    foreach (var naicsCodeId in memberCreateViewModel.SelectedNAICSCodeIds)
                    {
                        var existingNAICSCode = await _context.NAICSCodes
                            .FirstOrDefaultAsync(n => n.Id == naicsCodeId);

                        if (existingNAICSCode != null)
                        {
                            var industryNAICSCode = new IndustryNAICSCode
                            {
                                MemberId = memberId,
                                NAICSCodeId = existingNAICSCode.Id
                            };

                            _context.IndustryNAICSCodes.Add(industryNAICSCode);
                        }
                    }
                }


                // Handle multiple membership types
                if (memberCreateViewModel.SelectedMembershipTypes != null && memberCreateViewModel.SelectedMembershipTypes.Any())
                {
                    foreach (var membershipTypeId in memberCreateViewModel.SelectedMembershipTypes)
                    {
                        var existingMembershipType = await _context.MembershipTypes
                            .FirstOrDefaultAsync(n => n.ID == membershipTypeId);

                        if (existingMembershipType != null)
                        {
                            var memberMembershipType = new MemberMembershipType
                            {
                                MemberId = memberId,
                                MembershipTypeId = existingMembershipType.ID
                            };

                            _context.MemberMembershipTypes.Add(memberMembershipType);
                        }
                    }
                }

                // Save all related entities
                await _context.SaveChangesAsync();

                // Redirect to the Details view
                return RedirectToAction("Details", "Member", new { id = memberCreateViewModel.Member.ID });
            }

            // Re-populate the dropdowns in case the form is invalid
            ViewData["MembershipTypes"] = new SelectList(_context.MembershipTypes, "ID", "TypeName");
            ViewData["NAICSCodes"] = new SelectList(_context.NAICSCodes, "Id", "Code");

            return View(memberCreateViewModel);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                                        .Include(m => m.MemberThumbnail)
                                        .Include(m => m.Addresses)
                                        .Include(m => m.MemberMembershipTypes).ThenInclude(m => m.MembershipType)
                                        .Include(m => m.MemberContacts).ThenInclude(m => m.Contact)
                                        .Include(m => m.IndustryNAICSCodes).ThenInclude(m => m.NAICSCode)
                                        .FirstOrDefaultAsync(m => m.ID == id);

            if (member == null)
            {
                return NotFound();
            }

            var memberEditViewModel = new MemberCreateViewModel
            {
                Member = member,
                Address = member.Addresses.FirstOrDefault(),
                Contact = member.Contacts.FirstOrDefault(),
                IndustryNAICSCode = member.IndustryNAICSCodes?.ToList() ?? new List<IndustryNAICSCode>(),
                MemberMembershipTypes = member.MemberMembershipTypes?.ToList() ?? new List<MemberMembershipType>(),
                SelectedNAICSCodeIds = member.IndustryNAICSCodes?.Select(i => i.NAICSCodeId).ToList() ?? new List<int>(),
                SelectedMembershipTypes = member.MemberMembershipTypes?.Select(m => m.MembershipTypeId).ToList() ?? new List<int>(),
                MembershipTypes = _context.MembershipTypes.ToList() ?? new List<MembershipType>(),
                NAICSCodes = _context.NAICSCodes.ToList() ?? new List<NAICSCode>()
            };




            return View(memberEditViewModel);
        }





        // POST: MemberCreateViewModel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MemberCreateViewModel model)
        {
            

            var memberToUpdate = await _context.Members
                .Include(m => m.MemberThumbnail)
                .Include(m => m.Addresses)
                .Include(m => m.MemberMembershipTypes).ThenInclude(m => m.MembershipType)
                .Include(m => m.MemberContacts).ThenInclude(m => m.Contact)
                .Include(m => m.IndustryNAICSCodes).ThenInclude(m => m.NAICSCode)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (memberToUpdate == null)
            {
                return NotFound();
            }

            try
            {
                // Update the Member properties
                if (await TryUpdateModelAsync<Member>(memberToUpdate, "",
                    m => m.MemberName,
                    m => m.MemberSize,
                    m => m.WebsiteUrl,
                    m => m.JoinDate,
                    m => m.IsVIP,
                    m => m.MemberLogo
                ))
                {
                    // Handle Membership Types (Many-to-Many)
                    var currentMembershipTypes = memberToUpdate.MemberMembershipTypes.Select(m => m.MembershipTypeId).ToList();
                    var selectedMembershipTypes = model.SelectedMembershipTypes;

                    // Remove the old associations
                    var toRemoveMembershipTypes = memberToUpdate.MemberMembershipTypes.Where(m => !selectedMembershipTypes.Contains(m.MembershipTypeId)).ToList();
                    foreach (var item in toRemoveMembershipTypes)
                    {
                        memberToUpdate.MemberMembershipTypes.Remove(item);
                    }

                    // Add new associations
                    foreach (var membershipTypeId in selectedMembershipTypes)
                    {
                        if (!currentMembershipTypes.Contains(membershipTypeId))
                        {
                            memberToUpdate.MemberMembershipTypes.Add(new MemberMembershipType { MembershipTypeId = membershipTypeId, MemberId = memberToUpdate.ID });
                        }
                    }

                    // Handle NAICS Codes (Many-to-Many)
                    var currentNAICSCodes = memberToUpdate.IndustryNAICSCodes.Select(n => n.NAICSCodeId).ToList();
                    var selectedNAICSCodeIds = model.SelectedNAICSCodeIds;

                    // Remove the old associations
                    var toRemoveNAICSCode = memberToUpdate.IndustryNAICSCodes.Where(n => !selectedNAICSCodeIds.Contains(n.NAICSCodeId)).ToList();
                    foreach (var item in toRemoveNAICSCode)
                    {
                        memberToUpdate.IndustryNAICSCodes.Remove(item);
                    }

                    // Add new associations
                    foreach (var naicsCodeId in selectedNAICSCodeIds)
                    {
                        if (!currentNAICSCodes.Contains(naicsCodeId))
                        {
                            memberToUpdate.IndustryNAICSCodes.Add(new IndustryNAICSCode { NAICSCodeId = naicsCodeId, MemberId = memberToUpdate.ID });
                        }
                    }

                    // Handle single Address
                    if (model.Address != null)
                    {
                        var existingAddress = memberToUpdate.Addresses.FirstOrDefault(a => a.Id == model.Address.Id);
                        if (existingAddress != null)
                        {
                            // Update the existing address with the new values from model
                            existingAddress.AddressLine1 = model.Address.AddressLine1;
                            existingAddress.AddressLine2 = model.Address.AddressLine2;
                            existingAddress.City = model.Address.City;
                            existingAddress.StateProvince = model.Address.StateProvince;
                            existingAddress.PostalCode = model.Address.PostalCode;
                            _context.Entry(existingAddress).CurrentValues.SetValues(model.Address); // This updates the existing contact with new values.

                        }
                        else
                        {
                            // Add new address if none exists
                            memberToUpdate.Addresses.Add(model.Address);
                        }
                    }


                    // Handle single Contact
                    if (model.Contact != null)
                    {
                        var existingContact = memberToUpdate.Contacts.FirstOrDefault(c => c.Id == model.Contact.Id);
                        if (existingContact != null)
                        {
                            // Update the existing contact
                            existingContact.FirstName = model.Contact.FirstName;
                            existingContact.MiddleName = model.Contact.MiddleName;
                            existingContact.LastName = model.Contact.LastName;
                            existingContact.Title = model.Contact.Title;
                            existingContact.Department = model.Contact.Department;
                            existingContact.Email = model.Contact.Email;
                            existingContact.Phone = model.Contact.Phone;
                            existingContact.LinkedInUrl = model.Contact.LinkedInUrl;
                            existingContact.IsVip = model.Contact.IsVip;

                            // You can set additional properties if needed, just like you did for the address.
                            _context.Entry(existingContact).CurrentValues.SetValues(model.Contact); // This updates the existing contact with new values.
                        }
                        else
                        {
                            // Add new contact if none exists
                            model.Contact.MemberId = memberToUpdate.ID; // Ensure the contact is associated with the member
                            memberToUpdate.Contacts.Add(model.Contact);
                        }
                    }


                    // Save the changes to the database
                    _context.Update(memberToUpdate);
                    await _context.SaveChangesAsync();

                    // Redirect to the Member Index page
                    return RedirectToAction("Index", "Member");
                }
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
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

           

            return View(model);
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
