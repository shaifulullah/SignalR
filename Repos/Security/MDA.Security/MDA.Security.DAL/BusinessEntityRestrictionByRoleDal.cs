namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    public class BusinessEntityRestrictionByRoleDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteBusinessEntityRestrictionByRole(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(new BusinessEntityRestrictionByRole { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Get DropDown List of BusinessEntityRestrictionByRole
        /// </summary>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetBusinessEntityRestrictionByRoleDropDownList()
        {
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityRestrictionByRoleSet.AsEnumerable().Where(x => x.Id != 0).OrderBy(x => x.Value)
                    .Select(x => new SelectListItem { Value = x.Id.ToString(CultureInfo.InvariantCulture), Text = x.Value }).ToList();
            }
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByRole For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public BusinessEntityRestrictionByRole GetBusinessEntityRestrictionByRoleForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityRestrictionByRoleSet.Include(x => x.SecurityRolesObj).Include(x => x.SecurityRolesObj.CompanyInApplicationObj)
                    .Include(x => x.SecurityRolesObj.CompanyInApplicationObj.ApplicationObj).Include(x => x.SecurityRolesObj.CompanyInApplicationObj.CompanyObj)
                    .FirstOrDefault(x => x.Id == id);
            }
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
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityRestrictionByRoleSet.Where(x => x.LnSecurityBusinessEntitiesId == securityBusinessEntitiesId)
                    .Where(x => x.LnSecurityRolesId == securityRolesId).FirstOrDefault(x => x.Value == value);
            }
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
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityRestrictionByRoleSet.Include(x => x.SecurityRolesObj).Include(x => x.SecurityRolesObj.CompanyInApplicationObj)
                    .Include(x => x.SecurityRolesObj.CompanyInApplicationObj.ApplicationObj).Include(x => x.SecurityRolesObj.CompanyInApplicationObj.CompanyObj)
                    .Where(x => !x.SecurityRolesObj.IsDeleted).Where(x => x.LnSecurityBusinessEntitiesId == securityBusinessEntitiesId).ToListResult(sort, filter);
            }
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
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityRestrictionByRoleSet.Include(x => x.SecurityRolesObj).Include(x => x.SecurityRolesObj.CompanyInApplicationObj)
                    .Include(x => x.SecurityRolesObj.CompanyInApplicationObj.ApplicationObj).Include(x => x.SecurityRolesObj.CompanyInApplicationObj.CompanyObj)
                    .Where(x => !x.SecurityRolesObj.IsDeleted).Where(x => x.LnSecurityBusinessEntitiesId == securityBusinessEntitiesId).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="businessEntityRestrictionByRole">BusinessEntityRestrictionByRole Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertBusinessEntityRestrictionByRole(BusinessEntityRestrictionByRole businessEntityRestrictionByRole, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(businessEntityRestrictionByRole).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="businessEntityRestrictionByRole">BusinessEntityRestrictionByRole Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateBusinessEntityRestrictionByRole(BusinessEntityRestrictionByRole businessEntityRestrictionByRole, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(businessEntityRestrictionByRole).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}