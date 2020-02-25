using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Models
{
    public class ViewModelError
    {
        [Key]
        public int ErrorId { get; set; }
        [Display(Name = "Exception Message")]
        public string ExceptionMessage { get; set; }
        [Display(Name = "Exception Stack Trace")]
        public string ExceptionStackTrace { get; set; }
        [Display(Name = "Developer Email")]
        public string Email { get; set; }
        [Display(Name = "Exception Type")]
        public string ExceptionTypeString { get; set; }
        [Display(Name = "Inner Exception Message")]
        public string InnerExceptionMessage { get; set; }
        [Display(Name = "Inner Exception Type")]
        public string InnerExceptionType { get; set; }
        [Display(Name = "Inner Exception ToString")]
        public string InnerToString { get; set; }
        [Display(Name = "Exception Controller")]
        public string ExceptionController { get; set; }
        [Display(Name = "Exception Action")]
        public string ExceptionAction { get; set; }

        [Display(Name = "User")]
        public string User { get; set; }

        public ViewModelError(Exception ex, string Controller, string Action)
        {
            this.ErrorId = ex.HResult;
            this.ExceptionMessage = ex.Message;
            this.ExceptionStackTrace = ex.StackTrace;
            this.Email = "shaifulullah@gmail.com";
            this.ExceptionTypeString = ex.GetType().FullName;
            this.ExceptionController = Controller;
            this.ExceptionAction = Action;
        }

        public ViewModelError()
        {
        }

        public string GenerateEmailBody()
        {
            if (String.IsNullOrEmpty(this.User))
            {
                this.User = "";
            }
            string body =
                "<h2> Exception Type: " + this.ExceptionTypeString + "</h2>" +
                    "<h3>" +
                        "<p> Exception Id: " +
                            this.ErrorId +
                        "</p>" +
                        "<p> Exception Message: " +
                            this.ExceptionMessage +
                        "</p>" +
                        "<p> Exception StackTrace: " +
                            this.ExceptionStackTrace +
                        "</p>" +
                        "<p> User: " +
                            this.User +
                        "</p>" +
                    "</h3>" +
                "<h2> Inner Exception Type: " + this.InnerExceptionType + "</h2>" +
                    "<h3>" +
                        "<p> Inner Exception Message: " +
                            this.InnerExceptionMessage +
                        "</p>" +
                        "<p> Inner Exception ToString: " +
                            this.InnerToString +
                        "</p>" +
                    "</h3>";
            return body;
        }

    }
}
