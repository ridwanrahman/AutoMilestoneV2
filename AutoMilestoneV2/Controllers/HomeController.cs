using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMilestoneV2.Models;
using Microsoft.AspNet.Identity;

namespace AutoMilestoneV2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                string currentUserId = User.Identity.GetUserId();
                using (var context = new Entities3())
                {
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