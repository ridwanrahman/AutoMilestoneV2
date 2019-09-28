using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMilestoneV2.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace AutoMilestoneV2.Controllers.Customer
{
    [RequireHttps]
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            
            using (var context = new Entities3())
            {
                //ViewBag.ItemData = db.Vehicles.ToList();
                ViewBag.ItemData = context.Vehicles.ToList();
            }
            return View();
        }

        [HttpPost]
        public JsonResult CreateBooking(string sendInfo)
        {
            string userId = User.Identity.GetUserId();
            string date_from;
            string date_to;
            string car_id;
            string latitude;
            string longitude;
            
            List<LocationModel> location = new List<LocationModel>();
            String[] spearator = { ","};
            String[] result = sendInfo.Split(spearator,StringSplitOptions.RemoveEmptyEntries);
            foreach (String s in result)
            {
                Console.WriteLine(result);
            }
            date_from = result[0].Trim('\t', '[','"');
            date_to = result[1].Trim('\t', '[', '"');

            //DateTime dt2 = DateTime.ParseExact(date_from,
            //            "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            //DateTime dt3 = DateTime.ParseExact(date_to,
            //            "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture);


            car_id = result[2].Trim('\t', '[', '"');
            /*
            CustomerBookingModel cs = new CustomerBookingModel()
            {
                userId = User.Identity.GetUserId(),
                vehicle_id = Int32.Parse(car_id),
                from_date = dt2,
                to_date = dt3,
                isAccepted = "false",
                pickup_location = "location1",
                dropoff_location = "location2"
            };
            */
            var vehicle_id = Int32.Parse(car_id);
            using(var context = new Entities3())
            {
                //context.CustomerBookings.Add(cs);
                //context.SaveChanges();

                context.Database.ExecuteSqlCommand("insert into " +
                    "[dbo].[CustomerBooking]([userId],[vehicle_id]," +
                    "[isAccepted],[to_date],[from_date],[pickup_location],[dropoff_location]) " +
                    "values('" + userId + "', '" + car_id + "', 'false', '" + date_from + "'," +
                    "'" + date_to + "','location1','location2')");
                var lastId = (from c in context.CustomerBookings where 
                          c.userId==userId && 
                          c.vehicle_id== vehicle_id
                          select c.customer_booking_id).ToArray();
                //int lastIdAfterConvertingToInt = Int32.Parse(lastId);
                for (int i = 3; i < result.Length - 1; i++)
                {
                    latitude = result[i];
                    int j = i;
                    longitude = result[j + 1];
                    context.Database.ExecuteSqlCommand("insert into " +
                        "[dbo].[CustomerBookingLocation](customer_booking_id,latitude,longitude)" +
                        "values('" + lastId[0] + "', '" + latitude+ "', '" + longitude + "')");
                }
            }
            return Json("success", JsonRequestBehavior.AllowGet);
        }
    }
}