using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Models
{
    public class AuditLog
    {
        public AuditLog(ECR ecr)
        {
            ECRId = ecr.Id;
            PermanentChange = ecr.PermanentChange;
            Description = ecr.Description;
            ReasonForChange = ecr.ReasonForChange;
            ChangeType = ecr.ChangeType;
            ChangeTypeId = ecr.ChangeTypeId;
            BOMRequired = ecr.BOMRequired;
            ProductValidationTestingRequired = ecr.ProductValidationTestingRequired;
            PlannedImplementationDate = ecr.PlannedImplementationDate;
            ClosedDate = ecr.ClosedDate;
            CustomerImpact = ecr.CustomerImpact;
            PriorityLevel = ecr.PriorityLevel;
            ImplementationType = ecr.ImplementationType;
            Status = ecr.Status;
            PreviousRevision = ecr.PreviousRevision;
            NewRevision = ecr.NewRevision;
            LinkUrls = ecr.LinkUrls;
            NotesForApprovers = null;
            NotesForValidators = null;

            if (ecr.AffectedProducts != null)
            {
                foreach (var product in ecr.AffectedProducts)
                {
                    AffectedProducts += product.Product.Name + ";";
                }
            }
            if (ecr.AreasAffected != null)
            {
                foreach (var area in ecr.AreasAffected)
                {
                    AreasAffected += area.RequestType.Name + ";";
                }
            }
            if (ecr.RelatedECOs != null)
            {
                foreach (var eco in ecr.RelatedECOs)
                {
                    RelatedECOs += eco.ECOId + ";";
                }
            }
            if (ecr.Approvers != null)
            {
                foreach (var approver in ecr.Approvers)
                {
                    if (approver.UserRole != null)
                        if (approver.UserRole.User != null && approver.UserRole.RequestType != null)
                            Approvers += approver.UserRole.User.Name + ", " + approver.UserRole.RequestType.Name + ";";
                }
            }
            if (ecr.Notifications != null)
            {
                foreach (var notification in ecr.Notifications)
                {
                    if (notification.User != null)
                        Notifications += notification.User.Name + ";";
                }
            }
        }
        public AuditLog(ECO eco)
        {
            ECOId = eco.Id;
            PermanentChange = eco.PermanentChange;
            Description = eco.Description;
            ReasonForChange = eco.ReasonForChange;
            ChangeType = eco.ChangeType;
            ChangeTypeId = eco.ChangeTypeId;
            BOMRequired = eco.BOMRequired;
            ProductValidationTestingRequired = eco.ProductValidationTestingRequired;
            PlannedImplementationDate = eco.PlannedImplementationDate;
            ClosedDate = eco.ClosedDate;
            CustomerApproval = eco.CustomerApproval;
            PriorityLevel = eco.PriorityLevel;
            ImplementationType = eco.ImplementationType;
            Status = eco.Status;
            PreviousRevision = eco.PreviousRevision;
            NewRevision = eco.NewRevision;
            LinkUrls = eco.LinkUrls;
            NotesForApprovers = eco.NotesForApprover;
            NotesForValidators = eco.NotesForValidator;
            if (eco.AffectedProducts != null)
            {
                foreach (var product in eco.AffectedProducts)
                {
                    AffectedProducts += product.Product.Name + ";";
                }
            }
            if (eco.AreasAffected != null)
            {
                foreach (var area in eco.AreasAffected)
                {
                    AreasAffected += area.RequestType.Name + ";";
                }
            }
            if (eco.RelatedECRs != null)
            {
                foreach (var ecr in eco.RelatedECRs)
                {
                    RelatedECRs += ecr.ECRId + ";";
                }
            }
            if (eco.Approvers != null)
            {
                foreach (var approver in eco.Approvers)
                {
                    if (approver.UserRole != null)
                        if (approver.UserRole.User != null && approver.UserRole.RequestType != null)
                            Approvers += approver.UserRole.User.Name + " - " + approver.UserRole.RequestType.Name + ";";
                }
            }
            if (eco.Notifications != null)
            {
                foreach (var notification in eco.Notifications)
                {
                    Notifications += notification.User.Name + ";";
                }
            }
        }
        public AuditLog(ECN ecn)
        {
            ECNId = ecn.Id;
            ModelName = ecn.ModelName;
            ModelNumber = ecn.ModelNumber;
            DateOfNotice = ecn.DateOfNotice;
            ChangeTypeId = ecn.ChangeTypeId;
            PTCRBResubmissionRequired = ecn.PTCRBResubmissionRequired;
            CurrentFirmwareVersion = ecn.CurrentFirmwareVersion;
            NewFirmwareVersion = ecn.NewFirmwareVersion;
            Description = ecn.Description;
            ImpactMissingReqApprovalDate = ecn.ImpactMissingReqApprovalDate;
            foreach (var approver in ecn.Approvers)
            {
                if (approver.UserRole != null)
                    if (approver.UserRole.User != null)
                        Approvers += approver.UserRole.User.Name + ";";
            }

            if (ecn.Notifications != null)
            {
                foreach (var notification in ecn.Notifications)
                {
                    Notifications += notification.User.Name + ";";
                }
            }
        }
        public AuditLog()
        {
        }
        [Key]
        public int Id { get; set; }

        [Display(Name = "Audit User")]
        public User ChangeUser { get; set; }

        public int ChangeUserId { get; set; }

        [Display(Name = "Change Date Time")]
        public DateTime ChangeDateTime { get; set; } = DateTime.Now;

        [Display(Name = "Permanent Change")]
        public bool? PermanentChange { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Reason For Change")]
        public string ReasonForChange { get; set; }

        [Display(Name = "Change Type")]
        public virtual RequestType ChangeType { get; set; }

        public int? ChangeTypeId { get; set; }

        [Display(Name = "BOM Required")]
        public bool? BOMRequired { get; set; }

        [Display(Name = "Product Validation Testing Required")]
        public bool? ProductValidationTestingRequired { get; set; }
        [Display(Name = "Planned Implementation Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? PlannedImplementationDate { get; set; }

        [Display(Name = "Closed Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? ClosedDate { get; set; }

        [Display(Name = "Customers Impacted")]
        public bool? CustomerImpact { get; set; }
        [Display(Name = "Customer Approval Required")]
        public bool? CustomerApproval { get; set; }

        [Display(Name = "Priority Level")]
        public PriorityLevel PriorityLevel { get; set; }

        [Display(Name = "Implementation Type")]
        public ImpType ImplementationType { get; set; }

        public StatusOptions Status { get; set; }

        [Display(Name = "Previous Revision")]
        public string PreviousRevision { get; set; }

        [Display(Name = "New Revision")]
        public string NewRevision { get; set; }

        [Display(Name = "Links")]
        public Dictionary<string, string> LinkUrls { get; set; }

        [Display(Name = "Notes for Approvers")]
        public Dictionary<string, string> NotesForApprovers { get; set; }
        [Display(Name = "Notes for Validators")]
        public Dictionary<string, string> NotesForValidators { get; set; }
        //information from other tables, just inserting the values
        public string AffectedProducts { get; set; } // Id of the products
        [Display(Name = "Areas Affected by Change")]
        public string AreasAffected { get; set; } // name of the areas
        public string RelatedECOs { get; set; } // Id of the ECO
        public string RelatedECRs { get; set; } // Id of the ECR
        public string Approvers { get; set; } // name and area of the UserRole
        public string Notifications { get; set; } // name of the user

        public int? ECRId { get; set; }
        public int? ECOId { get; set; }
        public int? ECNId { get; set; }

        public virtual ECR ECR { get; set; }
        public virtual ECO ECO { get; set; }
        public virtual ECN ECN { get; set; }

        //ECN Fields:
        public string ModelName { get; set; }
        public string ModelNumber { get; set; }
        public DateTime? DateOfNotice { get; set; }
        public bool? PTCRBResubmissionRequired { get; set; }
        public string CurrentFirmwareVersion { get; set; }
        public string NewFirmwareVersion { get; set; }
        public string ImpactMissingReqApprovalDate { get; set; }
    }
}
