namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class BusinessEntityAccessByUserBll : IBusinessEntityAccessByUserBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteBusinessEntityAccessByUser(int id, string userName)
        {
            var businessEntityAccessByUserDal = new BusinessEntityAccessByUserDal();
            return businessEntityAccessByUserDal.DeleteBusinessEntityAccessByUser(id, userName);
        }

        /// <summary>
        /// Get DropDown List of BusinessEntityAccessByUser
        /// </summary>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetBusinessEntityAccessByUserDropDownList()
        {
            var businessEntityAccessByUserDal = new BusinessEntityAccessByUserDal();
            return businessEntityAccessByUserDal.GetBusinessEntityAccessByUserDropDownList();
        }

        /// <summary>
        /// Get BusinessEntityAccessByUser For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public BusinessEntityAccessByUserAccounts GetBusinessEntityAccessByUserForId(int id)
        {
            var businessEntityAccessByUserDal = new BusinessEntityAccessByUserDal();
            return businessEntityAccessByUserDal.GetBusinessEntityAccessByUserForId(id);
        }

        /// <summary>
        /// Get BusinessEntityAccessByUser For Value And User Account Id
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Business Entity</returns>
        public BusinessEntityAccessByUser GetBusinessEntityAccessByUserForValueAndUserAccountId(string value, int userAccountId, int securityBusinessEntitiesId)
        {
            var businessEntityAccessByUserDal = new BusinessEntityAccessByUserDal();
            return businessEntityAccessByUserDal.GetBusinessEntityAccessByUserForValueAndUserAccountId(value, userAccountId, securityBusinessEntitiesId);
        }

        /// <summary>
        /// Get BusinessEntityAccessByUser List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<BusinessEntityAccessByUserAccounts> GetBusinessEntityAccessByUserList(IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId, bool isHideTerminated)
        {
            var businessEntityAccessByUserDal = new BusinessEntityAccessByUserDal();
            return businessEntityAccessByUserDal.GetBusinessEntityAccessByUserList(sort, filter, securityBusinessEntitiesId, isHideTerminated);
        }

        /// <summary>
        /// Get BusinessEntityAccessByUser Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<BusinessEntityAccessByUserAccounts> GetBusinessEntityAccessByUserPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId, bool isHideTerminated)
        {
            var businessEntityAccessByUserDal = new BusinessEntityAccessByUserDal();
            return businessEntityAccessByUserDal.GetBusinessEntityAccessByUserPage(take, skip, sort, filter, securityBusinessEntitiesId, isHideTerminated);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="businessEntityAccessByUser">BusinessEntityAccessByUser Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertBusinessEntityAccessByUser(BusinessEntityAccessByUser businessEntityAccessByUser, string userName)
        {
            var businessEntityAccessByUserDal = new BusinessEntityAccessByUserDal();
            return businessEntityAccessByUserDal.InsertBusinessEntityAccessByUser(businessEntityAccessByUser, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="businessEntityAccessByUser">BusinessEntityAccessByUser Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateBusinessEntityAccessByUser(BusinessEntityAccessByUser businessEntityAccessByUser, string userName)
        {
            var businessEntityAccessByUserDal = new BusinessEntityAccessByUserDal();
            return businessEntityAccessByUserDal.UpdateBusinessEntityAccessByUser(businessEntityAccessByUser, userName);
        }
    }
}