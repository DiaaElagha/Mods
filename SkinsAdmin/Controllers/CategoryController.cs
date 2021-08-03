using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkinsAdmin.Data;
using SkinsAdmin.Helper;
using SkinsAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkinsAdmin.Controllers
{
    public class CategoryController : BaseController
    {
        public CategoryController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context) : base(userManager, roleManager, context)
        {
        }

        [HttpPost]
        public async Task<JsonResult> AjaxData([FromBody] dynamic data)
        {
            DataTableHelper d = new DataTableHelper(data);

            var query = await _context.Category.Where(x => d.SearchKey == null || x.Name.Contains(d.SearchKey))
                .OrderByDescending(x => x.CreateAt).ToListAsync();

            int totalCount = query.Count();

            var items = query.Select(x => new
            {
                x.Id,
                x.Name,
                TotalMods = _context.Mods.Count(c => c.CategoryId == x.Id),
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Area/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            if (ModelState.IsValid)
            {
                await _context.Category.AddAsync(model);
                await _context.SaveChangesAsync();
                return Content(ShowMessage.AddSuccessResult(), "application/json");
            }
            return View(model);
        }

        // GET: Admin/Area/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = await _context.Category.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind((nameof(Category.Name)), (nameof(Category.IsShowAndroid)), (nameof(Category.IsShowIOS)))] Category model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var baseEntoty = await _context.Category.FindAsync(id);
                    baseEntoty.Name = model.Name;
                    baseEntoty.IsShowAndroid = model.IsShowIOS;
                    baseEntoty.IsShowIOS = model.IsShowIOS;
                    baseEntoty.UpdateAt = DateTime.Now;
                    _context.Category.Update(baseEntoty);
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
            return View(model);
        }

        // GET: Admin/Area/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = await _context.Category.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            if (await _context.Mods.AnyAsync(x => x.CategoryId == id))
                return Content(ShowMessage.FailedResult(), "application/json");
            if (await _context.Apps.AnyAsync(x => x.CategoryId == id))
                return Content(ShowMessage.FailedResult(), "application/json");
            _context.Remove(model);
            await _context.SaveChangesAsync();
            return Content(ShowMessage.DeleteSuccessResult(), "application/json");
        }

        private async Task<bool> AreaExists(int id)
        {
            return await _context.Category.AnyAsync(e => e.Id == id);
        }

        [HttpPost]
        public async Task<string> DeleteCategories(int[] id)
        {
            try
            {
                var statusItem = await _context.Category.Where(x => id.Contains(x.Id)).ToListAsync();
                int result = 0;
                foreach (var item in statusItem)
                {
                    if (await _context.Apps.AnyAsync(x => x.CategoryId == item.Id) || await _context.Mods.AnyAsync(x => x.CategoryId == item.Id))
                    {
                        result += 1;
                        continue;
                    }
                    _context.Category.Remove(item);
                }
                await _context.SaveChangesAsync();
                if (result == statusItem.Count())
                {
                    return "error";
                }
                return "ok";
            }
            catch (Exception)
            {
                return "error";
            }
        }

    }
}
