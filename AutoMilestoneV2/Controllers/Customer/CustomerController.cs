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

        public JsonResult CheckBookingDate(string inputDates)
        {
            Console.WriteLine(inputDates);
            return Json("success", JsonRequestBehavior.AllowGet);
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

            car_id = result[2].Trim('\t', '[', '"');
            var vehicle_id = Int32.Parse(car_id);
            try
            {
                using (var context = new Entities3())
                {
                    context.Database.ExecuteSqlCommand("insert into " +
                        "[dbo].[CustomerBooking]([userId],[vehicle_id]," +
                        "[isAccepted],[to_date],[from_date],[pickup_location],[dropoff_location]) " +
                        "values('" + userId + "', '" + car_id + "', 'false', '" + date_from + "'," +
                        "'" + date_to + "','location1','location2')");
                    var lastId = (from c in context.CustomerBookings
                                  where c.userId == userId && c.vehicle_id == vehicle_id
                                  select c.customer_booking_id).ToArray();
                    for (int i = 3; i <= result.Length - 2; i = i + 2)
                    {
                        latitude = result[i].Trim('[', ']');
                        int j = i;
                        longitude = result[j + 1].Trim('[', ']');
                        context.Database.ExecuteSqlCommand("insert into " +
                            "[dbo].[CustomerBookingLocation](customer_booking_id,latitude,longitude)" +
                            "values('" + lastId[0] + "', '" + latitude + "', '" + longitude + "')");
                    }
                }
            }
            catch (Exception e)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            return Json("success", JsonRequestBehavior.AllowGet);
        }
    }
}