using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Chnage.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Mail;

namespace Controllers
{
    public class ErrorHandlerController : Controller
    {
        //private readonly MyECODBContext _context;
        public ErrorHandlerController()//MyECODBContext context)
        {
            //_context = context;
        }

        public IActionResult ErrorPage(ViewModelError viewModelError)
        {            
            SendEmail(viewModelError);
            return View(viewModelError);
        }

        public IActionResult ErrorMessagePage(string Message)
        {
            ViewData["Message"] = Message;
            return View();
        }

        public void SendEmail(ViewModelError viewModel)
        {
            var message = new MimeMessage();
            var messageBody = new BodyBuilder();
            string toName = "Rutvij Sharma";
            string toEmail = viewModel.Email;
            string body = viewModel.GenerateEmailBody();
            //body += "<h4> User: " + HttpContext.User.Identity.Name + "</h4>";
            string subject = viewModel.ExceptionTypeString +" - " + DateTime.Now.ToString();
            message.From.Add(new MailboxAddress("Exception Notification", "notificationsender@gmail.com"));
            message.To.Add(new MailboxAddress(toName, toEmail));
            var htmlContent = body;
            messageBody.HtmlBody = htmlContent;
            message.Body = messageBody.ToMessageBody();
            var mailMessage = new MailMessage
            {
                From = new MailAddress("notificationsender@gmail.com"),
                Subject = subject,
                Body = htmlContent,
                IsBodyHtml = true

            };
            mailMessage.To.Add(toEmail);

            var smtpClient = new System.Net.Mail.SmtpClient
            {
                Credentials = new NetworkCredential("internalsender", "ev5%+#L3f6eY2Lxe?mSAMtAW7"),
                Host = "smtp.sendgrid.net",
                Port = 587
            };

            smtpClient.Send(mailMessage);
        }
    }
}