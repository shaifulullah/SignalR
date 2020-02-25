using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Models
{
    public class RequestType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        [Display(Name = "ECRs of this Request Type")]
        public virtual ICollection<RequestTypeECR> RequestTypeECRs { get; set; }
        [Display(Name = "ECOs of this Request Type")]
        public virtual ICollection<RequestTypeECO> RequestTypeECOs { get; set; }
    }
    public class RequestTypeECR
    {
        public int RequestTypeId { get; set; }
        public RequestType RequestType { get; set; }
        public int ECRId { get; set; }
        public ECR ECR { get; set; }
    }
    public class RequestTypeECO
    {
        public int RequestTypeId { get; set; }
        public RequestType RequestType { get; set; }
        public int ECOId { get; set; }
        public ECO ECO { get; set; }
    }
}
