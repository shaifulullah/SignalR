namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class SecurityRolesDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteSecurityRoles(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                var securityRoles = db.SecurityRolesSet.FirstOrDefault(x => x.Id == id);
                if (!securityRoles.IsDeleted)
                {
                    securityRoles.IsDeleted = true;

                    db.Entry(securityRoles).State = EntityState.Modified;
                    return (db.SaveChanges(userName) > 0);
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Get Available SecurityRoles Page For User Account Id
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<SecurityRoles> GetAvailableSecurityRolesPageForUserAccountId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userAccountId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityRolesSet.Where(x => !x.IsDeleted).Include(x => x.CompanyInApplicationObj).Include(x => x.CompanyInApplicationObj.CompanyObj)
                    .Include(x => x.CompanyInApplicationObj.ApplicationObj)
                    .Where(x => db.SecurityUserInRolesSet.Where(y => y.UserInCompanyInApplicationObj.LnUserAccountId == userAccountId).Any(y => y.LnSecurityRolesId == x.Id)).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Get Available SecurityRoles Page For User In Company In Application Id
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<SecurityRoles> GetAvailableSecurityRolesPageForUserInCompanyInApplicationId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userInCompanyInApplicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityRolesSet.Where(x => !x.IsDeleted)
                    .Where(x => db.UserInCompanyInApplicationSet.Where(y => y.Id == userInCompanyInApplicationId).Any(y => y.LnCompanyInApplicationId == x.LnCompanyInApplicationId))
                    .Where(x => !db.SecurityUserInRolesSet.Where(y => y.LnUserInCompanyInApplicationId == userInCompanyInApplicationId).Any(y => y.LnSecurityRolesId == x.Id)).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Get SecurityRolesDetails List For Company Code And Application Code
        /// </summary>
        /// <param name="companyCode">Company Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityRolesDetails> GetSecurityRolesDetailsListForCompanyCodeAndApplicationCode(string companyCode, string applicationCode)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityRolesDetailsSet.Where(x => !x.IsDeleted).Where(x => x.CompanyCode == companyCode).Where(x => x.ApplicationCode == applicationCode).ToList();
            }
        }

        /// <summary>
        /// Get SecurityRolesDetailsWithUserName List For Company Code And Application Code
        /// </summary>
        /// <param name="companyCode">Company Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityRolesDetailsWithUserName> GetSecurityRolesDetailsWithUserNameListForCompanyCodeAndApplicationCode(string companyCode, string applicationCode)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityUserInRolesSet
                    .Include(x => x.UserInCompanyInApplicationObj).Include(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj)
                    .Include(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.CompanyObj).Include(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.ApplicationObj)
                    .Include(x => x.UserInCompanyInApplicationObj.UserAccountObj)
                    .Where(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.CompanyObj.Code == companyCode)
                    .Where(x => x.UserInCompanyInApplicationObj.CompanyInApplicationObj.ApplicationObj.Code == applicationCode)
                    .Join(db.SecurityRolesSet.Where(x => !x.IsDeleted), x => x.LnSecurityRolesId, y => y.Id, (x, y) => new { sUIRls = x, sRl = y })
                    .Select(x => new SecurityRolesDetailsWithUserName
                    {
                        Id = x.sRl.Id,
                        Code = x.sRl.Code,
                        Description = x.sRl.Description,
                        LnSkillCode = x.sRl.LnSkillCode,
                        LnCompanyInApplicationId = x.sRl.LnCompanyInApplicationId,
                        LnActiveDirectoryGroupName = x.sRl.LnActiveDirectoryGroupName,
                        UserName = x.sUIRls.UserInCompanyInApplicationObj.UserAccountObj.UserName,
                        UserAccountId = x.sUIRls.UserInCompanyInApplicationObj.LnUserAccountId
                    }).ToList();
            }
        }

        /// <summary>
        /// Get SecurityRoles For Code And Company In Application Id
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Business Entity</returns>
        public SecurityRoles GetSecurityRolesForCodeAndCompanyInApplicationId(string code, int companyInApplicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityRolesSet.Where(x => x.LnCompanyInApplicationId == companyInApplicationId).FirstOrDefault(x => x.Code == code);
            }
        }

        /// <summary>
        /// Get SecurityRoles For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public SecurityRoles GetSecurityRolesForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityRolesSet.Include(x => x.CompanyInApplicationObj).Include(x => x.CompanyInApplicationObj.CompanyObj)
                    .Include(x => x.CompanyInApplicationObj.ApplicationObj).FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get SecurityRoles List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityRoles> GetSecurityRolesList(IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityRolesSet.Where(x => x.LnCompanyInApplicationId == companyInApplicationId).ToListResult(sort, filter);
            }
        }

        /// <summary>
        /// Get SecurityRoles List For Application Code And Company Id
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityRoles> GetSecurityRolesListForApplicationCodeAndCompanyId(string applicationCode, int companyId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityRolesSet.Where(x => !x.IsDeleted).Include(x => x.CompanyInApplicationObj).Include(x => x.CompanyInApplicationObj.CompanyObj)
                    .Include(x => x.CompanyInApplicationObj.ApplicationObj).Where(x => x.CompanyInApplicationObj.ApplicationObj.Code == applicationCode)
                    .Where(x => x.CompanyInApplicationObj.LnCompanyId == companyId).ToList();
            }
        }

        /// <summary>
        /// Get SecurityRoles Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<SecurityRoles> GetSecurityRolesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityRolesSet.Where(x => x.LnCompanyInApplicationId == companyInApplicationId).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Get SecurityRoles Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<SecurityRoles> GetSecurityRolesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityRolesSet.Where(x => !x.IsDeleted).Include(x => x.CompanyInApplicationObj).Include(x => x.CompanyInApplicationObj.CompanyObj)
                    .Include(x => x.CompanyInApplicationObj.ApplicationObj).Where(x => x.Id != 0).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityRoles">SecurityRoles Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertSecurityRoles(SecurityRoles securityRoles, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(securityRoles).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityRoles">SecurityRoles Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateSecurityRoles(SecurityRoles securityRoles, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(securityRoles).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}