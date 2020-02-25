using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Active User")]
        public bool isActive { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<ECR> ECRs { get; set; }
        public virtual ICollection<ECO> ECOs { get; set; }
        public virtual ICollection<ECN> ECNs { get; set; }
        public virtual ICollection<Notifications> Notifications { get; set; }
    }
}
