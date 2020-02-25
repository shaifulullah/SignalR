namespace MDA.Security.DAL
{
    using IDAL;
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    public class SecurityLevelDal : IConfigurationDal<SecurityLevel>
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteConfiguration(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(new SecurityLevel { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Get DropDown List of SecurityLevel
        /// </summary>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetConfigurationDropDownList()
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityLevelSet.AsEnumerable().Where(x => x.Id != 0).OrderBy(x => x.Description)
                    .Select(x => new SelectListItem { Value = x.Id.ToString(CultureInfo.InvariantCulture), Text = x.Description }).ToList();
            }
        }

        /// <summary>
        /// Get SecurityLevel For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public SecurityLevel GetConfigurationForCode(string code)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityLevelSet.FirstOrDefault(x => x.Code == code);
            }
        }

        /// <summary>
        /// Get SecurityLevel For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public SecurityLevel GetConfigurationForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityLevelSet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get SecurityLevel List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityLevel> GetConfigurationList(IEnumerable<Sort> sort, LinqFilter filter)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityLevelSet.Where(x => x.Id != 0).ToListResult(sort, filter);
            }
        }

        /// <summary>
        /// Get SecurityLevel Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<SecurityLevel> GetConfigurationPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityLevelSet.Where(x => x.Id != 0).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityLevel">SecurityLevel Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertConfiguration(SecurityLevel securityLevel, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(securityLevel).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityLevel">SecurityLevel Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateConfiguration(SecurityLevel securityLevel, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(securityLevel).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}