using Microsoft.AspNetCore.Http;
using System;
using MimeKit;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Chnage.Public_Classes
{
    public class Emailer
    {
        private readonly HttpContext _currentContext;
        private readonly string _hostname;

        public Emailer(HttpContext context)
        {
            _currentContext = context;
        }

        public Emailer(string hostname)
        {
            _hostname = hostname;
        }

        public void SendEmailNotification(string toName, string toEmail, string Message, string NotificationType, string ChangeType, int ChangeId)
        {
            var message = new MimeMessage();
            var messageBody = new BodyBuilder();
            string subject = "Notification for " + ChangeType + " - " + ChangeId;
            message.From.Add(new MailboxAddress("Engineering Change Notifications", "notificationsender@geotab.com"));
            message.To.Add(new MailboxAddress(toName, toEmail));
            string header = "<div style='color:darkblue'>" +
                        "<h2>" +
                            "Engineering Change Notification: " + "<a href='" + GetBaseUrl() + "/"
                                            + ChangeType + "s/Details/" + ChangeId.ToString() + "'target ='_blank'>"
                                            + ChangeType + " - " + ChangeId.ToString() + "</a>" +
                        "</h2>" +
                    "</div>" +
                    "<hr />";
            string bodyLast = "<h3>" +
                                "<p>" +
                                     "You are currently set to receive notifications for <i> " + NotificationType + " </i>. You can manage your settings through the manage notifications section from the homepage or by clicking below link" +
                                "</p>" +
                            "</h3>";
            //string linkToDetails = "<div>" +
            //                                "<a href='" + GetBaseUrl() + "/"
            //                                + ChangeType + "s/Details/" + ChangeId.ToString() + "'target ='_blank'>"
            //                                + ChangeType + " - " + ChangeId.ToString() + "</a>" +
            //                            "</div>"; 
            string linkToNotification = "<div>" +
                                        "<a href='" + GetBaseUrl() + "/"
                                        + "Notifications/" + "Manage" + "'target ='_blank'>"
                                            + "Manage Notifications" + "</a>" +
                                        "</div>";
            //var htmlContent = header+ Message + linkToDetails + bodyLast + linkToNotification;
            var htmlContent = header + Message + bodyLast + linkToNotification;
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

            var smtpClient = new SmtpClient
            {
                Credentials = new NetworkCredential("internalsender", "ev5%+#L3f6eY2Lxe?mSAMtAW7"),
                Host = "smtp.sendgrid.net",
                Port = 587
            };

            smtpClient.Send(mailMessage);
        }

        private string GetBaseUrl()
        {
            var request = _currentContext.Request;

            var host = request.Host.ToUriComponent();

            var pathBase = request.PathBase.ToUriComponent();
            var baseUrl = $"{request.Scheme}://{host}{pathBase}";
            return baseUrl;
        }
    }
}
