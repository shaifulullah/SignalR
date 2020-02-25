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

    public class LogActionDal : IConfigurationDal<LogAction>
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
                db.Entry(new LogAction { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Get DropDown List of LogAction
        /// </summary>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetConfigurationDropDownList()
        {
            using (var db = new ApplicationDBContext())
            {
                return db.LogActionSet.AsEnumerable().Where(x => x.Id != 0).OrderBy(x => x.Description)
                    .Select(x => new SelectListItem { Value = x.Id.ToString(CultureInfo.InvariantCulture), Text = x.Description }).ToList();
            }
        }

        /// <summary>
        /// Get LogAction For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public LogAction GetConfigurationForCode(string code)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.LogActionSet.FirstOrDefault(x => x.Code == code);
            }
        }

        /// <summary>
        /// Get LogAction For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public LogAction GetConfigurationForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.LogActionSet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get LogAction List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<LogAction> GetConfigurationList(IEnumerable<Sort> sort, LinqFilter filter)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.LogActionSet.Where(x => x.Id != 0).ToListResult(sort, filter);
            }
        }

        /// <summary>
        /// Get LogAction Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<LogAction> GetConfigurationPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.LogActionSet.Where(x => x.Id != 0).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="logAction">LogAction Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertConfiguration(LogAction logAction, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(logAction).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="logAction">LogAction Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateConfiguration(LogAction logAction, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(logAction).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}