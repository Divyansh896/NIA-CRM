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
            string[] sortOptions = new[] { "Member Name", "Organization", "Industry" };
            int defaultPageSize = 10;
            int pageSize = pageSizeID ?? defaultPageSize;
            int pageNumber = page ?? 1;

            var memberDetails = _context.Members
                .Include(m => m.Organization)
                    .ThenInclude(o => o.Industry)
                .Include(m => m.Address)
                .Include(m => m.Organization.ContactOrganizations)
                    .ThenInclude(co => co.Contact)
                .Select(m => new
                {
                    MemberName = m.Summary,
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
                        ContactFirstName = co.Contact.ContactFirstName,
                        ContactLastName = co.Contact.ContactLastName,
                        ContactMiddleName = co.Contact.ContactMiddleName,
                        Title = co.Contact.Title,
                        Department = co.Contact.Department,
                        Email = co.Contact.EMail,
                        Phone = co.Contact.Phone,
                        Linkedin = co.Contact.LinkedinUrl,

                        IsVIP = co.Contact.IsVIP,
                        Summary = co.Contact.Summary
                    }).ToList()
                });

            if (!String.IsNullOrEmpty(actionButton))
            {
                pageNumber = 1;

                if (sortOptions.Contains(actionButton))
                {
                    if (actionButton == sortField)
                    {
                        sortDirection = sortDirection == "asc" ? "desc" : "asc";
                    }
                    sortField = actionButton;
                }
            }

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

            if (!string.IsNullOrEmpty(SearchString))
            {
                memberDetails = memberDetails.Where(m =>
                    m.MemberName.Contains(SearchString) ||
                    m.OrganizationName.Contains(SearchString) ||
                    m.IndustryName.Contains(SearchString));
            }

            //return View("MemberDetails", memberDetails.ToList());
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
