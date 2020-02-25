using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Models
{
    public class Notifications
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public virtual ECR ECR { get; set; }
        public virtual ECO ECO { get; set; }
        public virtual ECN ECN { get; set; }
        public int UserId { get; set; }
        public int? ECRId { get; set; }
        public int? ECOId { get; set; }
        public int? ECNId { get; set; }
        [Display(Name = "Notified on")]
        public NotificationOption Option { get; set; }
    }
    public enum NotificationOption
    {
        [Display(Name = "Any Change")]
        AnyChange = 0,
        [Display(Name = "Status Change")]
        StatusChange = 1,
        [Display(Name = "Description Change")]
        DescChange = 2,
        [Display(Name = "Reason Change")]
        ReasonChange = 6,
        [Display(Name = "Implementation Date Change")]
        ImpDateChange = 3,
        [Display(Name = "Approvers Change")]
        ApproversChange = 4,
        [Display(Name = "Approval")]
        Approval = 5
    }
}
