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
using AutoMilestoneV2.Controllers;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System.IO;
using Ganss.XSS;
using System.Web.Script.Serialization;

// This is the admin controller
namespace AutoMilestoneV2.Controllers.Admin
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {        
        // GET: Admin
        // This controller just loads up the index page.
        public ActionResult Index()
        {
            return View();
        }
        // This controller shows the add staff page where the admin can add staff members.
        public ActionResult AddStaff()
        {
            ViewBag.Message = "hi";
            return View();
        }

        // This is the function that is called when the staff page is submitted with the
        // staff username and password. It manipulates the database and executes a sql statement
        // to save the staff member to the database. Then sends a succcess message to the page.
        [HttpPost]
        public ActionResult AddStaff(RegisterViewModel newModel)
        {
            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);

            var user = new IdentityUser() { Email = newModel.Email, UserName=newModel.Email };
            IdentityResult result = manager.Create(user, newModel.Password);
            try
            {
                // traversing the database
                using (Entities3 db = new Entities3())
                {
                    // executing SQL comman
                    db.Database.ExecuteSqlCommand("insert into [dbo].[userrolesbridging]([UserId], [RoleId]) values ('" + user.Id + "',2);");
                }
                ModelState.Clear();
                ViewBag.Message = "success";
                return View();
            }
            catch
            {
                ViewBag.Message = "error";
                return View();
            }
        }
        // This controller opens the email page
        public ActionResult SendEmail()
        {
            return View();
        }
        // This controller reads the database to find the number of staff members and customers
        // then sends it to the front end of the site which uses ajax to call this controller
        // The data from this controller is used to generate a chart to show the number of staff
        // and customer active in the database.

        [AllowAnonymous]
        public JsonResult ShowAnalytics()
        {
            AdminAnalyticsUserAmount userAnalytics = new AdminAnalyticsUserAmount();
            using (var context = new Entities3())
            {
                string num = "2";
                string num2 = "3";
                // Linq statement to find data for analytics
                userAnalytics.customerNumber  = (from u in context.AspNetUsers join roles in context.userrolesbridgings
                                     on u.Id equals roles.UserId where roles.RoleId == num2 select u.Id).ToList().Count();
                userAnalytics.staffNumber = (from u in context.AspNetUsers join roles in context.userrolesbridgings on u.Id equals roles.UserId
                                     where roles.RoleId == num select u.Id).ToList().Count();
            }
            // serializing the object to make it easier to send it by json response
            JavaScriptSerializer js = new JavaScriptSerializer();
            var json = js.Serialize(userAnalytics);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        // This controller was written to find number of vehicles present in the database
        // and how much distance they have travelled.
        [AllowAnonymous]

        public JsonResult VehicleAnalytics()
        {
            /*
            using (var context = new Entities3())
            {
                //AdminAnalyticsVehicle vehicleAnalytics = new AdminAnalyticsVehicle();
                List<AdminAnalyticsVehicle> vehicleList = new List<AdminAnalyticsVehicle>();
                var req = (from v in context.Vehicles select v.Id).ToList();
                foreach (int i in req)
                {
                    vehicleAnalytics.vehicle_id = i;
                    Console.WriteLine(i);
                    int counter = 0;
                    var eachCar = (from c in context.CustomerBookings
                                   where c.vehicle_id == i
                                   select c.distance).ToList();
                    foreach (int j in eachCar)
                    {
                        counter = counter + j;
                    }
                    vehicleAnalytics.distance = counter;
                }
                Console.WriteLine(vehicleAnalytics);
            }
            */
            return null;
        }

        // This controller deals with sending email. It takes the email address, subject, message
        // and an optional file field and sends it to the email address provided.
        [HttpPost]
        public ActionResult SendEmail(EmailViewModel emailMessage)
        {
            if(emailMessage.messageTo == null || emailMessage.messageSubject == null ||
                emailMessage.messageBody == null)
            {
                ViewBag.Result = "error";
                return View();
            }
            else
            {
                try
                {
                    var sanitizer = new HtmlSanitizer();

                    //var html = @"" + emailMessage.messageBody + "";
                    emailMessage.messageBody = @"<p><script>alert('xss')</script>dell</p>";
                    String to = emailMessage.messageTo;
                    String messageSubject = emailMessage.messageSubject;
                    String messageBody = emailMessage.messageBody;

                    var sanitized = sanitizer.Sanitize(messageBody);

                    EmailSenderClass es = new EmailSenderClass();
                    if(emailMessage.attachment != null)
                    {
                        string path = Server.MapPath("~/App_Data/File");
                        string fileName = Path.GetFileName(emailMessage.attachment.FileName);
                        string fullPath = Path.Combine(path, fileName);
                        emailMessage.attachment.SaveAs(fullPath);
                        es.Send(to, messageSubject, messageBody, fullPath);
                    }
                    else
                    {
                        es.Send(to, messageSubject, messageBody, "nothing");
                    }
                    
                    ViewBag.Result = "success";
                }
                catch (Exception e)
                {
                    ViewBag.Result = "error";
                }
                return View();
            }
        }
        // This controller opens the bulk email page

        public ActionResult SendBulkEmail()
        {
            using (var context = new Entities3())
            {
                var req = (from u in context.AspNetUsers select u.Email).ToList();
                ViewBag.emails = req;
            }
            return View();
        }
        // This is the function which takes care of the bulk email feature. It reads the user
        // database and sends emails to all the email addresses present.
        [HttpPost]
        public ActionResult SendBulkEmail(BulkEmailViewModel emailMessage)
        {
            if (emailMessage.messageSubject == null ||
                emailMessage.messageBody == null)
            {
                ViewBag.Result = "error";
                return View();
            }
            else
            {
                try
                {
                    String to = "";
                    using (var context = new Entities3())
                    {
                        var req = (from u in context.AspNetUsers select u.Email).ToList();
                        to = req[0];
                        for(int i=1;i<req.Count();i++)
                        {
                            to = to + "," + req[i];
                        }
                    }
                    String messageSubject = emailMessage.messageSubject;
                    String messageBody = emailMessage.messageBody;
                    BulkEmailSenderClass bs = new BulkEmailSenderClass();
                                        
                    if(emailMessage.attachment != null)
                    {
                        string path = Server.MapPath("~/App_Data/File");
                        string fileName = Path.GetFileName(emailMessage.attachment.FileName);
                        string fullPath = Path.Combine(path, fileName);
                        emailMessage.attachment.SaveAs(fullPath);
                        bs.send(to, messageSubject, messageBody, fullPath, fileName);
                    }
                    else
                    {
                        bs.send(to, messageSubject, messageBody, "nothing", "nothing");
                    }
                    ModelState.Clear();
                    ViewBag.Result = "success";
                    return View();
                }
                catch (Exception e)
                {
                    ViewBag.Result = "error";
                    return View();
                }
            }
        }
    }
}
//api key: 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