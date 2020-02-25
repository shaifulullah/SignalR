using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Models
{
    public class UpdateWebAppLogs
    {
        [Key]
        public int iId { get; set; }
        [Display(Name = "Version")]
        public string sWebAppversion { get; set; }
        [Display(Name = "Upload Date")]
        public DateTime dtUploadDate { get; set; } = DateTime.UtcNow;
        [Display(Name = "Reason")]
        public string sReason { get; set; }
        [Display(Name = "Uploaded By (Email)")]
        public string sUserEmail { get; set; }
    }
}
