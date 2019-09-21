using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Assignment.Models;
using FIT5032_Week08A.Utils;
using Microsoft.AspNet.Identity;

namespace Assignment.Controllers
{
    public class RequestsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Requests
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var requests = db.Requests.Where(s => s.AspNetUsersId == userId).ToList();

            return View(requests);
        }

        // GET: Requests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requests requests = db.Requests.Find(id);
            if (requests == null)
            {
                return HttpNotFound();
            }
            return View(requests);
        }

        // GET: Requests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,RestaurantsId")] Requests requests, int restaurantId)
        {
            requests.AspNetUsersId = User.Identity.GetUserId();
            ModelState.Clear();
            TryValidateModel(requests);

            requests.RestaurantsId = restaurantId;

            if (ModelState.IsValid)
            {
                String toEmail = db.AspNetUsers.Find(db.Restaurants.Find(requests.RestaurantsId).AspNetUsersId).Email;
                String subject = "You have a new request";
                String contents = "From: " + db.AspNetUsers.Find(User.Identity.GetUserId()).Email + "\nDate: " + requests.Date;
                EmailSender es = new EmailSender();
                es.Send(toEmail, subject, contents);
                    
                db.Requests.Add(requests);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(requests);
        }

        // GET: Requests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requests requests = db.Requests.Find(id);
            if (requests == null)
            {
                return HttpNotFound();
            }
            return View(requests);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,AspNetUsersId,RestaurantsId")] Requests requests)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requests).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(requests);
        }

        // GET: Requests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requests requests = db.Requests.Find(id);
            if (requests == null)
            {
                return HttpNotFound();
            }
            return View(requests);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Requests requests = db.Requests.Find(id);
            db.Requests.Remove(requests);
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
