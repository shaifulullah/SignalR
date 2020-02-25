namespace MDA.Security.DAL
{
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class UserPreferencesDal
    {
        /// <summary>
        /// Get UserPreferences
        /// </summary>
        /// <param name="securityItemId">Security Item Id</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Business Entity</returns>
        public UserPreferences GetUserPreferences(int securityItemId, int userInCompanyInApplicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserPreferencesSet.Where(x => x.LnSecurityItemId == securityItemId).FirstOrDefault(x => x.LnUserInCompanyInApplicationId == userInCompanyInApplicationId);
            }
        }

        /// <summary>
        /// Get UserPreferences List For Code And Application Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserPreferences> GetUserPreferencesListForCodeAndApplicationCode(string code, string applicationCode, int companyId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserPreferencesSet.Include(x => x.UserInCompanyInApplicationObj).Include(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj)
                    .Include(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.ApplicationObj).Where(x => x.SecurityItemObj.Code == code)
                    .Where(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.ApplicationObj.Code == applicationCode)
                    .Where(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.LnCompanyId == companyId).ToList();
            }
        }

        /// <summary>
        /// Get UserPreferences List For User In Company In Application Id
        /// </summary>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserPreferences> GetUserPreferencesListForUserInCompanyInApplicationId(int userInCompanyInApplicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserPreferencesSet.Include(x => x.SecurityItemObj).Where(x => x.LnUserInCompanyInApplicationId == userInCompanyInApplicationId).ToList();
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userPreferences">UserPreferences Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertUserPreferences(UserPreferences userPreferences, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(userPreferences).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="userPreferences">UserPreferences Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateUserPreferences(UserPreferences userPreferences, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(userPreferences).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}