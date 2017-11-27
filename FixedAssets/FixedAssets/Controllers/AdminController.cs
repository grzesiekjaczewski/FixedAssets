using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using FixedAssets.Models;


namespace FixedAssets.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _db = new ApplicationDbContext();

        public AdminController()
        {
        }

        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        //[AllowAnonymous]
        public ActionResult AdminUserList()
        {
            var userList = UserManager.Users.OrderBy(m => m.UserName).ToList();
            return View(userList);
        }

        [Authorize(Roles = "Admin")]
        //[AllowAnonymous]
        public ActionResult AdminUserRolesList(Guid? id)
        {
            ApplicationUser user = UserManager.FindById(id.ToString());
            var userRoles = UserManager.GetRoles(id.ToString()).ToList();
            var roles = _db.Roles.OrderBy(x => x.Name).ToList();
            List<Role> activeRoles = new List<Role>();

            foreach (var role in roles)
            {
                Role activeRole = new Role();
                activeRole.Id = role.Id;
                activeRole.Name = role.Name;
                if (UserManager.IsInRole(id.ToString(), role.Name))
                {
                    activeRole.ActiveForUser = true;
                }
                else
                {
                    activeRole.ActiveForUser = false;
                }
                activeRoles.Add(activeRole);
            }

            ViewBag.User = user;
            return View(activeRoles);
        }
        
        [Authorize(Roles = "Admin")]
        //[AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminUserRolesList(FormCollection forms)
        {
            var userId = Request["UserId"];

            var roles = _db.Roles.OrderBy(x => x.Name).ToList();
            foreach (var role in roles)
            {
                var roleVal = Request[role.Name];
                if (roleVal == null)
                {
                    UserManager.RemoveFromRole(userId, role.Name);
                }
                else
                {
                    UserManager.AddToRole(userId, role.Name);
                }
            }
            return RedirectToAction("AdminUserList");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}