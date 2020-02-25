namespace MDA.Security.Service
{
    using Models;
    using System.ServiceModel;

    [ServiceContract(Namespace = "MDA.Security.Service")]
    public interface IUserLogService
    {
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userLog">UserLog Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        [OperationContract]
        bool InsertUserLog(UserLog userLog, string userName);
    }
}