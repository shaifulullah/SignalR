namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class SecurityUserInRolesDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteSecurityUserInRoles(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(new SecurityUserInRoles { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Delete SecurityUserInRoles For Security Roles Id
        /// </summary>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteSecurityUserInRolesForSecurityRolesId(int securityRolesId, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                if (db.SecurityUserInRolesSet.Any(x => x.LnSecurityRolesId == securityRolesId))
                {
                    db.SecurityUserInRolesSet.RemoveRange(db.SecurityUserInRolesSet.Where(x => x.LnSecurityRolesId == securityRolesId));
                    return (db.SaveChanges(userName) > 0);
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Get SecurityUserInRoles For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public SecurityUserInRoles GetSecurityUserInRolesForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityUserInRolesSet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get SecurityUserInRoles For Security Role Code And Application Code
        /// </summary>
        /// <param name="securityRoleCode">Security Role Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyCode">Company Code</param>
        /// <returns>Business Entity</returns>
        public SecurityUserInRoles GetSecurityUserInRolesForSecurityRoleCodeAndApplicationCode(string securityRoleCode, string applicationCode, string companyCode)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityUserInRolesSet.Include(x => x.SecurityRolesObj).Include(x => x.UserInCompanyInApplicationObj)
                    .Include(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj)
                    .Include(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.ApplicationObj)
                    .Include(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.CompanyObj)
                    .Where(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.ApplicationObj.Code == applicationCode)
                    .Where(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.CompanyObj.Code == companyCode)
                    .FirstOrDefault(x => x.SecurityRolesObj.Code == securityRoleCode);
            }
        }

        /// <summary>
        /// Get SecurityUserInRoles List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserAccountDetails> GetSecurityUserInRolesList(IEnumerable<Sort> sort, LinqFilter filter, int securityRolesId, bool isHideTerminated)
        {
            var securityUserInRolesListForSecurityRolesId = GetSecurityUserInRolesListForSecurityRolesId(securityRolesId);
            using (var db = new ApplicationDBContext())
            {
                var userAccountDetailsList = securityUserInRolesListForSecurityRolesId
                    .Join(db.UserAccountDetailsSet.Where(y => !y.IsRecordDeleted), x => x.UserInCompanyInApplicationObj.LnUserAccountId, y => y.Id, (x, y) => new { secUsRl = x, uAcc = y })
                    .Select(x =>
                    {
                        return new UserAccountDetails
                        {
                            Id = x.secUsRl.Id,
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
        /// Get SecurityUserInRoles List For Application Code And Company Id
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyId">Company Id</param>
        /// <param name="userAccountIdList">User Account Id List</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityUserInRoles> GetSecurityUserInRolesListForApplicationCodeAndCompanyId(string applicationCode, int companyId, IEnumerable<int> userAccountIdList)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityUserInRolesSet.Include(x => x.SecurityRolesObj).Where(x => !x.SecurityRolesObj.IsDeleted)
                    .Include(x => x.UserInCompanyInApplicationObj).Include(x => x.UserInCompanyInApplicationObj.UserAccountObj)
                    .Include(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj).Include(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.ApplicationObj)
                    .Where(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.ApplicationObj.Code == applicationCode)
                    .Where(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.LnCompanyId == companyId)
                    .Where(x => userAccountIdList.Contains(x.UserInCompanyInApplicationObj.LnUserAccountId)).ToList();
            }
        }

        /// <summary>
        /// Get SecurityUserInRoles List For Application Id And Company Id
        /// </summary>
        /// <param name="applicationId">Application Id</param>
        /// <param name="companyId">Company Id</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityUserInRoles> GetSecurityUserInRolesListForApplicationIdAndCompanyId(int applicationId, int companyId, int userAccountId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityUserInRolesSet.Include(x => x.SecurityRolesObj).Where(x => !x.SecurityRolesObj.IsDeleted)
                    .Include(x => x.UserInCompanyInApplicationObj).Include(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj)
                    .Where(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.LnApplicationId == applicationId)
                    .Where(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.LnCompanyId == companyId)
                    .Where(x => x.UserInCompanyInApplicationObj.LnUserAccountId == userAccountId).ToList();
            }
        }

        /// <summary>
        /// Get SecurityUserInRoles List For Security Roles Id
        /// </summary>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityUserInRoles> GetSecurityUserInRolesListForSecurityRolesId(int securityRolesId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityUserInRolesSet.Include(x => x.UserInCompanyInApplicationObj).Where(x => x.LnSecurityRolesId == securityRolesId).ToList();
            }
        }

        /// <summary>
        /// Get SecurityUserInRoles List For User In Company In Application Id
        /// </summary>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityUserInRoles> GetSecurityUserInRolesListForUserInCompanyInApplicationId(int userInCompanyInApplicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityUserInRolesSet.Include(x => x.SecurityRolesObj).Where(x => !x.SecurityRolesObj.IsDeleted)
                    .Where(x => x.LnUserInCompanyInApplicationId == userInCompanyInApplicationId).ToList();
            }
        }

        /// <summary>
        /// Get SecurityUserInRoles List For User In Company In Application Id
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityRolesDetails> GetSecurityUserInRolesListForUserInCompanyInApplicationId(IEnumerable<Sort> sort, LinqFilter filter, int userInCompanyInApplicationId)
        {
            var securityUserInRolesListForUserInCompanyInApplicationId = GetSecurityUserInRolesListForUserInCompanyInApplicationId(userInCompanyInApplicationId);
            using (var db = new ApplicationDBContext())
            {
                var securityRolesDetailsList = securityUserInRolesListForUserInCompanyInApplicationId
                    .Join(db.SecurityRolesDetailsSet, x => x.LnSecurityRolesId, y => y.Id, (x, y) => new { secUsRl = x, srd = y })
                    .Select(x =>
                    {
                        return new SecurityRolesDetails
                        {
                            Id = x.secUsRl.Id,
                            Code = x.srd.Code,
                            Description = x.srd.Description,
                            LnActiveDirectoryGroupName = x.srd.LnActiveDirectoryGroupName ?? string.Empty,
                            LnSkillCode = x.srd.LnSkillCode ?? string.Empty
                        };
                    }).ToList();

                return securityRolesDetailsList.ToListResult(sort, filter);
            }
        }

        /// <summary>
        /// Get SecurityUserInRoles Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<UserAccountDetails> GetSecurityUserInRolesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityRolesId, bool isHideTerminated)
        {
            var securityUserInRolesListForSecurityRolesId = GetSecurityUserInRolesListForSecurityRolesId(securityRolesId);
            using (var db = new ApplicationDBContext())
            {
                var userAccountDetailsList = securityUserInRolesListForSecurityRolesId
                    .Join(db.UserAccountDetailsSet.Where(y => !y.IsRecordDeleted), x => x.UserInCompanyInApplicationObj.LnUserAccountId, y => y.Id, (x, y) => new { secUsRl = x, uAcc = y })
                    .Select(x =>
                    {
                        return new UserAccountDetails
                        {
                            Id = x.secUsRl.Id,
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
        /// Get SecurityUserInRoles Page For User In Company In Application Id
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<SecurityRolesDetails> GetSecurityUserInRolesPageForUserInCompanyInApplicationId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userInCompanyInApplicationId)
        {
            var securityUserInRolesListForUserInCompanyInApplicationId = GetSecurityUserInRolesListForUserInCompanyInApplicationId(userInCompanyInApplicationId);
            using (var db = new ApplicationDBContext())
            {
                var securityRolesDetailsList = securityUserInRolesListForUserInCompanyInApplicationId
                    .Join(db.SecurityRolesDetailsSet, x => x.LnSecurityRolesId, y => y.Id, (x, y) => new { secUsRl = x, srd = y })
                    .Select(x =>
                    {
                        return new SecurityRolesDetails
                        {
                            Id = x.secUsRl.Id,
                            Code = x.srd.Code,
                            Description = x.srd.Description,
                            LnActiveDirectoryGroupName = x.srd.LnActiveDirectoryGroupName ?? string.Empty,
                            LnSkillCode = x.srd.LnSkillCode ?? string.Empty
                        };
                    }).ToList();

                return securityRolesDetailsList.ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Get UserAccount Id List For Security Roles Id
        /// </summary>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<int> GetUserAccountIdListForSecurityRolesId(int securityRolesId, int userAccountId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityUserInRolesSet.Include(x => x.UserInCompanyInApplicationObj).Where(x => x.LnSecurityRolesId == securityRolesId)
                    .Where(x => !db.UserDelegateSet.Where(y => y.LnUserAccountId == userAccountId).Where(y => y.DateTimeEnd > DateTime.Today)
                    .Where(y => y.LnSecurityRolesId == securityRolesId).Any(y => y.LnDelegateUserAccountId == x.UserInCompanyInApplicationObj.LnUserAccountId))
                    .Where(x => !(x.UserInCompanyInApplicationObj.LnUserAccountId == userAccountId)).Select(x => x.UserInCompanyInApplicationObj.LnUserAccountId).ToList();
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityUserInRoles">SecurityUserInRoles Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertSecurityUserInRoles(SecurityUserInRoles securityUserInRoles, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(securityUserInRoles).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Is SecurityUserInRoles Exists For Security Role Code And User Account Id
        /// </summary>
        /// <param name="securityRoleCode">Security Role Code</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyCode">Company Code</param>
        /// <returns>True if SecurityUserInRoles Exists, else False</returns>
        public bool IsSecurityUserInRolesExistsForSecurityRoleCodeAndUserAccountId(string securityRoleCode, int userAccountId, string applicationCode, string companyCode)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityUserInRolesSet.Include(x => x.SecurityRolesObj).Include(x => x.UserInCompanyInApplicationObj)
                    .Include(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj)
                    .Include(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.ApplicationObj)
                    .Include(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.CompanyObj)
                    .Where(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.ApplicationObj.Code == applicationCode)
                    .Where(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.CompanyObj.Code == companyCode)
                    .Where(x => x.SecurityRolesObj.Code == securityRoleCode).Any(x => x.UserInCompanyInApplicationObj.LnUserAccountId == userAccountId);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityUserInRoles">SecurityUserInRoles Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateSecurityUserInRoles(SecurityUserInRoles securityUserInRoles, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(securityUserInRoles).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}