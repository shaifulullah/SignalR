namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;

    public interface IUserInCompanyInApplicationBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteUserInCompanyInApplication(int id, string userName);

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
        DataSourceResult<UserAccountDetails> GetAvailableUserInCompanyInApplicationForSecurityRolesId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityRolesId, int companyInApplicationId, bool isHideTerminated);

        /// <summary>
        /// Get UserInCompanyInApplication For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        UserInCompanyInApplication GetUserInCompanyInApplicationForId(int id);

        /// <summary>
        /// Get UserInCompanyInApplication List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<UserAccountDetails> GetUserInCompanyInApplicationList(IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId, bool isHideTerminated);

        /// <summary>
        /// Get UserInCompanyInApplication List For Application Id And Company Id
        /// </summary>
        /// <param name="applicationId">Application Id</param>
        /// <param name="companyId">Company Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<UserAccountDetails> GetUserInCompanyInApplicationListForApplicationIdAndCompanyId(int applicationId, int companyId, bool isHideTerminated);

        /// <summary>
        /// Get UserInCompanyInApplication List For Company In Application Id
        /// </summary>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<UserInCompanyInApplication> GetUserInCompanyInApplicationListForCompanyInApplicationId(int companyInApplicationId);

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
        DataSourceResult<UserAccountDetails> GetUserInCompanyInApplicationPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId, bool isHideTerminated);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userInCompanyInApplication">UserInCompanyInApplication Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertUserInCompanyInApplication(UserInCompanyInApplication userInCompanyInApplication, string userName);
    }
}