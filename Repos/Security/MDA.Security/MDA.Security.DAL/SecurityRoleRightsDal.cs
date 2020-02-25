namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class SecurityRoleRightsDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteSecurityRoleRights(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(new SecurityRoleRights { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Get SecurityRoleRights For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public SecurityRoleRights GetSecurityRoleRightsForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityRoleRightsSet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get SecurityRoleRights List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityRightsForSecurityRole> GetSecurityRoleRightsList(IEnumerable<Sort> sort, LinqFilter filter, int securityRolesId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityRightsForSecurityRoleSet.Where(x => x.LnSecurityRolesId == securityRolesId).ToListResult(sort, filter);
            }
        }

        /// <summary>
        /// Get SecurityRoleRights Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<SecurityRightsForSecurityRole> GetSecurityRoleRightsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityRolesId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityRightsForSecurityRoleSet.Where(x => x.LnSecurityRolesId == securityRolesId).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Get UserSecurityRights List For Application Code And Active Directory Group Name
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="activeDirectoryGroupName">Active Directory Group Name</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserSecurityRights> GetUserSecurityRightsListForApplicationCodeAndActiveDirectoryGroupName(string applicationCode, string activeDirectoryGroupName, int companyId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityRoleRightsSet.Include(x => x.SecurityRolesObj).Where(x => !x.SecurityRolesObj.IsDeleted)
                    .Include(x => x.SecurityItemObj).Include(x => x.SecurityLevelObj)
                    .Join(db.SecurityRolesSet.Where(x => !x.IsDeleted), x => x.LnSecurityRolesId, y => y.Id, (x, y) => new { sRlRgt = x, sRl = y })
                    .Where(x => x.sRl.CompanyInApplicationObj.LnCompanyId == companyId).Where(x => x.sRl.LnActiveDirectoryGroupName == activeDirectoryGroupName)
                    .Where(x => x.sRl.CompanyInApplicationObj.ApplicationObj.Code == applicationCode)
                    .Select(x => new UserSecurityRights
                    {
                        Id = x.sRlRgt.Id,
                        Code = x.sRlRgt.SecurityItemObj.Code,
                        Description = x.sRlRgt.SecurityItemObj.SecurityTypeObj.Description,
                        Rights = x.sRlRgt.SecurityLevelObj.Code,
                        Role = x.sRlRgt.SecurityRolesObj.Code,
                        LnSecurityLevelId = x.sRlRgt.LnSecurityLevelId,
                        ActiveDirectoryGroupName = x.sRl.LnActiveDirectoryGroupName
                    }).GroupBy(z => z.Code).Select(a => a.OrderByDescending(b => b.LnSecurityLevelId).FirstOrDefault()).ToList();
            }
        }

        /// <summary>
        /// Get UserSecurityRights List For Application Code And Skill Code
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="skillCode">Skill Code</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserSecurityRights> GetUserSecurityRightsListForApplicationCodeAndSkillCode(string applicationCode, string skillCode, int companyId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityRoleRightsSet.Include(x => x.SecurityRolesObj).Where(x => !x.SecurityRolesObj.IsDeleted)
                    .Include(x => x.SecurityItemObj).Include(x => x.SecurityLevelObj)
                    .Join(db.SecurityRolesSet.Where(x => !x.IsDeleted), x => x.LnSecurityRolesId, y => y.Id, (x, y) => new { sRlRgt = x, sRl = y })
                    .Where(x => x.sRl.CompanyInApplicationObj.LnCompanyId == companyId).Where(x => x.sRl.LnSkillCode == skillCode)
                    .Where(x => x.sRl.CompanyInApplicationObj.ApplicationObj.Code == applicationCode)
                    .Select(x => new UserSecurityRights
                    {
                        Id = x.sRlRgt.Id,
                        Code = x.sRlRgt.SecurityItemObj.Code,
                        Description = x.sRlRgt.SecurityItemObj.SecurityTypeObj.Description,
                        Rights = x.sRlRgt.SecurityLevelObj.Code,
                        Role = x.sRlRgt.SecurityRolesObj.Code,
                        LnSecurityLevelId = x.sRlRgt.LnSecurityLevelId,
                        SkillCode = x.sRl.LnSkillCode
                    }).GroupBy(z => z.Code).Select(a => a.OrderByDescending(b => b.LnSecurityLevelId).FirstOrDefault()).ToList();
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
                return db.SecurityRoleRightsSet.Include(x => x.SecurityRolesObj).Where(x => !x.SecurityRolesObj.IsDeleted)
                    .Include(x => x.SecurityItemObj).Include(x => x.SecurityLevelObj)
                    .Join(db.SecurityUserInRolesSet, x => x.LnSecurityRolesId, y => y.LnSecurityRolesId, (x, y) => new { sRlRgt = x, sUsRl = y })
                    .Where(x => x.sUsRl.UserInCompanyInApplicationObj.LnUserAccountId == userAccountId)
                    .Where(x => x.sUsRl.UserInCompanyInApplicationObj.CompanyInApplicationObj.LnCompanyId == companyId)
                    .Where(x => x.sUsRl.UserInCompanyInApplicationObj.CompanyInApplicationObj.ApplicationObj.Code == applicationCode)
                    .Select(x => new UserSecurityRights
                    {
                        Id = x.sRlRgt.Id,
                        Code = x.sRlRgt.SecurityItemObj.Code,
                        Description = x.sRlRgt.SecurityItemObj.SecurityTypeObj.Description,
                        Rights = x.sRlRgt.SecurityLevelObj.Code,
                        Role = x.sRlRgt.SecurityRolesObj.Code,
                        LnSecurityLevelId = x.sRlRgt.LnSecurityLevelId
                    }).GroupBy(z => z.Code).Select(a => a.OrderByDescending(b => b.LnSecurityLevelId).FirstOrDefault()).ToList();
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
                return db.SecurityRoleRightsSet.Include(x => x.SecurityRolesObj).Where(x => !x.SecurityRolesObj.IsDeleted)
                    .Include(x => x.SecurityItemObj).Include(x => x.SecurityLevelObj)
                    .Join(db.SecurityUserInRolesSet, x => x.LnSecurityRolesId, y => y.LnSecurityRolesId, (x, y) => new { sRlRgt = x, sUsRl = y })
                    .Where(x => x.sUsRl.UserInCompanyInApplicationObj.LnUserAccountId == userAccountId)
                    .Where(x => x.sUsRl.UserInCompanyInApplicationObj.CompanyInApplicationObj.LnCompanyId == companyId)
                    .Where(x => x.sUsRl.UserInCompanyInApplicationObj.CompanyInApplicationObj.ApplicationObj.Code == applicationCode)
                    .Select(x => new UserSecurityRights
                    {
                        Id = x.sRlRgt.Id,
                        Code = x.sRlRgt.SecurityItemObj.Code,
                        Description = x.sRlRgt.SecurityItemObj.SecurityTypeObj.Description,
                        Rights = x.sRlRgt.SecurityLevelObj.Code,
                        Role = x.sRlRgt.SecurityRolesObj.Code,
                        LnSecurityLevelId = x.sRlRgt.LnSecurityLevelId
                    }).ToListResult(null, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityRoleRights">SecurityRoleRights Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertSecurityRoleRights(SecurityRoleRights securityRoleRights, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(securityRoleRights).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityRoleRights">SecurityRoleRights Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateSecurityRoleRights(SecurityRoleRights securityRoleRights, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(securityRoleRights).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}