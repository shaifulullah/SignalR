namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class UserDelegateDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteUserDelegate(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(new UserDelegate { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
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
            using (var db = new ApplicationDBContext())
            {
                var userAccountDetailsList = db.UserAccountDetailsSet.Where(x => userAccountIdList.Contains(x.Id)).Where(x => !x.IsRecordDeleted).ToList();
                if (userAccountDetailsList != null)
                {
                    userAccountDetailsList = isHideTerminated ?
                        userAccountDetailsList.Where(x => (x.LnEmployeeId.HasValue && x.StatusValue != -10133) || (x.LnExternalPersonId.HasValue && x.StatusValue == 1)).ToList() :
                        userAccountDetailsList;
                }

                return userAccountDetailsList.ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Get UserDelegate Count For User Account Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Count of User Delegate</returns>
        public int GetUserDelegateCountForUserAccountId(int userAccountId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserDelegateSet.Where(x => x.LnUserAccountId == userAccountId).Where(x => x.DateTimeEnd >= DateTime.Today).Where(x => x.FlagIsApprover).Count();
            }
        }

        /// <summary>
        /// Get UserDelegate For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public UserDelegate GetUserDelegateForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserDelegateSet.Include(x => x.DelegateUserAccountObj).Include(x => x.SecurityRolesObj).FirstOrDefault(x => x.Id == id);
            }
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
            using (var db = new ApplicationDBContext())
            {
                return db.UserDelegateSet.Include(x => x.SecurityRolesObj).Where(x => !x.SecurityRolesObj.IsDeleted).Include(x => x.DelegateUserAccountObj)
                    .Include(x => x.SecurityRolesObj.CompanyInApplicationObj).Include(x => x.SecurityRolesObj.CompanyInApplicationObj.ApplicationObj)
                    .Include(x => x.SecurityRolesObj.CompanyInApplicationObj.CompanyObj)
                    .Where(x => x.LnUserAccountId == userAccountId).Where(x => !x.DelegateUserAccountObj.IsRecordDeleted).ToListResult(sort, filter);
            }
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
            using (var db = new ApplicationDBContext())
            {
                return db.UserDelegateSet.Include(x => x.SecurityRolesObj).Where(x => !x.SecurityRolesObj.IsDeleted).Include(x => x.DelegateUserAccountObj).Include(x => x.SecurityRolesObj.CompanyInApplicationObj)
                    .Include(x => x.SecurityRolesObj.CompanyInApplicationObj.ApplicationObj).Include(x => x.SecurityRolesObj.CompanyInApplicationObj.CompanyObj)
                    .Where(x => x.LnUserAccountId == userAccountId).Where(x => !x.DelegateUserAccountObj.IsRecordDeleted).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userDelegate">UserDelegate Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertUserDelegate(UserDelegate userDelegate, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(userDelegate).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="userDelegate">UserDelegate Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateUserDelegate(UserDelegate userDelegate, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(userDelegate).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}