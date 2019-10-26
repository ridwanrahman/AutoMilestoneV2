using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AutoMilestoneV2.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// This is the customer controller. It takes care of the customer dashboard
namespace AutoMilestoneV2.Controllers.Customer
{
    [Authorize(Roles = "Customer")]
    [RequireHttps]
    public class CustomerController : Controller
    {
        // This loads the index page
        public ActionResult Index2()
        {
            return View();
        }
        [Authorize(Roles = "Customer")]
        // GET: Customer
        // This controller loads the index page with data from the database.
        public ActionResult Index()
        {
            
            using (var context = new Entities3())
            {
                //ViewBag.ItemData = db.Vehicles.ToList();
                ViewBag.ItemData = context.Vehicles.ToList();
            }
            return View();
        }
        // This controller handles the booking functionality.
        // When a user selects a car and selects from and to dates, through the use of ajax,
        // this function is called. This function checks the database by running a linq command
        // to check if the same car is already booked on the given dates or not. It then sends
        // a json response accordingly. The reponse determines if the user can create booking or not
        [AllowAnonymous]
        [HttpPost]
        public JsonResult CheckBookingDate(string inputDates)
        {
            var response = "";
            String[] spearator = { "," };
            String[] result = inputDates.Split(spearator, StringSplitOptions.RemoveEmptyEntries);
            int car_id = Int32.Parse(result[0].Trim('\t', '"'));
            DateTime date_from = Convert.ToDateTime(result[1].Trim('\t', '"'));
            DateTime date_to = Convert.ToDateTime(result[2].Trim('\t', '"'));
            var date_from_converted = date_from.ToString("yyyy-MM-dd");
            var date_to_converted = date_to.ToString("yyyy-MM-dd");
            DateTime date_from_date = Convert.ToDateTime(date_from_converted);
            DateTime date_to_date = Convert.ToDateTime(date_to_converted);
            using(var context = new Entities3())
            {
                try
                {
                    // Linq command where my booking constraint is implemented
                    var isBooked = (from c in context.CustomerBookings
                                    where ((c.from_date >= date_from_date && c.from_date <= date_to_date) ||
                                            (c.to_date >= date_to_date && c.to_date <= date_to_date) ||
                                                (c.from_date <= date_from_date && c.to_date >= date_to_date)) &&
                                    c.vehicle_id == car_id
                                    select c.vehicle_id).ToList();
                    Console.WriteLine(isBooked);
                    if (isBooked.Count > 0)
                    {
                        response = "already booked";
                    }
                    else
                    {
                        response = "not booked";
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        // This is the controller which creates the booking for a customer.
        [AllowAnonymous]
        [HttpPost]
        public JsonResult CreateBooking(string sendInfo)
        {
            string userId = User.Identity.GetUserId();
            //string date_from;
            //string date_to;
            string car_id;
            string latitude;
            string longitude;
            double distance = 0.0;
            double price = 0.0;
            
            List<LocationModel> location = new List<LocationModel>();
            String[] spearator = { ","};
            String[] result = sendInfo.Split(spearator,StringSplitOptions.RemoveEmptyEntries);
            foreach (String s in result)
            {
                Console.WriteLine(result);
            }

            DateTime date_from = Convert.ToDateTime(result[0].Trim('\t', '[', '"'));
            DateTime date_to = Convert.ToDateTime(result[1].Trim('\t', '[', '"'));
            var date_from_converted = date_from.ToString("yyyy-MM-dd");
            var date_to_converted = date_to.ToString("yyyy-MM-dd");
            DateTime date_from_date = Convert.ToDateTime(date_from_converted);
            DateTime date_to_date = Convert.ToDateTime(date_to_converted);

            car_id = result[2].Trim('\t', '[', '"');
            var abc = result[3].Trim('\t', '[', '"');
            distance = Convert.ToDouble(abc);
            var vehicle_id = Int32.Parse(car_id);
            price = distance * 3;
            try
            {
                using (var context = new Entities3())
                {
                    // this SQL command is executed to insert the booking into the database
                    context.Database.ExecuteSqlCommand("insert into " +
                        "[dbo].[CustomerBooking]([userId],[vehicle_id]," +
                        "[isAccepted],[to_date],[from_date],[pickup_location],[dropoff_location],[distance],[price]) " +
                        "values('" + userId + "', '" + car_id + "', 'false', '" + date_to_converted + "'," +
                        "'" + date_from_converted + "','location1','location2', '"+distance+"', '"+price+"')");
                    var lastId = (from c in context.CustomerBookings
                                  where c.userId == userId && c.vehicle_id == vehicle_id
                                  select c.customer_booking_id).ToArray();
                    for (int i = 4; i <= result.Length - 2; i = i + 2)
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
            CustomerResponse cs = new CustomerResponse();
            cs.response = "success";
            cs.distance = distance;
            cs.price = price;
            cs.message = "Thank you for using our services. Our staff will be in contact with you soon.";

            JavaScriptSerializer js = new JavaScriptSerializer();
            var json = js.Serialize(cs);
            
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}