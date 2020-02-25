namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IBusinessEntityRestrictionByRoleBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteBusinessEntityRestrictionByRole(int id, string userName);

        /// <summary>
        /// Get DropDown List of BusinessEntityRestrictionByRole
        /// </summary>
        /// <returns>DropDown List</returns>
        IEnumerable<SelectListItem> GetBusinessEntityRestrictionByRoleDropDownList();

        /// <summary>
        /// Get BusinessEntityRestrictionByRole For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        BusinessEntityRestrictionByRole GetBusinessEntityRestrictionByRoleForId(int id);

        /// <summary>
        /// Get BusinessEntityRestrictionByRole For Value And Security Roles Id
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Business Entity</returns>
        BusinessEntityRestrictionByRole GetBusinessEntityRestrictionByRoleForValueAndSecurityRolesId(string value, int securityRolesId, int securityBusinessEntitiesId);

        /// <summary>
        /// Get BusinessEntityRestrictionByRole List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<BusinessEntityRestrictionByRole> GetBusinessEntityRestrictionByRoleList(IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId);

        /// <summary>
        /// Get BusinessEntityRestrictionByRole Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<BusinessEntityRestrictionByRole> GetBusinessEntityRestrictionByRolePage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="businessEntityRestrictionByRole">BusinessEntityRestrictionByRole Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertBusinessEntityRestrictionByRole(BusinessEntityRestrictionByRole businessEntityRestrictionByRole, string userName);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="businessEntityRestrictionByRole">BusinessEntityRestrictionByRole Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool UpdateBusinessEntityRestrictionByRole(BusinessEntityRestrictionByRole businessEntityRestrictionByRole, string userName);
    }
}