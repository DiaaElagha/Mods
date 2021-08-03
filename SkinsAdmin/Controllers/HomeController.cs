using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkinsAdmin.Data;
using SkinsAdmin.Helper;
using SkinsAdmin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SkinsAdmin.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context) : base(userManager, roleManager, context)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            int totalMods = await _context.Mods.CountAsync();
            int totalApps = await _context.Apps.CountAsync();
            int totalCategories = await _context.Category.CountAsync();
            DashboardVM vm = new DashboardVM
            {
                totalMods = totalMods,
                totalApps = totalApps,
                totalCategories = totalCategories,
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> CreateUser(string username, string password)
        {
            var user = new ApplicationUser
            {
                UserName = username,
                FullName = "admin_" + username,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            await _userManager.CreateAsync(user, password);
            return Ok(user);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
