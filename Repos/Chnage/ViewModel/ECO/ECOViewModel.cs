using Microsoft.AspNetCore.Mvc.Rendering;
using Chnage.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.ViewModel.ECO
{
    public class ECOViewModel
    {
        public ECOViewModel()
        {

        }
        public ECOViewModel(Models.ECO eco)
        {
            Id = eco.Id;
            List<RequestTypeChangeCheckBox> requestTypeChangeCheckBoxes = new List<RequestTypeChangeCheckBox>();
            foreach (var req in eco.AreasAffected)
            {
                RequestTypeChangeCheckBox requestType = new RequestTypeChangeCheckBox(req.RequestType);
                requestTypeChangeCheckBoxes.Add(requestType);
            }
            AreasAffected = requestTypeChangeCheckBoxes;
            Approvers = eco.Approvers.ToList();
            if(eco.AffectedProducts != null)
            {
                AffectedProducts = eco.AffectedProducts.ToList();
            }
            Description = eco.Description;
            BOMRequired = eco.BOMRequired;
            CustomerApproval = eco.CustomerApproval;
            ImplementationType = eco.ImplementationType;
            ReasonForChange = eco.ReasonForChange;
            ChangeType = eco.ChangeType;
            ChangeTypeId = eco.ChangeTypeId;
            PermanentChange = eco.PermanentChange;
            PlannedImplementationDate = eco.PlannedImplementationDate;
            PriorityLevel = eco.PriorityLevel;
            ProductValidationTestingRequired = eco.ProductValidationTestingRequired;
            Notifications = eco.Notifications.ToList();
            NotesForApproverIds = eco.NotesForApprover;
            NotesForValidatorIds = eco.NotesForValidator;
            LinkUrls = eco.LinkUrls;
            Originator = eco.Originator;
            OriginatorId = eco.OriginatorId;
            Status = eco.Status;
            NewRevision = eco.NewRevision;
            PreviousRevision = eco.PreviousRevision;
        }
        public int Id { get; set; }
        [Display(Name = "Permanent Change"),Required]
        public bool PermanentChange { get; set; }
        public List<int> UsersToBeNotified { get; set; }
        public List<Notifications> Notifications { get; set; }
        public virtual User Originator { get; set; }
        public int OriginatorId { get; set; }
        [Display(Name = "Affected Products")]
        public virtual List<ProductECO> AffectedProducts { get; set; }
        public List<int> AffectedProductsIds { get; set; }

        //public virtual Models.ECR ECR { get; set; }
        //public int ECRId { get; set; }

        [Display(Name = "Description"),Required]
        public string Description { get; set; }

        [Display(Name = "Reason For Change"),Required]
        public string ReasonForChange { get; set; }

        [Display(Name = "Change Type")]
        public virtual RequestType ChangeType { get; set; }
        [Display(Name = "Change Type"), Required]
        public int ChangeTypeId { get; set; }

        [Display(Name = "Areas Affected by Change"), Required]
        public List<RequestTypeChangeCheckBox> AreasAffected { get; set; }

        [Display(Name = "BOM Required"),Required]
        public bool BOMRequired { get; set; }

        [Display(Name = "Product Validation Testing Required"),Required]
        public bool ProductValidationTestingRequired { get; set; }

        [Display(Name = "Planned Implementation Date"), Required]
        public DateTime PlannedImplementationDate { get; set; } = DateTime.Now.AddDays(1);

        [Display(Name = "Customer Approval Required"),Required]
        public bool CustomerApproval { get; set; }
        [Display(Name = "Notes for Approver")]
        public Dictionary<string, string> NotesForApproverIds { get; set; }
        [Display(Name = "Notes for Validator")]
        public Dictionary<string, string> NotesForValidatorIds { get; set; }

        [Display(Name = "Related ECRs")]
        public SelectList RelatedECRs { get; set; }
        public List<int> RelatedECRIds { get; set; }
        public virtual ICollection<ECRHasECO> RelaredECRsForVm { get; set; }

        [Display(Name = "Priority Level"), Required]
        public PriorityLevel PriorityLevel { get; set; }

        public List<UserRoleECO> Approvers { get; set; }
        public List<SelectList> ApproversList { get; set; }
        public List<UserRoleECO> Validators { get; set; }
        public List<SelectList> ValidatorsList { get; set; }
        [Display(Name = "Links")]
        public Dictionary<string, string> LinkUrls { get; set; }
        public Dictionary<string, Dictionary<string, Dictionary<Product, bool>>> ProductList { get; set; }

        [Display(Name = "Implementation Type"),Required]
        public ImpType ImplementationType { get; set; }

        public StatusOptions Status { get; set; }
        [Display(Name = "Previous Revision")]
        public string PreviousRevision { get; set; }

        [Display(Name = "New Revision")]
        public string NewRevision { get; set; }

        [Display(Name = "Deviation")]
        public bool DeviationSelected { get; set; }
        public string Deviation { get; set; }
        [Display(Name = "Deviation Quantity"), Required]
        public int? DeviationQuantity { get; set; }
        [Display(Name = "Deviation Ends Date"), Required]
        public DateTime? DeviationDate { get; set; }

    }
    public class RequestTypeChangeCheckBox
    {
        public RequestTypeChangeCheckBox(RequestType type)
        {
            TypeId = type.Id;
            Name = type.Name;
        }
        public RequestTypeChangeCheckBox()
        {

        }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}
