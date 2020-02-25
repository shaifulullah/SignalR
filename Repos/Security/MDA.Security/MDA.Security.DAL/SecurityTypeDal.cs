namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    public class SecurityTypeDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteSecurityType(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(new SecurityType { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Get DropDown List of SecurityType
        /// </summary>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetSecurityTypeDropDownList()
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityTypeSet.AsEnumerable().Where(x => x.Id != 0).OrderBy(x => x.Description)
                    .Select(x => new SelectListItem { Value = x.Id.ToString(CultureInfo.InvariantCulture), Text = x.Description }).ToList();
            }
        }

        /// <summary>
        /// Get SecurityType For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public SecurityType GetSecurityTypeForCode(string code)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityTypeSet.FirstOrDefault(x => x.Code == code);
            }
        }

        /// <summary>
        /// Get SecurityType For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public SecurityType GetSecurityTypeForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityTypeSet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get SecurityType List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityType> GetSecurityTypeList(IEnumerable<Sort> sort, LinqFilter filter)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityTypeSet.Where(x => x.Id != 0).ToListResult(sort, filter);
            }
        }

        /// <summary>
        /// Get SecurityType Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<SecurityType> GetSecurityTypePage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityTypeSet.Where(x => x.Id != 0).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityType">SecurityType Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertSecurityType(SecurityType securityType, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(securityType).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityType">SecurityType Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateSecurityType(SecurityType securityType, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(securityType).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}