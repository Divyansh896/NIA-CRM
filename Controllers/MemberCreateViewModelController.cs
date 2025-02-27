
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
    public class MemberCreateViewModelController : ElephantController
    {
        private readonly NIACRMContext _context;
        private readonly EmailService _emailService;


        public MemberCreateViewModelController(NIACRMContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;

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

                // Initialize list of Contacts (since Contact is now a list)
                Contacts = new List<Contact> { new Contact() },

                // Initialize lists to handle multiple selections
                IndustryNAICSCodes = new List<IndustryNAICSCode>(),
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

                // Save Contacts if they exist (and are properly added)
                if (memberCreateViewModel.Contacts != null && memberCreateViewModel.Contacts.Any())
                {
                    foreach (var contact in memberCreateViewModel.Contacts)
                    {
                        // Check if contact already exists
                        if (contact.Id != -1) // Assuming Id of 0 indicates a new contact
                        {
                            // Optionally, handle the case where a new Contact needs to be added.
                            _context.Contacts.Add(contact);
                            await _context.SaveChangesAsync(); // Save the new Contact
                            var memberContact = new MemberContact
                            {
                                MemberId = memberId,
                                ContactId = contact.Id // New Contact ID
                            };
                            _context.MemberContacts.Add(memberContact);
                        }
                        
                    }

                    await _context.SaveChangesAsync(); // Save all associations
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
            ViewData["Contacts"] = new SelectList(_context.Contacts, "Id", "FullName"); // Adjust based on your Contact model

            // Return view with existing values
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

                // Update for multiple contacts
                Contacts = member.MemberContacts?.Select(mc => mc.Contact).ToList() ?? new List<Contact>(),

                IndustryNAICSCodes = member.IndustryNAICSCodes?.ToList() ?? new List<IndustryNAICSCode>(),
                MemberMembershipTypes = member.MemberMembershipTypes?.ToList() ?? new List<MemberMembershipType>(),
                SelectedNAICSCodeIds = member.IndustryNAICSCodes?.Select(i => i.NAICSCodeId).ToList() ?? new List<int>(),
                SelectedMembershipTypes = member.MemberMembershipTypes?.Select(m => m.MembershipTypeId).ToList() ?? new List<int>(),
                MembershipTypes = _context.MembershipTypes.ToList() ?? new List<MembershipType>(),
                NAICSCodes = _context.NAICSCodes.ToList() ?? new List<NAICSCode>()
            };






            return View(memberEditViewModel);
        }


        [HttpGet]
        public IActionResult SendEmail() { return View(); }
        //Email sender
        [HttpPost]
        public async Task<IActionResult> SendEmail(string recipient, string subject, string message)
        {
            await _emailService.SendEmailAsync(recipient, subject, message);
            ViewBag.Message = "Email Sent Successfully!";
            return View("SendEmail"); // Redirect to home page
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


                    // Assuming the member is being created or updated
                    if (model.Contacts != null && model.Contacts.Any())
                    {
                        foreach (var contact in model.Contacts)
                        {
                            // Check if the contact already exists
                            var existingContact = memberToUpdate.MemberContacts
                                .FirstOrDefault(mc => mc.ContactId == contact.Id);

                            if (existingContact != null)
                            {
                                // Update the existing contact (if necessary)
                                existingContact.Contact = contact; // Update contact details if needed
                            }
                            else
                            {
                                // Add a new member-contact relationship
                                memberToUpdate.MemberContacts.Add(new MemberContact
                                {
                                    MemberId = memberToUpdate.ID,
                                    ContactId = contact.Id
                                });
                            }
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
