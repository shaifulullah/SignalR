using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Services
{
    public interface INotificationSender
    {
        #region Notification ECR
        void SendNotificationOnAnyChangeECR(int ECRId, List<string> changes);
        void SendNotificationOnStatusChangeECR(int ECRId, string chnagedDetails);
        void SendNotificationOnDescriptionChangeECR(int ECRId, string changedDetails);
        void SendNotificationOnReasonChangeECR(int ECRId, string changedDetails);
        void SendNotificationOnImplementationDateChangeECR(int ECRId, string changedDetails);
        void SendNotificationOnApproversChangeECR(int ECRId, List<string> approverChanges);
        void SendNotificationOnApprovalECR(int ECRId, string chnagedDetails);
        void SendReminderEmailECR(int ECRId, string msg);
        #endregion

        #region Notification ECO
        void SendNotificationOnAnyChangeECO(int ECOId, List<string> msg);
        void SendNotificationOnStatusChangeECO(int ECOId, string chnagedDetails);
        void SendNotificationOnDescriptionChangeECO(int ECOId, string chnagedDetails);
        void SendNotificationOnReasonChangeECO(int ECOId, string chnagedDetails);
        void SendNotificationOnImplementationDateChangeECO(int ECOId, string chnagedDetails);
        void SendNotificationOnApproversChangeECO(int ECOId, List<string> approverChanges);
        void SendNotificationOnApprovalECO(int ECOId, string chnagedDetails);
        void SendReminderEmailECO(int ECOId, string msg);
        #endregion

        #region Notification ECN
        void SendNotificationOnAnyChangeECN(int ECNId, List<string> msg);
        void SendNotificationOnStatusChangeECN(int ECNId, string changedDetails);
        void SendNotificationOnDescriptionChangeECN(int ECNId, string changedDetails);
        void SendNotificationOnReasonChangeECN(int ECNId);
        void SendNotificationOnImplementationDateChangeECN(int ECNId);
        void SendNotificationOnApproversChangeECN(int ECNId, List<string> approverChanges);
        void SendNotificationOnApprovalECN(int ECNId, string chnagedDetails);
        void SendReminderEmailECN(int ECNId, string msg);
        #endregion
    }
}
