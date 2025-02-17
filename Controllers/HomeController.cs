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
                .Include(m => m.Addresses)
                .Where(m => m.Addresses != null && m.Addresses.Any()) // Ensure no null addresses
                .SelectMany(m => m.Addresses)
                .ToListAsync();

            var cityCounts = await _context.Members
    .Where(m => m.Addresses != null && m.Addresses.Any())  // Ensure there are addresses
    .SelectMany(m => m.Addresses)  // Flatten the addresses for each member
    .GroupBy(a => a.City)  // Group by city
    .Select(g => new { City = g.Key, Count = g.Count() })  // Get the city and count of addresses
    .ToListAsync();

            // Pass cityCounts as a model or through ViewData
            ViewData["CityCounts"] = cityCounts;
            ViewData["MemberCount"] = await _context.Members.CountAsync();

            return View(); // Pass nothing if using ViewData, or you can directly pass data via View()
        }


    }





}
