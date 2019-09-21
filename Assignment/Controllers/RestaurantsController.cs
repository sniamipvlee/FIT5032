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
    public class RestaurantsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Restaurants
        [Authorize]
        public ActionResult Index()
        {
            //            var userId = User.Identity.GetUserId();
            var restaurants = db.Restaurants.ToList(); //.Where(s => s.AspNetUsersId == userId).ToList();
            return View(restaurants);
        }

        // GET: Restaurants/Details/5
        [ValidateInput(true)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurants restaurants = db.Restaurants.Find(id);
            if (restaurants == null)
            {
                return HttpNotFound();
            }

            double Latitude = double.Parse(restaurants.Latitude.Value.ToString("00.0000"));
            double Longitude = double.Parse(restaurants.Longitude.Value.ToString("00.0000"));
            ViewBag.Latitude = Latitude;
            ViewBag.Longitude = Longitude;
            ViewBag.Id = restaurants.Id;
            return View(restaurants);
        }

        // GET: Restaurants/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public ActionResult Create([Bind(Include = "Id,Name,Location,Description,Latitude,Longitude,AspNetUsersId")] Restaurants restaurants)
        {
            restaurants.AspNetUsersId = User.Identity.GetUserId();
            ModelState.Clear();
            TryValidateModel(restaurants);

            if (ModelState.IsValid)
            {
                db.Restaurants.Add(restaurants);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(restaurants);
        }

        // GET: Restaurants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurants restaurants = db.Restaurants.Find(id);
            if (restaurants == null)
            {
                return HttpNotFound();
            }
            return View(restaurants);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Location,Description,Latitude,Longitude,AspNetUsersId")] Restaurants restaurants)
        {
            if (ModelState.IsValid)
            {
                db.Entry(restaurants).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(restaurants);
        }

        // GET: Restaurants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurants restaurants = db.Restaurants.Find(id);
            if (restaurants == null)
            {
                return HttpNotFound();
            }
            return View(restaurants);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Restaurants restaurants = db.Restaurants.Find(id);
            db.Restaurants.Remove(restaurants);
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
