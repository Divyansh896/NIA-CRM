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
        public async Task<IActionResult> Index(string? SearchString, string? JoinDate, int? page, int? pageSizeID, string? actionButton, string sortDirection = "asc", string sortField = "Member Name")

        {
            string[] sortOptions = { "Member Name", "Industry" };
            int numberFilters = 0;

            var members = _context.Members
                .Include(m => m.MemberThumbnail)
                .Include(m =>m.Addresses)
                .Include(m => m.MemberMembershipTypes).ThenInclude(m=>m.MembershipType)
                .Include(m => m.MemberNotes)
                .Include(m=>m.MemberIndustries).ThenInclude(m=>m.Industry)
            .AsNoTracking();
                

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
                    m.MemberFirstName.ToUpper().Contains(SearchString.ToUpper()) ||
                    m.MemberLastName.ToUpper().Contains(SearchString.ToUpper()));
                numberFilters++;
            }

            if (!string.IsNullOrEmpty(JoinDate))
            {
                if (DateTime.TryParse(JoinDate, out var parsedDate))
                {
                    members = members.Where(m => m.JoinDate == parsedDate);
                }
                else
                {
                    ModelState.AddModelError("JoinDate", "Invalid date format. Please use YYYY-MM-DD.");
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

            // Handle paging
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Member>.CreateAsync(members, page ?? 1, pageSize);

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
                .Include(m => m.Addresses)
                .Include(m => m.MemberMembershipTypes).ThenInclude(m => m.MembershipType)
                .Include(m => m.MemberNotes)
                .Include(m => m.MemberIndustries).ThenInclude(m => m.Industry)
                .Include(m => m.Contacts)
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
        public async Task<IActionResult> Edit(int id, string? chkRemoveImage, IFormFile? thePicture)
        {
            var memberToUpdate = await _context.Members.FirstOrDefaultAsync(m => m.ID == id);

            if (memberToUpdate == null)

            {
                return NotFound();
            }

            // Try update model approach
            if (await TryUpdateModelAsync<Member>(memberToUpdate, "", m => m.MemberFirstName, m => m.MemberMiddleName, m => m.MemberLastName, m => m.JoinDate, m => m.StandingStatus))
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
                                member.MemberThumbnail.Content = ResizeImage.ShrinkImageWebp(pictureArray, 75, 90);
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
                                Content = ResizeImage.ShrinkImageWebp(pictureArray, 75, 90),
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
    }
}
