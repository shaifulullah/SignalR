using Chnage.Models;
using Chnage.Public_Classes;
using Chnage.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Repository
{
    public class NotificationSenderRepository : INotificationSender
    {
        private readonly MyECODBContext db;
        private readonly Emailer emailer;
        private string message;
        List<string> changes;
        public NotificationSenderRepository(MyECODBContext _db, IHttpContextAccessor httpContextAccessor)
        {
            db = _db;
            emailer = new Emailer(httpContextAccessor.HttpContext);
        }

        public string MessageBuilder(string ChangeType, int ChangeId, string NotificationType, string optionalMessage = "")
        {
            string message = "";
            if (changes != null)
            {
                foreach (var change in changes)
                {
                    message += change + "<br/>";
                }
            }

            //string header = "<div style='color:lightblue'>" +
            //            "<h2>" +
            //                "Engineering Change Notification: " + ChangeType + " - " + ChangeId.ToString() +
            //            "</h2>" +
            //        "</div>" +
            //        "<hr />";

            string body;
            switch (NotificationType)
            {
                case "on any change":
                    body = "<div>" +
                                "<h3>" +
                                    "<p>" +
                                    "You are receiving this email because there has been a change to an Engineering Change and you are in the Notification list." +
                                     "<br/>" +
                                    "These are the changes that were made: " +
                                    "<br/>"
                                    + message +
                                    "</p>" +
                                "</h3>" +
                           "</div>";
                    break;
                case "on approval change":
                    body = "<div>" +
                                "<h3>" +
                                    "<p>" +
                                    "You are receiving this email because there has been a change to an Engineering Change and you are in the Notification list." +
                                    "</p>" +
                                    "<p>" + optionalMessage + "</p>" +
                                "</h3>" +
                           "</div>";
                    break;
                case "on approvers change":
                    body = "<div>" +
                                "<h3>" +
                                    "<p>" +
                                    "You are receiving this email because there has been a change to an Engineering Change and you are in the Notification list." +
                                    "</p>" +
                                    "<p>" + optionalMessage + "</p>" +
                                "</h3>" +
                           "</div>";
                    break;
                case "on description change":
                    body = "<div>" +
                                "<h3>" +
                                    "<p>" +
                                    "You are receiving this email because there has been a change to an Engineering Change and you are in the Notification list." +
                                    "</p>" +
                                    "<p>" + optionalMessage + "</p>" +
                                "</h3>" +
                           "</div>";
                    break;
                case "on implementation date change":
                    body = "<div>" +
                                "<h3>" +
                                    "<p>" +
                                    "You are receiving this email because there has been a change to an Engineering Change and you are in the Notification list." +
                                    "</p>" +
                                    "<p>" + optionalMessage + "</p>" +
                                "</h3>" +
                           "</div>";
                    break;
                case "on reason change":
                    body = "<div>" +
                                "<h3>" +
                                    "<p>" +
                                    "You are receiving this email because there has been a change to an Engineering Change and you are in the Notification list." +
                                    "</p>" +
                                    "<p>" + optionalMessage + "</p>" +
                                "</h3>" +
                           "</div>";
                    break;
                case "on status change":
                    body = "<div>" +
                                "<h3>" +
                                    "<p>" +
                                    "You are receiving this email because there has been a change to an Engineering Change and you are in the Notification list." +
                                    "</p>" +
                                    "<p>" + optionalMessage + "</p>" +
                                "</h3>" +
                           "</div>";
                    break;
                case "reminder email":
                    body = "<div>" +
                                "<h3>" +
                                    "<p>" +
                                    "You are receiving this email because the originator of this Engineering Chnage wants to send you a reminder ." +
                                    "</p>" +
                                    "<p>" + optionalMessage + "</p>" +
                                "</h3>" +
                           "</div>";
                    break;
                default:
                    body = "<div>" +
                                "<h3>" +
                                    "<p>" +
                                    "You are receiving this email because there has been a change to an Engineering Change and you are in the Notification list." +
                                    "</p>" +
                                    "<p>" + optionalMessage + "</p>" +
                                "</h3>" +
                           "</div>";
                    break;
            }

            string returnMessage = /*header +*/ body;
            return returnMessage;
        }

        //removed all throw new NotImplementedException
        //figure something for the messages. same types will have the same message, only changing the Change Type in the message.

        #region Any Change
        public void SendNotificationOnAnyChangeECN(int ECNId, List<string> msg)
        {
            IQueryable<Notifications> AnyChange = db.Notifications.Where(n => n.ECNId == ECNId && n.Option == NotificationOption.AnyChange).Include(c => c.User);
            changes = msg;
            message = MessageBuilder("ECN", ECNId, "on any change");
            foreach (Notifications notification in AnyChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "on any change", "ECN", ECNId);
            }
        }

        public void SendNotificationOnAnyChangeECO(int ECOId, List<string> msg)
        {
            IQueryable<Notifications> AnyChange = db.Notifications.Where(n => n.ECOId == ECOId && n.Option == NotificationOption.AnyChange).Include(c => c.User);
            changes = msg;
            message = MessageBuilder("ECO", ECOId, "on any change");
            foreach (Notifications notification in AnyChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "on any change", "ECO", ECOId);
            }
        }

        public void SendNotificationOnAnyChangeECR(int ECRId, List<string> msg)
        {
            IQueryable<Notifications> AnyChange = db.Notifications.Where(n => n.ECRId == ECRId && n.Option == NotificationOption.AnyChange).Include(c => c.User);
            changes = msg;
            message = MessageBuilder("ECR", ECRId, "on any change");
            foreach (Notifications notification in AnyChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "on any change", "ECR", ECRId);
            }
        }
        #endregion
        #region On Approval Change
        public void SendNotificationOnApprovalECN(int ECNId, string chnagedDetails)
        {
            IQueryable<Notifications> Approval = db.Notifications.Where(n => n.ECNId == ECNId && n.Option == NotificationOption.Approval).Include(c => c.User);
            message = MessageBuilder("ECN", ECNId, "on approval change", chnagedDetails);
            foreach (Notifications notification in Approval)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "on approval change", "ECN", ECNId);
            }
        }

        public void SendNotificationOnApprovalECO(int ECOId, string chnagedDetails)
        {
            IQueryable<Notifications> ApproversChange = db.Notifications.Where(n => n.ECOId == ECOId && n.Option == NotificationOption.Approval).Include(c => c.User);
            message = MessageBuilder("ECO", ECOId, "on approval change", chnagedDetails);
            foreach (Notifications notification in ApproversChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "on approval change", "ECO", ECOId);
            }
        }

        public void SendNotificationOnApprovalECR(int ECRId, string chnagedDetails)
        {
            IQueryable<Notifications> ApproversChange = db.Notifications.Where(n => n.ECRId == ECRId && n.Option == NotificationOption.Approval).Include(c => c.User);
            message = MessageBuilder("ECR", ECRId, "on approval change", chnagedDetails);
            foreach (Notifications notification in ApproversChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "on approval change", "ECR", ECRId);
            }
        }
        #endregion
        #region On Approvers Change
        public void SendNotificationOnApproversChangeECN(int ECNId, List<string> approverChanges)
        {
            IQueryable<Notifications> ApproversChange = db.Notifications.Where(n => n.ECNId == ECNId && n.Option == NotificationOption.ApproversChange).Include(c => c.User);
            var approverChangesString = string.Join("<br/>", approverChanges);
            message = MessageBuilder("ECN", ECNId, "on approvers change", approverChangesString);
            foreach (Notifications notification in ApproversChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "on approvers change", "ECN", ECNId);
            }
        }

        public void SendNotificationOnApproversChangeECO(int ECOId, List<string> approverChanges)
        {
            IQueryable<Notifications> ApproversChange = db.Notifications.Where(n => n.ECOId == ECOId && n.Option == NotificationOption.ApproversChange).Include(c => c.User);
            var approverChangesString = string.Join("<br/>", approverChanges);
            message = MessageBuilder("ECO", ECOId, "on approvers change", approverChangesString);
            foreach (Notifications notification in ApproversChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "on approvers change", "ECO", ECOId);
            }
        }

        public void SendNotificationOnApproversChangeECR(int ECRId, List<string> approverChanges)
        {
            IQueryable<Notifications> ApproversChange = db.Notifications.Where(n => n.ECRId == ECRId && n.Option == NotificationOption.ApproversChange).Include(c => c.User);
            var approverChangesString = string.Join("<br/>", approverChanges);
            message = MessageBuilder("ECR", ECRId, "on approvers change", approverChangesString);
            foreach (Notifications notification in ApproversChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "on approvers change", "ECR", ECRId);
            }
        }
        #endregion
        #region On Description Change
        public void SendNotificationOnDescriptionChangeECN(int ECNId, string changedDetails)
        {
            IQueryable<Notifications> DescChange = db.Notifications.Where(n => n.ECNId == ECNId && n.Option == NotificationOption.DescChange).Include(c => c.User);
            message = MessageBuilder("ECN", ECNId, "on description change", changedDetails);
            foreach (Notifications notification in DescChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "on description change", "ECN", ECNId);
            }
        }

        public void SendNotificationOnDescriptionChangeECO(int ECOId, string changedDetails)
        {
            IQueryable<Notifications> DescChange = db.Notifications.Where(n => n.ECOId == ECOId && n.Option == NotificationOption.DescChange).Include(c => c.User);
            message = MessageBuilder("ECO", ECOId, "on description change", changedDetails);
            foreach (Notifications notification in DescChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "on description change", "ECO", ECOId);
            }
        }

        public void SendNotificationOnDescriptionChangeECR(int ECRId, string changedDetails)
        {
            IQueryable<Notifications> DescChange = db.Notifications.Where(n => n.ECRId == ECRId && n.Option == NotificationOption.DescChange).Include(c => c.User);
            message = MessageBuilder("ECR", ECRId, "on description change", changedDetails);
            foreach (Notifications notification in DescChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "on description change", "ECR", ECRId);
            }
        }
        #endregion
        #region On Implementation Date Change
        public void SendNotificationOnImplementationDateChangeECN(int ECNId)
        {
            IQueryable<Notifications> ImpDateChange = db.Notifications.Where(n => n.ECNId == ECNId && n.Option == NotificationOption.ImpDateChange).Include(c => c.User);
            message = MessageBuilder("ECN", ECNId, "on implementation date change");
            foreach (Notifications notification in ImpDateChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "on implementation date change", "ECN", ECNId);
            }
        }

        public void SendNotificationOnImplementationDateChangeECO(int ECOId, string changedDetails)
        {
            IQueryable<Notifications> ImpDateChange = db.Notifications.Where(n => n.ECOId == ECOId && n.Option == NotificationOption.ImpDateChange).Include(c => c.User);
            message = MessageBuilder("ECO", ECOId, "on implementation date change", changedDetails);
            foreach (Notifications notification in ImpDateChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "on implementation date change", "ECO", ECOId);
            }
        }

        public void SendNotificationOnImplementationDateChangeECR(int ECRId, string changedDetails)
        {
            IQueryable<Notifications> ImpDateChange = db.Notifications.Where(n => n.ECRId == ECRId && n.Option == NotificationOption.ImpDateChange).Include(c => c.User);
            message = MessageBuilder("ECR", ECRId, "on implementation date change", changedDetails);
            foreach (Notifications notification in ImpDateChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "on implementation date change", "ECR", ECRId);
            }
        }
        #endregion
        #region On Reason Change
        public void SendNotificationOnReasonChangeECN(int ECNId)
        {
            IQueryable<Notifications> StatusChange = db.Notifications.Where(n => n.ECNId == ECNId && n.Option == NotificationOption.ReasonChange).Include(c => c.User);
            message = MessageBuilder("ECN", ECNId, "on reason change");
            foreach (Notifications notification in StatusChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "on reason change", "ECN", ECNId);
            }
        }

        public void SendNotificationOnReasonChangeECO(int ECOId, string changedDetails)
        {
            IQueryable<Notifications> StatusChange = db.Notifications.Where(n => n.ECOId == ECOId && n.Option == NotificationOption.ReasonChange).Include(c => c.User);
            message = MessageBuilder("ECO", ECOId, "on reason change", changedDetails);
            foreach (Notifications notification in StatusChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "on reason change", "ECO", ECOId);
            }
        }

        public void SendNotificationOnReasonChangeECR(int ECRId, string changedDetails)
        {
            IQueryable<Notifications> StatusChange = db.Notifications.Where(n => n.ECRId == ECRId && n.Option == NotificationOption.ReasonChange).Include(c => c.User);
            message = MessageBuilder("ECR", ECRId, "on reason change", changedDetails);
            foreach (Notifications notification in StatusChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "on reason change", "ECR", ECRId);
            }
        }
        #endregion
        #region On Status Change
        public void SendNotificationOnStatusChangeECN(int ECNId, string changedDetails)
        {
            IQueryable<Notifications> StatusChange = db.Notifications.Where(n => n.ECNId == ECNId && n.Option == NotificationOption.StatusChange).Include(c => c.User);
            message = MessageBuilder("ECN", ECNId, "on status change", changedDetails);
            foreach (Notifications notification in StatusChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "on status change", "ECN", ECNId);
            }
        }

        public void SendNotificationOnStatusChangeECO(int ECOId, string chnagedDetails)
        {
            IQueryable<Notifications> StatusChange = db.Notifications.Where(n => n.ECOId == ECOId && n.Option == NotificationOption.StatusChange).Include(c => c.User);
            message = MessageBuilder("ECO", ECOId, "on status change", chnagedDetails);
            foreach (Notifications notification in StatusChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "on status change", "ECO", ECOId);
            }
        }

        public void SendNotificationOnStatusChangeECR(int ECRId, string chnagedDetails)
        {
            IQueryable<Notifications> StatusChange = db.Notifications.Where(n => n.ECRId == ECRId && n.Option == NotificationOption.StatusChange).Include(c => c.User);
            message = MessageBuilder("ECR", ECRId, "on status change", chnagedDetails);
            foreach (Notifications notification in StatusChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "on status change", "ECR", ECRId);
            }
        }
        #endregion
        #region Reminder Email
        public void SendReminderEmailECO(int ECOId, string msg)
        {
            IQueryable<Notifications> AnyChange = db.Notifications.Where(n => n.ECOId == ECOId).Include(c => c.User);
            message = MessageBuilder("ECO", ECOId, "reminder email", msg);
            foreach (Notifications notification in AnyChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "reminder email", "ECO", ECOId);
            }
        }

        public void SendReminderEmailECR(int ECRId, string msg)
        {
            IQueryable<Notifications> AnyChange = db.Notifications.Where(n => n.ECRId == ECRId).Include(c => c.User);
            message = MessageBuilder("ECR", ECRId, "reminder email", msg);
            foreach (Notifications notification in AnyChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "reminder email", "ECR", ECRId);
            }
        }

        public void SendReminderEmailECN(int ECNId, string msg)
        {
            IQueryable<Notifications> AnyChange = db.Notifications.Where(n => n.ECNId == ECNId).Include(c => c.User);
            message = MessageBuilder("ECN", ECNId, "reminder email", msg);
            foreach (Notifications notification in AnyChange)
            {
                emailer.SendEmailNotification(notification.User.Name, notification.User.Email, message, "reminder email", "ECN", ECNId);
            }
        }
        #endregion
    }
}
