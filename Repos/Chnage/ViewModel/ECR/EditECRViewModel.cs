using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Chnage.ViewModel.ECR
{
    public class EditECRViewModel : ECRViewModel
    {
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Closed Date")]
        public DateTime? ClosedDate { get; set; }
        public virtual ICollection<Models.ECO> RelatedECOs { get; set; }
    }
}
