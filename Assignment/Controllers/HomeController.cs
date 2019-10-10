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

            List<string> deliverNames2 = new List<string>();
            List<int> deliverCounts2 = new List<int>();
            List<string> deliverNames3 = new List<string>();
            List<int> deliverCounts3 = new List<int>();
            for (int i=0;i< deliverNames.Count;i++)
            {
                if (i>5 & i<12)
                {
                    deliverNames2.Add(deliverNames[i]);
                    deliverCounts2.Add(deliverCounts[i]);
                }
                if (i>11 & i<18)
                {
                    deliverNames3.Add(deliverNames[i]);
                    deliverCounts3.Add(deliverCounts[i]);
                }
                if (i>17)
                {
                    break;
                }
            }

            ViewBag.names1 = deliverNames.Take(6);
            ViewBag.counts1 = deliverCounts.Take(6);
            ViewBag.names2 = deliverNames2;
            ViewBag.counts2 = deliverCounts2;
            ViewBag.names3 = deliverNames3;
            ViewBag.counts3 = deliverCounts3;

            //--------------------------

            var reviews = db.Reviews.ToList();

            Dictionary<string, int> reviewCount = new Dictionary<string, int>();
            Dictionary<string, int> reviewRate = new Dictionary<string, int>();

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
                        reviewRate[restName] = reviewRate[restName] + Int32.Parse(r.Rate);
                    }
                    else
                    {
                        reviewCount.Add(restName, 1);
                        reviewRate.Add(restName, Int32.Parse(r.Rate));
                    }
                }
                catch (Exception e)
                {

                }
            }

            foreach (string key in reviewCount.Keys)
            {
                reviewRate[key] = reviewRate[key] / reviewCount[key];
            }

            var decSort_secondGraph = from rest in reviewRate orderby rest.Value descending select rest;
            List<string> deliverNames_secondGraph = new List<string>();
            List<int> deliverCounts_secondGraph = new List<int>();
            foreach (var item in decSort_secondGraph)
            {
                deliverNames_secondGraph.Add(item.Key);
                deliverCounts_secondGraph.Add(item.Value);
            }

            ViewBag.names_secondGraph = deliverNames_secondGraph.Take(6);
            ViewBag.counts_secondGraph = deliverCounts_secondGraph.Take(6);

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