using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMilestoneV2.Models;
using Microsoft.AspNet.Identity;

// This is the main home controller which takes care of loading up the homepage.

namespace AutoMilestoneV2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // checks if the user is authenticated
            if(User.Identity.IsAuthenticated)
            {
                //gets the current user
                string currentUserId = User.Identity.GetUserId();
                using (var context = new Entities3())
                {
                    // finds the role of the user. This is done to show specific navigation bar
                    // to specific roles of users.
                    var result = (from u in context.AspNetUsers
                                  join ur in context.userrolesbridgings on u.Id equals ur.UserId
                                  join ro in context.AspNetRoles on
                                  ur.RoleId equals ro.Id
                                  where u.Id == currentUserId
                                  select ro.Name).ToArray();
                    ViewBag.message = result[0];
                }
            }
            else
            {
                ViewBag.message = "nothing";
            }
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