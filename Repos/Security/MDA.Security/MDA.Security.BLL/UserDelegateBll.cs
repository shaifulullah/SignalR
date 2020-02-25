namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;

    public class UserDelegateBll : IUserDelegateBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteUserDelegate(int id, string userName)
        {
            var userDelegateDal = new UserDelegateDal();
            return userDelegateDal.DeleteUserDelegate(id, userName);
        }

        /// <summary>
        /// Get Available User Delegate Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountIdList">User Account Id List</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<UserAccountDetails> GetAvailableUserDelegatePage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, IEnumerable<int> userAccountIdList, bool isHideTerminated)
        {
            var userDelegateDal = new UserDelegateDal();
            return userDelegateDal.GetAvailableUserDelegatePage(take, skip, sort, filter, userAccountIdList, isHideTerminated);
        }

        /// <summary>
        /// Get UserDelegate Count For User Account Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Count of User Delegate</returns>
        public int GetUserDelegateCountForUserAccountId(int userAccountId)
        {
            var userDelegateDal = new UserDelegateDal();
            return userDelegateDal.GetUserDelegateCountForUserAccountId(userAccountId);
        }

        /// <summary>
        /// Get UserDelegate For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public UserDelegate GetUserDelegateForId(int id)
        {
            var userDelegateDal = new UserDelegateDal();
            return userDelegateDal.GetUserDelegateForId(id);
        }

        /// <summary>
        /// Get UserDelegate List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserDelegate> GetUserDelegateList(IEnumerable<Sort> sort, LinqFilter filter, int userAccountId)
        {
            var userDelegateDal = new UserDelegateDal();
            return userDelegateDal.GetUserDelegateList(sort, filter, userAccountId);
        }

        /// <summary>
        /// Get UserDelegate Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<UserDelegate> GetUserDelegatePage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userAccountId)
        {
            var userDelegateDal = new UserDelegateDal();
            return userDelegateDal.GetUserDelegatePage(take, skip, sort, filter, userAccountId);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userDelegate">UserDelegate Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertUserDelegate(UserDelegate userDelegate, string userName)
        {
            var userDelegateDal = new UserDelegateDal();
            return userDelegateDal.InsertUserDelegate(userDelegate, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="userDelegate">UserDelegate Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateUserDelegate(UserDelegate userDelegate, string userName)
        {
            var userDelegateDal = new UserDelegateDal();
            return userDelegateDal.UpdateUserDelegate(userDelegate, userName);
        }
    }
}