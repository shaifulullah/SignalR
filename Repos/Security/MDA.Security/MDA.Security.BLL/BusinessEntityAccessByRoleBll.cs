namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class BusinessEntityAccessByRoleBll : IBusinessEntityAccessByRoleBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteBusinessEntityAccessByRole(int id, string userName)
        {
            var businessEntityAccessByRoleDal = new BusinessEntityAccessByRoleDal();
            return businessEntityAccessByRoleDal.DeleteBusinessEntityAccessByRole(id, userName);
        }

        /// <summary>
        /// Get DropDown List of BusinessEntityAccessByRole
        /// </summary>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetBusinessEntityAccessByRoleDropDownList()
        {
            var businessEntityAccessByRoleDal = new BusinessEntityAccessByRoleDal();
            return businessEntityAccessByRoleDal.GetBusinessEntityAccessByRoleDropDownList();
        }

        /// <summary>
        /// Get BusinessEntityAccessByRole For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public BusinessEntityAccessByRole GetBusinessEntityAccessByRoleForId(int id)
        {
            var businessEntityAccessByRoleDal = new BusinessEntityAccessByRoleDal();
            return businessEntityAccessByRoleDal.GetBusinessEntityAccessByRoleForId(id);
        }

        /// <summary>
        /// Get BusinessEntityAccessByRole For Value And Security Roles Id
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Business Entity</returns>
        public BusinessEntityAccessByRole GetBusinessEntityAccessByRoleForValueAndSecurityRolesId(string value, int securityRolesId, int securityBusinessEntitiesId)
        {
            var businessEntityAccessByRoleDal = new BusinessEntityAccessByRoleDal();
            return businessEntityAccessByRoleDal.GetBusinessEntityAccessByRoleForValueAndSecurityRolesId(value, securityRolesId, securityBusinessEntitiesId);
        }

        /// <summary>
        /// Get BusinessEntityAccessByRole List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<BusinessEntityAccessByRole> GetBusinessEntityAccessByRoleList(IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId)
        {
            var businessEntityAccessByRoleDal = new BusinessEntityAccessByRoleDal();
            return businessEntityAccessByRoleDal.GetBusinessEntityAccessByRoleList(sort, filter, securityBusinessEntitiesId);
        }

        /// <summary>
        /// Get BusinessEntityAccessByRole Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<BusinessEntityAccessByRole> GetBusinessEntityAccessByRolePage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId)
        {
            var businessEntityAccessByRoleDal = new BusinessEntityAccessByRoleDal();
            return businessEntityAccessByRoleDal.GetBusinessEntityAccessByRolePage(take, skip, sort, filter, securityBusinessEntitiesId);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="businessEntityAccessByRole">BusinessEntityAccessByRole Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertBusinessEntityAccessByRole(BusinessEntityAccessByRole businessEntityAccessByRole, string userName)
        {
            var businessEntityAccessByRoleDal = new BusinessEntityAccessByRoleDal();
            return businessEntityAccessByRoleDal.InsertBusinessEntityAccessByRole(businessEntityAccessByRole, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="businessEntityAccessByRole">BusinessEntityAccessByRole Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateBusinessEntityAccessByRole(BusinessEntityAccessByRole businessEntityAccessByRole, string userName)
        {
            var businessEntityAccessByRoleDal = new BusinessEntityAccessByRoleDal();
            return businessEntityAccessByRoleDal.UpdateBusinessEntityAccessByRole(businessEntityAccessByRole, userName);
        }
    }
}