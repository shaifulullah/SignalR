using Chnage.CustomAttributes;
using Chnage.ViewModel.ECO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Models
{
    public class ECO
    {
        public ECO()
        {

        }
        public ECO(ECOViewModel vm)
        {
            PermanentChange = vm.PermanentChange;
            AffectedProducts = vm.AffectedProducts;
            Description = vm.Description;
            ReasonForChange = vm.ReasonForChange;
            ChangeType = vm.ChangeType;
            ChangeTypeId = vm.ChangeTypeId;
            BOMRequired = vm.BOMRequired;
            ProductValidationTestingRequired = vm.ProductValidationTestingRequired;
            PlannedImplementationDate = vm.PlannedImplementationDate;
            CustomerApproval = vm.CustomerApproval;
            PriorityLevel = vm.PriorityLevel;
            Approvers = vm.Approvers;
            Originator = vm.Originator;
            OriginatorId = vm.OriginatorId;
            ImplementationType = vm.ImplementationType;
            Status = vm.Status;
            NotesForApprover = vm.NotesForApproverIds;
            NotesForValidator = vm.NotesForValidatorIds;
            LinkUrls = vm.LinkUrls;
            PreviousRevision = vm.PreviousRevision;
            NewRevision = vm.NewRevision;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ExcludeFromStructuredJsonInfo]
        public int Id { get; set; }
        [Display(Name = "Permanent Change")]
        public bool PermanentChange { get; set; }

        [Display(Name = "Related ECR Ids")]
        public virtual ICollection<ECRHasECO> RelatedECRs { get; set; }

        [Display(Name = "Related ECN Ids")]
        public virtual ICollection<ECNHasECO> RelatedECNs { get; set; }

        public virtual ICollection<ProductECO> AffectedProducts { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Reason for Change")]
        public string ReasonForChange { get; set; }

        [Display(Name = "Change Type")]
        public virtual RequestType ChangeType { get; set; }

        [ExcludeFromStructuredJsonInfo]
        public int ChangeTypeId { get; set; }

        [Display(Name = "Areas Affected by Change")]
        public virtual ICollection<RequestTypeECO> AreasAffected { get; set; }

        [Display(Name = "BOM Required")]
        public bool BOMRequired { get; set; }

        [Display(Name = "Product Validation Testing Required")]
        public bool ProductValidationTestingRequired { get; set; }

        [Display(Name = "Planned Implementation Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime PlannedImplementationDate { get; set; }

        [Display(Name = "Created Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Closed Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? ClosedDate { get; set; }

        [Display(Name = "Customer Approval Required")]
        public bool CustomerApproval { get; set; }
        [Display(Name = "Priority Level")]
        public PriorityLevel PriorityLevel { get; set; }

        public virtual ICollection<UserRoleECO> Approvers { get; set; }
        public virtual User Originator { get; set; }

        [ExcludeFromStructuredJsonInfo]
        public int OriginatorId { get; set; }

        [Display(Name = "Implementation Type")]
        public ImpType ImplementationType { get; set; }

        public StatusOptions Status { get; set; }
        [Display(Name = "Notes for Approver")]
        public Dictionary<string, string> NotesForApprover { get; set; }
        [Display(Name = "Notes for Validator")]
        public Dictionary<string, string> NotesForValidator { get; set; }
        [Display(Name = "Previous Revision")]
        public string PreviousRevision { get; set; }
        [Display(Name = "New Revision")]
        public string NewRevision { get; set; }
        [Display(Name = "Links")]
        public Dictionary<string, string> LinkUrls { get; set; }

        public virtual ICollection<Notifications> Notifications { get; set; }
        public virtual List<AuditLog> AuditLogs { get; set; }
        public string RejectReason { get; set; }
    }
}
