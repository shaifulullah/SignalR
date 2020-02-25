using Chnage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Services
{
    public interface IECR
    {
        void Add(ECR ecr);
        void Update(ECR ecr);

        ECR GetECRbyId(int ecrId);

        IEnumerable<ECR> GetAllECRs();
        IEnumerable<ECR> GetAllUnfinishedECRs();
        IEnumerable<ECR> GetAllFinishedECRs();
        IEnumerable<ECR> GetAllECRsbyType(int originRequestId);
        IEnumerable<ECR> GetAllECRsbyPriority(PriorityLevel priorityLevel);
        IEnumerable<ECO> GetAllRelatedECOs(int ecrId);
        IEnumerable<ECR> GetAllECRsByStatus(StatusOptions status, int id);
    }
}
