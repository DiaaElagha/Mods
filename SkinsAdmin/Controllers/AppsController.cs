using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SkinsAdmin.Data;
using SkinsAdmin.Helper;
using SkinsAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkinsAdmin.Controllers
{
    public class AppsController : BaseController
    {
        public AppsController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, ApplicationDbContext context) : base(userManager, roleManager, context)
        {
        }

        [HttpPost]
        public async Task<JsonResult> AjaxData([FromBody] dynamic data)
        {
            DataTableHelper d = new DataTableHelper(data);
            string jsonString = data.ToString();
            JObject ss = JObject.Parse(jsonString);

            bool checkActivation = (bool)ss["checkActivation"];

            var query = await _context.Apps.Include(x => x.Category).Where(x => (d.SearchKey == null 
                || x.Name.Contains(d.SearchKey) 
                || x.AppKey.Contains(d.SearchKey)) && x.IsActive == checkActivation)
                .OrderByDescending(x => x.CreateAt).ToListAsync();

            int totalCount = query.Count();

            var items = query.Select(x => new
            {
                x.Id,
                x.Name,
                x.IsActive,
                x.AppKey,
                CategoryName = x.Category != null ? x.Category?.Name : null,
                createAt = x.CreateAt.Value.ToString("MM/dd/yyyy"),
            }).Skip(d.Start).Take(d.Length).ToList();
            var result =
               new
               {
                   draw = d.Draw,
                   recordsTotal = totalCount,
                   recordsFiltered = totalCount,
                   data = items
               };
            return Json(result);
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Admin/Area/Create
        public async Task<IActionResult> Create()
        {
            ViewData["listCategory"] = new SelectList(await _context.Category.ToListAsync(), nameof(Category.Id), nameof(Category.Name));
            return View();
        }

        // POST: Admin/Area/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Apps model)
        {
            if (ModelState.IsValid)
            {
                await _context.Apps.AddAsync(model);
                await _context.SaveChangesAsync();
                return Content(ShowMessage.AddSuccessResult(), "application/json");
            }
            ViewData["listCategory"] = new SelectList(await _context.Category.ToListAsync(), nameof(Category.Id), nameof(Category.Name));
            return View(model);
        }

        // GET: Admin/Area/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = await _context.Apps.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            ViewData["listCategory"] = new SelectList(await _context.Category.ToListAsync(), nameof(Category.Id), nameof(Category.Name));
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind((nameof(Apps.Name)), (nameof(Apps.AppKey)), (nameof(Apps.CategoryId)), (nameof(Apps.IsActive)))] Apps model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var baseEntoty = await _context.Apps.FindAsync(id);

                    baseEntoty.Name = model.Name;
                    baseEntoty.CategoryId = model.CategoryId;
                    baseEntoty.IsActive = model.IsActive;
                    baseEntoty.UpdateAt = DateTime.Now;
                    _context.Apps.Update(baseEntoty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await AreaExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Content(ShowMessage.EditSuccessResult(), "application/json");
            }
            ViewData["listCategory"] = new SelectList(await _context.Category.ToListAsync(), nameof(Category.Id), nameof(Category.Name));
            return View(model);
        }

        // GET: Admin/Area/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = await _context.Apps.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            _context.Remove(model);
            await _context.SaveChangesAsync();
            return Content(ShowMessage.DeleteSuccessResult(), "application/json");
        }

        private async Task<bool> AreaExists(int id)
        {
            return await _context.Apps.AnyAsync(e => e.Id == id);
        }

    }
}
