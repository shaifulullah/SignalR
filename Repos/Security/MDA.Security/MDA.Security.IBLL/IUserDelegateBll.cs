namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;

    public interface IUserDelegateBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteUserDelegate(int id, string userName);

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
        DataSourceResult<UserAccountDetails> GetAvailableUserDelegatePage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, IEnumerable<int> userAccountIdList, bool isHideTerminated);

        /// <summary>
        /// Get UserDelegate Count For User Account Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Count of User Delegate</returns>
        int GetUserDelegateCountForUserAccountId(int userAccountId);

        /// <summary>
        /// Get UserDelegate For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        UserDelegate GetUserDelegateForId(int id);

        /// <summary>
        /// Get UserDelegate List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<UserDelegate> GetUserDelegateList(IEnumerable<Sort> sort, LinqFilter filter, int userAccountId);

        /// <summary>
        /// Get UserDelegate Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<UserDelegate> GetUserDelegatePage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userAccountId);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userDelegate">UserDelegate Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertUserDelegate(UserDelegate userDelegate, string userName);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="userDelegate">UserDelegate Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool UpdateUserDelegate(UserDelegate userDelegate, string userName);
    }
}