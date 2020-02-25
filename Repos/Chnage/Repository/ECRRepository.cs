using Chnage.Models;
using Chnage.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Repository
{
    public class ECRRepository : IECR
    {
        private readonly MyECODBContext db;
        public ECRRepository()
        {

        }
        public ECRRepository(MyECODBContext _db)
        {
            db = _db;
        }
        public void Add(ECR ecr)
        {
            db.Add(ecr);
        }

        public IEnumerable<ECR> GetAllECRs() => db.ECRs.Include(ecr => ecr.Originator);
        public IEnumerable<ECR> GetAllUnfinishedECRs() => db.ECRs.Where(e => e.ECOsCompleted == false);
        public IEnumerable<ECR> GetAllFinishedECRs() => db.ECRs.Where(e => e.ECOsCompleted == true);
        public IEnumerable<ECR> GetAllECRsbyType(int originId) => db.ECRs.Where(e => e.ChangeTypeId == originId);
        public IEnumerable<ECR> GetAllECRsbyPriority(PriorityLevel priority) => db.ECRs.Where(e => e.PriorityLevel == priority);
        public IEnumerable<ECO> GetAllRelatedECOs(int ecrId)
        {
            return db.ECOs.Where(e => e.Id == e.RelatedECRs.Where(r => r.ECRId == ecrId).Single().ECO.Id);
        }

        public ECR GetECRbyId(int ecrId)
        {
            return db.ECRs.Where(e => e.Id == ecrId).Single();
        }

        public void Update(ECR ecr)
        {
            db.Update(ecr);
        }

        public IEnumerable<ECR> GetAllECRsByStatus(StatusOptions status, int id)
        {
            return db.ECRs.Where(e => e.Status == status && e.OriginatorId == id);
        }
    }
}
