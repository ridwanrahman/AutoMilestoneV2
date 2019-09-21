using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMilestoneV2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace AutoMilestoneV2.Controllers.Admin
{
    public class AdminController : Controller
    {        
        // GET: Admin
        public ActionResult Index()
        {
            
            
            return View();
        }
    }
}