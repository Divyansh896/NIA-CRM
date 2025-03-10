using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NIA_CRM.CustomControllers;
using NIA_CRM.Data;
using NIA_CRM.Models;
using NIA_CRM.Utilities;
using OfficeOpenXml.Style;
using OfficeOpenXml;
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
        public async Task<IActionResult> Index(string? SearchString, string? JoinDate, int? page, int? pageSizeID, string? actionButton, int? MembershipTypes, string sortDirection = "asc", string sortField = "Member Name")

        {

            PopulateDropdowns();
            string[] sortOptions = { "Member Name", "City", "Membership Type", "Sector", "NAICS Code", "Contacts" };
            int numberFilters = 0;

            var members = _context.Members
                                    .Include(m => m.MemberThumbnail)
                                    .Include(m => m.Addresses)
                                    .Include(m => m.MemberMembershipTypes).ThenInclude(m => m.MembershipType)
                                    .Include(m => m.MemberContacts).ThenInclude(m => m.Contact)
                                    .Include(m => m.IndustryNAICSCodes).ThenInclude(m => m.NAICSCode)
                                    .Where(m => !m.Cancellations.Any())  // Exclude members with cancellations
                                    .AsNoTracking();



            if (!string.IsNullOrEmpty(actionButton) && actionButton == "ExportExcel")
            {
                var exportData = await members.AsNoTracking().ToListAsync();
                return ExportMembersToExcel(exportData);
            }


            if (!String.IsNullOrEmpty(actionButton)) // Form Submitted!
    {
        page = 1; // Reset page to start

        if (sortOptions.Contains(actionButton)) // Change of sort is requested
        {
            if (actionButton == sortField) // Reverse order on same field
            {
                sortDirection = sortDirection == "asc" ? "desc" : "asc";
            }
            sortField = actionButton; // Sort by the button clicked
        }
    }

            members = sortField switch
            {
                "Member Name" => sortDirection == "asc"
                    ? members.OrderBy(e => e.MemberName)
                    : members.OrderByDescending(e => e.MemberName),

                "City" => sortDirection == "asc"
                    ? members.OrderBy(e => e.Addresses.FirstOrDefault().City) // Assuming Address has City
                    : members.OrderByDescending(e => e.Addresses.FirstOrDefault().City),

                "Membership Type" => sortDirection == "asc"
                    ? members.OrderBy(e => e.MemberMembershipTypes.FirstOrDefault().MembershipType.TypeName) // Assuming the MembershipType has a Name
                    : members.OrderByDescending(e => e.MemberMembershipTypes.FirstOrDefault().MembershipType.TypeName),

                "Sector" => sortDirection == "asc"
                    ? members.OrderBy(e => e.MemberSectors.FirstOrDefault().Sector.SectorName) // Assuming NAICSCode has Sector
                    : members.OrderByDescending(e => e.MemberSectors.FirstOrDefault().Sector.SectorName),

                "NAICS Code" => sortDirection == "asc"
                    ? members.OrderBy(e => e.IndustryNAICSCodes.FirstOrDefault().NAICSCode.Code) // Assuming NAICSCode has Code
                    : members.OrderByDescending(e => e.IndustryNAICSCodes.FirstOrDefault().NAICSCode.Code),

                "Contacts" => sortDirection == "asc"
                    ? members.OrderBy(e => e.MemberContacts.FirstOrDefault().Contact.Summary) // Assuming Contact has Name
                    : members.OrderByDescending(e => e.MemberContacts.FirstOrDefault().Contact.Summary),

                _ => members
            };


            if (!string.IsNullOrEmpty(SearchString))
            {
                members = members.Where(m =>
                    m.MemberName.ToUpper().Contains(SearchString.ToUpper()));
                numberFilters++;
                ViewData["SearchString"] = SearchString;
            }

            if (!string.IsNullOrEmpty(JoinDate))
            {
                if (DateTime.TryParse(JoinDate, out var parsedDate))
                {
                    members = members.Where(m => m.JoinDate == parsedDate);
                    ViewData["JoinDate"] = JoinDate;
                }
                else
                {
                    ModelState.AddModelError("JoinDate", "Invalid date format. Please use YYYY-MM-DD.");
                }
            }
            if (MembershipTypes.HasValue)
            {
                // Retrieve the MembershipType object from the database using the ID
                var membershipType = _context.MembershipTypes
                                             .FirstOrDefault(m => m.Id == MembershipTypes.Value);

                if (membershipType != null)
                {
                    // Filter members based on MembershipTypeId (MemberMembershipType)
                    members = members
                        .Where(p => p.MemberMembershipTypes
                            .Any(mmt => mmt.MembershipTypeId == MembershipTypes.Value));

                    numberFilters++;

                    // Store the name of the selected membership type in ViewData
                    ViewData["MembershipTypesFilter"] = membershipType.TypeName;
                }
            }


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
            ViewData["records"] = $"Records Found: {members.Count()}";

            // Handle paging
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Member>.CreateAsync(members, page ?? 1, pageSize);

            return View(pagedData);
        }



        //private IActionResult ExportMembersToExcel(List<Member> members)
        //{
        //    var package = new ExcelPackage(); // No 'using' block to avoid disposal
        //    var worksheet = package.Workbook.Worksheets.Add("Members");

        //    // Adding headers
        //    worksheet.Cells[1, 1].Value = "Member ID";
        //    worksheet.Cells[1, 2].Value = "Member Name";
        //    worksheet.Cells[1, 3].Value = "City";
        //    worksheet.Cells[1, 4].Value = "Join Date";
        //    worksheet.Cells[1, 5].Value = "Membership Type";
        //    worksheet.Cells[1, 6].Value = "Address";
        //    worksheet.Cells[1, 7].Value = "Contact";
        //    worksheet.Cells[1, 8].Value = "VIP Status";

        //    // Populating data
        //    int row = 2;
        //    foreach (var member in members)
        //    {
        //        worksheet.Cells[row, 1].Value = member.ID;
        //        worksheet.Cells[row, 2].Value = member.MemberName;
        //        worksheet.Cells[row, 3].Value = member.Addresses.FirstOrDefault()?.City ?? "N/A";
        //        worksheet.Cells[row, 4].Value = member.JoinDate.ToString("yyyy-MM-dd") ?? "N/A"; // Format date
        //        worksheet.Cells[row, 5].Value = member.MemberMembershipTypes.FirstOrDefault()?.MembershipType?.TypeName ?? "N/A";

        //        worksheet.Cells[row, 6].Value = member.Addresses.FirstOrDefault() != null
        //            ? $"{member.Addresses.FirstOrDefault().AddressLine2}, {member.Addresses.FirstOrDefault().City}, {member.Addresses.FirstOrDefault().StateProvince}, {member.Addresses.FirstOrDefault().PostalCode}"
        //            : "No Address Available";

        //        worksheet.Cells[row, 7].Value = member.MemberContacts.FirstOrDefault() != null
        //            ? $"{member.MemberContacts.FirstOrDefault().Contact.Phone} | {member.MemberContacts.FirstOrDefault().Contact.Email}"
        //            : "No Contacts Available";

        //        row++;
        //    }

        //    // Auto-fit columns for better readability
        //    worksheet.Cells.AutoFitColumns();

        //    var stream = new MemoryStream();
        //    package.SaveAs(stream);
        //    stream.Position = 0; // Reset position before returning

        //    string excelName = $"Members.xlsx";

        //    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        //}

        private IActionResult ExportMembersToExcel(List<Member> members)
        {
            var package = new ExcelPackage(); // No 'using' block to avoid disposal
            var worksheet = package.Workbook.Worksheets.Add("Members");

            // Adding headers
            worksheet.Cells[1, 1].Value = "Member ID";
            worksheet.Cells[1, 2].Value = "Member Name";
            worksheet.Cells[1, 3].Value = "City";
            worksheet.Cells[1, 4].Value = "Join Date";
            worksheet.Cells[1, 5].Value = "Membership Type";
            worksheet.Cells[1, 6].Value = "Address Line 1";
            worksheet.Cells[1, 7].Value = "Address Line 2";
            worksheet.Cells[1, 8].Value = "City";
            worksheet.Cells[1, 9].Value = "State/Province";
            worksheet.Cells[1, 10].Value = "Postal Code";
            worksheet.Cells[1, 11].Value = "Phone Number";
            worksheet.Cells[1, 12].Value = "Email Address";
            //worksheet.Cells[1, 13].Value = "VIP Status";

            // Populating data
            int row = 2;
            foreach (var member in members)
            {
                worksheet.Cells[row, 1].Value = member.ID;
                worksheet.Cells[row, 2].Value = member.MemberName;
                worksheet.Cells[row, 3].Value = member.Addresses.FirstOrDefault()?.City ?? "N/A";
                worksheet.Cells[row, 4].Value = member.JoinDate.ToString("yyyy-MM-dd") ?? "N/A"; // Format date
                worksheet.Cells[row, 5].Value = member.MemberMembershipTypes.FirstOrDefault()?.MembershipType?.TypeName ?? "N/A";

                // Separating address components
                var address = member.Addresses.FirstOrDefault();
                if (address != null)
                {
                    worksheet.Cells[row, 6].Value = address.AddressLine1;
                    worksheet.Cells[row, 7].Value = address.AddressLine2;
                    worksheet.Cells[row, 8].Value = address.City;
                    worksheet.Cells[row, 9].Value = address.StateProvince;
                    worksheet.Cells[row, 10].Value = address.PostalCode;
                }
                else
                {
                    worksheet.Cells[row, 6].Value = "No Address Available";
                    worksheet.Cells[row, 7].Value = "N/A";
                    worksheet.Cells[row, 8].Value = "N/A";
                    worksheet.Cells[row, 9].Value = "N/A";
                    worksheet.Cells[row, 10].Value = "N/A";
                }

                // Separating contact information
                var contact = member.MemberContacts.FirstOrDefault();
                if (contact != null)
                {
                    worksheet.Cells[row, 11].Value = contact.Contact.Phone;
                    worksheet.Cells[row, 12].Value = contact.Contact.Email;
                }
                else
                {
                    worksheet.Cells[row, 11].Value = "No Contact Available";
                    worksheet.Cells[row, 12].Value = "N/A";
                }

                //worksheet.Cells[row, 13].Value = member.IsVip ? "Yes" : "No";

                row++;
            }

            // Auto-fit columns for better readability
            worksheet.Cells.AutoFitColumns();

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0; // Reset position before returning

            string excelName = $"Members.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }


        public IActionResult ImportMembersFromExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please upload a valid Excel file.";
                return RedirectToAction("Index");
            }

            try
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                        if (worksheet == null || worksheet.Dimension == null)
                        {
                            TempData["Error"] = "The Excel file is empty or not formatted correctly.";
                            return RedirectToAction("Index");
                        }

                        int rowCount = worksheet.Dimension.Rows;
                        int colCount = worksheet.Dimension.Columns;

                        // Ensure the required columns exist (minimum 7 columns based on the import logic)
                        if (colCount < 7)
                        {
                            TempData["Error"] = "The Excel file is missing required columns.";
                            return RedirectToAction("Index");
                        }

                        List<Member> members = new List<Member>();

                        for (int row = 2; row <= rowCount; row++) // Start from row 2 to skip headers
                        {
                            if (worksheet.Cells[row, 1].Value == null) // Skip empty rows
                                continue;

                            var member = new Member
                            {
                                ID = int.TryParse(worksheet.Cells[row, 1].Value?.ToString(), out var id) ? id : 0,
                                MemberName = worksheet.Cells[row, 2].Value?.ToString()?.Trim() ?? "Unknown",

                                JoinDate = DateTime.TryParse(worksheet.Cells[row, 4].Value?.ToString(), out var joinDate)
                              ? joinDate : default(DateTime), // Join Date

                                // Membership Type
                                MemberMembershipTypes = new List<MemberMembershipType>
                        {
                            new MemberMembershipType
                            {
                                MembershipType = new MembershipType
                                {
                                    TypeName = worksheet.Cells[row, 5].Value?.ToString()?.Trim() ?? "Unknown"
                                }
                            }
                        },

                                // Address - Check for missing fields
                                Addresses = new List<Address>
                        {
                            new Address
                            {
                                City = worksheet.Cells[row, 3]?.Value?.ToString()?.Trim() ?? "N/A",
                                AddressLine2 = worksheet.Cells[row, 6]?.Value?.ToString()?.Trim() ?? "N/A"
                            }
                        },

                                // Contact Information
                                MemberContacts = new List<MemberContact>()
                            };

                            // Handle contact information safely
                            string contactInfo = worksheet.Cells[row, 7]?.Value?.ToString() ?? "";
                            string[] contactParts = contactInfo.Split('|');

                            member.MemberContacts.Add(new MemberContact
                            {
                                Contact = new Contact
                                {
                                    Phone = contactParts.Length > 0 ? contactParts[0].Trim() : "No Phone",
                                    Email = contactParts.Length > 1 ? contactParts[1].Trim() : "No Email"
                                }
                            });

                            members.Add(member);
                        }

                        // Save to database
                        if (members.Any())
                        {
                            _context.Members.AddRange(members);
                            _context.SaveChanges();
                            TempData["Success"] = "Members imported successfully!";
                        }
                        else
                        {
                            TempData["Error"] = "No valid data found in the Excel file.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error importing members: {ex.Message}";
            }

            return RedirectToAction("Index");
        }




        // GET: Member/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.Addresses)
                .Include(m => m.MemberMembershipTypes).ThenInclude(m => m.MembershipType)
                .Include(m => m.MemberContacts).ThenInclude(m => m.Contact)
                .Include(m => m.MemberLogo)
                .Include(m => m.IndustryNAICSCodes).ThenInclude(m => m.NAICSCode)
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
            Member member = new Member
            {
                Addresses = new List<Address> { new Address() },  // Initializing with one empty address
                MemberContacts = new List<MemberContact> { new MemberContact() }  // Initializing with one empty contact
            };
            PopulateAssignedMTagData(member);
            PopulateAssignedSectorData(member);
            PopulateAssignedMembershipTypeData(member);
            PopulateAssignedNaicsCodeData(member);
            return View();
        }

        // POST: Member/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,MemberName,MemberSize,WebsiteUrl,JoinDate,IsPaid,MemberNote")]
                                                Member member, IFormFile? thePicture, string[] selectedOptionsTag, 
                                                string[] selectedOptionsSector, string[] selectedOptionsMembership, string[] selectedOptionsNaicsCode)
        {
            try
            {
                // Update Member Tags (MTag)
                UpdateMemberMTag(selectedOptionsTag, member);

                // Update Member Sectors
                UpdateMemberSector(selectedOptionsSector, member);

                // Update Member Membership Types
                UpdateMemberMembershipType(selectedOptionsMembership, member);
                UpdateMemberNaicsCode(selectedOptionsNaicsCode, member);

                if (ModelState.IsValid)
                {
                    // Handle file upload for picture
                    await AddPicture(member, thePicture);

                    // Add the member to the context and save changes
                    _context.Add(member);
                    await _context.SaveChangesAsync();

                    // Success message
                    TempData["SuccessMessage"] = $"Member: {member.MemberName} added successfully!";
                    // Redirect to the index view
                    // Assuming you have a list of addresses and you want to pass the MemberId of the first address

                    return RedirectToAction(nameof(Create), "Address", new { MemberId = member.ID });
                    //return View(member);



                    // If no address found, handle it appropriately (e.g., show an error or return to a list page)
                    //return RedirectToAction("Index", "Address");
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }

            // Populate assigned data if the model state is invalid
            PopulateAssignedMTagData(member);
            PopulateAssignedSectorData(member);
            PopulateAssignedMembershipTypeData(member);  // New method to populate membership type data
            PopulateAssignedNaicsCodeData(member);

            return View(member);
        }


        // GET: Member/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.MemberThumbnail)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Member/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Byte[] RowVersion, string? chkRemoveImage, IFormFile? thePicture)
        {
            var memberToUpdate = await _context.Members
                .Include(m => m.MemberThumbnail)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (memberToUpdate == null)
            {
                return NotFound();
            }

            // Attach the RowVersion for concurrency tracking
            _context.Entry(memberToUpdate).Property("RowVersion").OriginalValue = RowVersion;

            // Try updating the model with user input
            if (await TryUpdateModelAsync<Member>(memberToUpdate, "",
                m => m.MemberName, m => m.MemberSize, m => m.WebsiteUrl, m => m.JoinDate, m => m.IsPaid, m => m.MemberLogo))
            {
                try
                {
                    if (chkRemoveImage != null)
                    {
                        // If we are deleting the image and thumbnail, make sure to notify the Change Tracker
                        memberToUpdate.MemberThumbnail = _context.MemebrThumbnails.FirstOrDefault(p => p.MemberID == memberToUpdate.ID);
                        // Set the image fields to null to delete them
                        memberToUpdate.MemberLogo = null;
                        memberToUpdate.MemberThumbnail = null;
                    }
                    else
                    {
                        // Add or update the picture if one is provided
                        await AddPicture(memberToUpdate, thePicture);
                    }

                    // Save changes
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Member: {memberToUpdate.MemberName} updated successfully!";

                    return RedirectToAction("Details", new { id = memberToUpdate.ID });
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Please try again later.");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Member)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();

                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "The record was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Member)databaseEntry.ToObject();

                        // Compare each field to provide specific feedback
                        if (databaseValues.MemberName != clientValues.MemberName)
                            ModelState.AddModelError("MemberName", $"Current value: {databaseValues.MemberName}");
                        if (databaseValues.MemberSize != clientValues.MemberSize)
                            ModelState.AddModelError("MemberSize", $"Current value: {databaseValues.MemberSize}");
                        if (databaseValues.WebsiteUrl != clientValues.WebsiteUrl)
                            ModelState.AddModelError("WebsiteUrl", $"Current value: {databaseValues.WebsiteUrl}");
                        if (databaseValues.JoinDate != clientValues.JoinDate)
                            ModelState.AddModelError("JoinDate", $"Current value: {databaseValues.JoinDate}");
                        if (databaseValues.IsPaid != clientValues.IsPaid)
                            ModelState.AddModelError("IsPaid", $"Current value: {databaseValues.IsPaid}");

                        // Handle MemberLogo and MemberThumbnail separately since they are related to the image
                        //if (databaseValues.MemberLogo != clientValues.MemberLogo)
                        //    ModelState.AddModelError("MemberLogo", $"Current value: {databaseValues.MemberLogo?.Content ?? "No logo"}");
                        //if (databaseValues.MemberThumbnail != clientValues.MemberThumbnail)
                        //    ModelState.AddModelError("MemberThumbnail", $"Current value: {databaseValues.MemberThumbnail?.FileName ?? "No thumbnail"}");

                        ModelState.AddModelError("", "The record was modified by another user after you started editing. If you still want to save your changes, click the Save button again.");

                        // Update RowVersion for the next attempt
                        memberToUpdate.RowVersion = databaseValues.RowVersion ?? Array.Empty<byte>();
                        ModelState.Remove("RowVersion");
                    }
                }
                catch (DbUpdateException dex)
                {
                    string message = dex.GetBaseException().Message;
                    ModelState.AddModelError("", $"Unable to save changes: {message}");
                }
            }

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
                .Include(m => m.MemberThumbnail)
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
            TempData["SuccessMessage"] = $"Member: {member.MemberName} archived successfully!";

            return RedirectToAction(nameof(Index));
        }

        private async Task AddPicture(Member member, IFormFile thePicture)
        {
            //Get the picture and save it with the Patient (2 sizes)
            if (thePicture != null)
            {
                string mimeType = thePicture.ContentType;
                long fileLength = thePicture.Length;
                if (!(mimeType == "" || fileLength == 0))//Looks like we have a file!!!
                {
                    if (mimeType.Contains("image"))
                    {
                        using var memoryStream = new MemoryStream();
                        await thePicture.CopyToAsync(memoryStream);
                        var pictureArray = memoryStream.ToArray();//Gives us the Byte[]

                        //Check if we are replacing or creating new
                        if (member.MemberLogo != null)
                        {
                            //We already have pictures so just replace the Byte[]
                            member.MemberLogo.Content = ResizeImage.ShrinkImageWebp(pictureArray, 500, 600);

                            //Get the Thumbnail so we can update it.  Remember we didn't include it
                            member.MemberThumbnail = _context.MemebrThumbnails.Where(p => p.MemberID == member.ID).FirstOrDefault();
                            if (member.MemberThumbnail != null)
                            {
                                member.MemberThumbnail.Content = ResizeImage.ShrinkImageWebp(pictureArray, 115, 125);
                            }
                        }
                        else //No pictures saved so start new
                        {
                            member.MemberLogo = new MemberLogo
                            {
                                Content = ResizeImage.ShrinkImageWebp(pictureArray, 500, 600),
                                MimeType = "image/webp"
                            };
                            member.MemberThumbnail = new MemberThumbnail
                            {
                                Content = ResizeImage.ShrinkImageWebp(pictureArray, 115, 125),
                                MimeType = "image/webp"
                            };
                        }
                    }
                }
            }
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.ID == id);
        }

        public async Task<IActionResult> GetMemberPreview(int id)
        {
            var member = await _context.Members
                .Include(m => m.Addresses) // Include the related Address
                .Include(m => m.MemberThumbnail)
                .Include(m => m.MemberMembershipTypes)
                .ThenInclude(mm => mm.MembershipType)
                .Include(m => m.MemberContacts).ThenInclude(m => m.Contact)
                .Include(m => m.IndustryNAICSCodes).ThenInclude(m => m.NAICSCode)
                .FirstOrDefaultAsync(m => m.ID == id); // Use async version for better performance

            if (member == null)
            {
                return NotFound(); // Return 404 if the member doesn't exist
            }

            return PartialView("_MemberPreview", member); // Ensure the partial view name matches
        }

        private void PopulateDropdowns()
        {
            var membershipTypes = _context.MembershipTypes.ToList();

            ViewData["MembershipTypes"] = new SelectList(membershipTypes, "Id", "TypeName");

        }

        private void PopulateAssignedMTagData(Member member)
        {
            // Get all available MTags
            var allTags = _context.MTags.ToList();
            // Get the set of currently selected MTag IDs for the member
            var currentTagsHS = new HashSet<int>(member.MemberTags.Select(mt => mt.MTagID));

            // Lists for selected and available tags
            var selected = new List<ListOptionVM>();
            var available = new List<ListOptionVM>();

            // Populate selected and available lists based on whether the tag is selected
            foreach (var tag in allTags)
            {
                var option = new ListOptionVM
                {
                    ID = tag.Id,
                    DisplayText = tag.MTagName
                };

                if (currentTagsHS.Contains(tag.Id))
                {
                    selected.Add(option);
                }
                else
                {
                    available.Add(option);
                }
            }

            // Sort and assign to ViewData for use in the view
            ViewData["selOptsTag"] = new MultiSelectList(selected.OrderBy(s => s.DisplayText), "ID", "DisplayText");
            ViewData["availOptsTag"] = new MultiSelectList(available.OrderBy(s => s.DisplayText), "ID", "DisplayText");
        }

        private void PopulateAssignedSectorData(Member member)
        {
            // Get all available MTags
            var allTags = _context.Sectors.ToList();
            // Get the set of currently selected MTag IDs for the member
            var currentTagsHS = new HashSet<int>(member.MemberSectors.Select(mt => mt.SectorId));

            // Lists for selected and available tags
            var selected = new List<ListOptionVM>();
            var available = new List<ListOptionVM>();

            // Populate selected and available lists based on whether the tag is selected
            foreach (var tag in allTags)
            {
                var option = new ListOptionVM
                {
                    ID = tag.Id,
                    DisplayText = tag.SectorName
                };

                if (currentTagsHS.Contains(tag.Id))
                {
                    selected.Add(option);
                }
                else
                {
                    available.Add(option);
                }
            }

            // Sort and assign to ViewData for use in the view
            ViewData["selOptsSector"] = new MultiSelectList(selected.OrderBy(s => s.DisplayText), "ID", "DisplayText");
            ViewData["availOptsSector"] = new MultiSelectList(available.OrderBy(s => s.DisplayText), "ID", "DisplayText");
        }

        private void PopulateAssignedMembershipTypeData(Member member)
        {
            // Get all available MembershipTypes
            var allMembershipTypes = _context.MembershipTypes.ToList();
            // Get the set of currently selected MembershipType IDs for the member
            var currentMembershipTypesHS = new HashSet<int>(member.MemberMembershipTypes.Select(mt => mt.MembershipTypeId));

            // Lists for selected and available membership types
            var selected = new List<ListOptionVM>();
            var available = new List<ListOptionVM>();

            // Populate selected and available lists based on whether the membership type is selected
            foreach (var membershipType in allMembershipTypes)
            {
                var option = new ListOptionVM
                {
                    ID = membershipType.Id,
                    DisplayText = membershipType.TypeName
                };

                if (currentMembershipTypesHS.Contains(membershipType.Id))
                {
                    selected.Add(option);
                }
                else
                {
                    available.Add(option);
                }
            }

            // Sort and assign to ViewData for use in the view
            ViewData["selOptsMembership"] = new MultiSelectList(selected.OrderBy(s => s.DisplayText), "ID", "DisplayText");
            ViewData["availOptsMembership"] = new MultiSelectList(available.OrderBy(s => s.DisplayText), "ID", "DisplayText");
        }

        private void PopulateAssignedNaicsCodeData(Member member)
        {
            // Get all available NAICS Codes
            var allNaicsCode = _context.NAICSCodes.ToList();

            // Get the set of currently selected NAICS Code IDs for the member
            var currentNaicsCodesHS = new HashSet<int>(member.IndustryNAICSCodes.Select(nc => nc.NAICSCodeId));

            // Lists for selected and available NAICS codes
            var selected = new List<ListOptionVM>();
            var available = new List<ListOptionVM>();

            // Populate selected and available lists based on whether the NAICS code is selected
            foreach (var naicsCode in allNaicsCode)
            {
                var option = new ListOptionVM
                {
                    ID = naicsCode.Id,  // Ensure "Id" exists in the NAICSCodes entity
                    DisplayText = naicsCode.Code // Adjust this based on your model property
                };

                if (currentNaicsCodesHS.Contains(naicsCode.Id))
                {
                    selected.Add(option);
                }
                else
                {
                    available.Add(option);
                }
            }

            // Sort and assign to ViewData for use in the view
            ViewData["selOptsNaiceCode"] = new MultiSelectList(selected.OrderBy(s => s.DisplayText), "ID", "DisplayText");
            ViewData["availOptsNaicsCode"] = new MultiSelectList(available.OrderBy(s => s.DisplayText), "ID", "DisplayText");
        }



        private void UpdateMemberMTag(string[] selectedOptions, Member memberToUpdate)
        {
            // If no specialties (MTags) are selected, clear the existing collection
            if (selectedOptions == null)
            {
                memberToUpdate.MemberTags = new List<MemberTag>();
                return;
            }

            // Create a HashSet for the selected options (IDs as strings)
            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            // Get the current MTag IDs of the member as a HashSet
            var currentTagsHS = new HashSet<int>(memberToUpdate.MemberTags.Select(mt => mt.MTagID));

            // Iterate over all available MTags
            foreach (var tag in _context.MTags)
            {
                // Check if the MTag is selected
                if (selectedOptionsHS.Contains(tag.Id.ToString())) // It's selected
                {
                    if (!currentTagsHS.Contains(tag.Id)) // It's not already in the member's tags list
                    {
                        // Add the tag to the member's tags
                        memberToUpdate.MemberTags.Add(new MemberTag
                        {
                            MTagID = tag.Id,
                            MemberId = memberToUpdate.ID
                        });
                    }
                }
                else // It's not selected
                {
                    if (currentTagsHS.Contains(tag.Id)) // But it is in the member's current list
                    {
                        // Remove the tag from the member's tags
                        var tagToRemove = memberToUpdate.MemberTags
                            .FirstOrDefault(mt => mt.MTagID == tag.Id);
                        if (tagToRemove != null)
                        {
                            memberToUpdate.MemberTags.Remove(tagToRemove);
                        }
                    }
                }
            }
        }

        private void UpdateMemberSector(string[] selectedOptions, Member memberToUpdate)
        {
            // If no specialties (MTags) are selected, clear the existing collection
            if (selectedOptions == null)
            {
                memberToUpdate.MemberSectors = new List<MemberSector>();
                return;
            }

            // Create a HashSet for the selected options (IDs as strings)
            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            // Get the current MTag IDs of the member as a HashSet
            var currentTagsHS = new HashSet<int>(memberToUpdate.MemberSectors.Select(mt => mt.SectorId));

            // Iterate over all available MTags
            foreach (var tag in _context.Sectors)
            {
                // Check if the MTag is selected
                if (selectedOptionsHS.Contains(tag.Id.ToString())) // It's selected
                {
                    if (!currentTagsHS.Contains(tag.Id)) // It's not already in the member's tags list
                    {
                        // Add the tag to the member's tags
                        memberToUpdate.MemberSectors.Add(new MemberSector
                        {
                            SectorId = tag.Id,
                            MemberId = memberToUpdate.ID
                        });
                    }
                }
                else // It's not selected
                {
                    if (currentTagsHS.Contains(tag.Id)) // But it is in the member's current list
                    {
                        // Remove the tag from the member's tags
                        var tagToRemove = memberToUpdate.MemberSectors
                            .FirstOrDefault(mt => mt.SectorId == tag.Id);
                        if (tagToRemove != null)
                        {
                            memberToUpdate.MemberSectors.Remove(tagToRemove);
                        }
                    }
                }
            }
        }

        private void UpdateMemberMembershipType(string[] selectedOptions, Member memberToUpdate)
        {
            // If no membership types are selected, clear the existing collection
            if (selectedOptions == null)
            {
                memberToUpdate.MemberMembershipTypes = new List<MemberMembershipType>();
                return;
            }

            // Create a HashSet for the selected options (IDs as strings)
            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            // Get the current MembershipType IDs of the member as a HashSet
            var currentMembershipTypesHS = new HashSet<int>(memberToUpdate.MemberMembershipTypes.Select(mt => mt.MembershipTypeId));

            // Iterate over all available MembershipTypes
            foreach (var membershipType in _context.MembershipTypes)
            {
                // Check if the MembershipType is selected
                if (selectedOptionsHS.Contains(membershipType.Id.ToString())) // It's selected
                {
                    if (!currentMembershipTypesHS.Contains(membershipType.Id)) // It's not already in the member's membership types list
                    {
                        // Add the membership type to the member's list
                        memberToUpdate.MemberMembershipTypes.Add(new MemberMembershipType
                        {
                            MembershipTypeId = membershipType.Id,
                            MemberId = memberToUpdate.ID
                        });
                    }
                }
                else // It's not selected
                {
                    if (currentMembershipTypesHS.Contains(membershipType.Id)) // But it is in the member's current list
                    {
                        // Remove the membership type from the member's list
                        var membershipTypeToRemove = memberToUpdate.MemberMembershipTypes
                            .FirstOrDefault(mt => mt.MembershipTypeId == membershipType.Id);
                        if (membershipTypeToRemove != null)
                        {
                            memberToUpdate.MemberMembershipTypes.Remove(membershipTypeToRemove);
                        }
                    }
                }
            }
        }


        private void UpdateMemberNaicsCode(string[] selectedOptions, Member memberToUpdate)
        {
            if (selectedOptions == null)
            {
                // If no NAICS codes are selected, clear the existing collection
                memberToUpdate.IndustryNAICSCodes = new List<IndustryNAICSCode>();
                return;
            }

            var selectedOptionsHS = new HashSet<int>(selectedOptions.Select(int.Parse));
            var currentNaicsCodeHS = new HashSet<int>(memberToUpdate.IndustryNAICSCodes.Select(naics => naics.NAICSCodeId));

            // Iterate over all available NAICS Codes
            foreach (var naicsCode in _context.NAICSCodes)
            {
                if (selectedOptionsHS.Contains(naicsCode.Id))  // Selected in UI
                {
                    if (!currentNaicsCodeHS.Contains(naicsCode.Id))  // Not already added
                    {
                        memberToUpdate.IndustryNAICSCodes.Add(new IndustryNAICSCode
                        {
                            NAICSCodeId = naicsCode.Id,
                            MemberId = memberToUpdate.ID
                        });
                    }
                }
                else  // Not selected in UI
                {
                    if (currentNaicsCodeHS.Contains(naicsCode.Id))  // But exists in DB
                    {
                        var naicsToRemove = memberToUpdate.IndustryNAICSCodes
                            .FirstOrDefault(nc => nc.NAICSCodeId == naicsCode.Id);
                        if (naicsToRemove != null)
                        {
                            memberToUpdate.IndustryNAICSCodes.Remove(naicsToRemove);
                        }
                    }
                }
            }
        }


        [HttpPost]
        public async Task<IActionResult> SaveMemberNote(int id, string note)
        {
            var memberToUpdate = await _context.Members.FirstOrDefaultAsync(m => m.ID == id);

            if (memberToUpdate == null)
            {
                return Json(new { success = false, message = "Member not found." });
            }

            // Update MemberNote
            memberToUpdate.MemberNote = note;

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
