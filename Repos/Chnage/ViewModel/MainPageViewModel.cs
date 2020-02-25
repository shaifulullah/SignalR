using Chnage.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Chnage.ViewModel
{
    public class MainPageViewModel
    {
        //public IEnumerable<Models.ECR> ECRs { get; set;}
        //public IEnumerable<Models.ECO> ECOs { get; set;}
        //public IEnumerable<Models.ECN> ECNs { get; set; }

        public MainPageViewModel(Models.ECR ecr)
        {
            Id = ecr.Id;
            ChangeType = ecr.ChangeType;
            Description = ecr.Description;
            ReasonForChange= ecr.ReasonForChange;
            CreatedDate = ecr.CreatedDate;
            PlannedImplementationDate = ecr.PlannedImplementationDate;
            ClosedDate = ecr.ClosedDate;
            PriorityLevel = ecr.PriorityLevel;
            Originator = ecr.Originator;
            ImplementationType = ecr.ImplementationType;
            Status = ecr.Status;
            Type = "ECR";
        }
        public MainPageViewModel(Models.ECO eco)
        {
            Id = eco.Id;
            ChangeType = eco.ChangeType;
            Description = eco.Description;
            ReasonForChange = eco.ReasonForChange;
            CreatedDate = eco.CreatedDate;
            PlannedImplementationDate = eco.PlannedImplementationDate;
            ClosedDate = eco.ClosedDate;
            PriorityLevel = eco.PriorityLevel;
            Originator = eco.Originator;
            ImplementationType = eco.ImplementationType;
            Status = eco.Status;
            Type = "ECO";
        }
        public MainPageViewModel(Models.ECN ecn)
        {
            Id = ecn.Id;
            ChangeType = ecn.ChangeType;
            Description = ecn.Description;
            CreatedDate = ecn.CreatedDate;
            ClosedDate = ecn.ClosedDate;
            Originator = ecn.Originator;
            Status = ecn.Status;
            Type = "ECN";
            PlannedImplementationDate = ecn.DateOfNotice;
            ReasonForChange = ecn.ImpactMissingReqApprovalDate;
        }
        public int Id { get; set; }
        [Display(Name = "Change Type")]
        public RequestType ChangeType { get; set; }
        [Display(Name = "Type")]
        public string Type { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Reason")]
        public string ReasonForChange { get; set; }
        [Display(Name = "Created Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Display(Name = "Implementation Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime PlannedImplementationDate { get; set; }
        [Display(Name = "Closed Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? ClosedDate { get; set; }
        [Display(Name = "Priority Level")]
        public PriorityLevel PriorityLevel { get; set; }
        [Display (Name = "Originator")]
        public virtual User Originator { get; set; }
        [Display(Name = "Implementation Type")]
        public ImpType ImplementationType { get; set; }
        public StatusOptions Status { get; set; }
        public ICollection<UserRole> Approvers { get; set; }
    }
}
