using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Chnage.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Chnage.ViewModel.ECR
{
    public class ECRViewModel 
    {
        public int Id { get; set; }
        [Display(Name = "Permanent Change"), Required]
        public bool PermanentChange { get; set; }

        public List<int> UsersToBeNotified { get; set; }
        public List<Notifications> Notifications { get; set; }
        public User Originator { get; set; }
        public int OriginatorId { get; set; }
        [Display (Name = "Affected Products")]
        public List<ProductECR> AffectedProducts { get; set; }
        public List<int> AffectedProductsIds { get; set; }

        [Display(Name = "Description"), Required]
        public string Description { get; set; }

        [Display(Name = "Reason For Change"), Required]
        public string ReasonForChange { get; set; }

        [Display(Name = "Change Type")]
        public virtual RequestType ChangeType { get; set; }
        [Display(Name ="Change Type"), Required]
        public int ChangeTypeId { get; set; }

        [Display(Name = "Areas Affected by Change"), Required]
        public List<RequestTypeChangeCheckBox> AreasAffected { get; set; }

        [Display(Name = "BOM Required"), Required]
        public bool BOMRequired { get; set; }

        [Display(Name = "Product Validation Testing Required"), Required]
        public bool ProductValidationTestingRequired { get; set; }

        [Display(Name = "Planned Implementation Date"), Required, DataType(DataType.Date)]
        public DateTime PlannedImplementationDate { get; set; } = DateTime.Now.AddDays(1);

        [Display(Name = "Customers Impacted"), Required]
        public bool CustomerImpact { get; set; }

        [Display(Name = "Priority Level"), Required]
        public PriorityLevel PriorityLevel { get; set; }        

        public List<UserRoleECR> Approvers { get; set; }
        public List<SelectList> ApproversList { get; set; }        

        [Display(Name = "Implementation Type"), Required]
        public ImpType ImplementationType { get; set; }

        [Display(Name = "Previous Revision")]
        public string PreviousRevision { get; set; }
        [Display(Name = "New Revision")]
        public string NewRevision { get; set; }

        [Display(Name = "Links")]
        public Dictionary<string,string> LinkUrls { get; set; }
        public Dictionary<string, Dictionary<string, Dictionary<Product, bool>>> ProductList { get; set; }
        public StatusOptions Status { get; set; }
        //[Display(Name = "all ECOs approved")]
        //public bool ECOsCompleted { get; set; }
        public ECRViewModel(Models.ECR ecr)
        {
            Id = ecr.Id;
            List<RequestTypeChangeCheckBox> requestTypeChangeCheckBoxes = new List<RequestTypeChangeCheckBox>();
            foreach (var req in ecr.AreasAffected)
            {
                RequestTypeChangeCheckBox requestType = new RequestTypeChangeCheckBox(req.RequestType);
                requestTypeChangeCheckBoxes.Add(requestType);
            }
            AreasAffected = requestTypeChangeCheckBoxes;
            Description = ecr.Description;
            BOMRequired = ecr.BOMRequired;
            CustomerImpact = ecr.CustomerImpact;
            Approvers = ecr.Approvers.ToList();
            AffectedProducts = ecr.AffectedProducts.ToList();
            ImplementationType = ecr.ImplementationType;
            ReasonForChange = ecr.ReasonForChange;
            ChangeType = ecr.ChangeType;
            ChangeTypeId = ecr.ChangeTypeId;
            PermanentChange = ecr.PermanentChange;
            NewRevision = ecr.NewRevision;
            PreviousRevision = ecr.PreviousRevision;
            PlannedImplementationDate = ecr.PlannedImplementationDate;
            PriorityLevel = ecr.PriorityLevel;
            ProductValidationTestingRequired = ecr.ProductValidationTestingRequired;
            Notifications = ecr.Notifications.ToList();
            LinkUrls = ecr.LinkUrls;
        }

        public ECRViewModel()
        {
        }

        public static implicit operator Models.ECR(ECRViewModel vm)
        {
            return new Models.ECR(vm);//{//    //AreasAffected = vm.AreasAffected,//    Description = vm.Description,//    BOMRequired = vm.BOMRequired,//    CustomerImpact = vm.CustomerImpact,//    Approvers = vm.Approvers.ToList(),//    ExcludedProducts = vm.ExcludedProducts.ToList(),//    ImplementationType = vm.ImplementationType,//    ReasonForChange = vm.ReasonForChange,//    ChangeType = vm.ChangeType,//    ChangeTypeId = vm.ChangeTypeId,//    PermanentChange = vm.PermanentChange,//    PlannedImplementationDate = vm.PlannedImplementationDate,//    PriorityLevel = vm.PriorityLevel,//    ProductValidationTestingRequired = vm.ProductValidationTestingRequired//};
        }
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
