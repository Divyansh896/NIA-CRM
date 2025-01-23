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
     int? IndustryID,
     int? page,
     int? pageSizeID,
     string? actionButton,
     string sortDirection = "asc",
     string sortField = "Member Name")
        {
            string[] sortOptions = { "Member Name", "Organization", "Industry" };
            int numberFilters = 0;

            // Populate dropdowns (ensure method works)
            PopulateDropdowns();

            // Fetch data from the database
            var memberDetailsQuery = _context.Members
                .Include(m => m.Organization)
                    .ThenInclude(o => o.Industry)
                .Include(m => m.Address)
                .Include(m => m.Organization.ContactOrganizations)
                    .ThenInclude(co => co.Contact)
                .Select(m => new DashboardDetailsViewModel
                {
                    ID = m.ID,
                    MemberFirstName = m.MemberFirstName,
                    MemberLastName = m.MemberLastName,
                    OrganizationID = m.OrganizationID,
                    OrganizationName = m.Organization.OrganizationName,
                    IndustryName = m.Organization.Industry.IndustryName,
                    IndustryID = m.Organization.IndustryID,
                    Address = new AddressViewModel
                    {
                        AddressLineOne = m.Address.AddressLineOne,
                        AddressLineTwo = m.Address.AddressLineTwo,
                        City = m.Address.City,
                        StateProvince = m.Address.StateProvince,
                        PostalCode = m.Address.PostalCode,
                        Country = m.Address.Country
                    },
                    Contacts = m.Organization.ContactOrganizations.Select(co => new ContactViewModel
                    {
                        ContactFirstName = co.Contact.ContactFirstName,
                        ContactLastName = co.Contact.ContactLastName,
                        ContactMiddleName = co.Contact.ContactMiddleName,
                        Title = co.Contact.Title,
                        Department = co.Contact.Department,
                        EMail = co.Contact.EMail,
                        Phone = co.Contact.Phone,
                        LinkedinUrl = co.Contact.LinkedinUrl,
                        IsVIP = co.Contact.IsVIP,
                        Summary = co.Contact.Summary
                    }).ToList()
                }).AsQueryable();

            // Count VIPs
            var memberCount = await _context.Members.CountAsync();
            var vipCount = await _context.Contacts.CountAsync(c => c.IsVIP);
                
            var copperportCount = await _context.Members
                .Include(m => m.Address)
                .CountAsync(m => m.Address.City == "Copperport");

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
            if (OrganizationID.HasValue)
            {
                memberDetailsQuery = memberDetailsQuery.Where(m => m.OrganizationID == OrganizationID.Value);
                numberFilters++;
            }

            if (IndustryID.HasValue)
            {
                memberDetailsQuery = memberDetailsQuery.Where(m => m.IndustryID == IndustryID.Value);
                numberFilters++;
            }

            if (!string.IsNullOrEmpty(SearchString))
            {
                memberDetailsQuery = memberDetailsQuery.Where(m =>
                    m.MemberFirstName.ToUpper().Contains(SearchString.ToUpper()) ||
                    m.MemberLastName.ToUpper().Contains(SearchString.ToUpper()));
                numberFilters++;
            }

            // Apply sorting
            memberDetailsQuery = sortField switch
            {
                "Member Name" => sortDirection == "asc"
                    ? memberDetailsQuery.OrderBy(m => m.MemberFirstName)
                    : memberDetailsQuery.OrderByDescending(m => m.MemberLastName),
                "Organization" => sortDirection == "asc"
                    ? memberDetailsQuery.OrderBy(m => m.OrganizationName)
                    : memberDetailsQuery.OrderByDescending(m => m.OrganizationName),
                "Industry" => sortDirection == "asc"
                    ? memberDetailsQuery.OrderBy(m => m.IndustryName)
                    : memberDetailsQuery.OrderByDescending(m => m.IndustryName),
                _ => memberDetailsQuery
            };

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
            var pagedData = await PaginatedList<DashboardDetailsViewModel>.CreateAsync(memberDetailsQuery.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);
        }

        private void PopulateDropdowns()
        {
            var organizations = _context.Members
     .Select(m => new { m.Organization.ID, m.Organization.OrganizationName })
     .Distinct()
     .OrderBy(o => o.OrganizationName)
     .AsNoTracking();

            var industries = _context.Members
                .Select(m => new { m.Organization.Industry.ID, m.Organization.Industry.IndustryName })
                .Distinct()
                .OrderBy(i => i.IndustryName)
                .AsNoTracking();

            ViewData["OrganizationID"] = new MultiSelectList(organizations, "ID", "OrganizationName");
            ViewData["IndustryID"] = new MultiSelectList(industries, "ID", "IndustryName");

        }

        public IActionResult GetMemberPreview(int id)
        {
            var member = _context.Members.Include(c => c.Address)
            .Include(co => co.Organization).FirstOrDefault(c => c.ID == id);
            if (member == null)
            {
                return NotFound();
            }
            return PartialView("_MemberContactPreview", member);  // Ensure the partial view name is correct
        }
    }
}
