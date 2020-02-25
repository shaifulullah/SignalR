namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class UserAccountDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteUserAccount(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                var userAccount = db.UserAccountSet.FirstOrDefault(x => x.Id == id);
                if (!userAccount.IsRecordDeleted)
                {
                    userAccount.IsRecordDeleted = true;

                    db.Entry(userAccount).State = EntityState.Modified;
                    return (db.SaveChanges(userName) > 0);
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Get Available UserAccount Page For User Account Id List
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountIdList">User Account Id List</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<UserAccountDetails> GetAvailableUserAccountPageForUserAccountIdList(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, IEnumerable<int> userAccountIdList, bool isHideTerminated)
        {
            using (var db = new ApplicationDBContext())
            {
                var userAccountDetailsList = db.UserAccountDetailsSet.Where(x => !userAccountIdList.Contains(x.Id)).Where(x => !x.IsRecordDeleted).ToList();
                if (userAccountDetailsList != null && isHideTerminated)
                {
                    userAccountDetailsList = userAccountDetailsList.Where(x => (x.LnEmployeeId.HasValue && x.LnEmployeeId != 0 && x.StatusValue != -10133) || (x.LnExternalPersonId.HasValue && x.LnExternalPersonId != 0 && x.StatusValue == 1)).ToList();
                }

                return userAccountDetailsList.ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Get UserAccountDetails Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<UserAccountDetails> GetUserAccountDetailsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, bool isHideTerminated)
        {
            using (var db = new ApplicationDBContext())
            {
                var userAccountDetailsList = db.UserAccountDetailsSet.Where(x => !x.IsRecordDeleted).ToList();
                if (userAccountDetailsList != null && isHideTerminated)
                {
                    userAccountDetailsList = userAccountDetailsList.Where(x => (x.LnEmployeeId.HasValue && x.LnEmployeeId != 0 && x.StatusValue != -10133) || (x.LnExternalPersonId.HasValue && x.LnExternalPersonId != 0 && x.StatusValue == 1)).ToList();
                }

                return userAccountDetailsList.ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Get UserAccount For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public UserAccount GetUserAccountForCode(string code)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserAccountSet.FirstOrDefault(x => x.UserName == code);
            }
        }

        /// <summary>
        /// Get UserAccount For Employee Id
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        /// <returns>Business Entity</returns>
        public UserAccountDetails GetUserAccountForEmployeeId(int employeeId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserAccountDetailsSet.FirstOrDefault(x => x.LnEmployeeId == employeeId);
            }
        }

        /// <summary>
        /// Get UserAccount For External Person Id
        /// </summary>
        /// <param name="externalPersonId">External Person Id</param>
        /// <returns>Business Entity</returns>
        public UserAccountDetails GetUserAccountForExternalPersonId(int externalPersonId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserAccountDetailsSet.FirstOrDefault(x => x.LnExternalPersonId == externalPersonId);
            }
        }

        /// <summary>
        /// Get UserAccount For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public UserAccountDetails GetUserAccountForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserAccountDetailsSet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get UserAccount For User Name And Domain
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="domain">Domain</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Business Entity</returns>
        public UserAccount GetUserAccountForUserNameAndDomain(string userName, string domain, string applicationCode)
        {
            using (var db = new ApplicationDBContext())
            {
                var userAccount = db.UserInCompanyInApplicationSet.Include(x => x.CompanyInApplicationObj).Where(x => x.UserAccountObj.UserName == userName)
                    .Where(x => x.UserAccountObj.Domain.ToUpper() == domain).Where(x => x.CompanyInApplicationObj.ApplicationObj.Code == applicationCode)
                    .Where(x => !x.UserAccountObj.IsRecordDeleted).Select(x => x.UserAccountObj).Include(x => x.CompanyObj).FirstOrDefault();

                return userAccount;
            }
        }

        /// <summary>
        /// Get UserAccount Id List For Is Hide Terminated
        /// </summary>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<int> GetUserAccountIdListForIsHideTerminated(bool isHideTerminated)
        {
            using (var db = new ApplicationDBContext())
            {
                return isHideTerminated ? db.UserAccountDetailsSet.Where(x => !x.IsRecordDeleted).Where(x => (x.LnEmployeeId.HasValue && x.LnEmployeeId != 0 && x.StatusValue != -10133) || (x.LnExternalPersonId.HasValue && x.LnExternalPersonId != 0 && x.StatusValue == 1)).Select(x => x.Id).ToList() : db.UserAccountDetailsSet.Where(x => !x.IsRecordDeleted).Select(x => x.Id).ToList();
            }
        }

        /// <summary>
        /// Get UserAccount List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserAccountDetails> GetUserAccountList(IEnumerable<Sort> sort, LinqFilter filter, bool isHideTerminated)
        {
            using (var db = new ApplicationDBContext())
            {
                var userAccountDetailsList = isHideTerminated ?
                    db.UserAccountDetailsSet.Where(x => (x.LnEmployeeId.HasValue && x.LnEmployeeId != 0 && x.StatusValue != -10133) || (x.LnExternalPersonId.HasValue && x.LnExternalPersonId != 0 && x.StatusValue == 1)) :
                    db.UserAccountDetailsSet;

                return userAccountDetailsList.ToListResult(sort, filter);
            }
        }

        /// <summary>
        /// Get UserAccount List For Employee Id List
        /// </summary>
        /// <param name="employeeIdList">Employee Id List</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserAccountDetails> GetUserAccountListForEmployeeIdList(IList<int> employeeIdList, bool isHideTerminated)
        {
            using (var db = new ApplicationDBContext())
            {
                return isHideTerminated ?
                    db.UserAccountDetailsSet.Where(x => !x.IsRecordDeleted).Where(x => x.LnEmployeeId.HasValue && x.LnEmployeeId != 0 && x.StatusValue != -10133).Where(x => employeeIdList.Contains(x.LnEmployeeId.Value)).ToList() :
                    db.UserAccountDetailsSet.Where(x => !x.IsRecordDeleted).Where(x => employeeIdList.Contains(x.LnEmployeeId.Value)).ToList();
            }
        }

        /// <summary>
        /// Get UserAccount Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<UserAccountDetails> GetUserAccountPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, bool isHideTerminated)
        {
            using (var db = new ApplicationDBContext())
            {
                var userAccountDetailsList = isHideTerminated ?
                    db.UserAccountDetailsSet.Where(x => (x.LnEmployeeId.HasValue && x.LnEmployeeId != 0 && x.StatusValue != -10133) || (x.LnExternalPersonId.HasValue && x.LnExternalPersonId != 0 && x.StatusValue == 1)) :
                    db.UserAccountDetailsSet;

                return userAccountDetailsList.ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userAccount">UserAccount Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertUserAccount(UserAccount userAccount, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(userAccount).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="userAccount">UserAccount Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateUserAccount(UserAccount userAccount, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(userAccount).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}