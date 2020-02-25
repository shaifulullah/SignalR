namespace MDA.Security.BLL
{
    using IBLL;
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class UserAccountDetailsBll : IUserAccountDetailsBll
    {
        /// <summary>
        /// Get UserAccountDetails List For Application Id And Company Id
        /// </summary>
        /// <param name="applicationId">Application Id</param>
        /// <param name="companyId">Company Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserAccountDetails> GetUserAccountDetailsListForApplicationIdAndCompanyId(int applicationId, int companyId, bool isHideTerminated)
        {
            IUserInCompanyInApplicationBll iUserInCompanyInApplicationBll = new UserInCompanyInApplicationBll();
            var userInCompanyInApplicationListForApplicationIdAndCompanyId = iUserInCompanyInApplicationBll.GetUserInCompanyInApplicationListForApplicationIdAndCompanyId(applicationId, companyId, isHideTerminated);

            var userInCompanyInApplicationList = userInCompanyInApplicationListForApplicationIdAndCompanyId as List<UserAccountDetails> ?? userInCompanyInApplicationListForApplicationIdAndCompanyId.ToList();

            // Get SecurityUserRights
            ISecurityUserRightsBll iSecurityUserRightsBll = new SecurityUserRightsBll();
            userInCompanyInApplicationList.ForEach(x => x.SecurityRightsForUserAccountList = iSecurityUserRightsBll.GetSecurityUserRightsListForApplicationIdAndCompanyId(applicationId, companyId, x.Id));

            // Get SecurityUserInRoles
            ISecurityUserInRolesBll iSecurityUserInRolesBll = new SecurityUserInRolesBll();
            userInCompanyInApplicationList.ForEach(x => x.SecurityUserInRolesList = iSecurityUserInRolesBll.GetSecurityUserInRolesListForApplicationIdAndCompanyId(applicationId, companyId, x.Id));

            return userInCompanyInApplicationList;
        }
    }
}