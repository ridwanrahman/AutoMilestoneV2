using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMilestoneV2.Models;

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
    }
}