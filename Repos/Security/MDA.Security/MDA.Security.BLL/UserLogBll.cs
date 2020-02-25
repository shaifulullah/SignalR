namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Models;

    public class UserLogBll : IUserLogBll
    {
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userLog">UserLog Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertUserLog(UserLog userLog, string userName)
        {
            var userLogDal = new UserLogDal();
            return userLogDal.InsertUserLog(userLog, userName);
        }
    }
}