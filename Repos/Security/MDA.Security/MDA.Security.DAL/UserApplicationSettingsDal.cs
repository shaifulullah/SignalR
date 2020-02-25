namespace MDA.Security.DAL
{
    using Models;
    using System.Data.Entity;
    using System.Linq;

    public class UserApplicationSettingsDal
    {
        /// <summary>
        /// Get UserApplicationSettings For User Account Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Business Entity</returns>
        public UserApplicationSettings GetUserApplicationSettingsForUserAccountId(int userAccountId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserApplicationSettingsSet.Include(x => x.CompanyObj).FirstOrDefault(x => x.LnUserAccountId == userAccountId);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userApplicationSettings">UserApplicationSettings Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertUserApplicationSettings(UserApplicationSettings userApplicationSettings, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(userApplicationSettings).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="userApplicationSettings">UserApplicationSettings Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateUserApplicationSettings(UserApplicationSettings userApplicationSettings, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(userApplicationSettings).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}