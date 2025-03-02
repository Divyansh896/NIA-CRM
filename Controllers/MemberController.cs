using System;
using System.Collections.Generic;
using System.Linq;
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
            string[] sortOptions = { "Member Name", "Industry" };
            int numberFilters = 0;

            var members = _context.Members
                .Include(m => m.MemberThumbnail)
                .Include(m => m.Addresses)
                .Include(m => m.MemberMembershipTypes).ThenInclude(m => m.MembershipType)
                .Include(m => m.MemberContacts).ThenInclude(m => m.Contact)
                .Include(m => m.IndustryNAICSCodes).ThenInclude(m => m.NAICSCode)
                .Include(m => m.Addresses) //new added for addresses
            .AsNoTracking();


            if (!string.IsNullOrEmpty(actionButton) && actionButton == "ExportExcel")
            {
                var exportData = await members.AsNoTracking().ToListAsync();
                return ExportMembersToExcel(exportData);
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
                                             .FirstOrDefault(m => m.ID == MembershipTypes.Value);

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
            worksheet.Cells[1, 6].Value = "Address";
            worksheet.Cells[1, 7].Value = "Contact";
            worksheet.Cells[1, 8].Value = "VIP Status";

            // Populating data
            int row = 2;
            foreach (var member in members)
            {
                worksheet.Cells[row, 1].Value = member.ID;
                worksheet.Cells[row, 2].Value = member.MemberName;
                worksheet.Cells[row, 3].Value = member.Addresses.FirstOrDefault()?.City ?? "N/A";
                worksheet.Cells[row, 4].Value = member.JoinDate.ToString("yyyy-MM-dd") ?? "N/A"; // Format date
                worksheet.Cells[row, 5].Value = member.MemberMembershipTypes.FirstOrDefault()?.MembershipType?.TypeName ?? "N/A";

                worksheet.Cells[row, 6].Value = member.Addresses.FirstOrDefault() != null
                    ? $"{member.Addresses.FirstOrDefault().AddressLine2}, {member.Addresses.FirstOrDefault().City}, {member.Addresses.FirstOrDefault().StateProvince}, {member.Addresses.FirstOrDefault().PostalCode}"
                    : "No Address Available";

                worksheet.Cells[row, 7].Value = member.MemberContacts.FirstOrDefault() != null
                    ? $"{member.MemberContacts.FirstOrDefault().Contact.Phone} | {member.MemberContacts.FirstOrDefault().Contact.Email}"
                    : "No Contacts Available";

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

        //IMPORTING TO EXCEL


        public IActionResult ImportMembersFromExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("No file uploaded.");

            var members = new List<Member>();

            using (var package = new ExcelPackage(file.OpenReadStream()))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Get the first worksheet
                var rowCount = worksheet.Dimension.Rows; // Get the total number of rows

                for (int row = 2; row <= rowCount; row++) // Start from row 2 (skip header)
                {
                    var member = new Member
                    {
                        ID = Convert.ToInt32(worksheet.Cells[row, 1].Value), // Member ID
                        MemberName = worksheet.Cells[row, 2].Value?.ToString(), // Member Name
                        //City = worksheet.Cells[row, 3].Value?.ToString(), // City
                        JoinDate = DateTime.TryParse(worksheet.Cells[row, 4].Value?.ToString(), out var joinDate)
                            ? joinDate : default(DateTime), // Join Date
                       

                        MemberMembershipTypes = new List<MemberMembershipType>
                    {
                        new MemberMembershipType
                        {
                            // Ensure you map the correct membership type. 
                            // Assuming that the type is stored as a name or code
                            MembershipType = new MembershipType
                            {
                                TypeName = worksheet.Cells[row, 5].Value?.ToString() // Membership Type
                            }
                        }
                    },
                        Addresses = new List<Address>
                    {
                        new Address
                        {
                            City = worksheet.Cells[row, 3].Value?.ToString(),
                            AddressLine2 = worksheet.Cells[row, 6].Value?.ToString()
                        }
                    },
                        MemberContacts = new List<MemberContact>
                    {
                        new MemberContact
                        {
                            Contact = new Contact
                            {
                                Phone = worksheet.Cells[row, 7].Value?.ToString().Split('|')[0]?.Trim(),
                                Email = worksheet.Cells[row, 7].Value?.ToString().Split('|')[1]?.Trim()
                            }
                        }
                    },
                    };

                    members.Add(member);
                }
            }

            
            _context.Members.AddRange(members); 
            _context.SaveChanges(); 

            // Optionally, you could return a success message or the updated list of members
            return RedirectToAction("MembersList"); // Redirect to a view that displays the members
        }

        // Optionally, this action will show the list of members
        public IActionResult MembersList()
        {
            var members = _context.Members.ToList(); // Fetch the list of members from the database
            return View(members); // Pass members to the view
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
            return View();
        }

        // POST: Member/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,MemberFirstName,MemberMiddleName,MemberLastName,JoinDate,StandingStatus")]
        Member member, IFormFile? thePicture)
        {
            if (ModelState.IsValid)
            {
                await AddPicture(member, thePicture);
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
                .Include(m => m.MemberLogo)
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
                .Include(m => m.MemberLogo)
                .Include(m => m.MemberThumbnail)  // Ensure we include the MemberThumbnail as well
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
                    return RedirectToAction("Index", "Home");
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
                .Include(m => m.MemberLogo)
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

            ViewData["MembershipTypes"] = new SelectList(membershipTypes, "ID", "TypeName");



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
