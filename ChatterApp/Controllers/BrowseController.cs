using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ChatterApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;

namespace ChatterApp.Controllers
{
    [Authorize]
    public class BrowseController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUser CurrentUser
        {
            get
            {
                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
                return currentUser;
            }
        }

        // GET: Browse
        public ActionResult Index()
        {
            // sort the users alphabetically
            var users = from u in db.Users
                        orderby u.UserName
                        select u;
            return View(users.ToList());
        }

        //public ActionResult Index(int? page)
        //{
        //    var members = db.Chats.Include(r => r.ApplicationUser.UserName);
        //    var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
        //    var onePageOfProducts = db.Chats.OrderBy(i => i.ID).ToPagedList(pageNumber, 25); // will only contain 10 reviews max because of the pageSize

        //    ViewBag.OnePageOfProducts = onePageOfProducts;
        //    return View();
        //}


        public ActionResult Follow(string username)
        {
            var targetUser = db.Users.Single(u => u.UserName == username);

            targetUser.Followers.Add(CurrentUser);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
