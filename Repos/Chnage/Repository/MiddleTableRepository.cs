using Chnage.Models;
using Chnage.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Repository
{
    public class MiddleTableRepository : IMiddleTables
    {
        private readonly MyECODBContext db;
        public MiddleTableRepository()
        {

        }
        public MiddleTableRepository(MyECODBContext _db)
        {
            db = _db;
        }

        #region RequestTypeECR

        public IEnumerable<RequestTypeECR> GetRequestTypesForECR(int ECRId)
        {
            return db.RequestTypeECRs.Where(r => r.ECRId == ECRId);
        }
        public IEnumerable<ECR> GetECRsForRequestType(int ReqId)
        {
            //return db.ECRs.Where(e => e.AreasAffected.Contains(GetExistingRequestTypeECR(ReqId, e.Id)));
            var a = db.RequestTypeECRs.Where(r => r.RequestTypeId == ReqId).Include(r => r.ECR);
            List<ECR> returnList = new List<ECR>();
            foreach (var item in a)
            {
                returnList.Add(item.ECR);
            }
            return returnList;
        }

        public RequestTypeECR GetExistingRequestTypeECR(int ReqId, int ECRId)
        {
            return db.RequestTypeECRs.Where(r => r.RequestTypeId == ReqId && r.ECRId == ECRId).FirstOrDefault();
        }

        #endregion

        #region RequestTypeECO

        public IEnumerable<RequestTypeECO> GetRequestTypesForECO(int ECOId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ECO> GetECOsForRequestType(int ReqId)
        {
            throw new NotImplementedException();
        }

        public RequestTypeECO GetExistingRequestTypeECO(int ReqId, int ECOId)
        {
            return db.RequestTypeECOs.Where(r => r.RequestTypeId == ReqId && r.ECOId == ECOId).FirstOrDefault();
        }

        #endregion

        #region UserRoleECR

        public IEnumerable<UserRoleECR> GetUserRolesForECR(int ECRId)
        {
            return db.UserRoleECRs.Where(u => u.ECRId == ECRId);
        }

        public IEnumerable<ECR> GetECRWithUserRole(int URId)
        {
            throw new NotImplementedException();
        }
        public UserRoleECR GetExistingUserRoleECR(int URId, int ECRId)
        {
            return db.UserRoleECRs.Where(u => u.ECRId == ECRId && u.UserRoleId == URId).Include(u => u.UserRole.User).FirstOrDefault();
        }
        public UserRoleECR GetExistingUserRoleECRByUser(int UserId, int ECRId)
        {
            return db.UserRoleECRs.Where(u => u.ECRId == ECRId && u.UserRole.UserId == UserId).FirstOrDefault();
        }

        #endregion

        #region UserRoleECO

        public IEnumerable<UserRoleECO> GetUserRolesForECO(int ECOId)
        {
            return db.UserRoleECOs.Where(u => u.ECOId == ECOId);
        }

        public IEnumerable<ECO> GetECOWithUserRole(int URId)
        {
            throw new NotImplementedException();
        }

        public UserRoleECO GetExistingUserRoleECO(int URId, int ECOId)
        {
            return db.UserRoleECOs.Where(u => u.ECOId == ECOId && u.UserRoleId == URId).Include(u => u.UserRole.User).FirstOrDefault();
        }
        public UserRoleECO GetExistingUserRoleECOByUser(int UserId, int ECOId)
        {
            return db.UserRoleECOs.Where(u => u.ECOId == ECOId && u.UserRole.UserId == UserId).FirstOrDefault();
        }

        #endregion

        #region UserRoleECN
        public IEnumerable<UserRoleECN> GetUserRolesForECN(int ECNId)
        {
            return db.UserRoleECNs.Where(u => u.ECNId == ECNId).Include(a => a.UserRole.User);
        }

        public IEnumerable<ECN> GetECNWithUserRole(int URId)
        {
            throw new NotImplementedException();
        }

        public UserRoleECN GetExistingUserRoleECN(int URId, int ECNId)
        {
            return db.UserRoleECNs.Where(u => u.ECNId == ECNId && u.UserRoleId == URId).Include(u => u.UserRole.User).FirstOrDefault();
        }
        public UserRoleECN GetExistingUserRoleECNByUser(int UserId, int ECNId)
        {
            return db.UserRoleECNs.Where(u => u.ECNId == ECNId && u.UserRole.UserId == UserId).FirstOrDefault();
        }

        #endregion

        #region Notification

        public IEnumerable<Notifications> GetNotificationsForECR(int ECRId)
        {
            return db.Notifications.Where(n => n.ECRId == ECRId);
        }

        public Notifications GetNotificationById(int NotificationId)
        {
            return db.Notifications.Where(n => n.Id == NotificationId).FirstOrDefault();
        }


        public Notifications GetNotificationByUserAndECR(int UserId, int ECRId)
        {
            return db.Notifications.Where(n => n.UserId == UserId && n.ECRId == ECRId).FirstOrDefault();
        }

        public Notifications GetNotificationByUserAndECO(int UserId, int ECOId)
        {
            return db.Notifications.Where(n => n.UserId == UserId && n.ECOId == ECOId).FirstOrDefault();
        }

        public Notifications GetNotificationByUserAndECN(int UserId, int ECNId)
        {
            return db.Notifications.Where(n => n.UserId == UserId && n.ECNId == ECNId).FirstOrDefault();
        }

        #endregion

        #region ECRHasECO
        public IEnumerable<ECRHasECO> GetRelatedECOByECR(int ECRId)
        {
            return db.ECRHasECOs.Where(e => e.ECRId == ECRId);
        }

        public IEnumerable<ECRHasECO> GetRelatedECRByECO(int ECOId)
        {
            return db.ECRHasECOs.Where(e => e.ECOId == ECOId);
        }

        public ECRHasECO GetRecordByCompKey(int ECRId, int ECOId)
        {
            return db.ECRHasECOs.Where(e => e.ECRId == ECRId && e.ECOId == ECOId).FirstOrDefault();
        }
        #endregion

        #region ECNHasECO

        public IEnumerable<ECNHasECO> GetRelatedECOByECNId(int ECNId)
        {
            return db.ECNHasECOs.Where(e => e.ECNId == ECNId);
        }
        public ECNHasECO GetECNHasECORecord(int ECOId, int ECNId)
        {
            return db.ECNHasECOs.Where(e => e.ECOId == ECOId && e.ECNId == ECNId).FirstOrDefault();
        }
        #endregion

        #region ProductECR

        public IEnumerable<ProductECR> GetProductECRsByECR(int ECRId)
        {
            return db.ProductECRs.Where(a => a.ECRId == ECRId);
        }

        public IEnumerable<ProductECR> GetProductECRsByProduct(int ProductId)
        {
            return db.ProductECRs.Where(a => a.ProductId == ProductId).Include(p => p.ECR);
        }

        public ProductECR GetExistingProductECR(int ProductId, int ECRId)
        {
            return db.ProductECRs.Where(a => a.ProductId == ProductId && a.ECRId == ECRId).FirstOrDefault();
        }

        #endregion

        #region ProductECO
        public IEnumerable<ProductECO> GetProductECOsByECO(int ECOId)
        {
            return db.ProductECOs.Where(a => a.ECOId == ECOId);
        }
        public IEnumerable<ProductECO> GetProductECOsByProduct(int ProductId)
        {
            return db.ProductECOs.Where(a => a.ProductId == ProductId).Include(p => p.ECO);
        }

        public ProductECO GetExistingProductECO(int ProductId, int ECOId)
        {
            return db.ProductECOs.Where(a => a.ProductId == ProductId && a.ECOId == ECOId).FirstOrDefault();
        }

        #endregion

    }
}
