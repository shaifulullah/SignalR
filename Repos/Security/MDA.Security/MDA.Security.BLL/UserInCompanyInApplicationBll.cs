namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class UserInCompanyInApplicationBll : IUserInCompanyInApplicationBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteUserInCompanyInApplication(int id, string userName)
        {
            var userInCompanyInApplicationDal = new UserInCompanyInApplicationDal();
            return userInCompanyInApplicationDal.DeleteUserInCompanyInApplication(id, userName);
        }

        /// <summary>
        /// Get Available UserInCompanyInApplication For Security Roles Id
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<UserAccountDetails> GetAvailableUserInCompanyInApplicationForSecurityRolesId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityRolesId, int companyInApplicationId, bool isHideTerminated)
        {
            ISecurityUserInRolesBll iSecurityUserInRolesBll = new SecurityUserInRolesBll();
            var securityUserInRolesList = iSecurityUserInRolesBll.GetSecurityUserInRolesListForSecurityRolesId(securityRolesId);

            var userAccountIdList = securityUserInRolesList == null ? new List<int>() : securityUserInRolesList.Select(x => x.UserInCompanyInApplicationObj.LnUserAccountId).ToList();

            var userInCompanyInApplicationDal = new UserInCompanyInApplicationDal();
            return userInCompanyInApplicationDal.GetAvailableUserInCompanyInApplicationForUserAccountIdList(take, skip, sort, filter, userAccountIdList, companyInApplicationId, isHideTerminated);
        }

        /// <summary>
        /// Get UserInCompanyInApplication For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public UserInCompanyInApplication GetUserInCompanyInApplicationForId(int id)
        {
            var userInCompanyInApplicationDal = new UserInCompanyInApplicationDal();
            return userInCompanyInApplicationDal.GetUserInCompanyInApplicationForId(id);
        }

        /// <summary>
        /// Get UserInCompanyInApplication List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserAccountDetails> GetUserInCompanyInApplicationList(IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId, bool isHideTerminated)
        {
            var userInCompanyInApplicationDal = new UserInCompanyInApplicationDal();
            return userInCompanyInApplicationDal.GetUserInCompanyInApplicationList(sort, filter, companyInApplicationId, isHideTerminated);
        }

        /// <summary>
        /// Get UserInCompanyInApplication List For Application Id And Company Id
        /// </summary>
        /// <param name="applicationId">Application Id</param>
        /// <param name="companyId">Company Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserAccountDetails> GetUserInCompanyInApplicationListForApplicationIdAndCompanyId(int applicationId, int companyId, bool isHideTerminated)
        {
            var userInCompanyInApplicationDal = new UserInCompanyInApplicationDal();
            return userInCompanyInApplicationDal.GetUserInCompanyInApplicationListForApplicationIdAndCompanyId(applicationId, companyId, isHideTerminated);
        }

        /// <summary>
        /// Get UserInCompanyInApplication List For Company In Application Id
        /// </summary>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserInCompanyInApplication> GetUserInCompanyInApplicationListForCompanyInApplicationId(int companyInApplicationId)
        {
            var userInCompanyInApplicationDal = new UserInCompanyInApplicationDal();
            return userInCompanyInApplicationDal.GetUserInCompanyInApplicationListForCompanyInApplicationId(companyInApplicationId);
        }

        /// <summary>
        /// Get UserInCompanyInApplication Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<UserAccountDetails> GetUserInCompanyInApplicationPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId, bool isHideTerminated)
        {
            var userInCompanyInApplicationDal = new UserInCompanyInApplicationDal();
            return userInCompanyInApplicationDal.GetUserInCompanyInApplicationPage(take, skip, sort, filter, companyInApplicationId, isHideTerminated);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userInCompanyInApplication">UserInCompanyInApplication Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertUserInCompanyInApplication(UserInCompanyInApplication userInCompanyInApplication, string userName)
        {
            var userInCompanyInApplicationDal = new UserInCompanyInApplicationDal();
            return userInCompanyInApplicationDal.InsertUserInCompanyInApplication(userInCompanyInApplication, userName);
        }
    }
}