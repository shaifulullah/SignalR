namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class SecurityUserRightsDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteSecurityUserRights(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(new SecurityUserRights { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Get SecurityUserRights For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public SecurityUserRights GetSecurityUserRightsForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityUserRightsSet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get SecurityUserRights List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityRightsForUserAccount> GetSecurityUserRightsList(IEnumerable<Sort> sort, LinqFilter filter, int userInCompanyInApplicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityRightsForUserAccountSet.Where(x => x.LnUserInCompanyInApplicationId == userInCompanyInApplicationId).ToListResult(sort, filter);
            }
        }

        /// <summary>
        /// Get SecurityUserRights List For Application Id And Company Id
        /// </summary>
        /// <param name="applicationId">Application Id</param>
        /// <param name="companyId">Company Id</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityRightsForUserAccount> GetSecurityUserRightsListForApplicationIdAndCompanyId(int applicationId, int companyId, int userAccountId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityRightsForUserAccountSet.Where(x => x.ApplicationId == applicationId).Where(x => x.CompanyId == companyId)
                    .Where(x => x.UserAccountId == userAccountId).ToList();
            }
        }

        /// <summary>
        /// Get SecurityUserRights Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<SecurityRightsForUserAccount> GetSecurityUserRightsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userInCompanyInApplicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityRightsForUserAccountSet.Where(x => x.LnUserInCompanyInApplicationId == userInCompanyInApplicationId).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Get UserSecurityRights List For Application Code And User Account Id
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserSecurityRights> GetUserSecurityRightsListForApplicationCodeAndUserAccountId(string applicationCode, int userAccountId, int companyId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityUserRightsSet.Include(x => x.SecurityItemObj).Include(x => x.SecurityLevelObj).Include(x => x.UserInCompanyInApplicationObj)
                    .Include(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj).Where(x => x.UserInCompanyInApplicationObj.LnUserAccountId == userAccountId)
                    .Where(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.LnCompanyId == companyId)
                    .Where(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.ApplicationObj.Code == applicationCode)
                    .Select(x => new UserSecurityRights
                    {
                        Id = x.Id,
                        Code = x.SecurityItemObj.Code,
                        Description = x.SecurityItemObj.SecurityTypeObj.Description,
                        Rights = x.SecurityLevelObj.Code
                    }).ToList();
            }
        }

        /// <summary>
        /// Get UserSecurityRights List For Filter And Application Code
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserSecurityRights> GetUserSecurityRightsListForFilterAndApplicationCode(LinqFilter filter, string applicationCode, int userAccountId, int companyId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityUserRightsSet.Include(x => x.SecurityItemObj).Include(x => x.SecurityLevelObj).Include(x => x.UserInCompanyInApplicationObj)
                    .Include(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj).Where(x => x.UserInCompanyInApplicationObj.LnUserAccountId == userAccountId)
                    .Where(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.LnCompanyId == companyId)
                    .Where(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.ApplicationObj.Code == applicationCode)
                    .Select(x => new UserSecurityRights
                    {
                        Id = x.Id,
                        Code = x.SecurityItemObj.Code,
                        Description = x.SecurityItemObj.SecurityTypeObj.Description,
                        Rights = x.SecurityLevelObj.Code
                    }).ToListResult(null, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityUserRights">SecurityUserRights Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertSecurityUserRights(SecurityUserRights securityUserRights, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(securityUserRights).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityUserRights">SecurityUserRights Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateSecurityUserRights(SecurityUserRights securityUserRights, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(securityUserRights).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}