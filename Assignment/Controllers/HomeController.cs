using Assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Assignment.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private Model1 db = new Model1();
        public ActionResult Index()
        {
            var requests = db.Requests.ToList();
            var restaurants = db.Restaurants.ToList();
            Dictionary<string, int> restaurantCount = new Dictionary<string, int>();

            foreach (Requests r in requests)
            {
                try
                {
                    //string restName = db.Restaurants.Find(r.RestaurantsId).Name;
                    string restName = "";
                    foreach (Restaurants rest in restaurants)
                    {
                        if (rest.Id == r.RestaurantsId)
                        {
                            restName = rest.Name;
                            break;
                        }
                    }

                    if (restaurantCount.ContainsKey(restName))
                    {
                        restaurantCount[restName] = restaurantCount[restName] + 1;
                    }
                    else
                    {
                        restaurantCount.Add(restName,1);
                    }
                }
                catch (Exception e)
                {

                }
            }

            var decSort = from rest in restaurantCount orderby rest.Value descending select rest;
            List<string> deliverNames = new List<string>();
            List<int> deliverCounts = new List<int>();
            foreach (var item in decSort)
            {
                deliverNames.Add(item.Key);
                deliverCounts.Add(item.Value);
            }

            ViewBag.names = deliverNames.Take(6);
            ViewBag.counts = deliverCounts.Take(6);

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}