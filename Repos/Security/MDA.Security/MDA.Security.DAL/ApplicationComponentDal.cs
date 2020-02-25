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

    public class ApplicationComponentDal : IConfigurationDal<ApplicationComponent>
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
                db.Entry(new ApplicationComponent { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Get DropDown List of ApplicationComponent
        /// </summary>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetConfigurationDropDownList()
        {
            using (var db = new ApplicationDBContext())
            {
                return db.ApplicationComponentSet.AsEnumerable().Where(x => x.Id != 0).OrderBy(x => x.Description)
                    .Select(x => new SelectListItem { Value = x.Id.ToString(CultureInfo.InvariantCulture), Text = x.Description }).ToList();
            }
        }

        /// <summary>
        /// Get ApplicationComponent For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public ApplicationComponent GetConfigurationForCode(string code)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.ApplicationComponentSet.FirstOrDefault(x => x.Code == code);
            }
        }

        /// <summary>
        /// Get ApplicationComponent For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public ApplicationComponent GetConfigurationForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.ApplicationComponentSet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get ApplicationComponent List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<ApplicationComponent> GetConfigurationList(IEnumerable<Sort> sort, LinqFilter filter)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.ApplicationComponentSet.Where(x => x.Id != 0).ToListResult(sort, filter);
            }
        }

        /// <summary>
        /// Get ApplicationComponent Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<ApplicationComponent> GetConfigurationPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.ApplicationComponentSet.Where(x => x.Id != 0).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="applicationComponent">ApplicationComponent Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertConfiguration(ApplicationComponent applicationComponent, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(applicationComponent).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="applicationComponent">ApplicationComponent Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateConfiguration(ApplicationComponent applicationComponent, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(applicationComponent).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}