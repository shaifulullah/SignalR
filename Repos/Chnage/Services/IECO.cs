using Chnage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Services
{
    public interface IECO
    {
        void Add(ECO eco);
        void Update(ECO eco);

        ECO GetECObyId(int ecoId);
        IEnumerable<ECO> GetAllECOs();
        IEnumerable<ECO> GetAllECOsByStatus(StatusOptions status, int id);
    }
}
