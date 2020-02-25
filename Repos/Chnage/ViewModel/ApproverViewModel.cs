using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.ViewModel
{
    public class ApproverViewModel
    {
        public bool ApproverOption { get; set; }
        public int UserRoleId { get; set; }
        public int ChangeId { get; set; }
        public string RejectReason { get; set; }
    }
}
