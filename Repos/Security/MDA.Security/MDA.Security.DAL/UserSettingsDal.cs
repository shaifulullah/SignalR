namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class UserSettingsDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteUserSettings(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(new UserSettings { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Get UserSettings For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public UserSettings GetUserSettingsForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserSettingsSet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get UserSettings For Key Name And User In Company In Application Id
        /// </summary>
        /// <param name="keyName">Key Name</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Business Entity</returns>
        public UserSettings GetUserSettingsForKeyNameAndUserInCompanyInApplicationId(string keyName, int userInCompanyInApplicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserSettingsSet.Where(x => x.LnUserInCompanyInApplicationId == userInCompanyInApplicationId).FirstOrDefault(x => x.KeyName == keyName);
            }
        }

        /// <summary>
        /// Get UserSettings List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserSettings> GetUserSettingsList(IEnumerable<Sort> sort, LinqFilter filter, int userInCompanyInApplicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserSettingsSet.Where(x => x.LnUserInCompanyInApplicationId == userInCompanyInApplicationId).ToListResult(sort, filter);
            }
        }

        /// <summary>
        /// Get UserSettings List For User Account Id And Company Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="companyId">Company Id</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserSettings> GetUserSettingsListForUserAccountIdAndCompanyId(int userAccountId, int companyId, string applicationCode)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserSettingsSet.Include(x => x.UserInCompanyInApplicationObj).Include(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj)
                    .Include(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.ApplicationObj)
                    .Where(x => x.UserInCompanyInApplicationObj.LnUserAccountId == userAccountId)
                    .Where(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.LnCompanyId == companyId)
                    .Where(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.ApplicationObj.Code == applicationCode).ToList();
            }
        }

        /// <summary>
        /// Get UserSettings Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<UserSettings> GetUserSettingsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userInCompanyInApplicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserSettingsSet.Where(x => x.LnUserInCompanyInApplicationId == userInCompanyInApplicationId).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userSettings">UserSettings Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertUserSettings(UserSettings userSettings, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(userSettings).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="userSettings">UserSettings Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateUserSettings(UserSettings userSettings, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(userSettings).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}