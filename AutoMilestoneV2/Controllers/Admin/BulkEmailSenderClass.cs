using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoMilestoneV2.Controllers.Admin
{
    public class BulkEmailSenderClass
    {
        private const String API_KEY = "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";
        
        public void send(String toEmail, String messageSubject, 
            String messageBody, String fullPath)
        {
            string[] emailAddresses = toEmail.Split(',');

            /*
            for(int i=0;i<emailAddresses.Length;i++)
            {
                var to_emails = new List<EmailAddress>
                {
                    new EmailAddress(emailAddresses[i], "Application")
                };
            };*/

            var client = new SendGridClient(API_KEY);

            
            var to_emails = new List<EmailAddress>();
            for(int i=0;i<emailAddresses.Length;i++)
            {
                to_emails.Add(new EmailAddress(emailAddresses[i], "User"));
            };

            var msg = new SendGridMessage();
            msg.SetFrom("ridwanrahman07@gmail.com", "FIT5032 BULK EMAIL");
            var plainTextContent = messageBody;
            var htmlContent = "<p>" + messageBody + "</p>";
            msg.PlainTextContent = plainTextContent;
            msg.Subject = messageSubject;
            msg.AddTos(to_emails);

            if (fullPath == "nothing")
            {
                //msg.AddAttachment(model.AttachedFile.FileName, file);                
                var response = client.SendEmailAsync(msg);
            }
            else
            {
                var bytes = System.IO.File.ReadAllBytes(fullPath);
                var file = Convert.ToBase64String(bytes);
                msg.AddAttachment("filename", file);
                var response = client.SendEmailAsync(msg);
            }

        }
    }
}