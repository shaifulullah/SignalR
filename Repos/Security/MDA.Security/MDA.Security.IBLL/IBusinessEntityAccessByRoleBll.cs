namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IBusinessEntityAccessByRoleBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteBusinessEntityAccessByRole(int id, string userName);

        /// <summary>
        /// Get DropDown List of BusinessEntityAccessByRole
        /// </summary>
        /// <returns>DropDown List</returns>
        IEnumerable<SelectListItem> GetBusinessEntityAccessByRoleDropDownList();

        /// <summary>
        /// Get BusinessEntityAccessByRole For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        BusinessEntityAccessByRole GetBusinessEntityAccessByRoleForId(int id);

        /// <summary>
        /// Get BusinessEntityAccessByRole For Value And Security Roles Id
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Business Entity</returns>
        BusinessEntityAccessByRole GetBusinessEntityAccessByRoleForValueAndSecurityRolesId(string value, int securityRolesId, int securityBusinessEntitiesId);

        /// <summary>
        /// Get BusinessEntityAccessByRole List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<BusinessEntityAccessByRole> GetBusinessEntityAccessByRoleList(IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId);

        /// <summary>
        /// Get BusinessEntityAccessByRole Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<BusinessEntityAccessByRole> GetBusinessEntityAccessByRolePage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="businessEntityAccessByRole">BusinessEntityAccessByRole Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertBusinessEntityAccessByRole(BusinessEntityAccessByRole businessEntityAccessByRole, string userName);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="businessEntityAccessByRole">BusinessEntityAccessByRole Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool UpdateBusinessEntityAccessByRole(BusinessEntityAccessByRole businessEntityAccessByRole, string userName);
    }
}