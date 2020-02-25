namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    public class ApplicationDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteApplication(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(new Application { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Get DropDown List of Application
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetApplicationDropDownList(int userAccountId)
        {
            using (var db = new ApplicationDBContext())
            {
                if (db.MasterUserSet.Any(x => x.LnUserAccountId == userAccountId))
                {
                    return db.ApplicationSet.Where(x => x.Id != 0).OrderBy(x => x.Description)
                        .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Description }).ToList();
                }
                else
                {
                    var applicationCodeList = db.BusinessEntityAccessByUserSet.Include(x => x.SecurityBusinessEntitiesObj)
                        .Where(x => x.SecurityBusinessEntitiesObj.Code == SecurityBusinessEntitiesCode.Application).Where(x => x.LnUserAccountId == userAccountId).Select(x => x.Value).ToList();

                    return db.ApplicationSet.Where(x => x.Id != 0).Where(x => applicationCodeList.Any(y => y == x.Code)).OrderBy(x => x.Description)
                        .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Description }).ToList();
                }
            }
        }

        /// <summary>
        /// Get Application For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public Application GetApplicationForCode(string code)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.ApplicationSet.FirstOrDefault(x => x.Code == code);
            }
        }

        /// <summary>
        /// Get Application For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public Application GetApplicationForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.ApplicationSet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get Application List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<Application> GetApplicationList(IEnumerable<Sort> sort, LinqFilter filter, int userAccountId)
        {
            using (var db = new ApplicationDBContext())
            {
                if (db.MasterUserSet.Any(x => x.LnUserAccountId == userAccountId))
                {
                    return db.ApplicationSet.Where(x => x.Id != 0).ToListResult(sort, filter);
                }
                else
                {
                    var applicationCodeList = db.BusinessEntityAccessByUserSet.Include(x => x.SecurityBusinessEntitiesObj)
                        .Where(x => x.SecurityBusinessEntitiesObj.Code == SecurityBusinessEntitiesCode.Application).Where(x => x.LnUserAccountId == userAccountId).Select(x => x.Value).ToList();

                    return db.ApplicationSet.Where(x => x.Id != 0).Where(x => applicationCodeList.Any(y => y == x.Code)).ToListResult(sort, filter);
                }
            }
        }

        /// <summary>
        /// Get Application Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<Application> GetApplicationPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userAccountId)
        {
            using (var db = new ApplicationDBContext())
            {
                if (db.MasterUserSet.Any(x => x.LnUserAccountId == userAccountId))
                {
                    return db.ApplicationSet.Where(x => x.Id != 0).ToDataSourceResult(take, skip, sort, filter);
                }
                else
                {
                    var applicationCodeList = db.BusinessEntityAccessByUserSet.Include(x => x.SecurityBusinessEntitiesObj)
                        .Where(x => x.SecurityBusinessEntitiesObj.Code == SecurityBusinessEntitiesCode.Application).Where(x => x.LnUserAccountId == userAccountId).Select(x => x.Value).ToList();

                    return db.ApplicationSet.Where(x => x.Id != 0).Where(x => applicationCodeList.Any(y => y == x.Code)).ToDataSourceResult(take, skip, sort, filter);
                }
            }
        }

        /// <summary>
        /// Get Available Application Page For User Account Id
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<Application> GetAvailableApplicationPageForUserAccountId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userAccountId)
        {
            using (var db = new ApplicationDBContext())
            {
                if (db.MasterUserSet.Any(x => x.LnUserAccountId == userAccountId))
                {
                    return db.ApplicationSet.Where(x => x.Id != 0)
                        .Where(x => !db.UserApplicationFavouritesSet.Where(y => y.LnUserAccountId == userAccountId).Any(y => y.LnApplicationId == x.Id))
                        .Where(x => db.UserInCompanyInApplicationSet.Where(z => z.CompanyInApplicationObj.LnApplicationId == x.Id).Any(z => z.LnUserAccountId == userAccountId)).ToDataSourceResult(take, skip, sort, filter);
                }
                else
                {
                    var applicationCodeList = db.BusinessEntityAccessByUserSet.Include(x => x.SecurityBusinessEntitiesObj)
                        .Where(x => x.SecurityBusinessEntitiesObj.Code == SecurityBusinessEntitiesCode.Application).Where(x => x.LnUserAccountId == userAccountId).Select(x => x.Value).ToList();

                    return db.ApplicationSet.Where(x => x.Id != 0).Where(x => applicationCodeList.Any(y => y == x.Code))
                        .Where(x => !db.UserApplicationFavouritesSet.Where(y => y.LnUserAccountId == userAccountId).Any(y => y.LnApplicationId == x.Id))
                        .Where(x => db.UserInCompanyInApplicationSet.Where(z => z.CompanyInApplicationObj.LnApplicationId == x.Id).Any(z => z.LnUserAccountId == userAccountId)).ToDataSourceResult(take, skip, sort, filter);
                }
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="application">Application Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertApplication(Application application, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(application).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="application">Application Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateApplication(Application application, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(application).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}