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
    [RequireHttps]
    public class ChatsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Chats

        private ApplicationUser CurrentUser
        {
            get
            {
                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
                
                return currentUser;
            }
        }

        public ActionResult Index(int? page)
        {
            ViewBag.CurrentUser = CurrentUser;

            // CurrentUser.Following
            var followingUsernames = (from f in CurrentUser.Following
                                      select f.UserName).ToList();

            followingUsernames.Add(CurrentUser.UserName);

            var messages = from m in db.Chats
                           where followingUsernames.Contains(m.ApplicationUser.UserName)
                           orderby m.DatePosted descending
                           select m;

            //return View(db.Chats.ToList());

            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
            List<string> following = new List<string>();
            foreach (ApplicationUser followed in currentUser.Following)
            {
                following.Add(followed.Id);
            }
            IEnumerable<Chat> chatEnumerable = db.Chats.Where(c => following.Contains(c.ApplicationUser.Id)).Select(c => c).OrderByDescending(c => c.ID).AsEnumerable();
            List<Chat> chats = chatEnumerable.ToList();
            ViewBag.chats = chats;
            return View(db.Chats.ToList());

            //var posts = db.Chats.Include(r => r.Message).Include(r => r.DatePosted).Include(r => r.ApplicationUser.UserName);
            //var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            //var onePageOfProducts = db.Chats.OrderBy(i => i.ID).ToPagedList(pageNumber, 25); // will only contain 10 reviews max because of the pageSize

            //ViewBag.OnePageOfProducts = onePageOfProducts;
            //return View();

        }

        //public ActionResult Index(int? page)
        //{
        //    var posts = db.Chats.Include(r => r.Message).Include(r => r.DatePosted).Include(r => r.ApplicationUser.UserName);
        //    var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
        //    var onePageOfProducts = db.Chats.OrderBy(i => i.ID).ToPagedList(pageNumber, 25); // will only contain 10 reviews max because of the pageSize

        //    ViewBag.OnePageOfProducts = onePageOfProducts;
        //    return View();
        //}


        public ActionResult Browse()
        {
            return View("Browse/Index");
        }

        // GET: Chats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chat chat = db.Chats.Find(id);
            if (chat == null)
            {
                return HttpNotFound();
            }
            return View(chat);
        }

        // GET: Chats/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Chats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Message,DatePosted")] Chat chat)
        {
            chat.ApplicationUser = CurrentUser;
            chat.DatePosted = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Chats.Add(chat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chat);
        }

        // GET: Chats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chat chat = db.Chats.Find(id);
            if (chat == null)
            {
                return HttpNotFound();
            }
            return View(chat);
        }

        // POST: Chats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Message,DatePosted")] Chat chat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chat);
        }

        // GET: Chats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chat chat = db.Chats.Find(id);
            if (chat == null)
            {
                return HttpNotFound();
            }
            return View(chat);
        }

        // POST: Chats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chat chat = db.Chats.Find(id);
            db.Chats.Remove(chat);
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
