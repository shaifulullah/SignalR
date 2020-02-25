namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class BusinessEntityRestrictionByUserBll : IBusinessEntityRestrictionByUserBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteBusinessEntityRestrictionByUser(int id, string userName)
        {
            var businessEntityRestrictionByUserDal = new BusinessEntityRestrictionByUserDal();
            return businessEntityRestrictionByUserDal.DeleteBusinessEntityRestrictionByUser(id, userName);
        }

        /// <summary>
        /// Get DropDown List of BusinessEntityRestrictionByUser
        /// </summary>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetBusinessEntityRestrictionByUserDropDownList()
        {
            var businessEntityRestrictionByUserDal = new BusinessEntityRestrictionByUserDal();
            return businessEntityRestrictionByUserDal.GetBusinessEntityRestrictionByUserDropDownList();
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByUser For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public BusinessEntityRestrictionByUserAccounts GetBusinessEntityRestrictionByUserForId(int id)
        {
            var businessEntityRestrictionByUserDal = new BusinessEntityRestrictionByUserDal();
            return businessEntityRestrictionByUserDal.GetBusinessEntityRestrictionByUserForId(id);
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByUser For Value And User Account Id
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Business Entity</returns>
        public BusinessEntityRestrictionByUser GetBusinessEntityRestrictionByUserForValueAndUserAccountId(string value, int userAccountId, int securityBusinessEntitiesId)
        {
            var businessEntityRestrictionByUserDal = new BusinessEntityRestrictionByUserDal();
            return businessEntityRestrictionByUserDal.GetBusinessEntityRestrictionByUserForValueAndUserAccountId(value, userAccountId, securityBusinessEntitiesId);
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByUser List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<BusinessEntityRestrictionByUserAccounts> GetBusinessEntityRestrictionByUserList(IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId, bool isHideTerminated)
        {
            var businessEntityRestrictionByUserDal = new BusinessEntityRestrictionByUserDal();
            return businessEntityRestrictionByUserDal.GetBusinessEntityRestrictionByUserList(sort, filter, securityBusinessEntitiesId, isHideTerminated);
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByUser Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<BusinessEntityRestrictionByUserAccounts> GetBusinessEntityRestrictionByUserPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId, bool isHideTerminated)
        {
            var businessEntityRestrictionByUserDal = new BusinessEntityRestrictionByUserDal();
            return businessEntityRestrictionByUserDal.GetBusinessEntityRestrictionByUserPage(take, skip, sort, filter, securityBusinessEntitiesId, isHideTerminated);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="businessEntityRestrictionByUser">BusinessEntityRestrictionByUser Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertBusinessEntityRestrictionByUser(BusinessEntityRestrictionByUser businessEntityRestrictionByUser, string userName)
        {
            var businessEntityRestrictionByUserDal = new BusinessEntityRestrictionByUserDal();
            return businessEntityRestrictionByUserDal.InsertBusinessEntityRestrictionByUser(businessEntityRestrictionByUser, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="businessEntityRestrictionByUser">BusinessEntityRestrictionByUser Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateBusinessEntityRestrictionByUser(BusinessEntityRestrictionByUser businessEntityRestrictionByUser, string userName)
        {
            var businessEntityRestrictionByUserDal = new BusinessEntityRestrictionByUserDal();
            return businessEntityRestrictionByUserDal.UpdateBusinessEntityRestrictionByUser(businessEntityRestrictionByUser, userName);
        }
    }
}