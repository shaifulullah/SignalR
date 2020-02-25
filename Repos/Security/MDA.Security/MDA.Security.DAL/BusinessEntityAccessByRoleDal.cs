namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    public class BusinessEntityAccessByRoleDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteBusinessEntityAccessByRole(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(new BusinessEntityAccessByRole { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Get DropDown List of BusinessEntityAccessByRole
        /// </summary>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetBusinessEntityAccessByRoleDropDownList()
        {
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityAccessByRoleSet.AsEnumerable().Where(x => x.Id != 0).OrderBy(x => x.Value)
                    .Select(x => new SelectListItem { Value = x.Id.ToString(CultureInfo.InvariantCulture), Text = x.Value }).ToList();
            }
        }

        /// <summary>
        /// Get BusinessEntityAccessByRole For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public BusinessEntityAccessByRole GetBusinessEntityAccessByRoleForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityAccessByRoleSet.Include(x => x.SecurityRolesObj).Include(x => x.SecurityRolesObj.CompanyInApplicationObj)
                    .Include(x => x.SecurityRolesObj.CompanyInApplicationObj.ApplicationObj).Include(x => x.SecurityRolesObj.CompanyInApplicationObj.CompanyObj)
                    .FirstOrDefault(x => x.Id == id);
            }
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
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityAccessByRoleSet.Where(x => x.LnSecurityBusinessEntitiesId == securityBusinessEntitiesId)
                    .Where(x => x.LnSecurityRolesId == securityRolesId).FirstOrDefault(x => x.Value == value);
            }
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
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityAccessByRoleSet.Include(x => x.SecurityRolesObj).Where(x => !x.SecurityRolesObj.IsDeleted)
                    .Include(x => x.SecurityRolesObj.CompanyInApplicationObj).Include(x => x.SecurityRolesObj.CompanyInApplicationObj.ApplicationObj)
                    .Include(x => x.SecurityRolesObj.CompanyInApplicationObj.CompanyObj)
                    .Where(x => x.LnSecurityBusinessEntitiesId == securityBusinessEntitiesId).ToListResult(sort, filter);
            }
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
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityAccessByRoleSet.Include(x => x.SecurityRolesObj).Where(x => !x.SecurityRolesObj.IsDeleted)
                    .Include(x => x.SecurityRolesObj.CompanyInApplicationObj).Include(x => x.SecurityRolesObj.CompanyInApplicationObj.ApplicationObj)
                    .Include(x => x.SecurityRolesObj.CompanyInApplicationObj.CompanyObj)
                    .Where(x => x.LnSecurityBusinessEntitiesId == securityBusinessEntitiesId).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="businessEntityAccessByRole">BusinessEntityAccessByRole Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertBusinessEntityAccessByRole(BusinessEntityAccessByRole businessEntityAccessByRole, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(businessEntityAccessByRole).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="businessEntityAccessByRole">BusinessEntityAccessByRole Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateBusinessEntityAccessByRole(BusinessEntityAccessByRole businessEntityAccessByRole, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(businessEntityAccessByRole).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}