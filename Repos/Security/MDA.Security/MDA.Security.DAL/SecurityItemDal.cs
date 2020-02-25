namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class SecurityItemDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteSecurityItem(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(new SecurityItem { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Get Available SecurityItem Page For Security Roles Id
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<SecurityItem> GetAvailableSecurityItemPageForSecurityRolesId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityRolesId, int companyInApplicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityItemSet.Include(x => x.SecurityTypeObj)
                    .Where(x => !(db.SecurityRoleRightsSet.Where(y => y.LnSecurityRolesId == securityRolesId).Any(y => y.LnSecurityItemId == x.Id)))
                    .Where(x => x.LnCompanyInApplicationId == companyInApplicationId).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Get Available SecurityItem Page For User In Company In Application Id
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<SecurityItem> GetAvailableSecurityItemPageForUserInCompanyInApplicationId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userInCompanyInApplicationId, int companyInApplicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityItemSet.Include(x => x.SecurityTypeObj)
                    .Where(x => !(db.SecurityUserRightsSet.Where(y => y.LnUserInCompanyInApplicationId == userInCompanyInApplicationId).Any(y => y.LnSecurityItemId == x.Id)))
                    .Where(x => x.LnCompanyInApplicationId == companyInApplicationId).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Get SecurityItem For Code And Company In Application Id
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Business Entity</returns>
        public SecurityItem GetSecurityItemForCodeAndCompanyInApplicationId(string code, int companyInApplicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityItemSet.Where(x => x.LnCompanyInApplicationId == companyInApplicationId).FirstOrDefault(x => x.Code == code);
            }
        }

        /// <summary>
        /// Get SecurityItem For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public SecurityItem GetSecurityItemForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityItemSet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get SecurityItem List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityItem> GetSecurityItemList(IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityItemSet.Include(x => x.SecurityTypeObj).Where(x => x.LnCompanyInApplicationId == companyInApplicationId)
                    .ToListResult(sort, filter);
            }
        }

        /// <summary>
        /// Get SecurityItem Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<SecurityItem> GetSecurityItemPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityItemSet.Include(x => x.SecurityTypeObj).Where(x => x.LnCompanyInApplicationId == companyInApplicationId)
                    .ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityItem">SecurityItem Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertSecurityItem(SecurityItem securityItem, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(securityItem).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityItem">SecurityItem Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateSecurityItem(SecurityItem securityItem, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(securityItem).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}