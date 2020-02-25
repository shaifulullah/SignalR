namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class UserInCompanyInApplicationDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteUserInCompanyInApplication(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(new UserInCompanyInApplication { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Get Available UserInCompanyInApplication For User Account Id List
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountIdList">User Account Id List</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<UserAccountDetails> GetAvailableUserInCompanyInApplicationForUserAccountIdList(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, IEnumerable<int> userAccountIdList, int companyInApplicationId, bool isHideTerminated)
        {
            var userInCompanyInApplicationListForCompanyInApplicationId = GetUserInCompanyInApplicationListForCompanyInApplicationId(companyInApplicationId);
            using (var db = new ApplicationDBContext())
            {
                var userAccountDetailsList = userInCompanyInApplicationListForCompanyInApplicationId
                    .Join(db.UserAccountDetailsSet.Where(x => !userAccountIdList.Contains(x.Id)).Where(x => !x.IsRecordDeleted),
                        x => x.LnUserAccountId, y => y.Id, (x, y) => new { uCmpApp = x, uAcc = y })
                        .Select(x =>
                        {
                            return new UserAccountDetails
                            {
                                Id = x.uCmpApp.Id,
                                UserName = x.uAcc.UserName,
                                Code = x.uAcc.Code,
                                FullName = x.uAcc.FullName,
                                eMail = x.uAcc.eMail ?? string.Empty,
                                StatusValue = x.uAcc.StatusValue,
                                LnEmployeeId = x.uAcc.LnEmployeeId,
                                LnExternalPersonId = x.uAcc.LnExternalPersonId,
                                Company = x.uAcc.Company
                            };
                        }).ToList();

                if (userAccountDetailsList != null && isHideTerminated)
                {
                    userAccountDetailsList = userAccountDetailsList.Where(x => (x.LnEmployeeId.HasValue && x.LnEmployeeId != 0 && x.StatusValue != -10133) || (x.LnExternalPersonId.HasValue && x.LnExternalPersonId != 0 && x.StatusValue == 1)).ToList();
                }

                return userAccountDetailsList.ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Get UserAccount Id List For Application Id And Company Id
        /// </summary>
        /// <param name="applicationId">Application Id</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<int> GetUserAccountIdListForApplicationIdAndCompanyId(int applicationId, int companyId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserInCompanyInApplicationSet.Include(x => x.CompanyInApplicationObj).Where(x => x.CompanyInApplicationObj.LnApplicationId == applicationId)
                    .Where(x => x.CompanyInApplicationObj.LnCompanyId == companyId).Select(x => x.LnUserAccountId).ToList();
            }
        }

        /// <summary>
        /// Get UserInCompanyInApplication For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public UserInCompanyInApplication GetUserInCompanyInApplicationForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserInCompanyInApplicationSet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get UserInCompanyInApplication List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserAccountDetails> GetUserInCompanyInApplicationList(IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId, bool isHideTerminated)
        {
            var userInCompanyInApplicationListForCompanyInApplicationId = GetUserInCompanyInApplicationListForCompanyInApplicationId(companyInApplicationId);
            using (var db = new ApplicationDBContext())
            {
                var userAccountDetailsList = userInCompanyInApplicationListForCompanyInApplicationId
                    .Join(db.UserAccountDetailsSet.Where(x => !x.IsRecordDeleted), x => x.LnUserAccountId, y => y.Id, (x, y) => new { uCmpApp = x, uAcc = y })
                    .Select(x =>
                    {
                        return new UserAccountDetails
                        {
                            Id = x.uCmpApp.Id,
                            UserName = x.uAcc.UserName,
                            Code = x.uAcc.Code,
                            FullName = x.uAcc.FullName,
                            eMail = x.uAcc.eMail ?? string.Empty,
                            StatusValue = x.uAcc.StatusValue,
                            LnEmployeeId = x.uAcc.LnEmployeeId,
                            LnExternalPersonId = x.uAcc.LnExternalPersonId,
                            Company = x.uAcc.Company
                        };
                    }).ToList();

                if (userAccountDetailsList != null && isHideTerminated)
                {
                    userAccountDetailsList = userAccountDetailsList.Where(x => (x.LnEmployeeId.HasValue && x.LnEmployeeId != 0 && x.StatusValue != -10133) || (x.LnExternalPersonId.HasValue && x.LnExternalPersonId != 0 && x.StatusValue == 1)).ToList();
                }

                return userAccountDetailsList.ToListResult(sort, filter);
            }
        }

        /// <summary>
        /// Get UserInCompanyInApplication List For Application Id And Company Id
        /// </summary>
        /// <param name="applicationId">Application Id</param>
        /// <param name="companyId">Company Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserAccountDetails> GetUserInCompanyInApplicationListForApplicationIdAndCompanyId(int applicationId, int companyId, bool isHideTerminated)
        {
            var userAccountIdListForApplicationIdAndCompanyId = GetUserAccountIdListForApplicationIdAndCompanyId(applicationId, companyId) ?? new List<int>();
            using (var db = new ApplicationDBContext())
            {
                var userAccountDetailsList = db.UserAccountDetailsSet.Where(x => userAccountIdListForApplicationIdAndCompanyId.Contains(x.Id))
                    .Where(x => !x.IsRecordDeleted).ToList();

                return userAccountDetailsList != null && isHideTerminated ?
                    userAccountDetailsList.Where(x => (x.LnEmployeeId.HasValue && x.LnEmployeeId != 0 && x.StatusValue != -10133) || (x.LnExternalPersonId.HasValue && x.LnExternalPersonId != 0 && x.StatusValue == 1)).ToList() :
                    userAccountDetailsList;
            }
        }

        /// <summary>
        /// Get UserInCompanyInApplication List For Company In Application Id
        /// </summary>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserInCompanyInApplication> GetUserInCompanyInApplicationListForCompanyInApplicationId(int companyInApplicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserInCompanyInApplicationSet.Where(x => x.LnCompanyInApplicationId == companyInApplicationId).ToList();
            }
        }

        /// <summary>
        /// Get UserInCompanyInApplication Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<UserAccountDetails> GetUserInCompanyInApplicationPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId, bool isHideTerminated)
        {
            var userInCompanyInApplicationListForCompanyInApplicationId = GetUserInCompanyInApplicationListForCompanyInApplicationId(companyInApplicationId);
            using (var db = new ApplicationDBContext())
            {
                var userAccountDetailsList = userInCompanyInApplicationListForCompanyInApplicationId
                    .Join(db.UserAccountDetailsSet.Where(x => !x.IsRecordDeleted), x => x.LnUserAccountId, y => y.Id, (x, y) => new { uCmpApp = x, uAcc = y })
                    .Select(x =>
                    {
                        return new UserAccountDetails
                        {
                            Id = x.uCmpApp.Id,
                            UserName = x.uAcc.UserName,
                            Code = x.uAcc.Code,
                            FullName = x.uAcc.FullName,
                            eMail = x.uAcc.eMail ?? string.Empty,
                            StatusValue = x.uAcc.StatusValue,
                            LnEmployeeId = x.uAcc.LnEmployeeId,
                            LnExternalPersonId = x.uAcc.LnExternalPersonId,
                            Company = x.uAcc.Company
                        };
                    }).ToList();

                if (userAccountDetailsList != null && isHideTerminated)
                {
                    userAccountDetailsList = userAccountDetailsList.Where(x => (x.LnEmployeeId.HasValue && x.LnEmployeeId != 0 && x.StatusValue != -10133) || (x.LnExternalPersonId.HasValue && x.LnExternalPersonId != 0 && x.StatusValue == 1)).ToList();
                }

                return userAccountDetailsList.ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userInCompanyInApplication">UserInCompanyInApplication Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertUserInCompanyInApplication(UserInCompanyInApplication userInCompanyInApplication, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(userInCompanyInApplication).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}