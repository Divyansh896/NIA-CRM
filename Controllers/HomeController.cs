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

        public async Task<IActionResult> Index()
        {
            var addresses = await _context.Members
                .Include(m => m.Address)
                .Where(m => m.Address != null) // Ensure no null addresses
                .ToListAsync();

            var cityCounts = await _context.Members
                                            .Where(m => m.Address != null)  // Ensure there are addresses
                                            .GroupBy(m => m.Address.City)  // Access City through Address
                                            .Select(g => new { City = g.Key, Count = g.Count() })  // Get the city and count of addresses
                                            .ToListAsync();

            var membershipCount = await _context.MemberMembershipTypes
                .GroupBy(mmt => mmt.MembershipType)
                .Select(g => new
                {
                    MembershipType = g.Key,
                    Count = g.Count()
                })
                .ToArrayAsync();

            var memberJoinDates = await _context.Members
                                                 .GroupBy(m => new { m.JoinDate.Year, m.JoinDate.Month })  // No need to check for null
                                                 .Select(g => new
                                                 {
                                                     Year = g.Key.Year,
                                                     Month = g.Key.Month,
                                                     MonthName = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM"),
                                                     Count = g.Count()
                                                 })
     .ToListAsync();
            // Execute the query asynchronously
            var memberAddress = await _context.Members
                                                .Include(m => m.Address)  // Include the Address navigation property
                                                .GroupBy(m => m.Address.City)  // Group by the City in Address
                                                .Select(g => new
                                                {
                                                    City = g.Key,
                                                    Count = g.Count()  // Count how many members have an address in each city
                                                })
                                                .ToListAsync();  // Execute the query asynchronously


            // Pass cityCounts as a model or through ViewData
            ViewData["CityCounts"] = cityCounts;
            ViewData["MemberCount"] = await _context.Members.CountAsync();
            ViewData["MembershipCount"] = membershipCount;
            ViewData["MembersJoins"] = memberJoinDates;
            ViewData["MembersAddress"] = memberAddress;

            return View(); // Pass nothing if using ViewData, or you can directly pass data via View()
        }
        

    }





}
