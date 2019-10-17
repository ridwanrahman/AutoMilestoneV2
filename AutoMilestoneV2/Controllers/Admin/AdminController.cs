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

namespace AutoMilestoneV2.Controllers.Admin
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {        
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult AddStaff()
        {
            ViewBag.Message = "hi";
            return View();
        }

        [HttpPost]
        public ActionResult AddStaff(RegisterViewModel newModel)
        {
            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);

            var user = new IdentityUser() { Email = newModel.Email, UserName=newModel.Email };
            IdentityResult result = manager.Create(user, newModel.Password);
            try
            {
                using (Entities3 db = new Entities3())
                {
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

        public ActionResult SendEmail()
        {
            return View();
        }

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
                    String to = emailMessage.messageTo;
                    String messageSubject = emailMessage.messageSubject;
                    String messageBody = emailMessage.messageBody;
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
        
        public ActionResult SendBulkEmail()
        {
            using (var context = new Entities3())
            {
                var req = (from u in context.AspNetUsers select u.Email).ToList();
                ViewBag.emails = req;
            }
            return View();
        }

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