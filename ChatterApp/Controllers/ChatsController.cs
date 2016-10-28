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

namespace ChatterApp.Controllers
{
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

        public ActionResult Index()
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

            return View(db.Chats.ToList());
        }

        //public ActionResult Index()
        //{
        //    if (Request.IsAuthenticated)
        //    {
        //        //UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        //        //ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
        //        var currentUser = User.Identity.GetUserId();
        //        //// assuming there is some kind of relationship between products and users
        //        //List<Chat> chats = db.Chats.Where(p => p.ApplicationUser.Equals(currentUser.ApplicationUser)).ToList(); // or .email or other field from your users table

        //        //var userChats = db.Chats.Where(p => p.ApplicationUserID == currentUser).Tolist();
        //        // OPTIONAL: Make sure they see something
        //        //if (chats.Count == 0) // They have no related products so just send all of them
        //        //    chats = db.Chats.ToList();

        //        // only send the products related to that user
        //        return View();
        //    }
        //    // User is not authenticated, send them all products
        //    return View(db.Chats.ToList());
        //}

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
