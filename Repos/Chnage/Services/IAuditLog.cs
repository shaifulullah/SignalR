using Chnage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Services
{
    public interface IAuditLog
    {
        void Add(ECR ecr, User changeUser);
        void Add(ECO eco, User changeUser);
        void Add(ECN ecn, User changeUser);

        ICollection<AuditLog> GetAuditLogsECR(int ECRId);
        ICollection<AuditLog> GetAuditLogsECO(int ECOId);
        ICollection<AuditLog> GetAuditLogsECN(int ECNId);
    }
}
