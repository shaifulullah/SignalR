using Chnage.Models;
using Chnage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Repository
{
    public class ECORepository : IECO
    {
        private readonly MyECODBContext db;
        public ECORepository(MyECODBContext _db)
        {
            db = _db;
        }

        public IEnumerable<ECO> GetAllECOsByStatus(StatusOptions status, int id)
        {
            return db.ECOs.Where(a => a.Status == status && a.OriginatorId == id);
        }

        void IECO.Add(ECO eco)
        {
            db.Add(eco);
        }

        IEnumerable<ECO> IECO.GetAllECOs()
        {
            return db.ECOs.ToList();
        }

        ECO IECO.GetECObyId(int ecoId)
        {
            return db.ECOs.Where(e => e.Id == ecoId).FirstOrDefault();
        }

        void IECO.Update(ECO eco)
        {
            db.Update(eco);
        }
    }
}
