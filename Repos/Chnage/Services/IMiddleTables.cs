using Chnage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Services
{
    public interface IMiddleTables
    {
        #region RequestTypeECR
        IEnumerable<RequestTypeECR> GetRequestTypesForECR(int ECRId);
        IEnumerable<ECR> GetECRsForRequestType(int ReqId);
        /// <summary>
        /// Gets the RequestTypeECR from the database using the Request Type Id and ECRId.
        /// </summary>
        /// <param name="ReqId">Request Type Id</param>
        /// <param name="ECRId">ECR Id</param>
        /// <returns>RequestTypeECR or Default(null)</returns>
        RequestTypeECR GetExistingRequestTypeECR(int ReqId, int ECRId);
        #endregion

        #region RequestTypeECO
        IEnumerable<RequestTypeECO> GetRequestTypesForECO(int ECOId);
        IEnumerable<ECO> GetECOsForRequestType(int ReqId);
        RequestTypeECO GetExistingRequestTypeECO(int ReqId, int ECOId);
        #endregion

        #region UserRoleECR
        IEnumerable<UserRoleECR> GetUserRolesForECR(int ECRId);
        IEnumerable<ECR> GetECRWithUserRole(int URId);
        /// <summary>
        /// Gets the UserRoleECR from the database using the User Role Id and ECRId.
        /// </summary>
        /// <param name="URId">User Role Id</param>
        /// <param name="ECRId">ECR Id</param>
        /// <returns>UserRoleECR or Default(null)</returns>
        UserRoleECR GetExistingUserRoleECR(int URId, int ECRId);
        UserRoleECR GetExistingUserRoleECRByUser(int UserId, int ECRId);
        #endregion

        #region UserRoleECO
        IEnumerable<UserRoleECO> GetUserRolesForECO(int ECOId);
        IEnumerable<ECO> GetECOWithUserRole(int URId);
        UserRoleECO GetExistingUserRoleECO(int URId, int ECOId);
        UserRoleECO GetExistingUserRoleECOByUser(int UserId, int ECOId);
        #endregion

        #region UserRoleECN
        IEnumerable<UserRoleECN> GetUserRolesForECN(int ECNId);
        IEnumerable<ECN> GetECNWithUserRole(int URId);
        UserRoleECN GetExistingUserRoleECN(int URId, int ECNId);
        UserRoleECN GetExistingUserRoleECNByUser(int UserId, int ECRId);
        #endregion

        #region Notifications
        IEnumerable<Notifications> GetNotificationsForECR(int ECRId);
        Notifications GetNotificationById(int NotificationId);
        /// <summary>
        /// Gets the Notification from the database using the User Id and ECRId.
        /// </summary>
        /// <param name="UserId">User Id</param>
        /// <param name="ECRId">ECR Id</param>
        /// <returns>Notification or Default(null)</returns>
        Notifications GetNotificationByUserAndECR(int UserId, int ECRId);
        Notifications GetNotificationByUserAndECO(int UserId, int ECOId);
        Notifications GetNotificationByUserAndECN(int UserId, int ECNId);
        #endregion

        #region ECRHasECO
        IEnumerable<ECRHasECO> GetRelatedECOByECR(int ECRId);
        IEnumerable<ECRHasECO> GetRelatedECRByECO(int ECOId);
        ECRHasECO GetRecordByCompKey(int ECRId, int ECOId);

        #endregion

        #region ECNHasECO
        ECNHasECO GetECNHasECORecord(int ECOId, int ECNId);
        IEnumerable<ECNHasECO> GetRelatedECOByECNId(int ECNId);

        #endregion

        #region ProductECR
        IEnumerable<ProductECR> GetProductECRsByECR(int ECRId);
        IEnumerable<ProductECR> GetProductECRsByProduct(int ProductId);
        ProductECR GetExistingProductECR(int ProductId, int ECRId);
        #endregion

        #region ProductECO
        IEnumerable<ProductECO> GetProductECOsByECO(int ECOId);
        IEnumerable<ProductECO> GetProductECOsByProduct(int ProductId);
        ProductECO GetExistingProductECO(int ProductId, int ECOId);
        #endregion
    }
}
