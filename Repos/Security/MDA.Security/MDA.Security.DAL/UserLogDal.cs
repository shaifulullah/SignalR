namespace MDA.Security.DAL
{
    using Models;
    using System.Data.Entity;

    public class UserLogDal
    {
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userLog">UserLog Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertUserLog(UserLog userLog, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(userLog).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}