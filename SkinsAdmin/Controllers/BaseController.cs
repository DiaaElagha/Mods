using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using SkinsAdmin.Data;
using SkinsAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkinsAdmin.Controllers
{
    [Route("[controller]/[action]")]
    [Route("[controller]/[action]/{id?}")]
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly ApplicationDbContext _context;
        protected String UserId;
        protected String UserName;

        public BaseController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
             ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionName = ((ControllerActionDescriptor)filterContext.ActionDescriptor).ActionName.ToString().ToLower();
            var controllerName = ((ControllerActionDescriptor)filterContext.ActionDescriptor).ControllerName.ToString().ToLower();
            if (User.Identity.IsAuthenticated)
            {
                base.OnActionExecuting(filterContext);
                try
                {
                    UserId = _userManager.GetUserId(HttpContext.User);
                    var user = _userManager.Users.SingleOrDefault(x => x.Id.Equals(UserId));
                    UserName = user.UserName;
                    ViewBag.UserId = UserId;
                    ViewBag.FullName = user.FullName;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public ApplicationUser GetUser()
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id.Equals(UserId));
            return user;
        }

        public string AbsoluteUri()
        {
            var absoluteUri = string.Concat(
                                Request.Scheme,
                                "://",
                                Request.Host.ToUriComponent(),
                                Request.PathBase.ToUriComponent());
            return absoluteUri;
        }

    }
}
