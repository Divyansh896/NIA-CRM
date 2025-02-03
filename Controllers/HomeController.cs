using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NIA_CRM.CustomControllers;
using NIA_CRM.Data;
using NIA_CRM.Models;
using NIA_CRM.Utilities;
using NIA_CRM.ViewModels;
using SQLitePCL;

namespace NIA_CRM.Controllers
{
    public class HomeController : ElephantController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NIACRMContext _context;
        public HomeController(ILogger<HomeController> logger, NIACRMContext context)
        {
            _logger = logger;
            _context = context;
        }





        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Index(string? SearchString,
     int? OrganizationID,
     int? Members,
     int? MembershipTypes,
     int? page,
     int? pageSizeID,
     string? actionButton,
     string sortDirection = "asc",
     string sortField = "Industry")
        {
            string[] sortOptions = { "Industry" };
            int numberFilters = 0;

            // Populate dropdowns (ensure method works)
            PopulateDropdowns();
            // Fetch data from the database
            var memberDetailsQuery = _context.Members

    .Include(m => m.Contacts)
    .Include(m => m.Addresses) // Include Addresses related to the Member
    .Include(m => m.MemberMembershipTypes) // Include MembershipTypes related to the Member
    .Include(m => m.Cancellations) // Include Cancellations related to the Member
    .Include(m => m.MemberNotes) // Include Member Notes
    .Include(m => m.Interactions) // Include Interactions related to the Member
    .Include(m => m.MemberThumbnail)
    .Include(m => m.Contacts)
                .Include(m => m.IndustryNAICSCodes).ThenInclude(m => m.NAICSCode)
    .AsNoTracking();



            // Count VIPs
            var memberCount = await _context.Members.CountAsync();
            var vipCount = await _context.Contacts.CountAsync(c => c.IsVip);

            var copperportCount = await _context.Members
    .Include(m => m.Addresses) // Ensure Addresses is included
    .CountAsync(m => m.Addresses.Any(a => a.City == "Copperport")); // Check if any address has the city "Copperport"



            ViewData["MemberCount"] = memberCount;
            ViewData["VipCount"] = vipCount;
            ViewData["CopperportCount"] = copperportCount;
            

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

            // Apply filters
            if (!string.IsNullOrEmpty(SearchString))
            {
                memberDetailsQuery = memberDetailsQuery.Where(m =>
                    m.MemberName.ToUpper().Contains(SearchString.ToUpper()));
                numberFilters++;
            }

            if (Members.HasValue)
            {
                memberDetailsQuery = memberDetailsQuery.Where(p => p.ID == Members);
                numberFilters++;
            }

            if (MembershipTypes.HasValue)
            {
                // Assuming MembershipTypes is the ID or a collection of IDs for the membership type
                memberDetailsQuery = memberDetailsQuery
                    .Where(p => p.MemberMembershipTypes.Any(mmt => mmt.MembershipTypeId == MembershipTypes.Value));
                numberFilters++;
            }


            if (sortField == "Industry")
            {
                if (sortDirection == "asc")
                {
                    memberDetailsQuery = memberDetailsQuery
                        .OrderByDescending(p => p.MemberName);
                }
                else
                {
                    memberDetailsQuery = memberDetailsQuery
                        .OrderBy(p => p.MemberName);
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
            ViewData["records"] = $"Records Found: {memberDetailsQuery.Count()}";

            // Handle paging
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Member>.CreateAsync(memberDetailsQuery, page ?? 1, pageSize);

            return View(pagedData);
        }

        public async Task<IActionResult> GetMemberPreview(int id)
        {
            var member = await _context.Members
                .Include(m => m.Addresses) // Include the related Address
                .Include(m => m.MemberThumbnail)
                .Include(m => m.MemberMembershipTypes)
                .ThenInclude(mm => mm.MembershipType)
                .Include(m => m.Contacts)
                .Include(m => m.IndustryNAICSCodes).ThenInclude(m => m.NAICSCode)
                .FirstOrDefaultAsync(m => m.ID == id); // Use async version for better performance

            if (member == null)
            {
                return NotFound(); // Return 404 if the member doesn't exist
            }

            return PartialView("_MemberContactPreview", member); // Ensure the partial view name matches
        }

        private void PopulateDropdowns()
        {
            // Fetch Members for dropdown
            var members = _context.Members.ToList();
            ViewData["Members"] = new SelectList(members, "ID", "MemberName");

            // Fetch Industry NAICS Codes
            // Query the IndustryNAICSCode table and include related NAICSCode data.
            var naicsCodes = _context.NAICSCodes.ToList();

            var membershipTypes = _context.MembershipTypes.ToList();

            ViewData["MembershipTypes"] = new SelectList(membershipTypes, "ID", "TypeName");

            // Create a SelectList using the "ID" as the value field and "NAICSCode.Code" as the display field.
            ViewData["IndustryNAICSCodes"] = new SelectList(naicsCodes, "ID", "Code");

        }

    }
}
