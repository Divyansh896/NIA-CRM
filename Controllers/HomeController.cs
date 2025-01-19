using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NIA_CRM.Data;
using NIA_CRM.Models;
using SQLitePCL;

namespace NIA_CRM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NIACRMContext _context;
        public HomeController(ILogger<HomeController> logger, NIACRMContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(string? SearchString, int? page, int? pageSizeID,
    string? actionButton, string sortDirection = "asc", string sortField = "Member Name")
        {
            // List of sort options (make sure they match the column names in the view)
            string[] sortOptions = new[] { "Member Name", "Organization", "Industry" };

            // Default page size if not provided
            int defaultPageSize = 10;
            int pageSize = pageSizeID ?? defaultPageSize; // Use the page size ID or the default size
            int pageNumber = page ?? 1; // Default to first page if no page is specified

            // Query for members with related data (industries, organizations, contacts, addresses)
            var memberDetails = _context.Members
                .Include(m => m.Organization) // Include organization for each member
                    .ThenInclude(o => o.Industry) // Include industry related to the organization
                .Include(m => m.Address) // Include address for each member
                .Include(m => m.Organization.ContactOrganizations) // Include contact organizations related to the organization
                    .ThenInclude(co => co.Contact) // Include the contact details for each contact organization
                .Select(m => new
                {
                    MemberName = m.MemberName, // Assuming 'MemberName' is the name property for members
                    OrganizationName = m.Organization.OrganizationName,
                    IndustryName = m.Organization.Industry.IndustryName,
                    Address = new
                    {
                        m.Address.AddressLineOne,
                        m.Address.AddressLineTwo,
                        m.Address.City,
                        m.Address.StateProvince,
                        m.Address.PostalCode,
                        m.Address.Country
                    },
                    Contacts = m.Organization.ContactOrganizations.Select(co => new
                    {
                        ContactName = co.Contact.ContactName,
                        Title = co.Contact.Title,
                        Department = co.Contact.Department,
                        Email = co.Contact.EMail,
                        Phone = co.Contact.Phone,
                        Linkedin = co.Contact.LinkedinUrl,
                        IsVIP = co.Contact.IsVIP
                    }).ToList()
                });

            // Before sorting, see if we have called for a change of filtering or sorting
            if (!String.IsNullOrEmpty(actionButton)) // Form Submitted!
            {
                pageNumber = 1; // Reset page to start

                if (sortOptions.Contains(actionButton)) // Change of sort is requested
                {
                    if (actionButton == sortField) // Reverse order on the same field
                    {
                        sortDirection = sortDirection == "asc" ? "desc" : "asc";
                    }
                    sortField = actionButton; // Sort by the button clicked
                }
            }

            // Apply sorting based on the selected field and direction
            if (sortField == "Member Name")
            {
                memberDetails = sortDirection == "asc"
                    ? memberDetails.OrderBy(m => m.MemberName)
                    : memberDetails.OrderByDescending(m => m.MemberName);
            }
            else if (sortField == "Organization")
            {
                memberDetails = sortDirection == "asc"
                    ? memberDetails.OrderBy(m => m.OrganizationName)
                    : memberDetails.OrderByDescending(m => m.OrganizationName);
            }
            else if (sortField == "Industry")
            {
                memberDetails = sortDirection == "asc"
                    ? memberDetails.OrderBy(m => m.IndustryName)
                    : memberDetails.OrderByDescending(m => m.IndustryName);
            }

            // If a search string is provided, filter the results
            if (!string.IsNullOrEmpty(SearchString))
            {
                memberDetails = memberDetails.Where(m =>
                    m.MemberName.Contains(SearchString) ||
                    m.OrganizationName.Contains(SearchString) ||
                    m.IndustryName.Contains(SearchString));
            }

           

            // Pass the model to the view
            //return View("MemberDetails", memberDetails);
            return View();
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
    }
}
