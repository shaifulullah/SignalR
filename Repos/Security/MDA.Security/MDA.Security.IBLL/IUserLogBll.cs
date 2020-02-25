namespace MDA.Security.IBLL
{
    using Models;

    public interface IUserLogBll
    {
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userLog">UserLog Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertUserLog(UserLog userLog, string userName);
    }
}