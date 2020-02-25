namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    public class BusinessEntityRestrictionByADDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteBusinessEntityRestrictionByAD(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(new BusinessEntityRestrictionByAD { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Get DropDown List of BusinessEntityRestrictionByAD
        /// </summary>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetBusinessEntityRestrictionByADDropDownList()
        {
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityRestrictionByADSet.AsEnumerable().Where(x => x.Id != 0).OrderBy(x => x.Value)
                    .Select(x => new SelectListItem { Value = x.Id.ToString(CultureInfo.InvariantCulture), Text = x.Value }).ToList();
            }
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByAD For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public BusinessEntityRestrictionByAD GetBusinessEntityRestrictionByADForCode(string code)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityRestrictionByADSet.FirstOrDefault(x => x.Value == code);
            }
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByAD For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public BusinessEntityRestrictionByAD GetBusinessEntityRestrictionByADForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityRestrictionByADSet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByAD List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<BusinessEntityRestrictionByAD> GetBusinessEntityRestrictionByADList(IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityRestrictionByADSet.Where(x => x.LnSecurityBusinessEntitiesId == securityBusinessEntitiesId).ToListResult(sort, filter);
            }
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByAD Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<BusinessEntityRestrictionByAD> GetBusinessEntityRestrictionByADPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityRestrictionByADSet.Where(x => x.LnSecurityBusinessEntitiesId == securityBusinessEntitiesId).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="businessEntityRestrictionByAD">BusinessEntityRestrictionByAD Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertBusinessEntityRestrictionByAD(BusinessEntityRestrictionByAD businessEntityRestrictionByAD, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(businessEntityRestrictionByAD).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="businessEntityRestrictionByAD">BusinessEntityRestrictionByAD Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateBusinessEntityRestrictionByAD(BusinessEntityRestrictionByAD businessEntityRestrictionByAD, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(businessEntityRestrictionByAD).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}