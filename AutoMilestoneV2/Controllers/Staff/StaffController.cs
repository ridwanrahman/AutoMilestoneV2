using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using AutoMilestoneV2.Models;
using Microsoft.AspNet.Identity;


namespace AutoMilestoneV2.Controllers.Staff
{
    public class StaffController : Controller
    {
        // GET: Staff
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            return View();
        }

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
                using(Entities2 db = new Entities2())
                {
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
    }
}