using Microsoft.AspNetCore.Mvc.Rendering;
using Chnage.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.ViewModel.ECN
{
    public class ECNViewModel
    {
        public ECNViewModel()
        {

        }
        public ECNViewModel(Models.ECN ecn)
        {
            Id = ecn.Id;
            ModelName = ecn.ModelName;
            ModelNumber = ecn.ModelNumber;
            DateOfNotice = ecn.DateOfNotice;
            CreatedDate = ecn.CreatedDate;
            ClosedDate = ecn.ClosedDate;
            PTCRBResubmissionRequired = ecn.PTCRBResubmissionRequired;
            CurrentFirmwareVersion = ecn.CurrentFirmwareVersion;
            NewFirmwareVersion = ecn.NewFirmwareVersion;
            ImpactMissingReqApprovalDate = ecn.ImpactMissingReqApprovalDate;
            Approvers = ecn.Approvers.ToList();
            Description = ecn.Description;
            ChangeType = ecn.ChangeType;
            ChangeTypeId = ecn.ChangeTypeId;
            Notifications = ecn.Notifications.ToList();
            Originator = ecn.Originator;
            OriginatorId = ecn.OriginatorId;
            Status = ecn.Status;
            NewFirmwareVersion = ecn.NewFirmwareVersion;
        }
        public int Id { get; set; }
        [Display(Name = "Model Name"), Required]
        public string ModelName { get; set; }
        [Display(Name = "Model Number"), Required]
        public string ModelNumber { get; set; }
        [Display(Name = "Date of Notice"), Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime DateOfNotice { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? ClosedDate { get; set; }
        public User Originator { get; set; }
        public int OriginatorId { get; set; }
        [Display(Name = "Change Type")]
        public virtual RequestType ChangeType { get; set; }
        [Display(Name = "Change Type"), Required]
        public int ChangeTypeId { get; set; }
        [Display(Name = "PTCRB Resubmission Required"), Required]
        public bool PTCRBResubmissionRequired { get; set; }
        [Display(Name = "Current Firmware Version"), Required]
        public string CurrentFirmwareVersion { get; set; }
        [Display(Name = "New Firmware Version"), Required]
        public string NewFirmwareVersion { get; set; }
        [Required]
        public string Description { get; set; }
        [Display(Name = "Impact of Missing Required Approval Date"), Required]
        public string ImpactMissingReqApprovalDate { get; set; }
        public virtual ICollection<UserRoleECN> Approvers { get; set; }

        public List<int> UsersToBeNotified { get; set; }
        public virtual ICollection<Notifications> Notifications { get; set; }
        public virtual ICollection<AuditLog> AuditLogs { get; set; }
        public StatusOptions Status { get; set; }

        [Display(Name = "Related ECOs")]
        public SelectList RelatedECOs { get; set; }
        public List<int> RelatedECOIds { get; set; }

        public virtual ICollection<ECNHasECO> ECNHasECOs { get; set; } //This property used to get related ECO details for findChnages function
    }
}
