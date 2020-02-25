using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Models
{
    public class Audit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public string TableName { get; set; }
        public string EntityId { get; set; }
        public string ChangedColumns { get; set; }
        public string OldData { get; set; }
        public string NewData { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
