﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using AutoMilestoneV2.Models;
using Microsoft.AspNet.Identity;

// This is the controller which takes care of all the staff functionalities.

namespace AutoMilestoneV2.Controllers.Staff
{
    public class StaffController : Controller
    {
        [Authorize(Roles ="Staff")]
        // GET: Staff
        public ActionResult Index()
        {
            // gets the current user
            string userId = User.Identity.GetUserId();
            using (var context = new Entities3())
            {
                // collects all the staff data and puts them inside ViewBag.
                var viewModel = (from u in context.AspNetUsers join c in context.CustomerBookings
                                 on u.Id equals c.userId
                                 select new StaffViewCustomerBookingInDashboard() { Id=u.Id, Email=u.Email,
                                 customer_booking_id=c.customer_booking_id,from_date=c.from_date,
                                 to_date=c.to_date,vehicle_id=c.vehicle_id}).ToList();
                ViewBag.itemData = viewModel;
                return View();
            }
        }

        [Authorize(Roles = "Staff")]
        public ActionResult AddVehicle()
        {
            string userId = User.Identity.GetUserId();
            return View();
        }

        [HttpPost]
        public ActionResult AddVehicle(StaffCarUploadModel newUpload)
        {
            //bd44b938 - 8f13 - 45c4 - 96ee - 08dd988b5d9c
            string path = Server.MapPath("~/App_Data/File");
            string fileName = Path.GetFileName(newUpload.carPicture.FileName);
            string fullPath = Path.Combine(path, fileName);
            string userId = User.Identity.GetUserId();
            try
            {
                using(Entities3 db = new Entities3())
                {
                    // This sql command inserts a vehicle into the database.
                    db.Database.ExecuteSqlCommand("insert into [dbo].[Vehicle] ([Name],[Model],[userId],[image_path]) Values ('"+newUpload.name+"', '"+newUpload.model+"', '"+userId+"', '"+ fullPath + "');");
                }
                newUpload.carPicture.SaveAs(fullPath);
                ViewBag.Message = "success";
            }
            catch
            {
                ViewBag.Message = "error";
            }

            return View();
        }

        public ActionResult ViewOrders()
        {
            return View();
        }

        public ActionResult ViewFleet()
        {
            Entities3 db = new Entities3();
            ViewBag.first = db.AspNetUsers.ToList();
            ViewBag.ItemData = db.Vehicles.ToList();
            return View();
        }
        [Authorize(Roles = "Staff")]
        public ActionResult ShowDetails(string query, int query2)
        {
            using (var context = new Entities3())
            {
                // for implementing the functionality of staff member checking booking details
                var customerDetails = (from u in context.AspNetUsers
                                   where u.Id == query
                                   select new CustomerModel() { Id=u.Id, Email=u.Email}).ToArray();
                var customerBookingDetails = (from bd in context.CustomerBookings
                                              where bd.customer_booking_id==query2
                                              select new 
                                              CustomerBookingModelForStaff() { customer_booking_id=bd.customer_booking_id,
                                              vehicle_id=bd.vehicle_id, to_date=bd.to_date,
                                              from_date=bd.from_date}).ToList();

                var customerBookingLocations = (from c in context.CustomerBookingLocations
                                                where c.customer_booking_id==query2
                                                select new CustomerBookingLocationForStaff() {
                                                longitude=c.longitude,latitude=c.latitude}).ToList();
                ViewBag.customerDetails = customerDetails;
                ViewBag.customerBookingDetails = customerBookingDetails;
                ViewBag.customerBookingLocations = customerBookingLocations;
                return View();
            }
            
        }

        public EmptyResult DeleteLocations()
        {
            using(var context = new Entities3())
            {

                
            }
            return null;
           // return View();
            
        }
    }
}