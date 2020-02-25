using Chnage.Models;
using Chnage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Repository
{
    public class AuditLogRepository : IAuditLog
    {
        private readonly MyECODBContext db;
        public AuditLogRepository()
        {
        }
        public AuditLogRepository(MyECODBContext _db)
        {
            db = _db;
        }
        public void Add(ECR ecr, User changeUser)
        {
            AuditLog auditLog = new AuditLog(ecr)
            {
                ChangeUser = changeUser,
                ChangeUserId = changeUser.Id
            };
            db.AuditLogs.Add(auditLog);
        }

        public void Add(ECO eco, User changeUser)
        {
            AuditLog auditLog = new AuditLog(eco)
            {
                ChangeUser = changeUser,
                ChangeUserId = changeUser.Id
            };
            db.AuditLogs.Add(auditLog);
        }

        public void Add(ECN ecn, User changeUser)
        {
            AuditLog auditLog = new AuditLog(ecn)
            {
                ChangeUser = changeUser,
                ChangeUserId = changeUser.Id
            };
            db.AuditLogs.Add(auditLog);
        }

        public ICollection<AuditLog> GetAuditLogsECN(int ECNId)
        {
            return db.AuditLogs.Where(a => a.ECNId == ECNId).OrderByDescending(a => a.ChangeDateTime).ToList();
        }

        public ICollection<AuditLog> GetAuditLogsECO(int ECOId)
        {
            return db.AuditLogs.Where(a => a.ECOId == ECOId).OrderByDescending(a => a.ChangeDateTime).ToList();
        }

        public ICollection<AuditLog> GetAuditLogsECR(int ECRId)
        {
            return db.AuditLogs.Where(a => a.ECRId == ECRId).OrderByDescending(a => a.ChangeDateTime).ToList();
        }
    }
}
