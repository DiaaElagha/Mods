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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SkinsAdmin.Controllers
{
    public class ModsController : BaseController
    {
        private readonly IHostingEnvironment _IHostingEnvironment;
        public ModsController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, ApplicationDbContext context,
            IHostingEnvironment IHostingEnvironment) : base(userManager, roleManager, context)
        {
            _IHostingEnvironment = IHostingEnvironment;
        }

        [HttpPost]
        public async Task<JsonResult> AjaxData([FromBody] dynamic data)
        {
            DataTableHelper d = new DataTableHelper(data);
            string jsonString = data.ToString();
            JObject ss = JObject.Parse(jsonString);

            int categoryId = (int)ss["categoryId"];

            var query = await _context.Mods.Include(x => x.Category).Where(x =>
                (d.SearchKey == null || x.ModName.Contains(d.SearchKey)) &&
                (categoryId == -1 || x.CategoryId == categoryId)).OrderByDescending(x => x.CreateAt).ToListAsync();

            int totalCount = query.Count();

            var items = query.Select(x => new
            {
                x.Id,
                x.ModName,
                x.ModUrl,
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

        public async Task<IActionResult> Index()
        {
            ViewData["listCategory"] = new SelectList(await _context.Category.ToListAsync(), nameof(Category.Id), nameof(Category.Name));
            return View();
        }

        // GET: Admin/Area/Create
        public async Task<IActionResult> Create()
        {
            ViewData["listCategory"] = new SelectList(await _context.Category.ToListAsync(), nameof(Category.Id), nameof(Category.Name));
            return View();
        }

        public async Task<IActionResult> Create2()
        {
            ViewData["listCategory"] = new SelectList(await _context.Category.ToListAsync(), nameof(Category.Id), nameof(Category.Name));
            return View();
        }

        // POST: Admin/Area/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Mods model, IFormFile ModThumbnailPath)
        {
            if (ModThumbnailPath != null)
            {
                ModelState.Remove(nameof(Mods.ModThumbnailPath));
            }
            if (ModelState.IsValid)
            {
                model.ModThumbnailPath = AbsoluteUri() + "/files/Images/";
                model.ModThumbnailPath += await ImageHelper.SaveImage(ModThumbnailPath, _IHostingEnvironment, "files/Images");

                await _context.Mods.AddAsync(model);
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
            var model = await _context.Mods.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            ViewData["listCategory"] = new SelectList(await _context.Category.ToListAsync(), nameof(Category.Id), nameof(Category.Name));
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind((nameof(Mods.ModName)), (nameof(Mods.ModDescription)), (nameof(Mods.ModUrl)), (nameof(Mods.CategoryId)))]
                Mods model, IFormFile skinFilePath, IFormFile ModThumbnailPath)
        {
            ModelState.Remove(nameof(Mods.ModThumbnailPath));
            if (ModelState.IsValid)
            {
                try
                {
                    var baseEntoty = await _context.Mods.FindAsync(id);
                    if (ModThumbnailPath != null)
                    {
                        baseEntoty.ModThumbnailPath = AbsoluteUri() + "/files/Images/";
                        baseEntoty.ModThumbnailPath += await ImageHelper.SaveImage(ModThumbnailPath, _IHostingEnvironment, "files/Images");
                    }

                    baseEntoty.ModName = model.ModName;
                    baseEntoty.CategoryId = model.CategoryId;
                    baseEntoty.UpdateAt = DateTime.Now;
                    _context.Mods.Update(baseEntoty);
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
            var model = await _context.Mods.FindAsync(id);
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
            return await _context.Mods.AnyAsync(e => e.Id == id);
        }

        [HttpPost]
        public async Task<string> DeleteSkins(int[] id)
        {
            try
            {
                var statusItem = await _context.Mods.Where(x => id.Contains(x.Id)).ToListAsync();
                _context.Mods.RemoveRange(statusItem);
                await _context.SaveChangesAsync();
                return "ok";
            }
            catch (Exception)
            {
                return "error";
            }
        }

    }
}
