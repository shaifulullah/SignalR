namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IBusinessEntityAccessByUserBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteBusinessEntityAccessByUser(int id, string userName);

        /// <summary>
        /// Get DropDown List of BusinessEntityAccessByUser
        /// </summary>
        /// <returns>DropDown List</returns>
        IEnumerable<SelectListItem> GetBusinessEntityAccessByUserDropDownList();

        /// <summary>
        /// Get BusinessEntityAccessByUser For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        BusinessEntityAccessByUserAccounts GetBusinessEntityAccessByUserForId(int id);

        /// <summary>
        /// Get BusinessEntityAccessByUser For Value And User Account Id
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Business Entity</returns>
        BusinessEntityAccessByUser GetBusinessEntityAccessByUserForValueAndUserAccountId(string value, int userAccountId, int securityBusinessEntitiesId);

        /// <summary>
        /// Get BusinessEntityAccessByUser List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<BusinessEntityAccessByUserAccounts> GetBusinessEntityAccessByUserList(IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId, bool isHideTerminated);

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
        DataSourceResult<BusinessEntityAccessByUserAccounts> GetBusinessEntityAccessByUserPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId, bool isHideTerminated);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="businessEntityAccessByUser">BusinessEntityAccessByUser Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertBusinessEntityAccessByUser(BusinessEntityAccessByUser businessEntityAccessByUser, string userName);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="businessEntityAccessByUser">BusinessEntityAccessByUser Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool UpdateBusinessEntityAccessByUser(BusinessEntityAccessByUser businessEntityAccessByUser, string userName);
    }
}