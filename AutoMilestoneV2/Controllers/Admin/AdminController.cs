﻿using System;
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
        public async Task<ActionResult> SendEmailAsync(EmailViewModel emailMessage)
        {
            Console.WriteLine(emailMessage);
            try
            {
                String to = emailMessage.messageTo;
                String messageSubject = emailMessage.messageSubject;
                String messageBody = emailMessage.messageBody;
                EmailSenderClass es = new EmailSenderClass();
                // var statusCode = await es.Send(to, messageSubject, messageBody);
                es.Send(to, messageSubject, messageBody);
                ViewBag.Result = "success";
            }
            catch(Exception e)
            {
                
            }
            return View();
        }
    }
}

//api key: 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