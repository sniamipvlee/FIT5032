using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Assignment.Models;
using Microsoft.AspNet.Identity;

namespace Assignment.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Reviews
        public ActionResult Index()
        {
            var reviews = db.Reviews.ToList();
            Dictionary<int, string> restaurant = new Dictionary<int, string>();
            Dictionary<int, string> user = new Dictionary<int, string>();

            foreach (Reviews o in reviews)
            {
                try
                {
                    restaurant.Add(o.RestaurantsId, db.Restaurants.Where(s => s.Id == o.RestaurantsId).ToList()[0].Name);
                    user.Add(o.RestaurantsId, db.AspNetUsers.Where(s => s.Id == o.AspNetUsersId).ToList()[0].UserName);
                }
                catch (Exception e)
                {

                }
            }

            ViewBag.restaurantNames = restaurant;
            ViewBag.userNames = user;

            return View(reviews);
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reviews reviews = db.Reviews.Find(id);
            if (reviews == null)
            {
                return HttpNotFound();
            }
            return View(reviews);
        }

        // GET: Reviews/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AspNetUsersId,RestaurantsId,Description")] Reviews reviews, int restId)
        {

            reviews.AspNetUsersId = User.Identity.GetUserId();
            ModelState.Clear();
            TryValidateModel(reviews);

            reviews.RestaurantsId = restId;

            if (ModelState.IsValid)
            {
                db.Reviews.Add(reviews);
                db.SaveChanges();
                return RedirectToAction("Index","Requests");
            }

            return View(reviews);
        }

        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reviews reviews = db.Reviews.Find(id);
            if (reviews == null)
            {
                return HttpNotFound();
            }
            return View(reviews);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AspNetUsersId,RestaurantsId,Description")] Reviews reviews)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reviews).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reviews);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reviews reviews = db.Reviews.Find(id);
            if (reviews == null)
            {
                return HttpNotFound();
            }
            return View(reviews);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reviews reviews = db.Reviews.Find(id);
            db.Reviews.Remove(reviews);
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
