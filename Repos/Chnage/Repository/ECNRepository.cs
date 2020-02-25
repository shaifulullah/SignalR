using Chnage.Models;
using Chnage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Repository
{
    public class ECNRepository : IECN
    {
        private readonly MyECODBContext db;
        public ECNRepository(MyECODBContext _db)
        {
            db = _db;
        }

        public IEnumerable<ECN> GetAllECNsByStatus(StatusOptions status, int id)
        {
            return db.ECNs.Where(e => e.Status == status && e.OriginatorId == id);
        }

        void IECN.Add(ECN ecn)
        {
            db.ECNs.Add(ecn);
        }

        IEnumerable<ECN> IECN.GetAllECNs()
        {
            return db.ECNs.ToList();
        }

        ECN IECN.GetECNbyId(int ecnId)
        {
            return db.ECNs.Where(e => e.Id == ecnId).Single();
        }

        void IECN.Update(ECN ecn)
        {
            db.ECNs.Update(ecn);
        }
    }
}
