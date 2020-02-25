namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    public class BusinessEntityAccessByADDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteBusinessEntityAccessByAD(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(new BusinessEntityAccessByAD { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Get DropDown List of BusinessEntityAccessByAD
        /// </summary>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetBusinessEntityAccessByADDropDownList()
        {
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityAccessByADSet.AsEnumerable().Where(x => x.Id != 0).OrderBy(x => x.Value)
                    .Select(x => new SelectListItem { Value = x.Id.ToString(CultureInfo.InvariantCulture), Text = x.Value }).ToList();
            }
        }

        /// <summary>
        /// Get BusinessEntityAccessByAD For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public BusinessEntityAccessByAD GetBusinessEntityAccessByADForCode(string code)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityAccessByADSet.FirstOrDefault(x => x.Value == code);
            }
        }

        /// <summary>
        /// Get BusinessEntityAccessByAD For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public BusinessEntityAccessByAD GetBusinessEntityAccessByADForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityAccessByADSet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get BusinessEntityAccessByAD List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<BusinessEntityAccessByAD> GetBusinessEntityAccessByADList(IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityAccessByADSet.Where(x => x.LnSecurityBusinessEntitiesId == securityBusinessEntitiesId).ToListResult(sort, filter);
            }
        }

        /// <summary>
        /// Get BusinessEntityAccessByAD Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<BusinessEntityAccessByAD> GetBusinessEntityAccessByADPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityAccessByADSet.Where(x => x.LnSecurityBusinessEntitiesId == securityBusinessEntitiesId).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="businessEntityAccessByAD">BusinessEntityAccessByAD Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertBusinessEntityAccessByAD(BusinessEntityAccessByAD businessEntityAccessByAD, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(businessEntityAccessByAD).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="businessEntityAccessByAD">BusinessEntityAccessByAD Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateBusinessEntityAccessByAD(BusinessEntityAccessByAD businessEntityAccessByAD, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(businessEntityAccessByAD).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}