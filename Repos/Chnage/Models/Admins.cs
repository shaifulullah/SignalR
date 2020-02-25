using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Models
{
    public class Admins
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int iId { get; set; }
        [Display(Name = "Admin Name")]
        public string sName { get; set; }
        [Display(Name = "Admin Email")]
        public string sEmail { get; set; }
    }
}
