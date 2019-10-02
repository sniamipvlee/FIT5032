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

            //--------------------------

            var reviews = db.Reviews.ToList();

            Dictionary<string, int> reviewCount = new Dictionary<string, int>();

            foreach (Reviews r in reviews)
            {
                try
                {
                    string restName = "";
                    foreach (Restaurants rest in restaurants)
                    {
                        if (rest.Id == r.RestaurantsId)
                        {
                            restName = rest.Name;
                            break;
                        }
                    }

                    if (reviewCount.ContainsKey(restName))
                    {
                        reviewCount[restName] = reviewCount[restName] + 1;
                    }
                    else
                    {
                        reviewCount.Add(restName, 1);
                    }
                }
                catch (Exception e)
                {

                }
            }

            var decSort2 = from rest in reviewCount orderby rest.Value descending select rest;
            List<string> deliverNames2 = new List<string>();
            List<int> deliverCounts2 = new List<int>();
            foreach (var item in decSort2)
            {
                deliverNames2.Add(item.Key);
                deliverCounts2.Add(item.Value);
            }

            ViewBag.names2 = deliverNames2.Take(6);
            ViewBag.counts2 = deliverCounts2.Take(6);

            //--------------------------------

            List<List<string>> restaurantList = new List<List<string>>();
            foreach (Restaurants r in restaurants)
            {
                try
                {
                    List<string> tempList = new List<string>();
                    tempList.Add(r.Latitude.ToString());
                    tempList.Add(r.Longitude.ToString());
                    tempList.Add(r.Id.ToString());
                    tempList.Add(r.Name);
                    restaurantList.Add(tempList);
                }
                catch (Exception e)
                {

                }
            }
            ViewBag.restaurants = restaurantList;

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