using Chnage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Services
{
    public interface IECN
    {
        void Add(ECN ecn);
        void Update(ECN ecn);

        ECN GetECNbyId(int ecnId);
        IEnumerable<ECN> GetAllECNs();
        IEnumerable<ECN> GetAllECNsByStatus(StatusOptions status, int id);
    }
}
