namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IBusinessEntityRestrictionByUserBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteBusinessEntityRestrictionByUser(int id, string userName);

        /// <summary>
        /// Get DropDown List of BusinessEntityRestrictionByUser
        /// </summary>
        /// <returns>DropDown List</returns>
        IEnumerable<SelectListItem> GetBusinessEntityRestrictionByUserDropDownList();

        /// <summary>
        /// Get BusinessEntityRestrictionByUser For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        BusinessEntityRestrictionByUserAccounts GetBusinessEntityRestrictionByUserForId(int id);

        /// <summary>
        /// Get BusinessEntityRestrictionByUser For Value And User Account Id
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Business Entity</returns>
        BusinessEntityRestrictionByUser GetBusinessEntityRestrictionByUserForValueAndUserAccountId(string value, int userAccountId, int securityBusinessEntitiesId);

        /// <summary>
        /// Get BusinessEntityRestrictionByUser List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<BusinessEntityRestrictionByUserAccounts> GetBusinessEntityRestrictionByUserList(IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId, bool isHideTerminated);

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
        DataSourceResult<BusinessEntityRestrictionByUserAccounts> GetBusinessEntityRestrictionByUserPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId, bool isHideTerminated);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="businessEntityRestrictionByUser">BusinessEntityRestrictionByUser Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertBusinessEntityRestrictionByUser(BusinessEntityRestrictionByUser businessEntityRestrictionByUser, string userName);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="businessEntityRestrictionByUser">BusinessEntityRestrictionByUser Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool UpdateBusinessEntityRestrictionByUser(BusinessEntityRestrictionByUser businessEntityRestrictionByUser, string userName);
    }
}