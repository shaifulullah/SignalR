using Chnage.ViewModel.ECN;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Models
{
    public class ECN
    {
        public ECN()
        {

        }
        public ECN(ECNViewModel vm)
        {
            ModelName = vm.ModelName;
            ModelNumber = vm.ModelNumber;
            DateOfNotice = vm.DateOfNotice;
            CreatedDate = vm.CreatedDate;
            ClosedDate = vm.ClosedDate;
            PTCRBResubmissionRequired = vm.PTCRBResubmissionRequired;
            CurrentFirmwareVersion = vm.CurrentFirmwareVersion;
            NewFirmwareVersion = vm.NewFirmwareVersion;
            ImpactMissingReqApprovalDate = vm.ImpactMissingReqApprovalDate;
            Description = vm.Description;
            ChangeType = vm.ChangeType;
            ChangeTypeId = vm.ChangeTypeId;
            Originator = vm.Originator;
            OriginatorId = vm.OriginatorId;
            Status = vm.Status;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public virtual ICollection<Notifications> Notifications { get; set; }
        public virtual ICollection<AuditLog> AuditLogs { get; set; }
        public StatusOptions Status { get; set; }
        public string RejectReason { get; set; }
        [Display(Name = "Related ECO Ids")]
        public virtual ICollection<ECNHasECO> RelatedECOs { get; set; }
    }
    public class ECNHasECO
    {
        public int ECOId { get; set; }
        public virtual ECO ECO { get; set; }
        public int ECNId { get; set; }
        public virtual ECN ECN { get; set; }
    }
}
