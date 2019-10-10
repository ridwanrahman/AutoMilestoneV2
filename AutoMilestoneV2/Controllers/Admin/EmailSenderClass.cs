using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AutoMilestoneV2.Controllers.Admin
{
    public class EmailSenderClass
    {
        private const String API_KEY = "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";

        public void Send(String toEmail, String messageSubject, String messageBody, String fullPath)
        {
            if (fullPath=="nothing")
            {
                var client = new SendGridClient(API_KEY);
                var from = new EmailAddress("ridwanrahman07@gmail.com", "FIT5042 Email");
                var to = new EmailAddress(toEmail, "");
                var plainTextContent = messageBody;
                var htmlContent = "<p>" + plainTextContent + "</p>";
                var msg = MailHelper.CreateSingleEmail(from, to, messageSubject,
                                                        plainTextContent, htmlContent);
                var response = client.SendEmailAsync(msg);
            }
            else
            {
                var bytes = System.IO.File.ReadAllBytes(fullPath);
                var file = Convert.ToBase64String(bytes);

                var client = new SendGridClient(API_KEY);
                var from = new EmailAddress("ridwanrahman07@gmail.com", "FIT5042 Email");
                var to = new EmailAddress(toEmail, "");
                var plainTextContent = messageBody;
                var htmlContent = "<p>" + plainTextContent + "</p>";
                var msg = MailHelper.CreateSingleEmail(from, to, messageSubject,
                                                        plainTextContent, htmlContent);
                msg.AddAttachment("Attachment", file);
                var response = client.SendEmailAsync(msg);
            }
        }

    }
}