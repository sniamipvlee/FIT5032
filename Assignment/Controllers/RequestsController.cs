using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Assignment.Models;
using Assignment.Utils;
using Microsoft.AspNet.Identity;

namespace Assignment.Controllers
{
    [ValidateInput(true)]
    public class RequestsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Requests
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var requests = db.Requests.Where(s => s.AspNetUsersId == userId).ToList();

            Dictionary<int,string> restaurant = new Dictionary<int,string>();
            foreach (Requests o in requests)
            {
                try
                {
                    restaurant.Add(o.RestaurantsId, db.Restaurants.Where(s => s.Id == o.RestaurantsId).ToList()[0].Name);
                }
                catch (Exception e)
                {

                }
            }
            ViewBag.restaurantNames = restaurant;

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
        public ActionResult Create(int restaurantId)
        {
            var collectRequest = db.Requests.Where(s => s.RestaurantsId == restaurantId).ToList();
            Dictionary<String,int> collects = new Dictionary<String,int>();
            foreach (Requests r in collectRequest)
            {
                String date = r.Date.ToString();
                if (collects.ContainsKey(date))
                {
                    collects[date] = collects[date] + 1;
                }
                else
                {
                    collects[date] = 1;
                }
            }
            List<String> collectNames = new List<String>();
            List<int> collectCounts = new List<int>();
            foreach (var item in collects)
            {
                collectNames.Add(item.Key);
                collectCounts.Add(item.Value);
            }
            ViewBag.requestNameList = collectNames;
            ViewBag.requestCountList = collectCounts;

            int collectSeats = db.Restaurants.Where(s => s.Id == restaurantId).ToList()[0].Seats;
            ViewBag.totalSeat = collectSeats;

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
            var restaurantName = db.Restaurants.Where(s => s.Id == restaurantId).ToList()[0].Name;

            if (ModelState.IsValid)
            {
                String toEmail = db.AspNetUsers.Find(db.Restaurants.Find(requests.RestaurantsId).AspNetUsersId).Email;
                String customer = db.AspNetUsers.Find(User.Identity.GetUserId()).Email; ;
                String subject = "You have a new request";
                EmailSender es = new EmailSender();
                es.Send(toEmail, subject, requests.Date.ToString(), restaurantName,customer);
                    
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
