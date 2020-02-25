using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Models
{
    public class UserRole
    {
        [Key]
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("UserId")]
        [Required]
        public virtual User User { get; set; }
        public int UserId { get; set; }
        [ForeignKey("RequestTypeId"), Display(Name = "Request Type")]
        [Required]
        public virtual RequestType RequestType { get; set; }
        public int RequestTypeId { get; set; }
        [Display(Name = "Role Type")]
        public Role RoleInt { get; set; }
        [Display(Name = "Active Role")]
        public bool isActive { get; set; } = true;

        public virtual ICollection<UserRoleECR> ECRs { get; set; }
        public virtual ICollection<UserRoleECO> ECOs { get; set; }
        public virtual ICollection<UserRoleECN> ECNs { get; set; }
    }
    public enum Role
    {
        Approver = 1,
        Validator = 2,
        ECNApprover = 3
    }
    public class UserRoleECR
    {
        public int UserRoleId { get; set; }
        public UserRole UserRole { get; set; }
        public int ECRId { get; set; }
        public ECR ECR { get; set; }
        public bool? Approval { get; set; }
        public string AprovedDate { get; set; }
    }
    public class UserRoleECO
    {
        public int UserRoleId { get; set; }
        public UserRole UserRole { get; set; }
        public int ECOId { get; set; }
        public ECO ECO { get; set; }
        public bool? Approval { get; set; }
        public string AprovedDate { get; set; }
    }
    public class UserRoleECN
    {
        public int UserRoleId { get; set; }
        public UserRole UserRole { get; set; }
        public int ECNId { get; set; }
        public ECN ECN { get; set; }
        public bool? Approval { get; set; }
        public string AprovedDate { get; set; }
    }
}
