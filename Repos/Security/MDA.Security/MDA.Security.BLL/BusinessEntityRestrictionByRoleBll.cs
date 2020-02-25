namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class BusinessEntityRestrictionByRoleBll : IBusinessEntityRestrictionByRoleBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteBusinessEntityRestrictionByRole(int id, string userName)
        {
            var businessEntityRestrictionByRoleDal = new BusinessEntityRestrictionByRoleDal();
            return businessEntityRestrictionByRoleDal.DeleteBusinessEntityRestrictionByRole(id, userName);
        }

        /// <summary>
        /// Get DropDown List of BusinessEntityRestrictionByRole
        /// </summary>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetBusinessEntityRestrictionByRoleDropDownList()
        {
            var businessEntityRestrictionByRoleDal = new BusinessEntityRestrictionByRoleDal();
            return businessEntityRestrictionByRoleDal.GetBusinessEntityRestrictionByRoleDropDownList();
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByRole For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public BusinessEntityRestrictionByRole GetBusinessEntityRestrictionByRoleForId(int id)
        {
            var businessEntityRestrictionByRoleDal = new BusinessEntityRestrictionByRoleDal();
            return businessEntityRestrictionByRoleDal.GetBusinessEntityRestrictionByRoleForId(id);
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByRole For Value And Security Roles Id
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Business Entity</returns>
        public BusinessEntityRestrictionByRole GetBusinessEntityRestrictionByRoleForValueAndSecurityRolesId(string value, int securityRolesId, int securityBusinessEntitiesId)
        {
            var businessEntityRestrictionByRoleDal = new BusinessEntityRestrictionByRoleDal();
            return businessEntityRestrictionByRoleDal.GetBusinessEntityRestrictionByRoleForValueAndSecurityRolesId(value, securityRolesId, securityBusinessEntitiesId);
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByRole List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<BusinessEntityRestrictionByRole> GetBusinessEntityRestrictionByRoleList(IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId)
        {
            var businessEntityRestrictionByRoleDal = new BusinessEntityRestrictionByRoleDal();
            return businessEntityRestrictionByRoleDal.GetBusinessEntityRestrictionByRoleList(sort, filter, securityBusinessEntitiesId);
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByRole Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<BusinessEntityRestrictionByRole> GetBusinessEntityRestrictionByRolePage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId)
        {
            var businessEntityRestrictionByRoleDal = new BusinessEntityRestrictionByRoleDal();
            return businessEntityRestrictionByRoleDal.GetBusinessEntityRestrictionByRolePage(take, skip, sort, filter, securityBusinessEntitiesId);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="businessEntityRestrictionByRole">BusinessEntityRestrictionByRole Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertBusinessEntityRestrictionByRole(BusinessEntityRestrictionByRole businessEntityRestrictionByRole, string userName)
        {
            var businessEntityRestrictionByRoleDal = new BusinessEntityRestrictionByRoleDal();
            return businessEntityRestrictionByRoleDal.InsertBusinessEntityRestrictionByRole(businessEntityRestrictionByRole, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="businessEntityRestrictionByRole">BusinessEntityRestrictionByRole Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateBusinessEntityRestrictionByRole(BusinessEntityRestrictionByRole businessEntityRestrictionByRole, string userName)
        {
            var businessEntityRestrictionByRoleDal = new BusinessEntityRestrictionByRoleDal();
            return businessEntityRestrictionByRoleDal.UpdateBusinessEntityRestrictionByRole(businessEntityRestrictionByRole, userName);
        }
    }
}