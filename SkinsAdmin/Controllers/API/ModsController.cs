using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkinsAdmin.Data;
using SkinsAdmin.Helper;
using SkinsAdmin.Models;

namespace SkinsAdmin.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ModsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ModsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Skins/GetSkins
        [HttpGet]
        public async Task<IActionResult> GetSkins(int page, string orderby = OrderByTypes.latest, string search = null)
        {
            var defultResponse = new
            {
                TotalPageNumber = 0,
                CurrentPage = 0,
                NumberOfRows = 0,
                Data = new Array[] { }
            };
            string apikeyWord = "apikey";
            string authHeader = Request.Headers["Authorization"].ToString();
            int? categoryId = null;
            Apps resultApps = null;
            if (!String.IsNullOrEmpty(authHeader.ToLower()) && authHeader.ToLower().StartsWith(apikeyWord))
            {
                //Extract credentials
                string extractedApiKey = authHeader.Substring(apikeyWord.Length).Trim();
                if (String.IsNullOrEmpty(extractedApiKey))
                    return Ok(defultResponse);
                resultApps = await _context.Apps.FirstOrDefaultAsync(x => x.AppKey.Equals(extractedApiKey));
                if (resultApps == null)
                    return Ok("Api Key was not provided.");
                categoryId = resultApps.CategoryId;
                if (!resultApps.IsActive)
                    return Ok(defultResponse);
            }
            else
            {
                return Ok("Headers Not Have Authorization");
            }
            if (categoryId == null)
                return Ok(defultResponse);

            IQueryable<Mods> listData = _context.Mods.Include(x => x.Category)
                .Where(x => (String.IsNullOrEmpty(search) || x.ModName.Contains(search) || x.ModDescription.Contains(search)) && x.CategoryId == categoryId);

            if (orderby.Equals(OrderByTypes.latest))
            {
                listData = listData.OrderByDescending(x => x.CreateAt);
            }
            else
            {
                listData = listData.OrderByDescending(x => x.CreateAt);
            }

            var rowPerPage = 20;

            var ordersCount = await listData.CountAsync();

            double numberOfPages = Math.Ceiling(ordersCount / (rowPerPage * 1.0));

            var skipValue = (page - 1) * rowPerPage;

            if (page < 1 || page > numberOfPages + 1)
            {
                return Ok("Invalid Page Value");
            }

            var listSkinData = await listData.Skip(skipValue).Take(rowPerPage).Select(x => new
            {
                Id = x.Id,
                ModName = x.ModName,
                ModThumbnailPath = x.ModThumbnailPath,
                CreateAt = x.CreateAt,
                CategoryId = x.CategoryId,
                CategoryName = x.Category != null ? x.Category.Name : null
            }).ToListAsync();


            var pagingResponse = new
            {
                TotalOfRows = ordersCount,
                TotalPageNumber = (int)numberOfPages,
                CurrentPage = page,
                NumberOfRows = listSkinData.Count(),
                Data = listSkinData
            };
            return Ok(pagingResponse);
        }

        [HttpGet]
        public IActionResult GetOrderbyTypes()
        {
            var listType = typeof(OrderByTypes)
                   .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                   .Where(fi => fi.IsLiteral && !fi.IsInitOnly) // constants, not readonly
                   .Where(fi => fi.FieldType == typeof(string)) // of type string
                   .Select(fi => fi.Name)
                   .ToList();
            return Ok(listType);
        }

    }
}
